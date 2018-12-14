using AutoUpdaterDotNET;
using Ninject;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Messages.Common.Companies;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Common;
using SirmiumERPGFC.Repository.Companies;
using SirmiumERPGFC.Repository.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace SirmiumERPGFC.Identity.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : INotifyPropertyChanged
    {
        #region Attributes
        #region Services
        ICompanyService companyService;
        IAuthenticationService authenticationService;
        IUserService userService;
        ICompanyUserService compUserService;
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

        #region CurrentCompany
        private CompanyViewModel _CurrentCompany;

        public CompanyViewModel CurrentCompany
        {
            get { return _CurrentCompany; }
            set
            {
                if (_CurrentCompany != value)
                {
                    _CurrentCompany = value;
                    NotifyPropertyChanged("CurrentCompany");
                }
            }
        }
        #endregion


        Notifier notifier;
        #endregion
        
        #region Constructor
        public LoginWindow()
        {
            companyService = DependencyResolver.Kernel.Get<ICompanyService>();
            authenticationService = DependencyResolver.Kernel.Get<IAuthenticationService>();
            userService = DependencyResolver.Kernel.Get<IUserService>();
            compUserService = DependencyResolver.Kernel.Get<ICompanyUserService>();

            InitializeComponent();

            this.DataContext = this;

            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(3));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            SQLiteInitializer.Initalize(false);

            new CompanySQLiteRepository().Sync(companyService);
            new UserSQLiteRepository().Sync(userService);
            new CompanyUserSQLiteRepository().Sync(compUserService);

            // Set company combobox
            List<CompanyViewModel> companiesFromDb = new List<CompanyViewModel>();
            try
            {
                companiesFromDb.Add(new CompanyViewModel() { CompanyName = "Odaberite firmu", CompanyCode = "0" });
                CompanyListResponse response = companyService.GetCompanies();
                if (response.Success && response.Companies != null && response.Companies.Count > 0)
                {
                    companiesFromDb.AddRange(response.Companies.OrderBy(x => x.CompanyName));
                }
            }
            catch (Exception ex)
            {
                CompanyListResponse response = new CompanySQLiteRepository().GetCompanies(null);//companyService.GetCompanies();
                if (response.Success && response.Companies != null && response.Companies.Count > 0)
                {
                    companiesFromDb.AddRange(response.Companies.OrderBy(x => x.CompanyName));
                }
            }

            CompaniesFromDB = new ObservableCollection<CompanyViewModel>(companiesFromDb ?? new List<CompanyViewModel>());
            CurrentCompany = CompaniesFromDB.FirstOrDefault();
            // Set focus
            txtUsername.Focus();

            InitializeUpdater();
        }

        #endregion

        #region Login, Cancel

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if (InputIsValid())
            {
                string username = txtUsername.Text;
                string password = txtPasswordBox.Password;
                try
                {
                    //Validate credentials through the authentication service
                    UserSQLiteRepository userSQLiteRepository = new UserSQLiteRepository();
                    UserViewModel userViewModel = new UserViewModel();


                    CompanyViewModel company = CompaniesFromDB.Where(x => x.Identifier != Guid.Empty).FirstOrDefault();

                    CompanyUserViewModel compUser = null;

                    userSQLiteRepository.GetUsers();
                    UserResponse userResponse = userSQLiteRepository.Authenticate(username, CalculateHash(password, username), company.Id);
                    if (userResponse.Success && userResponse.User != null)
                    {
                        userViewModel = userResponse.User;

                        var companyUser = new CompanyUserSQLiteRepository().GetCompanyUser(company.Id, userViewModel.Identifier);
                        if (companyUser.Success)
                        {
                            compUser = companyUser.CompanyUser;
                        }
                    }
                    else
                    {
                        userResponse = authenticationService.Authenticate(username, CalculateHash(password, username));
                        if (!userResponse.Success || userResponse.User == null)
                            throw new UnauthorizedAccessException();

                        userViewModel = userResponse.User;

                        var companyUser = new CompanyUserSQLiteRepository().GetCompanyUser(company.Id, userViewModel.Identifier);
                        if (companyUser.Success)
                        {
                            compUser = companyUser.CompanyUser;
                        }
                    }

                    if (compUser == null)
                    {
                        throw new UnauthorizedAccessException();
                    }


                    //Get the current principal object
                    CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                    if (customPrincipal == null)
                        throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

                    //Authenticate the user
                    customPrincipal.Identity = new CustomIdentity(
                        userViewModel.Id,
                        userViewModel.FirstName,
                        userViewModel.LastName,
                        userViewModel.Identifier,
                        userViewModel.Email,
                        company.Id,
                        company.Identifier,
                        company.CompanyName,
                        userViewModel,
                        compUser);


                    if (cbxLanguages.SelectedValue.ToString() == "Serbia")
                    {
                        ResourceDictionary dict = new ResourceDictionary();
                        dict.Source = new Uri("..\\..\\Resources\\Languages\\StringResources-SRB.xaml", UriKind.Relative);
                        App.Current.Resources.MergedDictionaries.Add(dict);
                    }
                    else if (cbxLanguages.SelectedValue.ToString() == "Germany")
                    {
                        ResourceDictionary dict = new ResourceDictionary();
                        dict.Source = new Uri("..\\..\\Resources\\Languages\\StringResources-GER.xaml", UriKind.Relative);
                        App.Current.Resources.MergedDictionaries.Add(dict);
                    }
                    else
                    {
                        ResourceDictionary dict = new ResourceDictionary();
                        dict.Source = new Uri("..\\..\\Resources\\Languages\\StringResources-ENG.xaml", UriKind.Relative);
                        App.Current.Resources.MergedDictionaries.Add(dict);
                    }

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    this.Close();

                }
                catch (UnauthorizedAccessException)
                {
                    notifier.ShowError("Korisničko ime, lozinka ili firma nisu korektni!");
                }
                catch (Exception ex)
                {
                    notifier?.ShowError("Greška: " + ex?.Message);
                }
            }
        }

        private bool InputIsValid()
        {
            bool isValid = true;

            // Check if username is entered
            if (String.IsNullOrEmpty(txtUsername.Text))
            {
                isValid = false;
                notifier.ShowError("Korisničko ime je obavezno!");
            }

            // Check if password is entered
            if (String.IsNullOrEmpty(txtPasswordBox.Password))
            {
                isValid = false;
                notifier.ShowError("Lozinka je obavezna!");
            }

            return isValid;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static string CalculateHash(string password, string username)
        {
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(password + username);
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            return Convert.ToBase64String(hash);
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

        #region AutoUpdate

        void InitializeUpdater()
        {
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.RunUpdateAsAdmin = true;
            AutoUpdater.CheckForUpdateEvent += AutoUpdater_CheckForUpdateEvent;
        }

        private void AutoUpdater_CheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {
                    if (args.InstalledVersion != args.CurrentVersion)
                    {
                        bool downloaded = AutoUpdater.DownloadUpdate();
                        if (!downloaded)
                        {
                            MessageBox.Show("Dogodila se greška prilikom preuzimanja nove verzije aplikacije!\nBiće pokrenuta prethodna verzija!");
                            return;
                        }
                        else
                        {
                            System.Windows.Application.Current.Shutdown();
                        }
                    }
                }
            }
        }
        #endregion

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                AutoUpdater.Start("http://sirmiumerp.com/Shared/Updates/SirmiumERPGCF/currentversion.xml");
            }
        }
    }
}
