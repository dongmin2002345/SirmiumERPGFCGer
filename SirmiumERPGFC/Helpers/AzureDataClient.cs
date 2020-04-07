﻿using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Helpers
{
    public class AzureDataClient
    {
        CloudStorageAccount cloudStorageAccount;

        CloudFileClient fileClient;

        CloudFileShare fileShare;

        public CloudFileDirectory rootDirectory;


        public bool CancelOperation = false;

        bool IsAzureClientValid = false;

        public AzureDataClient()
        {
            string connectionStr = "DefaultEndpointsProtocol=https" +
                ";AccountName=" + (AppConfigurationHelper.Configuration?.AzureNetworkDrive?.Username?.Replace("Azure\\", "") ?? "") +
                ";AccountKey=" + (AppConfigurationHelper.Configuration?.AzureNetworkDrive?.Password ?? "");

            try
            {
                cloudStorageAccount = CloudStorageAccount.Parse(connectionStr);

                fileClient = cloudStorageAccount.CreateCloudFileClient();

                fileShare = fileClient.GetShareReference(AppConfigurationHelper.Configuration?.AzureNetworkDrive?.SubDir?.Replace("\\", "") ?? "");

                rootDirectory = fileShare.GetRootDirectoryReference();

                IsAzureClientValid = true;
            } catch(Exception ex)
            {
                IsAzureClientValid = false;
            }
        }


        public List<CloudFileDirectory> GetSubDirectories(CloudFileDirectory parent)
        {
            try
            {
                CancelOperation = false;
                List<CloudFileDirectory> directories = parent?.ListFilesAndDirectories().OfType<CloudFileDirectory>().ToList() ?? new List<CloudFileDirectory>();

                return directories;
            } catch(Exception ex)
            {
                return new List<CloudFileDirectory>();
            }
        }

        public CloudFileDirectory CreateDirectory(CloudFileDirectory parent, string name)
        {
            try
            {
                CancelOperation = false;
                CloudFileDirectory cd = parent?.GetDirectoryReference(name);
                if (cd.Exists())
                    throw new Exception("Folder already exists!");

                cd.Create();

                return cd;
            } catch(Exception ex)
            {
                return null;
            }
        }

        public List<CloudFile> GetFiles(CloudFileDirectory parent)
        {
            try
            {
                CancelOperation = false;
                List<CloudFile> files = parent?.ListFilesAndDirectories().OfType<CloudFile>().ToList() ?? new List<CloudFile>();

                return files;
            } catch(Exception ex)
            {
                return new List<CloudFile>();
            }
        }

        public void FindFiles(CloudFileDirectory parent, string filter, bool recursive = false, Action<List<CloudFile>> foundFilesCallback = null, Action<string> currentPath = null)
        {
            try
            {
                if (CancelOperation)
                    return;

                currentPath?.Invoke("Searching: " + parent?.Uri?.LocalPath);

                List<CloudFile> files = parent?.ListFilesAndDirectories().OfType<CloudFile>().Where(x => x.Name.ToLower().Contains(filter)).ToList() ?? new List<CloudFile>();
                foundFilesCallback?.Invoke(files);
                if (recursive)
                {
                    List<CloudFileDirectory> dirs = parent?.ListFilesAndDirectories().OfType<CloudFileDirectory>().ToList() ?? new List<CloudFileDirectory>();
                    if (dirs != null)
                    {
                        foreach (CloudFileDirectory directory in dirs)
                        {
                            if (CancelOperation)
                                return;

                            FindFiles(directory, filter, recursive, foundFilesCallback, currentPath);
                        }
                    }
                }
            } catch(Exception ex)
            {

            }
        }

        public CloudFile CopyLocal(CloudFileDirectory dir, string path, Action<long, long> progressChanged)
        {
            try
            {

                var fileName = Path.GetFileName(path);
                CloudFile file = dir?.GetFileReference(fileName);
                if (file != null && file.Exists())
                    throw new Exception("File already exists on server!");

                using (FileStream fs = new ObservableFileStream(path, FileMode.Open, progressChanged))
                {
                    file?.UploadFromStream(fs);
                }

                return file;
            } catch(Exception ex)
            {
                return null;
            }
        }


        public CloudFile CopyRemote(CloudFile toCopy, string newName)
        {
            try
            {
                var dir = toCopy?.Parent;

                CloudFile newPath = dir?.GetFileReference(newName);
                if (newPath != null && newPath.Exists())
                    throw new Exception("File already exists on server!");


                newPath.StartCopy(toCopy);
                return newPath;
            } catch(Exception ex)
            {
                return null;
            }
        }

        public void Delete(CloudFile file)
        {
            try
            {
                file?.DeleteIfExists();
            } catch(Exception ex)
            {
            }
        }

        public CloudFile GetFile(string filePath)
        {
            try
            {
                var configRootPath = AppConfigurationHelper.Configuration?.AzureNetworkDrive?.SubDir?.Replace("\\", "/") ?? "";
                filePath = filePath.Replace(configRootPath, "")?.Substring(1);

                return rootDirectory.GetFileReference(filePath);
            } catch(Exception ex)
            {
                return null;
            }
        }

        public string DownloadFileToOpen(CloudFile toOpen, Action<long, long> callback)
        {
            try
            {
                var fileName = Path.GetFileName(toOpen?.Uri?.LocalPath?.Replace("/", "\\") ?? "");
                var tmpPath = Path.GetTempPath();

                string fullTmpPath = Path.Combine(tmpPath, fileName)?.Replace("\\", "/");

                if (File.Exists(fullTmpPath))
                    File.Delete(fullTmpPath);

                using (var fs = new ObservableFileStream(fullTmpPath, FileMode.OpenOrCreate, callback))
                {
                    toOpen?.DownloadToStream(fs);
                }

                return fullTmpPath;
            } catch(Exception ex)
            {
                return "";
            }
        }
    }
    public class ObservableFileStream : FileStream
    {
        private Action<long, long> _callback;

        public ObservableFileStream(String fileName, FileMode mode, Action<long, long> callback) : base(fileName, mode)
        {
            _callback = callback;
        }

        public override void Write(byte[] array, int offset, int count)
        {
            _callback?.Invoke(Position, Length);
            base.Write(array, offset, count);
        }

        public override int Read(byte[] array, int offset, int count)
        {
            _callback?.Invoke(Position, Length);
            return base.Read(array, offset, count);
        }
    }
}