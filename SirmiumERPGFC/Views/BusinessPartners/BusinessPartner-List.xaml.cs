using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Reports.BusinessPartners;
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
    public delegate void BusinessPartnerHandler();

    public partial class BusinessPartner_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerService businessPartnerService;
		IBusinessPartnerLocationService businessPartnerLocationService;
        IBusinessPartnerPhoneService businessPartnerPhoneService;
        IBusinessPartnerOrganizationUnitService businessPartnerOrganizationUnitService;
        IBusinessPartnerDocumentService businessPartnerDocumentService;
        IBusinessPartnerBankService businessPartnerBankService;
        IBusinessPartnerInstitutionService businessPartnerInstitutionService;
        IBusinessPartnerTypeService businessPartnerTypeService;
        IBusinessPartnerNoteService businessPartnerNoteService;
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

		#region BusinessPartnerLocationsFromDB
		private ObservableCollection<BusinessPartnerLocationViewModel> _BusinessPartnerLocationsFromDB = new ObservableCollection<BusinessPartnerLocationViewModel>();

		public ObservableCollection<BusinessPartnerLocationViewModel> BusinessPartnerLocationsFromDB
		{
			get { return _BusinessPartnerLocationsFromDB; }
			set
			{
				if (_BusinessPartnerLocationsFromDB != value)
				{
					_BusinessPartnerLocationsFromDB = value;
					NotifyPropertyChanged("BusinessPartnerLocationsFromDB");
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

                    if (_CurrentBusinessPartner != null)
                    {
                        Thread th = new Thread(() =>
                        {
                            DisplayDocumentData();
                            DisplayPhoneData();
							DisplayNoteData();
                        });
                        th.IsBackground = true;
                        th.Start();
                    }
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


		#region NotesFromDB
		private ObservableCollection<BusinessPartnerNoteViewModel> _NotesFromDB;

        public ObservableCollection<BusinessPartnerNoteViewModel> NotesFromDB
		{
            get { return _NotesFromDB; }
            set
            {
                if (_NotesFromDB != value)
                {
					_NotesFromDB = value;
                    NotifyPropertyChanged("NotesFromDB");
                }
            }
        }
		#endregion
        
		#region CurrentNoteDG
		private BusinessPartnerNoteViewModel _CurrentNoteDG;

        public BusinessPartnerNoteViewModel CurrentNoteDG
		{
            get { return _CurrentNoteDG; }
            set
            {
                if (_CurrentNoteDG != value)
                {
					_CurrentNoteDG = value;
                    NotifyPropertyChanged("CurrentNoteDG");
                }
            }
        }
		#endregion

		#region NoteDataLoading
		private bool _NoteDataLoading;

        public bool NoteDataLoading
		{
            get { return _NoteDataLoading; }
            set
            {
                if (_NoteDataLoading != value)
                {
					_NoteDataLoading = value;
                    NotifyPropertyChanged("NoteDataLoading");
                }
            }
        }
        #endregion


        #region PhonesFromDB
        private ObservableCollection<BusinessPartnerPhoneViewModel> _PhonesFromDB;

        public ObservableCollection<BusinessPartnerPhoneViewModel> PhonesFromDB
        {
            get { return _PhonesFromDB; }
            set
            {
                if (_PhonesFromDB != value)
                {
                    _PhonesFromDB = value;
                    NotifyPropertyChanged("PhonesFromDB");
                }
            }
        }
        #endregion
                
        #region CurrentPhoneDG
        private BusinessPartnerPhoneViewModel _CurrentPhoneDG;

        public BusinessPartnerPhoneViewModel CurrentPhoneDG
        {
            get { return _CurrentPhoneDG; }
            set
            {
                if (_CurrentPhoneDG != value)
                {
                    _CurrentPhoneDG = value;
                    NotifyPropertyChanged("CurrentPhoneDG");
                }
            }
        }
        #endregion

        #region PhoneDataLoading
        private bool _PhoneDataLoading;

        public bool PhoneDataLoading
        {
            get { return _PhoneDataLoading; }
            set
            {
                if (_PhoneDataLoading != value)
                {
                    _PhoneDataLoading = value;
                    NotifyPropertyChanged("PhoneDataLoading");
                }
            }
        }
        #endregion


        #region BusinessPartnerDocumentsFromDB
        private ObservableCollection<BusinessPartnerDocumentViewModel> _BusinessPartnerDocumentsFromDB;

        public ObservableCollection<BusinessPartnerDocumentViewModel> BusinessPartnerDocumentsFromDB
        {
            get { return _BusinessPartnerDocumentsFromDB; }
            set
            {
                if (_BusinessPartnerDocumentsFromDB != value)
                {
                    _BusinessPartnerDocumentsFromDB = value;
                    NotifyPropertyChanged("BusinessPartnerDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerDocument
        private BusinessPartnerDocumentViewModel _CurrentBusinessPartnerDocument;

        public BusinessPartnerDocumentViewModel CurrentBusinessPartnerDocument
        {
            get { return _CurrentBusinessPartnerDocument; }
            set
            {
                if (_CurrentBusinessPartnerDocument != value)
                {
                    _CurrentBusinessPartnerDocument = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerDocument");
                }
            }
        }
        #endregion

        #region BusinessPartnerDocumentDataLoading
        private bool _BusinessPartnerDocumentDataLoading;

        public bool BusinessPartnerDocumentDataLoading
        {
            get { return _BusinessPartnerDocumentDataLoading; }
            set
            {
                if (_BusinessPartnerDocumentDataLoading != value)
                {
                    _BusinessPartnerDocumentDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerDocumentDataLoading");
                }
            }
        }
        #endregion


        #region InstitutionsFromDB
        private ObservableCollection<BusinessPartnerInstitutionViewModel> _InstitutionsFromDB;

        public ObservableCollection<BusinessPartnerInstitutionViewModel> InstitutionsFromDB
        {
            get { return _InstitutionsFromDB; }
            set
            {
                if (_InstitutionsFromDB != value)
                {
                    _InstitutionsFromDB = value;
                    NotifyPropertyChanged("InstitutionsFromDB");
                }
            }
        }
        #endregion
                
        #region CurrentInstitutionDG
        private BusinessPartnerInstitutionViewModel _CurrentInstitutionDG;

        public BusinessPartnerInstitutionViewModel CurrentInstitutionDG
        {
            get { return _CurrentInstitutionDG; }
            set
            {
                if (_CurrentInstitutionDG != value)
                {
                    _CurrentInstitutionDG = value;
                    NotifyPropertyChanged("CurrentInstitutionDG");
                }
            }
        }
        #endregion

        #region InstitutionDataLoading
        private bool _InstitutionDataLoading;

        public bool InstitutionDataLoading
        {
            get { return _InstitutionDataLoading; }
            set
            {
                if (_InstitutionDataLoading != value)
                {
                    _InstitutionDataLoading = value;
                    NotifyPropertyChanged("InstitutionDataLoading");
                }
            }
        }
        #endregion


        #region BanksFromDB
        private ObservableCollection<BusinessPartnerBankViewModel> _BanksFromDB;

        public ObservableCollection<BusinessPartnerBankViewModel> BanksFromDB
        {
            get { return _BanksFromDB; }
            set
            {
                if (_BanksFromDB != value)
                {
                    _BanksFromDB = value;
                    NotifyPropertyChanged("BanksFromDB");
                }
            }
        }
        #endregion
                
        #region CurrentBankDG
        private BusinessPartnerBankViewModel _CurrentBankDG;

        public BusinessPartnerBankViewModel CurrentBankDG
        {
            get { return _CurrentBankDG; }
            set
            {
                if (_CurrentBankDG != value)
                {
                    _CurrentBankDG = value;
                    NotifyPropertyChanged("CurrentBankDG");
                }
            }
        }
        #endregion

        #region BankDataLoading
        private bool _BankDataLoading;

        public bool BankDataLoading
        {
            get { return _BankDataLoading; }
            set
            {
                if (_BankDataLoading != value)
                {
                    _BankDataLoading = value;
                    NotifyPropertyChanged("BankDataLoading");
                }
            }
        }
        #endregion


        #region LocationsFromDB
        private ObservableCollection<BusinessPartnerLocationViewModel> _LocationsFromDB;

        public ObservableCollection<BusinessPartnerLocationViewModel> LocationsFromDB
        {
            get { return _LocationsFromDB; }
            set
            {
                if (_LocationsFromDB != value)
                {
                    _LocationsFromDB = value;
                    NotifyPropertyChanged("LocationsFromDB");
                }
            }
        }
        #endregion
                
        #region CurrentLocationDG
        private BusinessPartnerLocationViewModel _CurrentLocationDG;

        public BusinessPartnerLocationViewModel CurrentLocationDG
        {
            get { return _CurrentLocationDG; }
            set
            {
                if (_CurrentLocationDG != value)
                {
                    _CurrentLocationDG = value;
                    NotifyPropertyChanged("CurrentLocationDG");
                }
            }
        }
        #endregion

        #region LocationDataLoading
        private bool _LocationDataLoading;

        public bool LocationDataLoading
        {
            get { return _LocationDataLoading; }
            set
            {
                if (_LocationDataLoading != value)
                {
                    _LocationDataLoading = value;
                    NotifyPropertyChanged("LocationDataLoading");
                }
            }
        }
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

        #region BusinessPartnerTypeDataLoading
        private bool _BusinessPartnerTypeDataLoading;

        public bool BusinessPartnerTypeDataLoading
        {
            get { return _BusinessPartnerTypeDataLoading; }
            set
            {
                if (_BusinessPartnerTypeDataLoading != value)
                {
                    _BusinessPartnerTypeDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerTypeDataLoading");
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
        private string _BusinessPartnerButtonContent = ((string)Application.Current.FindResource("Sinhronizacija_poslovnih_partnera"));

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


        #region SyncButtonContent
        private string _SyncButtonContent = " OSVEŽI ";

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

        public BusinessPartner_List()
        {
            // Get required service
            this.businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            this.businessPartnerLocationService = DependencyResolver.Kernel.Get<IBusinessPartnerLocationService>();
            this.businessPartnerPhoneService = DependencyResolver.Kernel.Get<IBusinessPartnerPhoneService>();
            this.businessPartnerOrganizationUnitService = DependencyResolver.Kernel.Get<IBusinessPartnerOrganizationUnitService>();
            this.businessPartnerDocumentService = DependencyResolver.Kernel.Get<IBusinessPartnerDocumentService>();
            this.businessPartnerBankService = DependencyResolver.Kernel.Get<IBusinessPartnerBankService>();
            this.businessPartnerInstitutionService = DependencyResolver.Kernel.Get<IBusinessPartnerInstitutionService>();
            this.businessPartnerTypeService = DependencyResolver.Kernel.Get<IBusinessPartnerTypeService>();
            this.businessPartnerNoteService = DependencyResolver.Kernel.Get<IBusinessPartnerNoteService>();

            // Initialize form components
            InitializeComponent();

            this.DataContext = this;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Thread displayThread = new Thread(() => SyncData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void PopulateInitialData()
        {
            if (BusinessPartners_List.ShowBusinessPartnerSerbia)
            {
                Thread displayThread = new Thread(() => SyncData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

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

        private void DisplayNoteData()
        {
			NoteDataLoading = true;

			BusinessPartnerNoteListResponse response = new BusinessPartnerNoteSQLiteRepository()
                .GetBusinessPartnerNotesByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
				NotesFromDB = new ObservableCollection<BusinessPartnerNoteViewModel>(
                    response.BusinessPartnerNotes ?? new List<BusinessPartnerNoteViewModel>());
            }
            else
            {
				NotesFromDB = new ObservableCollection<BusinessPartnerNoteViewModel>();
            }

			NoteDataLoading = false;
        }

        private void DisplayPhoneData()
        {
            PhoneDataLoading = true;

            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneSQLiteRepository()
                .GetBusinessPartnerPhonesByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                PhonesFromDB = new ObservableCollection<BusinessPartnerPhoneViewModel>(
                    response.BusinessPartnerPhones ?? new List<BusinessPartnerPhoneViewModel>());
            }
            else
            {
                PhonesFromDB = new ObservableCollection<BusinessPartnerPhoneViewModel>();
            }

            PhoneDataLoading = false;
        }

        private void DisplayDocumentData()
        {
            BusinessPartnerDocumentDataLoading = true;

            BusinessPartnerDocumentListResponse response = new BusinessPartnerDocumentSQLiteRepository()
                .GetBusinessPartnerDocumentsByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                BusinessPartnerDocumentsFromDB = new ObservableCollection<BusinessPartnerDocumentViewModel>(
                    response.BusinessPartnerDocuments ?? new List<BusinessPartnerDocumentViewModel>());
            }
            else
            {
                BusinessPartnerDocumentsFromDB = new ObservableCollection<BusinessPartnerDocumentViewModel>();
            }

            BusinessPartnerDocumentDataLoading = false;
        }

        private void DisplayLocationData()
        {
            LocationDataLoading = true;

            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationSQLiteRepository()
                .GetBusinessPartnerLocationsByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                LocationsFromDB = new ObservableCollection<BusinessPartnerLocationViewModel>(
                    response.BusinessPartnerLocations ?? new List<BusinessPartnerLocationViewModel>());
            }
            else
            {
                LocationsFromDB = new ObservableCollection<BusinessPartnerLocationViewModel>();
            }

            LocationDataLoading = false;
        }

        private void DisplayInstitutionData()
        {
            InstitutionDataLoading = true;

            BusinessPartnerInstitutionListResponse response = new BusinessPartnerInstitutionSQLiteRepository()
                .GetBusinessPartnerInstitutionsByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                InstitutionsFromDB = new ObservableCollection<BusinessPartnerInstitutionViewModel>(
                    response.BusinessPartnerInstitutions ?? new List<BusinessPartnerInstitutionViewModel>());
            }
            else
            {
                InstitutionsFromDB = new ObservableCollection<BusinessPartnerInstitutionViewModel>();
            }

            InstitutionDataLoading = false;
        }

        private void DisplayBankData()
        {
            BankDataLoading = true;

            BusinessPartnerBankListResponse response = new BusinessPartnerBankSQLiteRepository()
                .GetBusinessPartnerBanksByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                BanksFromDB = new ObservableCollection<BusinessPartnerBankViewModel>(
                    response.BusinessPartnerBanks ?? new List<BusinessPartnerBankViewModel>());
            }
            else
            {
                BanksFromDB = new ObservableCollection<BusinessPartnerBankViewModel>();
            }

           BankDataLoading = false;
        }

        private void DisplayTypeData()
        {
            BusinessPartnerTypeDataLoading = true;

            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeSQLiteRepository()
                .GetBusinessPartnerTypesByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                BusinessPartnerTypesFromDB = new ObservableCollection<BusinessPartnerTypeViewModel>(
                    response.BusinessPartnerTypes ?? new List<BusinessPartnerTypeViewModel>());
            }
            else
            {
                BusinessPartnerTypesFromDB = new ObservableCollection<BusinessPartnerTypeViewModel>();
            }

            BusinessPartnerTypeDataLoading = false;
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Poslovni partner ... ";
            new BusinessPartnerSQLiteRepository().Sync(businessPartnerService, (synced, toSync) => {
                SyncButtonContent = " Poslovni partner (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerNoteSQLiteRepository().Sync(businessPartnerNoteService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerDocumentSQLiteRepository().Sync(businessPartnerDocumentService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerPhoneSQLiteRepository().Sync(businessPartnerPhoneService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerLocationSQLiteRepository().Sync(businessPartnerLocationService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerInstitutionSQLiteRepository().Sync(businessPartnerInstitutionService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerBankSQLiteRepository().Sync(businessPartnerBankService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerTypeSQLiteRepository().Sync(businessPartnerTypeService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayData();
            CurrentBusinessPartner = null;
            NotesFromDB = new ObservableCollection<BusinessPartnerNoteViewModel>();
            BusinessPartnerDocumentsFromDB = new ObservableCollection<BusinessPartnerDocumentViewModel>();
            PhonesFromDB = new ObservableCollection<BusinessPartnerPhoneViewModel>();
            LocationsFromDB = new ObservableCollection<BusinessPartnerLocationViewModel>();
            BanksFromDB = new ObservableCollection<BusinessPartnerBankViewModel>();
            InstitutionsFromDB = new ObservableCollection<BusinessPartnerInstitutionViewModel>();
            BusinessPartnerTypesFromDB = new ObservableCollection<BusinessPartnerTypeViewModel>();
            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncNoteData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerNoteSQLiteRepository().Sync(businessPartnerNoteService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayNoteData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncDocumentData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerDocumentSQLiteRepository().Sync(businessPartnerDocumentService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayDocumentData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncPhoneData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerPhoneSQLiteRepository().Sync(businessPartnerPhoneService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayPhoneData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncLocationData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerLocationSQLiteRepository().Sync(businessPartnerLocationService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayLocationData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncInstitutionData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerInstitutionSQLiteRepository().Sync(businessPartnerInstitutionService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayInstitutionData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncBankData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerBankSQLiteRepository().Sync(businessPartnerBankService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayBankData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncTypeData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new BusinessPartnerTypeSQLiteRepository().Sync(businessPartnerTypeService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayTypeData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;

        }

        private void DgBusinessPartners_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgNotes_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgBusinessPartnerDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgPhones_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgBanks_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgLocations_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgBusinessPartnerTypes_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgInstitutions_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Add, Edit, Delete buttons click

        private void btnAddBusinessPartner_Click(object sender, RoutedEventArgs e)
        {
            BusinessPartnerViewModel businessPartnerViewModel = new BusinessPartnerViewModel();
            businessPartnerViewModel.Identifier = Guid.NewGuid();
           

            BusinessPartner_List_AddEdit addEditForm = new BusinessPartner_List_AddEdit(businessPartnerViewModel, true, false);
            addEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_poslovnim_partnerima")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_izmenuUzvičnik"));
                return;
            }
            BusinessPartner_List_AddEdit addEditForm = new BusinessPartner_List_AddEdit(CurrentBusinessPartner, true);
            addEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_poslovnim_partnerima")), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBusinessPartner == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Morate_odabrati_poslovnog_partnera_za_brisanjeUzvičnik"));
                return;
            }

            // Delete data
            var result = businessPartnerService.Delete(CurrentBusinessPartner.Identifier);
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

        #region Item
        private void BtnAddNote_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Morate odabrati poslosvnog partnera!";
                return;
            }

            #endregion

            BusinessPartner_Note_AddEdit businessPartnerNoteAddEditForm = new BusinessPartner_Note_AddEdit(CurrentBusinessPartner);
            businessPartnerNoteAddEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(SyncNoteData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Napomene")), 95, businessPartnerNoteAddEditForm);
        }

        private void BtnAddDocuments_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Morate odabrati poslovnog partnera!";
                return;
            }

            #endregion

            BusinessPartner_Document_AddEdit businessPartnerDocumentAddEditForm = new BusinessPartner_Document_AddEdit(CurrentBusinessPartner);
            businessPartnerDocumentAddEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(SyncDocumentData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Dokumenti")), 95, businessPartnerDocumentAddEditForm);
        }

        private void BtnAddPhone_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Morate odabrati poslosvnog partnera!";
                return;
            }

            #endregion

            BusinessPartner_Phone_AddEdit businessPartnerPhoneAddEditForm = new BusinessPartner_Phone_AddEdit(CurrentBusinessPartner);
            businessPartnerPhoneAddEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(SyncPhoneData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Telefoni")), 95, businessPartnerPhoneAddEditForm);
        }

        private void BtnAddLocation_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Morate odabrati poslovnog partnera!";
                return;
            }

            #endregion

            BusinessPartner_Location_AddEdit businessPartnerLocationAddEditForm = new BusinessPartner_Location_AddEdit(CurrentBusinessPartner);
            businessPartnerLocationAddEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(SyncLocationData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Lokacije")), 95, businessPartnerLocationAddEditForm);
        }

        private void BtnAddInstitution_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Morate odabrati poslosvnog partnera!";
                return;
            }

            #endregion

            BusinessPartner_Institution_AddEdit businessPartnerInstitutionAddEditForm = new BusinessPartner_Institution_AddEdit(CurrentBusinessPartner);
            businessPartnerInstitutionAddEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(SyncInstitutionData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Institucije")), 95, businessPartnerInstitutionAddEditForm);
        }

        private void BtnAddBank_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Morate odabrati poslovnog partnera!";
                return;
            }

            #endregion

            BusinessPartner_Bank_AddEdit businessPartnerBankAddEditForm = new BusinessPartner_Bank_AddEdit(CurrentBusinessPartner);
            businessPartnerBankAddEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(SyncBankData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Lokacije")), 95, businessPartnerBankAddEditForm);
        }

        private void BtnAddTypes_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Morate odabrati poslovnog partnera!";
                return;
            }

            #endregion

            BusinessPartner_Types_AddEdit businessPartnerTypeAddEditForm = new BusinessPartner_Types_AddEdit(CurrentBusinessPartner);
            businessPartnerTypeAddEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(SyncTypeData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Lokacije")), 95, businessPartnerTypeAddEditForm);
        }
        #endregion

        #region Display documents

        private void btnShowDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentBusinessPartnerDocument.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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

       
		private void btnPrint_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                BusinessPartnerExcelReport.Show(BusinessPartnersFromDB.ToList(), BusinessPartnerLocationsFromDB.ToList());
            }
            catch(Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }

        private void btnBusinessPartnerReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BusinessPartnerReport.Show(CurrentBusinessPartner);
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }
    }
}

