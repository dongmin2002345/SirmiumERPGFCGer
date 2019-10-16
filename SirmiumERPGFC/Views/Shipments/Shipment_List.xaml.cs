using Ninject;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Messages.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Shipments;
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

namespace SirmiumERPGFC.Views.Shipments
{
    public delegate void ShipmentHandler();
    public partial class Shipment_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IShipmentService ShipmentService;
        IShipmentDocumentService ShipmentDocumentService;
        #endregion

        #region ShipmentSearchObject
        private ShipmentViewModel _ShipmentSearchObject = new ShipmentViewModel();

        public ShipmentViewModel ShipmentSearchObject
        {
            get { return _ShipmentSearchObject; }
            set
            {
                if (_ShipmentSearchObject != value)
                {
                    _ShipmentSearchObject = value;
                    NotifyPropertyChanged("ShipmentSearchObject");
                }
            }
        }
        #endregion

        #region ShipmentsFromDB
        private ObservableCollection<ShipmentViewModel> _ShipmentsFromDB;

        public ObservableCollection<ShipmentViewModel> ShipmentsFromDB
        {
            get { return _ShipmentsFromDB; }
            set
            {
                if (_ShipmentsFromDB != value)
                {
                    _ShipmentsFromDB = value;
                    NotifyPropertyChanged("ShipmentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentShipment
        private ShipmentViewModel _CurrentShipment;

        public ShipmentViewModel CurrentShipment
        {
            get { return _CurrentShipment; }
            set
            {
                if (_CurrentShipment != value)
                {
                    _CurrentShipment = value;
                    NotifyPropertyChanged("CurrentShipment");
                    if (_CurrentShipment != null)
                    {
                        Thread displayItemThread = new Thread(() =>
                        {

                            DisplayShipmentDocumentData();



                        });
                        displayItemThread.IsBackground = true;
                        displayItemThread.Start();
                    }
                    else
                        ShipmentDocumentsFromDB = new ObservableCollection<ShipmentDocumentViewModel>();

                }
            }
        }
        #endregion

        #region ShipmentDataLoading
        private bool _ShipmentDataLoading = true;

        public bool ShipmentDataLoading
        {
            get { return _ShipmentDataLoading; }
            set
            {
                if (_ShipmentDataLoading != value)
                {
                    _ShipmentDataLoading = value;
                    NotifyPropertyChanged("ShipmentDataLoading");
                }
            }
        }
        #endregion

        #region ShipmentDocumentsFromDB
        private ObservableCollection<ShipmentDocumentViewModel> _ShipmentDocumentsFromDB;

        public ObservableCollection<ShipmentDocumentViewModel> ShipmentDocumentsFromDB
        {
            get { return _ShipmentDocumentsFromDB; }
            set
            {
                if (_ShipmentDocumentsFromDB != value)
                {
                    _ShipmentDocumentsFromDB = value;
                    NotifyPropertyChanged("ShipmentDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentShipmentDocument
        private ShipmentDocumentViewModel _CurrentShipmentDocument;

        public ShipmentDocumentViewModel CurrentShipmentDocument
        {
            get { return _CurrentShipmentDocument; }
            set
            {
                if (_CurrentShipmentDocument != value)
                {
                    _CurrentShipmentDocument = value;
                    NotifyPropertyChanged("CurrentShipmentDocument");
                }
            }
        }
        #endregion

        #region ShipmentDocumentDataLoading
        private bool _ShipmentDocumentDataLoading;

        public bool ShipmentDocumentDataLoading
        {
            get { return _ShipmentDocumentDataLoading; }
            set
            {
                if (_ShipmentDocumentDataLoading != value)
                {
                    _ShipmentDocumentDataLoading = value;
                    NotifyPropertyChanged("ShipmentDocumentDataLoading");
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

        /// <summary>
        /// FinancialInvoiceList constructor
        /// </summary>
        public Shipment_List()
        {
            // Get required service
            ShipmentService = DependencyResolver.Kernel.Get<IShipmentService>();

            ShipmentDocumentService = DependencyResolver.Kernel.Get<IShipmentDocumentService>();

            // Draw all components
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
            ShipmentDataLoading = true;

            ShipmentListResponse response = new ShipmentSQLiteRepository()
                .GetShipmentsByPage(MainWindow.CurrentCompanyId, ShipmentSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                ShipmentsFromDB = new ObservableCollection<ShipmentViewModel>(response.Shipments ?? new List<ShipmentViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                ShipmentsFromDB = new ObservableCollection<ShipmentViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            ShipmentDataLoading = false;
        }

       
        private void DisplayShipmentDocumentData()
        {
            ShipmentDocumentDataLoading = true;

            ShipmentDocumentListResponse response = new ShipmentDocumentSQLiteRepository()
                .GetShipmentDocumentsByShipment(MainWindow.CurrentCompanyId, CurrentShipment.Identifier);

            if (response.Success)
            {
                ShipmentDocumentsFromDB = new ObservableCollection<ShipmentDocumentViewModel>(
                    response.ShipmentDocuments ?? new List<ShipmentDocumentViewModel>());
            }
            else
            {
                ShipmentDocumentsFromDB = new ObservableCollection<ShipmentDocumentViewModel>();
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_prilikom_učitavanja_podatakaUzvičnik"));
            }

            ShipmentDocumentDataLoading = false;
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Posiljke ... ";
            new ShipmentSQLiteRepository().Sync(ShipmentService, (synced, toSync) => {
                SyncButtonContent = " Posiljke (" + synced + " / " + toSync + ")... ";
            });

           

            SyncButtonContent = " Stavke ... ";
            new ShipmentDocumentSQLiteRepository().Sync(ShipmentDocumentService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayData();
            CurrentShipment = null;
            ShipmentDocumentsFromDB = new ObservableCollection<ShipmentDocumentViewModel>();
            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        
        private void SyncDocumentData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new ShipmentDocumentSQLiteRepository().Sync(ShipmentDocumentService, (synced, toSync) => {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayShipmentDocumentData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }
        private void DgShipments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

       
        private void DgShipmentDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        #endregion

        #region Add, edit, delete, lock and cancel

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ShipmentViewModel Shipment = new ShipmentViewModel();
            Shipment.Identifier = Guid.NewGuid();
            Shipment.ShipmentDate = DateTime.Now;

            Shipment_AddEdit addEditForm = new Shipment_AddEdit(Shipment, true, false);
            addEditForm.ShipmentCreatedUpdated += new ShipmentHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("ULAZNE_FAKTURE")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentShipment == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Nije_moguće_menjati_ulayne_faktureUzvičnik"));
                return;
            }

            Shipment_AddEdit ShipmentAddEditForm = new Shipment_AddEdit(CurrentShipment, false);
            ShipmentAddEditForm.ShipmentCreatedUpdated += new ShipmentHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("ULAZNE_FAKTURE")), 95, ShipmentAddEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentShipment == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_brisanjeUzvičnik"));
                return;
            }

            // Delete data
            var result = ShipmentService.Delete(CurrentShipment.Identifier);
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

       
        private void BtnAddDocuments_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentShipment == null)
            {
                MainWindow.WarningMessage = "Morate odabrati račun!";
                return;
            }

            #endregion

            Shipment_Document_AddEdit ShipmentDocumentAddEditForm = new Shipment_Document_AddEdit(CurrentShipment);
            ShipmentDocumentAddEditForm.ShipmentCreatedUpdated += new ShipmentHandler(SyncDocumentData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Dokumenti")), 95, ShipmentDocumentAddEditForm);
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
        public void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        

        #region Display documents

        private void btnShowDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentShipmentDocument.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        
    }
}
