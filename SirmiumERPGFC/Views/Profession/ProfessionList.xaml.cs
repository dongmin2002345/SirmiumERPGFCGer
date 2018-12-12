using Ninject;
using ServiceInterfaces.Abstractions.Common.Professions;
using ServiceInterfaces.Messages.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Professions;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Professions;
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

namespace SirmiumERPGFC.Views.Profession
{
    public delegate void ProfessionHandler();
    /// <summary>
    /// Interaction logic for ProfessionList.xaml
    /// </summary>
    public partial class ProfessionList : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IProfessionService professionService;
        #endregion


        #region ProfessionsFromDB
        private ObservableCollection<ProfessionViewModel> _ProfessionsFromDB;

        public ObservableCollection<ProfessionViewModel> ProfessionsFromDB
        {
            get { return _ProfessionsFromDB; }
            set
            {
                if (_ProfessionsFromDB != value)
                {
                    _ProfessionsFromDB = value;
                    NotifyPropertyChanged("ProfessionsFromDB");
                }
            }
        }
        #endregion

        #region CurrentProfession
        private ProfessionViewModel _CurrentProfession;

        public ProfessionViewModel CurrentProfession
        {
            get { return _CurrentProfession; }
            set
            {
                if (_CurrentProfession != value)
                {
                    _CurrentProfession = value;
                    NotifyPropertyChanged("CurrentProfession");
                }
            }
        }
        #endregion

        #region ProfessionSearchObject
        private ProfessionViewModel _ProfessionSearchObject = new ProfessionViewModel();

        public ProfessionViewModel ProfessionSearchObject
        {
            get { return _ProfessionSearchObject; }
            set
            {
                if (_ProfessionSearchObject != value)
                {
                    _ProfessionSearchObject = value;
                    NotifyPropertyChanged("ProfessionSearchObject");
                }
            }
        }
        #endregion

        #region ProfessionDataLoading
        private bool _ProfessionDataLoading = true;

        public bool ProfessionDataLoading
        {
            get { return _ProfessionDataLoading; }
            set
            {
                if (_ProfessionDataLoading != value)
                {
                    _ProfessionDataLoading = value;
                    NotifyPropertyChanged("ProfessionDataLoading");
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

        public ProfessionList()
        {
            // Get required services
            professionService = DependencyResolver.Kernel.Get<IProfessionService>();

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
            ProfessionDataLoading = true;

            ProfessionListResponse response = new ProfessionSQLiteRepository()
                .GetProfessionsByPage(MainWindow.CurrentCompanyId, ProfessionSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                ProfessionsFromDB = new ObservableCollection<ProfessionViewModel>(response.Professions ?? new List<ProfessionViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                ProfessionsFromDB = new ObservableCollection<ProfessionViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            ProfessionDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = ((string)Application.Current.FindResource("Zanimanja_TriTacke"));
            new ProfessionSQLiteRepository().Sync(professionService);

            DisplayData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
        }

        #endregion


        #region Add city, edit city and delete city

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProfessionViewModel profession = new ProfessionViewModel();
            profession.Identifier = Guid.NewGuid();

            ProfessionAddEdit addEditForm = new ProfessionAddEdit(profession, true);
            addEditForm.ProfessionCreatedUpdated += new ProfessionHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_zanimanjima")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProfession == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_zanimanje_za_izmenuUzvičnik"));
                return;
            }

            ProfessionAddEdit addEditForm = new ProfessionAddEdit(CurrentProfession, false);
            addEditForm.ProfessionCreatedUpdated += new ProfessionHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_zanimanjima")), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProfession == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_zanimanje_za_brisanjeUzvičnik"));
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("grad", CurrentProfession.SecondCode + " " + CurrentProfession.Name);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                ProfessionResponse response = professionService.Delete(CurrentProfession.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new ProfessionSQLiteRepository().Delete(CurrentProfession.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Grad_je_uspešno_obrisanUzvičnik"));

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
