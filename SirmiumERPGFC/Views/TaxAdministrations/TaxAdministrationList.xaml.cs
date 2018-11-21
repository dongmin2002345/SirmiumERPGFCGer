using Ninject;
using ServiceInterfaces.Abstractions.Common.TaxAdministrations;
using ServiceInterfaces.Messages.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.TaxAdministrations;
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

namespace SirmiumERPGFC.Views.TaxAdministrations
{
    public delegate void TaxAdministrationHandler();

    public partial class TaxAdministrationList : UserControl, INotifyPropertyChanged
    {

        #region Attributes
        ITaxAdministrationService taxAdministrationService;

        #region TaxAdministrationsFromDB
        private ObservableCollection<TaxAdministrationViewModel> _TaxAdministrationsFromDB;

        public ObservableCollection<TaxAdministrationViewModel> TaxAdministrationsFromDB
        {
            get { return _TaxAdministrationsFromDB; }
            set
            {
                if (_TaxAdministrationsFromDB != value)
                {
                    _TaxAdministrationsFromDB = value;
                    NotifyPropertyChanged("TaxAdministrationsFromDB");
                }
            }
        }
        #endregion

        #region CurrentTaxAdministration
        private TaxAdministrationViewModel _CurrentTaxAdministration;

        public TaxAdministrationViewModel CurrentTaxAdministration
        {
            get { return _CurrentTaxAdministration; }
            set
            {
                if (_CurrentTaxAdministration != value)
                {
                    _CurrentTaxAdministration = value;
                    NotifyPropertyChanged("CurrentTaxAdministration");
                }
            }
        }
        #endregion

        #region TaxAdministrationSearchObject
        private TaxAdministrationViewModel _TaxAdministrationSearchObject = new TaxAdministrationViewModel();

        public TaxAdministrationViewModel TaxAdministrationSearchObject
        {
            get { return _TaxAdministrationSearchObject; }
            set
            {
                if (_TaxAdministrationSearchObject != value)
                {
                    _TaxAdministrationSearchObject = value;
                    NotifyPropertyChanged("TaxAdministrationSearchObject");
                }
            }
        }
        #endregion

        #region TaxAdministrationDataLoading
        private bool _TaxAdministrationDataLoading = true;

        public bool TaxAdministrationDataLoading
        {
            get { return _TaxAdministrationDataLoading; }
            set
            {
                if (_TaxAdministrationDataLoading != value)
                {
                    _TaxAdministrationDataLoading = value;
                    NotifyPropertyChanged("TaxAdministrationDataLoading");
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

        public TaxAdministrationList()
        {
            // Get required services
            this.taxAdministrationService = DependencyResolver.Kernel.Get<ITaxAdministrationService>();

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

                MainWindow.SuccessMessage = "Die Daten wurden erfolgreich synchronisiert!";
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
            TaxAdministrationDataLoading = true;

            TaxAdministrationListResponse response = new TaxAdministrationSQLiteRepository()
                .GetTaxAdministrationsByPage(MainWindow.CurrentCompanyId, TaxAdministrationSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                TaxAdministrationsFromDB = new ObservableCollection<TaxAdministrationViewModel>(response.TaxAdministrations ?? new List<TaxAdministrationViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                TaxAdministrationsFromDB = new ObservableCollection<TaxAdministrationViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " von " + totalItems;

            TaxAdministrationDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Steuerverwaltungen ... ";
            new TaxAdministrationSQLiteRepository().Sync(taxAdministrationService);

            DisplayData();

            RefreshButtonContent = " OSVEŽI ";
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            TaxAdministrationViewModel TaxAdministration = new TaxAdministrationViewModel();
            TaxAdministration.Identifier = Guid.NewGuid();

            TaxAdministrationAddEdit addEditForm = new TaxAdministrationAddEdit(TaxAdministration, true);
            addEditForm.TaxAdministrationCreatedUpdated += new TaxAdministrationHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Daten über die Steuerverwaltungen", 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTaxAdministration == null)
            {
                MainWindow.WarningMessage = "Sie müssen einen Eintrag für die Veränderung auswählen!";
                return;
            }

            TaxAdministrationAddEdit addEditForm = new TaxAdministrationAddEdit(CurrentTaxAdministration, false);
            addEditForm.TaxAdministrationCreatedUpdated += new TaxAdministrationHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Daten über die Steuerverwaltungen", 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTaxAdministration == null)
            {
                MainWindow.WarningMessage = "Sie müssen einen Eintrag fürs Löschen auswählen!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("Eintrag", CurrentTaxAdministration.Code);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                TaxAdministrationResponse response = taxAdministrationService.Delete(CurrentTaxAdministration.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Fehler beim Löschen vom Server!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new TaxAdministrationSQLiteRepository().Delete(CurrentTaxAdministration.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Fehler beim lokalen Löschen!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = "Der Eintrag wurde erfolgreich gelöscht!";

                Thread displayThread = new Thread(() => SyncData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
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

        //private void btnPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    //OutputInvoicesExcelReport.Show(OutputInvoicesFromDB.ToList());
        //}

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }
}
