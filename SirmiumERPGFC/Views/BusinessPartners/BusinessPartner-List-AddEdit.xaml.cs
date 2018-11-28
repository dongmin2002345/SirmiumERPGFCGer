using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
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
    public partial class BusinessPartner_List_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerService businessPartnerService;
        IBusinessPartnerTypeService businessPartnerTypeService;
        #endregion

        #region Events
        public event BusinessPartnerHandler BusinessPartnerCreatedUpdated;
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

        #region BankAccountDataLoading
        private bool _BankAccountDataLoading;

        public bool BankAccountDataLoading
        {
            get { return _BankAccountDataLoading; }
            set
            {
                if (_BankAccountDataLoading != value)
                {
                    _BankAccountDataLoading = value;
                    NotifyPropertyChanged("BankAccountDataLoading");
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

        #region CurrentPhoneForm
        private BusinessPartnerPhoneViewModel _CurrentPhoneForm = new BusinessPartnerPhoneViewModel();

        public BusinessPartnerPhoneViewModel CurrentPhoneForm
        {
            get { return _CurrentPhoneForm; }
            set
            {
                if (_CurrentPhoneForm != value)
                {
                    _CurrentPhoneForm = value;
                    NotifyPropertyChanged("CurrentPhoneForm");
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

        #region CurrentInstitutionForm
        private BusinessPartnerInstitutionViewModel _CurrentInstitutionForm = new BusinessPartnerInstitutionViewModel();

        public BusinessPartnerInstitutionViewModel CurrentInstitutionForm
        {
            get { return _CurrentInstitutionForm; }
            set
            {
                if (_CurrentInstitutionForm != value)
                {
                    _CurrentInstitutionForm = value;
                    NotifyPropertyChanged("CurrentInstitutionForm");
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

        #region CurrentBankForm
        private BusinessPartnerBankViewModel _CurrentBankForm = new BusinessPartnerBankViewModel();

        public BusinessPartnerBankViewModel CurrentBankForm
        {
            get { return _CurrentBankForm; }
            set
            {
                if (_CurrentBankForm != value)
                {
                    _CurrentBankForm = value;
                    NotifyPropertyChanged("CurrentBankForm");
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

        #region CurrentLocationForm
        private BusinessPartnerLocationViewModel _CurrentLocationForm = new BusinessPartnerLocationViewModel();

        public BusinessPartnerLocationViewModel CurrentLocationForm
        {
            get { return _CurrentLocationForm; }
            set
            {
                if (_CurrentLocationForm != value)
                {
                    _CurrentLocationForm = value;
                    NotifyPropertyChanged("CurrentLocationForm");
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

        #region CurrentBusinessPartnerDocumentForm
        private BusinessPartnerDocumentViewModel _CurrentBusinessPartnerDocumentForm = new BusinessPartnerDocumentViewModel();

        public BusinessPartnerDocumentViewModel CurrentBusinessPartnerDocumentForm
        {
            get { return _CurrentBusinessPartnerDocumentForm; }
            set
            {
                if (_CurrentBusinessPartnerDocumentForm != value)
                {
                    _CurrentBusinessPartnerDocumentForm = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerDocumentForm");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerDocumentDG
        private BusinessPartnerDocumentViewModel _CurrentBusinessPartnerDocumentDG;

        public BusinessPartnerDocumentViewModel CurrentBusinessPartnerDocumentDG
        {
            get { return _CurrentBusinessPartnerDocumentDG; }
            set
            {
                if (_CurrentBusinessPartnerDocumentDG != value)
                {
                    _CurrentBusinessPartnerDocumentDG = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerDocumentDG");
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


        #region BusinessPartnerNotesFromDB
        private ObservableCollection<BusinessPartnerNoteViewModel> _BusinessPartnerNotesFromDB;

        public ObservableCollection<BusinessPartnerNoteViewModel> BusinessPartnerNotesFromDB
        {
            get { return _BusinessPartnerNotesFromDB; }
            set
            {
                if (_BusinessPartnerNotesFromDB != value)
                {
                    _BusinessPartnerNotesFromDB = value;
                    NotifyPropertyChanged("BusinessPartnerNotesFromDB");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerNoteForm
        private BusinessPartnerNoteViewModel _CurrentBusinessPartnerNoteForm = new BusinessPartnerNoteViewModel() { NoteDate = DateTime.Now };

        public BusinessPartnerNoteViewModel CurrentBusinessPartnerNoteForm
        {
            get { return _CurrentBusinessPartnerNoteForm; }
            set
            {
                if (_CurrentBusinessPartnerNoteForm != value)
                {
                    _CurrentBusinessPartnerNoteForm = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerNoteForm");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerNoteDG
        private BusinessPartnerNoteViewModel _CurrentBusinessPartnerNoteDG;

        public BusinessPartnerNoteViewModel CurrentBusinessPartnerNoteDG
        {
            get { return _CurrentBusinessPartnerNoteDG; }
            set
            {
                if (_CurrentBusinessPartnerNoteDG != value)
                {
                    _CurrentBusinessPartnerNoteDG = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerNoteDG");
                }
            }
        }
        #endregion

        #region BusinessPartnerNoteDataLoading
        private bool _BusinessPartnerNoteDataLoading;

        public bool BusinessPartnerNoteDataLoading
        {
            get { return _BusinessPartnerNoteDataLoading; }
            set
            {
                if (_BusinessPartnerNoteDataLoading != value)
                {
                    _BusinessPartnerNoteDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerNoteDataLoading");
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


        #region SubmitButtonContent
        private string _SubmitButtonContent = " PROKNJIŽI ";

        public string SubmitButtonContent
        {
            get { return _SubmitButtonContent; }
            set
            {
                if (_SubmitButtonContent != value)
                {
                    _SubmitButtonContent = value;
                    NotifyPropertyChanged("SubmitButtonContent");
                }
            }
        }
        #endregion

        #region SubmitButtonEnabled
        private bool _SubmitButtonEnabled = true;

        public bool SubmitButtonEnabled
        {
            get { return _SubmitButtonEnabled; }
            set
            {
                if (_SubmitButtonEnabled != value)
                {
                    _SubmitButtonEnabled = value;
                    NotifyPropertyChanged("SubmitButtonEnabled");
                }
            }
        }
        #endregion


        #region IsInPdvOptions
        public ObservableCollection<String> IsInPdvOptions
        {
            get { return new ObservableCollection<String>(new List<string>() { "Da", "Ne" }); }
        }
        #endregion


        #region ItemsEnabled
        private bool _ItemsEnabled;

        public bool ItemsEnabled
        {
            get { return _ItemsEnabled; }
            set
            {
                if (_ItemsEnabled != value)
                {
                    _ItemsEnabled = value;
                    NotifyPropertyChanged("ItemsEnabled");
                }
            }
        }
        #endregion

        #region IsPopup
        private bool _IsPopup;

        public bool IsPopup
        {
            get { return _IsPopup; }
            set
            {
                if (_IsPopup != value)
                {
                    _IsPopup = value;
                    NotifyPropertyChanged("IsPopup");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public BusinessPartner_List_AddEdit(BusinessPartnerViewModel businessPartnerViewModel, bool itemsEnabled, bool isPopup = false)
        {
            this.businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            this.businessPartnerTypeService = DependencyResolver.Kernel.Get<IBusinessPartnerTypeService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentBusinessPartner = businessPartnerViewModel;
            ItemsEnabled = itemsEnabled;
            IsPopup = isPopup;

            Thread displayThread = new Thread(() => {
                PopulateBusinessPartnerTypeData();
                PopulateLocationData();
                PopulateInstitutionData();
                PopulatePhoneData();
                PopulateBankData();
                DisplayDocumentData();
                DisplayBusinessPartnerNoteData();
            });
            displayThread.IsBackground = true;
            displayThread.Start();

        }
        #endregion

        #region Display data

        private void PopulateLocationData()
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
                MainWindow.ErrorMessage = "Greška prilikom učitavanja podataka!";
            }

            LocationDataLoading = false;
        }

        private void PopulateInstitutionData()
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
                MainWindow.ErrorMessage = "Greška prilikom učitavanja podataka!";
            }

            InstitutionDataLoading = false;
        }

        private void PopulatePhoneData()
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
                MainWindow.ErrorMessage = "Greška prilikom učitavanja podataka!";
            }

            PhoneDataLoading = false;
        }

        private void PopulateBankData()
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
                MainWindow.ErrorMessage = "Greška prilikom učitavanja podataka!";
            }

            BankDataLoading = false;
        }

        private void PopulateBusinessPartnerTypeData()
        {
            BusinessPartnerTypeDataLoading = true;

            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeSQLiteRepository()
                .GetBusinessPartnerTypes(MainWindow.CurrentCompanyId);
            if (response.Success)
            {
                BusinessPartnerTypesFromDB = new ObservableCollection<BusinessPartnerTypeViewModel>(
                    response.BusinessPartnerTypes ?? new List<BusinessPartnerTypeViewModel>());

                List<BusinessPartnerTypeViewModel> currentBusinessPartnerType = new BusinessPartnerTypeSQLiteRepository()
                    .GetBusinessPartnerTypesByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier).BusinessPartnerTypes;

                foreach (var businessPartnerType in BusinessPartnerTypesFromDB.Where(x => currentBusinessPartnerType.Select(y => y.Identifier).Contains(x.Identifier)))
                {
                    businessPartnerType.IsSelected = true;
                }
            }
            else
            {
                BusinessPartnerTypesFromDB = new ObservableCollection<BusinessPartnerTypeViewModel>();
                MainWindow.ErrorMessage = "Greška prilikom učitavanja podataka!";
            }

            BusinessPartnerTypeDataLoading = false;
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

        private void DisplayBusinessPartnerNoteData()
        {
            BusinessPartnerNoteDataLoading = true;

            BusinessPartnerNoteListResponse response = new BusinessPartnerNoteSQLiteRepository()
                .GetBusinessPartnerNotesByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                BusinessPartnerNotesFromDB = new ObservableCollection<BusinessPartnerNoteViewModel>(
                    response.BusinessPartnerNotes ?? new List<BusinessPartnerNoteViewModel>());
            }
            else
            {
                BusinessPartnerNotesFromDB = new ObservableCollection<BusinessPartnerNoteViewModel>();
            }

            BusinessPartnerNoteDataLoading = false;
        }

        #endregion

        #region Save header

        private void btnSaveHeader_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentBusinessPartner.Name) && String.IsNullOrEmpty(CurrentBusinessPartner.NameGer))
            {
                MainWindow.WarningMessage = "Obavezno polje: Naziv poslovnog partnera!";
                return;
            }

            #endregion

            var sqLite = new BusinessPartnerSQLiteRepository();
            sqLite.Delete(CurrentBusinessPartner.Identifier);

            CurrentBusinessPartner.UpdatedAt = DateTime.Now;
            var response = sqLite.Create(CurrentBusinessPartner);
            if (response.Success)
            {
                MainWindow.SuccessMessage = "Zaglavlje je uspešno sačuvano";
                ItemsEnabled = true;
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        #endregion

        #region Add, edit, delete and cancel phone

        private void btnPhone_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentPhoneForm.Phone) &&
                String.IsNullOrEmpty(CurrentPhoneForm.Mobile) &&
                String.IsNullOrEmpty(CurrentPhoneForm.Fax) &&
                String.IsNullOrEmpty(CurrentPhoneForm.Email))
            {
                MainWindow.WarningMessage = "Morate uneti osnovne podatke!";
                return;
            }

            #endregion

            // If update process, first delete item
            new BusinessPartnerPhoneSQLiteRepository().Delete(CurrentPhoneForm.Identifier);

            CurrentPhoneForm.BusinessPartner = CurrentBusinessPartner;
            CurrentPhoneForm.Identifier = Guid.NewGuid();

            var response = new BusinessPartnerPhoneSQLiteRepository().Create(CurrentPhoneForm);
            if (response.Success)
            {
                CurrentPhoneForm = new BusinessPartnerPhoneViewModel();

                Thread displayThread = new Thread(() => PopulatePhoneData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtPhone.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnEditPhone_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhoneForm = CurrentPhoneDG;
        }

        private void btnDeletePhone_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("telefon", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new BusinessPartnerPhoneSQLiteRepository().Delete(CurrentPhoneDG.Identifier);

                MainWindow.SuccessMessage = "Telefon je uspešno obrisan!";

                Thread displayThread = new Thread(() => PopulatePhoneData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancelPhone_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhoneForm = new BusinessPartnerPhoneViewModel();
        }

        #endregion

        #region Add, edit, delete and cancel bank
        
        private void btnBank_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBankForm.Country == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Država";
                return;
            }

            if (CurrentBankForm.Bank == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Banka";
                return;
            }

            if (String.IsNullOrEmpty(CurrentBankForm.AccountNumber))
            {
                MainWindow.WarningMessage = "Obavezno polje: Broj računa";
                return;
            }

            #endregion

            // If update process, first delete item
            new BusinessPartnerBankSQLiteRepository().Delete(CurrentBankForm.Identifier);

            CurrentBankForm.BusinessPartner = CurrentBusinessPartner;
            CurrentBankForm.Identifier = Guid.NewGuid();

            var response = new BusinessPartnerBankSQLiteRepository().Create(CurrentBankForm);
            if (response.Success)
            {
                CurrentBankForm = new BusinessPartnerBankViewModel();

                Thread displayThread = new Thread(() => PopulateBankData());
                displayThread.IsBackground = true;
                displayThread.Start();

                popBankCountry.txtCountry.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnEditBank_Click(object sender, RoutedEventArgs e)
        {
            CurrentBankForm = CurrentBankDG;
        }

        private void btnDeleteBank_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("banku", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new BusinessPartnerBankSQLiteRepository().Delete(CurrentBankDG.Identifier);

                MainWindow.SuccessMessage = "Banka je uspešno obrisana!";

                Thread displayThread = new Thread(() => PopulateBankData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancelBank_Click(object sender, RoutedEventArgs e)
        {
            CurrentBankForm = new BusinessPartnerBankViewModel();
        }

        #endregion 

        #region Add, edit, delete and cancel location

        private void btnAddLocation_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentLocationForm.Address == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Adresa!";
                return;
            }

            #endregion

            // If update process, first delete item
            new BusinessPartnerLocationSQLiteRepository().Delete(CurrentLocationForm.Identifier);

            CurrentLocationForm.BusinessPartner = CurrentBusinessPartner;
            CurrentLocationForm.Identifier = Guid.NewGuid();

            var response = new BusinessPartnerLocationSQLiteRepository().Create(CurrentLocationForm);
            if (response.Success)
            {
                CurrentLocationForm = new BusinessPartnerLocationViewModel();

                Thread displayThread = new Thread(() => PopulateLocationData());
                displayThread.IsBackground = true;
                displayThread.Start();

                popCountry.txtCountry.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnEditLocation_Click(object sender, RoutedEventArgs e)
        {
            CurrentLocationForm = CurrentLocationDG;
        }

        private void btnDeleteLocation_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("lokaciju", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new BusinessPartnerLocationSQLiteRepository().Delete(CurrentLocationDG.Identifier);

                MainWindow.SuccessMessage = "Lokacija je uspešno obrisana!";

                Thread displayThread = new Thread(() => PopulateLocationData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancelLocation_Click(object sender, RoutedEventArgs e)
        {
            CurrentLocationForm = new BusinessPartnerLocationViewModel();
        }

        #endregion

        #region Add, edit, delete and cancle organization unit

        private void btnAddInstitution_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentInstitutionForm.Institution == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Naziv!";
                return;
            }

            #endregion

            // If update process, first delete item
            new BusinessPartnerInstitutionSQLiteRepository().Delete(CurrentInstitutionForm.Identifier);

            CurrentInstitutionForm.BusinessPartner = CurrentBusinessPartner;
            CurrentInstitutionForm.Identifier = Guid.NewGuid();

            var response = new BusinessPartnerInstitutionSQLiteRepository().Create(CurrentInstitutionForm);
            if (response.Success)
            {
                CurrentInstitutionForm = new BusinessPartnerInstitutionViewModel();

                Thread displayThread = new Thread(() => PopulateInstitutionData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtInstitution.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnEditInstitution_Click(object sender, RoutedEventArgs e)
        {
            CurrentInstitutionForm = CurrentInstitutionDG;
        }

        private void btnDeleteInstitution_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("organizacionu jedinicu", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new BusinessPartnerInstitutionSQLiteRepository().Delete(CurrentInstitutionDG.Identifier);

                MainWindow.SuccessMessage = "Organizaciona jedinica je uspešno obrisana!";

                Thread displayThread = new Thread(() => PopulateInstitutionData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancelInstitution_Click(object sender, RoutedEventArgs e)
        {
            CurrentInstitutionForm = new BusinessPartnerInstitutionViewModel();
        }

        #endregion

        #region Add, edit, delete and cancel document

        private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
            string[] fileNames = dialog.FileNames;

            if (fileNames.Length > 0)
                CurrentBusinessPartnerDocumentForm.Path = fileNames[0];
        }

        private void btnChooseDocument_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDIalog = new System.Windows.Forms.OpenFileDialog();

            fileDIalog.Multiselect = true;
            fileDIalog.FileOk += FileDIalog_FileOk;
            fileDIalog.Filter = "Image Files | *.pdf";
            fileDIalog.ShowDialog();
        }

        private void btnAddDocument_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentBusinessPartnerDocumentForm.Name))
            {
                MainWindow.WarningMessage = "Obavezno polje: Naziv";
                return;
            }

            if (String.IsNullOrEmpty(CurrentBusinessPartnerDocumentForm.Path))
            {
                MainWindow.WarningMessage = "Obavezno polje: Putanja";
                return;
            }

            if (CurrentBusinessPartnerDocumentForm.CreateDate == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Datum kreiranja";
                return;
            }

            #endregion

            // IF update process, first delete item
            new BusinessPartnerDocumentSQLiteRepository().Delete(CurrentBusinessPartnerDocumentForm.Identifier);

            CurrentBusinessPartnerDocumentForm.BusinessPartner = CurrentBusinessPartner;
            CurrentBusinessPartnerDocumentForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerDocumentForm.IsSynced = false;
            CurrentBusinessPartnerDocumentForm.UpdatedAt = DateTime.Now;
            CurrentBusinessPartnerDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentBusinessPartnerDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new BusinessPartnerDocumentSQLiteRepository().Create(CurrentBusinessPartnerDocumentForm);
            if (response.Success)
            {
                CurrentBusinessPartnerDocumentForm = new BusinessPartnerDocumentViewModel();

                Thread displayThread = new Thread(() => DisplayDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtDocumentName.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerDocumentForm = new BusinessPartnerDocumentViewModel();
        }

        private void btnEditDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerDocumentForm = CurrentBusinessPartnerDocumentDG;
        }

        private void btnDeleteDocument_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("dokument", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new BusinessPartnerDocumentSQLiteRepository().Delete(CurrentBusinessPartnerDocumentDG.Identifier);

                MainWindow.SuccessMessage = "Dokument je uspešno obrisan!";

                Thread displayThread = new Thread(() => DisplayDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        #endregion

        #region Add, edit, delete and cancel note

        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentBusinessPartnerNoteForm.Note))
            {
                MainWindow.WarningMessage = "Obavezno polje: Napomena";
                return;
            }

            if (CurrentBusinessPartnerNoteForm.NoteDate == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Datum napomene";
                return;
            }

            #endregion

            // IF update process, first delete item
            new BusinessPartnerNoteSQLiteRepository().Delete(CurrentBusinessPartnerNoteForm.Identifier);

            CurrentBusinessPartnerNoteForm.BusinessPartner = CurrentBusinessPartner;
            CurrentBusinessPartnerNoteForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerNoteForm.IsSynced = false;
            CurrentBusinessPartnerNoteForm.UpdatedAt = DateTime.Now;
            CurrentBusinessPartnerNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentBusinessPartnerNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new BusinessPartnerNoteSQLiteRepository().Create(CurrentBusinessPartnerNoteForm);
            if (response.Success)
            {
                CurrentBusinessPartnerNoteForm = new BusinessPartnerNoteViewModel();

                Thread displayThread = new Thread(() => DisplayBusinessPartnerNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtNote.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnEditNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerNoteForm = CurrentBusinessPartnerNoteDG;
        }

        private void btnDeleteNote_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku radnika", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new BusinessPartnerNoteSQLiteRepository().Delete(CurrentBusinessPartnerNoteDG.Identifier);

                MainWindow.SuccessMessage = "Stavka radnika je uspešno obrisana!";

                Thread displayThread = new Thread(() => DisplayBusinessPartnerNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerNoteForm = new BusinessPartnerNoteViewModel();
        }

        #endregion

        #region Select type

        private void btnSaveType_Click(object sender, RoutedEventArgs e)
        {
            // Delete connected items
            BusinessPartnerBusinessPartnerTypeSQLiteRepository sqLite = new BusinessPartnerBusinessPartnerTypeSQLiteRepository();
            sqLite.Delete(CurrentBusinessPartner.Identifier);

            // Insert selected items
            bool operationSuccessfull = true;
            foreach (var item in BusinessPartnerTypesFromDB.Where(x => x.IsSelected))
            {
                var response = sqLite.Create(CurrentBusinessPartner.Identifier, item.Identifier);
                operationSuccessfull = operationSuccessfull && response.Success;
            }

            if (operationSuccessfull)
                MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
            else
                MainWindow.ErrorMessage = "Greško kod čuvanja podataka!";
        }

        private void btnCancelType_Click(object sender, RoutedEventArgs e)
        {
            Thread displayThread = new Thread(() => PopulateBusinessPartnerTypeData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Submit and cancel

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartner.Name == null &&
                CurrentBusinessPartner.NameGer == null)
            {
                MainWindow.ErrorMessage = "Obavezno polje: Naziv!";
                SubmitButtonContent = " PROKNJIŽI ";
                SubmitButtonEnabled = true;
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonContent = " Čuvanje u toku... ";
                SubmitButtonEnabled = false;

                CurrentBusinessPartner.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentBusinessPartner.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                CurrentBusinessPartner.UpdatedAt = DateTime.Now;

                CurrentBusinessPartner.Locations = LocationsFromDB;
                CurrentBusinessPartner.Institutions = InstitutionsFromDB;
                CurrentBusinessPartner.Phones = PhonesFromDB;
                CurrentBusinessPartner.Banks = BanksFromDB;
                CurrentBusinessPartner.BusinessPartnerTypes = new ObservableCollection<BusinessPartnerTypeViewModel>(
                    new BusinessPartnerTypeSQLiteRepository()
                    .GetBusinessPartnerTypesByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier).BusinessPartnerTypes
                    ?? new List<BusinessPartnerTypeViewModel>());
                CurrentBusinessPartner.BusinessPartnerDocuments = BusinessPartnerDocumentsFromDB;
                CurrentBusinessPartner.BusinessPartnerNotes = BusinessPartnerNotesFromDB;

                BusinessPartnerResponse response = businessPartnerService.Create(CurrentBusinessPartner);

                if (response.Success)
                {
                    new BusinessPartnerSQLiteRepository().UpdateSyncStatus(CurrentBusinessPartner.Identifier, response.BusinessPartner.Code, response.BusinessPartner.UpdatedAt, response.BusinessPartner.Id, true);
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";

                    BusinessPartnerCreatedUpdated();

                    Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        if (IsPopup)
                            FlyoutHelper.CloseFlyoutPopup(this);
                        else
                            FlyoutHelper.CloseFlyout(this);
                    }));
                }
                else
                {
                    BusinessPartnerCreatedUpdated();

                    MainWindow.ErrorMessage = response.Message;

                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (IsPopup)
                FlyoutHelper.CloseFlyoutPopup(this);
            else
                FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        #region Mouse wheel event 

        private void PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        #endregion

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "") //[CallerMemberName]
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion

        private void btnShowDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentBusinessPartnerDocumentDG.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
