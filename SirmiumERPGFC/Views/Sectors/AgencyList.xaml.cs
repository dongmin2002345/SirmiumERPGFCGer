using Ninject;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Sectors;
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

namespace SirmiumERPGFC.Views.Sectors
{
    public delegate void AgencyHandler();

    public partial class AgencyList : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IAgencyService AgencyService;
        #endregion

        #region AgenciesFromDB
        private ObservableCollection<AgencyViewModel> _AgenciesFromDB;

        public ObservableCollection<AgencyViewModel> AgenciesFromDB
        {
            get { return _AgenciesFromDB; }
            set
            {
                if (_AgenciesFromDB != value)
                {
                    _AgenciesFromDB = value;
                    NotifyPropertyChanged("AgenciesFromDB");
                }
            }
        }
        #endregion

        #region CurrentAgency
        private AgencyViewModel _CurrentAgency;

        public AgencyViewModel CurrentAgency
        {
            get { return _CurrentAgency; }
            set
            {
                if (_CurrentAgency != value)
                {
                    _CurrentAgency = value;
                    NotifyPropertyChanged("CurrentAgency");
                }
            }
        }
        #endregion

        #region AgencySearchObject
        private AgencyViewModel _AgencySearchObject = new AgencyViewModel();

        public AgencyViewModel AgencySearchObject
        {
            get { return _AgencySearchObject; }
            set
            {
                if (_AgencySearchObject != value)
                {
                    _AgencySearchObject = value;
                    NotifyPropertyChanged("AgencySearchObject");
                }
            }
        }
        #endregion

        #region AgencyDataLoading
        private bool _AgencyDataLoading = true;

        public bool AgencyDataLoading
        {
            get { return _AgencyDataLoading; }
            set
            {
                if (_AgencyDataLoading != value)
                {
                    _AgencyDataLoading = value;
                    NotifyPropertyChanged("AgencyDataLoading");
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

        public AgencyList()
        {
            // Get required services
            AgencyService = DependencyResolver.Kernel.Get<IAgencyService>();

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
            AgencyDataLoading = true;

            AgencyListResponse response = new AgencySQLiteRepository()
                .GetAgenciesByPage(MainWindow.CurrentCompanyId, AgencySearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                AgenciesFromDB = new ObservableCollection<AgencyViewModel>(response.Agencies ?? new List<AgencyViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                AgenciesFromDB = new ObservableCollection<AgencyViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            AgencyDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Delatnosti ... ";
            new AgencySQLiteRepository().Sync(AgencyService);

            DisplayData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add Agency, edit Agency and delete Agency

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AgencyViewModel Agency = new AgencyViewModel();
            Agency.Identifier = Guid.NewGuid();

            AgencyAddEdit addEditForm = new AgencyAddEdit(Agency, true);
            addEditForm.AgencyCreatedUpdated += new AgencyHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o delatnostima", 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAgency == null)
            {
                MainWindow.WarningMessage = "Morate odabrati delatnost za izmenu!";
                return;
            }

            AgencyAddEdit addEditForm = new AgencyAddEdit(CurrentAgency, false);
            addEditForm.AgencyCreatedUpdated += new AgencyHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o delatnostima", 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAgency == null)
            {
                MainWindow.WarningMessage = "Morate odabrati delatnost za brisanje!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("delatnost", CurrentAgency.Code + " " + CurrentAgency.Name);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                AgencyResponse response = AgencyService.Delete(CurrentAgency.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new AgencySQLiteRepository().Delete(CurrentAgency.Identifier);
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
