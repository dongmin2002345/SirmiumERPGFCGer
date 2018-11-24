using Ninject;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Locations;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Locations;
using System;
using System.Collections.Generic;
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
    public partial class CountryAddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        ICountryService countryService;
        #endregion

        #region Events
        public event CountryHandler CountryCreatedUpdated;
        #endregion

        #region CurrentCountry
        private CountryViewModel _CurrentCountry = new CountryViewModel();

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
        private string _SaveButtonContent = " SAČUVAJ ";

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

        #endregion

        #region Constructor

        public CountryAddEdit(CountryViewModel countryViewModel, bool isCreateProcess, bool isPopup = false)
        {
            // Initialize service
            this.countryService = DependencyResolver.Kernel.Get<ICountryService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentCountry = countryViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;
        }

        #endregion

        #region  Save and Cancel button

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentCountry.Name))
            {
                MainWindow.WarningMessage = "Obavezno polje: Ime drzave";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SaveButtonContent = " Čuvanje u toku... ";
                SaveButtonEnabled = false;

                CurrentCountry.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentCountry.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentCountry.IsSynced = false;
                CurrentCountry.UpdatedAt = DateTime.Now;

                CountryResponse response = new CountrySQLiteRepository().Delete(CurrentCountry.Identifier);
                response = new CountrySQLiteRepository().Create(CurrentCountry);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;
                    return;
                }

                response = countryService.Create(CurrentCountry);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new CountrySQLiteRepository().UpdateSyncStatus(response.Country.Identifier, response.Country.Code, response.Country.UpdatedAt, response.Country.Id, true);
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;

                    CountryCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentCountry = new CountryViewModel();
                        CurrentCountry.Identifier = Guid.NewGuid();

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txtAlfaCode.Focus();
                            })
                        );
                    }
                    else
                    {
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
                }
            });
            th.IsBackground = true;
            th.Start();

            txtName.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (IsPopup)
                FlyoutHelper.CloseFlyoutPopup(this);
            else
                FlyoutHelper.CloseFlyout(this);
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
