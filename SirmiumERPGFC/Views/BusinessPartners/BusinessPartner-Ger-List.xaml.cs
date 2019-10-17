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
    public partial class BusinessPartner_Ger_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerService businessPartnerService;
        IBusinessPartnerLocationService businessPartnerLocationService;
        IBusinessPartnerPhoneService businessPartnerPhoneService;
        IBusinessPartnerOrganizationUnitService businessPartnerOrganizationUnitService;
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

        public BusinessPartner_Ger_List()
        {
            // Get required service
            this.businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            this.businessPartnerLocationService = DependencyResolver.Kernel.Get<IBusinessPartnerLocationService>();
            this.businessPartnerPhoneService = DependencyResolver.Kernel.Get<IBusinessPartnerPhoneService>();
            this.businessPartnerOrganizationUnitService = DependencyResolver.Kernel.Get<IBusinessPartnerOrganizationUnitService>();
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
            if (BusinessPartners_List.ShowBusinessPartnerGermany)
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

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = ((string)Application.Current.FindResource("Firme_TriTacke"));
            new BusinessPartnerSQLiteRepository().Sync(businessPartnerService);

            DisplayData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
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
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_poslovnim_partnerima")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            BusinessPartner_List_AddEdit addEditForm = new BusinessPartner_List_AddEdit(CurrentBusinessPartner, true);
            addEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(DisplayData);
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

        #region Sync data

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = ((string)Application.Current.FindResource("Firme_TriTacke"));
            new BusinessPartnerSQLiteRepository().Sync(businessPartnerService);

            RefreshButtonContent = ((string)Application.Current.FindResource("Telefoni_TriTacke"));
            new BusinessPartnerPhoneSQLiteRepository().Sync(businessPartnerPhoneService);

            RefreshButtonContent = ((string)Application.Current.FindResource("Lokacije_TriTacke"));
            new BusinessPartnerLocationSQLiteRepository().Sync(businessPartnerLocationService);

            RefreshButtonContent = ((string)Application.Current.FindResource("Banke_TriTacke"));
            new BusinessPartnerBankSQLiteRepository().Sync(businessPartnerBankService);

            RefreshButtonContent = ((string)Application.Current.FindResource("Institucije_TriTacke"));
            new BusinessPartnerInstitutionSQLiteRepository().Sync(businessPartnerInstitutionService);

            RefreshButtonContent = ((string)Application.Current.FindResource("Tipovi_TriTacke"));
            new BusinessPartnerTypeSQLiteRepository().Sync(businessPartnerTypeService);

            RefreshButtonContent = ((string)Application.Current.FindResource("Napomene_TriTacke"));
            new BusinessPartnerNoteSQLiteRepository().Sync(businessPartnerNoteService);

            DisplayData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
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
                BusinessPartnerGerExcelReport.Show(BusinessPartnersFromDB.ToList(), BusinessPartnerLocationsFromDB.ToList());
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }
	}
}
