using Microsoft.Office.Interop.Excel;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Repository.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Reports.BusinessPartners
{
    public class BusinessPartnerReport
    {
        public static void Show(BusinessPartnerViewModel businessPartner)
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
            int rightSideMax = 10;

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

            sheet1.Cells[rowCounter, leftSideMin] = "Poslovni partneri Srbija/Geschäftspartner Serbien";

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
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.Code;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Interna šifra/Interne Nummer: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.InternalCode;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Ime/Name: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.Name;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Matični broj/Identifikationsnummer: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            //sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.Gender == 1 ? "Muski" : "Zenski";
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.IdentificationNumber;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;


            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Poreski broj (PIB/JMBG): ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.PIB;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Registarski broj/Registrierungsnummer: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.PIO;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "PDV broj/MwSt: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            //sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.PDV;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Rabat/Rabatt: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.Customer;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;


            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Valuta/Währung: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.DueDate;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Obveznik PDV-a/Mehrwertsteuerpflicht: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.IsInPDV ? "Ne/Nein" : "Da/Ja";

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Sajt/Web­sei­te: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.WebSite;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion

            #region Podaci o poslovnom partneru Nemačka

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "PODACI O POSLOVNOM PARTNERU NEMAČKA/Geschäftspartner-Deutschland";

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
            sheet1.Cells[rowCounter, columnCounter] = "Šifra/Code: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.InternalCode;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Naziv kompanije/Firmenname: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.Name;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;


            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Država/Land: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.Country?.Name;

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Sektor/Sektorname: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            //sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.VisaFrom?.ToString("dd.MM.yyyy");
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.Sector?.Name;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;


            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Delatnost/Tätigkeit: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.Agency?.Name; //ili sektor?

            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Poreski broj/Steuernummer: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.TaxNr;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Obveznik PDV-a/Mehrwertsteuerpflicht: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.IsInPDVGer ? "Ne/Nein" : "Da/Ja";

            //columnCounter += 4; PROBLEM!!!

            //sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            //sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            //sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            //sheet1.Cells[rowCounter, columnCounter] = "Poreska uprava/Steuerverwaltung: ";

            //sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            //sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            //sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            //sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.TaxAdministration;

            //sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            //columnCounter = leftSideMin;
            //rowCounter++;

            //sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            //sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            //sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            //sheet1.Cells[rowCounter, columnCounter] = "IBAN: ";

            //sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            //sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            //sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            //sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.IBAN;
            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Betriebs broj/Betriebsnummer: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.BetriebsNumber;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Registarski broj/Registrierungsnummer: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.CommercialNr;
            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Kontakt osoba/Kontaktperson: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.ContactPersonGer;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

            columnCounter = leftSideMin;
            rowCounter++;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Odbitak poreza OD/Freistellung VON: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.VatDeductionFrom?.ToString("dd.MM.yyyy");
            columnCounter += 4;

            sheet1.Cells[rowCounter, columnCounter].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Cells[rowCounter, columnCounter].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter] = "Odbitak poreza DO/Freistellung BIS: ";

            sheet1.Range[sheet1.Cells[rowCounter, columnCounter + 1], sheet1.Cells[rowCounter, columnCounter + 3]].Merge();
            sheet1.Cells[rowCounter, columnCounter + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
            sheet1.Cells[rowCounter, columnCounter + 1].Font.Size = 10;
            sheet1.Cells[rowCounter, columnCounter + 1] = businessPartner.VatDeductionFrom?.ToString("dd.MM.yyyy");

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion


            #region Telefoni

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "TELEFONI/TELEFONE";

            rowCounter++;

            //original var employeeProfessionItems = new EmployeeProfessionItemSQLiteRepository().GetEmployeeProfessionsByEmployee(MainWindow.CurrentCompanyId, employee.Identifier)?.EmployeeProfessionItems ?? new List<EmployeeProfessionItemViewModel>();
            var businessPartnerPhoneItems = new BusinessPartnerPhoneSQLiteRepository().GetBusinessPartnerPhonesByBusinessPartner(MainWindow.CurrentCompanyId, businessPartner.Identifier)?.BusinessPartnerPhones ?? new List<BusinessPartnerPhoneViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 3]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "KONTAKT OSOBA/Kontaktperson";
            sheet1.Cells[rowCounter, leftSideMin + 4] = "MOBILNI/Handynummer";
            sheet1.Cells[rowCounter, leftSideMin + 5] = "TELEFON/Telefonnummer";
            sheet1.Cells[rowCounter, leftSideMin + 6] = "FAX";
            sheet1.Cells[rowCounter, leftSideMin + 7] = "EMAIL";
            sheet1.Cells[rowCounter, leftSideMin + 8] = "ROĐENDAN/Geburtstag";
            sheet1.Cells[rowCounter, leftSideMin + 9] = "NAPOMENA/Anmerkung";
            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            int columnCounterItem = leftSideMin;

            for (int i = 0; i < businessPartnerPhoneItems?.Count; i++)
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
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerPhoneItems[i].ContactPersonFirstName + " " + businessPartnerPhoneItems[i].ContactPersonLastName;

                columnCounterItem += 3;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerPhoneItems[i].Mobile;
                columnCounterItem += 1;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerPhoneItems[i].Phone;
                columnCounterItem += 1;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerPhoneItems[i].Fax;
                columnCounterItem += 1;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerPhoneItems[i].Email;
                columnCounterItem += 1;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerPhoneItems[i].Birthday;
                columnCounterItem += 1;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerPhoneItems[i].Description;
                columnCounterItem += 1;

                sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

                rowCounter++;
            }

            rowCounter--;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion


            #region Lokacije

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "LOKACIJE/Lokationen";

            rowCounter++;

            var businessPartnerLocationItems = new BusinessPartnerLocationSQLiteRepository().GetBusinessPartnerLocationsByBusinessPartner(MainWindow.CurrentCompanyId, businessPartner.Identifier)?.BusinessPartnerLocations ?? new List<BusinessPartnerLocationViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 3]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "DRŽAVA/Land";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 4], sheet1.Cells[rowCounter, leftSideMin + 6]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 4] = "REGION/Region";
            sheet1.Cells[rowCounter, leftSideMin + 7] = "OPŠTINA/StadtGemeinde";
            sheet1.Cells[rowCounter, leftSideMin + 8] = "GRAD/Stadt";
            sheet1.Cells[rowCounter, leftSideMin + 9] = "ADRESA/Adresse";
            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            columnCounterItem = leftSideMin;

            for (int i = 0; i < businessPartnerLocationItems?.Count; i++)
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
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerLocationItems[i].Country?.Name;
                columnCounterItem += 3;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 2]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerLocationItems[i].Region?.Name;

                columnCounterItem += 3;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerLocationItems[i].Municipality?.Name;

                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerLocationItems[i].City?.Name;
                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerLocationItems[i].Address;
                columnCounterItem++;
                sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

                rowCounter++;
            }

            rowCounter--;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion

            #region Banke

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "BANKE/BANKEN";

            rowCounter++;

            var businessPartnerBankItems = new BusinessPartnerBankSQLiteRepository().GetBusinessPartnerBanksByBusinessPartner(MainWindow.CurrentCompanyId, businessPartner.Identifier)?.BusinessPartnerBanks ?? new List<BusinessPartnerBankViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 3]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "DRŽAVA/Land";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 4], sheet1.Cells[rowCounter, leftSideMin + 6]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 4] = "BANKA/Bank";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 7], sheet1.Cells[rowCounter, leftSideMin + 9]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 7] = "BROJ RAČUNA/Kontonummer";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            columnCounterItem = leftSideMin;

            for (int i = 0; i < businessPartnerBankItems?.Count; i++)
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
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerBankItems[i].Country?.Name;
                columnCounterItem += 3;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 2]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerBankItems[i].Bank?.Name;

                columnCounterItem += 3;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 2]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerBankItems[i].AccountNumber;

                columnCounterItem += 3;

                sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

                rowCounter++;
            }

            rowCounter--;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion

            #region Institucije

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "INSTITUCIJE/Institutionen";

            rowCounter++;

            var businessPartnerInstitutionItems = new BusinessPartnerInstitutionSQLiteRepository().GetBusinessPartnerInstitutionsByBusinessPartner(MainWindow.CurrentCompanyId, businessPartner.Identifier)?.BusinessPartnerInstitutions ?? new List<BusinessPartnerInstitutionViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 3]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "INSTITUCIJA/Institution";
            sheet1.Cells[rowCounter, leftSideMin + 4] = "KORISNIČKO IME/Benutzername";
            sheet1.Cells[rowCounter, leftSideMin + 5] = "LOZINKA/Passwort";
            sheet1.Cells[rowCounter, leftSideMin + 6] = "KONTAKT OSOBA/Kontaktperson";
            sheet1.Cells[rowCounter, leftSideMin + 7] = "TELEFON";
            sheet1.Cells[rowCounter, leftSideMin + 8] = "FAX";
            sheet1.Cells[rowCounter, leftSideMin + 9] = "E-MAIL";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            columnCounterItem = leftSideMin;

            for (int i = 0; i < businessPartnerInstitutionItems?.Count; i++)
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
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerInstitutionItems[i].Institution;
                columnCounterItem += 3;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerInstitutionItems[i].Username;
                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 6]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerInstitutionItems[i].Password;
                columnCounterItem++;


                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerInstitutionItems[i].ContactPerson;
                columnCounterItem++;
                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 6]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerInstitutionItems[i].Phone;
                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerInstitutionItems[i].Fax;
                columnCounterItem++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerInstitutionItems[i].Email;
                columnCounterItem++;

                sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

                rowCounter++;
            }

            rowCounter--;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion

            #region Tipovi poslovnog partnera

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "TIPOVI POSLOVNOG PARTNERA/Geschäftspartnertypen";

            rowCounter++;

            var businessPartnerTypeItems = new BusinessPartnerTypeSQLiteRepository().GetBusinessPartnerTypesByBusinessPartner(MainWindow.CurrentCompanyId, businessPartner.Identifier)?.BusinessPartnerTypes ?? new List<BusinessPartnerTypeViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 3]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "ŠIFRA/Code";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 4], sheet1.Cells[rowCounter, leftSideMin + 9]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 4] = "TIP POSLOVNOG PARTNERA/Geschäftspartnertyp";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            columnCounterItem = leftSideMin;

            for (int i = 0; i < businessPartnerTypeItems?.Count; i++)
            {
                columnCounterItem = leftSideMin;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = (i + 1) + ". ";
                columnCounterItem ++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 2]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerTypeItems[i].Code;
                columnCounterItem += 3;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 7]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerTypeItems[i].Name;

                columnCounterItem += 6;

                sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

                rowCounter++;
            }

            rowCounter--;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion

            #region Dokumenti

            rowCounter++;
            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 12;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Merge();

            sheet1.Cells[rowCounter, leftSideMin] = "DOKUMENTI";

            rowCounter++;

            var businessPartnerDocumentItems = new BusinessPartnerDocumentSQLiteRepository().GetBusinessPartnerDocumentsByBusinessPartner(MainWindow.CurrentCompanyId, businessPartner.Identifier)?.BusinessPartnerDocuments ?? new List<BusinessPartnerDocumentViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 3]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "NAZIV DOKUMENTA/Dokumenttitel";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 4], sheet1.Cells[rowCounter, leftSideMin + 6]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 4] = "DATUM UNOSA/Eintragsdatum";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 7], sheet1.Cells[rowCounter, leftSideMin + 9]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 7] = "PUTANJA/Pfade";


            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            columnCounterItem = leftSideMin;

            for (int i = 0; i < businessPartnerDocumentItems?.Count; i++)
            {
                columnCounterItem = leftSideMin;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = (i + 1) + ". ";
                columnCounterItem ++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 2]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerDocumentItems[i].Name;
                columnCounterItem += 3;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 2]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerDocumentItems[i].CreateDate;

                columnCounterItem += 3;
                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 2]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerDocumentItems[i].Path;
                columnCounterItem += 3;

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

            sheet1.Cells[rowCounter, leftSideMin] = "NAPOMENE/Anmerkungen";

            rowCounter++;

            var businessPartnerNoteItems = new BusinessPartnerNoteSQLiteRepository().GetBusinessPartnerNotesByBusinessPartner(MainWindow.CurrentCompanyId, businessPartner.Identifier)?.BusinessPartnerNotes ?? new List<BusinessPartnerNoteViewModel>();

            rowCounter++;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Interior.Color = XlRgbColor.rgbLightGray;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].VerticalAlignment = XlVAlign.xlVAlignTop;

            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Bold = true;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Font.Size = 8;

            sheet1.Cells[rowCounter, leftSideMin] = "RB.";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 1], sheet1.Cells[rowCounter, leftSideMin + 7]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 1] = "NAPOMENA/Anmerkung";
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin + 8], sheet1.Cells[rowCounter, leftSideMin + 9]].Merge();
            sheet1.Cells[rowCounter, leftSideMin + 8] = "DATUM NAPOMENE/Datum der Anmerkung";

            rowCounter++;
            //rowCounter++; 

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            columnCounterItem = leftSideMin;

            for (int i = 0; i < businessPartnerNoteItems?.Count; i++)
            {
                columnCounterItem = leftSideMin;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Cells[rowCounter, columnCounterItem] = (i + 1) + ". ";
                columnCounterItem ++;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 6]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerNoteItems[i].Note;
                columnCounterItem += 7;

                sheet1.Cells[rowCounter, columnCounterItem].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].VerticalAlignment = XlVAlign.xlVAlignCenter;
                sheet1.Cells[rowCounter, columnCounterItem].Font.Size = 10;
                sheet1.Range[sheet1.Cells[rowCounter, columnCounterItem], sheet1.Cells[rowCounter, columnCounterItem + 1]].Merge();
                sheet1.Cells[rowCounter, columnCounterItem] = businessPartnerNoteItems[i].NoteDate;
                columnCounterItem += 2;

                sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDash;

                rowCounter++;
            }

            rowCounter--;

            // line
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet1.Range[sheet1.Cells[rowCounter, leftSideMin], sheet1.Cells[rowCounter, rightSideMax]].Borders[XlBordersIndex.xlEdgeBottom].Weight = 2d;

            #endregion

            sheet1.Columns.AutoFit();

        }
    }
}
