using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfAppCommonCode.Helpers
{
    public static class ImageConverter
    {
        public static BitmapImage ConvertBytesToBitmapImage(this byte[] data, out bool success)
        {
            BitmapImage bmpImageResult = null;
            try
            {
                using (var ms = new System.IO.MemoryStream(data))
                {
                    bmpImageResult = new BitmapImage();
                    bmpImageResult.BeginInit();
                    bmpImageResult.CacheOption = BitmapCacheOption.OnLoad; // here
                    bmpImageResult.StreamSource = ms;
                    bmpImageResult.EndInit();
                }
                success = true;
            }
            catch (Exception ex)
            {
                bmpImageResult = new BitmapImage();
                success = false;
            }

            return bmpImageResult;
        }


        public static bool SaveBitmapToPath(this BitmapImage bmp, string path)
        {
            bool success;
            try
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));

                using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }


        public static bool SaveDataToPath(this byte[] data, string path)
        {
            bool success;
            try
            {
                var fileStream = File.Open(path, FileMode.OpenOrCreate);
                var binaryWriter = new BinaryWriter(fileStream);
                binaryWriter.Write(data);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }
    }
}

