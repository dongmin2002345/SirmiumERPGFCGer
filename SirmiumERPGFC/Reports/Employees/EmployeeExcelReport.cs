using Microsoft.Office.Interop.Excel;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Reports.Employees
{
    public class EmployeeExcelReport
    {
        public static void Show(EmployeeViewModel employee)
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
            sheet1.PageSetup.LeftHeader = "&16&B Radnik";
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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.EmployeeCode;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.Name + " " + employee.SurName;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.DateOfBirth?.ToString("dd.MM.yyyy");

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.Gender == 1 ? "Muski" : "Zenski";

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.Country?.Name;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.Region?.Name;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.Municipality?.Name;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.City?.Name;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.Address;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.ConstructionSiteCode;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.PassportCountry?.Name;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.PassportCity?.Name;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.Passport;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.VisaFrom?.ToString("dd.MM.yyyy");

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.VisaTo?.ToString("dd.MM.yyyy");

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.ResidenceCountry?.Name;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.ResidenceCity?.Name;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.ResidenceAddress;

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.EmbassyDate?.ToString("dd.MM.yyyy");

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.VisaDate?.ToString("dd.MM.yyyy");

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.VisaValidFrom?.ToString("dd.MM.yyyy");

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.VisaValidTo?.ToString("dd.MM.yyyy");

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.WorkPermitFrom?.ToString("dd.MM.yyyy");

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
            sheet1.Cells[rowCounter, columnCounter + 1] = employee.WorkPermitTo?.ToString("dd.MM.yyyy");

            columnCounter = leftSideMin;
            rowCounter++;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion







            #region Zanimanja

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "ZANIMANJA";

            rowCounter++;

            var employeeProfessionItems = new EmployeeProfessionItemSQLiteRepository().GetEmployeeProfessionsByEmployee(MainWindow.CurrentCompanyId, employee.Identifier)?.EmployeeProfessionItems ?? new List<EmployeeProfessionItemViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 3]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "DRŽAVA";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 4], sheet1.Cells[rowCounter, leftSideMin + 8]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 4] = "ZANIMANJE";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            int columnCounterItem = leftSideMin;

            for (int i = 0; i < employeeProfessionItems?.Count; i++)
            {
                columnCounterItem = leftSideMin;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = (i + 1) + ". ";

                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 2]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = employeeProfessionItems[i].Country?.Name;
                columnCounterItem += 3;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 4]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = employeeProfessionItems[i].Profession?.Name;

                sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

                rowCounter++;
            }

            rowCounter--;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion


            #region Stavke dozvole

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "STAVKE DOZVOLE";

            rowCounter++;

            var employeeLicenceItems = new EmployeeLicenceItemSQLiteRepository().GetEmployeeLicencesByEmployee(MainWindow.CurrentCompanyId, employee.Identifier)?.EmployeeLicenceItems ?? new List<EmployeeLicenceItemViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 2]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "DRŽAVA";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 3], sheet1.Cells[rowCounter, leftSideMin + 6]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 3] = "DOZVOLA";
            sheet1.Cells[rowCounter, leftSideMin + 7] = "VAŽI OD";
            sheet1.Cells[rowCounter, leftSideMin + 8] = "VAŽI DO";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            columnCounterItem = leftSideMin;

            for (int i = 0; i < employeeLicenceItems?.Count; i++)
            {
                columnCounterItem = leftSideMin;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = (i + 1) + ". ";

                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 1]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = employeeLicenceItems[i].Country?.Name;
                columnCounterItem += 2;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 3]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = employeeLicenceItems[i].Licence?.Code;

                columnCounterItem+=4;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = employeeLicenceItems[i].ValidFrom?.ToString("dd.MM.yyyy");

                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = employeeLicenceItems[i].ValidTo?.ToString("dd.MM.yyyy");

                sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

                rowCounter++;
            }

            rowCounter--;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion




            #region Spajanje porodice

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "SPAJANJE PORODICE";

            rowCounter++;

            var employeeItems = new EmployeeItemSQLiteRepository().GetEmployeeItemsByEmployee(MainWindow.CurrentCompanyId, employee.Identifier)?.EmployeeItems ?? new List<EmployeeItemViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 2]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "ČLAN PORODICE";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 3], sheet1.Cells[rowCounter, leftSideMin + 5]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 3] = "IME";
            sheet1.Cells[rowCounter, leftSideMin + 6] = "DATUM ROĐENJA";
            sheet1.Cells[rowCounter, leftSideMin + 7] = "BROJ PASOŠA";
            sheet1.Cells[rowCounter, leftSideMin + 8] = "DATUM AMBASADE";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            columnCounterItem = leftSideMin;

            for (int i = 0; i < employeeItems?.Count; i++)
            {
                columnCounterItem = leftSideMin;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = (i + 1) + ". ";

                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 1]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = employeeItems[i].FamilyMember?.Name;
                columnCounterItem += 2;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 2]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = employeeItems[i].Name;

                columnCounterItem += 3;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = employeeItems[i].DateOfBirth?.ToString("dd.MM.yyyy");

                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = employeeItems[i].Passport;

                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = employeeItems[i].EmbassyDate?.ToString("dd.MM.yyyy");

                sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

                rowCounter++;
            }

            rowCounter--;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion




            #region Napomene

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "NAPOMENE";

            rowCounter++;

            var employeeNotes = new EmployeeNoteSQLiteRepository().GetEmployeeNotesByEmployee(MainWindow.CurrentCompanyId, employee.Identifier)?.EmployeeNotes ?? new List<EmployeeNoteViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 7]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "NAPOMENA";
            sheet1.Cells[rowCounter, leftSideMin + 8] = "DATUM NAPOMENE";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            columnCounterItem = leftSideMin;

            for (int i = 0; i < employeeNotes?.Count; i++)
            {
                columnCounterItem = leftSideMin;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = (i + 1) + ". ";

                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 6]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = employeeNotes[i].Note;
                columnCounterItem += 7;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = employeeNotes[i].NoteDate.ToString("dd.MM.yyyy");

                sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

                rowCounter++;
            }

            rowCounter--;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion





            #region Istorija radnika

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "ISTORIJA RADNIKA";

            rowCounter++;

            var employeeCards = new EmployeeCardSQLiteRepository().GetEmployeeCardsByEmployee(MainWindow.CurrentCompanyId, employee.Identifier)?.EmployeeCards ?? new List<EmployeeCardViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 7]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "NAPOMENA";
            sheet1.Cells[rowCounter, leftSideMin + 8] = "DATUM IZMENE";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            columnCounterItem = leftSideMin;

            for (int i = 0; i < employeeCards?.Count; i++)
            {
                columnCounterItem = leftSideMin;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = (i + 1) + ". ";

                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignLeft;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 6]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem].WrapText = true;
                sheet1.Cells[rowCounter, columnCounterItem] = employeeCards[i].Description;
                columnCounterItem += 7;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = employeeCards[i].CardDate?.ToString("dd.MM.yyyy");

                sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

                sheet1.Rows[rowCounter].RowHeight = 30;

                rowCounter++;
            }

            rowCounter--;

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
