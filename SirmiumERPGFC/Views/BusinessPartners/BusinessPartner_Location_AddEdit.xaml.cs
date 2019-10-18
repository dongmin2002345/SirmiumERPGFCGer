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
    /// Interaction logic for BusinessPartner_Location_AddEdit.xaml
    /// </summary>
    public partial class BusinessPartner_Location_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerService businessPartnerService;
        IBusinessPartnerLocationService businessPartnerLocationService;
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


        #region BusinessPartnerLocationsFromDB
        private ObservableCollection<BusinessPartnerLocationViewModel> _BusinessPartnerLocationsFromDB;

        public ObservableCollection<BusinessPartnerLocationViewModel> BusinessPartnerLocationsFromDB
        {
            get { return _BusinessPartnerLocationsFromDB; }
            set
            {
                if (_BusinessPartnerLocationsFromDB != value)
                {
                    _BusinessPartnerLocationsFromDB = value;
                    NotifyPropertyChanged("BusinessPartnerLocationsFromDB");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerLocationForm
        private BusinessPartnerLocationViewModel _CurrentBusinessPartnerLocationForm = new BusinessPartnerLocationViewModel();

        public BusinessPartnerLocationViewModel CurrentBusinessPartnerLocationForm
        {
            get { return _CurrentBusinessPartnerLocationForm; }
            set
            {
                if (_CurrentBusinessPartnerLocationForm != value)
                {
                    _CurrentBusinessPartnerLocationForm = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerLocationForm");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerLocationDG
        private BusinessPartnerLocationViewModel _CurrentBusinessPartnerLocationDG;

        public BusinessPartnerLocationViewModel CurrentBusinessPartnerLocationDG
        {
            get { return _CurrentBusinessPartnerLocationDG; }
            set
            {
                if (_CurrentBusinessPartnerLocationDG != value)
                {
                    _CurrentBusinessPartnerLocationDG = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerLocationDG");
                }
            }
        }
        #endregion

        #region BusinessPartnerLocationDataLoading
        private bool _BusinessPartnerLocationDataLoading;

        public bool BusinessPartnerLocationDataLoading
        {
            get { return _BusinessPartnerLocationDataLoading; }
            set
            {
                if (_BusinessPartnerLocationDataLoading != value)
                {
                    _BusinessPartnerLocationDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerLocationDataLoading");
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

        public BusinessPartner_Location_AddEdit(BusinessPartnerViewModel businessPartner)
        {
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            businessPartnerLocationService = DependencyResolver.Kernel.Get<IBusinessPartnerLocationService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentBusinessPartner = businessPartner;
            CurrentBusinessPartnerLocationForm = new BusinessPartnerLocationViewModel();
            CurrentBusinessPartnerLocationForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerLocationForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayBusinessPartnerLocationData());
            displayThread.IsBackground = true;
            displayThread.Start();

            txtAddress.Focus();
        }

        #endregion

        #region Display data

        public void DisplayBusinessPartnerLocationData()
        {
            BusinessPartnerLocationDataLoading = true;

            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationSQLiteRepository()
                .GetBusinessPartnerLocationsByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                BusinessPartnerLocationsFromDB = new ObservableCollection<BusinessPartnerLocationViewModel>(
                    response.BusinessPartnerLocations ?? new List<BusinessPartnerLocationViewModel>());
            }
            else
            {
                BusinessPartnerLocationsFromDB = new ObservableCollection<BusinessPartnerLocationViewModel>();
            }

            BusinessPartnerLocationDataLoading = false;
        }

        private void DgBusinessPartnerLocations_LoadingRow(object sender, DataGridRowEventArgs e)
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

        private void btnAddLocation_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartnerLocationForm.Country == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Država"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;


                CurrentBusinessPartnerLocationForm.BusinessPartner = CurrentBusinessPartner;


                CurrentBusinessPartnerLocationForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentBusinessPartnerLocationForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                new BusinessPartnerLocationSQLiteRepository().Delete(CurrentBusinessPartnerLocationForm.Identifier);
                var response = new BusinessPartnerLocationSQLiteRepository().Create(CurrentBusinessPartnerLocationForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentBusinessPartnerLocationForm = new BusinessPartnerLocationViewModel();
                    CurrentBusinessPartnerLocationForm.Identifier = Guid.NewGuid();
                    CurrentBusinessPartnerLocationForm.ItemStatus = ItemStatus.Added;
                    CurrentBusinessPartnerLocationForm.IsSynced = false;
                    return;
                }

                CurrentBusinessPartnerLocationForm = new BusinessPartnerLocationViewModel();
                CurrentBusinessPartnerLocationForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerLocationForm.ItemStatus = ItemStatus.Added;
                CurrentBusinessPartnerLocationForm.IsSynced = false;
                BusinessPartnerCreatedUpdated();

                DisplayBusinessPartnerLocationData();

                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        txtAddress.Focus();
                    })
                );

                SubmitButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnEditLocation_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerLocationForm = new BusinessPartnerLocationViewModel();
            CurrentBusinessPartnerLocationForm.Identifier = CurrentBusinessPartnerLocationDG.Identifier;
            CurrentBusinessPartnerLocationForm.ItemStatus = ItemStatus.Edited;

            CurrentBusinessPartnerLocationForm.IsSynced = CurrentBusinessPartnerLocationDG.IsSynced;
            CurrentBusinessPartnerLocationForm.Country = CurrentBusinessPartnerLocationDG.Country;
            CurrentBusinessPartnerLocationForm.Municipality = CurrentBusinessPartnerLocationDG.Municipality;
            CurrentBusinessPartnerLocationForm.Region = CurrentBusinessPartnerLocationDG.Region;
            CurrentBusinessPartnerLocationForm.City = CurrentBusinessPartnerLocationDG.City;
            CurrentBusinessPartnerLocationForm.Address = CurrentBusinessPartnerLocationDG.Address;
            CurrentBusinessPartnerLocationForm.UpdatedAt = CurrentBusinessPartnerLocationDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new BusinessPartnerLocationSQLiteRepository().SetStatusDeleted(CurrentBusinessPartnerLocationDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentBusinessPartnerLocationForm = new BusinessPartnerLocationViewModel();
                CurrentBusinessPartnerLocationForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerLocationForm.ItemStatus = ItemStatus.Added;

                CurrentBusinessPartnerLocationDG = null;

                BusinessPartnerCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayBusinessPartnerLocationData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelLocation_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerLocationForm = new BusinessPartnerLocationViewModel();
            CurrentBusinessPartnerLocationForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerLocationForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (BusinessPartnerLocationsFromDB == null || BusinessPartnerLocationsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentBusinessPartner.Locations = BusinessPartnerLocationsFromDB;
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
