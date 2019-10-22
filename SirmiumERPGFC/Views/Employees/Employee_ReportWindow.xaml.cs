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
    /// Interaction logic for Employee_ReportWindow.xaml
    /// </summary>
    public partial class Employee_ReportWindow : MetroWindow, INotifyPropertyChanged
    {

        #region EmployeeSearchObject
        private EmployeeViewModel _EmployeeSearchObject = new EmployeeViewModel();

        public EmployeeViewModel EmployeeSearchObject
        {
            get { return _EmployeeSearchObject; }
            set
            {
                if (_EmployeeSearchObject != value)
                {
                    _EmployeeSearchObject = value;
                    NotifyPropertyChanged("EmployeeSearchObject");
                }
            }
        }
        #endregion
        public Employee_ReportWindow(EmployeeViewModel employeeView)
        {
            InitializeComponent();

            rdlcEmployeeReport.LocalReport.DataSources.Clear();

            List<EmployeesReportViewModel> employee = new List<EmployeesReportViewModel>();
            List<EmployeeViewModel> employeeItems = new EmployeeSQLiteRepository().GetEmployeesByPage(MainWindow.CurrentCompanyId, EmployeeSearchObject, 1, 50).Employees;
            int counter = 1;
            foreach (var employeeItem in employeeItems)
            {

                employee.Add(new EmployeesReportViewModel()
                {
                
                    
                    OrderNumbersForEmployees = counter++,
                    EmployeeCode = employeeItem?.EmployeeCode ?? "",
                    Name = employeeItem?.Name ?? "",
                    SurName = employeeItem?.SurName ?? "",
                    ConstructionSiteCode = employeeItem?.ConstructionSiteCode ?? "",
                    ConstructionSiteName = employeeItem?.ConstructionSiteName ?? "",
                    DateOfBirth = employeeItem?.DateOfBirth?.ToString("dd.MM.yyyy") ?? "",
                    Passport = employeeItem?.Passport ?? "",
                    ResidenceCountryName = employeeItem?.ResidenceCountry?.Name ?? "",
                    ResidenceCityName = employeeItem?.ResidenceCity?.Name ?? "",
                    ResidenceAddress = employeeItem?.ResidenceAddress ?? ""
                    

                });
            }
            var rpdsModel = new ReportDataSource()
            {
                Name = "DataSet1",
                Value = employee
            };
            rdlcEmployeeReport.LocalReport.DataSources.Add(rpdsModel);

            //List<ReportParameter> reportParams = new List<ReportParameter>();
            //string parameterText = "Dana " + (CurrentEmployee?.InvoiceDate.ToString("dd.MM.yyyy") ?? "") + " na stočni depo klanice Bioesen primljeno je:";
            //reportParams.Add(new ReportParameter("txtEmployeeDate", parameterText));


            //var businessPartnerList = new List<InvoiceBusinessPartnerViewModel>();
            //businessPartnerList.Add(new InvoiceBusinessPartnerViewModel() { Name = "Pera peric " });
            //var businessPartnerModel = new ReportDataSource() { Name = "DataSet2", Value = businessPartnerList };
            //rdlcInputNoteReport.LocalReport.DataSources.Add(businessPartnerModel);

            string exeFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            string ContentStart = System.IO.Path.Combine(exeFolder, @"SirmiumERPGFC\RdlcReports\Employees\EmployeesReport.rdlc");

            rdlcEmployeeReport.LocalReport.ReportPath = ContentStart;
            // rdlcEmployeeReport.LocalReport.SetParameters(reportParams);
            rdlcEmployeeReport.SetDisplayMode(DisplayMode.PrintLayout);
            rdlcEmployeeReport.Refresh();
            rdlcEmployeeReport.ZoomMode = ZoomMode.Percent;
            rdlcEmployeeReport.ZoomPercent = 100;
            rdlcEmployeeReport.RefreshReport();

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
