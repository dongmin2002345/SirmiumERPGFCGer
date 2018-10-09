using Ninject;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Messages.Banks;
using ServiceInterfaces.ViewModels.Banks;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Banks;
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

namespace SirmiumERPGFC.Views.Banks
{
	public delegate void BankHandler();
	/// <summary>
	/// Interaction logic for Bank_List.xaml
	/// </summary>
	public partial class Bank_List : UserControl, INotifyPropertyChanged
	{
		//public Bank_List()
		//{
		//    InitializeComponent();
		//}

		//private void btnSearch_Click(object sender, RoutedEventArgs e)
		//{

		//}

		//private void btnAdd_Click(object sender, RoutedEventArgs e)
		//{
		//    FlyoutHelper.OpenFlyout(this, "Podaci o bankama", 95, new Bank_List_AddEdit());
		//}

		//private void btnRefresh_Click(object sender, RoutedEventArgs e)
		//{

		//}

		//private void btnFirstPage_Click(object sender, RoutedEventArgs e)
		//{

		//}

		//private void btnPrevPage_Click(object sender, RoutedEventArgs e)
		//{

		//}

		//private void btnNextPage_Click(object sender, RoutedEventArgs e)
		//{

		//}

		//private void btnLastPage_Click(object sender, RoutedEventArgs e)
		//{

		//}

		//private void btnEdit_Click(object sender, RoutedEventArgs e)
		//{

		//}

		//private void btnDelete_Click(object sender, RoutedEventArgs e)
		//{

		//}
		#region Attributes

		#region Services
		IBankService bankService;
		#endregion

		#region BanksFromDB
		private ObservableCollection<BankViewModel> _BanksFromDB;

		public ObservableCollection<BankViewModel> BanksFromDB
		{
			get { return _BanksFromDB; }
			set
			{
				if (_BanksFromDB != value)
				{
					_BanksFromDB = value;
					NotifyPropertyChanged("BanksFromDB");
				}
			}
		}
		#endregion

		#region CurrentBank
		private BankViewModel _CurrentBank;

		public BankViewModel CurrentBank
		{
			get { return _CurrentBank; }
			set
			{
				if (_CurrentBank != value)
				{
					_CurrentBank = value;
					NotifyPropertyChanged("CurrentBank");
				}
			}
		}
		#endregion

		#region BankSearchObject
		private BankViewModel _BankSearchObject = new BankViewModel();

		public BankViewModel BankSearchObject
		{
			get { return _BankSearchObject; }
			set
			{
				if (_BankSearchObject != value)
				{
					_BankSearchObject = value;
					NotifyPropertyChanged("BankSearchObject");
				}
			}
		}
		#endregion

		#region BankDataLoading
		private bool _BankDataLoading = true;

		public bool BankDataLoading
		{
			get { return _BankDataLoading; }
			set
			{
				if (_BankDataLoading != value)
				{
					_BankDataLoading = value;
					NotifyPropertyChanged("BankDataLoading");
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

		public Bank_List()
		{
			// Get required services
			this.bankService = DependencyResolver.Kernel.Get<IBankService>();

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
			BankDataLoading = true;

			BankListResponse response = new BankSQLiteRepository()
				.GetBanksByPage(MainWindow.CurrentCompanyId, BankSearchObject, currentPage, itemsPerPage);

			if (response.Success)
			{
				BanksFromDB = new ObservableCollection<BankViewModel>(response.Banks ?? new List<BankViewModel>());
				totalItems = response.TotalItems;
			}
			else
			{
				BanksFromDB = new ObservableCollection<BankViewModel>();
				totalItems = 0;
				MainWindow.ErrorMessage = response.Message;
			}

			int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
			int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

			PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

			BankDataLoading = false;
		}

		private void SyncData()
		{
			RefreshButtonEnabled = false;

			RefreshButtonContent = " Drzava ... ";
			new BankSQLiteRepository().Sync(bankService);

			DisplayData();

			RefreshButtonContent = " Osveži ";
			RefreshButtonEnabled = true;
		}

		#endregion

		#region Add, edit and delete methods

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{

			BankViewModel bank = new BankViewModel();
			bank.Identifier = Guid.NewGuid();

			Bank_List_AddEdit addEditForm = new Bank_List_AddEdit(bank, true);
			addEditForm.BankCreatedUpdated += new BankHandler(SyncData);
			FlyoutHelper.OpenFlyout(this, "Podaci o drzavama", 95, addEditForm);
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentBank == null)
			{
				MainWindow.WarningMessage = "Morate odabrati drzavu za izmenu!";
				return;
			}

			Bank_List_AddEdit addEditForm = new Bank_List_AddEdit(CurrentBank, false);
			addEditForm.BankCreatedUpdated += new BankHandler(SyncData);
			FlyoutHelper.OpenFlyout(this, "Podaci o drzavi", 95, addEditForm);
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentBank == null)
			{
				MainWindow.WarningMessage = "Morate odabrati banku za brisanje!";
				return;
			}

			SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

			// Create confirmation window
			DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("banka", CurrentBank.Name + CurrentBank.Code);
			var showDialog = deleteConfirmationForm.ShowDialog();
			if (showDialog != null && showDialog.Value)
			{
				BankResponse response = bankService.Delete(CurrentBank.Identifier);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
					SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
					return;
				}

				response = new BankSQLiteRepository().Delete(CurrentBank.Identifier);
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
	}
}
