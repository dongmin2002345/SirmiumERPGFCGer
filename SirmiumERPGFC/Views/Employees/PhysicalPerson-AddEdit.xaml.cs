using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
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
using WpfAppCommonCode.Converters;

namespace SirmiumERPGFC.Views.Employees
{
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


        #region PhysicalPersonItemsFromDB
        private ObservableCollection<PhysicalPersonItemViewModel> _PhysicalPersonItemsFromDB;

        public ObservableCollection<PhysicalPersonItemViewModel> PhysicalPersonItemsFromDB
        {
            get { return _PhysicalPersonItemsFromDB; }
            set
            {
                if (_PhysicalPersonItemsFromDB != value)
                {
                    _PhysicalPersonItemsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonItemsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonItemForm
        private PhysicalPersonItemViewModel _CurrentPhysicalPersonItemForm = new PhysicalPersonItemViewModel();

        public PhysicalPersonItemViewModel CurrentPhysicalPersonItemForm
        {
            get { return _CurrentPhysicalPersonItemForm; }
            set
            {
                if (_CurrentPhysicalPersonItemForm != value)
                {
                    _CurrentPhysicalPersonItemForm = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonItemForm");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonItemDG
        private PhysicalPersonItemViewModel _CurrentPhysicalPersonItemDG;

        public PhysicalPersonItemViewModel CurrentPhysicalPersonItemDG
        {
            get { return _CurrentPhysicalPersonItemDG; }
            set
            {
                if (_CurrentPhysicalPersonItemDG != value)
                {
                    _CurrentPhysicalPersonItemDG = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonItemDG");
                }
            }
        }
        #endregion

        #region PhysicalPersonItemDataLoading
        private bool _PhysicalPersonItemDataLoading;

        public bool PhysicalPersonItemDataLoading
        {
            get { return _PhysicalPersonItemDataLoading; }
            set
            {
                if (_PhysicalPersonItemDataLoading != value)
                {
                    _PhysicalPersonItemDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonItemDataLoading");
                }
            }
        }
        #endregion


        #region PhysicalPersonProfessionItemsFromDB
        private ObservableCollection<PhysicalPersonProfessionViewModel> _PhysicalPersonProfessionItemsFromDB;

        public ObservableCollection<PhysicalPersonProfessionViewModel> PhysicalPersonProfessionItemsFromDB
        {
            get { return _PhysicalPersonProfessionItemsFromDB; }
            set
            {
                if (_PhysicalPersonProfessionItemsFromDB != value)
                {
                    _PhysicalPersonProfessionItemsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonProfessionItemsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonProfessionItemForm
        private PhysicalPersonProfessionViewModel _CurrentPhysicalPersonProfessionItemForm;

        public PhysicalPersonProfessionViewModel CurrentPhysicalPersonProfessionItemForm
        {
            get { return _CurrentPhysicalPersonProfessionItemForm; }
            set
            {
                if (_CurrentPhysicalPersonProfessionItemForm != value)
                {
                    _CurrentPhysicalPersonProfessionItemForm = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonProfessionItemForm");
                }
            }
        }
        #endregion

        #region CurrentProfessionDG
        private PhysicalPersonProfessionViewModel _CurrentProfessionDG;

        public PhysicalPersonProfessionViewModel CurrentProfessionDG
        {
            get { return _CurrentProfessionDG; }
            set
            {
                if (_CurrentProfessionDG != value)
                {
                    _CurrentProfessionDG = value;
                    NotifyPropertyChanged("CurrentProfessionDG");
                }
            }
        }
        #endregion

        #region LoadingProfessionItems
        private bool _LoadingProfessionItems;

        public bool LoadingProfessionItems
        {
            get { return _LoadingProfessionItems; }
            set
            {
                if (_LoadingProfessionItems != value)
                {
                    _LoadingProfessionItems = value;
                    NotifyPropertyChanged("LoadingProfessionItems");
                }
            }
        }
        #endregion


        #region PhysicalPersonLicenceItemsFromDB
        private ObservableCollection<PhysicalPersonLicenceViewModel> _PhysicalPersonLicenceItemsFromDB;

        public ObservableCollection<PhysicalPersonLicenceViewModel> PhysicalPersonLicenceItemsFromDB
        {
            get { return _PhysicalPersonLicenceItemsFromDB; }
            set
            {
                if (_PhysicalPersonLicenceItemsFromDB != value)
                {
                    _PhysicalPersonLicenceItemsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonLicenceItemsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonLicenceItemForm
        private PhysicalPersonLicenceViewModel _CurrentPhysicalPersonLicenceItemForm;

        public PhysicalPersonLicenceViewModel CurrentPhysicalPersonLicenceItemForm
        {
            get { return _CurrentPhysicalPersonLicenceItemForm; }
            set
            {
                if (_CurrentPhysicalPersonLicenceItemForm != value)
                {
                    _CurrentPhysicalPersonLicenceItemForm = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonLicenceItemForm");
                }
            }
        }
        #endregion

        #region CurrentLicenceDG
        private PhysicalPersonLicenceViewModel _CurrentLicenceDG;

        public PhysicalPersonLicenceViewModel CurrentLicenceDG
        {
            get { return _CurrentLicenceDG; }
            set
            {
                if (_CurrentLicenceDG != value)
                {
                    _CurrentLicenceDG = value;
                    NotifyPropertyChanged("CurrentLicenceDG");
                }
            }
        }
        #endregion

        #region LoadingLicenceItems
        private bool _LoadingLicenceItems;

        public bool LoadingLicenceItems
        {
            get { return _LoadingLicenceItems; }
            set
            {
                if (_LoadingLicenceItems != value)
                {
                    _LoadingLicenceItems = value;
                    NotifyPropertyChanged("LoadingLicenceItems");
                }
            }
        }
        #endregion


        #region PhysicalPersonDocumentsFromDB
        private ObservableCollection<PhysicalPersonDocumentViewModel> _PhysicalPersonDocumentsFromDB;

        public ObservableCollection<PhysicalPersonDocumentViewModel> PhysicalPersonDocumentsFromDB
        {
            get { return _PhysicalPersonDocumentsFromDB; }
            set
            {
                if (_PhysicalPersonDocumentsFromDB != value)
                {
                    _PhysicalPersonDocumentsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonDocumentForm
        private PhysicalPersonDocumentViewModel _CurrentPhysicalPersonDocumentForm = new PhysicalPersonDocumentViewModel();

        public PhysicalPersonDocumentViewModel CurrentPhysicalPersonDocumentForm
        {
            get { return _CurrentPhysicalPersonDocumentForm; }
            set
            {
                if (_CurrentPhysicalPersonDocumentForm != value)
                {
                    _CurrentPhysicalPersonDocumentForm = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonDocumentForm");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonDocumentDG
        private PhysicalPersonDocumentViewModel _CurrentPhysicalPersonDocumentDG;

        public PhysicalPersonDocumentViewModel CurrentPhysicalPersonDocumentDG
        {
            get { return _CurrentPhysicalPersonDocumentDG; }
            set
            {
                if (_CurrentPhysicalPersonDocumentDG != value)
                {
                    _CurrentPhysicalPersonDocumentDG = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonDocumentDG");
                }
            }
        }
        #endregion

        #region PhysicalPersonDocumentDataLoading
        private bool _PhysicalPersonDocumentDataLoading;

        public bool PhysicalPersonDocumentDataLoading
        {
            get { return _PhysicalPersonDocumentDataLoading; }
            set
            {
                if (_PhysicalPersonDocumentDataLoading != value)
                {
                    _PhysicalPersonDocumentDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonDocumentDataLoading");
                }
            }
        }
        #endregion


        #region PhysicalPersonNotesFromDB
        private ObservableCollection<PhysicalPersonNoteViewModel> _PhysicalPersonNotesFromDB;

        public ObservableCollection<PhysicalPersonNoteViewModel> PhysicalPersonNotesFromDB
        {
            get { return _PhysicalPersonNotesFromDB; }
            set
            {
                if (_PhysicalPersonNotesFromDB != value)
                {
                    _PhysicalPersonNotesFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonNotesFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonNoteForm
        private PhysicalPersonNoteViewModel _CurrentPhysicalPersonNoteForm = new PhysicalPersonNoteViewModel() { NoteDate = DateTime.Now };

        public PhysicalPersonNoteViewModel CurrentPhysicalPersonNoteForm
        {
            get { return _CurrentPhysicalPersonNoteForm; }
            set
            {
                if (_CurrentPhysicalPersonNoteForm != value)
                {
                    _CurrentPhysicalPersonNoteForm = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonNoteForm");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonNoteDG
        private PhysicalPersonNoteViewModel _CurrentPhysicalPersonNoteDG;

        public PhysicalPersonNoteViewModel CurrentPhysicalPersonNoteDG
        {
            get { return _CurrentPhysicalPersonNoteDG; }
            set
            {
                if (_CurrentPhysicalPersonNoteDG != value)
                {
                    _CurrentPhysicalPersonNoteDG = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonNoteDG");
                }
            }
        }
        #endregion

        #region PhysicalPersonNoteDataLoading
        private bool _PhysicalPersonNoteDataLoading;

        public bool PhysicalPersonNoteDataLoading
        {
            get { return _PhysicalPersonNoteDataLoading; }
            set
            {
                if (_PhysicalPersonNoteDataLoading != value)
                {
                    _PhysicalPersonNoteDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonNoteDataLoading");
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

        #region SubmitButtonContent
        private string _SubmitButtonContent = " Sačuvaj i proknjiži ";

        public string SubmitButtonContent
        {
            get { return _SubmitButtonContent; }
            set
            {
                if (_SubmitButtonContent != value)
                {
                    _SubmitButtonContent = value;
                    NotifyPropertyChanged("SubmitButtonContent");
                }
            }
        }
        #endregion

        #region SubmitButtonEnabled
        private bool _SubmitButtonEnabled = true;

        public bool SubmitButtonEnabled
        {
            get { return _SubmitButtonEnabled; }
            set
            {
                if (_SubmitButtonEnabled != value)
                {
                    _SubmitButtonEnabled = value;
                    NotifyPropertyChanged("SubmitButtonEnabled");
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

        #endregion

        #region Constructor

        public PhysicalPerson_AddEdit(PhysicalPersonViewModel physicalPersonViewModel, bool isCreateProcess, bool isPopup = false)
		{
			// Initialize service
			this.physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();

			InitializeComponent();

			this.DataContext = this;

			CurrentPhysicalPerson = physicalPersonViewModel;


            CurrentPhysicalPersonItemForm = new PhysicalPersonItemViewModel();
            CurrentPhysicalPersonProfessionItemForm = new PhysicalPersonProfessionViewModel();
            CurrentPhysicalPersonLicenceItemForm = new PhysicalPersonLicenceViewModel();

            IsCreateProcess = isCreateProcess;
            IsHeaderCreated = !isCreateProcess;
            IsPopup = isPopup;

            Thread displayThread = new Thread(() =>
            {
                DisplayPhysicalPersonItemData();
                DisplayProfessionItemData();
                DisplayLicenceItemData();
                DisplayDocumentData();
                DisplayPhysicalPersonNoteData();
            });
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion


        #region Display data

        private void DisplayPhysicalPersonItemData()
        {
            PhysicalPersonItemDataLoading = true;

            PhysicalPersonItemListResponse response = new PhysicalPersonItemSQLiteRepository()
                .GetPhysicalPersonItemsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonItemsFromDB = new ObservableCollection<PhysicalPersonItemViewModel>(
                    response.PhysicalPersonItems ?? new List<PhysicalPersonItemViewModel>());
            }
            else
            {
                PhysicalPersonItemsFromDB = new ObservableCollection<PhysicalPersonItemViewModel>();
            }

            PhysicalPersonItemDataLoading = false;
        }


        private void DisplayProfessionItemData()
        {
            LoadingProfessionItems = true;

            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionSQLiteRepository()
                .GetPhysicalPersonProfessionsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonProfessionItemsFromDB = new ObservableCollection<PhysicalPersonProfessionViewModel>(
                    response.PhysicalPersonProfessions ?? new List<PhysicalPersonProfessionViewModel>());
            }
            else
            {
                PhysicalPersonProfessionItemsFromDB = new ObservableCollection<PhysicalPersonProfessionViewModel>();
            }

            LoadingProfessionItems = false;
        }


        private void DisplayLicenceItemData()
        {
            LoadingLicenceItems = true;

            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceSQLiteRepository()
                .GetPhysicalPersonLicencesByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonLicenceItemsFromDB = new ObservableCollection<PhysicalPersonLicenceViewModel>(
                    response.PhysicalPersonLicences ?? new List<PhysicalPersonLicenceViewModel>());
            }
            else
            {
                PhysicalPersonLicenceItemsFromDB = new ObservableCollection<PhysicalPersonLicenceViewModel>();
            }

            LoadingLicenceItems = false;
        }

        private void DisplayDocumentData()
        {
            PhysicalPersonDocumentDataLoading = true;

            PhysicalPersonDocumentListResponse response = new PhysicalPersonDocumentSQLiteRepository()
                .GetPhysicalPersonDocumentsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonDocumentsFromDB = new ObservableCollection<PhysicalPersonDocumentViewModel>(
                    response.PhysicalPersonDocuments ?? new List<PhysicalPersonDocumentViewModel>());
            }
            else
            {
                PhysicalPersonDocumentsFromDB = new ObservableCollection<PhysicalPersonDocumentViewModel>();
            }

            PhysicalPersonDocumentDataLoading = false;
        }

        private void DisplayPhysicalPersonNoteData()
        {
            PhysicalPersonNoteDataLoading = true;

            PhysicalPersonNoteListResponse response = new PhysicalPersonNoteSQLiteRepository()
                .GetPhysicalPersonNotesByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonNotesFromDB = new ObservableCollection<PhysicalPersonNoteViewModel>(
                    response.PhysicalPersonNotes ?? new List<PhysicalPersonNoteViewModel>());
            }
            else
            {
                PhysicalPersonNotesFromDB = new ObservableCollection<PhysicalPersonNoteViewModel>();
            }

            PhysicalPersonNoteDataLoading = false;
        }

        #endregion

        #region Save header

        private void btnSaveHeader_Click(object sender, RoutedEventArgs e)
        {
            IsHeaderCreated = false;

            //#region Validation

            //if (CurrentPhysicalPerson.InputNoteDate == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Datum prijema";
            //    return;
            //}

            //if (CurrentPhysicalPerson.Supplier == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Dobavljač";
            //    return;
            //}

            //if (CurrentPhysicalPerson.Country == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Država";
            //    return;
            //}

            //if (CurrentPhysicalPerson.ReceivedWeight == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Težina kod prijema";
            //    return;
            //}

            //if (CurrentPhysicalPerson.FarmWeight == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Težina na farmi";
            //    return;
            //}

            //if (CurrentPhysicalPerson.Quantity == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Količina / Broj grla";
            //    return;
            //}

            //#endregion

            CurrentPhysicalPerson.IsSynced = false;
            CurrentPhysicalPerson.UpdatedAt = DateTime.Now;
            CurrentPhysicalPerson.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentPhysicalPerson.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var sqLite = new PhysicalPersonSQLiteRepository();
            sqLite.Delete(CurrentPhysicalPerson.Identifier);
            var response = sqLite.Create(CurrentPhysicalPerson);
            if (response.Success)
            {
                MainWindow.SuccessMessage = "Zaglavlje je uspešno sačuvano!";
                IsHeaderCreated = true;

                popCountry2.txtCountry.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        #endregion


        #region Add, edit, delete and cancel item

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentPhysicalPersonItemForm.Name))
            {
                MainWindow.WarningMessage = "Obavezno polje: Ime";
                return;
            }

            if (CurrentPhysicalPersonItemForm.DateOfBirth == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Datum rođenja";
                return;
            }

            #endregion

            // IF update process, first delete item
            new PhysicalPersonItemSQLiteRepository().Delete(CurrentPhysicalPersonItemForm.Identifier);

            CurrentPhysicalPersonItemForm.PhysicalPerson = CurrentPhysicalPerson;
            CurrentPhysicalPersonItemForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonItemForm.IsSynced = false;
            CurrentPhysicalPersonItemForm.UpdatedAt = DateTime.Now;
            CurrentPhysicalPersonItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentPhysicalPersonItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new PhysicalPersonItemSQLiteRepository().Create(CurrentPhysicalPersonItemForm);
            if (response.Success)
            {
                CurrentPhysicalPersonItemForm = new PhysicalPersonItemViewModel();

                Thread displayThread = new Thread(() => DisplayPhysicalPersonItemData());
                displayThread.IsBackground = true;
                displayThread.Start();

                popFamilyMember.txtFamilyMember.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonItemForm = CurrentPhysicalPersonItemDG;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku fizičkog lica", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new PhysicalPersonItemSQLiteRepository().Delete(CurrentPhysicalPersonItemDG.Identifier);

                MainWindow.SuccessMessage = "Stavka fizičkog lica je uspešno obrisana!";

                Thread displayThread = new Thread(() => DisplayPhysicalPersonItemData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancelItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonItemForm = new PhysicalPersonItemViewModel();
        }

        #endregion

        #region Add, edit, delete and cancel document

        private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
            string[] fileNames = dialog.FileNames;

            if (fileNames.Length > 0)
                CurrentPhysicalPersonDocumentForm.Path = fileNames[0];
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

            if (String.IsNullOrEmpty(CurrentPhysicalPersonDocumentForm.Name))
            {
                MainWindow.WarningMessage = "Obavezno polje: Naziv";
                return;
            }

            if (String.IsNullOrEmpty(CurrentPhysicalPersonDocumentForm.Path))
            {
                MainWindow.WarningMessage = "Obavezno polje: Putanja";
                return;
            }

            if (CurrentPhysicalPersonDocumentForm.CreateDate == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Datum kreiranja";
                return;
            }

            #endregion

            // IF update process, first delete item
            new PhysicalPersonDocumentSQLiteRepository().Delete(CurrentPhysicalPersonDocumentForm.Identifier);

            CurrentPhysicalPersonDocumentForm.PhysicalPerson = CurrentPhysicalPerson;
            CurrentPhysicalPersonDocumentForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonDocumentForm.IsSynced = false;
            CurrentPhysicalPersonDocumentForm.UpdatedAt = DateTime.Now;
            CurrentPhysicalPersonDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentPhysicalPersonDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new PhysicalPersonDocumentSQLiteRepository().Create(CurrentPhysicalPersonDocumentForm);
            if (response.Success)
            {
                CurrentPhysicalPersonDocumentForm = new PhysicalPersonDocumentViewModel();

                Thread displayThread = new Thread(() => DisplayDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtDocumentName.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonDocumentForm = new PhysicalPersonDocumentViewModel();
        }

        private void btnEditDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonDocumentForm = CurrentPhysicalPersonDocumentDG;
        }

        private void btnDeleteDocument_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("dokument", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new PhysicalPersonDocumentSQLiteRepository().Delete(CurrentPhysicalPersonDocumentDG.Identifier);

                MainWindow.SuccessMessage = "Dokument je uspešno obrisan!";

                Thread displayThread = new Thread(() => DisplayDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        #endregion

        #region Add, edit, delete and cancel note

        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentPhysicalPersonNoteForm.Note))
            {
                MainWindow.WarningMessage = "Obavezno polje: Napomena";
                return;
            }

            if (CurrentPhysicalPersonNoteForm.NoteDate == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Datum napomene";
                return;
            }

            #endregion

            // IF update process, first delete item
            new PhysicalPersonNoteSQLiteRepository().Delete(CurrentPhysicalPersonNoteForm.Identifier);

            CurrentPhysicalPersonNoteForm.PhysicalPerson = CurrentPhysicalPerson;
            CurrentPhysicalPersonNoteForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonNoteForm.IsSynced = false;
            CurrentPhysicalPersonNoteForm.UpdatedAt = DateTime.Now;
            CurrentPhysicalPersonNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentPhysicalPersonNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new PhysicalPersonNoteSQLiteRepository().Create(CurrentPhysicalPersonNoteForm);
            if (response.Success)
            {
                CurrentPhysicalPersonNoteForm = new PhysicalPersonNoteViewModel();

                Thread displayThread = new Thread(() => DisplayPhysicalPersonNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtNote.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnEditNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonNoteForm = CurrentPhysicalPersonNoteDG;
        }

        private void btnDeleteNote_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku fizičkog lica", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new PhysicalPersonNoteSQLiteRepository().Delete(CurrentPhysicalPersonNoteDG.Identifier);

                MainWindow.SuccessMessage = "Stavka fizičkog lica je uspešno obrisana!";

                Thread displayThread = new Thread(() => DisplayPhysicalPersonNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonNoteForm = new PhysicalPersonNoteViewModel();
        }

        #endregion

        #region Submit and Cancel button

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            SubmitConfirmation submitConfirmationForm = new SubmitConfirmation();
            var showDialog = submitConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                // Save header for any new change
                btnSaveHeader_Click(sender, e);

                #region Validation

                if (!IsHeaderCreated)
                {
                    MainWindow.WarningMessage = "Zaglavlje nije sačuvano";
                    return;
                }

                #endregion

                Thread th = new Thread(() =>
                {
                    SubmitButtonContent = " Čuvanje u toku... ";
                    SubmitButtonEnabled = false;

                    CurrentPhysicalPerson.PhysicalPersonItems = PhysicalPersonItemsFromDB;
                    CurrentPhysicalPerson.PhysicalPersonLicences = PhysicalPersonLicenceItemsFromDB;
                    CurrentPhysicalPerson.PhysicalPersonProfessions = PhysicalPersonProfessionItemsFromDB;
                    CurrentPhysicalPerson.PhysicalPersonDocuments = PhysicalPersonDocumentsFromDB;
                    CurrentPhysicalPerson.PhysicalPersonNotes = PhysicalPersonNotesFromDB;

                    PhysicalPersonResponse response = physicalPersonService.Create(CurrentPhysicalPerson);

                    if (response.Success)
                    {
                        new PhysicalPersonSQLiteRepository().UpdateSyncStatus(CurrentPhysicalPerson.Identifier, response.PhysicalPerson.Code, response.PhysicalPerson.UpdatedAt, response.PhysicalPerson.Id, true);
                        MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                        SubmitButtonContent = " Proknjiži ";
                        SubmitButtonEnabled = true;

                        PhysicalPersonCreatedUpdated();

                        CurrentPhysicalPerson = new PhysicalPersonViewModel();
                        CurrentPhysicalPerson.Identifier = Guid.NewGuid();

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
                    else
                    {
                        MainWindow.ErrorMessage = "Greška kod čuvanja na serveru! " + response.Message;

                        SubmitButtonContent = " Proknjiži ";
                        SubmitButtonEnabled = true;
                    }
                });
                th.IsBackground = true;
                th.Start();
            }
            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            PhysicalPersonCreatedUpdated();

            if (IsPopup)
                FlyoutHelper.CloseFlyoutPopup(this);
            else
                FlyoutHelper.CloseFlyout(this);
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

        private void btnAddProfessionItem_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhysicalPersonProfessionItemForm.Profession == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Zanimanje";
                return;
            }

            if (CurrentPhysicalPersonProfessionItemForm.Country == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Država";
                return;
            }

            #endregion
            // IF update process, first delete item
            new PhysicalPersonProfessionSQLiteRepository().Delete(CurrentPhysicalPersonProfessionItemForm.Identifier);

            CurrentPhysicalPersonProfessionItemForm.PhysicalPerson = CurrentPhysicalPerson;
            CurrentPhysicalPersonProfessionItemForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonProfessionItemForm.IsSynced = false;
            CurrentPhysicalPersonProfessionItemForm.UpdatedAt = DateTime.Now;
            CurrentPhysicalPersonProfessionItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentPhysicalPersonProfessionItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new PhysicalPersonProfessionSQLiteRepository().Create(CurrentPhysicalPersonProfessionItemForm);
            if (response.Success)
            {
                CurrentPhysicalPersonProfessionItemForm = new PhysicalPersonProfessionViewModel();

                Thread displayThread = new Thread(() => DisplayProfessionItemData());
                displayThread.IsBackground = true;
                displayThread.Start();

                popCountry2.txtCountry.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelProfessionItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonProfessionItemForm = new PhysicalPersonProfessionViewModel();
        }

        private void btnAddDItem_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhysicalPersonLicenceItemForm.Licence == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Vrsta dozvole";
                return;
            }

            if (CurrentPhysicalPersonLicenceItemForm.Country == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Država";
                return;
            }

            #endregion
            // IF update process, first delete item
            new PhysicalPersonLicenceSQLiteRepository().Delete(CurrentPhysicalPersonLicenceItemForm.Identifier);

            CurrentPhysicalPersonLicenceItemForm.PhysicalPerson = CurrentPhysicalPerson;
            CurrentPhysicalPersonLicenceItemForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonLicenceItemForm.IsSynced = false;
            CurrentPhysicalPersonLicenceItemForm.UpdatedAt = DateTime.Now;
            CurrentPhysicalPersonLicenceItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentPhysicalPersonLicenceItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new PhysicalPersonLicenceSQLiteRepository().Create(CurrentPhysicalPersonLicenceItemForm);
            if (response.Success)
            {
                CurrentPhysicalPersonLicenceItemForm = new PhysicalPersonLicenceViewModel();

                Thread displayThread = new Thread(() => DisplayLicenceItemData());
                displayThread.IsBackground = true;
                displayThread.Start();

                popCountry3.txtCountry.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonLicenceItemForm = new PhysicalPersonLicenceViewModel();
        }

        private void btnEditProfession_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonProfessionItemForm = new PhysicalPersonProfessionViewModel()
            {
                Company = CurrentProfessionDG.Company,
                Country = CurrentProfessionDG.Country,
                CreatedAt = CurrentProfessionDG.CreatedAt,
                CreatedBy = CurrentProfessionDG.CreatedBy,
                PhysicalPerson = CurrentProfessionDG.PhysicalPerson,
                Id = CurrentProfessionDG.Id,
                Identifier = CurrentProfessionDG.Identifier,
                IsActive = CurrentProfessionDG.IsActive,
                IsSynced = CurrentProfessionDG.IsSynced,
                Profession = CurrentProfessionDG.Profession,
                UpdatedAt = CurrentProfessionDG.UpdatedAt
            };
        }

        private void btnDeleteProfession_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProfessionDG == null)
                return;

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku fizičkog lica", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new PhysicalPersonProfessionSQLiteRepository().Delete(CurrentProfessionDG.Identifier);

                MainWindow.SuccessMessage = "Stavka fizičkog lica je uspešno obrisana!";

                Thread displayThread = new Thread(() => DisplayProfessionItemData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnEditLicence_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonLicenceItemForm = new PhysicalPersonLicenceViewModel()
            {
                Company = CurrentLicenceDG.Company,
                Country = CurrentLicenceDG.Country,
                CreatedAt = CurrentLicenceDG.CreatedAt,
                CreatedBy = CurrentLicenceDG.CreatedBy,
                PhysicalPerson = CurrentLicenceDG.PhysicalPerson,
                ValidFrom = CurrentLicenceDG.ValidFrom,
                ValidTo = CurrentLicenceDG.ValidTo,
                Id = CurrentLicenceDG.Id,
                Identifier = CurrentLicenceDG.Identifier,
                IsActive = CurrentLicenceDG.IsActive,
                IsSynced = CurrentLicenceDG.IsSynced,
                Licence = CurrentLicenceDG.Licence,
                UpdatedAt = CurrentLicenceDG.UpdatedAt
            };
        }

        private void btnDeleteLicence_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentLicenceDG == null)
                return;

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku fizičkog lica", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new PhysicalPersonLicenceSQLiteRepository().Delete(CurrentLicenceDG.Identifier);

                MainWindow.SuccessMessage = "Stavka fizičkog lica je uspešno obrisana!";

                Thread displayThread = new Thread(() => DisplayLicenceItemData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

    }
}
