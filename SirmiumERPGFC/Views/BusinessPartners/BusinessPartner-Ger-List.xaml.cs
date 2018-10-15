using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
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
    public partial class BusinessPartner_Ger_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerService businessPartnerService;
        IBusinessPartnerLocationService businessPartnerLocationService;
        IBusinessPartnerPhoneService businessPartnerPhoneService;
        IBusinessPartnerOrganizationUnitService businessPartnerOrganizationUnitService;
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

        #region BusinessPartnerButtonContent
        private string _BusinessPartnerButtonContent = " Sinhronizacija poslovnih partnera ";

        public string BusinessPartnerButtonContent
        {
            get { return _BusinessPartnerButtonContent; }
            set
            {
                if (_BusinessPartnerButtonContent != value)
                {
                    _BusinessPartnerButtonContent = value;
                    NotifyPropertyChanged("BusinessPartnerButtonContent");
                }
            }
        }
        #endregion

        #region BusinessPartnerButtonEnabled
        private bool _BusinessPartnerButtonEnabled = true;

        public bool BusinessPartnerButtonEnabled
        {
            get { return _BusinessPartnerButtonEnabled; }
            set
            {
                if (_BusinessPartnerButtonEnabled != value)
                {
                    _BusinessPartnerButtonEnabled = value;
                    NotifyPropertyChanged("BusinessPartnerButtonEnabled");
                }
            }
        }
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

        public BusinessPartner_Ger_List()
        {
            // Get required service
            this.businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            this.businessPartnerLocationService = DependencyResolver.Kernel.Get<IBusinessPartnerLocationService>();
            this.businessPartnerPhoneService = DependencyResolver.Kernel.Get<IBusinessPartnerPhoneService>();
            this.businessPartnerOrganizationUnitService = DependencyResolver.Kernel.Get<IBusinessPartnerOrganizationUnitService>();

            // Initialize form components
            InitializeComponent();

            this.DataContext = this;

            Thread displayThread = new Thread(() => DisplayData());
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

                MainWindow.SuccessMessage = "Podaci su uspešno sinhronizovani!";
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

        private void DisplayData()
        {
            BusinessPartnerDataLoading = true;

            var response = new BusinessPartnerSQLiteRepository().GetBusinessPartnersByPage(MainWindow.CurrentCompanyId, BusinessPartnerSearchObject, currentPage, itemsPerPage);
            if (response.Success)
            {
                BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>(response.BusinessPartners ?? new List<BusinessPartnerViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>(new List<BusinessPartnerViewModel>());
                MainWindow.ErrorMessage = response.Message;
                totalItems = 0;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            BusinessPartnerDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Firme ... ";
            new BusinessPartnerSQLiteRepository().Sync(businessPartnerService);

            DisplayData();

            RefreshButtonContent = " Osveži ";
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add, Edit, Delete buttons click

        private void btnAddBusinessPartner_Click(object sender, RoutedEventArgs e)
        {
            BusinessPartnerViewModel businessPartnerViewModel = new BusinessPartnerViewModel();
            businessPartnerViewModel.Code = new BusinessPartnerSQLiteRepository().GetNewCodeValue(MainWindow.CurrentCompanyId);
            businessPartnerViewModel.Identifier = Guid.NewGuid();

            BusinessPartner_List_AddEdit addEditForm = new BusinessPartner_List_AddEdit(businessPartnerViewModel, false);
            addEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(DisplayData);
            FlyoutHelper.OpenFlyout(this, "Podaci o poslovnim partnerima", 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            BusinessPartner_List_AddEdit addEditForm = new BusinessPartner_List_AddEdit(CurrentBusinessPartner, true);
            addEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(DisplayData);
            FlyoutHelper.OpenFlyout(this, "Podaci o poslovnim partnerima", 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBusinessPartner == null)
            {
                MainWindow.ErrorMessage = ("Morate odabrati poslovnog partnera za brisanje!");
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("poslovnog partnera", CurrentBusinessPartner.Name);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                BusinessPartnerResponse response = businessPartnerService.Delete(CurrentBusinessPartner.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new BusinessPartnerSQLiteRepository().Delete(CurrentBusinessPartner.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = "Poslovni partner je uspešno obrisan!";

                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            // Remove blur effects
            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        #endregion

        #region Sync data

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                BusinessPartnerButtonContent = " Sinhronizacija u toku... ";
                BusinessPartnerButtonEnabled = false;

                BusinessPartnerSQLiteRepository sqlLite = new BusinessPartnerSQLiteRepository();
                List<BusinessPartnerViewModel> unsincedBusinessPartner = sqlLite.GetUnSyncedBusinessPartners(MainWindow.CurrentCompanyId).BusinessPartners;
                int total = unsincedBusinessPartner.Count;
                int counter = 0;

                while (counter < total)
                {
                    SyncBusinessPartnerRequest request = new SyncBusinessPartnerRequest();
                    request.CompanyId = MainWindow.CurrentCompanyId;
                    request.UnSyncedBusinessPartners = sqlLite.GetUnSyncedBusinessPartners(MainWindow.CurrentCompanyId).BusinessPartners.Take(100).ToList();
                    request.LastUpdatedAt = sqlLite.GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                    BusinessPartnerListResponse response = businessPartnerService.Sync(request);
                    if (response.Success)
                    {
                        List<BusinessPartnerViewModel> businessPartnersFromDB = response.BusinessPartners;
                        foreach (var businessPartner in businessPartnersFromDB.OrderBy(x => x.Id))
                        {
                            BusinessPartnerButtonContent = " Poslovni partneri: " + counter++ + " od " + total;
                            sqlLite.Delete(businessPartner.Identifier);
                            businessPartner.IsSynced = true;
                            sqlLite.Create(businessPartner);
                        }
                    }
                }

                BusinessPartnerLocationSQLiteRepository sqlLiteLocation = new BusinessPartnerLocationSQLiteRepository();
                List<BusinessPartnerLocationViewModel> unsincedBusinessPartnetLocations = sqlLiteLocation.GetUnSyncedBusinessPartnerLocations(MainWindow.CurrentCompanyId).BusinessPartnerLocations;
                total = unsincedBusinessPartnetLocations.Count;
                counter = 0;

                while (counter < total)
                {
                    SyncBusinessPartnerLocationRequest bankAccountRequest = new SyncBusinessPartnerLocationRequest();
                    bankAccountRequest.CompanyId = MainWindow.CurrentCompanyId;
                    bankAccountRequest.UnSyncedBusinessPartnerLocations = sqlLiteLocation.GetUnSyncedBusinessPartnerLocations(MainWindow.CurrentCompanyId).BusinessPartnerLocations.Take(100).ToList();
                    bankAccountRequest.LastUpdatedAt = sqlLiteLocation.GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                    BusinessPartnerLocationListResponse bankAccountRespose = businessPartnerLocationService.Sync(bankAccountRequest);
                    if (bankAccountRespose.Success)
                    {
                        List<BusinessPartnerLocationViewModel> businessPartnerLocationsFromDB = bankAccountRespose.BusinessPartnerLocations;
                        foreach (var businessPartnerLocation in businessPartnerLocationsFromDB.OrderBy(x => x.Id))
                        {
                            BusinessPartnerButtonContent = " Lokacije: " + counter++ + " od " + total;
                            sqlLiteLocation.Delete(businessPartnerLocation.Identifier);
                            businessPartnerLocation.IsSynced = true;
                            sqlLiteLocation.Create(businessPartnerLocation);
                        }
                    }
                }

                BusinessPartnerPhoneSQLiteRepository sqlLitePhone = new BusinessPartnerPhoneSQLiteRepository();
                List<BusinessPartnerPhoneViewModel> unsincedBusinessPartnetPhones = sqlLitePhone.GetUnSyncedBusinessPartnerPhones(MainWindow.CurrentCompanyId).BusinessPartnerPhones;
                total = unsincedBusinessPartnetPhones.Count;
                counter = 0;

                while (counter < total)
                {
                    SyncBusinessPartnerPhoneRequest bankAccountRequest = new SyncBusinessPartnerPhoneRequest();
                    bankAccountRequest.CompanyId = MainWindow.CurrentCompanyId;
                    bankAccountRequest.UnSyncedBusinessPartnerPhones = sqlLitePhone.GetUnSyncedBusinessPartnerPhones(MainWindow.CurrentCompanyId).BusinessPartnerPhones.Take(100).ToList();
                    bankAccountRequest.LastUpdatedAt = sqlLitePhone.GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                    BusinessPartnerPhoneListResponse bankAccountRespose = businessPartnerPhoneService.Sync(bankAccountRequest);
                    if (bankAccountRespose.Success)
                    {
                        List<BusinessPartnerPhoneViewModel> businessPartnerPhonesFromDB = bankAccountRespose.BusinessPartnerPhones;
                        foreach (var businessPartnerPhone in businessPartnerPhonesFromDB.OrderBy(x => x.Id))
                        {
                            BusinessPartnerButtonContent = " Telefoni: " + counter++ + " od " + total;
                            sqlLitePhone.Delete(businessPartnerPhone.Identifier);
                            businessPartnerPhone.IsSynced = true;
                            sqlLitePhone.Create(businessPartnerPhone);
                        }
                    }
                }

                unsincedBusinessPartner = sqlLite.GetUnSyncedBusinessPartners(MainWindow.CurrentCompanyId).BusinessPartners;

                MainWindow.SuccessMessage = "Podaci su uspešno sinhronizovani!";

                DisplayData();

                BusinessPartnerButtonContent = " Sinhronizacija poslovnih partnera ";
                BusinessPartnerButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
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
