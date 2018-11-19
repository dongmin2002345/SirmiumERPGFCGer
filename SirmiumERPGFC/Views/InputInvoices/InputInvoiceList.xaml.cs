using Ninject;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.InputInvoices;
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
using WpfAppCommonCode.Converters;

namespace SirmiumERPGFC.Views.InputInvoices
{
	public delegate void InputInvoiceHandler();
	public partial class InputInvoiceList : UserControl, INotifyPropertyChanged
	{
		#region Attributes

		#region Services
		IInputInvoiceService inputInvoiceService;
		#endregion

		#region InputInvoiceSearchObject
		private InputInvoiceViewModel _InputInvoiceSearchObject = new InputInvoiceViewModel();

		public InputInvoiceViewModel InputInvoiceSearchObject
		{
			get { return _InputInvoiceSearchObject; }
			set
			{
				if (_InputInvoiceSearchObject != value)
				{
					_InputInvoiceSearchObject = value;
					NotifyPropertyChanged("InputInvoiceSearchObject");
				}
			}
		}
		#endregion

		#region InputInvoicesFromDB
		private ObservableCollection<InputInvoiceViewModel> _InputInvoicesFromDB;

		public ObservableCollection<InputInvoiceViewModel> InputInvoicesFromDB
		{
			get { return _InputInvoicesFromDB; }
			set
			{
				if (_InputInvoicesFromDB != value)
				{
					_InputInvoicesFromDB = value;
					NotifyPropertyChanged("InputInvoicesFromDB");
				}
			}
		}
		#endregion

		#region CurrentInputInvoice
		private InputInvoiceViewModel _CurrentInputInvoice;

		public InputInvoiceViewModel CurrentInputInvoice
		{
			get { return _CurrentInputInvoice; }
			set
			{
				if (_CurrentInputInvoice != value)
				{
					_CurrentInputInvoice = value;
					NotifyPropertyChanged("CurrentInputInvoice");
				}
			}
		}
		#endregion

		#region InputInvoiceDataLoading
		private bool _InputInvoiceDataLoading = true;

		public bool InputInvoiceDataLoading
		{
			get { return _InputInvoiceDataLoading; }
			set
			{
				if (_InputInvoiceDataLoading != value)
				{
					_InputInvoiceDataLoading = value;
					NotifyPropertyChanged("InputInvoiceDataLoading");
				}
			}
		}
		#endregion

		#region StatusOptions
		public ObservableCollection<String> StatusOptions
		{
			get
			{
				return new ObservableCollection<String>(new List<string>() {
						   ChooseStatusConverter.ChooseO,
						   ChooseStatusConverter.ChooseB,

				});
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

		/// <summary>
		/// FinancialInvoiceList constructor
		/// </summary>
		public InputInvoiceList()
		{
			// Get required service
			this.inputInvoiceService = DependencyResolver.Kernel.Get<IInputInvoiceService>();

			// Draw all components
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
			InputInvoiceDataLoading = true;

			InputInvoiceListResponse response = new InputInvoiceSQLiteRepository()
				.GetInputInvoicesByPage(MainWindow.CurrentCompanyId, InputInvoiceSearchObject, currentPage, itemsPerPage);

			if (response.Success)
			{
				InputInvoicesFromDB = new ObservableCollection<InputInvoiceViewModel>(response.InputInvoices ?? new List<InputInvoiceViewModel>());
				totalItems = response.TotalItems;
			}
			else
			{
				InputInvoicesFromDB = new ObservableCollection<InputInvoiceViewModel>();
				totalItems = 0;
				MainWindow.ErrorMessage = response.Message;
			}

			int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
			int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

			PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

			InputInvoiceDataLoading = false;
		}

		private void SyncData()
		{
			RefreshButtonEnabled = false;

			RefreshButtonContent = " Ulazne fakture ... ";
			new InputInvoiceSQLiteRepository().Sync(inputInvoiceService);

			DisplayData();

			RefreshButtonContent = " Osveži ";
			RefreshButtonEnabled = true;
		}

		#endregion

		#region Add, edit, delete, lock and cancel

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			InputInvoiceViewModel inputInvoice = new InputInvoiceViewModel();
			inputInvoice.Identifier = Guid.NewGuid();
			inputInvoice.InvoiceDate = DateTime.Now;

			InputInvoiceAddEdit addEditForm = new InputInvoiceAddEdit(inputInvoice, true, false);
			addEditForm.InputInvoiceCreatedUpdated += new InputInvoiceHandler(SyncData);
			FlyoutHelper.OpenFlyout(this, "ULAZNE FAKTURE", 95, addEditForm);
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentInputInvoice == null)
			{
				MainWindow.WarningMessage = "Nije moguće menjati ulayne fakture!";
				return;
			}

			InputInvoiceAddEdit InputInvoiceAddEditForm = new InputInvoiceAddEdit(CurrentInputInvoice, false);
			InputInvoiceAddEditForm.InputInvoiceCreatedUpdated += new InputInvoiceHandler(SyncData);
			FlyoutHelper.OpenFlyout(this, "ULAZNE FAKTURE", 95, InputInvoiceAddEditForm);
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentInputInvoice == null)
			{
				MainWindow.WarningMessage = "Nije moguće menjati ulazne fakture!";
				return;
			}

			SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

			// Create confirmation window
			DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("ULAZNE FAKTURE", CurrentInputInvoice.Code + CurrentInputInvoice.InvoiceNumber);
			var showDialog = deleteConfirmationForm.ShowDialog();
			if (showDialog != null && showDialog.Value)
			{
				InputInvoiceResponse response = inputInvoiceService.Delete(CurrentInputInvoice.Identifier);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
					SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
					return;
				}

				response = new InputInvoiceSQLiteRepository().Delete(CurrentInputInvoice.Identifier);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
					SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
					return;
				}

				MainWindow.SuccessMessage = "Ulazni avansni račun je uspešno obrisana!";

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
		public void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
