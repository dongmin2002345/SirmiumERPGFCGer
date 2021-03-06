﻿using Ninject;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.ConstructionSites;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.ConstructionSites;
using SirmiumERPGFC.Repository.Employees;
using SirmiumERPGFC.Views.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.ConstructionSites
{
    public partial class ConstructionSiteEmployee_List_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region 
        IEmployeeByConstructionSiteService employeeByConstructionSiteService;
        IEmployeeByBusinessPartnerService employeeByBusinessPartnerService;
        IEmployeeService employeeService;
        IConstructionSiteService constructionSiteService;
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


        #region EmployeesNotOnConstructionSiteFromDB
        private ObservableCollection<EmployeeViewModel> _EmployeesNotOnConstructionSiteFromDB;

        public ObservableCollection<EmployeeViewModel> EmployeesNotOnConstructionSiteFromDB
        {
            get { return _EmployeesNotOnConstructionSiteFromDB; }
            set
            {
                if (_EmployeesNotOnConstructionSiteFromDB != value)
                {
                    _EmployeesNotOnConstructionSiteFromDB = value;
                    NotifyPropertyChanged("EmployeesNotOnConstructionSiteFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeNotOnConstructionSite
        private EmployeeViewModel _CurrentEmployeeNotOnConstructionSite;

        public EmployeeViewModel CurrentEmployeeNotOnConstructionSite
        {
            get { return _CurrentEmployeeNotOnConstructionSite; }
            set
            {
                if (_CurrentEmployeeNotOnConstructionSite != value)
                {
                    _CurrentEmployeeNotOnConstructionSite = value;
                    NotifyPropertyChanged("CurrentEmployeeNotOnConstructionSite");
                }
            }
        }
        #endregion

        #region EmployeeNotOnConstructionSiteSearchObject
        private EmployeeViewModel _EmployeeNotOnConstructionSiteSearchObject = new EmployeeViewModel();

        public EmployeeViewModel EmployeeNotOnConstructionSiteSearchObject
        {
            get { return _EmployeeNotOnConstructionSiteSearchObject; }
            set
            {
                if (_EmployeeNotOnConstructionSiteSearchObject != value)
                {
                    _EmployeeNotOnConstructionSiteSearchObject = value;
                    NotifyPropertyChanged("EmployeeNotOnConstructionSiteSearchObject");
                }
            }
        }
        #endregion

        #region EmployeeNotOnConstructionSiteDataLoading
        private bool _EmployeeNotOnConstructionSiteDataLoading;

        public bool EmployeeNotOnConstructionSiteDataLoading
        {
            get { return _EmployeeNotOnConstructionSiteDataLoading; }
            set
            {
                if (_EmployeeNotOnConstructionSiteDataLoading != value)
                {
                    _EmployeeNotOnConstructionSiteDataLoading = value;
                    NotifyPropertyChanged("EmployeeNotOnConstructionSiteDataLoading");
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

        #region EmployeeOnConstructionSiteSearchObject
        private EmployeeViewModel _EmployeeOnConstructionSiteSearchObject = new EmployeeViewModel();

        public EmployeeViewModel EmployeeOnConstructionSiteSearchObject
        {
            get { return _EmployeeOnConstructionSiteSearchObject; }
            set
            {
                if (_EmployeeOnConstructionSiteSearchObject != value)
                {
                    _EmployeeOnConstructionSiteSearchObject = value;
                    NotifyPropertyChanged("EmployeeOnConstructionSiteSearchObject");
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

        #region Pagination data Right
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
        private string _SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

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

        public ConstructionSiteEmployee_List_AddEdit(ConstructionSiteViewModel constructionSiteViewModel, BusinessPartnerViewModel businessPartnerViewModel)
        {
            employeeByConstructionSiteService = DependencyResolver.Kernel.Get<IEmployeeByConstructionSiteService>();
            employeeByBusinessPartnerService = DependencyResolver.Kernel.Get<IEmployeeByBusinessPartnerService>();
            employeeService = DependencyResolver.Kernel.Get<IEmployeeService>();
            constructionSiteService = DependencyResolver.Kernel.Get<IConstructionSiteService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentConstructionSite = constructionSiteViewModel;
            CurrentBusinessPartner = businessPartnerViewModel;

            Thread displayThread = new Thread(() =>
            {
                Sync();
            });
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void btnSearchEmployee_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayEmployeesNotOnConstructionSiteData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        public void DisplayEmployeesOnConstructionSiteData()
        {
            EmployeeOnConstructionSiteDataLoading = true;

            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteSQLiteRepository()
                .GetByConstructionSiteAndBusinessPartner(CurrentConstructionSite.Identifier, CurrentBusinessPartner?.Identifier, currentPageRight, itemsPerPageRight);

            if (response.Success)
            {
                EmployeesOnConstructionSiteFromDB = new ObservableCollection<EmployeeByConstructionSiteViewModel>(response?.EmployeeByConstructionSites ?? new List<EmployeeByConstructionSiteViewModel>());
                totalItemsRight = response.TotalItems;
            }
            else
            {
                EmployeesOnConstructionSiteFromDB = new ObservableCollection<EmployeeByConstructionSiteViewModel>();
                totalItemsRight = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItemsRight != 0 ? (currentPageRight - 1) * itemsPerPageRight + 1 : 0;
            int itemTo = currentPageRight * itemsPerPageRight < totalItemsRight ? currentPageRight * itemsPerPageRight : totalItemsRight;

            PaginationDisplayRight = itemFrom + " - " + itemTo + " od " + totalItemsRight;

            EmployeeOnConstructionSiteDataLoading = false;
        }

        public void DisplayEmployeesNotOnConstructionSiteData()
        {
            EmployeeNotOnConstructionSiteDataLoading = true;

            EmployeeListResponse response = new EmployeeSQLiteRepository()
                .GetEmployeesNotOnConstructionSiteByPage(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier, CurrentBusinessPartner.Identifier, EmployeeOnConstructionSiteSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                EmployeesNotOnConstructionSiteFromDB = new ObservableCollection<EmployeeViewModel>(response?.Employees ?? new List<EmployeeViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                EmployeesNotOnConstructionSiteFromDB = new ObservableCollection<EmployeeViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            EmployeeNotOnConstructionSiteDataLoading = false;
        }

        private void Sync()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Radnici_TriTacke"));
            new EmployeeSQLiteRepository().Sync(employeeService);

            SyncButtonContent = ((string)Application.Current.FindResource("Gradilišta_TriTacke"));
            new ConstructionSiteSQLiteRepository().Sync(constructionSiteService);

            SyncButtonContent = ((string)Application.Current.FindResource("RadTačka_na_gradilistu_TriTacke"));
            new EmployeeByConstructionSiteSQLiteRepository().Sync(employeeByConstructionSiteService);

            SyncButtonContent = ((string)Application.Current.FindResource("RadTačka_na_gradilistu_TriTacke"));
            new EmployeeByBusinessPartnerSQLiteRepository().Sync(employeeByBusinessPartnerService);

            DisplayEmployeesNotOnConstructionSiteData();
            DisplayEmployeesOnConstructionSiteData();

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

        private void dgConstructionSiteEmployees_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Add, delete and cancel

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeNotOnConstructionSite == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Radnik_bez_gradilišta"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                EmployeeByConstructionSiteViewModel employeeByConstructionSite = new EmployeeByConstructionSiteViewModel()
                {
                    Identifier = Guid.NewGuid(),
                    Employee = CurrentEmployeeNotOnConstructionSite,
                    ConstructionSite = CurrentConstructionSite,
                    BusinessPartner = CurrentBusinessPartner,
                    StartDate = CurrentEmployeeNotOnConstructionSite.ContractStartDate,
                    EndDate = CurrentEmployeeNotOnConstructionSite.ContractEndDate, 
                    Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                    CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId }
                };

                EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteSQLiteRepository().Create(employeeByConstructionSite);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
                    return;
                }

                response = employeeByConstructionSiteService.Create(employeeByConstructionSite);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik"));
                    return;
                }

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_unetiUzvičnik"));

                DisplayEmployeesNotOnConstructionSiteData();
                DisplayEmployeesOnConstructionSiteData();
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeOnConstructionSite == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Radnik_na_odabranom_gradilištu"));
                return;
            }

            #endregion

                Thread th = new Thread(() =>
                {
                    EmployeeByConstructionSiteResponse response = employeeByConstructionSiteService.Delete(CurrentEmployeeOnConstructionSite);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
                        return;
                    }

                    response = new EmployeeByConstructionSiteSQLiteRepository().Delete(CurrentEmployeeOnConstructionSite.Identifier);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
                        return;
                    }

                    DisplayEmployeesNotOnConstructionSiteData();
                    DisplayEmployeesOnConstructionSiteData();
                });
                th.IsBackground = true;
                th.Start();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        #region Pagination   


        private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage = 1;
                Thread displayThread = new Thread(() => DisplayEmployeesNotOnConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                Thread displayThread = new Thread(() => DisplayEmployeesNotOnConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;
                Thread displayThread = new Thread(() => DisplayEmployeesNotOnConstructionSiteData());
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
                Thread displayThread = new Thread(() => DisplayEmployeesNotOnConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        #endregion

        #region Pagination Right

        private void btnFirstPageRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageRight > 1)
            {
                currentPageRight = 1;
                Thread displayThread = new Thread(() => DisplayEmployeesOnConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPageRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageRight > 1)
            {
                currentPageRight--;
                Thread displayThread = new Thread(() => DisplayEmployeesOnConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        private void btnNextPageRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageRight < Math.Ceiling((double)this.totalItemsRight / this.itemsPerPageRight))
            {
                currentPageRight++;
                Thread displayThread = new Thread(() => DisplayEmployeesOnConstructionSiteData());
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
                Thread displayThread = new Thread(() => DisplayEmployeesOnConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        #endregion

        private void txtSearchByBusinessPartnerEmployeeCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            Thread th = new Thread(() => DisplayEmployeesNotOnConstructionSiteData());
            th.IsBackground = true;
            th.Start();
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

        private void btnAddPopup_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeNotOnConstructionSite.AddPopupOpened = true;
        }

        private void btnCancelAdd_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeNotOnConstructionSite.AddPopupOpened = false;
        }

        private void btnDeletePopup_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeOnConstructionSite.DeletePopupOpened = true;
        }

        private void btnCancelDelete_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeOnConstructionSite.DeletePopupOpened = false;
        }
    }
}
