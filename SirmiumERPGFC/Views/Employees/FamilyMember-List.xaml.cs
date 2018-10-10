using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Employees;
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

namespace SirmiumERPGFC.Views.Employees
{
    public delegate void FamilyMemberHandler();

    public partial class FamilyMember_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IFamilyMemberService familyMemberService;
        #endregion


        #region FamilyMembersFromDB
        private ObservableCollection<FamilyMemberViewModel> _FamilyMembersFromDB;

        public ObservableCollection<FamilyMemberViewModel> FamilyMembersFromDB
        {
            get { return _FamilyMembersFromDB; }
            set
            {
                if (_FamilyMembersFromDB != value)
                {
                    _FamilyMembersFromDB = value;
                    NotifyPropertyChanged("FamilyMembersFromDB");
                }
            }
        }
        #endregion

        #region CurrentFamilyMember
        private FamilyMemberViewModel _CurrentFamilyMember;

        public FamilyMemberViewModel CurrentFamilyMember
        {
            get { return _CurrentFamilyMember; }
            set
            {
                if (_CurrentFamilyMember != value)
                {
                    _CurrentFamilyMember = value;
                    NotifyPropertyChanged("CurrentFamilyMember");
                }
            }
        }
        #endregion

        #region FamilyMemberSearchObject
        private FamilyMemberViewModel _FamilyMemberSearchObject = new FamilyMemberViewModel();

        public FamilyMemberViewModel FamilyMemberSearchObject
        {
            get { return _FamilyMemberSearchObject; }
            set
            {
                if (_FamilyMemberSearchObject != value)
                {
                    _FamilyMemberSearchObject = value;
                    NotifyPropertyChanged("FamilyMemberSearchObject");
                }
            }
        }
        #endregion

        #region FamilyMemberDataLoading
        private bool _FamilyMemberDataLoading = true;

        public bool FamilyMemberDataLoading
        {
            get { return _FamilyMemberDataLoading; }
            set
            {
                if (_FamilyMemberDataLoading != value)
                {
                    _FamilyMemberDataLoading = value;
                    NotifyPropertyChanged("FamilyMemberDataLoading");
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

        public FamilyMember_List()
        {
            // Get required services
            familyMemberService = DependencyResolver.Kernel.Get<IFamilyMemberService>();

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
            FamilyMemberDataLoading = true;

            FamilyMemberListResponse response = new FamilyMemberSQLiteRepository()
                .GetFamilyMembersByPage(MainWindow.CurrentCompanyId, FamilyMemberSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                FamilyMembersFromDB = new ObservableCollection<FamilyMemberViewModel>(response.FamilyMembers ?? new List<FamilyMemberViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                FamilyMembersFromDB = new ObservableCollection<FamilyMemberViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            FamilyMemberDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Gradovi ... ";
            new FamilyMemberSQLiteRepository().Sync(familyMemberService);

            DisplayData();

            RefreshButtonContent = " Osveži ";
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add, edit and delete 

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FamilyMemberViewModel FamilyMember = new FamilyMemberViewModel();
            FamilyMember.Identifier = Guid.NewGuid();

            FamilyMember_List_AddEdit addEditForm = new FamilyMember_List_AddEdit(FamilyMember, true);
            addEditForm.FamilyMemberCreatedUpdated += new FamilyMemberHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o clanovima porodice", 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentFamilyMember == null)
            {
                MainWindow.WarningMessage = "Morate odabrati clana za izmenu!";
                return;
            }

            FamilyMember_List_AddEdit addEditForm = new FamilyMember_List_AddEdit(CurrentFamilyMember, false);
            addEditForm.FamilyMemberCreatedUpdated += new FamilyMemberHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o clanovima porodice", 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentFamilyMember == null)
            {
                MainWindow.WarningMessage = "Morate odabrati clana za brisanje!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("clan", CurrentFamilyMember.Code + " " + CurrentFamilyMember.Name);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                FamilyMemberResponse response = familyMemberService.Delete(CurrentFamilyMember.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new FamilyMemberSQLiteRepository().Delete(CurrentFamilyMember.Identifier);
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
