using Ninject;
using ServiceInterfaces.Abstractions.Vats;
using ServiceInterfaces.Messages.Vats;
using ServiceInterfaces.ViewModels.Vats;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Vats;
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

namespace SirmiumERPGFC.Views.Vats
{
    /// <summary>
    /// Interaction logic for Vat_List.xaml
    /// </summary>
    public delegate void VatHandler();
    public partial class Vat_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IVatService VatService;
        #endregion


        #region VatsFromDB
        private ObservableCollection<VatViewModel> _VatsFromDB;

        public ObservableCollection<VatViewModel> VatsFromDB
        {
            get { return _VatsFromDB; }
            set
            {
                if (_VatsFromDB != value)
                {
                    _VatsFromDB = value;
                    NotifyPropertyChanged("VatsFromDB");
                }
            }
        }
        #endregion

        #region CurrentVat
        private VatViewModel _CurrentVat;

        public VatViewModel CurrentVat
        {
            get { return _CurrentVat; }
            set
            {
                if (_CurrentVat != value)
                {
                    _CurrentVat = value;
                    NotifyPropertyChanged("CurrentVat");
                }
            }
        }
        #endregion

        #region VatSearchObject
        private VatViewModel _VatSearchObject = new VatViewModel();

        public VatViewModel VatSearchObject
        {
            get { return _VatSearchObject; }
            set
            {
                if (_VatSearchObject != value)
                {
                    _VatSearchObject = value;
                    NotifyPropertyChanged("VatSearchObject");
                }
            }
        }
        #endregion

        #region VatDataLoading
        private bool _VatDataLoading = true;

        public bool VatDataLoading
        {
            get { return _VatDataLoading; }
            set
            {
                if (_VatDataLoading != value)
                {
                    _VatDataLoading = value;
                    NotifyPropertyChanged("VatDataLoading");
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

        public Vat_List()
        {
            VatService = DependencyResolver.Kernel.Get<IVatService>();

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

        public void DisplayVatData()
        {
            VatDataLoading = true;

            VatListResponse response = new VatSQLiteRepository()
                .GetVatsByPage(MainWindow.CurrentCompanyId, VatSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                VatsFromDB = new ObservableCollection<VatViewModel>(response.Vats ?? new List<VatViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                VatsFromDB = new ObservableCollection<VatViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            VatDataLoading = false;
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " PDV ... ";
            new VatSQLiteRepository().Sync(VatService, (synced, toSync) =>
            {
                if (toSync > 0)
                    SyncButtonContent = " PDV (" + synced + "/" + toSync + ")";
            });

            DisplayVatData();

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

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayVatData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        private void dgVats_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            VatViewModel Vat = new VatViewModel();
            Vat.Identifier = Guid.NewGuid();

            Vat_AddEdit addEditForm = new Vat_AddEdit(Vat, true);
            addEditForm.VatCreatedUpdated += new VatHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, (string)Application.Current.FindResource("Podaci_o_PDV_u"), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVat == null)
            {
                MainWindow.WarningMessage = (string)Application.Current.FindResource("Morate_izabrati_stavku");
                return;
            }

            Vat_AddEdit addEditForm = new Vat_AddEdit(CurrentVat, false);
            addEditForm.VatCreatedUpdated += new VatHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, (string)Application.Current.FindResource("Podaci_o_PDV_u"), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                VatDataLoading = true;

                if (CurrentVat == null)
                {
                    MainWindow.WarningMessage = (string)Application.Current.FindResource("Morate_izabrati_stavku");
                    VatDataLoading = false;
                    return;
                }

                VatResponse response = VatService.Delete(CurrentVat.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = (string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik");
                    VatDataLoading = false;
                    return; 
                }

                response = new VatSQLiteRepository().Delete(CurrentVat.Identifier);
                if (!response.Success) 
                {
                    MainWindow.ErrorMessage = (string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik");
                    VatDataLoading = false;
                    return;
                }
                
                MainWindow.SuccessMessage = (string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik");

                DisplayVatData();

                VatDataLoading = false;
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

                Thread displayThread = new Thread(() => DisplayVatData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => DisplayVatData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => DisplayVatData());
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

                Thread displayThread = new Thread(() => DisplayVatData());
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
                //VatsExcelReport.Show(VatsFromDB.ToList());
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
