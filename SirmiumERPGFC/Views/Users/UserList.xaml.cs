using Ninject;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Companies;
using SirmiumERPGFC.Repository.Users;
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

namespace SirmiumERPGFC.Views.Users
{
    public delegate void UserHandler();

    public partial class UserList : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IUserService userService;
        ICompanyService companyService;
        ICompanyUserService compUserService;
        #endregion

        #region CurrentUser
        private UserViewModel _CurrentUser;

        public UserViewModel CurrentUser
        {
            get { return _CurrentUser; }
            set
            {
                if (_CurrentUser != value)
                {
                    _CurrentUser = value;
                    NotifyPropertyChanged("CurrentUser");

                    if (_CurrentUser != null)
                    {
                        Thread td = new Thread(() => DisplayItems());
                        td.IsBackground = true;
                        td.Start();
                    }
                    else
                    {
                        CompanyUsersFromDB = new ObservableCollection<CompanyUserViewModel>();
                    }
                }
            }
        }

        private void DisplayItems()
        {
            LoadingItemsData = true;

            var response = new CompanyUserSQLiteRepository().GetCompanyUsers(CurrentUser.Identifier);
            if (response.Success)
            {
                CompanyUsersFromDB = new ObservableCollection<CompanyUserViewModel>(response?.CompanyUsers ?? new List<CompanyUserViewModel>());
            }
            else
            {
                CompanyUsersFromDB = new ObservableCollection<CompanyUserViewModel>();
                MainWindow.ErrorMessage = "";
            }

            LoadingItemsData = false;
        }
        #endregion

        #region LoadingItemsData
        private bool _LoadingItemsData;

        public bool LoadingItemsData
        {
            get { return _LoadingItemsData; }
            set
            {
                if (_LoadingItemsData != value)
                {
                    _LoadingItemsData = value;
                    NotifyPropertyChanged("LoadingItemsData");
                }
            }
        }
        #endregion


        #region CompanyUsersFromDB
        private ObservableCollection<CompanyUserViewModel> _CompanyUsersFromDB;

        public ObservableCollection<CompanyUserViewModel> CompanyUsersFromDB
        {
            get { return _CompanyUsersFromDB; }
            set
            {
                if (_CompanyUsersFromDB != value)
                {
                    _CompanyUsersFromDB = value;
                    NotifyPropertyChanged("CompanyUsersFromDB");
                }
            }
        }
        #endregion


        #region UsersFromDB
        private ObservableCollection<UserViewModel> _UsersFromDB;

        public ObservableCollection<UserViewModel> UsersFromDB
        {
            get { return _UsersFromDB; }
            set
            {
                if (_UsersFromDB != value)
                {
                    _UsersFromDB = value;
                    NotifyPropertyChanged("UsersFromDB");
                }
            }
        }
        #endregion

        #region UserSearchObject
        private UserViewModel _UserSearchObject = new UserViewModel();

        public UserViewModel UserSearchObject
        {
            get { return _UserSearchObject; }
            set
            {
                if (_UserSearchObject != value)
                {
                    _UserSearchObject = value;
                    NotifyPropertyChanged("UserSearchObject");
                }
            }
        }
        #endregion

        #region UserDataLoading
        private bool _UserDataLoading = true;

        public bool UserDataLoading
        {
            get { return _UserDataLoading; }
            set
            {
                if (_UserDataLoading != value)
                {
                    _UserDataLoading = value;
                    NotifyPropertyChanged("UserDataLoading");
                }
            }
        }
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

        #endregion

        #region Constructor

        public UserList()
        {
            // Get required services
            this.userService = DependencyResolver.Kernel.Get<IUserService>();
            this.companyService = DependencyResolver.Kernel.Get<ICompanyService>();
            this.compUserService = DependencyResolver.Kernel.Get<ICompanyUserService>();

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
            UserDataLoading = true;

            UserListResponse response = new UserSQLiteRepository()
                .GetUsersByPage(MainWindow.CurrentCompanyId, UserSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                UsersFromDB = new ObservableCollection<UserViewModel>(response.Users ?? new List<UserViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                UsersFromDB = new ObservableCollection<UserViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            UserDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = ((string)Application.Current.FindResource("Korisnici_TriTacke"));
            new UserSQLiteRepository().Sync(userService);

            RefreshButtonContent = ((string)Application.Current.FindResource("Prava_pristupa_TriTacke"));
            new CompanyUserSQLiteRepository().Sync(compUserService);

            DisplayData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
        }
        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel user = new UserViewModel();
            user.Identifier = Guid.NewGuid();

            UserAddEdit addEditForm = new UserAddEdit(user, true);
            addEditForm.UserCreatedUpdated += new UserHandler(DisplayData);
            FlyoutHelper.OpenFlyout(this, "Podaci o korisnicima", 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_korisnika_za_izmenuUzvičnik"));
                return;
            }

            UserAddEdit addEditForm = new UserAddEdit(CurrentUser, false);
            addEditForm.UserCreatedUpdated += new UserHandler(DisplayData);
            FlyoutHelper.OpenFlyout(this, "Podaci o korisnicima", 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_korisnika_za_brisanjeUzvičnik"));
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("korisnik", CurrentUser.Username + CurrentUser.Code);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                UserResponse response = userService.Delete(CurrentUser.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new UserSQLiteRepository().Delete(CurrentUser.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Korisnik_je_uspešno_obrisanUzvičnik"));

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

        #region Export to excel

        /// <summary>
        /// Export data to Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    // Create excel workbook and sheet
            //    Excel.Application excel = new Excel.Application();
            //    excel.Visible = true;
            //    Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            //    Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            //    // Load data that will be exported to excel
            //    List<UserViewModel> usersForExport = GetUsers();

            //    // Insert document headers
            //    sheet1.Range[sheet1.Cells[1, 1], sheet1.Cells[1, 6]].Merge();
            //    sheet1.Cells[1, 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //    sheet1.Cells[1, 1].Font.Bold = true;
            //    sheet1.Cells[1, 1] = "Podaci o korisnicima";

            //    // Insert row headers
            //    sheet1.Rows[3].Font.Bold = true;
            //    sheet1.Cells[3, 1] = "Korisničko ime";
            //    sheet1.Cells[3, 2] = "Ime";
            //    sheet1.Cells[3, 3] = "Prezime";
            //    sheet1.Cells[3, 4] = "E-mail";
            //    sheet1.Cells[3, 5] = "Prava pristupa";
            //    sheet1.Cells[3, 6] = "Firme";

            //    // Insert data to excel
            //    for (int i = 0; i < usersForExport.Count; i++)
            //    {
            //        sheet1.Cells[i + 4, 1] = usersForExport[i].Username;
            //        sheet1.Cells[i + 4, 2] = usersForExport[i].FirstName;
            //        sheet1.Cells[i + 4, 3] = usersForExport[i].LastName;
            //        sheet1.Cells[i + 4, 4] = usersForExport[i].Email;
            //        sheet1.Cells[i + 4, 5] = usersForExport[i].Roles.ToString();
            //        sheet1.Cells[i + 4, 6] = usersForExport[i].Companies.ToString();
            //    }

            //    // Set additional options
            //    sheet1.Columns.AutoFit();
            //}
            //catch (Exception ex)
            //{
            //    notifier.ShowError(ex.Message);
            //}
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }
}
