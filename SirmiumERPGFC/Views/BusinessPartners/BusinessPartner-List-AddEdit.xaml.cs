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


        #region OrganizationUnitsFromDB
        private ObservableCollection<BusinessPartnerOrganizationUnitViewModel> _OrganizationUnitsFromDB;

        public ObservableCollection<BusinessPartnerOrganizationUnitViewModel> OrganizationUnitsFromDB
        {
            get { return _OrganizationUnitsFromDB; }
            set
            {
                if (_OrganizationUnitsFromDB != value)
                {
                    _OrganizationUnitsFromDB = value;
                    NotifyPropertyChanged("OrganizationUnitsFromDB");
                }
            }
        }
        #endregion

        #region CurrentOrganizationUnitForm
        private BusinessPartnerOrganizationUnitViewModel _CurrentOrganizationUnitForm = new BusinessPartnerOrganizationUnitViewModel();

        public BusinessPartnerOrganizationUnitViewModel CurrentOrganizationUnitForm
        {
            get { return _CurrentOrganizationUnitForm; }
            set
            {
                if (_CurrentOrganizationUnitForm != value)
                {
                    _CurrentOrganizationUnitForm = value;
                    NotifyPropertyChanged("CurrentOrganizationUnitForm");
                }
            }
        }
        #endregion

        #region CurrentOrganizationUnitDG
        private BusinessPartnerOrganizationUnitViewModel _CurrentOrganizationUnitDG;

        public BusinessPartnerOrganizationUnitViewModel CurrentOrganizationUnitDG
        {
            get { return _CurrentOrganizationUnitDG; }
            set
            {
                if (_CurrentOrganizationUnitDG != value)
                {
                    _CurrentOrganizationUnitDG = value;
                    NotifyPropertyChanged("CurrentOrganizationUnitDG");
                }
            }
        }
        #endregion

        #region OrganizationUnitDataLoading
        private bool _OrganizationUnitDataLoading;

        public bool OrganizationUnitDataLoading
        {
            get { return _OrganizationUnitDataLoading; }
            set
            {
                if (_OrganizationUnitDataLoading != value)
                {
                    _OrganizationUnitDataLoading = value;
                    NotifyPropertyChanged("OrganizationUnitDataLoading");
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
        private string _SubmitButtonContent = " Proknjiži ";

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
                PopulateOrganizationUnitData();
                PopulatePhoneData();
                PopulateBankData();
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

        private void PopulateOrganizationUnitData()
        {
            OrganizationUnitDataLoading = true;

            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitSQLiteRepository()
                .GetBusinessPartnerOrganizationUnitsByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);
            if (response.Success)
            {
                OrganizationUnitsFromDB = new ObservableCollection<BusinessPartnerOrganizationUnitViewModel>(
                    response.BusinessPartnerOrganizationUnits ?? new List<BusinessPartnerOrganizationUnitViewModel>());
            }
            else
            {
                OrganizationUnitsFromDB = new ObservableCollection<BusinessPartnerOrganizationUnitViewModel>();
                MainWindow.ErrorMessage = "Greška prilikom učitavanja podataka!";
            }

            OrganizationUnitDataLoading = false;
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

        //private void btnAddOrganizationUnit_Click(object sender, RoutedEventArgs e)
        //{
        //    #region Validation

        //    if (CurrentOrganizationUnitForm.Name == null)
        //    {
        //        MainWindow.WarningMessage = "Obavezno polje: Naziv!";
        //        return;
        //    }

        //    #endregion

        //    // If update process, first delete item
        //    new BusinessPartnerOrganizationUnitSQLiteRepository().Delete(CurrentOrganizationUnitForm.Identifier);

        //    CurrentOrganizationUnitForm.BusinessPartner = CurrentBusinessPartner;
        //    CurrentOrganizationUnitForm.Identifier = Guid.NewGuid();

        //    var response = new BusinessPartnerOrganizationUnitSQLiteRepository().Create(CurrentOrganizationUnitForm);
        //    if (response.Success)
        //    {
        //        CurrentOrganizationUnitForm = new BusinessPartnerOrganizationUnitViewModel();

        //        Thread displayThread = new Thread(() => PopulateOrganizationUnitData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();

        //        txtOrganizationUnitCode.Focus();
        //    }
        //    else
        //        MainWindow.ErrorMessage = response.Message;
        //}

        //private void btnEditOrganizationUnit_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentOrganizationUnitForm = CurrentOrganizationUnitDG;
        //}

        //private void btnDeleteOrganizationUnit_Click(object sender, RoutedEventArgs e)
        //{
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("organizacionu jedinicu", "");
        //    var showDialog = deleteConfirmationForm.ShowDialog();
        //    if (showDialog != null && showDialog.Value)
        //    {
        //        new BusinessPartnerOrganizationUnitSQLiteRepository().Delete(CurrentOrganizationUnitDG.Identifier);

        //        MainWindow.SuccessMessage = "Organizaciona jedinica je uspešno obrisana!";

        //        Thread displayThread = new Thread(() => PopulateOrganizationUnitData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }

        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //private void btnCancelOrganizationUnit_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentOrganizationUnitForm = new BusinessPartnerOrganizationUnitViewModel();
        //}

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
                SubmitButtonContent = " Proknjiži ";
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
                //CurrentBusinessPartner.OrganizationUnits = OrganizationUnitsFromDB;
                CurrentBusinessPartner.Phones = PhonesFromDB;
                CurrentBusinessPartner.Banks = BanksFromDB;
                CurrentBusinessPartner.BusinessPartnerTypes = new ObservableCollection<BusinessPartnerTypeViewModel>(
                    new BusinessPartnerTypeSQLiteRepository()
                    .GetBusinessPartnerTypesByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier).BusinessPartnerTypes
                    ?? new List<BusinessPartnerTypeViewModel>());

                BusinessPartnerResponse response = businessPartnerService.Create(CurrentBusinessPartner);

                if (response.Success)
                {
                    new BusinessPartnerSQLiteRepository().UpdateSyncStatus(CurrentBusinessPartner.Identifier, response.BusinessPartner.Id, true);
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";

                    BusinessPartnerCreatedUpdated();

                    Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        FlyoutHelper.CloseFlyout(this);
                    }));
                }
                else
                {
                    MainWindow.ErrorMessage = response.Message;

                    SubmitButtonContent = " Proknjiži ";
                    SubmitButtonEnabled = true;
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
