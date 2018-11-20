using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Employees;
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

namespace SirmiumERPGFC.Views.Employees
{
	/// <summary>
	/// Interaction logic for PhysicalPerson_AddEdit.xaml
	/// </summary>
	public partial class PhysicalPerson_AddEdit : UserControl, INotifyPropertyChanged
	{
		#region Attributes

		#region Services
		IPhysicalPersonService physicalPersonService;
		#endregion

		#region Events
		public event PhysicalPersonHandler PhysicalPersonCreatedUpdated;
		#endregion

		#region CurrentPhysicalPerson
		private PhysicalPersonViewModel _CurrentPhysicalPerson = new PhysicalPersonViewModel();

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

		#region GenderOptions
		public ObservableCollection<String> GenderOptions
		{
			get
			{
				return new ObservableCollection<String>(new List<string>() {
						   GenderConverter.Choose,
						   GenderConverter.ChooseM,
						   GenderConverter.ChooseF});
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
		private string _SaveButtonContent = " SAČUVAJ ";

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

		public PhysicalPerson_AddEdit(PhysicalPersonViewModel physicalPersonViewModel, bool isCreateProcess, bool isPopup = false)
		{
			// Initialize service
			this.physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();

			InitializeComponent();

			this.DataContext = this;

			CurrentPhysicalPerson = physicalPersonViewModel;
			IsCreateProcess = isCreateProcess;
			IsPopup = isPopup;
		}

		#endregion

		#region  Save and Cancel button

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			#region Validation

			if (String.IsNullOrEmpty(CurrentPhysicalPerson.Name))
			{
				MainWindow.WarningMessage = "Obavezno polje: Ime fizičkog lica";
				return;
			}

			#endregion

			Thread th = new Thread(() =>
			{
				SaveButtonContent = " Čuvanje u toku... ";
				SaveButtonEnabled = false;

				CurrentPhysicalPerson.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
				CurrentPhysicalPerson.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

				CurrentPhysicalPerson.IsSynced = false;
				CurrentPhysicalPerson.UpdatedAt = DateTime.Now;

				PhysicalPersonResponse response = new PhysicalPersonSQLiteRepository().Delete(CurrentPhysicalPerson.Identifier);
				response = new PhysicalPersonSQLiteRepository().Create(CurrentPhysicalPerson);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
					SaveButtonContent = " SAČUVAJ ";
					SaveButtonEnabled = true;
					return;
				}

				response = physicalPersonService.Create(CurrentPhysicalPerson);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
					SaveButtonContent = " SAČUVAJ ";
					SaveButtonEnabled = true;
				}

				if (response.Success)
				{
					new PhysicalPersonSQLiteRepository().UpdateSyncStatus(response.PhysicalPerson.Identifier, response.PhysicalPerson.Id, true);
					MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
					SaveButtonContent = " SAČUVAJ ";
					SaveButtonEnabled = true;

					PhysicalPersonCreatedUpdated();

					if (IsCreateProcess)
					{
						CurrentPhysicalPerson = new PhysicalPersonViewModel();
						CurrentPhysicalPerson.Identifier = Guid.NewGuid();

						Application.Current.Dispatcher.BeginInvoke(
							System.Windows.Threading.DispatcherPriority.Normal,
							new Action(() =>
							{
								txtCode.Focus();
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
