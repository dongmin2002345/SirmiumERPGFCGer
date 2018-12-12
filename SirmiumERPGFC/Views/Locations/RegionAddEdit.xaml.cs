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
    public partial class RegionAddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IRegionService regionService;
        #endregion

        #region Events
        public event RegionHandler RegionCreatedUpdated;
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

        #endregion

        #region Constructor

        public RegionAddEdit(RegionViewModel regionViewModel, bool isCreateProcess, bool isPopup = false)
        {
            // Initialize service
            this.regionService = DependencyResolver.Kernel.Get<IRegionService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentRegion = regionViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;
        }

        #endregion

        #region Save and Cancel button

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentRegion.Name))
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv_regiona"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SaveButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SaveButtonEnabled = false;

                CurrentRegion.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentRegion.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentRegion.IsSynced = false;

                RegionResponse response = new RegionSQLiteRepository().Delete(CurrentRegion.Identifier);
                response = new RegionSQLiteRepository().Create(CurrentRegion);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
                    SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));
                    SaveButtonEnabled = true;
                    return;
                }

                response = regionService.Create(CurrentRegion);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik"));
                    SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new RegionSQLiteRepository().UpdateSyncStatus(CurrentRegion.Identifier, response.Region.Code, response.Region.UpdatedAt, response.Region.Id, true);
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));
                    SaveButtonEnabled = true;

                    RegionCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentRegion = new RegionViewModel();
                        CurrentRegion.Identifier = Guid.NewGuid();

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txtRegionCode.Focus();
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
