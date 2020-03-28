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
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.BusinessPartners
{
    public partial class BusinessPartnerEmployee_List_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Atributes

        #region Services
        IEmployeeByBusinessPartnerService employeeByBusinessPartnerService;
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

        #region PaginationRight data

        int currentPageRight = 1;
        int itemsPerPageRight = 50;
        int totalItemsRight = 0;

        #region PaginationDisplayRight
        private string _PaginationDisplayRight;

        public string PaginationDisplayRight
        {
            get { return _PaginationDisplayRight; }
            set
            {
                if (_PaginationDisplayRight != value)
                {
                    _PaginationDisplayRight = value;
                    NotifyPropertyChanged("PaginationDisplayRight");
                }
            }
        }
        #endregion

        #endregion

        #region SyncButtonContent
        private string _SyncButtonContent = ((string)Application.Current.FindResource("SINHRONIZUJ"));

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
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayEmployeesNotOnBusinessPartnerData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        public void DisplayEmployeesOnBusinessPartnerData()
        {
            EmployeeOnBusinessPartnerDataLoading = true;

            EmployeeByBusinessPartnerListResponse response = new EmployeeByBusinessPartnerSQLiteRepository()
                .GetByBusinessPartner(CurrentBusinessPartner.Identifier, "", currentPageRight, itemsPerPageRight);

            if (response.Success)
            {
                EmployeesOnBusinessPartnerFromDB = new ObservableCollection<EmployeeByBusinessPartnerViewModel>(response?.EmployeeByBusinessPartners ?? new List<EmployeeByBusinessPartnerViewModel>());
                totalItemsRight = response.TotalItems;
            }
            else
            {
                EmployeesOnBusinessPartnerFromDB = new ObservableCollection<EmployeeByBusinessPartnerViewModel>();
                totalItemsRight = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItemsRight != 0 ? (currentPageRight - 1) * itemsPerPageRight + 1 : 0;
            int itemTo = currentPageRight * itemsPerPageRight < totalItemsRight ? currentPageRight * itemsPerPageRight : totalItemsRight;

            PaginationDisplayRight = itemFrom + " - " + itemTo + " od " + totalItemsRight;

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

            SyncButtonContent = ((string)Application.Current.FindResource("Radnici_TriTacke"));
            new EmployeeSQLiteRepository().Sync(employeeService);

            SyncButtonContent = ((string)Application.Current.FindResource("Firme_TriTacke"));
            new BusinessPartnerSQLiteRepository().Sync(businessPartnerService);

            SyncButtonContent = ((string)Application.Current.FindResource("RadTačka_u_firmi_TriTacke"));
            new EmployeeByBusinessPartnerSQLiteRepository().Sync(employeeByBusinessPartnerService);

            DisplayEmployeesNotOnBusinessPartnerData();
            DisplayEmployeesOnBusinessPartnerData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
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

        private void dgEmployees_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void dgCompanyEmployees_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeNotOnBusinessPartner == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Radnik_bez_firme"));
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
                    StartDate = CurrentEmployeeNotOnBusinessPartner.ContractStartDate,
                    EndDate = CurrentEmployeeNotOnBusinessPartner.ContractEndDate,
                    Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                    CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId }
                };

                EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerSQLiteRepository().Create(employeeByBusinessPartner);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
                    return;
                }

                response = employeeByBusinessPartnerService.Create(employeeByBusinessPartner);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik"));
                    return;
                }

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_unetiUzvičnik"));

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
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Radnik_u_odabranoj_firmi"));
                return;
            }

            #endregion

                Thread th = new Thread(() =>
                {
                    EmployeeByBusinessPartnerResponse response = employeeByBusinessPartnerService.Delete(CurrentEmployeeOnBusinessPartner);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
                        return;
                    }

                    response = new EmployeeByBusinessPartnerSQLiteRepository().Delete(CurrentEmployeeOnBusinessPartner.Identifier, CurrentBusinessPartner.Identifier);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
                        return;
                    }

                    Sync();
                });
                th.IsBackground = true;
                th.Start();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.CloseFlyout(this);
        }

        #region Pagination   
        
        private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage = 1;
                Thread displayThread = new Thread(() => DisplayEmployeesNotOnBusinessPartnerData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                Thread displayThread = new Thread(() => DisplayEmployeesNotOnBusinessPartnerData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;
                Thread displayThread = new Thread(() => DisplayEmployeesNotOnBusinessPartnerData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        private void btnLastPage_Click(object sender, RoutedEventArgs e)
        {
            int lastPage = (int)Math.Ceiling((double)this.totalItems / this.itemsPerPage);
            if (currentPage < lastPage)
            {
                currentPage = lastPage;
                Thread displayThread = new Thread(() => DisplayEmployeesNotOnBusinessPartnerData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        #endregion

        #region Pagination right  

        private void btnFirstPageRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageRight > 1)
            {
                currentPageRight = 1;
                Thread displayThread = new Thread(() => DisplayEmployeesOnBusinessPartnerData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPageRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageRight > 1)
            {
                currentPageRight--;
                Thread displayThread = new Thread(() => DisplayEmployeesOnBusinessPartnerData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        private void btnNextPageRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageRight < Math.Ceiling((double)this.totalItemsRight / this.itemsPerPageRight))
            {
                currentPageRight++;
                Thread displayThread = new Thread(() => DisplayEmployeesOnBusinessPartnerData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        private void btnLastPageRight_Click(object sender, RoutedEventArgs e)
        {
            int lastPageRight = (int)Math.Ceiling((double)this.totalItemsRight / this.itemsPerPageRight);
            if (currentPageRight < lastPageRight)
            {
                currentPageRight = lastPageRight;
                Thread displayThread = new Thread(() => DisplayEmployeesOnBusinessPartnerData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        #endregion



        private void txtSearchByBusinessPartnerEmployeeCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            Thread th = new Thread(() => DisplayEmployeesNotOnBusinessPartnerData());
            th.IsBackground = true;
            th.Start();
        }

        private void btnAddPopup_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeNotOnBusinessPartner.AddPopupOpened = true;
        }

        private void btnCancelAdd_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeNotOnBusinessPartner.AddPopupOpened = false;
        }

        private void btnDeletePopup_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeOnBusinessPartner.DeletePopupOpened = true;
        }

        private void btnCancelDelete_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeOnBusinessPartner.DeletePopupOpened = false;
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
