using MahApps.Metro.Controls;
using Microsoft.Reporting.WinForms;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.RdlcReports.Employees;
using SirmiumERPGFC.Repository.Employees;
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

namespace SirmiumERPGFC.Views.Employees
{
    /// <summary>
    /// Interaction logic for PhysicalPerson_ReportWindow.xaml
    /// </summary>
    public partial class PhysicalPerson_ReportWindow : MetroWindow, INotifyPropertyChanged
    {

        #region PhysicalPersonSearchObject
        private PhysicalPersonViewModel _PhysicalPersonSearchObject = new PhysicalPersonViewModel();

        public PhysicalPersonViewModel PhysicalPersonSearchObject
        {
            get { return _PhysicalPersonSearchObject; }
            set
            {
                if (_PhysicalPersonSearchObject != value)
                {
                    _PhysicalPersonSearchObject = value;
                    NotifyPropertyChanged("PhysicalPersonSearchObject");
                }
            }
        }
        #endregion
        public PhysicalPerson_ReportWindow(PhysicalPersonViewModel physicalPersonView)
        {
            InitializeComponent();

            rdlcPhysicalPersonReport.LocalReport.DataSources.Clear();

            List<PhysicalPersonsReportViewModel> PhysicalPerson = new List<PhysicalPersonsReportViewModel>();
            List<PhysicalPersonViewModel> PhysicalPersonItems = new PhysicalPersonSQLiteRepository().GetPhysicalPersonsByPage(MainWindow.CurrentCompanyId, PhysicalPersonSearchObject, 1, 50).PhysicalPersons;
            int counter = 1;
            foreach (var PhysicalPersonItem in PhysicalPersonItems)
            {

                PhysicalPerson.Add(new PhysicalPersonsReportViewModel()
                {


                    OrderNumbersForPhysicalPersons = counter++,
                    PhysicalPersonCode = PhysicalPersonItem?.PhysicalPersonCode ?? "",
                    Name = PhysicalPersonItem?.Name ?? "",
                    SurName = PhysicalPersonItem?.SurName ?? "",
                    ConstructionSiteCode = PhysicalPersonItem?.ConstructionSiteCode ?? "",
                    ConstructionSiteName = PhysicalPersonItem?.ConstructionSiteName ?? "",
                    DateOfBirth = PhysicalPersonItem?.DateOfBirth?.ToString("dd.MM.yyyy") ?? "",
                    Passport = PhysicalPersonItem?.Passport ?? "",
                    ResidenceCountryName = PhysicalPersonItem?.ResidenceCountry?.Name ?? "",
                    ResidenceCityName = PhysicalPersonItem?.ResidenceCity?.Name ?? "",
                    ResidenceAddress = PhysicalPersonItem?.ResidenceAddress ?? ""


                });
            }
            var rpdsModel = new ReportDataSource()
            {
                Name = "DataSet1",
                Value = PhysicalPerson
            };
            rdlcPhysicalPersonReport.LocalReport.DataSources.Add(rpdsModel);

            //List<ReportParameter> reportParams = new List<ReportParameter>();
            //string parameterText = "Dana " + (CurrentEmployee?.InvoiceDate.ToString("dd.MM.yyyy") ?? "") + " na stočni depo klanice Bioesen primljeno je:";
            //reportParams.Add(new ReportParameter("txtEmployeeDate", parameterText));


            //var businessPartnerList = new List<InvoiceBusinessPartnerViewModel>();
            //businessPartnerList.Add(new InvoiceBusinessPartnerViewModel() { Name = "Pera peric " });
            //var businessPartnerModel = new ReportDataSource() { Name = "DataSet2", Value = businessPartnerList };
            //rdlcInputNoteReport.LocalReport.DataSources.Add(businessPartnerModel);

            string exeFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            string ContentStart = System.IO.Path.Combine(exeFolder, @"SirmiumERPGFC\RdlcReports\Employees\PhysicalsReport.rdlc");

            rdlcPhysicalPersonReport.LocalReport.ReportPath = ContentStart;
            // rdlcPhysicalPersonReport.LocalReport.SetParameters(reportParams);
            rdlcPhysicalPersonReport.SetDisplayMode(DisplayMode.PrintLayout);
            rdlcPhysicalPersonReport.Refresh();
            rdlcPhysicalPersonReport.ZoomMode = ZoomMode.Percent;
            rdlcPhysicalPersonReport.ZoomPercent = 100;
            rdlcPhysicalPersonReport.RefreshReport();

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
