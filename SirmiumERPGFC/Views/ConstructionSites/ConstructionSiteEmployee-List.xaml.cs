using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.ConstructionSites;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
using SirmiumERPGFC.Repository.ConstructionSites;
using SirmiumERPGFC.Repository.Employees;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.ConstructionSites
{
    public delegate void EmployeeByConstructionSiteHandler();

    public partial class ConstructionSiteEmployee_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IEmployeeByConstructionSiteService employeeByConstructionSiteService;
        #endregion

        #region ConstructionSitesFromDB
        private ObservableCollection<ConstructionSiteViewModel> _ConstructionSitesFromDB;

        public ObservableCollection<ConstructionSiteViewModel> ConstructionSitesFromDB
        {
            get { return _ConstructionSitesFromDB; }
            set
            {
                if (_ConstructionSitesFromDB != value)
                {
                    _ConstructionSitesFromDB = value;
                    NotifyPropertyChanged("ConstructionSitesFromDB");

                    if (_ConstructionSitesFromDB != null)
                    {
                        TotalNumOfConstructionSites = _ConstructionSitesFromDB.Count;
                    }
                }
            }
        }
        #endregion

        #region CurrentConstructionSite
        private ConstructionSiteViewModel _CurrentConstructionSite;

        public ConstructionSiteViewModel CurrentConstructionSite
        {
            get { return _CurrentConstructionSite; }
            set
            {
                if (_CurrentConstructionSite != value)
                {
                    _CurrentConstructionSite = value;
                    NotifyPropertyChanged("CurrentConstructionSite");

                    if (_CurrentConstructionSite != null)
                    {
                        DisplayBusinessPartnersOnConstructionSiteData();
                        DisplayEmployeesOnConstructionSiteData();
                    }
                    else
                    {
                        EmployeesOnConstructionSiteFromDB = new ObservableCollection<EmployeeByConstructionSiteViewModel>();
                        BusinessPartnersOnConstructionSiteFromDB = new ObservableCollection<BusinessPartnerByConstructionSiteViewModel>();
                    }
                }
            }
        }
        #endregion

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

        #region ConstructionSiteDataLoading
        private bool _ConstructionSiteDataLoading;

        public bool ConstructionSiteDataLoading
        {
            get { return _ConstructionSiteDataLoading; }
            set
            {
                if (_ConstructionSiteDataLoading != value)
                {
                    _ConstructionSiteDataLoading = value;
                    NotifyPropertyChanged("ConstructionSiteDataLoading");
                }
            }
        }
        #endregion
        

        #region BusinessPartnersOnConstructionSiteFromDB
        private ObservableCollection<BusinessPartnerByConstructionSiteViewModel> _BusinessPartnersOnConstructionSiteFromDB;

        public ObservableCollection<BusinessPartnerByConstructionSiteViewModel> BusinessPartnersOnConstructionSiteFromDB
        {
            get { return _BusinessPartnersOnConstructionSiteFromDB; }
            set
            {
                if (_BusinessPartnersOnConstructionSiteFromDB != value)
                {
                    _BusinessPartnersOnConstructionSiteFromDB = value;
                    NotifyPropertyChanged("BusinessPartnersOnConstructionSiteFromDB");

                    if (_BusinessPartnersOnConstructionSiteFromDB != null)
                    {
                        TotalNumOfBusinessPartners = _BusinessPartnersOnConstructionSiteFromDB.Count;
                    }
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerOnConstructionSite
        private BusinessPartnerByConstructionSiteViewModel _CurrentBusinessPartnerOnConstructionSite;

        public BusinessPartnerByConstructionSiteViewModel CurrentBusinessPartnerOnConstructionSite
        {
            get { return _CurrentBusinessPartnerOnConstructionSite; }
            set
            {
                if (_CurrentBusinessPartnerOnConstructionSite != value)
                {
                    _CurrentBusinessPartnerOnConstructionSite = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerOnConstructionSite");
                }
            }
        }
        #endregion

        #region BusinessPartnerOnConstructionSiteDataLoading
        private bool _BusinessPartnerOnConstructionSiteDataLoading;

        public bool BusinessPartnerOnConstructionSiteDataLoading
        {
            get { return _BusinessPartnerOnConstructionSiteDataLoading; }
            set
            {
                if (_BusinessPartnerOnConstructionSiteDataLoading != value)
                {
                    _BusinessPartnerOnConstructionSiteDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerOnConstructionSiteDataLoading");
                }
            }
        }
        #endregion


        #region EmployeesOnConstructionSiteFromDB
        private ObservableCollection<EmployeeByConstructionSiteViewModel> _EmployeesOnConstructionSiteFromDB;

        public ObservableCollection<EmployeeByConstructionSiteViewModel> EmployeesOnConstructionSiteFromDB
        {
            get { return _EmployeesOnConstructionSiteFromDB; }
            set
            {
                if (_EmployeesOnConstructionSiteFromDB != value)
                {
                    _EmployeesOnConstructionSiteFromDB = value;
                    NotifyPropertyChanged("EmployeesOnConstructionSiteFromDB");

                    if (_EmployeesOnConstructionSiteFromDB != null)
                    {
                        TotalNumOfEmployees = _EmployeesOnConstructionSiteFromDB.Count;
                    }
                }
            }
        }
        #endregion

        #region CurrentEmployeeOnConstructionSite
        private EmployeeByConstructionSiteViewModel _CurrentEmployeeOnConstructionSite;

        public EmployeeByConstructionSiteViewModel CurrentEmployeeOnConstructionSite
        {
            get { return _CurrentEmployeeOnConstructionSite; }
            set
            {
                if (_CurrentEmployeeOnConstructionSite != value)
                {
                    _CurrentEmployeeOnConstructionSite = value;
                    NotifyPropertyChanged("CurrentEmployeeOnConstructionSite");
                }
            }
        }
        #endregion

        #region EmployeeOnConstructionSiteDataLoading
        private bool _EmployeeOnConstructionSiteDataLoading;

        public bool EmployeeOnConstructionSiteDataLoading
        {
            get { return _EmployeeOnConstructionSiteDataLoading; }
            set
            {
                if (_EmployeeOnConstructionSiteDataLoading != value)
                {
                    _EmployeeOnConstructionSiteDataLoading = value;
                    NotifyPropertyChanged("EmployeeOnConstructionSiteDataLoading");
                }
            }
        }
        #endregion


        #region TotalNumOfConstructionSites
        private int _TotalNumOfConstructionSites;

        public int TotalNumOfConstructionSites
        {
            get { return _TotalNumOfConstructionSites; }
            set
            {
                if (_TotalNumOfConstructionSites != value)
                {
                    _TotalNumOfConstructionSites = value;
                    NotifyPropertyChanged("TotalNumOfConstructionSites");
                }
            }
        }
        #endregion

        #region TotalNumOfBusinessPartners
        private int _TotalNumOfBusinessPartners;

        public int TotalNumOfBusinessPartners
        {
            get { return _TotalNumOfBusinessPartners; }
            set
            {
                if (_TotalNumOfBusinessPartners != value)
                {
                    _TotalNumOfBusinessPartners = value;
                    NotifyPropertyChanged("TotalNumOfBusinessPartners");
                }
            }
        }
        #endregion

        #region TotalNumOfEmployees
        private int _TotalNumOfEmployees;

        public int TotalNumOfEmployees
        {
            get { return _TotalNumOfEmployees; }
            set
            {
                if (_TotalNumOfEmployees != value)
                {
                    _TotalNumOfEmployees = value;
                    NotifyPropertyChanged("TotalNumOfEmployees");
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


        #region RefreshButtonContent
        private string _RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

        public string RefreshButtonContent
        {
            get { return _RefreshButtonContent; }
            set
            {
                if (_RefreshButtonContent != value)
                {
                    _RefreshButtonContent = value;
                    NotifyPropertyChanged("RefreshButtonContent");
                }
            }
        }
        #endregion

        #region RefreshButtonEnabled
        private bool _RefreshButtonEnabled = true;

        public bool RefreshButtonEnabled
        {
            get { return _RefreshButtonEnabled; }
            set
            {
                if (_RefreshButtonEnabled != value)
                {
                    _RefreshButtonEnabled = value;
                    NotifyPropertyChanged("RefreshButtonEnabled");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public ConstructionSiteEmployee_List()
        {
            employeeByConstructionSiteService = DependencyResolver.Kernel.Get<IEmployeeByConstructionSiteService>();

            InitializeComponent();

            this.DataContext = this;

            Thread displayThread = new Thread(() => SyncData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread syncThread = new Thread(() =>
            {
                SyncData();

                MainWindow.SuccessMessage = "Podaci su uspešno sinhronizovani!";
            });
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        public void DisplayData()
        {
            ConstructionSiteDataLoading = true;

            ConstructionSiteListResponse response = new ConstructionSiteSQLiteRepository()
                .GetConstructionSitesByPage(MainWindow.CurrentCompanyId, ConstructionSiteSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                ConstructionSitesFromDB = new ObservableCollection<ConstructionSiteViewModel>(response?.ConstructionSites ?? new List<ConstructionSiteViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                ConstructionSitesFromDB = new ObservableCollection<ConstructionSiteViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            ConstructionSiteDataLoading = false;
        }

        public void DisplayBusinessPartnersOnConstructionSiteData()
        {
            BusinessPartnerOnConstructionSiteDataLoading = true;

            BusinessPartnerByConstructionSiteListResponse response = new BusinessPartnerByConstructionSiteSQLiteRepository()
                .GetByConstructionSite(CurrentConstructionSite.Identifier);

            if (response.Success)
            {
                BusinessPartnersOnConstructionSiteFromDB = new ObservableCollection<BusinessPartnerByConstructionSiteViewModel>(response?.BusinessPartnerByConstructionSites ?? new List<BusinessPartnerByConstructionSiteViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                BusinessPartnersOnConstructionSiteFromDB = new ObservableCollection<BusinessPartnerByConstructionSiteViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            BusinessPartnerOnConstructionSiteDataLoading = false;
        }

        public void DisplayEmployeesOnConstructionSiteData()
        {
            EmployeeOnConstructionSiteDataLoading = true;

            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteSQLiteRepository()
                .GetByConstructionSiteAndBusinessPartner(CurrentConstructionSite.Identifier, CurrentBusinessPartnerOnConstructionSite?.BusinessPartner?.Identifier);

            if (response.Success)
            {
                EmployeesOnConstructionSiteFromDB = new ObservableCollection<EmployeeByConstructionSiteViewModel>(response?.EmployeeByConstructionSites ?? new List<EmployeeByConstructionSiteViewModel>());
            }
            else
            {
                EmployeesOnConstructionSiteFromDB = new ObservableCollection<EmployeeByConstructionSiteViewModel>();
                MainWindow.ErrorMessage = response.Message;
            }

            EmployeeOnConstructionSiteDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Radnici ... ";
            new EmployeeByConstructionSiteSQLiteRepository().Sync(employeeByConstructionSiteService);

            DisplayData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Pagination

        private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage = 1;
                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;
                Thread displayThread = new Thread(() => DisplayData());
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
                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }
        #endregion

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.OpenFlyout(this, "Radnici po gradilištu", 95, new ConstructionSiteEmployee_List_AddEdit(CurrentConstructionSite, CurrentBusinessPartnerOnConstructionSite.BusinessPartner));
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
