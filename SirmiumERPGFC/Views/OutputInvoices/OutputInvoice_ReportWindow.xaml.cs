using MahApps.Metro.Controls;
using Microsoft.Reporting.WinForms;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using SirmiumERPGFC.RdlcReports.OutputInvoices;
using SirmiumERPGFC.Repository.OutputInvoices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace SirmiumERPGFC.Views.OutputInvoices
{
    /// <summary>
    /// Interaction logic for OutputInvoice_ReportWindow.xaml
    /// </summary>
    public partial class OutputInvoice_ReportWindow : MetroWindow, INotifyPropertyChanged
    {
        #region OutputInvoiceSearchObject
        private OutputInvoiceViewModel _OutputInvoiceSearchObject = new OutputInvoiceViewModel();

        public OutputInvoiceViewModel OutputInvoiceSearchObject
        {
            get { return _OutputInvoiceSearchObject; }
            set
            {
                if (_OutputInvoiceSearchObject != value)
                {
                    _OutputInvoiceSearchObject = value;
                    NotifyPropertyChanged("OutputInvoiceSearchObject");
                }
            }
        }
        #endregion
        public OutputInvoice_ReportWindow(OutputInvoiceViewModel outputInvoiceView)
        {
            InitializeComponent();
            rdlcOutputInvoiceReport.LocalReport.DataSources.Clear();

            List<OutputInvoicesReportViewModel> outputInvoice = new List<OutputInvoicesReportViewModel>();
            List<OutputInvoiceViewModel> OutputInvoiceItems = new OutputInvoiceSQLiteRepository().GetOutputInvoicesByPage(MainWindow.CurrentCompanyId, OutputInvoiceSearchObject, 1, 50).OutputInvoices;
            int counter = 1;
            foreach (var OutputInvoiceItem in OutputInvoiceItems)
            {
                outputInvoice.Add(new OutputInvoicesReportViewModel()
                {
                    OrderNumbersForOutputInvoices = counter++,
                    BusinessPartnerName = OutputInvoiceItem?.BusinessPartner?.Name ?? "",
                    Supplier = OutputInvoiceItem?.Supplier ?? "",
                    Address = OutputInvoiceItem?.Address ?? "",
                    InvoiceNumber = OutputInvoiceItem?.InvoiceNumber ?? "",
                    InvoiceDate = OutputInvoiceItem?.InvoiceDate.ToString("dd.MM.yyyy") ?? "",
                    AmountNet = OutputInvoiceItem?.AmountNet.ToString("#.00") ?? "",
                    PDVPercent = OutputInvoiceItem?.PdvPercent.ToString("#.00") ?? "",
                    PDV = OutputInvoiceItem?.Pdv.ToString() ?? "",
                    AmountGros = OutputInvoiceItem?.AmountGross.ToString("#.00") ?? "",
                    Currency = OutputInvoiceItem?.Currency.ToString("#.00") ?? "",
                    DateOfPaymetse = OutputInvoiceItem?.DateOfPayment.ToString("dd.MM.yyyy") ?? "",
                    Statuse = OutputInvoiceItem?.Status.ToString() ?? "",
                    StatusDate = OutputInvoiceItem?.StatusDate.ToString("dd.MM.yyyy") ?? ""

                });
            }
            var rpdsModel = new ReportDataSource()
            {
                Name = "DataSet1",
                Value = outputInvoice
            };
            rdlcOutputInvoiceReport.LocalReport.DataSources.Add(rpdsModel);

            //List<ReportParameter> reportParams = new List<ReportParameter>();
            //string parameterText = "Dana " + (CurrentOutputInvoice?.InvoiceDate.ToString("dd.MM.yyyy") ?? "") + " na stočni depo klanice Bioesen primljeno je:";
            //reportParams.Add(new ReportParameter("txtOutputInvoiceDate", parameterText));


            //var businessPartnerList = new List<InvoiceBusinessPartnerViewModel>();
            //businessPartnerList.Add(new InvoiceBusinessPartnerViewModel() { Name = "Pera peric " });
            //var businessPartnerModel = new ReportDataSource() { Name = "DataSet2", Value = businessPartnerList };
            //rdlcOutputNoteReport.LocalReport.DataSources.Add(businessPartnerModel);

            //////string exeFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            //////string ContentStart = System.IO.Path.Combine(exeFolder, @"SirmiumERPGFC\RdlcReports\OutputInvoices\OutputInvoicesReport.rdlc");

            //////rdlcOutputInvoiceReport.LocalReport.ReportPath = ContentStart;
            ///

            string exeFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string ContentStart = System.IO.Path.Combine(exeFolder, @"RdlcReports\OutputInvoices\OutputInvoicesReport.rdlc");
            rdlcOutputInvoiceReport.LocalReport.ReportPath = ContentStart;
            // rdlcOutputInvoiceReport.LocalReport.SetParameters(reportParams);
            rdlcOutputInvoiceReport.SetDisplayMode(DisplayMode.PrintLayout);
            rdlcOutputInvoiceReport.Refresh();
            rdlcOutputInvoiceReport.ZoomMode = ZoomMode.Percent;
            rdlcOutputInvoiceReport.ZoomPercent = 100;
            rdlcOutputInvoiceReport.RefreshReport();

        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        public void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
