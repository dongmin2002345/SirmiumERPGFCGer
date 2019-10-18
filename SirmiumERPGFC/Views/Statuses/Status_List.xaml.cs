using Ninject;
using ServiceInterfaces.Abstractions;
using ServiceInterfaces.Abstractions.Statuses;
using ServiceInterfaces.Messages.Statuses;
using ServiceInterfaces.ViewModels.Statuses;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Statuses;
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

namespace SirmiumERPGFC.Views.Statuses
{
    /// <summary>
    /// Interaction logic for Status_List.xaml
    /// </summary>
    public delegate void StatusHandler();
    public partial class Status_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IStatusService StatusService;
        #endregion


        #region StatusesFromDB
        private ObservableCollection<StatusViewModel> _StatusesFromDB;

        public ObservableCollection<StatusViewModel> StatusesFromDB
        {
            get { return _StatusesFromDB; }
            set
            {
                if (_StatusesFromDB != value)
                {
                    _StatusesFromDB = value;
                    NotifyPropertyChanged("StatusesFromDB");
                }
            }
        }
        #endregion

        #region CurrentStatus
        private StatusViewModel _CurrentStatus;

        public StatusViewModel CurrentStatus
        {
            get { return _CurrentStatus; }
            set
            {
                if (_CurrentStatus != value)
                {
                    _CurrentStatus = value;
                    NotifyPropertyChanged("CurrentStatus");
                }
            }
        }
        #endregion

        #region StatusSearchObject
        private StatusViewModel _StatusSearchObject = new StatusViewModel();

        public StatusViewModel StatusSearchObject
        {
            get { return _StatusSearchObject; }
            set
            {
                if (_StatusSearchObject != value)
                {
                    _StatusSearchObject = value;
                    NotifyPropertyChanged("StatusSearchObject");
                }
            }
        }
        #endregion

        #region StatusDataLoading
        private bool _StatusDataLoading = true;

        public bool StatusDataLoading
        {
            get { return _StatusDataLoading; }
            set
            {
                if (_StatusDataLoading != value)
                {
                    _StatusDataLoading = value;
                    NotifyPropertyChanged("StatusDataLoading");
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

        #endregion

        #region Constructor

        public Status_List()
        {
            StatusService = DependencyResolver.Kernel.Get<IStatusService>();

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

        public void DisplayStatusData()
        {
            StatusDataLoading = true;

            StatusListResponse response = new StatusSQLiteRepository()
                .GetStatusesByPage(MainWindow.CurrentCompanyId, StatusSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                StatusesFromDB = new ObservableCollection<StatusViewModel>(response.Statuses ?? new List<StatusViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                StatusesFromDB = new ObservableCollection<StatusViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            StatusDataLoading = false;
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Status ... ";
            new StatusSQLiteRepository().Sync(StatusService, (synced, toSync) =>
            {
                if (toSync > 0)
                    SyncButtonContent = " Status (" + synced + "/" + toSync + ")";
            });

            DisplayStatusData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void btnSync_Click(object sender, RoutedEventArgs e)
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

        private void txtSearchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayStatusData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        private void dgStatuses_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            StatusViewModel Status = new StatusViewModel();
            Status.Identifier = Guid.NewGuid();

            Status_AddEdit addEditForm = new Status_AddEdit(Status, true);
            addEditForm.StatusCreatedUpdated += new StatusHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, (string)Application.Current.FindResource("Podaci_o_statusu"), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentStatus == null)
            {
                MainWindow.WarningMessage = (string)Application.Current.FindResource("Morate_izabrati_stavku");
                return;
            }

            Status_AddEdit addEditForm = new Status_AddEdit(CurrentStatus, false);
            addEditForm.StatusCreatedUpdated += new StatusHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, (string)Application.Current.FindResource("Podaci_o_statusu"), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                StatusDataLoading = true;

                if (CurrentStatus == null)
                {
                    MainWindow.WarningMessage = (string)Application.Current.FindResource("Morate_izabrati_stavku");
                    StatusDataLoading = false;
                    return;
                }

                StatusResponse response = StatusService.Delete(CurrentStatus.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = (string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik");
                    StatusDataLoading = false;
                    return;
                }

                response = new StatusSQLiteRepository().Delete(CurrentStatus.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = (string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik");
                    StatusDataLoading = false;
                    return;
                }

                MainWindow.SuccessMessage = (string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik");

                DisplayStatusData();

                StatusDataLoading = false;
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

                Thread displayThread = new Thread(() => DisplayStatusData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => DisplayStatusData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => DisplayStatusData());
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

                Thread displayThread = new Thread(() => DisplayStatusData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        #endregion

        #region Export to excel 

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //StatusesExcelReport.Show(StatusesFromDB.ToList());
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
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