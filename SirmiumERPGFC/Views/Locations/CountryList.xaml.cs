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
    public delegate void CountryHandler();

    public partial class CountryList : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        ICountryService countryService;
        #endregion


        #region CountriesFromDB
        private ObservableCollection<CountryViewModel> _CountriesFromDB;

        public ObservableCollection<CountryViewModel> CountriesFromDB
        {
            get { return _CountriesFromDB; }
            set
            {
                if (_CountriesFromDB != value)
                {
                    _CountriesFromDB = value;
                    NotifyPropertyChanged("CountriesFromDB");
                }
            }
        }
        #endregion

        #region CurrentCountry
        private CountryViewModel _CurrentCountry;

        public CountryViewModel CurrentCountry
        {
            get { return _CurrentCountry; }
            set
            {
                if (_CurrentCountry != value)
                {
                    _CurrentCountry = value;
                    NotifyPropertyChanged("CurrentCountry");
                }
            }
        }
        #endregion

        #region CountrySearchObject
        private CountryViewModel _CountrySearchObject = new CountryViewModel();

        public CountryViewModel CountrySearchObject
        {
            get { return _CountrySearchObject; }
            set
            {
                if (_CountrySearchObject != value)
                {
                    _CountrySearchObject = value;
                    NotifyPropertyChanged("CountrySearchObject");
                }
            }
        }
        #endregion

        #region CountryDataLoading
        private bool _CountryDataLoading = true;

        public bool CountryDataLoading
        {
            get { return _CountryDataLoading; }
            set
            {
                if (_CountryDataLoading != value)
                {
                    _CountryDataLoading = value;
                    NotifyPropertyChanged("CountryDataLoading");
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
        private string _RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

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

        public CountryList()
        {
            // Get required services
            this.countryService = DependencyResolver.Kernel.Get<ICountryService>();

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
            CountryDataLoading = true;

            CountryListResponse response = new CountrySQLiteRepository()
                .GetCountriesByPage(MainWindow.CurrentCompanyId, CountrySearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                CountriesFromDB = new ObservableCollection<CountryViewModel>(response.Countries ?? new List<CountryViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                CountriesFromDB = new ObservableCollection<CountryViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            CountryDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Drzava ... ";
            new CountrySQLiteRepository().Sync(countryService);

            DisplayData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            CountryViewModel country = new CountryViewModel();
            country.Identifier = Guid.NewGuid();

            CountryAddEdit addEditForm = new CountryAddEdit(country, true);
            addEditForm.CountryCreatedUpdated += new CountryHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o drzavama", 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCountry == null)
            {
                MainWindow.WarningMessage = "Morate odabrati drzavu za izmenu!";
                return;
            }

            CountryAddEdit addEditForm = new CountryAddEdit(CurrentCountry, false);
            addEditForm.CountryCreatedUpdated += new CountryHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o drzavi", 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCountry == null)
            {
                MainWindow.WarningMessage = "Morate odabrati drzavu za brisanje!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("drzava", CurrentCountry.Name + CurrentCountry.Code);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                CountryResponse response = countryService.Delete(CurrentCountry.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new CountrySQLiteRepository().Delete(CurrentCountry.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = "Drzava je uspešno obrisana!";

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
