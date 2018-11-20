using Ninject;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Locations;
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

namespace SirmiumERPGFC.Views.Locations
{
    public delegate void MunicipalityHandler();

    public partial class MunicipalityList : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IMunicipalityService municipalityService;
        #endregion


        #region MunicipalitiesFromDB
        private ObservableCollection<MunicipalityViewModel> _MunicipalitiesFromDB;

        public ObservableCollection<MunicipalityViewModel> MunicipalitiesFromDB
        {
            get { return _MunicipalitiesFromDB; }
            set
            {
                if (_MunicipalitiesFromDB != value)
                {
                    _MunicipalitiesFromDB = value;
                    NotifyPropertyChanged("MunicipalitiesFromDB");
                }
            }
        }
        #endregion

        #region CurrentMunicipality
        private MunicipalityViewModel _CurrentMunicipality;

        public MunicipalityViewModel CurrentMunicipality
        {
            get { return _CurrentMunicipality; }
            set
            {
                if (_CurrentMunicipality != value)
                {
                    _CurrentMunicipality = value;
                    NotifyPropertyChanged("CurrentMunicipality");
                }
            }
        }
        #endregion

        #region MunicipalitySearchObject
        private MunicipalityViewModel _MunicipalitySearchObject = new MunicipalityViewModel();

        public MunicipalityViewModel MunicipalitySearchObject
        {
            get { return _MunicipalitySearchObject; }
            set
            {
                if (_MunicipalitySearchObject != value)
                {
                    _MunicipalitySearchObject = value;
                    NotifyPropertyChanged("MunicipalitySearchObject");
                }
            }
        }
        #endregion

        #region MunicipalityDataLoading
        private bool _MunicipalityDataLoading = true;

        public bool MunicipalityDataLoading
        {
            get { return _MunicipalityDataLoading; }
            set
            {
                if (_MunicipalityDataLoading != value)
                {
                    _MunicipalityDataLoading = value;
                    NotifyPropertyChanged("MunicipalityDataLoading");
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

        public MunicipalityList()
        {
            // Get required services
            this.municipalityService = DependencyResolver.Kernel.Get<IMunicipalityService>();

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

                MainWindow.SuccessMessage = "Podaci su uspešno sinhronizovani!";
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
            MunicipalityDataLoading = true;

            MunicipalityListResponse response = new MunicipalitySQLiteRepository()
                .GetMunicipalitiesByPage(MainWindow.CurrentCompanyId, MunicipalitySearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                MunicipalitiesFromDB = new ObservableCollection<MunicipalityViewModel>(response.Municipalities ?? new List<MunicipalityViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                MunicipalitiesFromDB = new ObservableCollection<MunicipalityViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            MunicipalityDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Opštine ... ";
            new MunicipalitySQLiteRepository().Sync(municipalityService);

            DisplayData();

            RefreshButtonContent = " OSVEŽI ";
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MunicipalityViewModel Municipality = new MunicipalityViewModel();
            Municipality.Identifier = Guid.NewGuid();

            MunicipalityAddEdit addEditForm = new MunicipalityAddEdit(Municipality, true);
            addEditForm.MunicipalityCreatedUpdated += new MunicipalityHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o opštinama", 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentMunicipality == null)
            {
                MainWindow.WarningMessage = "Morate odabrati opštinu za izmenu!";
                return;
            }

            MunicipalityAddEdit addEditForm = new MunicipalityAddEdit(CurrentMunicipality, false);
            addEditForm.MunicipalityCreatedUpdated += new MunicipalityHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o opštinama", 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentMunicipality == null)
            {
                MainWindow.WarningMessage = "Morate odabrati opštinu za brisanje!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("opština", CurrentMunicipality.Name + CurrentMunicipality.Code);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                MunicipalityResponse response = municipalityService.Delete(CurrentMunicipality.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new MunicipalitySQLiteRepository().Delete(CurrentMunicipality.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = "Opština je uspešno obrisana!";

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

        #endregion
    }
}

