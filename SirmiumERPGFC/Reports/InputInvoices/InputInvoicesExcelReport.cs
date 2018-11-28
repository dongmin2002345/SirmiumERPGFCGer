using Microsoft.Office.Interop.Excel;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Reports.InputInvoices
{
	public class InputInvoicesExcelReport
	{
		public static void Show(List<InputInvoiceViewModel> inputInvoices)
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

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

			string[,] tabelaNasloviKolona = new string[,] { { "RB.", "POSLOVNI PARTNER", "DOBAVLJAČ", "ADRESA", "BROJ FAKTURE", "DATUM FAKTURE", "IZNOS NETO", "PDV%", "PDV", "IZNOS BRUTO", "VALUTA", "DATUM PLAĆANJA", "STATUS", "DATUM STATUSA" } };
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]] = tabelaNasloviKolona;

			rowCounter++;
			//rowCounter++; 

			// line
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

			rowCounter++;
			int columnCounter = leftSideMin;

			for (int i = 0; i < inputInvoices?.Count; i++)
			{
				columnCounter = leftSideMin;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = (i + 1) + ". ";
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].BusinessPartner?.Name;
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].Supplier;
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].Address;
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].InvoiceNumber.ToString();
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter].C = 10;
                sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].InvoiceDate.ToString("dd.MM.yyyy");
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].AmountNet;
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].PDVPercent;
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].PDV;
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].AmountGross;
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].Currency;
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].DateOfPaymet.ToString("dd.MM.yyyy");
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].Status;
				columnCounter++;

				sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounter] = inputInvoices[i].StatusDate.ToString("dd.MM.yyyy");
				columnCounter++;

				rowCounter++;

				sheet1.Range[sheet1.Cells[rowCounter - 1, leftSideMin], sheet1.Cells[rowCounter - 1, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;
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
