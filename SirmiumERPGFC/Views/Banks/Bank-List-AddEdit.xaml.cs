using Ninject;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Messages.Banks;
using ServiceInterfaces.ViewModels.Banks;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Banks;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for Bank_List_AddEdit.xaml
    /// </summary>
    public partial class Bank_List_AddEdit : UserControl, INotifyPropertyChanged
	{
		//public Bank_List_AddEdit()
		//{
		//    InitializeComponent();
		//}

		//private void btnSave_Click(object sender, RoutedEventArgs e)
		//{

		//}

		//private void btnCancel_Click(object sender, RoutedEventArgs e)
		//{
		//    FlyoutHelper.CloseFlyout(this);
		//}

		#region Attributes

		#region Services
		IBankService bankService;
		#endregion

		#region Events
		public event BankHandler BankCreatedUpdated;
		#endregion

		#region CurrentBank
		private BankViewModel _CurrentBank = new BankViewModel();

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


		#region SaveButtonContent
		private string _SaveButtonContent = " Sačuvaj ";

		public string SaveButtonContent
		{
			get { return _SaveButtonContent; }
			set
			{
				if (_SaveButtonContent != value)
				{
					_SaveButtonContent = value;
					NotifyPropertyChanged("SaveButtonContent");
				}
			}
		}
		#endregion

		#region SaveButtonEnabled
		private bool _SaveButtonEnabled = true;

		public bool SaveButtonEnabled
		{
			get { return _SaveButtonEnabled; }
			set
			{
				if (_SaveButtonEnabled != value)
				{
					_SaveButtonEnabled = value;
					NotifyPropertyChanged("SaveButtonEnabled");
				}
			}
		}
		#endregion

		#endregion

		#region Constructor

		public Bank_List_AddEdit(BankViewModel bankViewModel, bool isCreateProcess, bool isPopup = false)
		{
			// Initialize service
			this.bankService = DependencyResolver.Kernel.Get<IBankService>();

			InitializeComponent();

			this.DataContext = this;

			CurrentBank = bankViewModel;
			IsCreateProcess = isCreateProcess;
			IsPopup = isPopup;
		}

		#endregion

		#region  Save and Cancel button

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			#region Validation

			if (String.IsNullOrEmpty(CurrentBank.Name))
			{
				MainWindow.WarningMessage = "Obavezno polje: Ime banke";
				return;
			}

			#endregion

			Thread th = new Thread(() =>
			{
				SaveButtonContent = " Čuvanje u toku... ";
				SaveButtonEnabled = false;

				CurrentBank.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
				CurrentBank.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

				CurrentBank.IsSynced = false;
				CurrentBank.UpdatedAt = DateTime.Now;

				BankResponse response = new BankSQLiteRepository().Delete(CurrentBank.Identifier);
				response = new BankSQLiteRepository().Create(CurrentBank);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
					SaveButtonContent = " Sačuvaj ";
					SaveButtonEnabled = true;
					return;
				}

				response = bankService.Create(CurrentBank);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
					SaveButtonContent = " Sačuvaj ";
					SaveButtonEnabled = true;
				}

				if (response.Success)
				{
					new BankSQLiteRepository().UpdateSyncStatus(response.Bank.Identifier, response.Bank.Id, true);
					MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
					SaveButtonContent = " Sačuvaj ";
					SaveButtonEnabled = true;

					BankCreatedUpdated();

					if (IsCreateProcess)
					{
						CurrentBank = new BankViewModel();
						CurrentBank.Identifier = Guid.NewGuid();

						Application.Current.Dispatcher.BeginInvoke(
							System.Windows.Threading.DispatcherPriority.Normal,
							new Action(() =>
							{
								txtName.Focus();
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

			txtName.Focus();
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
