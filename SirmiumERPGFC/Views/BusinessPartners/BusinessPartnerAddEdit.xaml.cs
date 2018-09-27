using GlobalValidations;
using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Identity;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace SirmiumERPGFC.Views.BusinessPartners
{
    /// <summary>
    /// Interaction logic for BusinessPartnerAddEdit.xaml
    /// </summary>
    public partial class BusinessPartnerAddEdit : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Event for handling business partner create and update
        /// </summary>
        public event BusinessPartnerHandler BusinessPartnerCreatedUpdated;

        /// <summary>
        /// Service for accessing business partners
        /// </summary>
        IBusinessPartnerService businessPartnerService;

        #region CurrentBusinessPartner
        private BusinessPartnerViewModel _CurrentBusinessPartner;

        public BusinessPartnerViewModel CurrentBusinessPartner
        {
            get { return _CurrentBusinessPartner; }
            set
            {
                if (_CurrentBusinessPartner != value)
                {
                    _CurrentBusinessPartner = value;
                    NotifyPropertyChanged("CurrentBusinessPartner");
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

        #region SaveButtonContent
        private string _SaveButtonContent = " Sačuvaj ";

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

        /// <summary>
        /// Notifier for displaying error and success messages
        /// </summary>
        Notifier notifier;

        #region Constructor

        /// <summary>
        /// BusinessPartnerAddEdit constructor
        /// </summary>
        /// <param name="businessPartnerViewModel"></param>
        public BusinessPartnerAddEdit(BusinessPartnerViewModel businessPartnerViewModel, bool isCreateProcess)
        {
            // Initialize service
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();

            // Draw all components
            InitializeComponent();

            this.DataContext = this;

            CurrentBusinessPartner = businessPartnerViewModel;
            IsCreateProcess = isCreateProcess;
        }
        #endregion

        #region Cancel save button 

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.CloseFlyout(this);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentBusinessPartner.Mobile))
            {
                MainWindow.WarningMessage = "Morate uneti mobilni!";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SaveButtonContent = " Čuvanje u toku... ";
                SaveButtonEnabled = false;

                CurrentBusinessPartner.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentBusinessPartner.IsSynced = false;
                CurrentBusinessPartner.UpdatedAt = DateTime.Now;

                BusinessPartnerResponse response = new BusinessPartnerSQLiteRepository().Delete(CurrentBusinessPartner.Identifier);
                response = new BusinessPartnerSQLiteRepository().Create(CurrentBusinessPartner);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                    return;
                }

                response = businessPartnerService.Create(CurrentBusinessPartner);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new BusinessPartnerSQLiteRepository().UpdateSyncStatus(response.BusinessPartner.Identifier, response.BusinessPartner.Id, true);
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;

                    BusinessPartnerCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentBusinessPartner = new BusinessPartnerViewModel();
                        CurrentBusinessPartner.Identifier = Guid.NewGuid();
                        CurrentBusinessPartner.Code = new BusinessPartnerSQLiteRepository().GetNewCodeValue();

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txtName.Focus();
                            })
                        );
                    }
                    else
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                FlyoutHelper.CloseFlyout(this);
                            })
                        );
                    }
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        #endregion


        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FlyoutHelper.CloseFlyout(this);
            }

            if (e.Key == Key.S && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                btnSave_Click(sender, e);
            }
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
