using Microsoft.Office.Interop.Excel;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Reports.Employees
{
    public class MS0129Report
    {
        public static void Show(EmployeeViewModel employee)
        {
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

            int column1Min = 1;
            int column1Max = 12;
            int column2Min = 13;
            int column2Max = 24;
            int column3Min = 25;
            int column3Max = 36;

            for (int i = 1; i <= column3Max; i++)
            {
                sheet1.Columns[i].ColumnWidth = 3;
            }
            

            int rowCounter = 1;

            sheet1.Cells[rowCounter, column1Min] = "Bei Auswahlkästchen";

            sheet1.Range[sheet1.Cells[rowCounter, column1Max - 2], sheet1.Cells[rowCounter, column1Max - 2]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Max - 2], sheet1.Cells[rowCounter, column1Max - 2]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Max - 2], sheet1.Cells[rowCounter, column1Max - 2]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Max - 2], sheet1.Cells[rowCounter, column1Max - 2]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Cells[rowCounter, column2Min] = "Zutreffendes bitte ankreuzen";

            sheet1.Range[sheet1.Cells[rowCounter, column2Max - 2], sheet1.Cells[rowCounter, column2Max - 2]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Max - 2], sheet1.Cells[rowCounter, column2Max - 2]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Max - 2], sheet1.Cells[rowCounter, column2Max - 2]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Max - 2], sheet1.Cells[rowCounter, column2Max - 2]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 18;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 30;
            sheet1.Cells[rowCounter, column1Min] = "Antrag auf Zustimmung zum Aufenthaltstitel";

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "für Werkvertragsarbeitnehmer in Form einer ";

            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter, column3Max]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 18;
            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 30;
            sheet1.Cells[rowCounter, column2Min] = "Werkvertragsarbeitnehmerkarte";


            sheet1.Range[sheet1.Cells[rowCounter - 1, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter - 1, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter - 1, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter - 1, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "Angaben zum ausländischen Arbeitnehmer";

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "1 Name";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Merge();
            sheet1.Cells[rowCounter + 1, column1Min] = employee.Name;
            sheet1.Cells[rowCounter + 1, column1Min].HorizontalAlignment = XlHAlign.xlHAlignCenter;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;


            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column3Min] = "2 ggf. Geburtsname";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Merge();

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "Vorname";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Merge();
            sheet1.Cells[rowCounter + 1, column1Min] = employee.SurName;
            sheet1.Cells[rowCounter + 1, column1Min].HorizontalAlignment = XlHAlign.xlHAlignCenter;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column3Min] = "4 Staatsangehörigkeit";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Merge();
            sheet1.Cells[rowCounter + 1, column3Min] = employee.Country?.Name;
            sheet1.Cells[rowCounter + 1, column3Min].HorizontalAlignment = XlHAlign.xlHAlignCenter;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "3 Geburtsdatum";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Merge();
            sheet1.Cells[rowCounter + 1, column1Min] = employee.DateOfBirth?.ToString("dd.MM.yyyy");
            sheet1.Cells[rowCounter + 1, column1Min].HorizontalAlignment = XlHAlign.xlHAlignCenter;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max - 4]].Merge();
            sheet1.Cells[rowCounter, column3Min] = "5 Geschlecht";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column3Min], sheet1.Cells[rowCounter + 1, column3Max - 4]].Merge();
            sheet1.Range[sheet1.Cells[rowCounter, column3Max - 3], sheet1.Cells[rowCounter, column3Max - 1]].Merge();
            sheet1.Cells[rowCounter, column3Max - 3] = "männlich";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column3Max - 3], sheet1.Cells[rowCounter + 1, column3Max - 1]].Merge();
            sheet1.Cells[rowCounter + 1, column3Max - 3] = "weiblich";

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max - 1]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Max], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "6 Name und Anschrift des entsendenden Unternehmens bzw. der Niederlassung im";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Merge();
            sheet1.Rows[rowCounter + 1].RowHeight = 15;
            sheet1.Cells[rowCounter + 1, column1Min] = "Bundesgebiet";
            sheet1.Range[sheet1.Cells[rowCounter + 2, column1Min], sheet1.Cells[rowCounter + 2, column2Max]].Merge();
            sheet1.Rows[rowCounter + 2].RowHeight = 45;
            sheet1.Cells[rowCounter + 2, column1Min] = employee.Company?.CompanyName;
            sheet1.Cells[rowCounter + 2, column1Min].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter + 2, column1Min].VerticalAlignment = XlVAlign.xlVAlignCenter;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 2, column2Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 2, column2Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 2, column2Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column3Min] = "7 Wohnung im Bundesgebiet";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Merge();
            sheet1.Rows[rowCounter + 1].RowHeight = 15;
            sheet1.Cells[rowCounter + 1, column3Min] = "- soweit nicht nebenstehend -";
            sheet1.Range[sheet1.Cells[rowCounter + 2, column3Min], sheet1.Cells[rowCounter + 2, column3Max]].Merge();
            sheet1.Rows[rowCounter + 2].RowHeight = 45;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 2, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 2, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 2, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "8 Pass-Nr. oder Passersatz-Nr.";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column1Max]].Merge();
            sheet1.Cells[rowCounter + 1, column1Min] = employee.Passport;
            sheet1.Cells[rowCounter + 1, column1Min].HorizontalAlignment = XlHAlign.xlHAlignCenter;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column1Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column1Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column1Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter, column2Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter, column2Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column2Min] = "9 ausgestellt am";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column2Min], sheet1.Cells[rowCounter + 1, column2Max]].Merge();
            sheet1.Cells[rowCounter + 1, column2Min] = employee.VisaFrom?.ToString("dd.MM.yyyy");
            sheet1.Cells[rowCounter + 1, column2Min].HorizontalAlignment = XlHAlign.xlHAlignCenter;

            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter, column2Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column3Min] = "10 von Behörde/Staat";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Merge();
            sheet1.Cells[rowCounter + 1, column3Min] = employee.PassportCity?.Name;
            sheet1.Cells[rowCounter + 1, column3Min].HorizontalAlignment = XlHAlign.xlHAlignCenter;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max - 4]].Merge();
            sheet1.Cells[rowCounter, column1Min] = "11 Aufenthaltstitel ist";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column1Max - 4]].Merge();
            sheet1.Range[sheet1.Cells[rowCounter, column1Max - 3], sheet1.Cells[rowCounter, column1Max - 1]].Merge();
            sheet1.Cells[rowCounter, column1Max - 3] = "erteilt";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Max - 3], sheet1.Cells[rowCounter + 1, column1Max - 1]].Merge();
            sheet1.Cells[rowCounter + 1, column1Max - 3] = "beantragt";

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column1Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column1Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column1Max - 1]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column1Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Max], sheet1.Cells[rowCounter, column1Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter, column2Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter, column2Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column2Min] = "bis";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column2Min], sheet1.Cells[rowCounter + 1, column2Max]].Merge();

            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter, column2Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column3Min] = "12 von/bei Ausländerbehörde";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Merge();
            sheet1.Cells[rowCounter + 1, column3Min] = employee.EmployeeProfessions?.FirstOrDefault()?.Profession?.Name;
            sheet1.Cells[rowCounter + 1, column3Min].HorizontalAlignment = XlHAlign.xlHAlignCenter;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 18;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 30;
            sheet1.Cells[rowCounter, column1Min] = "Werkvertragsarbeitnehmerkarte wird beantragt";

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "13 von";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column1Max]].Merge();

            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter, column2Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column2Min], sheet1.Cells[rowCounter, column2Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column2Min] = "bis";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column2Min], sheet1.Cells[rowCounter + 1, column2Max]].Merge();

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column3Min] = "14 als (Art der auszuübenden Beschäftigung)";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Merge();

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "15 im Rahmen des Werkvertrages vom";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Merge();

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column3Min] = "16 Auftragsnummer";

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            for (int i = 0; i < 12; i++)
                sheet1.Range[sheet1.Cells[rowCounter + 1, column3Min + i], sheet1.Cells[rowCounter + 1, column3Min + i]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "17 Auftragnehmer (ausländisches Unternehmen)";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Merge();

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column3Min] = "18 Auftraggeber";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Merge();

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "19 Betriebsstätte/Baustelle (Anschrift: Straße, Nr., PLZ, Ort)";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column3Max]].Merge();
            sheet1.Rows[rowCounter + 1].RowHeight = 45;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "20 Es wird bestätigt, dass der Arbeitnehmer entsprechend dem Antrag beschäftigt ";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column2Max]].Merge();
            sheet1.Rows[rowCounter + 1].RowHeight = 15;
            sheet1.Cells[rowCounter + 1, column1Min] = "werden soll";
            sheet1.Range[sheet1.Cells[rowCounter + 2, column1Min], sheet1.Cells[rowCounter + 2, column2Max]].Merge();
            sheet1.Rows[rowCounter + 2].RowHeight = 15;
            sheet1.Cells[rowCounter + 2, column1Min] = "Stempel und Unterschrift des Arbeitgebers";
            sheet1.Cells[rowCounter + 2, column1Min].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter + 3, column1Min], sheet1.Cells[rowCounter + 3, column2Max]].Merge();
            sheet1.Rows[rowCounter + 3].RowHeight = 45;
            sheet1.Range[sheet1.Cells[rowCounter + 4, column1Min], sheet1.Cells[rowCounter + 4, column2Max]].Merge();
            sheet1.Rows[rowCounter + 4].RowHeight = 15;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 4, column2Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 4, column2Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 4, column2Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column3Min] = "21 Unterschrift des Arbeitnehmers";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column3Min], sheet1.Cells[rowCounter + 1, column3Max]].Merge();
            sheet1.Rows[rowCounter + 1].RowHeight = 15;
            sheet1.Range[sheet1.Cells[rowCounter + 2, column3Min], sheet1.Cells[rowCounter + 2, column3Max]].Merge();
            sheet1.Rows[rowCounter + 2].RowHeight = 15;
            sheet1.Range[sheet1.Cells[rowCounter + 3, column3Min], sheet1.Cells[rowCounter + 3, column3Max]].Merge();
            sheet1.Rows[rowCounter + 3].RowHeight = 45;
            sheet1.Range[sheet1.Cells[rowCounter + 4, column3Min], sheet1.Cells[rowCounter + 4, column3Max]].Merge();
            sheet1.Rows[rowCounter + 4].RowHeight = 15;
            sheet1.Cells[rowCounter + 4, column3Min] = "22 Datum";

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 4, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 4, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter + 4, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;
            rowCounter++;

            rowCounter++;
            rowCounter++;
            rowCounter++;
            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 8;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "Wird von der Agentur für Arbeit Stuttgart ausgefüllt";

            rowCounter++;


            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 18;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 60;
            sheet1.Cells[rowCounter, column1Min] = "Werkvertragsarbeitnehmerkarte wird beantragt";
            sheet1.Cells[rowCounter, column1Min].HorizontalAlignment = XlHAlign.xlHAlignRight;
            sheet1.Cells[rowCounter, column1Min].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            Microsoft.Office.Interop.Excel.Range picPosition = sheet1.Cells[rowCounter, 1]; // retrieve the range for picture insert
            Microsoft.Office.Interop.Excel.Pictures p = sheet1.Pictures(System.Reflection.Missing.Value) as Microsoft.Office.Interop.Excel.Pictures;
            Microsoft.Office.Interop.Excel.Picture pic = p.Insert(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Bundesagentur.png", Type.Missing);
            pic.Left = Convert.ToDouble(picPosition.Left) + 5;
            pic.Top = picPosition.Top + 5;
            pic.Placement = Microsoft.Office.Interop.Excel.XlPlacement.xlMoveAndSize;

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "Die Zustimmung zur Erteilung eines Aufenthaltstitels an den oben genannten Arbeitnehmer, der auf der Grundlage einer ";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column3Max]].Merge();
            sheet1.Rows[rowCounter + 1].RowHeight = 15;
            sheet1.Cells[rowCounter + 1, column1Min] = "zwischenstaatlichen Vereinbarung über die Entsendung und Beschäftigung von Werkvertragsarbeitnehmern entsandt wurde, wird ";
            sheet1.Range[sheet1.Cells[rowCounter + 2, column1Min], sheet1.Cells[rowCounter + 2, column3Max]].Merge();
            sheet1.Rows[rowCounter + 2].RowHeight = 15;
            sheet1.Cells[rowCounter + 2, column1Min] = "gemäß § 39 Aufenthaltsgesetz in Form einer Werkvertragsarbeitnehmerkarte erteilt.";
            sheet1.Range[sheet1.Cells[rowCounter + 3, column1Min], sheet1.Cells[rowCounter + 3, column3Max]].Merge();
            sheet1.Rows[rowCounter + 3].RowHeight = 45;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 3, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 3, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 3, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;
            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "Diese Werkvertragsarbeitnehmerkarte gilt nur in Verbindung mit einem gültigen Aufenthaltstitel, für eine berufliche Tätigkeit nach ";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column3Max]].Merge();
            sheet1.Rows[rowCounter + 1].RowHeight = 15;
            sheet1.Cells[rowCounter + 1, column1Min] = "Ziffer 14 nur bei dem unter Ziffer 17 genannten Unternehmen, nur für die unter Ziffer 16 genannte Auftragsnummer und nur für die ";
            sheet1.Range[sheet1.Cells[rowCounter + 2, column1Min], sheet1.Cells[rowCounter + 2, column3Max]].Merge();
            sheet1.Rows[rowCounter + 2].RowHeight = 15;
            sheet1.Cells[rowCounter + 2, column1Min] = "unter Ziffer 19 genannte Betriebsstätte/Baustelle.";
            sheet1.Range[sheet1.Cells[rowCounter + 3, column1Min], sheet1.Cells[rowCounter + 3, column3Max]].Merge();
            sheet1.Rows[rowCounter + 3].RowHeight = 45;


            sheet1.Range[sheet1.Cells[rowCounter + 3, column1Min + 6], sheet1.Cells[rowCounter + 3, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Cells[rowCounter + 4, column1Min + 6] = "Geltungsdauer";
            sheet1.Cells[rowCounter + 4, column3Min] = "von";
            sheet1.Cells[rowCounter + 4, column3Min + 6] = "bis";
            sheet1.Range[sheet1.Cells[rowCounter + 4, column3Min], sheet1.Cells[rowCounter + 4, column3Min]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter + 5, column1Min + 6], sheet1.Cells[rowCounter + 5, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            for(int i = 0; i < 12; i++)
                sheet1.Range[sheet1.Cells[rowCounter + 5, column3Min + i], sheet1.Cells[rowCounter + 5, column3Min + i]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter + 6, column1Min], sheet1.Cells[rowCounter + 6, column3Max]].Merge();
            sheet1.Rows[rowCounter + 6].RowHeight = 45;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 6, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 6, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 6, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;
            rowCounter++;
            rowCounter++;
            rowCounter++;
            rowCounter++;
            rowCounter++;


            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max + 6]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Min] = "Agentur für Arbeit ";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Min], sheet1.Cells[rowCounter + 1, column1Max + 6]].Merge();
            sheet1.Rows[rowCounter + 1].RowHeight = 15;
            sheet1.Cells[rowCounter + 1, column1Min] = "Stuttgart";
            sheet1.Range[sheet1.Cells[rowCounter + 2, column1Min], sheet1.Cells[rowCounter + 2, column1Max + 6]].Merge();
            sheet1.Rows[rowCounter + 3].RowHeight = 45;
            sheet1.Range[sheet1.Cells[rowCounter + 3, column1Min], sheet1.Cells[rowCounter + 3, column1Max + 6]].Merge();
            sheet1.Rows[rowCounter + 4].RowHeight = 15;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column2Max + 6]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 4, column2Max + 6]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 4, column2Max + 6]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter + 4, column2Max + 6]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column1Max + 7], sheet1.Cells[rowCounter, column2Max + 6]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column1Max + 7] = "Im Auftrag ";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column1Max + 7], sheet1.Cells[rowCounter + 1, column2Max + 6]].Merge();
            sheet1.Rows[rowCounter + 1].RowHeight = 15;
            sheet1.Range[sheet1.Cells[rowCounter + 2, column1Max + 7], sheet1.Cells[rowCounter + 2, column2Max + 6]].Merge();
            sheet1.Rows[rowCounter + 3].RowHeight = 45;
            sheet1.Range[sheet1.Cells[rowCounter + 3, column1Max + 7], sheet1.Cells[rowCounter + 3, column2Max + 6]].Merge();
            sheet1.Rows[rowCounter + 4].RowHeight = 15;
            sheet1.Cells[rowCounter + 4, column1Max + 7] = "Stuttgart";

            sheet1.Range[sheet1.Cells[rowCounter, column1Max + 7], sheet1.Cells[rowCounter, column1Max + 6]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Max + 7], sheet1.Cells[rowCounter + 4, column2Max + 6]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Max + 7], sheet1.Cells[rowCounter + 4, column2Max + 6]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column1Max + 7], sheet1.Cells[rowCounter + 4, column2Max + 6]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            sheet1.Range[sheet1.Cells[rowCounter, column2Max + 7], sheet1.Cells[rowCounter, column3Max]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, column2Max + 7], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Rows[rowCounter].RowHeight = 15;
            sheet1.Cells[rowCounter, column2Max + 7] = "Dienststempel";
            sheet1.Range[sheet1.Cells[rowCounter + 1, column2Max + 7], sheet1.Cells[rowCounter + 1, column3Max]].Merge();
            sheet1.Rows[rowCounter + 1].RowHeight = 15;
            sheet1.Range[sheet1.Cells[rowCounter + 2, column2Max + 7], sheet1.Cells[rowCounter + 2, column3Max]].Merge();
            sheet1.Rows[rowCounter + 2].RowHeight = 15;
            sheet1.Range[sheet1.Cells[rowCounter + 3, column2Max + 7], sheet1.Cells[rowCounter + 3, column3Max]].Merge();
            sheet1.Rows[rowCounter + 3].RowHeight = 45;
            sheet1.Range[sheet1.Cells[rowCounter + 4, column2Max + 7], sheet1.Cells[rowCounter + 4, column3Max]].Merge();
            sheet1.Rows[rowCounter + 4].RowHeight = 15;

            sheet1.Range[sheet1.Cells[rowCounter, column2Max + 7], sheet1.Cells[rowCounter, column3Max]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Max + 7], sheet1.Cells[rowCounter + 4, column3Max]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Max + 7], sheet1.Cells[rowCounter + 4, column3Max]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, column2Max + 7], sheet1.Cells[rowCounter + 4, column3Max]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

            rowCounter++;
            rowCounter++;
            rowCounter++;
            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column3Max]].Font.Size = 8;
            sheet1.Range[sheet1.Cells[rowCounter, column1Min], sheet1.Cells[rowCounter, column1Max]].Merge();
            sheet1.Cells[rowCounter, column1Min].HorizontalAlignment = XlHAlign.xlHAlignLeft;
            sheet1.Cells[rowCounter, column1Min] = "BA-Ausl. Nr.1 WV-AK 01 18";

            sheet1.Range[sheet1.Cells[rowCounter, column3Min], sheet1.Cells[rowCounter, column3Max]].Merge();
            sheet1.Cells[rowCounter, column3Min].HorizontalAlignment = XlHAlign.xlHAlignRight;
            sheet1.Cells[rowCounter, column3Min] = "BUNDESDRUCKEREI Artikel 3102385";
        }
    }
}
