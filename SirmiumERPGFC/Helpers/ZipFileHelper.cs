using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Helpers
{
    public static class ZipFileHelper
    {
        public static string MakeArchiveFromFiles(List<string> paths, string customPath = null)
        {
            try
            {
                string tempPath = customPath;
                if (String.IsNullOrEmpty(tempPath))
                    tempPath = Path.GetTempPath();
                //string tempPath = Path.GetTempPath();
                //string tempPath = "C:\\Users\\nedeljko\\Documents\\zipovi\\";

                string tempFile = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + "_Documents.zip";

                string tempFilePath = Path.Combine(tempPath, tempFile);


                using (ZipFile f = ZipFile.Create(tempFilePath))
                {
                    f.BeginUpdate();

                    foreach (var item in paths)
                    {
                        string fileName = Path.GetFileName(item);
                        f.Add(item, fileName);
                    }
                    f.CommitUpdate();

                    f.Close();
                }

                return tempFilePath;
            } catch(Exception ex)
            {
                return null;
            }
        }
    }
}
