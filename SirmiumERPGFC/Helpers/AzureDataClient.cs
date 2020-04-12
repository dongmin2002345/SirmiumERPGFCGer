using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.File;
using Newtonsoft.Json;
using ServiceInterfaces.Abstractions.Common.DocumentStores;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using SirmiumERPGFC.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Helpers
{
    public delegate void IndexingDirectoryHandler(string dirPath, int directoryNumber);
    public class AzureDataClient
    {
        CloudStorageAccount cloudStorageAccount;

        CloudFileClient fileClient;

        CloudFileShare fileShare;

        public CloudFileDirectory rootDirectory;

        public event IndexingDirectoryHandler IndexingDirectoryChanged;

        public bool CancelOperation = false;

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

            } catch(Exception ex)
            {
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


        public void GetDirectoryStructure(DirectoryTreeItemViewModel dir)
        {
            if (CancelOperation)
                return;

            if (dir != null)
            {
                if (dir.Directory != null)
                {
                    var subItems = dir.Directory.ListFilesAndDirectories().OfType<CloudFileDirectory>()
                        .Select(x => new DirectoryTreeItemViewModel() { Directory = x, Name = x.Name, IsDirectory = true, FullPath = x.Uri?.LocalPath, ParentNode = dir })
                        .ToList();

                    foreach (var item in subItems)
                        GetDirectoryStructure(item);
                }
            }
        }


        public void GetDocumentAttributes(DocumentFileViewModel doc)
        {
            CloudFile file = GetFile(doc.Path);
            if(file != null && file.Exists())
            {
                Debug.WriteLine($"Fetching attributes for {doc.Path}");
                file.FetchAttributes();

                if(file.Properties != null)
                {
                    doc.Size = file.Properties.Length / 1024;
                    doc.CreatedAt = file.Properties.LastModified.Value.DateTime;
                }
            }
        }


        int currentlyIndexedNumber = 0;

        public void ResetIndexNumber() => currentlyIndexedNumber = 0;

        public void GetDocumentFolders(IDocumentFolderService service, IDocumentFileService fileService, DocumentFolderViewModel dir, bool recursive = false)
        {
            if (CancelOperation)
                return;

            if(dir != null)
            {
                CloudFileDirectory dirPath = GetDirectory(dir.Path);
                if (dirPath != null && dirPath.Exists())
                {
                    currentlyIndexedNumber++;
                    IndexingDirectoryChanged?.Invoke(dir.Path, currentlyIndexedNumber);

                    if (dir.Id < 1)
                    {
                        var response = service.Create(dir);
                        if (response.Success)
                        {
                            dir.Id = response?.DocumentFolder?.Id ?? 0;
                        }
                        else
                            dir.Id = -1;
                    }
                    if(dir.Id > 0)
                    {
                        dirPath.FetchAttributes();
                        var subFilesAndDirectories = dirPath.ListFilesAndDirectories();

                        var subFiles = subFilesAndDirectories.OfType<CloudFile>()
                            .Select(x => new DocumentFileViewModel()
                            {
                                Identifier = Guid.NewGuid(),
                                DocumentFolder = dir,
                                Name = x.Name,
                                Path = x.Uri.LocalPath,
                                
                                Company = new ServiceInterfaces.ViewModels.Common.Companies.CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                                CreatedBy = new ServiceInterfaces.ViewModels.Common.Identity.UserViewModel() { Id = MainWindow.CurrentUserId }
                            })
                            .ToList();
                        subFiles.ForEach(x => GetDocumentAttributes(x));

                        var fileResponse = fileService.SubmitList(subFiles);



                        var subDirectories = subFilesAndDirectories.OfType<CloudFileDirectory>()
                            .Select(x => new DocumentFolderViewModel() 
                            { 
                                Path = x.Uri.LocalPath, Name = x.Name, Identifier = Guid.NewGuid(), ParentFolder = dir,
                                Company = new ServiceInterfaces.ViewModels.Common.Companies.CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                                CreatedBy = new ServiceInterfaces.ViewModels.Common.Identity.UserViewModel() { Id = MainWindow.CurrentUserId }
                            })
                            .ToList();

                        var response = service.SubmitList(subDirectories);
                        if(response.Success)
                        {
                            dir.SubDirectories = new ObservableCollection<DocumentFolderViewModel>(response?.DocumentFolders ?? new List<DocumentFolderViewModel>());

                            if (recursive)
                            {
                                foreach (var item in dir.SubDirectories)
                                {
                                    if (CancelOperation)
                                        return;

                                    GetDocumentFolders(service, fileService, item, recursive);
                                }
                            }
                        }
                    }
                }
            }
        }




        public DirectoryTreeItemViewModel GetDirectoryIndex()
        {
            var indexFile = GetDirectoryIndexFile();

            if(indexFile != null && indexFile.Exists())
            {
                string indexContent = indexFile.DownloadText();

                var index = JsonConvert.DeserializeObject<DirectoryTreeItemViewModel>(indexContent);

                return index;
            }
            return null;
        }

        CloudFile GetDirectoryIndexFile()
        {
            if(rootDirectory != null)
            {
                if(!rootDirectory.Exists())
                    return null;

                var indexDirectory = rootDirectory.GetDirectoryReference(".indexes");
                indexDirectory.CreateIfNotExists();

                var indexFile = indexDirectory.GetFileReference("directories.index");
                return indexFile;
            }
            return null;
        }



        public DirectoryTreeItemViewModel RecalculateIndex()
        {
            var indexFile = GetDirectoryIndexFile();
            if(indexFile != null)
            {
                DirectoryTreeItemViewModel rootDir = new DirectoryTreeItemViewModel();
                rootDir.IsDirectory = true;
                rootDir.Directory = rootDirectory;
                rootDir.FullPath = rootDirectory.Uri.LocalPath;
                rootDir.Name = "Documents";
                rootDir.ParentNode = null;
                rootDir.Items = new ObservableCollection<DirectoryTreeItemViewModel>();
                DirectoryTreeItemViewModel calculatedIndexes = CalculateDirectoryIndex(rootDir);

                string serialized = JsonConvert.SerializeObject(calculatedIndexes, new JsonSerializerSettings() { 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.None
                });

                indexFile.UploadText(serialized);

                return calculatedIndexes;
            }
            return null;
        }

        public DirectoryTreeItemViewModel CalculateDirectoryIndex(DirectoryTreeItemViewModel dir)
        {
            IndexingDirectoryChanged?.Invoke(dir.FullPath, 0);
            if(dir != null)
            {
                if(dir.Directory != null)
                {
                    //var files = dir.Directory.ListFilesAndDirectories().OfType<CloudFile>();
                    var directories = dir.Directory.ListFilesAndDirectories().OfType<CloudFileDirectory>();
                    if (dir.Items == null)
                        dir.Items = new ObservableCollection<DirectoryTreeItemViewModel>();

                    //if(files != null && files.Count() > 0)
                    //{
                    //    foreach (var item in files)
                    //    {
                    //        item.FetchAttributes();
                    //        DirectoryTreeItemViewModel file = new DirectoryTreeItemViewModel();
                    //        file.Name = item.Name;
                    //        file.IsDirectory = false;
                    //        file.ParentNode = dir;
                    //        file.FullPath = item.Uri.LocalPath;
                    //        if (item.Properties != null)
                    //        {
                    //            file.CreatedAt = item.Properties.LastModified?.DateTime ?? DateTime.MinValue;
                    //            file.FileSize = item.Properties.Length / 1024;
                    //        }
                    //        dir.Items.Add(file);
                    //    }
                    //}

                    if(directories != null && directories.Count() > 0)
                    {
                        foreach(var item in directories)
                        {
                            DirectoryTreeItemViewModel directory = new DirectoryTreeItemViewModel();
                            directory.Name = item.Name;
                            directory.IsDirectory = true;
                            directory.ParentNode = dir;
                            directory.FullPath = item.Uri.LocalPath;

                            directory.Directory = item;

                            dir.Items.Add(directory);
                            directory = CalculateDirectoryIndex(directory);
                        }
                    }
                }
            }
            return dir;
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

        public CloudFileDirectory GetDirectory(string filePath)
        {
            try
            {
                var configRootPath = AppConfigurationHelper.Configuration?.AzureNetworkDrive?.SubDir?.Replace("\\", "/") ?? "";
                filePath = filePath.Replace(configRootPath, "");
                if (!String.IsNullOrEmpty(filePath) && filePath.Length > 1)
                    filePath = filePath.Substring(1);

                if (String.IsNullOrEmpty(filePath))
                    return rootDirectory;
                else
                    return rootDirectory.GetDirectoryReference(filePath);
            }
            catch (Exception ex)
            {
                return null;
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

    public static class AzureExtensionMethods
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> act)
        {
            foreach (var item in list)
                act(item);
        }
    }
}
