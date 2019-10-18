using Ninject;
using ServiceInterfaces.Abstractions;
using ServiceInterfaces.Messages.Statuses;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Statuses;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Statuses;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.Statuses
{
    /// <summary>
    /// Interaction logic for Status_AddEdit.xaml
    /// </summary>
    public partial class Status_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IStatusService StatusService;
        #endregion

        #region Events
        public event StatusHandler StatusCreatedUpdated;
        #endregion


        #region CurrentStatus
        private StatusViewModel _CurrentStatus = new StatusViewModel();

        public StatusViewModel CurrentStatus
        {
            get { return _CurrentStatus; }
            set
            {
                if (_CurrentStatus != value)
                {
                    _CurrentStatus = value;
                    NotifyPropertyChanged("CurrentStatus");
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


        #region SubmitButtonContent
        private string _SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));

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

        #endregion

        #region Constructor

        public Status_AddEdit(StatusViewModel StatusViewModel, bool isCreateProcess, bool isPopup = false)
        {
            StatusService = DependencyResolver.Kernel.Get<IStatusService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentStatus = StatusViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;
        }

        #endregion

        #region  Submit and Cancel button

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentStatus.Name))
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_polje_naziv"));
                return;
            }

            if (String.IsNullOrEmpty(CurrentStatus.ShortName))
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_polje_skraćeni_naziv"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentStatus.IsSynced = false;
                CurrentStatus.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentStatus.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                StatusResponse response = new StatusSQLiteRepository().Delete(CurrentStatus.Identifier);
                response = new StatusSQLiteRepository().Create(CurrentStatus);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_čuvanja_na_serveruUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                    return;
                }

                response = StatusService.Create(CurrentStatus);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik")) + response.Message;
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;

                    StatusCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentStatus = new StatusViewModel();
                        CurrentStatus.Identifier = Guid.NewGuid();

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
