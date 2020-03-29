using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Helpers
{
    public delegate void ProgressChangeDelegate(double Persentage, ref bool Cancel);
    public delegate void Completedelegate();

    public class CopyFileHelper
    {

        public event ProgressChangeDelegate OnProgressChanged;
        public event Completedelegate OnComplete;
        string oldPath;
        string newPath;

        public CopyFileHelper(string oldFile, string newFile) {
            oldPath = oldFile;
            newPath = newFile;
        }

        public void Copy()
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
            bool cancel = false;
            using (FileStream oldFile = new FileStream(oldPath, FileMode.Open, FileAccess.Read))
            {
                long fileLength = oldFile.Length;
                using (FileStream newFile = new FileStream(newPath, FileMode.CreateNew, FileAccess.Write))
                {
                    long totalBytes = 0;
                    int currentBlockSize = 0;

                    while ((currentBlockSize = oldFile.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalBytes += currentBlockSize;
                        double persentage = (double)totalBytes * 100.0 / fileLength;

                        newFile.Write(buffer, 0, currentBlockSize);
                        cancel = false;

                        OnProgressChanged?.Invoke(persentage, ref cancel);

                        if (cancel)
                        {
                            break;
                        }
                    }

                    OnComplete?.Invoke();
                }
            }
        }
    }
}
