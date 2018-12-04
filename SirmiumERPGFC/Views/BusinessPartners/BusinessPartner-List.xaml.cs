﻿using Ninject;
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
		private ObservableCollection<BusinessPartnerLocationViewModel> _BusinessPartnerLocationsFromDB;

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
        private string _RefreshButtonContent = " OSVEŽI ";

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

            var response = new BusinessPartnerSQLiteRepository().GetSerbianBusinessPartnersByPage(MainWindow.CurrentCompanyId, BusinessPartnerSearchObject, currentPage, itemsPerPage);
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

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Firme ... ";
            new BusinessPartnerSQLiteRepository().Sync(businessPartnerService);

            RefreshButtonContent = " Telefoni ... ";
            new BusinessPartnerPhoneSQLiteRepository().Sync(businessPartnerPhoneService);

            RefreshButtonContent = " Lokacije ... ";
            new BusinessPartnerLocationSQLiteRepository().Sync(businessPartnerLocationService);

            RefreshButtonContent = " Banke ... ";
            new BusinessPartnerBankSQLiteRepository().Sync(businessPartnerBankService);

            RefreshButtonContent = " Institucije ... ";
            new BusinessPartnerInstitutionSQLiteRepository().Sync(businessPartnerInstitutionService);

            RefreshButtonContent = " Dokumenti ... ";
            new BusinessPartnerDocumentSQLiteRepository().Sync(businessPartnerDocumentService);

            RefreshButtonContent = " Tipovi ... ";
            new BusinessPartnerTypeSQLiteRepository().Sync(businessPartnerTypeService);

            DisplayData();

            RefreshButtonContent = " OSVEŽI ";
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add, Edit, Delete buttons click

        private void btnAddBusinessPartner_Click(object sender, RoutedEventArgs e)
        {
            BusinessPartnerViewModel businessPartnerViewModel = new BusinessPartnerViewModel();
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
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Firme ... ";
            new BusinessPartnerSQLiteRepository().Sync(businessPartnerService);

            RefreshButtonContent = " Telefoni ... ";
            new BusinessPartnerPhoneSQLiteRepository().Sync(businessPartnerPhoneService);

            RefreshButtonContent = " Lokacije ... ";
            new BusinessPartnerLocationSQLiteRepository().Sync(businessPartnerLocationService);

            RefreshButtonContent = " Banke ... ";
            new BusinessPartnerBankSQLiteRepository().Sync(businessPartnerBankService);

            RefreshButtonContent = " Institucije ... ";
            new BusinessPartnerInstitutionSQLiteRepository().Sync(businessPartnerInstitutionService);

            RefreshButtonContent = " Tipovi ... ";
            new BusinessPartnerTypeSQLiteRepository().Sync(businessPartnerTypeService);

            RefreshButtonContent = " Dokumenti ... ";
            new BusinessPartnerDocumentSQLiteRepository().Sync(businessPartnerDocumentService);

            RefreshButtonContent = " Napomene ... ";
            new BusinessPartnerNoteSQLiteRepository().Sync(businessPartnerNoteService);



            DisplayData();

            RefreshButtonContent = " OSVEŽI ";
            RefreshButtonEnabled = true;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateInitialData();
        }
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
	}
}

