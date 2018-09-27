using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.CsvDataOperations
{
    public class FileOperations
    {
        public static void OpenFileAndWriteLines(string fileName, string directoryName, List<String> lines)
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory + "CsvFiles\\";

            string directoryFull = basePath + directoryName + "\\";
            Directory.CreateDirectory(directoryFull);

            using (FileStream fs = File.Open(directoryFull + fileName + ".json", FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (String line in lines)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
        }

        public static void OpenFileAndWriteLine(string fileName, string directoryName, String line)
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory + "CsvFiles\\";

            string directoryFull = basePath + directoryName + "\\";
            Directory.CreateDirectory(directoryFull);

            using (FileStream fs = File.Open(directoryFull + fileName + ".json", FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine(line);
                }
            }
        }
    }
}
