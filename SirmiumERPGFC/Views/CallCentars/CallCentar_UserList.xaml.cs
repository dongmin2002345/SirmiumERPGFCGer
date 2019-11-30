using Ninject;
using ServiceInterfaces.Abstractions.Common.CallCentars;
using ServiceInterfaces.Messages.Common.CallCentars;
using ServiceInterfaces.ViewModels.Common.CallCentars;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.CallCentars;
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

namespace SirmiumERPGFC.Views.CallCentars
{
    public partial class CallCentar_UserList : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        ICallCentarService CallCentarService;
        #endregion


        #region CallCentarsFromDB
        private ObservableCollection<CallCentarViewModel> _CallCentarsFromDB;

        public ObservableCollection<CallCentarViewModel> CallCentarsFromDB
        {
            get { return _CallCentarsFromDB; }
            set
            {
                if (_CallCentarsFromDB != value)
                {
                    _CallCentarsFromDB = value;
                    NotifyPropertyChanged("CallCentarsFromDB");
                }
            }
        }
        #endregion

        #region CurrentCallCentar
        private CallCentarViewModel _CurrentCallCentar;

        public CallCentarViewModel CurrentCallCentar
        {
            get { return _CurrentCallCentar; }
            set
            {
                if (_CurrentCallCentar != value)
                {
                    _CurrentCallCentar = value;
                    NotifyPropertyChanged("CurrentCallCentar");
                }
            }
        }
        #endregion

        #region CallCentarSearchObject
        private CallCentarViewModel _CallCentarSearchObject = new CallCentarViewModel();

        public CallCentarViewModel CallCentarSearchObject
        {
            get { return _CallCentarSearchObject; }
            set
            {
                if (_CallCentarSearchObject != value)
                {
                    _CallCentarSearchObject = value;
                    NotifyPropertyChanged("CallCentarSearchObject");
                }
            }
        }
        #endregion

        #region CallCentarDataLoading
        private bool _CallCentarDataLoading;

        public bool CallCentarDataLoading
        {
            get { return _CallCentarDataLoading; }
            set
            {
                if (_CallCentarDataLoading != value)
                {
                    _CallCentarDataLoading = value;
                    NotifyPropertyChanged("CallCentarDataLoading");
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
        public CallCentar_UserList()
        {
            CallCentarService = DependencyResolver.Kernel.Get<ICallCentarService>();

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

        #region Display

        private void BtnSync_Click(object sender, RoutedEventArgs e)
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

        //private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    currentPage = 1;

        //    Thread displayThread = new Thread(() => DisplayCallCentarData());
        //    displayThread.IsBackground = true;
        //    displayThread.Start();
        //}

        public void DisplayCallCentarData()
        {
            CallCentarDataLoading = true;

            int userId = (MainWindow.CurrentUser?.Id ?? 0);

            CallCentarListResponse response = new CallCentarSQLiteRepository()
                .GetCallCentarsByPage(MainWindow.CurrentCompanyId, CallCentarSearchObject, currentPage, itemsPerPage, userId);

            if (response.Success)
            {
                CallCentarsFromDB = new ObservableCollection<CallCentarViewModel>(response.CallCentars ?? new List<CallCentarViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                CallCentarsFromDB = new ObservableCollection<CallCentarViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            CallCentarDataLoading = false;
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("CallCentarTritacke"));
            new CallCentarSQLiteRepository().Sync(CallCentarService, (synced, toSync) =>
            {
                if (toSync > 0)
                    SyncButtonContent = ((string)Application.Current.FindResource("CallCentar")) + "(" + synced + "/" + toSync + ")";
            });

            DisplayCallCentarData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }
        private void DgCallCentars_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        #endregion


        #region Add, Edit and delete 
        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                CallCentarDataLoading = true;

                if (CurrentCallCentar == null)
                {
                    //MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_brisanjeUzvičnik"));
                    CallCentarDataLoading = false;
                    return;
                }

                CurrentCallCentar.CheckedDone = true;
                CurrentCallCentar.IsSynced = false;


                CallCentarResponse response = new CallCentarSQLiteRepository().Delete(CurrentCallCentar.Identifier);

                response = new CallCentarSQLiteRepository().Create(CurrentCallCentar);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
                    CallCentarDataLoading = false;
                    return;
                }


                response = CallCentarService.Create(CurrentCallCentar);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik"));
                    CallCentarDataLoading = false;
                    return;
                }

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));

                SyncData();

                CallCentarDataLoading = false;
            });
            th.IsBackground = true;
            th.Start();
        }
        #endregion

        #region Pagination
        private void BtnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage = 1;

                Thread displayThread = new Thread(() => DisplayCallCentarData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => DisplayCallCentarData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => DisplayCallCentarData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void BtnLastPage_Click(object sender, RoutedEventArgs e)
        {
            int lastPage = (int)Math.Ceiling((double)this.totalItems / this.itemsPerPage);
            if (currentPage < lastPage)
            {
                currentPage = lastPage;

                Thread displayThread = new Thread(() => DisplayCallCentarData());
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
    }
}
