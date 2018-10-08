using Ninject;
using ServiceInterfaces.Abstractions.Common.Cities;
using ServiceInterfaces.Messages.Common.Cities;
using ServiceInterfaces.ViewModels.Common.Cities;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Cities;
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

        #region CitiesearchObject
        private CityViewModel _CitiesearchObject = new CityViewModel();

        public CityViewModel CitiesearchObject
        {
            get { return _CitiesearchObject; }
            set
            {
                if (_CitiesearchObject != value)
                {
                    _CitiesearchObject = value;
                    NotifyPropertyChanged("CitiesearchObject");
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


        #region CityButtonContent
        private string _CityButtonContent = " Sinhronizacija gradova sa serverom ";

        public string CityButtonContent
        {
            get { return _CityButtonContent; }
            set
            {
                if (_CityButtonContent != value)
                {
                    _CityButtonContent = value;
                    NotifyPropertyChanged("CityButtonContent");
                }
            }
        }
        #endregion

        #region CityButtonEnabled
        private bool _CityButtonEnabled = true;

        public bool CityButtonEnabled
        {
            get { return _CityButtonEnabled; }
            set
            {
                if (_CityButtonEnabled != value)
                {
                    _CityButtonEnabled = value;
                    NotifyPropertyChanged("CityButtonEnabled");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public CityList()
        {
            // Initialize services
            cityService = DependencyResolver.Kernel.Get<ICityService>();

            InitializeComponent();

            this.DataContext = this;

            Thread displayThread = new Thread(() => DisplayData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        private void DisplayData()
        {
            CityDataLoading = true;

            CityListResponse response = new CitySQLiteRepository().GetCitiesByPage(CitiesearchObject, currentPage, itemsPerPage);
            if (response.Success)
            {
                CitiesFromDB = new ObservableCollection<CityViewModel>(response.Cities);
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

        #endregion

        #region Add City, edit City and delete City

        private void btnAddCity_Click(object sender, RoutedEventArgs e)
        {
            CityViewModel City = new CityViewModel();
            City.Code = new CitySQLiteRepository().GetNewCodeValue().ToString();
            City.Identifier = Guid.NewGuid();

            CityAddEdit CityAddEditForm = new CityAddEdit(City, true);
            CityAddEditForm.CityCreatedUpdated += new CityHandler(DisplayData);
            FlyoutHelper.OpenFlyout(this, "Grad", 95, CityAddEditForm);

        }

        private void btnEditCity_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCity == null)
            {
                MainWindow.WarningMessage = "Morate odabrati grad za izmenu!";
                return;
            }

            CityAddEdit CityAddEditForm = new CityAddEdit(CurrentCity, false);
            CityAddEditForm.CityCreatedUpdated += new CityHandler(DisplayData);
            FlyoutHelper.OpenFlyout(this, "Depo", 95, CityAddEditForm);
        }

        private void btnDeleteCity_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCity == null)
            {
                MainWindow.WarningMessage = "Morate odabrati grad za brisanje!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("grad", CurrentCity.Name);
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

                Thread displayThread = new Thread(() => DisplayData());
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

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion
    }
}
