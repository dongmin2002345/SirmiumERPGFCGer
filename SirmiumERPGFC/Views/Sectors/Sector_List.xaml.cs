using Ninject;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Sectors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.Sectors
{
	public delegate void SectorHandler();

	public partial class Sector_List : UserControl, INotifyPropertyChanged
	{
		#region Attributes

		#region Services
		ISectorService sectorService;
		#endregion


		#region SectorsFromDB
		private ObservableCollection<SectorViewModel> _SectorsFromDB;

		public ObservableCollection<SectorViewModel> SectorsFromDB
		{
			get { return _SectorsFromDB; }
			set
			{
				if (_SectorsFromDB != value)
				{
					_SectorsFromDB = value;
					NotifyPropertyChanged("SectorsFromDB");
				}
			}
		}
		#endregion

		#region CurrentSector
		private SectorViewModel _CurrentSector;

		public SectorViewModel CurrentSector
		{
			get { return _CurrentSector; }
			set
			{
				if (_CurrentSector != value)
				{
					_CurrentSector = value;
					NotifyPropertyChanged("CurrentSector");
				}
			}
		}
		#endregion

		#region SectorSearchObject
		private SectorViewModel _SectorSearchObject = new SectorViewModel();

		public SectorViewModel SectorSearchObject
		{
			get { return _SectorSearchObject; }
			set
			{
				if (_SectorSearchObject != value)
				{
					_SectorSearchObject = value;
					NotifyPropertyChanged("SectorSearchObject");
				}
			}
		}
		#endregion

		#region SectorDataLoading
		private bool _SectorDataLoading = true;

		public bool SectorDataLoading
		{
			get { return _SectorDataLoading; }
			set
			{
				if (_SectorDataLoading != value)
				{
					_SectorDataLoading = value;
					NotifyPropertyChanged("SectorDataLoading");
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


		#region RefreshButtonContent
		private string _RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

        public string RefreshButtonContent
		{
			get { return _RefreshButtonContent; }
			set
			{
				if (_RefreshButtonContent != value)
				{
					_RefreshButtonContent = value;
					NotifyPropertyChanged("RefreshButtonContent");
				}
			}
		}
		#endregion

		#region RefreshButtonEnabled
		private bool _RefreshButtonEnabled = true;

		public bool RefreshButtonEnabled
		{
			get { return _RefreshButtonEnabled; }
			set
			{
				if (_RefreshButtonEnabled != value)
				{
					_RefreshButtonEnabled = value;
					NotifyPropertyChanged("RefreshButtonEnabled");
				}
			}
		}
		#endregion

		#endregion

		#region Constructor

		public Sector_List()
		{
			// Get required services
			sectorService = DependencyResolver.Kernel.Get<ISectorService>();

			// Initialize form components
			InitializeComponent();

			this.DataContext = this;

			Thread displayThread = new Thread(() => SyncData());
			displayThread.IsBackground = true;
			displayThread.Start();
		}

		#endregion

		#region Display data

		private void btnRefresh_Click(object sender, RoutedEventArgs e)
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

		public void DisplayData()
		{
			SectorDataLoading = true;

			SectorListResponse response = new SectorSQLiteRepository()
				.GetSectorsByPage(MainWindow.CurrentCompanyId, SectorSearchObject, currentPage, itemsPerPage);

			if (response.Success)
			{
				SectorsFromDB = new ObservableCollection<SectorViewModel>(response.Sectors ?? new List<SectorViewModel>());
				totalItems = response.TotalItems;
			}
			else
			{
				SectorsFromDB = new ObservableCollection<SectorViewModel>();
				totalItems = 0;
				MainWindow.ErrorMessage = response.Message;
			}

			int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
			int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

			PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

			SectorDataLoading = false;
		}

		private void SyncData()
		{
			RefreshButtonEnabled = false;

			RefreshButtonContent = ((string)Application.Current.FindResource("Sektori_TriTacke"));
            new SectorSQLiteRepository().Sync(sectorService);

			DisplayData();

			RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
		}

		private void dgSectors_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = (e.Row.GetIndex() + 1).ToString();
		}

		#endregion


		#region Add sector, edit sector and delete sector

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			SectorViewModel sector = new SectorViewModel();
			sector.Identifier = Guid.NewGuid();

			Sector_AddEdit addEditForm = new Sector_AddEdit(sector, true);
			addEditForm.SectorCreatedUpdated += new SectorHandler(SyncData);
			FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_sektrorima")), 95, addEditForm);
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentSector == null)
			{
				MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_sektor_za_izmenuUzvičnik"));
                return;
			}

			Sector_AddEdit addEditForm = new Sector_AddEdit(CurrentSector, false);
			addEditForm.SectorCreatedUpdated += new SectorHandler(SyncData);
			FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_sektrorima")), 95, addEditForm);
		}

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSector == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_brisanjeUzvičnik"));
                return;
            }

            // Delete data
            var result = sectorService.Delete(CurrentSector.Identifier);
            if (result.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_obrisaniUzvičnik"));

                Thread displayThread = new Thread(() => SyncData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
            {
                MainWindow.ErrorMessage = result.Message;
            }
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
