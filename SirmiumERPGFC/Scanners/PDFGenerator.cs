using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Scanners
{
    public class PDFGenerator
    {
        List<string> paths = new List<string>();
        string Author { get; set; }
        string Title { get; set; }
        string FilePath { get; set; }
        public PDFGenerator(List<string> imagePaths, string docPath, string title, string author)
        {
            paths = imagePaths;
            FilePath = docPath;
            Title = title;
            Author = author;
        }

        public string Generate()
        {
            string filename = $"{FilePath}{Title}.pdf";
            var s_document = new PdfDocument();
            s_document.Info.Title = Title;
            s_document.Info.Author = Author;
            s_document.Info.Subject = "Auto-Generated PDF from scanned documents";
            s_document.Info.Keywords = "SirmiumERP, GFC";

            foreach(var image in paths)
            {
                this.DrawPage(s_document.AddPage(), image);
            }

            // Save the s_document...

            if (File.Exists(filename))
            {
                filename = $"{FilePath}{Title}_{Guid.NewGuid()}.pdf";
            }
            s_document.Save(filename);

            return filename;
        }
        void DrawPage(PdfPage page, string image)
        {
            XGraphics gfx = XGraphics.FromPdfPage(page);

            this.DrawImg(gfx, image);
        }

        private void DrawImg(XGraphics gfx, string filePath)
        {
            XImage image = XImage.FromFile(filePath);

            // Left position in point
            double x = (250 - image.PixelWidth * 72 / image.HorizontalResolution) / 2;
            gfx.DrawImage(image, 0, 0, gfx.PageSize.Width, gfx.PageSize.Height);
        }
    }
}
