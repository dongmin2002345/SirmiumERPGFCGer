using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
using SirmiumERPGFC.Repository.ConstructionSites;
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

namespace SirmiumERPGFC.Views.ConstructionSites
{
    public partial class ConstructionSiteBusinessPartner_List_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region 
        IBusinessPartnerByConstructionSiteService businessPartnerByConstructionSiteService;
        IBusinessPartnerByConstructionSiteHistoryService businessPartnerByConstructionSiteHistoryService;
        IBusinessPartnerService businessPartnerService;
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

        public ConstructionSiteBusinessPartner_List_AddEdit(ConstructionSiteViewModel constructionSiteViewModel)
        {
            businessPartnerByConstructionSiteService = DependencyResolver.Kernel.Get<IBusinessPartnerByConstructionSiteService>();
            businessPartnerByConstructionSiteHistoryService = DependencyResolver.Kernel.Get<IBusinessPartnerByConstructionSiteHistoryService>();
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            constructionSiteService = DependencyResolver.Kernel.Get<IConstructionSiteService>();

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

        public void DisplayBusinessPartnersData()
        {
            BusinessPartnerDataLoading = true;

            BusinessPartnerListResponse response = new BusinessPartnerSQLiteRepository()
                .GetBusinessPartnersByPage(MainWindow.CurrentCompanyId, BusinessPartnerOnConstructionSiteSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>(response?.BusinessPartners ?? new List<BusinessPartnerViewModel>());
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

            SyncButtonContent = " Firme ... ";
            new BusinessPartnerSQLiteRepository().Sync(businessPartnerService);

            SyncButtonContent = " Gradilišta ... ";
            new ConstructionSiteSQLiteRepository().Sync(constructionSiteService);

            SyncButtonContent = " Firme. na gradilistu ... ";
            new BusinessPartnerByConstructionSiteSQLiteRepository().Sync(businessPartnerByConstructionSiteService);

            DisplayBusinessPartnersData();
            DisplayBusinessPartnersOnConstructionSiteData();

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

        #region Add, delete and cancel

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Firma bez gradilišta";
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
                    StartDate = ContractStartDate,
                    //EndDate = ContractEndDate,
                    Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                    CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId }
                };

                BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteSQLiteRepository().Create(businessPartnerByConstructionSite);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    return;
                }

                response = businessPartnerByConstructionSiteService.Create(businessPartnerByConstructionSite);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    return;
                }

                MainWindow.SuccessMessage = "Podaci su uspešno uneti!";

                DisplayBusinessPartnersData();
                DisplayBusinessPartnersOnConstructionSiteData();
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartnerOnConstructionSite == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Radnik na odabranom gradilištu";
                return;
            }

            #endregion

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("firmu", CurrentBusinessPartnerOnConstructionSite.BusinessPartner.Name);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                Thread th = new Thread(() =>
                {
                    BusinessPartnerByConstructionSiteListResponse listResponse = new BusinessPartnerByConstructionSiteSQLiteRepository().GetByConstructionSite(CurrentConstructionSite.Identifier);
                    BusinessPartnerByConstructionSiteViewModel businessPartnerByConstructionSite = listResponse.BusinessPartnerByConstructionSites
                        .FirstOrDefault(x => x.BusinessPartner.Identifier == CurrentBusinessPartnerOnConstructionSite.BusinessPartner.Identifier);
                    BusinessPartnerByConstructionSiteResponse response = businessPartnerByConstructionSiteService.Delete(businessPartnerByConstructionSite.Identifier);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                        return;
                    }

                    response = new BusinessPartnerByConstructionSiteSQLiteRepository().Delete(CurrentBusinessPartnerOnConstructionSite.BusinessPartner.Identifier, CurrentConstructionSite.Identifier);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                        return;
                    }

                    DisplayBusinessPartnersData();
                    DisplayBusinessPartnersOnConstructionSiteData();
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

        #endregion

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
