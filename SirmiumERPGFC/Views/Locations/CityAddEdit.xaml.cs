using Ninject;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Locations;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using ServiceInterfaces.ViewModels.Common.Companies;

namespace SirmiumERPGFC.Views.Locations
{
    public partial class CityAddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        ICityService cityService;
        #endregion

        #region Event
        public event CityHandler CityCreatedUpdated;
        #endregion

        #region CurrentCity
        private CityViewModel _CurrentCity;

        public CityViewModel CurrentCity
        {
            get { return _CurrentCity; }
            set
            {
                if (_CurrentCity != value)
                {
                    _CurrentCity = value;
                    NotifyPropertyChanged("CurrentCity");
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

        public CityAddEdit(CityViewModel cityViewModel, bool isCreateProcess, bool isPopup = false)
        {
            cityService = DependencyResolver.Kernel.Get<ICityService>();

            // Initialize form components
            InitializeComponent();

            this.DataContext = this;

            CurrentCity = cityViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;
        }

        #endregion

        #region Cancel and Save buttons

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentCity.ZipCode == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Postanski_broj"));
                return;
            }

            if (String.IsNullOrEmpty(CurrentCity.Name))
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv_grada"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
				SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
				SubmitButtonEnabled = false;

                CurrentCity.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentCity.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentCity.IsSynced = false;

                CityResponse response = new CitySQLiteRepository().Delete(CurrentCity.Identifier);
                response = new CitySQLiteRepository().Create(CurrentCity);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
					SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                    return;
                }

                response = cityService.Create(CurrentCity);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik"));
					SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
					SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;

                    CityCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentCity = new CityViewModel();
                        CurrentCity.Identifier = Guid.NewGuid();

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txtZipCode.Focus();
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
