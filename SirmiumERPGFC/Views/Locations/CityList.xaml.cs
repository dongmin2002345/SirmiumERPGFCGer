﻿using Ninject;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Locations;
using SirmiumERPGFC.Views.Common;
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

namespace SirmiumERPGFC.Views.Locations
{
    public delegate void CityHandler();

    public partial class CityList : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        ICityService cityService;
        #endregion


        #region CitiesFromDB
        private ObservableCollection<CityViewModel> _CitiesFromDB;

        public ObservableCollection<CityViewModel> CitiesFromDB
        {
            get { return _CitiesFromDB; }
            set
            {
                if (_CitiesFromDB != value)
                {
                    _CitiesFromDB = value;
                    NotifyPropertyChanged("CitiesFromDB");
                }
            }
        }
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

        #region CitySearchObject
        private CityViewModel _CitySearchObject = new CityViewModel();

        public CityViewModel CitySearchObject
        {
            get { return _CitySearchObject; }
            set
            {
                if (_CitySearchObject != value)
                {
                    _CitySearchObject = value;
                    NotifyPropertyChanged("CitySearchObject");
                }
            }
        }
        #endregion

        #region CityDataLoading
        private bool _CityDataLoading = true;

        public bool CityDataLoading
        {
            get { return _CityDataLoading; }
            set
            {
                if (_CityDataLoading != value)
                {
                    _CityDataLoading = value;
                    NotifyPropertyChanged("CityDataLoading");
                }
            }
        }
        #endregion


        #region Pagination data
        int currentPage = 1;
        int itemsPerPage = 50;
        int totalItems = 0;

        #region PaginationDisplay
        private string _PaginationDisplay;

        public string PaginationDisplay
        {
            get { return _PaginationDisplay; }
            set
            {
                if (_PaginationDisplay != value)
                {
                    _PaginationDisplay = value;
                    NotifyPropertyChanged("PaginationDisplay");
                }
            }
        }
		#endregion

		#endregion


		#region SyncButtonContent
		private string _SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

		public string SyncButtonContent
		{
			get { return _SyncButtonContent; }
			set
			{
				if (_SyncButtonContent != value)
				{
					_SyncButtonContent = value;
					NotifyPropertyChanged("SyncButtonContent");
				}
			}
		}
		#endregion

		#region SyncButtonEnabled
		private bool _SyncButtonEnabled = true;

		public bool SyncButtonEnabled
		{
			get { return _SyncButtonEnabled; }
			set
			{
				if (_SyncButtonEnabled != value)
				{
					_SyncButtonEnabled = value;
					NotifyPropertyChanged("SyncButtonEnabled");
				}
			}
		}
		#endregion


        #endregion

        #region Constructor

        public CityList()
        {
            // Get required services
            cityService = DependencyResolver.Kernel.Get<ICityService>();

            // Initialize form components
            InitializeComponent();

            this.DataContext = this;
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			Thread displayThread = new Thread(() => SyncData());
			displayThread.IsBackground = true;
			displayThread.Start();
		}

		#endregion

		#region Display data

		private void btnSync_Click(object sender, RoutedEventArgs e)
		{
			currentPage = 1;

			Thread syncThread = new Thread(() =>
			{
				SyncData();

				MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sinhronizovaniUzvičnik"));
            });
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

		private void dgCities_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = (e.Row.GetIndex() + 1).ToString();
		}

		public void DisplayData()
        {
            CityDataLoading = true;

            CityListResponse response = new CitySQLiteRepository()
                .GetCitiesByPage(MainWindow.CurrentCompanyId, CitySearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                CitiesFromDB = new ObservableCollection<CityViewModel>(response.Cities ?? new List<CityViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                CitiesFromDB = new ObservableCollection<CityViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            CityDataLoading = false;
        }

        private void SyncData()
        {
			SyncButtonEnabled = false;

			SyncButtonContent = ((string)Application.Current.FindResource("Gradovi_TriTacke"));
			new CitySQLiteRepository().Sync(cityService, (synced, toSync) =>
			{
				if (toSync > 0)
					SyncButtonContent = ((string)Application.Current.FindResource("Gradovi")) +"(" + synced + "/" + toSync + ")";
			});

			DisplayData();

			SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
			SyncButtonEnabled = true;


			//RefreshButtonEnabled = false;

   //         RefreshButtonContent = ((string)Application.Current.FindResource("Gradovi_TriTacke"));
   //         new CitySQLiteRepository().Sync(cityService);

   //         DisplayData();

   //         RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
   //         RefreshButtonEnabled = true;
        }

        #endregion

        #region Add city, edit city and delete city

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CityViewModel city = new CityViewModel();
            city.Identifier = Guid.NewGuid();

            CityAddEdit addEditForm = new CityAddEdit(city, true);
            addEditForm.CityCreatedUpdated += new CityHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_gradovima")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCity == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_grad_za_izmenuUzvičnik"));
                return;
            }

            CityAddEdit addEditForm = new CityAddEdit(CurrentCity, false);
            addEditForm.CityCreatedUpdated += new CityHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_gradovima")), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
			Thread th = new Thread(() =>
			{
				CityDataLoading = true;

				if (CurrentCity == null)
				{
					MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_grad_za_brisanjeUzvičnik"));
					CityDataLoading = false;
					return;
				}

				CityResponse response = cityService.Delete(CurrentCity.Identifier);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
					CityDataLoading = false;
					return;
				}

				response = new CitySQLiteRepository().Delete(CurrentCity.Identifier);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
					CityDataLoading = false;
					return;
				}

				MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Grad_je_uspešno_obrisanUzvičnik"));

				DisplayData();

				CityDataLoading = false;
			});
			th.IsBackground = true;
			th.Start();
			//if (CurrentCity == null)
			//{
			//    MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_grad_za_brisanjeUzvičnik"));
			//    return;
			//}

			//SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

			//// Create confirmation window
			//DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("grad", CurrentCity.ZipCode + " " + CurrentCity.Name);
			//var showDialog = deleteConfirmationForm.ShowDialog();
			//if (showDialog != null && showDialog.Value)
			//{
			//    CityResponse response = cityService.Delete(CurrentCity.Identifier);
			//    if (!response.Success)
			//    {
			//        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
			//        SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
			//        return;
			//    }

			//    response = new CitySQLiteRepository().Delete(CurrentCity.Identifier);
			//    if (!response.Success)
			//    {
			//        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
			//        SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
			//        return;
			//    }

			//    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Grad_je_uspešno_obrisanUzvičnik"));

			//    Thread displayThread = new Thread(() => SyncData());
			//    displayThread.IsBackground = true;
			//    displayThread.Start();
			//}

			//SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
		}
        #endregion

        #region Pagination

        private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage = 1;

                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnLastPage_Click(object sender, RoutedEventArgs e)
        {
            int lastPage = (int)Math.Ceiling((double)this.totalItems / this.itemsPerPage);
            if (currentPage < lastPage)
            {
                currentPage = lastPage;

                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
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
