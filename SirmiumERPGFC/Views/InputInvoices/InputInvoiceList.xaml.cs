using Ninject;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Reports.InputInvoices;
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
		IInputInvoiceNoteService inputInvoiceNoteService;
		IInputInvoiceDocumentService inputInvoiceDocumentService;
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
					if (_CurrentInputInvoice != null)
					{
						Thread displayItemThread = new Thread(() =>
						{
							
							DisplayInputInvoiceDocumentData();
							
							DisplayInputInvoiceNoteData();

						});
						displayItemThread.IsBackground = true;
						displayItemThread.Start();
					}
					else
						NotesFromDB = new ObservableCollection<InputInvoiceNoteViewModel>();
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

		#region CurrentInputInvoiceDocument
		private InputInvoiceDocumentViewModel _CurrentInputInvoiceDocument;

		public InputInvoiceDocumentViewModel CurrentInputInvoiceDocument
		{
			get { return _CurrentInputInvoiceDocument; }
			set
			{
				if (_CurrentInputInvoiceDocument != value)
				{
					_CurrentInputInvoiceDocument = value;
					NotifyPropertyChanged("CurrentInputInvoiceDocument");
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

		#region NotesFromDB
		private ObservableCollection<InputInvoiceNoteViewModel> _NotesFromDB;

		public ObservableCollection<InputInvoiceNoteViewModel> NotesFromDB
		{
			get { return _NotesFromDB; }
			set
			{
				if (_NotesFromDB != value)
				{
					_NotesFromDB = value;
					NotifyPropertyChanged("NotesFromDB");
				}
			}
		}
		#endregion

		#region CurrentNoteForm
		private InputInvoiceNoteViewModel _CurrentNoteForm = new InputInvoiceNoteViewModel();

		public InputInvoiceNoteViewModel CurrentNoteForm
		{
			get { return _CurrentNoteForm; }
			set
			{
				if (_CurrentNoteForm != value)
				{
					_CurrentNoteForm = value;
					NotifyPropertyChanged("CurrentNoteForm");
				}
			}
		}
		#endregion

		#region CurrentNoteDG
		private InputInvoiceNoteViewModel _CurrentNoteDG;

		public InputInvoiceNoteViewModel CurrentNoteDG
		{
			get { return _CurrentNoteDG; }
			set
			{
				if (_CurrentNoteDG != value)
				{
					_CurrentNoteDG = value;
					NotifyPropertyChanged("CurrentNoteDG");
				}
			}
		}
		#endregion

		#region NoteDataLoading
		private bool _NoteDataLoading;

		public bool NoteDataLoading
		{
			get { return _NoteDataLoading; }
			set
			{
				if (_NoteDataLoading != value)
				{
					_NoteDataLoading = value;
					NotifyPropertyChanged("NoteDataLoading");
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
		private string _RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

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
			this.inputInvoiceNoteService = DependencyResolver.Kernel.Get<IInputInvoiceNoteService>();
			this.inputInvoiceDocumentService = DependencyResolver.Kernel.Get<IInputInvoiceDocumentService>();

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

				MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sinhronizovaniUzvičnik"));
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

		private void DisplayInputInvoiceNoteData()
		{
			NoteDataLoading = true;

			InputInvoiceNoteListResponse response = new InputInvoiceNoteSQLiteRepository()
				.GetInputInvoiceNotesByInputInvoice(MainWindow.CurrentCompanyId, CurrentInputInvoice.Identifier);

			if (response.Success)
			{
				NotesFromDB = new ObservableCollection<InputInvoiceNoteViewModel>(
					response.InputInvoiceNotes ?? new List<InputInvoiceNoteViewModel>());
			}
			else
			{
				NotesFromDB = new ObservableCollection<InputInvoiceNoteViewModel>();
			}

			NoteDataLoading = false;
		}
		private void DisplayInputInvoiceDocumentData()
		{
			InputInvoiceDocumentDataLoading = true;

			InputInvoiceDocumentListResponse response = new InputInvoiceDocumentSQLiteRepository()
				.GetInputInvoiceDocumentsByInputInvoice(MainWindow.CurrentCompanyId, CurrentInputInvoice.Identifier);

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

		private void SyncData()
		{
			RefreshButtonEnabled = false;

			RefreshButtonContent = ((string)Application.Current.FindResource("Ulazne_fakture_TriTacke"));
            new InputInvoiceSQLiteRepository().Sync(inputInvoiceService);
			new InputInvoiceNoteSQLiteRepository().Sync(inputInvoiceNoteService);
			new InputInvoiceDocumentSQLiteRepository().Sync(inputInvoiceDocumentService);

			DisplayData();

			RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
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
			FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("ULAZNE_FAKTURE")), 95, addEditForm);
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentInputInvoice == null)
			{
				MainWindow.WarningMessage = ((string)Application.Current.FindResource("Nije_moguće_menjati_ulayne_faktureUzvičnik"));
                return;
			}

			InputInvoiceAddEdit InputInvoiceAddEditForm = new InputInvoiceAddEdit(CurrentInputInvoice, false);
			InputInvoiceAddEditForm.InputInvoiceCreatedUpdated += new InputInvoiceHandler(SyncData);
			FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("ULAZNE_FAKTURE")), 95, InputInvoiceAddEditForm);
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentInputInvoice == null)
			{
				MainWindow.WarningMessage = ((string)Application.Current.FindResource("Nije_moguće_menjati_ulazne_faktureUzvičnik"));
                return;
			}

			SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

			// Create confirmation window
			DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation(((string)Application.Current.FindResource("ULAZNE_FAKTURE")), CurrentInputInvoice.Code + CurrentInputInvoice.InvoiceNumber);
			var showDialog = deleteConfirmationForm.ShowDialog();
			if (showDialog != null && showDialog.Value)
			{
				InputInvoiceResponse response = inputInvoiceService.Delete(CurrentInputInvoice.Identifier);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
					return;
				}

				response = new InputInvoiceSQLiteRepository().Delete(CurrentInputInvoice.Identifier);
				if (!response.Success)
				{
					MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
					return;
				}

				MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Ulazni_avansni_račun_je_uspešno_obrisanaUzvičnik"));

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

		private void btnPrint_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                InputInvoicesExcelReport.Show(InputInvoicesFromDB.ToList());
            }
            catch(Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
		}

		//private void btnExcel_Click(object sender, RoutedEventArgs e)
		//{

		//}

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

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentInputInvoice.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

		#region Display documents

		private void btnShowDocument_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process process = new System.Diagnostics.Process();
				//string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
				Uri pdf = new Uri(CurrentInputInvoiceDocument.Path, UriKind.RelativeOrAbsolute);
				process.StartInfo.FileName = pdf.LocalPath;
				process.Start();
			}
			catch (Exception error)
			{
				MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		#endregion

		private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InputInvoiceExcelReport.Show(CurrentInputInvoice);
            }
            catch(Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }
    }
}
