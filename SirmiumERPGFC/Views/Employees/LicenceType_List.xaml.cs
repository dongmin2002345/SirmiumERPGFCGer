using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Employees;
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

namespace SirmiumERPGFC.Views.Employees
{
	public delegate void LicenceTypeHandler(); 
	/// <summary>
	/// Interaction logic for LicenceType_List.xaml
	/// </summary>
	public partial class LicenceType_List : UserControl, INotifyPropertyChanged
	{
		#region Attributes

		#region Services
		ILicenceTypeService licenceTypeService;
		#endregion

		#region BanksFromDB
		private ObservableCollection<LicenceTypeViewModel> _LicenceTypesFromDB;

		public ObservableCollection<LicenceTypeViewModel> LicenceTypesFromDB
		{
			get { return _LicenceTypesFromDB; }
			set
			{
				if (_LicenceTypesFromDB != value)
				{
					_LicenceTypesFromDB = value;
					NotifyPropertyChanged("LicenceTypesFromDB");
				}
			}
		}
		#endregion

		#region CurrentLicenceType
		private LicenceTypeViewModel _CurrentLicenceType;

		public LicenceTypeViewModel CurrentLicenceType
		{
			get { return _CurrentLicenceType; }
			set
			{
				if (_CurrentLicenceType != value)
				{
					_CurrentLicenceType = value;
					NotifyPropertyChanged("CurrentLicenceType");
				}
			}
		}
		#endregion

		#region LicenceTypeSearchObject
		private LicenceTypeViewModel _LicenceTypeSearchObject = new LicenceTypeViewModel();

		public LicenceTypeViewModel LicenceTypeSearchObject
		{
			get { return _LicenceTypeSearchObject; }
			set
			{
				if (_LicenceTypeSearchObject != value)
				{
					_LicenceTypeSearchObject = value;
					NotifyPropertyChanged("LicenceTypeSearchObject");
				}
			}
		}
		#endregion

		#region LicenceTypeDataLoading
		private bool _LicenceTypeDataLoading = true;

		public bool LicenceTypeDataLoading
		{
			get { return _LicenceTypeDataLoading; }
			set
			{
				if (_LicenceTypeDataLoading != value)
				{
					_LicenceTypeDataLoading = value;
					NotifyPropertyChanged("LicenceTypeDataLoading");
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
		private string _RefreshButtonContent = " Osveži ";

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

		public LicenceType_List()
		{
			// Get required services
			this.licenceTypeService = DependencyResolver.Kernel.Get<ILicenceTypeService>();

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

				MainWindow.SuccessMessage = "Podaci su uspešno sinhronizovani!";
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
			LicenceTypeDataLoading = true;

			LicenceTypeListResponse response = new LicenceTypeSQLiteRepository()
				.GetLicenceTypesByPage(MainWindow.CurrentCompanyId, LicenceTypeSearchObject, currentPage, itemsPerPage);

			if (response.Success)
			{
				LicenceTypesFromDB = new ObservableCollection<LicenceTypeViewModel>(response.LicenceTypes ?? new List<LicenceTypeViewModel>());
				totalItems = response.TotalItems;
			}
			else
			{
				LicenceTypesFromDB = new ObservableCollection<LicenceTypeViewModel>();
				totalItems = 0;
				MainWindow.ErrorMessage = response.Message;
			}

			int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
			int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

			PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

			LicenceTypeDataLoading = false;
		}

		private void SyncData()
		{
			RefreshButtonEnabled = false;

			RefreshButtonContent = " Drzava ... ";
			new LicenceTypeSQLiteRepository().Sync(licenceTypeService);

			DisplayData();

			RefreshButtonContent = " Osveži ";
			RefreshButtonEnabled = true;
		}

		#endregion

		#region Add, edit and delete methods

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{

			LicenceTypeViewModel licenceType = new LicenceTypeViewModel();
			licenceType.Identifier = Guid.NewGuid();

			LicenceType_List_AddEdit addEditForm = new LicenceType_List_AddEdit(licenceType, true);
			addEditForm.LicenceTypeCreatedUpdated += new LicenceTypeHandler(SyncData);
			FlyoutHelper.OpenFlyout(this, "Podaci o dozvolu", 95, addEditForm);
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentLicenceType == null)
			{
				MainWindow.WarningMessage = "Morate odabrati dozvolu za izmenu!";
				return;
			}

			LicenceType_List_AddEdit addEditForm = new LicenceType_List_AddEdit(CurrentLicenceType, false);
			addEditForm.LicenceTypeCreatedUpdated += new LicenceTypeHandler(SyncData);
			FlyoutHelper.OpenFlyout(this, "Podaci o dozvoli", 95, addEditForm);
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentLicenceType == null)
			{
				MainWindow.WarningMessage = "Morate odabrati dozvolu za brisanje!";
				return;
			}

			SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

			// Create confirmation window
			DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("dozvola", CurrentLicenceType.Name + CurrentLicenceType.Code);
			var showDialog = deleteConfirmationForm.ShowDialog();
			if (showDialog != null && showDialog.Value)
			{
				LicenceTypeResponse response = licenceTypeService.Delete(CurrentLicenceType.Identifier);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
					SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
					return;
				}

				response = new LicenceTypeSQLiteRepository().Delete(CurrentLicenceType.Identifier);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
					SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
					return;
				}

				MainWindow.SuccessMessage = "Dozvola je uspešno obrisana!";

				Thread displayThread = new Thread(() => SyncData());
				displayThread.IsBackground = true;
				displayThread.Start();
			}

			SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
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
