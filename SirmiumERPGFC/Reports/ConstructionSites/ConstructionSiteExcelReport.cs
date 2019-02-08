using Microsoft.Office.Interop.Excel;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Repository.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Reports.ConstructionSites
{
	public class ConstructionSiteExcelReport
	{
		public static void Show(ConstructionSiteViewModel constructionSite)
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
			sheet1.PageSetup.LeftHeader = "&16&B Gradilište";
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
			int rightSideMax = 8;

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

			sheet1.Cells[rowCounter, leftSideMin] = "OSNOVNI PODACI/GRUNDDATEN ÜBER DIE BAUSTELLEN";

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
			sheet1.Cells[rowCounter, columnCounter] = "Šifra/Code: ";

			sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
			sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter + 1] = constructionSite.Code;

			columnCounter += 4;

			sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter] = "Šifra gradilišta/Vertragsnummer: ";

			sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
			sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter + 1] = constructionSite.InternalCode;

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

			columnCounter = leftSideMin;
			rowCounter++;

			sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter] = "Naziv gradilišta/Baustelle: ";

			sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
			sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter + 1] = constructionSite.Name;

			

			columnCounter += 4;

			sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter] = "Država/Land: ";

			sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
			sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter + 1] = constructionSite.Country?.Name;

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

			columnCounter = leftSideMin;
			rowCounter++;


			sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter] = "Grad/Stadt: ";

			sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
			sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter + 1] = constructionSite.City?.Name;

			columnCounter += 4;

			sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter] = "Adresa/Adresse: ";

			sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
			sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter + 1] = constructionSite.Address;

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

			columnCounter = leftSideMin;
			rowCounter++;


			sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter] = "Maksimalni broj radnika/Maximale Anzahl der Arbeiter: ";

			sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
			sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter + 1] = constructionSite.MaxWorkers;

			columnCounter += 4;

			sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter] = "Datum predugovora/Werkvertrages von: ";

			sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
			sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter + 1] = constructionSite.ProContractDate.ToString("dd.MM.yyyy");

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

			columnCounter = leftSideMin;
			rowCounter++;


			sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter] = "Početak ugovora/Vertragsbeginn: ";

			sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
			sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter + 1] = constructionSite.ContractStart.ToString("dd.MM.yyyy");

			columnCounter += 4;

			sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter] = "Istek ugovora/Vertragsende: ";

			sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
			sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
			sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
			sheet1.Cells[rowCounter, columnCounter + 1] = constructionSite.ContractExpiration.ToString("dd.MM.yyyy");

			// line
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

			#endregion

			#region PODACI O DOKUMENTIMA

			rowCounter++;
			rowCounter++;

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

			sheet1.Cells[rowCounter, leftSideMin] = "DOKUMENTI/DOKUMENTE";

			rowCounter++;

			var constructionSiteDocuments = new ConstructionSiteDocumentSQLiteRepository().GetConstructionSiteDocumentsByConstructionSite(MainWindow.CurrentCompanyId, constructionSite.Identifier)?.ConstructionSiteDocuments ?? new List<ConstructionSiteDocumentViewModel>();

			rowCounter++;

			//sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
			//sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			//sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
			//sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
			//sheet1.Cells[rowCounter, columnCounter] = "DATUM: ";

			//sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
			//sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			//sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
			//sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
			//sheet1.Cells[rowCounter, columnCounter + 1] = constructionSiteDocuments.;

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

			sheet1.Cells[rowCounter, leftSideMin] = "RB.";
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 2]].Merge();
			sheet1.Cells[rowCounter, leftSideMin + 1] = "IME/NAME";
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 3], sheet1.Cells[rowCounter, leftSideMin + 6]].Merge();
			sheet1.Cells[rowCounter, leftSideMin + 3] = "DATUM";
			//sheet1.Cells[rowCounter, leftSideMin + 7] = "PUTANJA";


			rowCounter++;
			//rowCounter++; 

			// line
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

			int columnCounterItem = leftSideMin;

			for (int i = 0; i < constructionSiteDocuments?.Count; i++)
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
				sheet1.Cells[rowCounter, columnCounterItem] = constructionSiteDocuments[i].Name;
				columnCounterItem += 2;

				sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
				sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 3]].Merge();
				sheet1.Cells[rowCounter, columnCounterItem] = constructionSiteDocuments[i].CreateDate?.ToString("dd.MM.yyyy");

				//columnCounterItem += 4;

				//sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				//sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
				//sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
				//sheet1.Cells[rowCounter, columnCounterItem] = constructionSiteDocuments[i].Path;

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

			sheet1.Cells[rowCounter, leftSideMin] = "NAPOMENE/ANMERKUNG";

			rowCounter++;

			var constructionSiteNote = new ConstructionSiteNoteSQLiteRepository().GetConstructionSiteNotesByConstructionSite(MainWindow.CurrentCompanyId, constructionSite.Identifier)?.ConstructionSiteNotes ?? new List<ConstructionSiteNoteViewModel>();

			rowCounter++;

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

			sheet1.Cells[rowCounter, leftSideMin] = "RB.";
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 7]].Merge();
			sheet1.Cells[rowCounter, leftSideMin + 1] = "NAPOMENA/ANMERKUNG";
			sheet1.Cells[rowCounter, leftSideMin + 8] = "DATUM";

			rowCounter++;
			//rowCounter++; 

			// line
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
			sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

			columnCounterItem = leftSideMin;

			for (int i = 0; i < constructionSiteNote?.Count; i++)
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
				sheet1.Cells[rowCounter, columnCounterItem] = constructionSiteNote[i].Note;
				columnCounterItem += 7;

				sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
				sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
				sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
				sheet1.Cells[rowCounter, columnCounterItem] = constructionSiteNote[i].NoteDate.ToString("dd.MM.yyyy");

				sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

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
			sheet1.Range[sheet1.Cells[rowCounter, 5], sheet1.Cells[rowCounter, 6]] = "Odgovorno lice/Verantwortliches Gesicht";

			rowCounter++;
			rowCounter++;

			sheet1.Range[sheet1.Cells[rowCounter, 5], sheet1.Cells[rowCounter, 7]].Merge();
			sheet1.Range[sheet1.Cells[rowCounter, 5], sheet1.Cells[rowCounter, 7]] = "_____________________________";

			sheet1.Columns.AutoFit();

		}
	}
}
