using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.ConstructionSites;
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
    public partial class ConstructionSiteBusinessPartner_List_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Atributes

        #region Services 
        IBusinessPartnerByConstructionSiteService businessPartnerByConstructionSiteService;
        IBusinessPartnerService businessPartnerService;
        IConstructionSiteService constructionSiteService;
        IEmployeeByConstructionSiteService employeeByConstructionSiteService;
        #endregion


        #region Events
        public event ConstructionSiteBusinessPartnerHandler ConstructionSiteBusinessPartnerUpdated;
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


        #region BusinessPartnersFromDB
        private ObservableCollection<BusinessPartnerViewModel> _BusinessPartnersFromDB;

        public ObservableCollection<BusinessPartnerViewModel> BusinessPartnersFromDB
        {
            get { return _BusinessPartnersFromDB; }
            set
            {
                if (_BusinessPartnersFromDB != value)
                {
                    _BusinessPartnersFromDB = value;
                    NotifyPropertyChanged("BusinessPartnersFromDB");
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

        #region BusinessPartnerDataLoading
        private bool _BusinessPartnerDataLoading;

        public bool BusinessPartnerDataLoading
        {
            get { return _BusinessPartnerDataLoading; }
            set
            {
                if (_BusinessPartnerDataLoading != value)
                {
                    _BusinessPartnerDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerDataLoading");
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

        #region BusinessPartnerOnConstructionSiteSearchObject
        private BusinessPartnerViewModel _BusinessPartnerOnConstructionSiteSearchObject = new BusinessPartnerViewModel();

        public BusinessPartnerViewModel BusinessPartnerOnConstructionSiteSearchObject
        {
            get { return _BusinessPartnerOnConstructionSiteSearchObject; }
            set
            {
                if (_BusinessPartnerOnConstructionSiteSearchObject != value)
                {
                    _BusinessPartnerOnConstructionSiteSearchObject = value;
                    NotifyPropertyChanged("BusinessPartnerOnConstructionSiteSearchObject");
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
        private string _SyncButtonContent = ((string)Application.Current.FindResource("SNIHRONIZUJ"));

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

        public ConstructionSiteBusinessPartner_List_AddEdit(ConstructionSiteViewModel constructionSiteViewModel)
        {
            businessPartnerByConstructionSiteService = DependencyResolver.Kernel.Get<IBusinessPartnerByConstructionSiteService>();
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            constructionSiteService = DependencyResolver.Kernel.Get<IConstructionSiteService>();
            employeeByConstructionSiteService = DependencyResolver.Kernel.Get<IEmployeeByConstructionSiteService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentConstructionSite = constructionSiteViewModel;

            Thread displayThread = new Thread(() =>
            {
                Sync();
            });
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void btnSearchBusinessPartner_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayBusinessPartnersOnConstructionSiteData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        public void DisplayBusinessPartnersOnConstructionSiteData()
        {
            BusinessPartnerOnConstructionSiteDataLoading = true;

            BusinessPartnerByConstructionSiteListResponse response = new BusinessPartnerByConstructionSiteSQLiteRepository()
                .GetByConstructionSite(CurrentConstructionSite.Identifier, "", currentPageRight, itemsPerPageRight);

            if (response.Success)
            {
                BusinessPartnersOnConstructionSiteFromDB = new ObservableCollection<BusinessPartnerByConstructionSiteViewModel>(response?.BusinessPartnerByConstructionSites ?? new List<BusinessPartnerByConstructionSiteViewModel>());
                totalItemsRight = response.TotalItems;

                DisplayBusinessPartnersData(BusinessPartnersOnConstructionSiteFromDB);
            }
            else
            {
                BusinessPartnersOnConstructionSiteFromDB = new ObservableCollection<BusinessPartnerByConstructionSiteViewModel>();
                totalItemsRight = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItemsRight != 0 ? (currentPageRight - 1) * itemsPerPageRight + 1 : 0;
            int itemTo = currentPageRight * itemsPerPageRight < totalItemsRight ? currentPageRight * itemsPerPageRight : totalItemsRight;

            PaginationDisplayRight = itemFrom + " - " + itemTo + " od " + totalItemsRight;

            BusinessPartnerOnConstructionSiteDataLoading = false;
        }

        public void DisplayBusinessPartnersData(ObservableCollection<BusinessPartnerByConstructionSiteViewModel> businessPartnersOnConstructionSites)
        {
            BusinessPartnerDataLoading = true;

            BusinessPartnerListResponse response = new BusinessPartnerSQLiteRepository()
                .GetBusinessPartnersByPage(MainWindow.CurrentCompanyId, BusinessPartnerOnConstructionSiteSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>(response.BusinessPartners ?? new List<BusinessPartnerViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            BusinessPartnerDataLoading = false;
        }

        private void Sync()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Firme_TriTacke"));
            new BusinessPartnerSQLiteRepository().Sync(businessPartnerService);

            SyncButtonContent = ((string)Application.Current.FindResource("Gradilišta_TriTacke"));
            new ConstructionSiteSQLiteRepository().Sync(constructionSiteService);

            SyncButtonContent = ((string)Application.Current.FindResource("FirmeTačka_na_gradilistu_TriTacke"));
            new BusinessPartnerByConstructionSiteSQLiteRepository().Sync(businessPartnerByConstructionSiteService);

            DisplayBusinessPartnersOnConstructionSiteData();

            SyncButtonContent = ((string)Application.Current.FindResource("SNIHRONIZUJ"));
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

        private void dgBusinessPartners_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void dgConstructionSiteBusinessPartners_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Add, delete and cancel

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Firma_bez_gradilišta"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                BusinessPartnerByConstructionSiteViewModel businessPartnerByConstructionSite = new BusinessPartnerByConstructionSiteViewModel()
                {
                    Identifier = Guid.NewGuid(),
                    BusinessPartner = CurrentBusinessPartner,
                    ConstructionSite = CurrentConstructionSite,
                    StartDate = CurrentBusinessPartner.ContractStartDate,
                    EndDate = CurrentBusinessPartner.ContractEndDate,
                    MaxNumOfEmployees = CurrentBusinessPartner.MaxNumOfEmployees,
                    Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                    CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId }
                };

                BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteSQLiteRepository().Create(businessPartnerByConstructionSite);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
                    return;
                }

                response = businessPartnerByConstructionSiteService.Create(businessPartnerByConstructionSite);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik"));
                    return;
                }

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_unetiUzvičnik"));

                DisplayBusinessPartnersOnConstructionSiteData();

                //ConstructionSiteBusinessPartnerUpdated();
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartnerOnConstructionSite == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Poslovni_partner_na_odabranom_gradilištu"));
                return;
            }

            #endregion

                Thread th = new Thread(() =>
                {
                    // Remove business partner on construction site
                    BusinessPartnerByConstructionSiteResponse response = businessPartnerByConstructionSiteService.Delete(CurrentBusinessPartnerOnConstructionSite);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
                        return;
                    }

                    // Remove employees on that construction site from that business partner
                    EmployeeByConstructionSiteListResponse employeesResponse = new EmployeeByConstructionSiteSQLiteRepository()
                    .GetByConstructionSiteAndBusinessPartner(
                        CurrentBusinessPartnerOnConstructionSite.ConstructionSite.Identifier,
                        CurrentBusinessPartnerOnConstructionSite.BusinessPartner.Identifier);

                    foreach (var item in employeesResponse.EmployeeByConstructionSites)
                    {
                        item.RealEndDate = CurrentBusinessPartnerOnConstructionSite.RealEndDate;
                        EmployeeByConstructionSiteResponse employeeResponse = employeeByConstructionSiteService.Delete(item);
                        new EmployeeByConstructionSiteSQLiteRepository().Delete(item.Identifier);
                    }                  


                    response = new BusinessPartnerByConstructionSiteSQLiteRepository().Delete(CurrentBusinessPartnerOnConstructionSite.BusinessPartner.Identifier, CurrentConstructionSite.Identifier);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
                        return;
                    }

                    DisplayBusinessPartnersOnConstructionSiteData();

                    ConstructionSiteBusinessPartnerUpdated();
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
                Thread displayThread = new Thread(() => DisplayBusinessPartnersOnConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                Thread displayThread = new Thread(() => DisplayBusinessPartnersOnConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;
                Thread displayThread = new Thread(() => DisplayBusinessPartnersOnConstructionSiteData());
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
                Thread displayThread = new Thread(() => DisplayBusinessPartnersOnConstructionSiteData());
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
                Thread displayThread = new Thread(() => DisplayBusinessPartnersOnConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPageRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageRight > 1)
            {
                currentPageRight--;
                Thread displayThread = new Thread(() => DisplayBusinessPartnersOnConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        private void btnNextPageRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageRight < Math.Ceiling((double)this.totalItemsRight / this.itemsPerPageRight))
            {
                currentPageRight++;
                Thread displayThread = new Thread(() => DisplayBusinessPartnersOnConstructionSiteData());
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
                Thread displayThread = new Thread(() => DisplayBusinessPartnersOnConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

        }

        #endregion

        private void txtSearchByBusinessPartnerEmployeeCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            Thread th = new Thread(() => DisplayBusinessPartnersOnConstructionSiteData());
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
            CurrentBusinessPartner.AddPopupOpened = true;
        }

        private void btnCancelAdd_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartner.AddPopupOpened = false;
        }

        private void btnDeletePopup_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerOnConstructionSite.DeletePopupOpened = true;
        }

        private void btnCancelDelete_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerOnConstructionSite.DeletePopupOpened = false;
        }
    }
}
