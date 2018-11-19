using Ninject;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.InputInvoices;
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

	public partial class InputInvoiceAddEdit : UserControl, INotifyPropertyChanged
	{
		#region Attributes

		#region Services
		IInputInvoiceService InputInvoiceService;
		#endregion

		#region Events
		public event InputInvoiceHandler InputInvoiceCreatedUpdated;
		#endregion

		#region currentInputInvoice
		private InputInvoiceViewModel _currentInputInvoice;

		public InputInvoiceViewModel currentInputInvoice
		{
			get { return _currentInputInvoice; }
			set
			{
				if (_currentInputInvoice != value)
				{
					_currentInputInvoice = value;
					NotifyPropertyChanged("currentInputInvoice");
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
						   ChooseStatusConverter.Choose,
						   ChooseStatusConverter.ChooseO,
						   ChooseStatusConverter.ChooseB,
				});
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

		public InputInvoiceAddEdit(InputInvoiceViewModel InputInvoiceViewModel, bool isCreateProcess, bool isPopup = false)
		{
			// Initialize service
			InputInvoiceService = DependencyResolver.Kernel.Get<IInputInvoiceService>();

			InitializeComponent();

			this.DataContext = this;

			currentInputInvoice = InputInvoiceViewModel;
			IsCreateProcess = isCreateProcess;
			IsPopup = isPopup;
		}

		#endregion

		#region Save and Cancel button

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			#region Validation

			if (String.IsNullOrEmpty(currentInputInvoice.InvoiceNumber))
			{
				MainWindow.WarningMessage = "Obavezno polje: Broj fakture";
				return;
			}

			#endregion

			Thread th = new Thread(() =>
			{
				SaveButtonContent = " Čuvanje u toku... ";
				SaveButtonEnabled = false;

				currentInputInvoice.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
				currentInputInvoice.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

				currentInputInvoice.IsSynced = false;
				currentInputInvoice.UpdatedAt = DateTime.Now;

				InputInvoiceResponse response = new InputInvoiceSQLiteRepository().Delete(currentInputInvoice.Identifier);
				response = new InputInvoiceSQLiteRepository().Create(currentInputInvoice);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
					SaveButtonContent = " Sačuvaj ";
					SaveButtonEnabled = true;
					return;
				}

				response = InputInvoiceService.Create(currentInputInvoice);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
					SaveButtonContent = " Sačuvaj ";
					SaveButtonEnabled = true;
				}

				if (response.Success)
				{
					new InputInvoiceSQLiteRepository().UpdateSyncStatus(response.InputInvoice.Identifier, response.InputInvoice.Id, true);
					MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
					SaveButtonContent = " Sačuvaj ";
					SaveButtonEnabled = true;

					InputInvoiceCreatedUpdated();

					if (IsCreateProcess)
					{
						currentInputInvoice = new InputInvoiceViewModel();
						currentInputInvoice.Identifier = Guid.NewGuid();

						Application.Current.Dispatcher.BeginInvoke(
							System.Windows.Threading.DispatcherPriority.Normal,
							new Action(() =>
							{
								txtAddress.Focus();
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
			InputInvoiceCreatedUpdated();

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
