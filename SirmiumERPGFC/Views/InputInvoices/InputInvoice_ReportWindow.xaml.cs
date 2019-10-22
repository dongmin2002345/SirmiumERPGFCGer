using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Reporting.WinForms;
using SirmiumERPGFC.RdlcReports.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using SirmiumERPGFC.Repository.InputInvoices;
using System.ComponentModel;

namespace SirmiumERPGFC.Views.InputInvoices
{
    /// <summary>
    /// Interaction logic for InputInvoice_ReportWindow.xaml
    /// </summary>
    public partial class InputInvoice_ReportWindow : MetroWindow, INotifyPropertyChanged
    {

        #region InputInvoiceSearchObject
        private InputInvoiceViewModel _InputInvoiceSearchObject = new InputInvoiceViewModel();

        public InputInvoiceViewModel InputInvoiceSearchObject
        {
            get { return _InputInvoiceSearchObject; }
            set
            {
                if (_InputInvoiceSearchObject != value)
                {
                    _InputInvoiceSearchObject = value;
                    NotifyPropertyChanged("InputInvoiceSearchObject");
                }
            }
        }
        #endregion
        public InputInvoice_ReportWindow(InputInvoiceViewModel inputInvoiceView)
        {
            InitializeComponent();

            rdlcInputInvoiceReport.LocalReport.DataSources.Clear();

            List<InputInvoicesReportsViewModel> inputInvoice = new List<InputInvoicesReportsViewModel>();
            List<InputInvoiceViewModel> InputInvoiceItems = new InputInvoiceSQLiteRepository().GetInputInvoicesByPage(MainWindow.CurrentCompanyId, InputInvoiceSearchObject, 1, 50).InputInvoices;
            int counter = 1;
            foreach (var InputInvoiceItem in InputInvoiceItems)
            {
                inputInvoice.Add(new InputInvoicesReportsViewModel()
                {
                    OrderNumbersForInputInvoices = counter++,
                    BusinessPartnerName = InputInvoiceItem?.BusinessPartner?.Name ?? "",
                    Supplier = InputInvoiceItem?.Supplier ?? "",
                    Address = InputInvoiceItem?.Address ?? "",
                    InvoiceNumber = InputInvoiceItem?.InvoiceNumber ?? "",
                    InvoiceDate = InputInvoiceItem?.InvoiceDate.ToString("dd.MM.yyyy") ?? "",
                    AmountNet = InputInvoiceItem?.AmountNet.ToString("#.00") ?? "",
                    PDVPercent = InputInvoiceItem?.PDVPercent.ToString("#.00") ?? "",
                    PDV = InputInvoiceItem?.PDV.ToString() ?? "",
                    AmountGros = InputInvoiceItem?.AmountGross.ToString("#.00") ?? "",
                    Currency = InputInvoiceItem?.Currency.ToString("#.00") ?? "",
                    DateOfPaymetse = InputInvoiceItem?.DateOfPaymet.ToString("dd.MM.yyyy") ?? "",
                    Statuse = InputInvoiceItem?.Status.ToString() ?? "",
                    StatusDate = InputInvoiceItem?.StatusDate.ToString("dd.MM.yyyy") ?? ""

                });
            }
            var rpdsModel = new ReportDataSource()
            {
                Name = "DataSet1",
                Value = inputInvoice
            };
            rdlcInputInvoiceReport.LocalReport.DataSources.Add(rpdsModel);

            //List<ReportParameter> reportParams = new List<ReportParameter>();
            //string parameterText = "Dana " + (CurrentInputInvoice?.InvoiceDate.ToString("dd.MM.yyyy") ?? "") + " na stočni depo klanice Bioesen primljeno je:";
            //reportParams.Add(new ReportParameter("txtInputInvoiceDate", parameterText));


            //var businessPartnerList = new List<InvoiceBusinessPartnerViewModel>();
            //businessPartnerList.Add(new InvoiceBusinessPartnerViewModel() { Name = "Pera peric " });
            //var businessPartnerModel = new ReportDataSource() { Name = "DataSet2", Value = businessPartnerList };
            //rdlcInputNoteReport.LocalReport.DataSources.Add(businessPartnerModel);

            string exeFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            string ContentStart = System.IO.Path.Combine(exeFolder, @"SirmiumERPGFC\RdlcReports\InputInvoices\InputInvoicesReport.rdlc");

            rdlcInputInvoiceReport.LocalReport.ReportPath = ContentStart;
            // rdlcInputInvoiceReport.LocalReport.SetParameters(reportParams);
            rdlcInputInvoiceReport.SetDisplayMode(DisplayMode.PrintLayout);
            rdlcInputInvoiceReport.Refresh();
            rdlcInputInvoiceReport.ZoomMode = ZoomMode.Percent;
            rdlcInputInvoiceReport.ZoomPercent = 100;
            rdlcInputInvoiceReport.RefreshReport();

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
