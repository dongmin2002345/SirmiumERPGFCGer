using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
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
    /// Interaction logic for BusinessPartner_Types_AddEdit.xaml
    /// </summary>
    public partial class BusinessPartner_Types_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerService businessPartnerService;
        IBusinessPartnerTypeService businessPartnerTypeService;
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


        #region BusinessPartnerTypesFromDB
        private ObservableCollection<BusinessPartnerTypeViewModel> _BusinessPartnerTypesFromDB;

        public ObservableCollection<BusinessPartnerTypeViewModel> BusinessPartnerTypesFromDB
        {
            get { return _BusinessPartnerTypesFromDB; }
            set
            {
                if (_BusinessPartnerTypesFromDB != value)
                {
                    _BusinessPartnerTypesFromDB = value;
                    NotifyPropertyChanged("BusinessPartnerTypesFromDB");
                }
            }
        }
        #endregion

        #region BusinessPartnerTypeDataLoading
        private bool _BusinessPartnerTypeDataLoading;

        public bool BusinessPartnerTypeDataLoading
        {
            get { return _BusinessPartnerTypeDataLoading; }
            set
            {
                if (_BusinessPartnerTypeDataLoading != value)
                {
                    _BusinessPartnerTypeDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerTypeDataLoading");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerTypeDG
        private BusinessPartnerTypeViewModel _CurrentBusinessPartnerTypeDG = new BusinessPartnerTypeViewModel();

        public BusinessPartnerTypeViewModel CurrentBusinessPartnerTypeDG
        {
            get { return _CurrentBusinessPartnerTypeDG; }
            set
            {
                if (_CurrentBusinessPartnerTypeDG != value)
                {
                    _CurrentBusinessPartnerTypeDG = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerTypeDG");
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

        public BusinessPartner_Types_AddEdit(BusinessPartnerViewModel businessPartner)
        {
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            businessPartnerTypeService = DependencyResolver.Kernel.Get<IBusinessPartnerTypeService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentBusinessPartner = businessPartner;
            CurrentBusinessPartnerTypeDG = new BusinessPartnerTypeViewModel();
            CurrentBusinessPartnerTypeDG.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerTypeDG.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => PopulateBusinessPartnerTypeData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnSaveType.Focus();
        }

        #endregion

        #region Display data

        private void PopulateBusinessPartnerTypeData()
        {
            BusinessPartnerTypeDataLoading = true;

            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeSQLiteRepository()
                .GetBusinessPartnerTypes(MainWindow.CurrentCompanyId);
            if (response.Success)
            {
                BusinessPartnerTypesFromDB = new ObservableCollection<BusinessPartnerTypeViewModel>(
                    response.BusinessPartnerTypes ?? new List<BusinessPartnerTypeViewModel>());

                List<BusinessPartnerTypeViewModel> currentBusinessPartnerType = new BusinessPartnerTypeSQLiteRepository()
                    .GetBusinessPartnerTypesByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier).BusinessPartnerTypes;

                foreach (var businessPartnerType in BusinessPartnerTypesFromDB.Where(x => currentBusinessPartnerType.Select(y => y.Identifier).Contains(x.Identifier)))
                {
                    businessPartnerType.IsSelected = true;
                }
            }
            else
            {
                BusinessPartnerTypesFromDB = new ObservableCollection<BusinessPartnerTypeViewModel>();
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_prilikom_učitavanja_podatakaUzvičnik"));
            }

            BusinessPartnerTypeDataLoading = false;
        }

        private void DgBusinessPartnerTypes_LoadingRow(object sender, DataGridRowEventArgs e)
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

        #region Select type

        private void btnSaveType_Click(object sender, RoutedEventArgs e)
        {
            // Delete connected items
            BusinessPartnerBusinessPartnerTypeSQLiteRepository sqLite = new BusinessPartnerBusinessPartnerTypeSQLiteRepository();
            sqLite.Delete(CurrentBusinessPartner.Identifier);

            // Insert selected items
            bool operationSuccessfull = true;
            foreach (var item in BusinessPartnerTypesFromDB.Where(x => x.IsSelected))
            {
                var response = sqLite.Create(CurrentBusinessPartner.Identifier, item.Identifier);
                operationSuccessfull = operationSuccessfull && response.Success;
            }

            if (operationSuccessfull)
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
            else
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greško_kod_čuvanja_podatakaUzvičnik"));
        }

        private void btnCancelType_Click(object sender, RoutedEventArgs e)
        {
            Thread displayThread = new Thread(() => PopulateBusinessPartnerTypeData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        
        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (BusinessPartnerTypesFromDB == null || BusinessPartnerTypesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentBusinessPartner.BusinessPartnerTypes = BusinessPartnerTypesFromDB;
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
