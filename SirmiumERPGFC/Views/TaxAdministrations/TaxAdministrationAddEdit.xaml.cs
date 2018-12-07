using Ninject;
using ServiceInterfaces.Abstractions.Common.TaxAdministrations;
using ServiceInterfaces.Messages.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Locations;
using SirmiumERPGFC.Repository.TaxAdministrations;
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

namespace SirmiumERPGFC.Views.TaxAdministrations
{
    /// <summary>
    /// Interaction logic for TaxAdministrationAddEdit.xaml
    /// </summary>
    public partial class TaxAdministrationAddEdit : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Event for handling output invoice create and update
        /// </summary>
        public event TaxAdministrationHandler TaxAdministrationCreatedUpdated;

        /// <summary>
        /// Service for accessing output invoice
        /// </summary>
        ITaxAdministrationService taxAdministrationService;

        #region CurrentTaxAdministration
        private TaxAdministrationViewModel _CurrentTaxAdministration;

        public TaxAdministrationViewModel CurrentTaxAdministration
        {
            get { return _CurrentTaxAdministration; }
            set
            {
                if (_CurrentTaxAdministration != value)
                {
                    _CurrentTaxAdministration = value;
                    NotifyPropertyChanged("CurrentTaxAdministration");
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

        #region Germany
        private CountryViewModel _Germany;

        public CountryViewModel Germany
        {
            get { return _Germany; }
            set
            {
                if (_Germany != value)
                {
                    _Germany = value;
                    NotifyPropertyChanged("Germany");
                }
            }
        }
        #endregion


        #region Constructor

        public TaxAdministrationAddEdit(TaxAdministrationViewModel TaxAdministrationViewModel, bool isCreateProcess, bool isPopup = false)
        {
            // Initialize service
            taxAdministrationService = DependencyResolver.Kernel.Get<ITaxAdministrationService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentTaxAdministration = TaxAdministrationViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;

            Thread th = new Thread(() =>
            {
                Germany = new CountrySQLiteRepository().GetCountriesByPage(MainWindow.CurrentCompanyId, new CountryViewModel(), 1, Int32.MaxValue).Countries.FirstOrDefault(x => x.Name == "Nemačka");
            });
            th.IsBackground = true;
            th.Start();
        }

        #endregion

        #region Save and Cancel button

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentTaxAdministration.Name))
            {
                MainWindow.WarningMessage = "Sie müssen einen Namen schreiben!";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SaveButtonContent = " Wird geschpeichert... ";
                SaveButtonEnabled = false;

                CurrentTaxAdministration.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentTaxAdministration.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentTaxAdministration.IsSynced = false;
                CurrentTaxAdministration.UpdatedAt = DateTime.Now;

                TaxAdministrationResponse response = new TaxAdministrationSQLiteRepository().Delete(CurrentTaxAdministration.Identifier);
                response = new TaxAdministrationSQLiteRepository().Create(CurrentTaxAdministration);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Lokaler Speicherfehler!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;
                    return;
                }

                response = taxAdministrationService.Create(CurrentTaxAdministration);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Die Daten wurden im Lokal gespeichert. Server-Speicherfehler!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new TaxAdministrationSQLiteRepository().UpdateSyncStatus(response.TaxAdministration.Identifier, response.TaxAdministration.Code, response.TaxAdministration.UpdatedAt, response.TaxAdministration.Id, true);
                    MainWindow.SuccessMessage = "Daten wurden erfolgreich gespeichert!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;

                    TaxAdministrationCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentTaxAdministration = new TaxAdministrationViewModel();
                        CurrentTaxAdministration.Identifier = Guid.NewGuid();

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txtSecondCode.Focus();
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
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (IsPopup)
                FlyoutHelper.CloseFlyoutPopup(this);
            else
                FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "") //[CallerMemberName]
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion

    }
}
