﻿using Ninject;
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
	/// <summary>
	/// Interaction logic for LicenceType_List_AddEdit.xaml
	/// </summary>
	public partial class LicenceType_List_AddEdit : UserControl, INotifyPropertyChanged
	{
		#region Attributes

		#region Services
		ILicenceTypeService licenceTypeService;
		#endregion

		#region Events
		public event LicenceTypeHandler LicenceTypeCreatedUpdated;
		#endregion

		#region CurrentLicenceType
		private LicenceTypeViewModel _CurrentLicenceType = new LicenceTypeViewModel();

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

		public LicenceType_List_AddEdit(LicenceTypeViewModel licenceTypeViewModel, bool isCreateProcess, bool isPopup = false)
		{
			// Initialize service
			this.licenceTypeService = DependencyResolver.Kernel.Get<ILicenceTypeService>();

			InitializeComponent();

			this.DataContext = this;

			CurrentLicenceType = licenceTypeViewModel;
			IsCreateProcess = isCreateProcess;
			IsPopup = isPopup;
		}

		#endregion

		#region  Save and Cancel button

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			#region Validation

			if (String.IsNullOrEmpty(CurrentLicenceType.Category))
			{
				MainWindow.WarningMessage = "Obavezno polje: Ime dozvole";
				return;
			}

			#endregion

			Thread th = new Thread(() =>
			{
				SaveButtonContent = " Čuvanje u toku... ";
				SaveButtonEnabled = false;

				CurrentLicenceType.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
				CurrentLicenceType.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

				CurrentLicenceType.IsSynced = false;
				CurrentLicenceType.UpdatedAt = DateTime.Now;

				LicenceTypeResponse response = new LicenceTypeSQLiteRepository().Delete(CurrentLicenceType.Identifier);
				response = new LicenceTypeSQLiteRepository().Create(CurrentLicenceType);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
					SaveButtonContent = " Sačuvaj ";
					SaveButtonEnabled = true;
					return;
				}

				response = licenceTypeService.Create(CurrentLicenceType);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
					SaveButtonContent = " Sačuvaj ";
					SaveButtonEnabled = true;
				}

				if (response.Success)
				{
					new LicenceTypeSQLiteRepository().UpdateSyncStatus(response.LicenceType.Identifier, response.LicenceType.Id, true);
					MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
					SaveButtonContent = " Sačuvaj ";
					SaveButtonEnabled = true;

					LicenceTypeCreatedUpdated();

					if (IsCreateProcess)
					{
						CurrentLicenceType = new LicenceTypeViewModel();
						CurrentLicenceType.Identifier = Guid.NewGuid();

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