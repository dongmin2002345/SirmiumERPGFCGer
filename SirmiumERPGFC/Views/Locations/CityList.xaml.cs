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
    public delegate void CityHandler();

    public partial class CityList : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        ICityService cityService;
        #endregion


        #region CitiesFromDB
        private ObservableCollection<CityViewModel> _CitiesFromDB;

        public ObservableCollection<CityViewModel> CitiesFromDB
        {
            get { return _CitiesFromDB; }
            set
            {
                if (_CitiesFromDB != value)
                {
                    _CitiesFromDB = value;
                    NotifyPropertyChanged("CitiesFromDB");
                }
            }
        }
        #endregion

        #region CurrentCity
        private CityViewModel _CurrentCity;

        public CityViewModel CurrentCity
        {
            get { return _CurrentCity; }
            set
            {
                if (_CurrentCity != value)
                {
                    _CurrentCity = value;
                    NotifyPropertyChanged("CurrentCity");
                }
            }
        }
        #endregion

        #region CitySearchObject
        private CityViewModel _CitySearchObject = new CityViewModel();

        public CityViewModel CitySearchObject
        {
            get { return _CitySearchObject; }
            set
            {
                if (_CitySearchObject != value)
                {
                    _CitySearchObject = value;
                    NotifyPropertyChanged("CitySearchObject");
                }
            }
        }
        #endregion

        #region CityDataLoading
        private bool _CityDataLoading = true;

        public bool CityDataLoading
        {
            get { return _CityDataLoading; }
            set
            {
                if (_CityDataLoading != value)
                {
                    _CityDataLoading = value;
                    NotifyPropertyChanged("CityDataLoading");
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

        public CityList()
        {
            // Get required services
            cityService = DependencyResolver.Kernel.Get<ICityService>();

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
            CityDataLoading = true;

            CityListResponse response = new CitySQLiteRepository()
                .GetCitiesByPage(MainWindow.CurrentCompanyId, CitySearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                CitiesFromDB = new ObservableCollection<CityViewModel>(response.Cities ?? new List<CityViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                CitiesFromDB = new ObservableCollection<CityViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            CityDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Gradovi ... ";
            new CitySQLiteRepository().Sync(cityService);

            DisplayData();

            RefreshButtonContent = " OSVEŽI ";
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add city, edit city and delete city

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CityViewModel city = new CityViewModel();
            city.Identifier = Guid.NewGuid();

            CityAddEdit addEditForm = new CityAddEdit(city, true);
            addEditForm.CityCreatedUpdated += new CityHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o gradovima", 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCity == null)
            {
                MainWindow.WarningMessage = "Morate odabrati grad za izmenu!";
                return;
            }

            CityAddEdit addEditForm = new CityAddEdit(CurrentCity, false);
            addEditForm.CityCreatedUpdated += new CityHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o lekovima", 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCity == null)
            {
                MainWindow.WarningMessage = "Morate odabrati grad za brisanje!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("grad", CurrentCity.ZipCode + " " + CurrentCity.Name);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                CityResponse response = cityService.Delete(CurrentCity.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new CitySQLiteRepository().Delete(CurrentCity.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = "Grad je uspešno obrisan!";

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
