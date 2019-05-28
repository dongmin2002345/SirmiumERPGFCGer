using Ninject;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Reports.ConstructionSites;
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
    public delegate void ConstructionSiteHandler();
    public delegate void ConstructionSiteCalculationHandler();

    public partial class ConstructionSite_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IConstructionSiteService constructionSiteService;
        IConstructionSiteCalculationService constructionSiteCalculationService;
        IConstructionSiteDocumentService constructionSiteDocumentService;
        IConstructionSiteNoteService constructionSiteNoteService;
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
                        Thread th = new Thread(() => {
                            DisplayConstructionSiteCalculationData();
							DisplayConstructionSiteNoteData();
							DisplayDocumentData();
                        });
                        th.IsBackground = true;
                        th.Start();
                    }
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
        private bool _ConstructionSiteDataLoading = true;

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


        #region ConstructionSiteDocumentsFromDB
        private ObservableCollection<ConstructionSiteDocumentViewModel> _ConstructionSiteDocumentsFromDB;

        public ObservableCollection<ConstructionSiteDocumentViewModel> ConstructionSiteDocumentsFromDB
        {
            get { return _ConstructionSiteDocumentsFromDB; }
            set
            {
                if (_ConstructionSiteDocumentsFromDB != value)
                {
                    _ConstructionSiteDocumentsFromDB = value;
                    NotifyPropertyChanged("ConstructionSiteDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteDocument
        private ConstructionSiteDocumentViewModel _CurrentConstructionSiteDocument;

        public ConstructionSiteDocumentViewModel CurrentConstructionSiteDocument
        {
            get { return _CurrentConstructionSiteDocument; }
            set
            {
                if (_CurrentConstructionSiteDocument != value)
                {
                    _CurrentConstructionSiteDocument = value;
                    NotifyPropertyChanged("CurrentConstructionSiteDocument");
                }
            }
        }
        #endregion

        #region ConstructionSiteDocumentDataLoading
        private bool _ConstructionSiteDocumentDataLoading;

        public bool ConstructionSiteDocumentDataLoading
        {
            get { return _ConstructionSiteDocumentDataLoading; }
            set
            {
                if (_ConstructionSiteDocumentDataLoading != value)
                {
                    _ConstructionSiteDocumentDataLoading = value;
                    NotifyPropertyChanged("ConstructionSiteDocumentDataLoading");
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


        #region ConstructionSiteCalculationsFromDB
        private ObservableCollection<ConstructionSiteCalculationViewModel> _ConstructionSiteCalculationsFromDB;

        public ObservableCollection<ConstructionSiteCalculationViewModel> ConstructionSiteCalculationsFromDB
        {
            get { return _ConstructionSiteCalculationsFromDB; }
            set
            {
                if (_ConstructionSiteCalculationsFromDB != value)
                {
                    _ConstructionSiteCalculationsFromDB = value;
                    NotifyPropertyChanged("ConstructionSiteCalculationsFromDB");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteCalculation
        private ConstructionSiteCalculationViewModel _CurrentConstructionSiteCalculation;

        public ConstructionSiteCalculationViewModel CurrentConstructionSiteCalculation
        {
            get { return _CurrentConstructionSiteCalculation; }
            set
            {
                if (_CurrentConstructionSiteCalculation != value)
                {
                    _CurrentConstructionSiteCalculation = value;
                    NotifyPropertyChanged("CurrentConstructionSiteCalculation");
                }
            }
        }
        #endregion

        #region ConstructionSiteCalculationDataLoading
        private bool _ConstructionSiteCalculationDataLoading;

        public bool ConstructionSiteCalculationDataLoading
        {
            get { return _ConstructionSiteCalculationDataLoading; }
            set
            {
                if (_ConstructionSiteCalculationDataLoading != value)
                {
                    _ConstructionSiteCalculationDataLoading = value;
                    NotifyPropertyChanged("ConstructionSiteCalculationDataLoading");
                }
            }
        }
		#endregion

		#region NotesFromDB
		private ObservableCollection<ConstructionSiteNoteViewModel> _NotesFromDB;

		public ObservableCollection<ConstructionSiteNoteViewModel> NotesFromDB
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

		#region CurrentNoteForm
		private ConstructionSiteNoteViewModel _CurrentNoteForm = new ConstructionSiteNoteViewModel();

		public ConstructionSiteNoteViewModel CurrentNoteForm
		{
			get { return _CurrentNoteForm; }
			set
			{
				if (_CurrentNoteForm != value)
				{
					_CurrentNoteForm = value;
					NotifyPropertyChanged("CurrentNoteForm");
				}
			}
		}
		#endregion

		#region CurrentNoteDG
		private ConstructionSiteNoteViewModel _CurrentNoteDG;

		public ConstructionSiteNoteViewModel CurrentNoteDG
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

        public ConstructionSite_List()
        {
            // Get required services
            constructionSiteService = DependencyResolver.Kernel.Get<IConstructionSiteService>();
            constructionSiteCalculationService = DependencyResolver.Kernel.Get<IConstructionSiteCalculationService>();
            constructionSiteDocumentService = DependencyResolver.Kernel.Get<IConstructionSiteDocumentService>();
            constructionSiteNoteService = DependencyResolver.Kernel.Get<IConstructionSiteNoteService>();

            // Initialize form components
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

            Thread displayThread = new Thread(() => DisplayConstructionSiteData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        public void DisplayConstructionSiteData()
        {
            ConstructionSiteDataLoading = true;

            ConstructionSiteListResponse response = new ConstructionSiteSQLiteRepository()
                .GetConstructionSitesByPage(MainWindow.CurrentCompanyId, ConstructionSiteSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                ConstructionSitesFromDB = new ObservableCollection<ConstructionSiteViewModel>(response.ConstructionSites ?? new List<ConstructionSiteViewModel>());
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



        private void DisplayConstructionSiteCalculationData()
        {
            ConstructionSiteCalculationDataLoading = true;

            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationSQLiteRepository()
                .GetConstructionSiteCalculationsByConstructionSite(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier);

            if (response.Success)
            {
                ConstructionSiteCalculationsFromDB = new ObservableCollection<ConstructionSiteCalculationViewModel>(response.ConstructionSiteCalculations ?? new List<ConstructionSiteCalculationViewModel>());
            }
            else
            {
                ConstructionSiteCalculationsFromDB = new ObservableCollection<ConstructionSiteCalculationViewModel>();
                MainWindow.ErrorMessage = response.Message;
            }

            ConstructionSiteCalculationDataLoading = false;
        }

		private void DisplayConstructionSiteNoteData()
		{
			NoteDataLoading = true;

			ConstructionSiteNoteListResponse response = new ConstructionSiteNoteSQLiteRepository()
				.GetConstructionSiteNotesByConstructionSite(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier);

			if (response.Success)
			{
				NotesFromDB = new ObservableCollection<ConstructionSiteNoteViewModel>(response.ConstructionSiteNotes ?? new List<ConstructionSiteNoteViewModel>());
			}
			else
			{
				NotesFromDB = new ObservableCollection<ConstructionSiteNoteViewModel>();
				MainWindow.ErrorMessage = response.Message;
			}

			NoteDataLoading = false;
		}

		private void DisplayDocumentData()
        {
            ConstructionSiteDocumentDataLoading = true;

            ConstructionSiteDocumentListResponse response = new ConstructionSiteDocumentSQLiteRepository()
                .GetConstructionSiteDocumentsByConstructionSite(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier);

            if (response.Success)
            {
                ConstructionSiteDocumentsFromDB = new ObservableCollection<ConstructionSiteDocumentViewModel>(
                    response.ConstructionSiteDocuments ?? new List<ConstructionSiteDocumentViewModel>());
            }
            else
            {
                ConstructionSiteDocumentsFromDB = new ObservableCollection<ConstructionSiteDocumentViewModel>();
            }

            ConstructionSiteDocumentDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = ((string)Application.Current.FindResource("Gradilišta_TriTacke"));
            new ConstructionSiteSQLiteRepository().Sync(constructionSiteService);

            RefreshButtonContent = ((string)Application.Current.FindResource("Kalkulacije_TriTacke"));
            new ConstructionSiteCalculationSQLiteRepository().Sync(constructionSiteCalculationService);

            RefreshButtonContent = ((string)Application.Current.FindResource("Dokumenti_TriTacke"));
            new ConstructionSiteDocumentSQLiteRepository().Sync(constructionSiteDocumentService);

            RefreshButtonContent = ((string)Application.Current.FindResource("Napomene_TriTacke"));
            new ConstructionSiteNoteSQLiteRepository().Sync(constructionSiteNoteService);

            DisplayConstructionSiteData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add city, edit city and delete city

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ConstructionSiteViewModel constructionSite = new ConstructionSiteViewModel();
            constructionSite.Identifier = Guid.NewGuid();
            constructionSite.ContractStart = DateTime.Now;
            constructionSite.ProContractDate = DateTime.Now;

            ConstructionSite_List_AddEdit addEditForm = new ConstructionSite_List_AddEdit(constructionSite, true);
            addEditForm.ConstructionSiteCreatedUpdated += new ConstructionSiteHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_gradilistima")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentConstructionSite == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_gradiliste_za_izmenuUzvičnik"));
                return;
            }

            ConstructionSite_List_AddEdit addEditForm = new ConstructionSite_List_AddEdit(CurrentConstructionSite, false);
            addEditForm.ConstructionSiteCreatedUpdated += new ConstructionSiteHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_gradilistima")), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentConstructionSite == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_gradiliste_za_brisanjeUzvičnik"));
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("grad", CurrentConstructionSite.Code + " " + CurrentConstructionSite.Name);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                ConstructionSiteResponse response = constructionSiteService.Delete(CurrentConstructionSite.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new ConstructionSiteSQLiteRepository().Delete(CurrentConstructionSite.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Grad_je_uspešno_obrisanUzvičnik"));

                Thread displayThread = new Thread(() => SyncData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }
        #endregion

        #region Display documents

        private void btnShowDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentConstructionSiteDocument.Path, UriKind.RelativeOrAbsolute);
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

                Thread displayThread = new Thread(() => DisplayConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => DisplayConstructionSiteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => DisplayConstructionSiteData());
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

                Thread displayThread = new Thread(() => DisplayConstructionSiteData());
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

        private void btnAddEmployees_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentConstructionSite == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Nije_odabrano_gradilišteUzvičnik"));
                return;
            }

            ConstructionSiteCalculationViewModel constructionSiteCalculation = new ConstructionSiteCalculationViewModel();
            constructionSiteCalculation.Identifier = Guid.NewGuid();
            constructionSiteCalculation.ConstructionSite = CurrentConstructionSite;
            constructionSiteCalculation.EmployeePrice = 75M;
            constructionSiteCalculation.PlusMinus = "+";

            ConstructionSite_List_Calculation_Add addForm = new ConstructionSite_List_Calculation_Add(constructionSiteCalculation);
            addForm.ConstructionSiteCalculationCreatedUpdated += new ConstructionSiteCalculationHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_gradilistima")), 95, addForm);
        }

        private void btnRemoveEmployees_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentConstructionSite == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Nije_odabrano_gradilišteUzvičnik"));
                return;
            }

            ConstructionSiteCalculationViewModel constructionSiteCalculation = new ConstructionSiteCalculationViewModel();
            constructionSiteCalculation.Identifier = Guid.NewGuid();
            constructionSiteCalculation.ConstructionSite = CurrentConstructionSite;
            constructionSiteCalculation.EmployeePrice = 75M;
            constructionSiteCalculation.PlusMinus = "-";

            ConstructionSite_List_Calculation_Remove removeForm = new ConstructionSite_List_Calculation_Remove(constructionSiteCalculation);
            removeForm.ConstructionSiteCalculationCreatedUpdated += new ConstructionSiteCalculationHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_gradilistima")), 95, removeForm);
        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConstructionSitesExcelReport.Show(ConstructionSitesFromDB.ToList());
            }
            catch(Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }
		private void btnConstructionSiteExcel_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ConstructionSiteExcelReport.Show(CurrentConstructionSite);
			}
			catch (Exception ex)
			{
				MainWindow.ErrorMessage = ex.Message;
			}
		}
	}
}
