using Ninject;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Helpers;
using ServiceInterfaces.Messages.Common.Companies;
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
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.Users
{
    /// <summary>
    /// Interaction logic for UserAddEdit.xaml
    /// </summary>
    public partial class UserAddEdit : UserControl, INotifyPropertyChanged
    {
        public event UserHandler UserCreatedUpdated;

        IUserService userService;
        ICompanyService companyService;

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
                }
            }
        }
        #endregion

        #region SelectedCompanyUserDG
        private CompanyUserViewModel _SelectedCompanyUserDG;

        public CompanyUserViewModel SelectedCompanyUserDG
        {
            get { return _SelectedCompanyUserDG; }
            set
            {
                if (_SelectedCompanyUserDG != value)
                {
                    _SelectedCompanyUserDG = value;
                    NotifyPropertyChanged("SelectedCompanyUserDG");
                }
            }
        }
        #endregion


        #region CompaniesFromDB
        private ObservableCollection<CompanyViewModel> _CompaniesFromDB;

        public ObservableCollection<CompanyViewModel> CompaniesFromDB
        {
            get { return _CompaniesFromDB; }
            set
            {
                if (_CompaniesFromDB != value)
                {
                    _CompaniesFromDB = value;
                    NotifyPropertyChanged("CompaniesFromDB");
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



        #region IsCreateProcess
        private bool _IsCreateProcess;

        public bool IsCreateProcess
        {
            get { return _IsCreateProcess; }
            set
            {
                if (_IsCreateProcess != value)
                {
                    _IsCreateProcess = value;
                    NotifyPropertyChanged("IsCreateProcess");
                }
            }
        }
        #endregion

        #region IsHeaderCreated
        private bool _IsHeaderCreated;

        public bool IsHeaderCreated
        {
            get { return _IsHeaderCreated; }
            set
            {
                if (_IsHeaderCreated != value)
                {
                    _IsHeaderCreated = value;
                    NotifyPropertyChanged("IsHeaderCreated");
                }
            }
        }
        #endregion

        #region IsPopup
        private bool _IsPopup;

        public bool IsPopup
        {
            get { return _IsPopup; }
            set
            {
                if (_IsPopup != value)
                {
                    _IsPopup = value;
                    NotifyPropertyChanged("IsPopup");
                }
            }
        }
        #endregion


        #region SaveButtonContent
        private string _SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));

        public string SaveButtonContent
        {
            get { return _SaveButtonContent; }
            set
            {
                if (_SaveButtonContent != value)
                {
                    _SaveButtonContent = value;
                    NotifyPropertyChanged("SaveButtonContent");
                }
            }
        }
        #endregion

        #region SaveButtonEnabled
        private bool _SaveButtonEnabled = true;

        public bool SaveButtonEnabled
        {
            get { return _SaveButtonEnabled; }
            set
            {
                if (_SaveButtonEnabled != value)
                {
                    _SaveButtonEnabled = value;
                    NotifyPropertyChanged("SaveButtonEnabled");
                }
            }
        }
        #endregion


        #region SubmitButtonContent
        private string _SubmitButtonContent = ((string)Application.Current.FindResource("Sačuvaj_i_proknjiži"));

        public string SubmitButtonContent
        {
            get { return _SubmitButtonContent; }
            set
            {
                if (_SubmitButtonContent != value)
                {
                    _SubmitButtonContent = value;
                    NotifyPropertyChanged("SubmitButtonContent");
                }
            }
        }
        #endregion

        #region SubmitButtonEnabled
        private bool _SubmitButtonEnabled = true;

        public bool SubmitButtonEnabled
        {
            get { return _SubmitButtonEnabled; }
            set
            {
                if (_SubmitButtonEnabled != value)
                {
                    _SubmitButtonEnabled = value;
                    NotifyPropertyChanged("SubmitButtonEnabled");
                }
            }
        }
        #endregion

        #region RolesFromDB
        private ObservableCollection<UserRoleViewModel> _RolesFromDB;

        public ObservableCollection<UserRoleViewModel> RolesFromDB
        {
            get { return _RolesFromDB; }
            set
            {
                if (_RolesFromDB != value)
                {
                    _RolesFromDB = value;
                    NotifyPropertyChanged("RolesFromDB");
                }
            }
        }
        #endregion

        #region UserRolesItemForm
        private CompanyUserViewModel _UserRolesItemForm;

        public CompanyUserViewModel UserRolesItemForm
        {
            get { return _UserRolesItemForm; }
            set
            {
                if (_UserRolesItemForm != value)
                {
                    _UserRolesItemForm = value;
                    NotifyPropertyChanged("UserRolesItemForm");
                }
            }
        }
        #endregion

        #region CompanyUserDataLoading
        private bool _CompanyUserDataLoading;

        public bool CompanyUserDataLoading
        {
            get { return _CompanyUserDataLoading; }
            set
            {
                if (_CompanyUserDataLoading != value)
                {
                    _CompanyUserDataLoading = value;
                    NotifyPropertyChanged("CompanyUserDataLoading");
                }
            }
        }
        #endregion


        #region Constructor

        public UserAddEdit(UserViewModel userViewModel, bool isCreateProcess, bool isPopup = false)
        {
            // Initialize service
            this.userService = DependencyResolver.Kernel.Get<IUserService>();
            this.companyService = DependencyResolver.Kernel.Get<ICompanyService>();
            InitializeComponent();

            this.DataContext = this;

            CurrentUser = userViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;

            RolesFromDB = new ObservableCollection<UserRoleViewModel>(UserRolesHelper.GetAvailableRoles());
            UserRolesItemForm = new CompanyUserViewModel();

            Thread td = new Thread(() => DisplayItems());
            td.IsBackground = true;
            td.Start();

            Thread displayThread = new Thread(() => PopulateCompanyData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        private void DisplayItems()
        {
            CompanyUserDataLoading = true;

            CompanyUserListResponse response = new CompanyUserSQLiteRepository().GetCompanyUsers(CurrentUser.Identifier);

            if (response.Success)
            {
                CompanyUsersFromDB = new ObservableCollection<CompanyUserViewModel>(response?.CompanyUsers ?? new List<CompanyUserViewModel>());
            }
            else
            {
                CompanyUsersFromDB = new ObservableCollection<CompanyUserViewModel>();
                MainWindow.ErrorMessage = response.Message;
            }

            CompanyUserDataLoading = false;
        }

        #endregion

        private void PopulateCompanyData()
        {
            CompanyListResponse response = new CompanySQLiteRepository()
                .GetCompanies(null);

            List<CompanyViewModel> companies = new List<CompanyViewModel>()
            {
                new CompanyViewModel()
                {
                    CompanyName = "Odaberite firmu..."
                }
            };
            if (response.Success)
            {
                companies.AddRange(response.Companies ?? new List<CompanyViewModel>());
            }
            else
            {
                companies = new List<CompanyViewModel>()
                {
                    new CompanyViewModel()
                    {
                        CompanyName = "Greška pri učitavanju podataka!"
                    }
                };
                MainWindow.ErrorMessage = response.Message;
            }

            CompaniesFromDB = new ObservableCollection<CompanyViewModel>(companies ?? new List<CompanyViewModel>());
            UserRolesItemForm.Company = CompaniesFromDB.FirstOrDefault();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            int compId = SelectedCompanyUserDG?.Company?.Id ?? 0;
            UserRolesItemForm = SelectedCompanyUserDG;
            UserRolesItemForm.Company = CompaniesFromDB.FirstOrDefault(x => x.Id == compId);

            foreach (var item in RolesFromDB)
            {
                if (UserRolesItemForm.UserRoles.Any(x => x.Name == item.Name))
                    item.IsChecked = true;
                else
                    item.IsChecked = false;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCompanyUserDG == null)
                return;

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("pristupna prava korisnika", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new CompanyUserSQLiteRepository().Delete(SelectedCompanyUserDG.Identifier);

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                Thread displayThread = new Thread(() => DisplayItems());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnSaveCompanyUser_Click(object sender, RoutedEventArgs e)
        {
            // Save header for any new change
            btnSaveHeader_Click(sender, e);

            #region Validation

            #endregion

            if (!IsHeaderCreated)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Zaglavlje_nije_sačuvano"));
                return;
            }

            var rolesList = RolesFromDB.Where(x => x.IsChecked)
                .ToList();

            UserRolesItemForm.UserRoles = rolesList;
            UserRolesItemForm.User = CurrentUser;

            if (UserRolesItemForm.Identifier == Guid.Empty)
                UserRolesItemForm.Identifier = Guid.NewGuid();

            var sqLite = new CompanyUserSQLiteRepository();
            sqLite.Delete(UserRolesItemForm.Identifier);
            var response = sqLite.Create(UserRolesItemForm);
            if (!response.Success)
                MainWindow.ErrorMessage = response.Message;
            else
            {
                Thread td = new Thread(() => DisplayItems());
                td.IsBackground = true;
                td.Start();

                UserRolesItemForm = new CompanyUserViewModel();
                UserRolesItemForm.Company = CompaniesFromDB.FirstOrDefault();
            }
        }

        private void btnCancelCompanyUser_Click(object sender, RoutedEventArgs e)
        {
            UserRolesItemForm = new CompanyUserViewModel();


            foreach (var item in RolesFromDB)
            {
                item.IsChecked = false;
            }
        }

        private void btnSaveHeader_Click(object sender, RoutedEventArgs e)
        {
            IsHeaderCreated = false;

            if (CurrentUser.Identifier == Guid.Empty)
                CurrentUser.Identifier = Guid.NewGuid();

            #region Validation
            if (String.IsNullOrEmpty(CurrentUser.Username))

                if (CurrentUser.Id < 1 && String.IsNullOrEmpty(txtPassword.Password))
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Morate_uneti_korisničko_imeUzvičnik"));
                    return;
                }

            if (!String.IsNullOrEmpty(txtPassword.Password))
            {
                if (txtPassword.Password != txtPasswordRepeat.Password)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Unete_lozinke_se_moraju_poklapatiUzvičnik"));
                    return;
                }
                else
                {
                    CurrentUser.Password = CalculateHash(txtPassword.Password, CurrentUser.Username);
                }
            }
            #endregion

            CurrentUser.IsSynced = false;
            CurrentUser.UpdatedAt = DateTime.Now;
            var sqLite = new UserSQLiteRepository();
            sqLite.Delete(CurrentUser.Identifier);
            var response = sqLite.Create(CurrentUser);
            if (response.Success)
            {
                IsHeaderCreated = true;
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }



        public static string CalculateHash(string password, string username)
        {
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(password + username);
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            return Convert.ToBase64String(hash);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Save header for any new change
            btnSaveHeader_Click(sender, e);

            #region Validation

            if (!IsHeaderCreated)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Zaglavlje_nije_sačuvano"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentUser.CompanyUsers = CompanyUsersFromDB.ToList();

                UserResponse response = userService.Create(CurrentUser);

                if (response.Success)
                {
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;

                    UserCreatedUpdated();

                    CurrentUser = new UserViewModel();
                    CurrentUser.Identifier = Guid.NewGuid();

                    Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (IsPopup)
                                FlyoutHelper.CloseFlyoutPopup(this);
                            else
                                FlyoutHelper.CloseFlyout(this);
                        })
                    );
                }
                else
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_čuvanja_na_serveruUzvičnik"));

                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                }
            });
            th.IsBackground = true;
            th.Start();

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            UserCreatedUpdated();
            if (IsPopup)
                FlyoutHelper.CloseFlyoutPopup(this);
            else
                FlyoutHelper.CloseFlyout(this);
        }

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
