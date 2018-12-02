using Microsoft.Office.Interop.Excel;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Reports.PhysicalPersons
{
    public class PhysicalPersonExcelReport
    {
        public static void Show(PhysicalPersonViewModel physicalPerson)
        {
            //Create excel workbook and sheet

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;

            excel.DisplayFullScreen = true;
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
            sheet1.PageSetup.LeftHeader = "&16&B Fizičko lice";
            sheet1.PageSetup.RightHeader = "&8Stranica &P/&N";

            sheet1.PageSetup.CenterHeaderPicture.Filename = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\image005.jpg";
            sheet1.PageSetup.CenterHeaderPicture.Width = 150;
            sheet1.PageSetup.CenterHeaderPicture.Height = 50;
            sheet1.PageSetup.CenterHeader = "&G";

            sheet1.PageSetup.CenterFooterPicture.Filename = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\erp8.png";
            sheet1.PageSetup.CenterFooterPicture.Width = 100;
            sheet1.PageSetup.CenterFooterPicture.Height = 40;
            sheet1.PageSetup.CenterFooter = "&G";


            int leftSideMin = 1;
            int rightSideMax = 9;

            int rowCounter = 1;

            rowCounter++;
            rowCounter++;
            rowCounter++;

            // rowCounter = 26; //top chart area

            #region Osnovni podaci

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "OSNOVNI PODACI";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            rowCounter++;
            int columnCounter = leftSideMin;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Šifra: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.PhysicalPersonCode;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Ime: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.Name + " " + physicalPerson.SurName;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;


            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Datum rođenja: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.DateOfBirth?.ToString("dd.MM.yyyy");

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Pol: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.Gender == 1 ? "Muski" : "Zenski";

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;


            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Država: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.Country?.Name;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Region: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.Region?.Name;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;


            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Opština: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.Municipality?.Name;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Grad: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.City?.Name;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;


            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Adresa: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.Address;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Gradilište: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.ConstructionSiteCode;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion

            #region Podaci o pasošu

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "PODACI O PASOŠU";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            rowCounter++;
            columnCounter = leftSideMin;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Država: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.PassportCountry?.Name;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Grad: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.PassportCity?.Name;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;


            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Broj pasoša: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.Passport;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Datum izdavanja od: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.VisaFrom?.ToString("dd.MM.yyyy");

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;


            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Važi do: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.VisaTo?.ToString("dd.MM.yyyy");

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion


            #region Podaci o prebivalištu

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "PODACI O PREBIVALIŠTU";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            rowCounter++;
            columnCounter = leftSideMin;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Država: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.ResidenceCountry?.Name;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Grad: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.ResidenceCity?.Name;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;


            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Adresa: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.ResidenceAddress;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion


            #region Podaci o vizi i radnoj dozvoli

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "PODACI O VIZI I RADNOJ DOZVOLI";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            rowCounter++;
            columnCounter = leftSideMin;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Prijem u ambasadu: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.EmbassyDate?.ToString("dd.MM.yyyy");

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Uzimanje vize: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.VisaDate?.ToString("dd.MM.yyyy");

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Datum vize od: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.VisaValidFrom?.ToString("dd.MM.yyyy");

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Datum vize do: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.VisaValidTo?.ToString("dd.MM.yyyy");

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Radna dozvola od: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.WorkPermitFrom?.ToString("dd.MM.yyyy");

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Radna dozvola do: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = physicalPerson.WorkPermitTo?.ToString("dd.MM.yyyy");

            columnCounter = leftSideMin;
            rowCounter++;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion



            rowCounter++;
            rowCounter++;

            rowCounter++;
            rowCounter++;
            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, 5], sheet1.Cells[rowCounter, 6]].Merge();
            sheet1.Range[sheet1.Cells[rowCounter, 5], sheet1.Cells[rowCounter, 6]].Font.Size = 10;
            sheet1.Range[sheet1.Cells[rowCounter, 5], sheet1.Cells[rowCounter, 6]] = "Odgovorno lice";

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, 5], sheet1.Cells[rowCounter, 7]].Merge();
            sheet1.Range[sheet1.Cells[rowCounter, 5], sheet1.Cells[rowCounter, 7]] = "_____________________________";

            sheet1.Columns.AutoFit();

        }
    }
}
