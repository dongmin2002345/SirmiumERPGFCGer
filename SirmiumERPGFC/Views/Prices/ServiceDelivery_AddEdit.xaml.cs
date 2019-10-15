using Ninject;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Prices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Prices;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.Prices
{
    /// <summary>
    /// Interaction logic for ServiceDelivery_AddEdit.xaml
    /// </summary>
    public partial class ServiceDelivery_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IServiceDeliveryService ServiceDeliveryService;
        #endregion

        #region Events
        public event ServiceDeliveryHandler ServiceDeliveryCreatedUpdated;
        #endregion

        #region CurrentServiceDelivery
        private ServiceDeliveryViewModel _CurrentServiceDelivery = new ServiceDeliveryViewModel();

        public ServiceDeliveryViewModel CurrentServiceDelivery
        {
            get { return _CurrentServiceDelivery; }
            set
            {
                if (_CurrentServiceDelivery != value)
                {
                    _CurrentServiceDelivery = value;
                    NotifyPropertyChanged("CurrentServiceDelivery");
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
        private string _SubmitButtonContent = " PROKNJIŽI ";

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

        public ServiceDelivery_AddEdit(ServiceDeliveryViewModel ServiceDeliveryViewModel, bool isCreateProcess, bool isPopup = false)
        {
            ServiceDeliveryService = DependencyResolver.Kernel.Get<IServiceDeliveryService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentServiceDelivery = ServiceDeliveryViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;
        }

        #endregion

        #region Submit ans cancel
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentServiceDelivery.Name))
            {
                MainWindow.WarningMessage = "Obavezno polje: Naziv popusta";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonContent = " Čuvanje u toku... ";
                SubmitButtonEnabled = false;

                CurrentServiceDelivery.IsSynced = false;
                CurrentServiceDelivery.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentServiceDelivery.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                ServiceDeliveryResponse response = new ServiceDeliverySQLiteRepository().Delete(CurrentServiceDelivery.Identifier);
                response = new ServiceDeliverySQLiteRepository().Create(CurrentServiceDelivery);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;
                    return;
                }

                response = ServiceDeliveryService.Create(CurrentServiceDelivery);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;

                    ServiceDeliveryCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentServiceDelivery = new ServiceDeliveryViewModel();
                        CurrentServiceDelivery.Identifier = Guid.NewGuid();

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txtAmount.Focus();
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
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
