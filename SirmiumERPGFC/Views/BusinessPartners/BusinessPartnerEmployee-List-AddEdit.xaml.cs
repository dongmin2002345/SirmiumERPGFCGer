using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
using SirmiumERPGFC.Repository.Employees;
using SirmiumERPGFC.Views.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SirmiumERPGFC.Views.BusinessPartners
{
    public partial class BusinessPartnerEmployee_List_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Atributes

        #region Services
        IEmployeeByBusinessPartnerService employeeByBusinessPartnerService;
        IEmployeeByBusinessPartnerHistoryService employeeByBusinessPartnerHistoryService;
        IEmployeeService employeeService;
        IBusinessPartnerService businessPartnerService;
        #endregion

        #region CurrentBusinessPartner
        private BusinessPartnerViewModel _CurrentBusinessPartner;

        public BusinessPartnerViewModel CurrentBusinessPartner
        {
            get { return _CurrentBusinessPartner; }
            set
            {
                if (_CurrentBusinessPartner != value)
                {
                    _CurrentBusinessPartner = value;
                    NotifyPropertyChanged("CurrentBusinessPartner");
                }
            }
        }
        #endregion

        #region ContractStartDate
        private DateTime _ContractStartDate = DateTime.Now;

        public DateTime ContractStartDate
        {
            get { return _ContractStartDate; }
            set
            {
                if (_ContractStartDate != value)
                {
                    _ContractStartDate = value;
                    NotifyPropertyChanged("ContractStartDate");
                }
            }
        }
        #endregion

        #region ContractEndDate
        private DateTime _ContractEndDate = DateTime.Now;

        public DateTime ContractEndDate
        {
            get { return _ContractEndDate; }
            set
            {
                if (_ContractEndDate != value)
                {
                    _ContractEndDate = value;
                    NotifyPropertyChanged("ContractEndDate");
                }
            }
        }
        #endregion


        #region EmployeesNotOnBusinessPartnerFromDB
        private ObservableCollection<EmployeeViewModel> _EmployeesNotOnBusinessPartnerFromDB;

        public ObservableCollection<EmployeeViewModel> EmployeesNotOnBusinessPartnerFromDB
        {
            get { return _EmployeesNotOnBusinessPartnerFromDB; }
            set
            {
                if (_EmployeesNotOnBusinessPartnerFromDB != value)
                {
                    _EmployeesNotOnBusinessPartnerFromDB = value;
                    NotifyPropertyChanged("EmployeesNotOnBusinessPartnerFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeNotOnBusinessPartner
        private EmployeeViewModel _CurrentEmployeeNotOnBusinessPartner;

        public EmployeeViewModel CurrentEmployeeNotOnBusinessPartner
        {
            get { return _CurrentEmployeeNotOnBusinessPartner; }
            set
            {
                if (_CurrentEmployeeNotOnBusinessPartner != value)
                {
                    _CurrentEmployeeNotOnBusinessPartner = value;
                    NotifyPropertyChanged("CurrentEmployeeNotOnBusinessPartner");
                }
            }
        }
        #endregion

        #region EmployeeNotOnBusinessPartnerSearchObject
        private EmployeeViewModel _EmployeeNotOnBusinessPartnerSearchObject = new EmployeeViewModel();

        public EmployeeViewModel EmployeeNotOnBusinessPartnerSearchObject
        {
            get { return _EmployeeNotOnBusinessPartnerSearchObject; }
            set
            {
                if (_EmployeeNotOnBusinessPartnerSearchObject != value)
                {
                    _EmployeeNotOnBusinessPartnerSearchObject = value;
                    NotifyPropertyChanged("EmployeeNotOnBusinessPartnerSearchObject");
                }
            }
        }
        #endregion

        #region EmployeeNotOnBusinessPartnerDataLoading
        private bool _EmployeeNotOnBusinessPartnerDataLoading;

        public bool EmployeeNotOnBusinessPartnerDataLoading
        {
            get { return _EmployeeNotOnBusinessPartnerDataLoading; }
            set
            {
                if (_EmployeeNotOnBusinessPartnerDataLoading != value)
                {
                    _EmployeeNotOnBusinessPartnerDataLoading = value;
                    NotifyPropertyChanged("EmployeeNotOnBusinessPartner" +
                        "DataLoading");
                }
            }
        }
        #endregion


        #region EmployeesOnBusinessPartnereFromDB
        private ObservableCollection<EmployeeByBusinessPartnerViewModel> _EmployeesOnBusinessPartnerFromDB;

        public ObservableCollection<EmployeeByBusinessPartnerViewModel> EmployeesOnBusinessPartnerFromDB
        {
            get { return _EmployeesOnBusinessPartnerFromDB; }
            set
            {
                if (_EmployeesOnBusinessPartnerFromDB != value)
                {
                    _EmployeesOnBusinessPartnerFromDB = value;
                    NotifyPropertyChanged("EmployeesOnBusinessPartnerFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeOnBusinessPartner
        private EmployeeByBusinessPartnerViewModel _CurrentEmployeeOnBusinessPartner;

        public EmployeeByBusinessPartnerViewModel CurrentEmployeeOnBusinessPartner
        {
            get { return _CurrentEmployeeOnBusinessPartner; }
            set
            {
                if (_CurrentEmployeeOnBusinessPartner != value)
                {
                    _CurrentEmployeeOnBusinessPartner = value;
                    NotifyPropertyChanged("CurrentEmployeeOnBusinessPartner");
                }
            }
        }
        #endregion

        #region EmployeeOnBusinessPartnerSearchObject
        private EmployeeViewModel _EmployeeOnBusinessPartnerSearchObject = new EmployeeViewModel();

        public EmployeeViewModel EmployeeOnBusinessPartnerSearchObject
        {
            get { return _EmployeeOnBusinessPartnerSearchObject; }
            set
            {
                if (_EmployeeOnBusinessPartnerSearchObject != value)
                {
                    _EmployeeOnBusinessPartnerSearchObject = value;
                    NotifyPropertyChanged("EmployeeOnBusinessPartnerSearchObject");
                }
            }
        }
        #endregion

        #region EmployeeOnBusinessPartnerDataLoading
        private bool _EmployeeOnBusinessPartnerDataLoading;

        public bool EmployeeOnBusinessPartnerDataLoading
        {
            get { return _EmployeeOnBusinessPartnerDataLoading; }
            set
            {
                if (_EmployeeOnBusinessPartnerDataLoading != value)
                {
                    _EmployeeOnBusinessPartnerDataLoading = value;
                    NotifyPropertyChanged("EmployeeOnBusinessPartnerDataLoading");
                }
            }
        }
        #endregion


        #region Pagination data

        int currentPage = 1;
        int itemsPerPage = 50;
        int totalItems = 0;

        #region PaginationDisplay
        private string _PaginationDisplay;

        public string PaginationDisplay
        {
            get { return _PaginationDisplay; }
            set
            {
                if (_PaginationDisplay != value)
                {
                    _PaginationDisplay = value;
                    NotifyPropertyChanged("PaginationDisplay");
                }
            }
        }
        #endregion

        #endregion


        #region SyncButtonContent
        private string _SyncButtonContent = " Sinhronizuj ";

        public string SyncButtonContent
        {
            get { return _SyncButtonContent; }
            set
            {
                if (_SyncButtonContent != value)
                {
                    _SyncButtonContent = value;
                    NotifyPropertyChanged("SyncButtonContent");
                }
            }
        }
        #endregion

        #region SyncButtonEnabled
        private bool _SyncButtonEnabled = true;

        public bool SyncButtonEnabled
        {
            get { return _SyncButtonEnabled; }
            set
            {
                if (_SyncButtonEnabled != value)
                {
                    _SyncButtonEnabled = value;
                    NotifyPropertyChanged("SyncButtonEnabled");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public BusinessPartnerEmployee_List_AddEdit(BusinessPartnerViewModel businessPartnerViewModel)
        {
            employeeByBusinessPartnerService = DependencyResolver.Kernel.Get<IEmployeeByBusinessPartnerService>();
            employeeByBusinessPartnerHistoryService = DependencyResolver.Kernel.Get<IEmployeeByBusinessPartnerHistoryService>();
            employeeService = DependencyResolver.Kernel.Get<IEmployeeService>();
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentBusinessPartner = businessPartnerViewModel;

            Thread displayThread = new Thread(() =>
            {
                Sync();
            });
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region DisplayData

        private void btnSearchEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        public void DisplayEmployeesOnBusinessPartnerData()
        {
            EmployeeOnBusinessPartnerDataLoading = true;

            EmployeeByBusinessPartnerListResponse response = new EmployeeByBusinessPartnerSQLiteRepository()
                .GetByBusinessPartner(CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                EmployeesOnBusinessPartnerFromDB = new ObservableCollection<EmployeeByBusinessPartnerViewModel>(response?.EmployeeByBusinessPartners ?? new List<EmployeeByBusinessPartnerViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                EmployeesOnBusinessPartnerFromDB = new ObservableCollection<EmployeeByBusinessPartnerViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            EmployeeOnBusinessPartnerDataLoading = false;
        }

        public void DisplayEmployeesNotOnBusinessPartnerData()
        {
            EmployeeNotOnBusinessPartnerDataLoading = true;

            EmployeeListResponse response = new EmployeeSQLiteRepository()
                .GetEmployeesNotOnBusinessPartnerByPage(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier, EmployeeOnBusinessPartnerSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                EmployeesNotOnBusinessPartnerFromDB = new ObservableCollection<EmployeeViewModel>(response?.Employees ?? new List<EmployeeViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                EmployeesNotOnBusinessPartnerFromDB = new ObservableCollection<EmployeeViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            EmployeeNotOnBusinessPartnerDataLoading = false;
        }

        private void Sync()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Radnici ... ";
            new EmployeeSQLiteRepository().Sync(employeeService);

            SyncButtonContent = " Firme ... ";
            new BusinessPartnerSQLiteRepository().Sync(businessPartnerService);

            SyncButtonContent = " Rad. u firmi ... ";
            new EmployeeByBusinessPartnerSQLiteRepository().Sync(employeeByBusinessPartnerService);

            DisplayEmployeesNotOnBusinessPartnerData();
            DisplayEmployeesOnBusinessPartnerData();

            SyncButtonContent = " Osveži ";
            SyncButtonEnabled = true;

        }

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                Sync();
            })
            {
                IsBackground = true
            };
            th.Start();
        }

        #endregion


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeNotOnBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Radnik bez firme";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                EmployeeByBusinessPartnerViewModel employeeByBusinessPartner = new EmployeeByBusinessPartnerViewModel()
                {
                    Identifier = Guid.NewGuid(),
                    Employee = CurrentEmployeeNotOnBusinessPartner,
                    BusinessPartner = CurrentBusinessPartner,
                    StartDate = ContractStartDate,
                    EndDate = ContractEndDate,
                    Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                    CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId }
                };

                EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerSQLiteRepository().Create(employeeByBusinessPartner);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    return;
                }

                response = employeeByBusinessPartnerService.Create(employeeByBusinessPartner);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    return;
                }

                MainWindow.SuccessMessage = "Podaci su uspešno uneti!";

                ContractStartDate = DateTime.Now;
                ContractEndDate = DateTime.Now;

                DisplayEmployeesNotOnBusinessPartnerData();
                DisplayEmployeesOnBusinessPartnerData();
            });
            th.IsBackground = true;
            th.Start();

        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeOnBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Radnik u odabranoj firmi";
                return;
            }

            #endregion

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("radnika", CurrentEmployeeOnBusinessPartner.Employee.Name + " " + CurrentEmployeeOnBusinessPartner.Employee.SurName);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                Thread th = new Thread(() =>
                {
                    EmployeeByBusinessPartnerListResponse listResponse = new EmployeeByBusinessPartnerSQLiteRepository().GetByBusinessPartner(CurrentBusinessPartner.Identifier);
                    EmployeeByBusinessPartnerViewModel employeeByBusinessPartner = listResponse.EmployeeByBusinessPartners.FirstOrDefault(x => x.Employee.Identifier == CurrentEmployeeOnBusinessPartner.Employee.Identifier);
                    EmployeeByBusinessPartnerResponse response = employeeByBusinessPartnerService.Delete(employeeByBusinessPartner?.Identifier ?? Guid.NewGuid());
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                        return;
                    }

                    response = new EmployeeByBusinessPartnerSQLiteRepository().Delete(CurrentEmployeeOnBusinessPartner.Identifier, CurrentBusinessPartner.Identifier);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                        return;
                    }

                    DisplayEmployeesNotOnBusinessPartnerData();
                    DisplayEmployeesOnBusinessPartnerData();
                });
                th.IsBackground = true;
                th.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.CloseFlyout(this);
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
