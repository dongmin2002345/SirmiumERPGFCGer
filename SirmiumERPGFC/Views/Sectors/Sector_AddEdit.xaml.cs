﻿using Ninject;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Sectors;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Sectors;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.Sectors
{
	public partial class Sector_AddEdit : UserControl, INotifyPropertyChanged
	{
		#region Attributes

		#region Services
		ISectorService sectorService;
		#endregion


		#region Event
		public event SectorHandler SectorCreatedUpdated;
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
		private string _SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));

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

		public Sector_AddEdit(SectorViewModel sectorViewModel, bool isCreateProcess, bool isPopup = false)
		{
			sectorService = DependencyResolver.Kernel.Get<ISectorService>();

			// Initialize form components
			InitializeComponent();

			this.DataContext = this;

			CurrentSector = sectorViewModel;
			IsCreateProcess = isCreateProcess;
			IsPopup = isPopup;
		}

		#endregion

		#region Cancel and Save buttons

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			#region Validation

			if (CurrentSector.SecondCode == null)
			{
				MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Šifra"));
                return;
			}

			if (String.IsNullOrEmpty(CurrentSector.Name))
			{
				MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv_sektora"));
                return;
			}

            if (CurrentSector.Country == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Ime_drzave"));
                return;
            }

			#endregion

			Thread th = new Thread(() =>
			{
				SaveButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SaveButtonEnabled = false;

                CurrentSector.IsSynced = false;

                CurrentSector.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
				CurrentSector.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };


                SectorResponse  response = sectorService.Create(CurrentSector);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_čuvanja_na_serveruUzvičnik"));
                    SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));
                    SaveButtonEnabled = true;
				}

				if (response.Success)
				{
					MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));
                    SaveButtonEnabled = true;

					SectorCreatedUpdated();

					if (IsCreateProcess)
					{
						CurrentSector = new SectorViewModel();
						CurrentSector.Identifier = Guid.NewGuid();

						Application.Current.Dispatcher.BeginInvoke(
							System.Windows.Threading.DispatcherPriority.Normal,
							new Action(() =>
							{
								txtSecondCode.Focus();
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
