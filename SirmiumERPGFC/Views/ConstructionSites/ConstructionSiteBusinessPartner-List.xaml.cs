using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
using SirmiumERPGFC.Repository.ConstructionSites;
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
    public partial class ConstructionSiteBusinessPartner_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerByConstructionSiteService businessPartnerByConstructionSiteService;
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

                    if (_CurrentConstructionSite != null)
                    {
                        var response = new BusinessPartnerSQLiteRepository().GetBusinessPartnersOnConstructionSiteByPage(MainWindow.CurrentCompanyId, _CurrentConstructionSite.Identifier, new BusinessPartnerViewModel(), 1, Int32.MaxValue);
                        if (response.Success)
                        {
                            BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>(response.BusinessPartners ?? new List<BusinessPartnerViewModel>());
                        }
                    }
                    else
                        BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>();
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

        public ConstructionSiteBusinessPartner_List()
        {
            businessPartnerByConstructionSiteService = DependencyResolver.Kernel.Get<IBusinessPartnerByConstructionSiteService>();

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

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Poslovni partneri ... ";
            new BusinessPartnerByConstructionSiteSQLiteRepository().Sync(businessPartnerByConstructionSiteService);

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
            FlyoutHelper.OpenFlyout(this, "Firme po gradilištima", 95, new ConstructionSiteBusinessPartner_List_AddEdit(CurrentConstructionSite));
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
