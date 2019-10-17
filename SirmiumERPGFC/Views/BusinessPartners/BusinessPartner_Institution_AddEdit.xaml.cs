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
    /// Interaction logic for BusinessPartner_Institution_AddEdit.xaml
    /// </summary>
    public partial class BusinessPartner_Institution_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerService businessPartnerService;
        IBusinessPartnerInstitutionService businessPartnerInstitutionService;
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


        #region BusinessPartnerInstitutionsFromDB
        private ObservableCollection<BusinessPartnerInstitutionViewModel> _BusinessPartnerInstitutionsFromDB;

        public ObservableCollection<BusinessPartnerInstitutionViewModel> BusinessPartnerInstitutionsFromDB
        {
            get { return _BusinessPartnerInstitutionsFromDB; }
            set
            {
                if (_BusinessPartnerInstitutionsFromDB != value)
                {
                    _BusinessPartnerInstitutionsFromDB = value;
                    NotifyPropertyChanged("BusinessPartnerInstitutionsFromDB");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerInstitutionForm
        private BusinessPartnerInstitutionViewModel _CurrentBusinessPartnerInstitutionForm = new BusinessPartnerInstitutionViewModel();

        public BusinessPartnerInstitutionViewModel CurrentBusinessPartnerInstitutionForm
        {
            get { return _CurrentBusinessPartnerInstitutionForm; }
            set
            {
                if (_CurrentBusinessPartnerInstitutionForm != value)
                {
                    _CurrentBusinessPartnerInstitutionForm = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerInstitutionForm");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerInstitutionDG
        private BusinessPartnerInstitutionViewModel _CurrentBusinessPartnerInstitutionDG;

        public BusinessPartnerInstitutionViewModel CurrentBusinessPartnerInstitutionDG
        {
            get { return _CurrentBusinessPartnerInstitutionDG; }
            set
            {
                if (_CurrentBusinessPartnerInstitutionDG != value)
                {
                    _CurrentBusinessPartnerInstitutionDG = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerInstitutionDG");
                }
            }
        }
        #endregion

        #region BusinessPartnerInstitutionDataLoading
        private bool _BusinessPartnerInstitutionDataLoading;

        public bool BusinessPartnerInstitutionDataLoading
        {
            get { return _BusinessPartnerInstitutionDataLoading; }
            set
            {
                if (_BusinessPartnerInstitutionDataLoading != value)
                {
                    _BusinessPartnerInstitutionDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerInstitutionDataLoading");
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

        public BusinessPartner_Institution_AddEdit(BusinessPartnerViewModel businessPartner)
        {
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            businessPartnerInstitutionService = DependencyResolver.Kernel.Get<IBusinessPartnerInstitutionService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentBusinessPartner = businessPartner;
            CurrentBusinessPartnerInstitutionForm = new BusinessPartnerInstitutionViewModel();
            CurrentBusinessPartnerInstitutionForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerInstitutionForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayBusinessPartnerInstitutionData());
            displayThread.IsBackground = true;
            displayThread.Start();

            txtCode.Focus();
        }

        #endregion

        #region Display data

        public void DisplayBusinessPartnerInstitutionData()
        {
            BusinessPartnerInstitutionDataLoading = true;

            BusinessPartnerInstitutionListResponse response = new BusinessPartnerInstitutionSQLiteRepository()
                .GetBusinessPartnerInstitutionsByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                BusinessPartnerInstitutionsFromDB = new ObservableCollection<BusinessPartnerInstitutionViewModel>(
                    response.BusinessPartnerInstitutions ?? new List<BusinessPartnerInstitutionViewModel>());
            }
            else
            {
                BusinessPartnerInstitutionsFromDB = new ObservableCollection<BusinessPartnerInstitutionViewModel>();
            }

            BusinessPartnerInstitutionDataLoading = false;
        }

        private void DgBusinessPartnerInstitutions_LoadingRow(object sender, DataGridRowEventArgs e)
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

        private void btnAddInstitution_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartnerInstitutionForm.Institution == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Institucija"));
                return;
            }

            #endregion

            new BusinessPartnerInstitutionSQLiteRepository().Delete(CurrentBusinessPartnerInstitutionForm.Identifier);

            CurrentBusinessPartnerInstitutionForm.BusinessPartner = CurrentBusinessPartner;

            CurrentBusinessPartnerInstitutionForm.IsSynced = false;
            CurrentBusinessPartnerInstitutionForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentBusinessPartnerInstitutionForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new BusinessPartnerInstitutionSQLiteRepository().Create(CurrentBusinessPartnerInstitutionForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentBusinessPartnerInstitutionForm = new BusinessPartnerInstitutionViewModel();
                CurrentBusinessPartnerInstitutionForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerInstitutionForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentBusinessPartnerInstitutionForm = new BusinessPartnerInstitutionViewModel();
            CurrentBusinessPartnerInstitutionForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerInstitutionForm.ItemStatus = ItemStatus.Added;

            BusinessPartnerCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayBusinessPartnerInstitutionData());
            displayThread.IsBackground = true;
            displayThread.Start();

            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    txtCode.Focus();
                })
            );

            SubmitButtonEnabled = true;
        }

        private void btnEditInstitution_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerInstitutionForm = new BusinessPartnerInstitutionViewModel();
            CurrentBusinessPartnerInstitutionForm.Identifier = CurrentBusinessPartnerInstitutionDG.Identifier;
            CurrentBusinessPartnerInstitutionForm.ItemStatus = ItemStatus.Edited;

            CurrentBusinessPartnerInstitutionForm.Code = CurrentBusinessPartnerInstitutionDG.Code;
            CurrentBusinessPartnerInstitutionForm.Institution = CurrentBusinessPartnerInstitutionDG.Institution;
            CurrentBusinessPartnerInstitutionForm.Username = CurrentBusinessPartnerInstitutionDG.Username;
            CurrentBusinessPartnerInstitutionForm.Password = CurrentBusinessPartnerInstitutionDG.Password;
            CurrentBusinessPartnerInstitutionForm.ContactPerson = CurrentBusinessPartnerInstitutionDG.ContactPerson;
            CurrentBusinessPartnerInstitutionForm.Phone = CurrentBusinessPartnerInstitutionDG.Phone;
            CurrentBusinessPartnerInstitutionForm.Fax = CurrentBusinessPartnerInstitutionDG.Fax;
            CurrentBusinessPartnerInstitutionForm.Email = CurrentBusinessPartnerInstitutionDG.Email;
            CurrentBusinessPartnerInstitutionForm.Note = CurrentBusinessPartnerInstitutionDG.Note;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new BusinessPartnerInstitutionSQLiteRepository().SetStatusDeleted(CurrentBusinessPartnerInstitutionDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentBusinessPartnerInstitutionForm = new BusinessPartnerInstitutionViewModel();
                CurrentBusinessPartnerInstitutionForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerInstitutionForm.ItemStatus = ItemStatus.Added;

                CurrentBusinessPartnerInstitutionDG = null;

                BusinessPartnerCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayBusinessPartnerInstitutionData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelInstitution_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerInstitutionForm = new BusinessPartnerInstitutionViewModel();
            CurrentBusinessPartnerInstitutionForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerInstitutionForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (BusinessPartnerInstitutionsFromDB == null || BusinessPartnerInstitutionsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentBusinessPartner.Institutions = BusinessPartnerInstitutionsFromDB;
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
