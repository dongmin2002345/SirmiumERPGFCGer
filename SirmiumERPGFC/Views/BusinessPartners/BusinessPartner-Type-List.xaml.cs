using Newtonsoft.Json;
using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.BusinessPartners
{
    public delegate void BusinessPartnerTypeHandler();

    public partial class BusinessPartner_Type_List : UserControl, INotifyPropertyChanged
    {
        #region Atributes

        #region Service
        IBusinessPartnerTypeService businessPartnerTypeService;
        #endregion

        #region BusinessPartnerTypesFromDB
        private ObservableCollection<BusinessPartnerTypeViewModel> _BusinessPartnerTypesFromDB;

        public ObservableCollection<BusinessPartnerTypeViewModel> BusinessPartnerTypesFromDB
        {
            get { return _BusinessPartnerTypesFromDB; }
            set
            {
                if (_BusinessPartnerTypesFromDB != value)
                {
                    _BusinessPartnerTypesFromDB = value;
                    NotifyPropertyChanged("BusinessPartnerTypesFromDB");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerType
        private BusinessPartnerTypeViewModel _CurrentBusinessPartnerType;

        public BusinessPartnerTypeViewModel CurrentBusinessPartnerType
        {
            get { return _CurrentBusinessPartnerType; }
            set
            {
                if (_CurrentBusinessPartnerType != value)
                {
                    _CurrentBusinessPartnerType = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerType");
                }
            }
        }
        #endregion

        #region BusinessPartnerTypeSearchObject
        private BusinessPartnerTypeViewModel _BusinessPartnerTypeSearchObject = new BusinessPartnerTypeViewModel();

        public BusinessPartnerTypeViewModel BusinessPartnerTypeSearchObject
        {
            get { return _BusinessPartnerTypeSearchObject; }
            set
            {
                if (_BusinessPartnerTypeSearchObject != value)
                {
                    _BusinessPartnerTypeSearchObject = value;
                    NotifyPropertyChanged("BusinessPartnerTypeSearchObject");
                }
            }
        }
        #endregion

        #region BusinessPartnerTypesDataLoading
        private bool _BusinessPartnerTypesDataLoading = true;

        public bool BusinessPartnerTypesDataLoading
        {
            get { return _BusinessPartnerTypesDataLoading; }
            set
            {
                if (_BusinessPartnerTypesDataLoading != value)
                {
                    _BusinessPartnerTypesDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerTypesDataLoading");
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

        public BusinessPartner_Type_List()
        {
            // Get required services
            this.businessPartnerTypeService = DependencyResolver.Kernel.Get<IBusinessPartnerTypeService>();

            InitializeComponent();

            this.DataContext = this;

            Thread displayThread = new Thread(() => SyncData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread syncThread = new Thread(() =>
            {
                SyncData();

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sinhronizovaniUzvičnik"));
            });
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        public void DisplayData()
        {
            BusinessPartnerTypesDataLoading = true;

            string SearchObjectJson = JsonConvert.SerializeObject(BusinessPartnerTypeSearchObject,
              Formatting.Indented,
              new JsonSerializerSettings
              {
                  DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
              });

            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeSQLiteRepository()
                .GetBusinessPartnerTypesByPage(MainWindow.CurrentCompanyId, BusinessPartnerTypeSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                BusinessPartnerTypesFromDB = new ObservableCollection<BusinessPartnerTypeViewModel>(response.BusinessPartnerTypes ?? new List<BusinessPartnerTypeViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                BusinessPartnerTypesFromDB = new ObservableCollection<BusinessPartnerTypeViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            BusinessPartnerTypesDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = ((string)Application.Current.FindResource("Tip_PPTačka_TriTacke"));
            new BusinessPartnerTypeSQLiteRepository().Sync(businessPartnerTypeService);

            DisplayData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
        }

        private void dgBusinessPartnerTypes_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            BusinessPartnerTypeViewModel businessPartnerType = new BusinessPartnerTypeViewModel();
            businessPartnerType.Identifier = Guid.NewGuid();

            BusinessPartner_Type_AddEdit addEditForm = new BusinessPartner_Type_AddEdit(businessPartnerType, true);
            addEditForm.BusinessPartnerTypeCreatedUpdated += new BusinessPartnerTypeHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_vrsti")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBusinessPartnerType == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_vrstu_za_izmenuUzvičnik"));
                return;
            }

            BusinessPartner_Type_AddEdit addEditForm = new BusinessPartner_Type_AddEdit(CurrentBusinessPartnerType, false);
            addEditForm.BusinessPartnerTypeCreatedUpdated += new BusinessPartnerTypeHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_vrsti")), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBusinessPartnerType == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_brisanjeUzvičnik"));
                return;
            }

            // Delete data
            var result = businessPartnerTypeService.Delete(CurrentBusinessPartnerType.Identifier);
            if (result.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_obrisaniUzvičnik"));

                Thread displayThread = new Thread(() => SyncData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
            {
                MainWindow.ErrorMessage = result.Message;
            }
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
