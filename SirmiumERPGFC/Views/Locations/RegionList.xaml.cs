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
    public delegate void RegionHandler();

    public partial class RegionList : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IRegionService regionService;
        #endregion


        #region RegionsFromDB
        private ObservableCollection<RegionViewModel> _RegionsFromDB;

        public ObservableCollection<RegionViewModel> RegionsFromDB
        {
            get { return _RegionsFromDB; }
            set
            {
                if (_RegionsFromDB != value)
                {
                    _RegionsFromDB = value;
                    NotifyPropertyChanged("RegionsFromDB");
                }
            }
        }
        #endregion

        #region CurrentRegion
        private RegionViewModel _CurrentRegion;

        public RegionViewModel CurrentRegion
        {
            get { return _CurrentRegion; }
            set
            {
                if (_CurrentRegion != value)
                {
                    _CurrentRegion = value;
                    NotifyPropertyChanged("CurrentRegion");
                }
            }
        }
        #endregion

        #region RegionSearchObject
        private RegionViewModel _RegionSearchObject = new RegionViewModel();

        public RegionViewModel RegionSearchObject
        {
            get { return _RegionSearchObject; }
            set
            {
                if (_RegionSearchObject != value)
                {
                    _RegionSearchObject = value;
                    NotifyPropertyChanged("RegionSearchObject");
                }
            }
        }
        #endregion

        #region RegionDataLoading
        private bool _RegionDataLoading = true;

        public bool RegionDataLoading
        {
            get { return _RegionDataLoading; }
            set
            {
                if (_RegionDataLoading != value)
                {
                    _RegionDataLoading = value;
                    NotifyPropertyChanged("RegionDataLoading");
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
        private string _RefreshButtonContent = " Osveži ";

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

        public RegionList()
        {
            // Get required services
            this.regionService = DependencyResolver.Kernel.Get<IRegionService>();

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
            RegionDataLoading = true;

            RegionListResponse response = new RegionSQLiteRepository()
                .GetRegionsByPage(MainWindow.CurrentCompanyId, RegionSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                RegionsFromDB = new ObservableCollection<RegionViewModel>(response.Regions ?? new List<RegionViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                RegionsFromDB = new ObservableCollection<RegionViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            RegionDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Regioni ... ";
            new RegionSQLiteRepository().Sync(regionService);

            DisplayData();

            RefreshButtonContent = " Osveži ";
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            RegionViewModel region = new RegionViewModel();
            region.Identifier = Guid.NewGuid();

            RegionAddEdit addEditForm = new RegionAddEdit(region, true);
            addEditForm.RegionCreatedUpdated += new RegionHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o regionu", 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentRegion == null)
            {
                MainWindow.WarningMessage = "Morate odabrati region za izmenu!";
                return;
            }

            RegionAddEdit addEditForm = new RegionAddEdit(CurrentRegion, false);
            addEditForm.RegionCreatedUpdated += new RegionHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o regionima", 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentRegion == null)
            {
                MainWindow.WarningMessage = "Morate odabrati region za brisanje!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("region", CurrentRegion.Name + CurrentRegion.Code);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                RegionResponse response = regionService.Delete(CurrentRegion.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new RegionSQLiteRepository().Delete(CurrentRegion.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = " Region je uspešno obrisan!";

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