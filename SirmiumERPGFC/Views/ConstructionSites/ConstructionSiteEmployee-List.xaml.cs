using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.ConstructionSites;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
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


        #region EmployeesFromDB
        private ObservableCollection<EmployeeViewModel> _EmployeesFromDB;

        public ObservableCollection<EmployeeViewModel> EmployeesFromDB
        {
            get { return _EmployeesFromDB; }
            set
            {
                if (_EmployeesFromDB != value)
                {
                    _EmployeesFromDB = value;
                    NotifyPropertyChanged("EmployeesFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployee
        private EmployeeViewModel _CurrentEmployee;

        public EmployeeViewModel CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set
            {
                if (_CurrentEmployee != value)
                {
                    _CurrentEmployee = value;
                    NotifyPropertyChanged("CurrentEmployee");
                }
            }
        }
        #endregion

        #region EmployeeDataLoading
        private bool _EmployeeDataLoading;

        public bool EmployeeDataLoading
        {
            get { return _EmployeeDataLoading; }
            set
            {
                if (_EmployeeDataLoading != value)
                {
                    _EmployeeDataLoading = value;
                    NotifyPropertyChanged("EmployeeDataLoading");
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
        private string _RefreshButtonContent = " Osveži ";

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

        private void PopulateDataItems()
        {
            ConstructionSiteDataLoading = true;

            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteSQLiteRepository()
                .GetByConstructionSite(CurrentConstructionSite.Identifier);

            //if (response.Success)
            //{
            //    FoodInputNoteItemsFromDB = new ObservableCollection<FoodInputNoteItemViewModel>(
            //        response.FoodInputNoteItems ?? new List<FoodInputNoteItemViewModel>());
            //}
            //else
            //{
            //    FoodInputNoteItemsFromDB = new ObservableCollection<FoodInputNoteItemViewModel>();
            //    MainWindow.ErrorMessage = "Greška prilikom učitavanja podataka!";
            //}

            ConstructionSiteDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Radnici ... ";
            new EmployeeByConstructionSiteSQLiteRepository().Sync(employeeByConstructionSiteService);

            DisplayData();

            RefreshButtonContent = " Osveži ";
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
            FlyoutHelper.OpenFlyout(this, "Radnici po gradilištu", 95, new ConstructionSiteEmployee_List_AddEdit(CurrentConstructionSite));
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
