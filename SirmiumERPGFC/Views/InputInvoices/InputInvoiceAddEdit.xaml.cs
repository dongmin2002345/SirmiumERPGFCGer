using Ninject;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
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
                           ChooseStatusConverter.ChooseM,
                });
			}
		}
        #endregion


        #region InputInvoiceNotesFromDB
        private ObservableCollection<InputInvoiceNoteViewModel> _InputInvoiceNotesFromDB;

        public ObservableCollection<InputInvoiceNoteViewModel> InputInvoiceNotesFromDB
        {
            get { return _InputInvoiceNotesFromDB; }
            set
            {
                if (_InputInvoiceNotesFromDB != value)
                {
                    _InputInvoiceNotesFromDB = value;
                    NotifyPropertyChanged("InputInvoiceNotesFromDB");
                }
            }
        }
        #endregion

        #region CurrentInputInvoiceNoteForm
        private InputInvoiceNoteViewModel _CurrentInputInvoiceNoteForm = new InputInvoiceNoteViewModel() { NoteDate = DateTime.Now };

        public InputInvoiceNoteViewModel CurrentInputInvoiceNoteForm
        {
            get { return _CurrentInputInvoiceNoteForm; }
            set
            {
                if (_CurrentInputInvoiceNoteForm != value)
                {
                    _CurrentInputInvoiceNoteForm = value;
                    NotifyPropertyChanged("CurrentInputInvoiceNoteForm");
                }
            }
        }
        #endregion

        #region CurrentInputInvoiceNoteDG
        private InputInvoiceNoteViewModel _CurrentInputInvoiceNoteDG;

        public InputInvoiceNoteViewModel CurrentInputInvoiceNoteDG
        {
            get { return _CurrentInputInvoiceNoteDG; }
            set
            {
                if (_CurrentInputInvoiceNoteDG != value)
                {
                    _CurrentInputInvoiceNoteDG = value;
                    NotifyPropertyChanged("CurrentInputInvoiceNoteDG");
                }
            }
        }
        #endregion

        #region InputInvoiceNoteDataLoading
        private bool _InputInvoiceNoteDataLoading;

        public bool InputInvoiceNoteDataLoading
        {
            get { return _InputInvoiceNoteDataLoading; }
            set
            {
                if (_InputInvoiceNoteDataLoading != value)
                {
                    _InputInvoiceNoteDataLoading = value;
                    NotifyPropertyChanged("InputInvoiceNoteDataLoading");
                }
            }
        }
		#endregion

		#region InputInvoiceDocumentsFromDB
		private ObservableCollection<InputInvoiceDocumentViewModel> _InputInvoiceDocumentsFromDB;

		public ObservableCollection<InputInvoiceDocumentViewModel> InputInvoiceDocumentsFromDB
		{
			get { return _InputInvoiceDocumentsFromDB; }
			set
			{
				if (_InputInvoiceDocumentsFromDB != value)
				{
					_InputInvoiceDocumentsFromDB = value;
					NotifyPropertyChanged("InputInvoiceDocumentsFromDB");
				}
			}
		}
		#endregion

		#region CurrentInputInvoiceDocumentForm
		private InputInvoiceDocumentViewModel _CurrentInputInvoiceDocumentForm = new InputInvoiceDocumentViewModel() { CreateDate = DateTime.Now };

		public InputInvoiceDocumentViewModel CurrentInputInvoiceDocumentForm
		{
			get { return _CurrentInputInvoiceDocumentForm; }
			set
			{
				if (_CurrentInputInvoiceDocumentForm != value)
				{
					_CurrentInputInvoiceDocumentForm = value;
					NotifyPropertyChanged("CurrentInputInvoiceDocumentForm");
				}
			}
		}
		#endregion

		#region CurrentInputInvoiceDocumentDG
		private InputInvoiceDocumentViewModel _CurrentInputInvoiceDocumentDG;

		public InputInvoiceDocumentViewModel CurrentInputInvoiceDocumentDG
		{
			get { return _CurrentInputInvoiceDocumentDG; }
			set
			{
				if (_CurrentInputInvoiceDocumentDG != value)
				{
					_CurrentInputInvoiceDocumentDG = value;
					NotifyPropertyChanged("CurrentInputInvoiceDocumentDG");
				}
			}
		}
		#endregion

		#region InputInvoiceDocumentDataLoading
		private bool _InputInvoiceDocumentDataLoading;

		public bool InputInvoiceDocumentDataLoading
		{
			get { return _InputInvoiceDocumentDataLoading; }
			set
			{
				if (_InputInvoiceDocumentDataLoading != value)
				{
					_InputInvoiceDocumentDataLoading = value;
					NotifyPropertyChanged("InputInvoiceDocumentDataLoading");
				}
			}
		}
		#endregion


		#region IsHeaderCreated
		private bool _IsHeaderCreated;

        public bool IsHeaderCreated
        {
            get { return _IsHeaderCreated; }
            set
            {
                if (_IsHeaderCreated != value)
                {
                    _IsHeaderCreated = value;
                    NotifyPropertyChanged("IsHeaderCreated");
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

		public InputInvoiceAddEdit(InputInvoiceViewModel InputInvoiceViewModel, bool isCreateProcess, bool isPopup = false)
		{
			// Initialize service
			InputInvoiceService = DependencyResolver.Kernel.Get<IInputInvoiceService>();

			InitializeComponent();

			this.DataContext = this;

            IsHeaderCreated = !isCreateProcess;

            currentInputInvoice = InputInvoiceViewModel;
			IsCreateProcess = isCreateProcess;
			IsPopup = isPopup;

            Thread displayThread = new Thread(() =>
            {
                DisplayInputInvoiceNoteData();
				DisplayInputInvoiceDocumentData();
			});
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void DisplayInputInvoiceNoteData()
        {
            InputInvoiceNoteDataLoading = true;

            InputInvoiceNoteListResponse response = new InputInvoiceNoteSQLiteRepository()
                .GetInputInvoiceNotesByInputInvoice(MainWindow.CurrentCompanyId, currentInputInvoice.Identifier);

            if (response.Success)
            {
                InputInvoiceNotesFromDB = new ObservableCollection<InputInvoiceNoteViewModel>(
                    response.InputInvoiceNotes ?? new List<InputInvoiceNoteViewModel>());
            }
            else
            {
                InputInvoiceNotesFromDB = new ObservableCollection<InputInvoiceNoteViewModel>();
            }

            InputInvoiceNoteDataLoading = false;
        }

		private void DisplayInputInvoiceDocumentData()
		{
			InputInvoiceDocumentDataLoading = true;

			InputInvoiceDocumentListResponse response = new InputInvoiceDocumentSQLiteRepository()
				.GetInputInvoiceDocumentsByInputInvoice(MainWindow.CurrentCompanyId, currentInputInvoice.Identifier);

			if (response.Success)
			{
				InputInvoiceDocumentsFromDB = new ObservableCollection<InputInvoiceDocumentViewModel>(
					response.InputInvoiceDocuments ?? new List<InputInvoiceDocumentViewModel>());
			}
			else
			{
				InputInvoiceDocumentsFromDB = new ObservableCollection<InputInvoiceDocumentViewModel>();
			}

			InputInvoiceDocumentDataLoading = false;
		}

		#endregion


		private void btnSaveHeader_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(currentInputInvoice.InvoiceNumber))
            {
                MainWindow.WarningMessage = "Obavezno polje: Broj fakture";
                return;
            }

            #endregion

            currentInputInvoice.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            currentInputInvoice.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            currentInputInvoice.IsSynced = false;

            InputInvoiceResponse response = new InputInvoiceSQLiteRepository().Delete(currentInputInvoice.Identifier);
            response = new InputInvoiceSQLiteRepository().Create(currentInputInvoice);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Zaglavlje_je_uspešno_sačuvanoUzvičnik"));
                IsHeaderCreated = true;

                txtNote.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

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

				InputInvoiceResponse response = new InputInvoiceSQLiteRepository().Delete(currentInputInvoice.Identifier);
				response = new InputInvoiceSQLiteRepository().Create(currentInputInvoice);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
					SaveButtonContent = " Sačuvaj ";
					SaveButtonEnabled = true;
					return;
				}

                currentInputInvoice.InputInvoiceNotes = InputInvoiceNotesFromDB;
				currentInputInvoice.InputInvoiceDocuments = InputInvoiceDocumentsFromDB;
				response = InputInvoiceService.Create(currentInputInvoice);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
					SaveButtonContent = " Sačuvaj ";
					SaveButtonEnabled = true;
				}

				if (response.Success)
				{
					new InputInvoiceSQLiteRepository().UpdateSyncStatus(response.InputInvoice.Identifier, response.InputInvoice.Code, response.InputInvoice.UpdatedAt, response.InputInvoice.Id, true);
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
								txtInputInvoiceCode.Focus();
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

        #region Add, edit, delete and cancel note

        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentInputInvoiceNoteForm.Note))
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Napomena"));
                return;
            }

            if (CurrentInputInvoiceNoteForm.NoteDate == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Datum_napomene"));
                return;
            }

            #endregion

            // IF update process, first delete item
            new InputInvoiceNoteSQLiteRepository().Delete(CurrentInputInvoiceNoteForm.Identifier);

            CurrentInputInvoiceNoteForm.InputInvoice = currentInputInvoice;
            CurrentInputInvoiceNoteForm.Identifier = Guid.NewGuid();
            CurrentInputInvoiceNoteForm.IsSynced = false;
            CurrentInputInvoiceNoteForm.UpdatedAt = DateTime.Now;
            CurrentInputInvoiceNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentInputInvoiceNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new InputInvoiceNoteSQLiteRepository().Create(CurrentInputInvoiceNoteForm);
            if (response.Success)
            {
                CurrentInputInvoiceNoteForm = new InputInvoiceNoteViewModel();

                Thread displayThread = new Thread(() => DisplayInputInvoiceNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtNote.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnEditNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentInputInvoiceNoteForm = CurrentInputInvoiceNoteDG;
        }

        private void btnDeleteNote_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation(((string)Application.Current.FindResource("stavku_radnika")), "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new InputInvoiceNoteSQLiteRepository().Delete(CurrentInputInvoiceNoteDG.Identifier);

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_radnika_je_uspešno_obrisanaUzvičnik"));

                Thread displayThread = new Thread(() => DisplayInputInvoiceNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentInputInvoiceNoteForm = new InputInvoiceNoteViewModel();
        }

		#endregion

		#region Add, edit, delete and cancel document

		private void FileDIalog_FileOk(object sender, CancelEventArgs e)
		{
			System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
			string[] fileNames = dialog.FileNames;

			if (fileNames.Length > 0)
				CurrentInputInvoiceDocumentForm.Path = fileNames[0];
		}

		private void btnChooseDocument_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.OpenFileDialog fileDIalog = new System.Windows.Forms.OpenFileDialog();

			fileDIalog.Multiselect = true;
			fileDIalog.FileOk += FileDIalog_FileOk;
			fileDIalog.Filter = "Image Files | *.pdf";
			fileDIalog.ShowDialog();
		}

		private void btnAddDocument_Click(object sender, RoutedEventArgs e)
		{
			#region Validation

			if (String.IsNullOrEmpty(CurrentInputInvoiceDocumentForm.Name))
			{
				MainWindow.WarningMessage = "Obavezno polje: Naziv";
				return;
			}

			if (String.IsNullOrEmpty(CurrentInputInvoiceDocumentForm.Path))
			{
				MainWindow.WarningMessage = "Obavezno polje: Putanja";
				return;
			}

			if (CurrentInputInvoiceDocumentForm.CreateDate == null)
			{
				MainWindow.WarningMessage = "Obavezno polje: Datum kreiranja";
				return;
			}

			#endregion

			// IF update process, first delete item
			new InputInvoiceDocumentSQLiteRepository().Delete(CurrentInputInvoiceDocumentForm.Identifier);

			CurrentInputInvoiceDocumentForm.InputInvoice = currentInputInvoice;
			CurrentInputInvoiceDocumentForm.Identifier = Guid.NewGuid();
			CurrentInputInvoiceDocumentForm.IsSynced = false;
			CurrentInputInvoiceDocumentForm.UpdatedAt = DateTime.Now;
			CurrentInputInvoiceDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
			CurrentInputInvoiceDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

			var response = new InputInvoiceDocumentSQLiteRepository().Create(CurrentInputInvoiceDocumentForm);
			if (response.Success)
			{
				CurrentInputInvoiceDocumentForm = new InputInvoiceDocumentViewModel();
				CurrentInputInvoiceDocumentForm.CreateDate = DateTime.Now;

				Thread displayThread = new Thread(() => DisplayInputInvoiceDocumentData());
				displayThread.IsBackground = true;
				displayThread.Start();

				txtDocumentName.Focus();
			}
			else
				MainWindow.ErrorMessage = response.Message;
		}

		private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
		{
			CurrentInputInvoiceDocumentForm = new InputInvoiceDocumentViewModel();
			CurrentInputInvoiceDocumentForm.CreateDate = DateTime.Now;
		}

		private void btnEditDocument_Click(object sender, RoutedEventArgs e)
		{
			CurrentInputInvoiceDocumentForm = CurrentInputInvoiceDocumentDG;
		}

		private void btnDeleteDocument_Click(object sender, RoutedEventArgs e)
		{
			SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

			DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("dokument", "");
			var showDialog = deleteConfirmationForm.ShowDialog();
			if (showDialog != null && showDialog.Value)
			{
				new InputInvoiceDocumentSQLiteRepository().Delete(CurrentInputInvoiceDocumentDG.Identifier);

				MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Dokument_je_uspešno_obrisanUzvičnik"));

				Thread displayThread = new Thread(() => DisplayInputInvoiceDocumentData());
				displayThread.IsBackground = true;
				displayThread.Start();
			}

			SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
		}

		#endregion

		#region Mouse wheel event 

		private void PreviewMouseWheelEv(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
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
