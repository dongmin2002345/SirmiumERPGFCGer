using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Reports.Employees;
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
	public delegate void PhysicalPersonHandler();
	public partial class PhysicalPerson_List : UserControl, INotifyPropertyChanged
	{
		#region Attributes

		#region Services
		IPhysicalPersonService physicalPersonService;
		#endregion

		#region PhysicalPersonsFromDB
		private ObservableCollection<PhysicalPersonViewModel> _PhysicalPersonsFromDB;

		public ObservableCollection<PhysicalPersonViewModel> PhysicalPersonsFromDB
		{
			get { return _PhysicalPersonsFromDB; }
			set
			{
				if (_PhysicalPersonsFromDB != value)
				{
					_PhysicalPersonsFromDB = value;
					NotifyPropertyChanged("PhysicalPersonsFromDB");
				}
			}
		}
		#endregion

		#region CurrentPhysicalPerson
		private PhysicalPersonViewModel _CurrentPhysicalPerson;

		public PhysicalPersonViewModel CurrentPhysicalPerson
		{
			get { return _CurrentPhysicalPerson; }
			set
			{
				if (_CurrentPhysicalPerson != value)
				{
					_CurrentPhysicalPerson = value;
					NotifyPropertyChanged("CurrentPhysicalPerson");
				}
			}
		}
		#endregion

		#region PhysicalPersonSearchObject
		private PhysicalPersonViewModel _PhysicalPersonSearchObject = new PhysicalPersonViewModel();

		public PhysicalPersonViewModel PhysicalPersonSearchObject
		{
			get { return _PhysicalPersonSearchObject; }
			set
			{
				if (_PhysicalPersonSearchObject != value)
				{
					_PhysicalPersonSearchObject = value;
					NotifyPropertyChanged("PhysicalPersonSearchObject");
				}
			}
		}
		#endregion

		#region PhysicalPersonDataLoading
		private bool _PhysicalPersonDataLoading = true;

		public bool PhysicalPersonDataLoading
		{
			get { return _PhysicalPersonDataLoading; }
			set
			{
				if (_PhysicalPersonDataLoading != value)
				{
					_PhysicalPersonDataLoading = value;
					NotifyPropertyChanged("PhysicalPersonDataLoading");
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
		private string _RefreshButtonContent = " OSVEŽI ";

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

		public PhysicalPerson_List()
		{
			// Get required services
			this.physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();

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
			PhysicalPersonDataLoading = true;

			PhysicalPersonListResponse response = new PhysicalPersonSQLiteRepository()
				.GetPhysicalPersonsByPage(MainWindow.CurrentCompanyId, PhysicalPersonSearchObject, currentPage, itemsPerPage);

			if (response.Success)
			{
				PhysicalPersonsFromDB = new ObservableCollection<PhysicalPersonViewModel>(response.PhysicalPersons ?? new List<PhysicalPersonViewModel>());
				totalItems = response.TotalItems;
			}
			else
			{
				PhysicalPersonsFromDB = new ObservableCollection<PhysicalPersonViewModel>();
				totalItems = 0;
				MainWindow.ErrorMessage = response.Message;
			}

			int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
			int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

			PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

			PhysicalPersonDataLoading = false;
		}

		private void SyncData()
		{
			RefreshButtonEnabled = false;

			RefreshButtonContent = " Fizička lica ... ";
			new PhysicalPersonSQLiteRepository().Sync(physicalPersonService);

			DisplayData();

			RefreshButtonContent = " OSVEŽI ";
			RefreshButtonEnabled = true;
		}

		#endregion

		#region Add, edit and delete methods

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{

			PhysicalPersonViewModel physicalPerson = new PhysicalPersonViewModel();
			physicalPerson.Identifier = Guid.NewGuid();

			PhysicalPerson_AddEdit addEditForm = new PhysicalPerson_AddEdit(physicalPerson, true);
			addEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncData);
			FlyoutHelper.OpenFlyout(this, "Podaci o fizičkim licima", 95, addEditForm);
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentPhysicalPerson == null)
			{
				MainWindow.WarningMessage = "Morate odabrati fizičko lice za izmenu!";
				return;
			}

			PhysicalPerson_AddEdit addEditForm = new PhysicalPerson_AddEdit(CurrentPhysicalPerson, false);
			addEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncData);
			FlyoutHelper.OpenFlyout(this, "Podaci o fizičkom licu", 95, addEditForm);
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentPhysicalPerson == null)
			{
				MainWindow.WarningMessage = "Morate odabrati fizičko lice za brisanje!";
				return;
			}

			SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

			// Create confirmation window
			DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("banka", CurrentPhysicalPerson.Name + CurrentPhysicalPerson.Code);
			var showDialog = deleteConfirmationForm.ShowDialog();
			if (showDialog != null && showDialog.Value)
			{
				PhysicalPersonResponse response = physicalPersonService.Delete(CurrentPhysicalPerson.Identifier);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
					SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
					return;
				}

				response = new PhysicalPersonSQLiteRepository().Delete(CurrentPhysicalPerson.Identifier);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
					SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
					return;
				}

				MainWindow.SuccessMessage = "Banka je uspešno obrisana!";

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

		private void btnExcel_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                PhysicalPersonsExcelReport.Show(PhysicalPersonsFromDB.ToList());
            }
            catch(Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
		}
	}
}
