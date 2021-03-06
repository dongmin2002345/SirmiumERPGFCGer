﻿using Microsoft.Office.Interop.Excel;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppCommonCode.Converters;

namespace SirmiumERPGFC.Reports.OutputInvoices
{
    public class OutputInvoiceExcelReport
    {
        public static void Show(OutputInvoiceViewModel outputInvoice)
        {
            //Create excel workbook and sheet
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;

            //excel.DisplayFullScreen = true;
            excel.WindowState = XlWindowState.xlMaximized;

            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            //excel.StandardFont = "Calibri"; //"Arial Narrow" "Times New Roman" "Arial" "Bahnschrift SemiBold Condensed"
            //excel.StandardFontSize = 8;

            sheet1.PageSetup.PaperSize = XlPaperSize.xlPaperA4;
            sheet1.PageSetup.Orientation = XlPageOrientation.xlPortrait;
            sheet1.PageSetup.FitToPagesTall = false;
            sheet1.PageSetup.FitToPagesWide = 1;
            sheet1.PageSetup.Zoom = false;


            // Set header rows
            sheet1.PageSetup.PrintTitleRows = "$1:$2";

            sheet1.PageSetup.HeaderMargin = 30;
            sheet1.PageSetup.LeftHeader = "&16&B Ulazne fakture";
            sheet1.PageSetup.RightHeader = "&8Stranica &P/&N";

            sheet1.PageSetup.CenterHeaderPicture.Filename = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\image005.jpg";
            sheet1.PageSetup.CenterHeaderPicture.Width = 150;
            sheet1.PageSetup.CenterHeaderPicture.Height = 50;
            sheet1.PageSetup.CenterHeader = "&G";

            sheet1.PageSetup.CenterFooterPicture.Filename = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\erp8.png";
            sheet1.PageSetup.CenterFooterPicture.Width = 100;
            sheet1.PageSetup.CenterFooterPicture.Height = 40;
            sheet1.PageSetup.CenterFooter = "&G";
            //sheet1.PageSetup.LeftFooter = " www.sirmiumerp.com ";
            //sheet1.PageSetup.CenterFooter = " www.sirmiumerp.com ";


            int leftSideMin = 1;
            int rightSideMax = 14;

            int rowCounter = 1;

            rowCounter++;
            rowCounter++;
            rowCounter++;

            // rowCounter = 26; //top chart area

            #region Table

            rowCounter++;
            rowCounter++;

            int columnCounter = 2;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "POSLOVNI PARTNER/GESCHÄFTSPARTNER: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.BusinessPartner?.Name;

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "DOBAVLJAČ/LIEFERANT: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.Supplier;

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "ADRESA/ADRESSE: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.Address;

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "BROJ FAKTURE/FAKTURNUMMER: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.InvoiceNumber;

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "DATUM FAKTURE/FAKTURDATUM: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.InvoiceDate.ToString("dd.MM.yyyy");

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "IZNOS NETO/BETRAG NETTO: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.AmountNet;

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "PDV%: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.PdvPercent;

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "PDV: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.Pdv;

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "IZNOS BRUTO/BETRAG BRUTTO: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.AmountGross;

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "VALUTA: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.Currency;

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "DATUM PLAĆANJA/ZAHLUNGSDATUM: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.DateOfPayment.ToString("dd.MM.yyyy");

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "STATUS: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = new ChooseStatusConverter().Convert(outputInvoice.Status, null, null, CultureInfo.InvariantCulture);

            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "DATUM STATUSA/STATUSDATUM: ";

            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = outputInvoice.StatusDate.ToString("dd.MM.yyyy");

            rowCounter++;

            #endregion

            rowCounter++;
            rowCounter++;
            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, 3], sheet1.Cells[rowCounter, 4]].Merge();
            sheet1.Range[sheet1.Cells[rowCounter, 3], sheet1.Cells[rowCounter, 4]].Font.Size = 10;
            sheet1.Range[sheet1.Cells[rowCounter, 3], sheet1.Cells[rowCounter, 4]] = "Odgovorno lice/Verantwortliche Person";

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, 3], sheet1.Cells[rowCounter, 5]].Merge();
            sheet1.Range[sheet1.Cells[rowCounter, 3], sheet1.Cells[rowCounter, 5]] = "_____________________________";

            sheet1.Columns.AutoFit();

        }
    }
}
