using Ninject;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Messages.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Phonebooks;
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

namespace SirmiumERPGFC.Views.Phonebooks
{
    public delegate void PhonebookHandler();
    public partial class Phonebook_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhonebookService PhonebookService;
        IPhonebookNoteService PhonebookNoteService;
        IPhonebookDocumentService PhonebookDocumentService;
        IPhonebookPhoneService PhonebookPhoneService;
        #endregion

        #region PhonebookSearchObject
        private PhonebookViewModel _PhonebookSearchObject = new PhonebookViewModel();

        public PhonebookViewModel PhonebookSearchObject
        {
            get { return _PhonebookSearchObject; }
            set
            {
                if (_PhonebookSearchObject != value)
                {
                    _PhonebookSearchObject = value;
                    NotifyPropertyChanged("PhonebookSearchObject");
                }
            }
        }
        #endregion

        #region PhonebooksFromDB
        private ObservableCollection<PhonebookViewModel> _PhonebooksFromDB;

        public ObservableCollection<PhonebookViewModel> PhonebooksFromDB
        {
            get { return _PhonebooksFromDB; }
            set
            {
                if (_PhonebooksFromDB != value)
                {
                    _PhonebooksFromDB = value;
                    NotifyPropertyChanged("PhonebooksFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhonebook
        private PhonebookViewModel _CurrentPhonebook;

        public PhonebookViewModel CurrentPhonebook
        {
            get { return _CurrentPhonebook; }
            set
            {
                if (_CurrentPhonebook != value)
                {
                    _CurrentPhonebook = value;
                    NotifyPropertyChanged("CurrentPhonebook");
                    if (_CurrentPhonebook != null)
                    {
                        Thread displayItemThread = new Thread(() =>
                        {

                            DisplayPhonebookDocumentData();
                            DisplayPhonebookNoteData();
                            DisplayPhonebookPhoneData();

                        });
                        displayItemThread.IsBackground = true;
                        displayItemThread.Start();
                    }
                    else
                        NotesFromDB = new ObservableCollection<PhonebookNoteViewModel>();
                }
            }
        }
        #endregion

        #region PhonebookDataLoading
        private bool _PhonebookDataLoading = true;

        public bool PhonebookDataLoading
        {
            get { return _PhonebookDataLoading; }
            set
            {
                if (_PhonebookDataLoading != value)
                {
                    _PhonebookDataLoading = value;
                    NotifyPropertyChanged("PhonebookDataLoading");
                }
            }
        }
        #endregion

        #region PhonebookDocumentsFromDB
        private ObservableCollection<PhonebookDocumentViewModel> _PhonebookDocumentsFromDB;

        public ObservableCollection<PhonebookDocumentViewModel> PhonebookDocumentsFromDB
        {
            get { return _PhonebookDocumentsFromDB; }
            set
            {
                if (_PhonebookDocumentsFromDB != value)
                {
                    _PhonebookDocumentsFromDB = value;
                    NotifyPropertyChanged("PhonebookDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhonebookDocument
        private PhonebookDocumentViewModel _CurrentPhonebookDocument;

        public PhonebookDocumentViewModel CurrentPhonebookDocument
        {
            get { return _CurrentPhonebookDocument; }
            set
            {
                if (_CurrentPhonebookDocument != value)
                {
                    _CurrentPhonebookDocument = value;
                    NotifyPropertyChanged("CurrentPhonebookDocument");
                }
            }
        }
        #endregion

        #region PhonebookDocumentDataLoading
        private bool _PhonebookDocumentDataLoading;

        public bool PhonebookDocumentDataLoading
        {
            get { return _PhonebookDocumentDataLoading; }
            set
            {
                if (_PhonebookDocumentDataLoading != value)
                {
                    _PhonebookDocumentDataLoading = value;
                    NotifyPropertyChanged("PhonebookDocumentDataLoading");
                }
            }
        }
        #endregion

        #region NotesFromDB
        private ObservableCollection<PhonebookNoteViewModel> _NotesFromDB;

        public ObservableCollection<PhonebookNoteViewModel> NotesFromDB
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
        private PhonebookNoteViewModel _CurrentNoteForm = new PhonebookNoteViewModel();

        public PhonebookNoteViewModel CurrentNoteForm
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
        private PhonebookNoteViewModel _CurrentNoteDG;

        public PhonebookNoteViewModel CurrentNoteDG
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


        #region PhonesFromDB
        private ObservableCollection<PhonebookPhoneViewModel> _PhonesFromDB;

        public ObservableCollection<PhonebookPhoneViewModel> PhonesFromDB
        {
            get { return _PhonesFromDB; }
            set
            {
                if (_PhonesFromDB != value)
                {
                    _PhonesFromDB = value;
                    NotifyPropertyChanged("PhonesFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhoneDG
        private PhonebookPhoneViewModel _CurrentPhoneDG;

        public PhonebookPhoneViewModel CurrentPhoneDG
        {
            get { return _CurrentPhoneDG; }
            set
            {
                if (_CurrentPhoneDG != value)
                {
                    _CurrentPhoneDG = value;
                    NotifyPropertyChanged("CurrentPhoneDG");
                }
            }
        }
        #endregion

        #region PhoneDataLoading
        private bool _PhoneDataLoading;

        public bool PhoneDataLoading
        {
            get { return _PhoneDataLoading; }
            set
            {
                if (_PhoneDataLoading != value)
                {
                    _PhoneDataLoading = value;
                    NotifyPropertyChanged("PhoneDataLoading");
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

        #region SyncButtonContent
        private string _SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

        public string SyncButtonContent
        {
            get { return _SyncButtonContent; }
            set
            {
                if (_SyncButtonContent != value)
                {
                    _SyncButtonContent = value;
                    NotifyPropertyChanged("SyncButtonContent");
                }
            }
        }
        #endregion

        #region SyncButtonEnabled
        private bool _SyncButtonEnabled = true;

        public bool SyncButtonEnabled
        {
            get { return _SyncButtonEnabled; }
            set
            {
                if (_SyncButtonEnabled != value)
                {
                    _SyncButtonEnabled = value;
                    NotifyPropertyChanged("SyncButtonEnabled");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public Phonebook_List()
        {
            // Get required service
            PhonebookService = DependencyResolver.Kernel.Get<IPhonebookService>();
            PhonebookNoteService = DependencyResolver.Kernel.Get<IPhonebookNoteService>();
            PhonebookDocumentService = DependencyResolver.Kernel.Get<IPhonebookDocumentService>();
            PhonebookPhoneService = DependencyResolver.Kernel.Get<IPhonebookPhoneService>();
            // Draw all components
            InitializeComponent();

            this.DataContext = this;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
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
            PhonebookDataLoading = true;

            PhonebookListResponse response = new PhonebookSQLiteRepository()
                .GetPhonebooksByPage(MainWindow.CurrentCompanyId, PhonebookSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                PhonebooksFromDB = new ObservableCollection<PhonebookViewModel>(response.Phonebooks ?? new List<PhonebookViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                PhonebooksFromDB = new ObservableCollection<PhonebookViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            PhonebookDataLoading = false;
        }

        private void DisplayPhonebookNoteData()
        {
            NoteDataLoading = true;

            PhonebookNoteListResponse response = new PhonebookNoteSQLiteRepository()
                .GetPhonebookNotesByPhonebook(MainWindow.CurrentCompanyId, CurrentPhonebook.Identifier);

            if (response.Success)
            {
                NotesFromDB = new ObservableCollection<PhonebookNoteViewModel>(
                    response.PhonebookNotes ?? new List<PhonebookNoteViewModel>());
            }
            else
            {
                NotesFromDB = new ObservableCollection<PhonebookNoteViewModel>();
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_prilikom_učitavanja_podatakaUzvičnik"));
            }

            NoteDataLoading = false;
        }
        private void DisplayPhonebookDocumentData()
        {
            PhonebookDocumentDataLoading = true;

            PhonebookDocumentListResponse response = new PhonebookDocumentSQLiteRepository()
                .GetPhonebookDocumentsByPhonebook(MainWindow.CurrentCompanyId, CurrentPhonebook.Identifier);

            if (response.Success)
            {
                PhonebookDocumentsFromDB = new ObservableCollection<PhonebookDocumentViewModel>(
                    response.PhonebookDocuments ?? new List<PhonebookDocumentViewModel>());
            }
            else
            {
                PhonebookDocumentsFromDB = new ObservableCollection<PhonebookDocumentViewModel>();
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_prilikom_učitavanja_podatakaUzvičnik"));
            }

            PhonebookDocumentDataLoading = false;
        }

        private void DisplayPhonebookPhoneData()
        {
            PhoneDataLoading = true;

            PhonebookPhoneListResponse response = new PhonebookPhoneSQLiteRepository()
                .GetPhonebookPhonesByPhonebook(MainWindow.CurrentCompanyId, CurrentPhonebook.Identifier);

            if (response.Success)
            {
                PhonesFromDB = new ObservableCollection<PhonebookPhoneViewModel>(
                    response.PhonebookPhones ?? new List<PhonebookPhoneViewModel>());
            }
            else
            {
                PhonesFromDB = new ObservableCollection<PhonebookPhoneViewModel>();
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_prilikom_učitavanja_podatakaUzvičnik"));
            }

            PhoneDataLoading = false;
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Telefonski_imenik_tritacke"));
            new PhonebookSQLiteRepository().Sync(PhonebookService, (synced, toSync) => {
                SyncButtonContent = ((string)Application.Current.FindResource("Telefonski_imenik")) + "(" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new PhonebookNoteSQLiteRepository().Sync(PhonebookNoteService, (synced, toSync) => {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new PhonebookDocumentSQLiteRepository().Sync(PhonebookDocumentService, (synced, toSync) => {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new PhonebookPhoneSQLiteRepository().Sync(PhonebookPhoneService, (synced, toSync) => {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            DisplayData();
            CurrentPhonebook = null;
            NotesFromDB = new ObservableCollection<PhonebookNoteViewModel>();
            PhonebookDocumentsFromDB = new ObservableCollection<PhonebookDocumentViewModel>();
            PhonesFromDB = new ObservableCollection<PhonebookPhoneViewModel>();
            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncItemData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new PhonebookNoteSQLiteRepository().Sync(PhonebookNoteService, (synced, toSync) => {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            DisplayPhonebookNoteData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncDocumentData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new PhonebookDocumentSQLiteRepository().Sync(PhonebookDocumentService, (synced, toSync) => {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            DisplayPhonebookDocumentData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncPhoneData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new PhonebookPhoneSQLiteRepository().Sync(PhonebookPhoneService, (synced, toSync) => {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            DisplayPhonebookPhoneData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void DgPhonebooks_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgPhonebookNotes_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgPhonebookDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgPhonebookPhones_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Add, edit, delete, lock and cancel

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PhonebookViewModel Phonebook = new PhonebookViewModel();
            Phonebook.Identifier = Guid.NewGuid();


            Phonebook_AddEdit addEditForm = new Phonebook_AddEdit(Phonebook, true, false);
            addEditForm.PhonebookCreatedUpdated += new PhonebookHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Telefonski_imenik")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPhonebook == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Nije_moguće_menjati_ulayne_faktureUzvičnik"));
                return;
            }

            Phonebook_AddEdit PhonebookAddEditForm = new Phonebook_AddEdit(CurrentPhonebook, false);
            PhonebookAddEditForm.PhonebookCreatedUpdated += new PhonebookHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Telefonski_imenik")), 95, PhonebookAddEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPhonebook == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_brisanjeUzvičnik"));
                return;
            }

            // Delete data
            var result = PhonebookService.Delete(CurrentPhonebook.Identifier);
            if (result.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_obrisaniUzvičnik"));

                Thread displayThread = new Thread(() => SyncData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
            {
                MainWindow.ErrorMessage = result.Message;
            }
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhonebook == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_racunUzvičnik"));
                return;
            }

            #endregion

            Phonebook_Note_AddEdit PhonebookNoteAddEditForm = new Phonebook_Note_AddEdit(CurrentPhonebook);
            PhonebookNoteAddEditForm.PhonebookCreatedUpdated += new PhonebookHandler(SyncItemData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Napomene")), 95, PhonebookNoteAddEditForm);
        }

        private void BtnAddDocuments_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhonebook == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_racunUzvičnik"));
                return;
            }

            #endregion

            Phonebook_Document_AddEdit PhonebookDocumentAddEditForm = new Phonebook_Document_AddEdit(CurrentPhonebook);
            PhonebookDocumentAddEditForm.PhonebookCreatedUpdated += new PhonebookHandler(SyncDocumentData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Dokumenti")), 95, PhonebookDocumentAddEditForm);
        }

        private void BtnAddPhone_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhonebook == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_racunUzvičnik"));
                return;
            }

            #endregion

            Phonebook_Phone_AddEdit PhonebookPhoneAddEditForm = new Phonebook_Phone_AddEdit(CurrentPhonebook);
            PhonebookPhoneAddEditForm.PhonebookCreatedUpdated += new PhonebookHandler(SyncPhoneData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Telefoni")), 95, PhonebookPhoneAddEditForm);
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

        #region Display documents

        private void btnShowDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentPhonebookDocument.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion
    }
}
