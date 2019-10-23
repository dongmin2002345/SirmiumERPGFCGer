using MahApps.Metro.Controls;
using Microsoft.Reporting.WinForms;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.RdlcReports.ConstructionSites;
using SirmiumERPGFC.Repository.ConstructionSites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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


namespace SirmiumERPGFC.Views.ConstructionSites
{
    /// <summary>
    /// Interaction logic for ConstructionSite_ReportWindow.xaml
    /// </summary>
    public partial class ConstructionSite_ReportWindow : MetroWindow, INotifyPropertyChanged
    {
        #region ConstructionSiteSearchObject
        private ConstructionSiteViewModel _ConstructionSiteSearchObject = new ConstructionSiteViewModel();

        public ConstructionSiteViewModel ConstructionSiteSearchObject
        {
            get { return _ConstructionSiteSearchObject; }
            set
            {
                if (_ConstructionSiteSearchObject != value)
                {
                    _ConstructionSiteSearchObject = value;
                    NotifyPropertyChanged("ConstructionSiteSearchObject");
                }
            }
        }
        #endregion
        public ConstructionSite_ReportWindow(ConstructionSiteViewModel constructionSiteView)
        {
            InitializeComponent();
            rdlcConstructionSiteReport.LocalReport.DataSources.Clear();

            List<ConstructionSitesReportViewModel> constructionSites = new List<ConstructionSitesReportViewModel>();
            List<ConstructionSiteViewModel> ConstructionSiteItems = new ConstructionSiteSQLiteRepository().GetConstructionSitesByPage(MainWindow.CurrentCompanyId, ConstructionSiteSearchObject, 1, 50).ConstructionSites;
            int counter = 1;
            foreach (var ConstructionSiteItem in ConstructionSiteItems)
            {
                constructionSites.Add(new ConstructionSitesReportViewModel()
                {
                    OrderNumbersForConstructionSites = counter++,
                    ConstructionSiteCode = ConstructionSiteItem?.Code ?? "",
                    InternalCode = ConstructionSiteItem?.InternalCode ?? "",
                    Name = ConstructionSiteItem?.Name ?? "",
                    CityName = ConstructionSiteItem?.City?.Name ?? "",
                    CountryName = ConstructionSiteItem?.Country?.Name ?? "",
                    BusinessPartnerName = ConstructionSiteItem?.BusinessPartner?.Name ?? "",
                    StatusName = ConstructionSiteItem?.Status?.Name ?? "",
                    Address = ConstructionSiteItem?.Address ?? "",
                    MaxWorkers = ConstructionSiteItem?.MaxWorkers.ToString() ?? "",
                    ProContractDate = ConstructionSiteItem?.ProContractDate.ToString("dd.MM.yyyy") ?? "",
                    ContractStart = ConstructionSiteItem?.ContractStart.ToString("dd.MM.yyyy") ?? "",
                    ContractExpiration = ConstructionSiteItem?.ContractExpiration.ToString("dd.MM.yyyy") ?? "",
                });
            }
            var rpdsModel = new ReportDataSource()
            {
                Name = "DataSet1",
                Value = constructionSites
            };
            rdlcConstructionSiteReport.LocalReport.DataSources.Add(rpdsModel);

            //List<ReportParameter> reportParams = new List<ReportParameter>();
            //string parameterText = "Dana " + (CurrentConstructionSite?.InvoiceDate.ToString("dd.MM.yyyy") ?? "") + " na stočni depo klanice Bioesen primljeno je:";
            //reportParams.Add(new ReportParameter("txtConstructionSiteDate", parameterText));


            //var businessPartnerList = new List<InvoiceBusinessPartnerViewModel>();
            //businessPartnerList.Add(new InvoiceBusinessPartnerViewModel() { Name = "Pera peric " });
            //var businessPartnerModel = new ReportDataSource() { Name = "DataSet2", Value = businessPartnerList };
            //rdlcOutputNoteReport.LocalReport.DataSources.Add(businessPartnerModel);

            string exeFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            string ContentStart = System.IO.Path.Combine(exeFolder, @"SirmiumERPGFC\RdlcReports\ConstructionSites\ConstructionSitesReport.rdlc");

            rdlcConstructionSiteReport.LocalReport.ReportPath = ContentStart;
            // rdlcConstructionSiteReport.LocalReport.SetParameters(reportParams);
            rdlcConstructionSiteReport.SetDisplayMode(DisplayMode.PrintLayout);
            rdlcConstructionSiteReport.Refresh();
            rdlcConstructionSiteReport.ZoomMode = ZoomMode.Percent;
            rdlcConstructionSiteReport.ZoomPercent = 100;
            rdlcConstructionSiteReport.RefreshReport();

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

