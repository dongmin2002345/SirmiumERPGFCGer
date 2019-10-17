using Ninject;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Prices;
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

namespace SirmiumERPGFC.Views.Prices
{
    /// <summary>
    /// Interaction logic for ServiceDelivery_List.xaml
    /// </summary>
    public delegate void ServiceDeliveryHandler();
    public partial class ServiceDelivery_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IServiceDeliveryService ServiceDeliveryService;
        #endregion


        #region ServiceDeliverysFromDB
        private ObservableCollection<ServiceDeliveryViewModel> _ServiceDeliverysFromDB;

        public ObservableCollection<ServiceDeliveryViewModel> ServiceDeliverysFromDB
        {
            get { return _ServiceDeliverysFromDB; }
            set
            {
                if (_ServiceDeliverysFromDB != value)
                {
                    _ServiceDeliverysFromDB = value;
                    NotifyPropertyChanged("ServiceDeliverysFromDB");
                }
            }
        }
        #endregion

        #region CurrentServiceDelivery
        private ServiceDeliveryViewModel _CurrentServiceDelivery;

        public ServiceDeliveryViewModel CurrentServiceDelivery
        {
            get { return _CurrentServiceDelivery; }
            set
            {
                if (_CurrentServiceDelivery != value)
                {
                    _CurrentServiceDelivery = value;
                    NotifyPropertyChanged("CurrentServiceDelivery");
                }
            }
        }
        #endregion

        #region ServiceDeliverySearchObject
        private ServiceDeliveryViewModel _ServiceDeliverySearchObject = new ServiceDeliveryViewModel();

        public ServiceDeliveryViewModel ServiceDeliverySearchObject
        {
            get { return _ServiceDeliverySearchObject; }
            set
            {
                if (_ServiceDeliverySearchObject != value)
                {
                    _ServiceDeliverySearchObject = value;
                    NotifyPropertyChanged("ServiceDeliverySearchObject");
                }
            }
        }
        #endregion

        #region ServiceDeliveryDataLoading
        private bool _ServiceDeliveryDataLoading;

        public bool ServiceDeliveryDataLoading
        {
            get { return _ServiceDeliveryDataLoading; }
            set
            {
                if (_ServiceDeliveryDataLoading != value)
                {
                    _ServiceDeliveryDataLoading = value;
                    NotifyPropertyChanged("ServiceDeliveryDataLoading");
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
        private string _SyncButtonContent = " OSVEŽI ";

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
        public ServiceDelivery_List()
        {
            ServiceDeliveryService = DependencyResolver.Kernel.Get<IServiceDeliveryService>();

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

                MainWindow.SuccessMessage = "Podaci su uspešno sinhronizovani!";
            });
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayServiceDeliveryData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        public void DisplayServiceDeliveryData()
        {
            ServiceDeliveryDataLoading = true;

            ServiceDeliveryListResponse response = new ServiceDeliverySQLiteRepository()
                .GetServiceDeliverysByPage(MainWindow.CurrentCompanyId, ServiceDeliverySearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                ServiceDeliverysFromDB = new ObservableCollection<ServiceDeliveryViewModel>(response.ServiceDeliverys ?? new List<ServiceDeliveryViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                ServiceDeliverysFromDB = new ObservableCollection<ServiceDeliveryViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            ServiceDeliveryDataLoading = false;
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Kurirska sluzba ... ";
            new ServiceDeliverySQLiteRepository().Sync(ServiceDeliveryService, (synced, toSync) =>
            {
                if (toSync > 0)
                    SyncButtonContent = " Kurirska sluzba (" + synced + "/" + toSync + ")";
            });

            DisplayServiceDeliveryData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }
        private void DgServiceDeliverys_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        #endregion


        #region Add, Edit and delete 
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ServiceDeliveryViewModel ServiceDelivery = new ServiceDeliveryViewModel();
            ServiceDelivery.Identifier = Guid.NewGuid();

            ServiceDelivery_AddEdit addEditForm = new ServiceDelivery_AddEdit(ServiceDelivery, true);
            addEditForm.ServiceDeliveryCreatedUpdated += new ServiceDeliveryHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o popustima", 95, addEditForm);
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentServiceDelivery == null)
            {
                MainWindow.WarningMessage = "Morate odabrati podatak za izmenu!";
                return;
            }

            ServiceDelivery_AddEdit addEditForm = new ServiceDelivery_AddEdit(CurrentServiceDelivery, false);
            addEditForm.ServiceDeliveryCreatedUpdated += new ServiceDeliveryHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o popustima", 95, addEditForm);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                ServiceDeliveryDataLoading = true;

                if (CurrentServiceDelivery == null)
                {
                    MainWindow.WarningMessage = "Morate odabrati podatak za brisanje!";
                    ServiceDeliveryDataLoading = false;
                    return;
                }

                ServiceDeliveryResponse response = ServiceDeliveryService.Delete(CurrentServiceDelivery.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    ServiceDeliveryDataLoading = false;
                    return;
                }

                response = new ServiceDeliverySQLiteRepository().Delete(CurrentServiceDelivery.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                    ServiceDeliveryDataLoading = false;
                    return;
                }

                MainWindow.SuccessMessage = "Podatak je uspešno obrisan!";

                DisplayServiceDeliveryData();

                ServiceDeliveryDataLoading = false;
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

                Thread displayThread = new Thread(() => DisplayServiceDeliveryData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => DisplayServiceDeliveryData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => DisplayServiceDeliveryData());
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

                Thread displayThread = new Thread(() => DisplayServiceDeliveryData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }
        #endregion

        private void btnShowURL_Click(object sender, EventArgs e)
        {
            string websiteName = CurrentServiceDelivery.URL; //or simply write the webiste name instead of textBox1.Text      
            System.Diagnostics.Process.Start("iexplore.exe", websiteName);
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
}
