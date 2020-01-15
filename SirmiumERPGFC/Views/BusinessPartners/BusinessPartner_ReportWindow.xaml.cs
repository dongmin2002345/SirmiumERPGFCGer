using MahApps.Metro.Controls;
using Microsoft.Reporting.WinForms;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.RdlcReports.BusinessPartners;
using SirmiumERPGFC.Repository.BusinessPartners;
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

namespace SirmiumERPGFC.Views.BusinessPartners
{
    /// <summary>
    /// Interaction logic for BusinessPartner_ReportWindow.xaml
    /// </summary>
    public partial class BusinessPartner_ReportWindow : MetroWindow, INotifyPropertyChanged
    {

        #region BusinessPartnerSearchObject
        private BusinessPartnerViewModel _BusinessPartnerSearchObject = new BusinessPartnerViewModel();

        public BusinessPartnerViewModel BusinessPartnerSearchObject
        {
            get { return _BusinessPartnerSearchObject; }
            set
            {
                if (_BusinessPartnerSearchObject != value)
                {
                    _BusinessPartnerSearchObject = value;
                    NotifyPropertyChanged("BusinessPartnerSearchObject");
                }
            }
        }
        #endregion
        public BusinessPartner_ReportWindow(BusinessPartnerViewModel businessPartnerView)
        {
            InitializeComponent();

            string path = "";
            try
            {
                rdlcBusinessPartnerReport.LocalReport.DataSources.Clear();

                List<BusinessPartnersReportViewModel> businessPartner = new List<BusinessPartnersReportViewModel>();
                List<BusinessPartnerViewModel> BusinessPartnerItems = new BusinessPartnerSQLiteRepository().GetBusinessPartnersByPage(MainWindow.CurrentCompanyId, BusinessPartnerSearchObject, 1, 50).BusinessPartners;
                int counter = 1;
                foreach (var BusinessPartnerItem in BusinessPartnerItems)
                {
                    businessPartner.Add(new BusinessPartnersReportViewModel()
                    {
                        OrderNumbersForBusinessPartners = counter++,
                        InternalCode = BusinessPartnerItem?.InternalCode ?? "",
                        Name = BusinessPartnerItem?.Name ?? "",
                        NameGer = BusinessPartnerItem?.NameGer ?? "",
                        TaxNr = BusinessPartnerItem?.TaxNr ?? "",
                        Valuta = BusinessPartnerItem?.DueDate.ToString("#.00") ?? "",
                        VatDescription = BusinessPartnerItem?.Vat?.Amount.ToString() ?? "",
                        PIO = BusinessPartnerItem?.PIO ?? "",
                        Customer = BusinessPartnerItem?.Customer ?? "",
                        DiscountName = BusinessPartnerItem?.Discount?.Amount.ToString("#.00") ?? "",
                        IsInPDV = BusinessPartnerItem?.IsInPDV.ToString() ?? ""
                    });
                }
                var rpdsModel = new ReportDataSource()
                {
                    Name = "DataSet1",
                    Value = businessPartner
                };
                rdlcBusinessPartnerReport.LocalReport.DataSources.Add(rpdsModel);

                //List<ReportParameter> reportParams = new List<ReportParameter>();
                //string parameterText = "Dana " + (CurrentInputInvoice?.InvoiceDate.ToString("dd.MM.yyyy") ?? "") + " na stočni depo klanice Bioesen primljeno je:";
                //reportParams.Add(new ReportParameter("txtInputInvoiceDate", parameterText));


                //var businessPartnerList = new List<InvoiceBusinessPartnerViewModel>();
                //businessPartnerList.Add(new InvoiceBusinessPartnerViewModel() { Name = "Pera peric " });
                //var businessPartnerModel = new ReportDataSource() { Name = "DataSet2", Value = businessPartnerList };
                //rdlcInputNoteReport.LocalReport.DataSources.Add(businessPartnerModel);


                string exeFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string ContentStart = System.IO.Path.Combine(exeFolder, @"RdlcReports\BusinessPartners\BusinessPartnersReport.rdlc");
                rdlcBusinessPartnerReport.LocalReport.ReportPath = ContentStart;
                // rdlcBusinessPartnerReport.LocalReport.SetParameters(reportParams);
                rdlcBusinessPartnerReport.SetDisplayMode(DisplayMode.PrintLayout);
                rdlcBusinessPartnerReport.Refresh();
                rdlcBusinessPartnerReport.ZoomMode = ZoomMode.Percent;
                rdlcBusinessPartnerReport.ZoomPercent = 100;
                rdlcBusinessPartnerReport.RefreshReport();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
