using Ninject;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.ToDos;
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

namespace SirmiumERPGFC.Views.Home
{
    /// <summary>
    /// Interaction logic for Status_List.xaml
    /// </summary>
    public delegate void ToDoStatusHandler();
    public partial class ToDoStatus_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IToDoStatusService ToDoStatusService;
        #endregion


        #region ToDoStatusesFromDB
        private ObservableCollection<ToDoStatusViewModel> _ToDoStatusesFromDB;

        public ObservableCollection<ToDoStatusViewModel> ToDoStatusesFromDB
        {
            get { return _ToDoStatusesFromDB; }
            set
            {
                if (_ToDoStatusesFromDB != value)
                {
                    _ToDoStatusesFromDB = value;
                    NotifyPropertyChanged("ToDoStatusesFromDB");
                }
            }
        }
        #endregion

        #region CurrentToDoStatus
        private ToDoStatusViewModel _CurrentToDoStatus;

        public ToDoStatusViewModel CurrentToDoStatus
        {
            get { return _CurrentToDoStatus; }
            set
            {
                if (_CurrentToDoStatus != value)
                {
                    _CurrentToDoStatus = value;
                    NotifyPropertyChanged("CurrentToDoStatus");
                }
            }
        }
        #endregion

        #region ToDoStatusSearchObject
        private ToDoStatusViewModel _ToDoStatusSearchObject = new ToDoStatusViewModel();

        public ToDoStatusViewModel ToDoStatusSearchObject
        {
            get { return _ToDoStatusSearchObject; }
            set
            {
                if (_ToDoStatusSearchObject != value)
                {
                    _ToDoStatusSearchObject = value;
                    NotifyPropertyChanged("ToDoStatusSearchObject");
                }
            }
        }
        #endregion

        #region ToDoStatusDataLoading
        private bool _ToDoStatusDataLoading = true;

        public bool ToDoStatusDataLoading
        {
            get { return _ToDoStatusDataLoading; }
            set
            {
                if (_ToDoStatusDataLoading != value)
                {
                    _ToDoStatusDataLoading = value;
                    NotifyPropertyChanged("ToDoStatusDataLoading");
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

        public ToDoStatus_List()
        {
            ToDoStatusService = DependencyResolver.Kernel.Get<IToDoStatusService>();

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

        public void DisplayToDoStatusData()
        {
            ToDoStatusDataLoading = true;

            ToDoStatusListResponse response = new ToDoStatusSQLiteRepository()
                .GetToDoStatusesByPage(MainWindow.CurrentCompanyId, ToDoStatusSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                ToDoStatusesFromDB = new ObservableCollection<ToDoStatusViewModel>(response.ToDoStatuses ?? new List<ToDoStatusViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                ToDoStatusesFromDB = new ObservableCollection<ToDoStatusViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            ToDoStatusDataLoading = false;
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Odradjen Status ... ";
            new ToDoStatusSQLiteRepository().Sync(ToDoStatusService, (synced, toSync) =>
            {
                if (toSync > 0)
                    SyncButtonContent = "odradjeno Status (" + synced + "/" + toSync + ")";
            });

            DisplayToDoStatusData();

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

            Thread displayThread = new Thread(() => DisplayToDoStatusData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        private void dgToDoStatuses_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ToDoStatusViewModel ToDoStatus = new ToDoStatusViewModel();
            ToDoStatus.Identifier = Guid.NewGuid();

            ToDoStatus_AddEdit addEditForm = new ToDoStatus_AddEdit(ToDoStatus, true);
            addEditForm.ToDoStatusCreatedUpdated += new ToDoStatusHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, (string)Application.Current.FindResource("Podaci_o_statusu"), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentToDoStatus == null)
            {
                MainWindow.WarningMessage = (string)Application.Current.FindResource("Morate_izabrati_stavku");
                return;
            }

            ToDoStatus_AddEdit addEditForm = new ToDoStatus_AddEdit(CurrentToDoStatus, false);
            addEditForm.ToDoStatusCreatedUpdated += new ToDoStatusHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, (string)Application.Current.FindResource("Podaci_o_statusu"), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                ToDoStatusDataLoading = true;

                if (CurrentToDoStatus == null)
                {
                    MainWindow.WarningMessage = (string)Application.Current.FindResource("Morate_izabrati_stavku");
                    ToDoStatusDataLoading = false;
                    return;
                }

                ToDoStatusResponse response = ToDoStatusService.Delete(CurrentToDoStatus.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = (string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik");
                    ToDoStatusDataLoading = false;
                    return;
                }

                response = new ToDoStatusSQLiteRepository().Delete(CurrentToDoStatus.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = (string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik");
                    ToDoStatusDataLoading = false;
                    return;
                }

                MainWindow.SuccessMessage = (string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik");

                DisplayToDoStatusData();

                ToDoStatusDataLoading = false;
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

                Thread displayThread = new Thread(() => DisplayToDoStatusData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => DisplayToDoStatusData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => DisplayToDoStatusData());
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

                Thread displayThread = new Thread(() => DisplayToDoStatusData());
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