using Microsoft.Reporting.WinForms;
using Ninject;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.RdlcReports.ConstructionSites;
using SirmiumERPGFC.Reports.ConstructionSites;
using SirmiumERPGFC.Repository.ConstructionSites;
using SirmiumERPGFC.Views.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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


namespace SirmiumERPGFC.Views.ConstructionSites
{
    public delegate void ConstructionSiteHandler();

    public partial class ConstructionSite_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IConstructionSiteService constructionSiteService;
        IConstructionSiteCalculationService constructionSiteCalculationService;
        IConstructionSiteDocumentService constructionSiteDocumentService;
        IConstructionSiteNoteService constructionSiteNoteService;
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
                            DisplayConstructionSiteDocumentData();
                        });
                        th.IsBackground = true;
                        th.Start();
                    }
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


        #region ConstructionSiteNotesFromDB
        private ObservableCollection<ConstructionSiteNoteViewModel> _ConstructionSiteNotesFromDB;

        public ObservableCollection<ConstructionSiteNoteViewModel> ConstructionSiteNotesFromDB
        {
            get { return _ConstructionSiteNotesFromDB; }
            set
            {
                if (_ConstructionSiteNotesFromDB != value)
                {
                    _ConstructionSiteNotesFromDB = value;
                    NotifyPropertyChanged("ConstructionSiteNotesFromDB");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteNoteForm
        private ConstructionSiteNoteViewModel _CurrentConstructionSiteNoteForm = new ConstructionSiteNoteViewModel();

        public ConstructionSiteNoteViewModel CurrentConstructionSiteNoteForm
        {
            get { return _CurrentConstructionSiteNoteForm; }
            set
            {
                if (_CurrentConstructionSiteNoteForm != value)
                {
                    _CurrentConstructionSiteNoteForm = value;
                    NotifyPropertyChanged("CurrentConstructionSiteNoteForm");
                }
            }
        }
        #endregion

        #region ConstructionSiteNoteDataLoading
        private bool _ConstructionSiteNoteDataLoading;

        public bool ConstructionSiteNoteDataLoading
        {
            get { return _ConstructionSiteNoteDataLoading; }
            set
            {
                if (_ConstructionSiteNoteDataLoading != value)
                {
                    _ConstructionSiteNoteDataLoading = value;
                    NotifyPropertyChanged("ConstructionSiteNoteDataLoading");
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
        private string _SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

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

        #region TotalSum
        private decimal _TotalSum = 0;

        public decimal TotalSum
        {
            get { return _TotalSum; }
            set
            {
                if (_TotalSum != value)
                {
                    _TotalSum = value;
                    NotifyPropertyChanged("TotalSum");
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
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
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
                TotalSum = 0;
                foreach (var item in ConstructionSiteCalculationsFromDB)
                {
                    TotalSum += item.NewValue;
                }
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
            ConstructionSiteNoteDataLoading = true;

			ConstructionSiteNoteListResponse response = new ConstructionSiteNoteSQLiteRepository()
				.GetConstructionSiteNotesByConstructionSite(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier);

			if (response.Success)
			{
                ConstructionSiteNotesFromDB = new ObservableCollection<ConstructionSiteNoteViewModel>(response.ConstructionSiteNotes ?? new List<ConstructionSiteNoteViewModel>());
			}
			else
			{
                ConstructionSiteNotesFromDB = new ObservableCollection<ConstructionSiteNoteViewModel>();
				MainWindow.ErrorMessage = response.Message;
			}

            ConstructionSiteNoteDataLoading = false;
		}

		private void DisplayConstructionSiteDocumentData()
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
            SyncButtonEnabled = false;

            SyncButtonContent = " Računi ... ";
            new ConstructionSiteSQLiteRepository().Sync(constructionSiteService, (synced, toSync) => {
                SyncButtonContent = " Računi (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new ConstructionSiteNoteSQLiteRepository().Sync(constructionSiteNoteService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new ConstructionSiteDocumentSQLiteRepository().Sync(constructionSiteDocumentService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new ConstructionSiteCalculationSQLiteRepository().Sync(constructionSiteCalculationService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayData();
            CurrentConstructionSite = null;
            ConstructionSiteNotesFromDB = new ObservableCollection<ConstructionSiteNoteViewModel>();
            ConstructionSiteDocumentsFromDB = new ObservableCollection<ConstructionSiteDocumentViewModel>();
            ConstructionSiteCalculationsFromDB = new ObservableCollection<ConstructionSiteCalculationViewModel>();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncConstructionSiteNoteData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new ConstructionSiteNoteSQLiteRepository().Sync(constructionSiteNoteService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayConstructionSiteNoteData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncConstructionSiteDocumentData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new ConstructionSiteDocumentSQLiteRepository().Sync(constructionSiteDocumentService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayConstructionSiteDocumentData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncConstructionSiteCalculationData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new ConstructionSiteCalculationSQLiteRepository().Sync(constructionSiteCalculationService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayConstructionSiteCalculationData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        #endregion

        private void DgConstructionSites_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgConstructionSiteNotes_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgConstructionSiteDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgConstructionSiteCalculations_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #region Add , edit  and delete 

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

            // Delete data
            var result = constructionSiteService.Delete(CurrentConstructionSite.Identifier);
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

        private void btnShowNoteDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentConstructionSiteNoteForm.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
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

       

        #region Add Items
        private void BtnAddConstructionSiteNote_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentConstructionSite == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Nije_odabrano_gradilišteUzvičnik"));
                return;
            }

            #endregion

            ConstructionSite_Note_AddEdit constructionSiteNoteAddEditForm = new ConstructionSite_Note_AddEdit(CurrentConstructionSite);
            constructionSiteNoteAddEditForm.ConstructionSiteCreatedUpdated += new ConstructionSiteHandler(SyncConstructionSiteNoteData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci")), 95, constructionSiteNoteAddEditForm);
        }

        private void BtnAddConstructionSiteDocument_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentConstructionSite == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Nije_odabrano_gradilišteUzvičnik"));
                return;
            }

            #endregion

            ConstructionSite_Document_AddEdit constructionSiteDocumentAddEditForm = new ConstructionSite_Document_AddEdit(CurrentConstructionSite);
            constructionSiteDocumentAddEditForm.ConstructionSiteCreatedUpdated += new ConstructionSiteHandler(SyncConstructionSiteDocumentData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci")), 95, constructionSiteDocumentAddEditForm);
        }

        private void BtnAddConstructionSiteCalculation_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentConstructionSite == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Nije_odabrano_gradilišteUzvičnik"));
                return;
            }

            #endregion

            ConstructionSite_Calculation_AddEdit outputInvoiceCalculationAddEditForm = new ConstructionSite_Calculation_AddEdit(CurrentConstructionSite);
            outputInvoiceCalculationAddEditForm.ConstructionSiteCreatedUpdated += new ConstructionSiteHandler(SyncConstructionSiteCalculationData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci")), 95, outputInvoiceCalculationAddEditForm);
        }

        #endregion


        #region Excel
        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConstructionSitesExcelReport.Show(ConstructionSitesFromDB.ToList());
            }
            catch (Exception ex)
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
        #endregion
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            ConstructionSite_ReportWindow reportWindow = new ConstructionSite_ReportWindow(CurrentConstructionSite);
            reportWindow.Show();
        }

        private void BtnPrintConstructionSiteReport_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentConstructionSite == null)
            {
                MainWindow.WarningMessage = (string)Application.Current.FindResource("Nije_odabrano_gradilišteUzvičnik");
                return;
            }

            #endregion

            rdlcConstructionSiteReport.LocalReport.DataSources.Clear();

            List<ConstructionSiteReportViewModel> constructionSite = new List<ConstructionSiteReportViewModel>()
            {
                new ConstructionSiteReportViewModel()
                {
                    ConstructionSiteCode = CurrentConstructionSite?.Code ?? "",
                    InternalCode = CurrentConstructionSite?.InternalCode ?? "",
                    Name = CurrentConstructionSite?.Name ?? "",
                    CityName = CurrentConstructionSite?.City?.Name ?? "",
                    CountryName = CurrentConstructionSite?.Country?.Name ?? "",
                    BusinessPartnerName = CurrentConstructionSite?.BusinessPartner?.Name ?? "",
                    StatusName = CurrentConstructionSite?.Status?.Name ?? "",
                    Address = CurrentConstructionSite?.Address ?? "",
                    MaxWorkers = CurrentConstructionSite?.MaxWorkers.ToString() ?? "",
                    ProContractDate = CurrentConstructionSite?.ProContractDate.ToString("dd.MM.yyyy") ?? "",
                    ContractStart = CurrentConstructionSite?.ContractStart.ToString("dd.MM.yyyy") ?? "",
                    ContractExpiration = CurrentConstructionSite?.ContractExpiration.ToString("dd.MM.yyyy") ?? "",
                }
            };

            var rpdsModel = new ReportDataSource()
            {
                Name = "DataSet1",
                Value = constructionSite
            };
            rdlcConstructionSiteReport.LocalReport.DataSources.Add(rpdsModel);

            List<ConstructionSiteNoteViewModel> constructionSiteNotes = new ConstructionSiteNoteSQLiteRepository().GetConstructionSiteNotesByConstructionSite(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier).ConstructionSiteNotes;
            List<ConstructionSiteReportViewModel> notes = new List<ConstructionSiteReportViewModel>();
            int counter = 1;
            foreach (var note in constructionSiteNotes)
            {
                notes.Add(new ConstructionSiteReportViewModel() 
                {
                    NoteOrderNumber = counter++,
                    NoteName = note.Note,
                    NoteDate = note.NoteDate.ToString()
                });
            }

            var rpdsNoteModel = new ReportDataSource()
            {
                Name = "DataSet2",
                Value = notes
            };

            rdlcConstructionSiteReport.LocalReport.DataSources.Add(rpdsNoteModel);

            List<ConstructionSiteDocumentViewModel> constructionSiteDocuments = new ConstructionSiteDocumentSQLiteRepository().GetConstructionSiteDocumentsByConstructionSite(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier).ConstructionSiteDocuments;
            List<ConstructionSiteReportViewModel> documents = new List<ConstructionSiteReportViewModel>();
            counter = 1;
            foreach (var document in constructionSiteDocuments)
            {
                documents.Add(new ConstructionSiteReportViewModel()
                {
                    DocumentOrderNumber = counter++,
                    DocumentName = document.Name,
                    DocumentDate = document.CreateDate.ToString()
                });
            }
                var rpdsDocumentModel = new ReportDataSource()
                {
                    Name = "DataSet3",
                    Value = documents
                };
                rdlcConstructionSiteReport.LocalReport.DataSources.Add(rpdsDocumentModel);
          

            //List<ReportParameter> reportParams = new List<ReportParameter>();
            //string parameterText = "Dana " + (CurrentInputInvoice?.InvoiceDate.ToString("dd.MM.yyyy") ?? "") + " na stočni depo klanice Bioesen primljeno je:";
            //reportParams.Add(new ReportParameter("txtInputInvoiceDate", parameterText));


            //var businessPartnerList = new List<InvoiceBusinessPartnerViewModel>();
            //businessPartnerList.Add(new InvoiceBusinessPartnerViewModel() { Name = "Pera peric " });
            //var businessPartnerModel = new ReportDataSource() { Name = "DataSet2", Value = businessPartnerList };
            //rdlcInputNoteReport.LocalReport.DataSources.Add(businessPartnerModel);


            string exeFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string ContentStart = System.IO.Path.Combine(exeFolder, @"RdlcReports\ConstructionSites\ConstructionSiteReport.rdlc");
            rdlcConstructionSiteReport.LocalReport.ReportPath = ContentStart;
            // rdlcInputInvoiceReport.LocalReport.SetParameters(reportParams);
            rdlcConstructionSiteReport.SetDisplayMode(DisplayMode.PrintLayout);
            rdlcConstructionSiteReport.Refresh();
            rdlcConstructionSiteReport.ZoomMode = ZoomMode.Percent;
            rdlcConstructionSiteReport.ZoomPercent = 100;
            rdlcConstructionSiteReport.RefreshReport();
        }
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
    

    //private void btnAddEmployees_Click(object sender, RoutedEventArgs e)
    //{
    //    if (CurrentConstructionSite == null)
    //    {
    //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Nije_odabrano_gradilišteUzvičnik"));
    //        return;
    //    }

    //    ConstructionSiteCalculationViewModel constructionSiteCalculation = new ConstructionSiteCalculationViewModel();
    //    constructionSiteCalculation.Identifier = Guid.NewGuid();
    //    constructionSiteCalculation.ConstructionSite = CurrentConstructionSite;
    //    constructionSiteCalculation.EmployeePrice = 75M;
    //    constructionSiteCalculation.PlusMinus = "+";

    //    ConstructionSite_List_Calculation_Add addForm = new ConstructionSite_List_Calculation_Add(constructionSiteCalculation);
    //    addForm.ConstructionSiteCalculationCreatedUpdated += new ConstructionSiteCalculationHandler(SyncData);
    //    FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_gradilistima")), 95, addForm);
    //}

    //private void btnRemoveEmployees_Click(object sender, RoutedEventArgs e)
    //{
    //    if (CurrentConstructionSite == null)
    //    {
    //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Nije_odabrano_gradilišteUzvičnik"));
    //        return;
    //    }

    //    ConstructionSiteCalculationViewModel constructionSiteCalculation = new ConstructionSiteCalculationViewModel();
    //    constructionSiteCalculation.Identifier = Guid.NewGuid();
    //    constructionSiteCalculation.ConstructionSite = CurrentConstructionSite;
    //    constructionSiteCalculation.EmployeePrice = 75M;
    //    constructionSiteCalculation.PlusMinus = "-";

    //    ConstructionSite_List_Calculation_Remove removeForm = new ConstructionSite_List_Calculation_Remove(constructionSiteCalculation);
    //    removeForm.ConstructionSiteCalculationCreatedUpdated += new ConstructionSiteCalculationHandler(SyncData);
    //    FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_gradilistima")), 95, removeForm);
    //}


}

