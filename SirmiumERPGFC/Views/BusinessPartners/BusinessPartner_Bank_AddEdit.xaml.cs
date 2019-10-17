using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Common;
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

namespace SirmiumERPGFC.Views.BusinessPartners
{
    /// <summary>
    /// Interaction logic for BusinessPartner_Bank_AddEdit.xaml
    /// </summary>
    public partial class BusinessPartner_Bank_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerService businessPartnerService;
        IBusinessPartnerBankService businessPartnerBankService;
        #endregion


        #region Event
        public event BusinessPartnerHandler BusinessPartnerCreatedUpdated;
        #endregion


        #region CurrentBusinessPartner
        private BusinessPartnerViewModel _CurrentBusinessPartner = new BusinessPartnerViewModel();

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


        #region BusinessPartnerBanksFromDB
        private ObservableCollection<BusinessPartnerBankViewModel> _BusinessPartnerBanksFromDB;

        public ObservableCollection<BusinessPartnerBankViewModel> BusinessPartnerBanksFromDB
        {
            get { return _BusinessPartnerBanksFromDB; }
            set
            {
                if (_BusinessPartnerBanksFromDB != value)
                {
                    _BusinessPartnerBanksFromDB = value;
                    NotifyPropertyChanged("BusinessPartnerBanksFromDB");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerBankForm
        private BusinessPartnerBankViewModel _CurrentBusinessPartnerBankForm = new BusinessPartnerBankViewModel();

        public BusinessPartnerBankViewModel CurrentBusinessPartnerBankForm
        {
            get { return _CurrentBusinessPartnerBankForm; }
            set
            {
                if (_CurrentBusinessPartnerBankForm != value)
                {
                    _CurrentBusinessPartnerBankForm = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerBankForm");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerBankDG
        private BusinessPartnerBankViewModel _CurrentBusinessPartnerBankDG;

        public BusinessPartnerBankViewModel CurrentBusinessPartnerBankDG
        {
            get { return _CurrentBusinessPartnerBankDG; }
            set
            {
                if (_CurrentBusinessPartnerBankDG != value)
                {
                    _CurrentBusinessPartnerBankDG = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerBankDG");
                }
            }
        }
        #endregion

        #region BusinessPartnerBankDataLoading
        private bool _BusinessPartnerBankDataLoading;

        public bool BusinessPartnerBankDataLoading
        {
            get { return _BusinessPartnerBankDataLoading; }
            set
            {
                if (_BusinessPartnerBankDataLoading != value)
                {
                    _BusinessPartnerBankDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerBankDataLoading");
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

        public BusinessPartner_Bank_AddEdit(BusinessPartnerViewModel businessPartner)
        {
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            businessPartnerBankService = DependencyResolver.Kernel.Get<IBusinessPartnerBankService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentBusinessPartner = businessPartner;
            CurrentBusinessPartnerBankForm = new BusinessPartnerBankViewModel();
            CurrentBusinessPartnerBankForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerBankForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayBusinessPartnerBankData());
            displayThread.IsBackground = true;
            displayThread.Start();

            txtAccountNumber.Focus();
        }

        #endregion

        #region Display data

        public void DisplayBusinessPartnerBankData()
        {
            BusinessPartnerBankDataLoading = true;

            BusinessPartnerBankListResponse response = new BusinessPartnerBankSQLiteRepository()
                .GetBusinessPartnerBanksByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                BusinessPartnerBanksFromDB = new ObservableCollection<BusinessPartnerBankViewModel>(
                    response.BusinessPartnerBanks ?? new List<BusinessPartnerBankViewModel>());
            }
            else
            {
                BusinessPartnerBanksFromDB = new ObservableCollection<BusinessPartnerBankViewModel>();
            }

            BusinessPartnerBankDataLoading = false;
        }

        private void DgBusinessPartnerBanks_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void dg_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        #endregion

        #region Add, Edit and Delete 

        private void btnAddBank_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartnerBankForm.Bank.Name == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Ime_banke"));
                return;
            }

            #endregion

            new BusinessPartnerBankSQLiteRepository().Delete(CurrentBusinessPartnerBankForm.Identifier);

            CurrentBusinessPartnerBankForm.BusinessPartner = CurrentBusinessPartner;

            CurrentBusinessPartnerBankForm.IsSynced = false;
            CurrentBusinessPartnerBankForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentBusinessPartnerBankForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new BusinessPartnerBankSQLiteRepository().Create(CurrentBusinessPartnerBankForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentBusinessPartnerBankForm = new BusinessPartnerBankViewModel();
                CurrentBusinessPartnerBankForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerBankForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentBusinessPartnerBankForm = new BusinessPartnerBankViewModel();
            CurrentBusinessPartnerBankForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerBankForm.ItemStatus = ItemStatus.Added;

            BusinessPartnerCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayBusinessPartnerBankData());
            displayThread.IsBackground = true;
            displayThread.Start();

            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    txtAccountNumber.Focus();
                })
            );

            SubmitButtonEnabled = true;
        }

        private void btnEditBank_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerBankForm = new BusinessPartnerBankViewModel();
            CurrentBusinessPartnerBankForm.Identifier = CurrentBusinessPartnerBankDG.Identifier;
            CurrentBusinessPartnerBankForm.ItemStatus = ItemStatus.Edited;

            CurrentBusinessPartnerBankForm.Country.Name = CurrentBusinessPartnerBankDG.Country.Name;
            CurrentBusinessPartnerBankForm.Bank.Name = CurrentBusinessPartnerBankDG.Bank.Name;
            CurrentBusinessPartnerBankForm.AccountNumber = CurrentBusinessPartnerBankDG.AccountNumber;

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new BusinessPartnerBankSQLiteRepository().SetStatusDeleted(CurrentBusinessPartnerBankDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentBusinessPartnerBankForm = new BusinessPartnerBankViewModel();
                CurrentBusinessPartnerBankForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerBankForm.ItemStatus = ItemStatus.Added;

                CurrentBusinessPartnerBankDG = null;

                BusinessPartnerCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayBusinessPartnerBankData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelBank_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerBankForm = new BusinessPartnerBankViewModel();
            CurrentBusinessPartnerBankForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerBankForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (BusinessPartnerBanksFromDB == null || BusinessPartnerBanksFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentBusinessPartner.Banks = BusinessPartnerBanksFromDB;
                BusinessPartnerResponse response = businessPartnerService.Create(CurrentBusinessPartner);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_čuvanja_na_serveruUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;

                    BusinessPartnerCreatedUpdated();

                    Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            FlyoutHelper.CloseFlyout(this);
                        })
                    );
                }
            });
            td.IsBackground = true;
            td.Start();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            BusinessPartnerCreatedUpdated();

            FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion
    }
}
