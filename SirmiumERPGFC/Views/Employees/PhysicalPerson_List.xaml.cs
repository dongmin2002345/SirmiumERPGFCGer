using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Reports.PhysicalPersons;
using SirmiumERPGFC.Reports.Employees;
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
    public delegate void PhysicalPersonHandler();

    public partial class PhysicalPerson_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhysicalPersonService physicalPersonService;
        IPhysicalPersonNoteService physicalPersonNoteService;
        IPhysicalPersonDocumentService physicalPersonDocumentService;
        IPhysicalPersonCardService physicalPersonCardService;
        IPhysicalPersonProfessionService physicalPersonProfessionService;
        IPhysicalPersonLicenceService physicalPersonLicenceService;
        IPhysicalPersonItemService physicalPersonItemService;
        #endregion

        #region PhysicalPersonSearchObject
        private PhysicalPersonViewModel _PhysicalPersonSearchObject = new PhysicalPersonViewModel();

        public PhysicalPersonViewModel PhysicalPersonSearchObject
        {
            get { return _PhysicalPersonSearchObject; }
            set
            {
                if (_PhysicalPersonSearchObject != value)
                {
                    _PhysicalPersonSearchObject = value;
                    NotifyPropertyChanged("PhysicalPersonSearchObject");
                }
            }
        }
        #endregion

        #region PhysicalPersonsFromDB
        private ObservableCollection<PhysicalPersonViewModel> _PhysicalPersonsFromDB;

        public ObservableCollection<PhysicalPersonViewModel> PhysicalPersonsFromDB
        {
            get { return _PhysicalPersonsFromDB; }
            set
            {
                if (_PhysicalPersonsFromDB != value)
                {
                    _PhysicalPersonsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPerson
        private PhysicalPersonViewModel _CurrentPhysicalPerson;

        public PhysicalPersonViewModel CurrentPhysicalPerson
        {
            get { return _CurrentPhysicalPerson; }
            set
            {
                if (_CurrentPhysicalPerson != value)
                {
                    _CurrentPhysicalPerson = value;
                    NotifyPropertyChanged("CurrentPhysicalPerson");
                    if (_CurrentPhysicalPerson != null)
                    {
                        Thread displayItemThread = new Thread(() =>
                        {
                            DisplayPhysicalPersonCardData();
                            DisplayPhysicalPersonDocumentData();
                            DisplayPhysicalPersonNoteData();
                            DisplayPhysicalPersonProfessionData();
                            DisplayPhysicalPersonLicenceData();
                            DisplayPhysicalPersonItemData();


                        });
                        displayItemThread.IsBackground = true;
                        displayItemThread.Start();
                    }
                    else
                        PhysicalPersonNotesFromDB = new ObservableCollection<PhysicalPersonNoteViewModel>();
                }
            }
        }
        #endregion

        #region PhysicalPersonDataLoading
        private bool _PhysicalPersonDataLoading = true;

        public bool PhysicalPersonDataLoading
        {
            get { return _PhysicalPersonDataLoading; }
            set
            {
                if (_PhysicalPersonDataLoading != value)
                {
                    _PhysicalPersonDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonDataLoading");
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

        #region CurrentPhysicalPersonDocument
        private PhysicalPersonDocumentViewModel _CurrentPhysicalPersonDocument;

        public PhysicalPersonDocumentViewModel CurrentPhysicalPersonDocument
        {
            get { return _CurrentPhysicalPersonDocument; }
            set
            {
                if (_CurrentPhysicalPersonDocument != value)
                {
                    _CurrentPhysicalPersonDocument = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonDocument");
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
        private PhysicalPersonNoteViewModel _CurrentPhysicalPersonNoteForm = new PhysicalPersonNoteViewModel();

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

        #region PhysicalPersonCardsFromDB
        private ObservableCollection<PhysicalPersonCardViewModel> _PhysicalPersonCardsFromDB;

        public ObservableCollection<PhysicalPersonCardViewModel> PhysicalPersonCardsFromDB
        {
            get { return _PhysicalPersonCardsFromDB; }
            set
            {
                if (_PhysicalPersonCardsFromDB != value)
                {
                    _PhysicalPersonCardsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonCardsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonCard
        private PhysicalPersonCardViewModel _CurrentPhysicalPersonCard;

        public PhysicalPersonCardViewModel CurrentPhysicalPersonCard
        {
            get { return _CurrentPhysicalPersonCard; }
            set
            {
                if (_CurrentPhysicalPersonCard != value)
                {
                    _CurrentPhysicalPersonCard = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonCard");
                }
            }
        }
        #endregion

        #region PhysicalPersonCardDataLoading
        private bool _PhysicalPersonCardDataLoading;

        public bool PhysicalPersonCardDataLoading
        {
            get { return _PhysicalPersonCardDataLoading; }
            set
            {
                if (_PhysicalPersonCardDataLoading != value)
                {
                    _PhysicalPersonCardDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonCardDataLoading");
                }
            }
        }
        #endregion

        #region PhysicalPersonProfessionsFromDB
        private ObservableCollection<PhysicalPersonProfessionViewModel> _PhysicalPersonProfessionsFromDB;

        public ObservableCollection<PhysicalPersonProfessionViewModel> PhysicalPersonProfessionsFromDB
        {
            get { return _PhysicalPersonProfessionsFromDB; }
            set
            {
                if (_PhysicalPersonProfessionsFromDB != value)
                {
                    _PhysicalPersonProfessionsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonProfessionsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonProfession
        private PhysicalPersonCardViewModel _CurrentPhysicalPersonProfession;

        public PhysicalPersonCardViewModel CurrentPhysicalPersonProfession
        {
            get { return _CurrentPhysicalPersonProfession; }
            set
            {
                if (_CurrentPhysicalPersonProfession != value)
                {
                    _CurrentPhysicalPersonProfession = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonProfession");
                }
            }
        }
        #endregion

        #region PhysicalPersonProfessionDataLoading
        private bool _PhysicalPersonProfessionDataLoading;

        public bool PhysicalPersonProfessionDataLoading
        {
            get { return _PhysicalPersonProfessionDataLoading; }
            set
            {
                if (_PhysicalPersonProfessionDataLoading != value)
                {
                    _PhysicalPersonProfessionDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonProfessionDataLoading");
                }
            }
        }
        #endregion

        #region PhysicalPersonLicencesFromDB
        private ObservableCollection<PhysicalPersonLicenceViewModel> _PhysicalPersonLicencesFromDB;

        public ObservableCollection<PhysicalPersonLicenceViewModel> PhysicalPersonLicencesFromDB
        {
            get { return _PhysicalPersonLicencesFromDB; }
            set
            {
                if (_PhysicalPersonLicencesFromDB != value)
                {
                    _PhysicalPersonLicencesFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonLicencesFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonLicence
        private PhysicalPersonLicenceViewModel _CurrentPhysicalPersonLicence;

        public PhysicalPersonLicenceViewModel CurrentPhysicalPersonLicence
        {
            get { return _CurrentPhysicalPersonLicence; }
            set
            {
                if (_CurrentPhysicalPersonLicence != value)
                {
                    _CurrentPhysicalPersonLicence = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonLicence");
                }
            }
        }
        #endregion

        #region PhysicalPersonLicenceDataLoading
        private bool _PhysicalPersonLicenceDataLoading;

        public bool PhysicalPersonLicenceDataLoading
        {
            get { return _PhysicalPersonLicenceDataLoading; }
            set
            {
                if (_PhysicalPersonLicenceDataLoading != value)
                {
                    _PhysicalPersonLicenceDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonLicenceDataLoading");
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

        #region CurrentPhysicalPersonItem
        private PhysicalPersonCardViewModel _CurrentPhysicalPersonItem;

        public PhysicalPersonCardViewModel CurrentPhysicalPersonItem
        {
            get { return _CurrentPhysicalPersonItem; }
            set
            {
                if (_CurrentPhysicalPersonItem != value)
                {
                    _CurrentPhysicalPersonItem = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonItem");
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
                    NotifyPropertyChanged("PhysicalPersonProfessionDataLoading");
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

        #region SyncButtonContent
        private string _SyncButtonContent = " OSVEŽI ";

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

        public PhysicalPerson_List()
        {
            // Get required services
            this.physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();
            this.physicalPersonNoteService = DependencyResolver.Kernel.Get<IPhysicalPersonNoteService>();
            this.physicalPersonDocumentService = DependencyResolver.Kernel.Get<IPhysicalPersonDocumentService>();

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
            PhysicalPersonDataLoading = true;

            PhysicalPersonListResponse response = new PhysicalPersonSQLiteRepository()
                .GetPhysicalPersonsByPage(MainWindow.CurrentCompanyId, PhysicalPersonSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                PhysicalPersonsFromDB = new ObservableCollection<PhysicalPersonViewModel>(response.PhysicalPersons ?? new List<PhysicalPersonViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                PhysicalPersonsFromDB = new ObservableCollection<PhysicalPersonViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            PhysicalPersonDataLoading = false;
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
        private void DisplayPhysicalPersonDocumentData()
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

        private void DisplayPhysicalPersonCardData()
        {
            PhysicalPersonCardDataLoading = true;

            PhysicalPersonCardListResponse response = new PhysicalPersonCardSQLiteRepository()
                .GetPhysicalPersonCardsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonCardsFromDB = new ObservableCollection<PhysicalPersonCardViewModel>(
                    response.PhysicalPersonCards ?? new List<PhysicalPersonCardViewModel>());
            }
            else
            {
                PhysicalPersonCardsFromDB = new ObservableCollection<PhysicalPersonCardViewModel>();
            }

            PhysicalPersonCardDataLoading = false;
        }

        private void DisplayPhysicalPersonProfessionData()
        {
            PhysicalPersonProfessionDataLoading = true;

            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionSQLiteRepository()
                .GetPhysicalPersonProfessionsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonProfessionsFromDB = new ObservableCollection<PhysicalPersonProfessionViewModel>(
                    response.PhysicalPersonProfessions ?? new List<PhysicalPersonProfessionViewModel>());
            }
            else
            {
                PhysicalPersonProfessionsFromDB = new ObservableCollection<PhysicalPersonProfessionViewModel>();
            }

            PhysicalPersonDocumentDataLoading = false;
        }

        private void DisplayPhysicalPersonLicenceData()
        {
            PhysicalPersonLicenceDataLoading = true;

            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceSQLiteRepository()
                .GetPhysicalPersonLicencesByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonLicencesFromDB = new ObservableCollection<PhysicalPersonLicenceViewModel>(
                    response.PhysicalPersonLicences ?? new List<PhysicalPersonLicenceViewModel>());
            }
            else
            {
                PhysicalPersonLicencesFromDB = new ObservableCollection<PhysicalPersonLicenceViewModel>();
            }

            PhysicalPersonLicenceDataLoading = false;
        }

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

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Računi ... ";
            new PhysicalPersonSQLiteRepository().Sync(physicalPersonService, (synced, toSync) =>
            {
                SyncButtonContent = " Računi (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonNoteSQLiteRepository().Sync(physicalPersonNoteService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonDocumentSQLiteRepository().Sync(physicalPersonDocumentService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonCardSQLiteRepository().Sync(physicalPersonCardService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonProfessionSQLiteRepository().Sync(physicalPersonProfessionService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonLicenceSQLiteRepository().Sync(physicalPersonLicenceService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonItemSQLiteRepository().Sync(physicalPersonItemService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });
            DisplayData();
            CurrentPhysicalPerson = null;
            PhysicalPersonNotesFromDB = new ObservableCollection<PhysicalPersonNoteViewModel>();
            PhysicalPersonDocumentsFromDB = new ObservableCollection<PhysicalPersonDocumentViewModel>();
            PhysicalPersonCardsFromDB = new ObservableCollection<PhysicalPersonCardViewModel>();
            PhysicalPersonProfessionsFromDB = new ObservableCollection<PhysicalPersonProfessionViewModel>();
            PhysicalPersonLicencesFromDB = new ObservableCollection<PhysicalPersonLicenceViewModel>();
            PhysicalPersonItemsFromDB = new ObservableCollection<PhysicalPersonItemViewModel>();
            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }
        private void SyncNoteData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonNoteSQLiteRepository().Sync(physicalPersonNoteService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayPhysicalPersonNoteData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }

        private void SyncDocumentData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonDocumentSQLiteRepository().Sync(physicalPersonDocumentService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayPhysicalPersonDocumentData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }

        private void SyncCardData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonCardSQLiteRepository().Sync(physicalPersonCardService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayPhysicalPersonCardData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }

        private void SyncProfessionData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonProfessionSQLiteRepository().Sync(physicalPersonProfessionService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayPhysicalPersonProfessionData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }

        private void SyncLicenceData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonLicenceSQLiteRepository().Sync(physicalPersonLicenceService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayPhysicalPersonLicenceData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }

        private void SyncItemData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new PhysicalPersonItemSQLiteRepository().Sync(physicalPersonItemService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayPhysicalPersonItemData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }
        private void DgPhysicalPersons_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgPhysicalPersonCards_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgPhysicalPersonNotes_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgPhysicalPersonDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgPhysicalPersonProfessions_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgPhysicalPersonLicences_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgPhysicalPersonItems_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
       
        #endregion

        #region Add, edit, delete, lock and cancel

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PhysicalPersonViewModel physicalPerson = new PhysicalPersonViewModel();
            physicalPerson.Identifier = Guid.NewGuid();
            //physicalPerson.InvoiceDate = DateTime.Now;

            PhysicalPerson_AddEdit addEditForm = new PhysicalPerson_AddEdit(physicalPerson, true, false);
            addEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_fizičkim_licima")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPhysicalPerson == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_fizičko_lice_za_izmenuUzvičnik"));
                return;
            }

            PhysicalPerson_AddEdit addEditForm = new PhysicalPerson_AddEdit(CurrentPhysicalPerson, false);
            addEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_fizičkom_licu")), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPhysicalPerson == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_fizičko_lice_za_brisanjeUzvičnik"));
                return;
            }

            // Delete data
            var result = physicalPersonService.Delete(CurrentPhysicalPerson.Identifier);
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

        private void BtnAddCards_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhysicalPerson == null)
            {
                MainWindow.WarningMessage = "Morate odabrati račun!";
                return;
            }

            #endregion

            PhysicalPerson_Card_AddEdit physicalPersonCardAddEditForm = new PhysicalPerson_Card_AddEdit(CurrentPhysicalPerson);
            physicalPersonCardAddEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncItemData);
            FlyoutHelper.OpenFlyout(this, "Podaci", 95, physicalPersonCardAddEditForm);
        }

        private void BtnAddDocuments_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhysicalPerson == null)
            {
                MainWindow.WarningMessage = "Morate odabrati račun!";
                return;
            }

            #endregion

            PhysicalPerson_Document_AddEdit physicalPersonDocumentAddEditForm = new PhysicalPerson_Document_AddEdit(CurrentPhysicalPerson);
            physicalPersonDocumentAddEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncDocumentData);
            FlyoutHelper.OpenFlyout(this, "Podaci", 95, physicalPersonDocumentAddEditForm);
        }

        private void BtnAddNotes_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhysicalPerson == null)
            {
                MainWindow.WarningMessage = "Morate odabrati račun!";
                return;
            }

            #endregion

            PhysicalPerson_Note_AddEdit physicalPersonNoteAddEditForm = new PhysicalPerson_Note_AddEdit(CurrentPhysicalPerson);
            physicalPersonNoteAddEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncDocumentData);
            FlyoutHelper.OpenFlyout(this, "Podaci", 95, physicalPersonNoteAddEditForm);
        }
        private void BtnAddProfessions_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhysicalPerson == null)
            {
                MainWindow.WarningMessage = "Morate odabrati račun!";
                return;
            }

            #endregion

            PhysicalPerson_Profession_AddEdit physicalPersonProfessionAddEditForm = new PhysicalPerson_Profession_AddEdit(CurrentPhysicalPerson);
            physicalPersonProfessionAddEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncDocumentData);
            FlyoutHelper.OpenFlyout(this, "Podaci", 95, physicalPersonProfessionAddEditForm);
        }
        private void BtnAddLicences_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhysicalPerson == null)
            {
                MainWindow.WarningMessage = "Morate odabrati račun!";
                return;
            }

            #endregion

            PhysicalPerson_Licence_AddEdit physicalPersonLicenceAddEditForm = new PhysicalPerson_Licence_AddEdit(CurrentPhysicalPerson);
            physicalPersonLicenceAddEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncDocumentData);
            FlyoutHelper.OpenFlyout(this, "Podaci", 95, physicalPersonLicenceAddEditForm);
        }
        private void BtnAddItems_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhysicalPerson == null)
            {
                MainWindow.WarningMessage = "Morate odabrati račun!";
                return;
            }

            #endregion

            PhysicalPerson_Item_AddEdit physicalPersonItemAddEditForm = new PhysicalPerson_Item_AddEdit(CurrentPhysicalPerson);
            physicalPersonItemAddEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncDocumentData);
            FlyoutHelper.OpenFlyout(this, "Podaci", 95, physicalPersonItemAddEditForm);
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
                PhysicalPersonsExcelReport.Show(PhysicalPersonsFromDB.ToList());
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }
        //private void btnShow_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        System.Diagnostics.Process process = new System.Diagnostics.Process();
        //        //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
        //        Uri pdf = new Uri(CurrentPhysicalPerson.Path, UriKind.RelativeOrAbsolute);
        //        process.StartInfo.FileName = pdf.LocalPath;
        //        process.Start();
        //    }
        //    catch (Exception error)
        //    {
        //        MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
        //}
        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PhysicalPersonExcelReport.Show(CurrentPhysicalPerson);
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }
        #region Display documents

        private void btnShowDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentPhysicalPersonDocument.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void txtSearchByPhysicalPersonCode_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #endregion
        //#region Attributes

        //#region Services
        //IPhysicalPersonService physicalPersonService;
        //      IPhysicalPersonItemService physicalPersonItemService;
        //      IPhysicalPersonLicenceService physicalPersonLicenceService;
        //      IPhysicalPersonProfessionService physicalPersonProfessionService;
        //      IPhysicalPersonDocumentService physicalPersonDocumentService;
        //      IPhysicalPersonCardService physicalPersonCardService;
        //      IPhysicalPersonNoteService physicalPersonNoteService;
        //      #endregion

        //      #region PhysicalPersonsFromDB
        //      private ObservableCollection<PhysicalPersonViewModel> _PhysicalPersonsFromDB;

        //      public ObservableCollection<PhysicalPersonViewModel> PhysicalPersonsFromDB
        //      {
        //          get { return _PhysicalPersonsFromDB; }
        //          set
        //          {
        //              if (_PhysicalPersonsFromDB != value)
        //              {
        //                  _PhysicalPersonsFromDB = value;
        //                  NotifyPropertyChanged("PhysicalPersonsFromDB");
        //              }
        //          }
        //      }
        //      #endregion

        //      #region CurrentPhysicalPerson
        //      private PhysicalPersonViewModel _CurrentPhysicalPerson;

        //      public PhysicalPersonViewModel CurrentPhysicalPerson
        //      {
        //          get { return _CurrentPhysicalPerson; }
        //          set
        //          {
        //              if (_CurrentPhysicalPerson != value)
        //              {
        //                  _CurrentPhysicalPerson = value;
        //                  NotifyPropertyChanged("CurrentPhysicalPerson");


        //                  if (_CurrentPhysicalPerson != null)
        //                  {
        //                      Thread displayItemThread = new Thread(() =>
        //                      {
        //                          DisplayPhysicalPersonItemData();
        //                          DisplayProfessionData();
        //                          DisplayLicenceData();
        //                          DisplayDocumentData();
        //                          DisplayCardData();
        //					DisplayPhysicalPersonNoteData();

        //				});
        //                      displayItemThread.IsBackground = true;
        //                      displayItemThread.Start();
        //                  }
        //                  else
        //                      PhysicalPersonItemsFromDB = new ObservableCollection<PhysicalPersonItemViewModel>();
        //              }
        //          }
        //      }
        //      #endregion

        //      #region PhysicalPersonSearchObject
        //      private PhysicalPersonViewModel _PhysicalPersonSearchObject = new PhysicalPersonViewModel();

        //      public PhysicalPersonViewModel PhysicalPersonSearchObject
        //      {
        //          get { return _PhysicalPersonSearchObject; }
        //          set
        //          {
        //              if (_PhysicalPersonSearchObject != value)
        //              {
        //                  _PhysicalPersonSearchObject = value;
        //                  NotifyPropertyChanged("PhysicalPersonSearchObject");
        //              }
        //          }
        //      }
        //      #endregion

        //      #region PhysicalPersonDataLoading
        //      private bool _PhysicalPersonDataLoading = true;

        //      public bool PhysicalPersonDataLoading
        //      {
        //          get { return _PhysicalPersonDataLoading; }
        //          set
        //          {
        //              if (_PhysicalPersonDataLoading != value)
        //              {
        //                  _PhysicalPersonDataLoading = value;
        //                  NotifyPropertyChanged("PhysicalPersonDataLoading");
        //              }
        //          }
        //      }
        //      #endregion


        //      #region PhysicalPersonItemsFromDB
        //      private ObservableCollection<PhysicalPersonItemViewModel> _PhysicalPersonItemsFromDB;

        //      public ObservableCollection<PhysicalPersonItemViewModel> PhysicalPersonItemsFromDB
        //      {
        //          get { return _PhysicalPersonItemsFromDB; }
        //          set
        //          {
        //              if (_PhysicalPersonItemsFromDB != value)
        //              {
        //                  _PhysicalPersonItemsFromDB = value;
        //                  NotifyPropertyChanged("PhysicalPersonItemsFromDB");
        //              }
        //          }
        //      }
        //      #endregion

        //      #region PhysicalPersonItemDataLoading
        //      private bool _PhysicalPersonItemDataLoading;

        //      public bool PhysicalPersonItemDataLoading
        //      {
        //          get { return _PhysicalPersonItemDataLoading; }
        //          set
        //          {
        //              if (_PhysicalPersonItemDataLoading != value)
        //              {
        //                  _PhysicalPersonItemDataLoading = value;
        //                  NotifyPropertyChanged("PhysicalPersonItemDataLoading");
        //              }
        //          }
        //      }
        //      #endregion


        //      #region PhysicalPersonProfessionsFromDB
        //      private ObservableCollection<PhysicalPersonProfessionViewModel> _PhysicalPersonProfessionsFromDB;

        //      public ObservableCollection<PhysicalPersonProfessionViewModel> PhysicalPersonProfessionsFromDB
        //      {
        //          get { return _PhysicalPersonProfessionsFromDB; }
        //          set
        //          {
        //              if (_PhysicalPersonProfessionsFromDB != value)
        //              {
        //                  _PhysicalPersonProfessionsFromDB = value;
        //                  NotifyPropertyChanged("PhysicalPersonProfessionsFromDB");
        //              }
        //          }
        //      }
        //      #endregion

        //      #region LoadingProfessions
        //      private bool _LoadingProfessions;

        //      public bool LoadingProfessions
        //      {
        //          get { return _LoadingProfessions; }
        //          set
        //          {
        //              if (_LoadingProfessions != value)
        //              {
        //                  _LoadingProfessions = value;
        //                  NotifyPropertyChanged("LoadingProfessions");
        //              }
        //          }
        //      }
        //      #endregion


        //      #region PhysicalPersonLicencesFromDB
        //      private ObservableCollection<PhysicalPersonLicenceViewModel> _PhysicalPersonLicencesFromDB;

        //      public ObservableCollection<PhysicalPersonLicenceViewModel> PhysicalPersonLicencesFromDB
        //      {
        //          get { return _PhysicalPersonLicencesFromDB; }
        //          set
        //          {
        //              if (_PhysicalPersonLicencesFromDB != value)
        //              {
        //                  _PhysicalPersonLicencesFromDB = value;
        //                  NotifyPropertyChanged("PhysicalPersonLicencesFromDB");
        //              }
        //          }
        //      }
        //      #endregion

        //      #region LoadingLicences
        //      private bool _LoadingLicences;

        //      public bool LoadingLicences
        //      {
        //          get { return _LoadingLicences; }
        //          set
        //          {
        //              if (_LoadingLicences != value)
        //              {
        //                  _LoadingLicences = value;
        //                  NotifyPropertyChanged("LoadingLicences");
        //              }
        //          }
        //      }
        //#endregion

        //#region NotesFromDB
        //private ObservableCollection<PhysicalPersonNoteViewModel> _NotesFromDB;

        //public ObservableCollection<PhysicalPersonNoteViewModel> NotesFromDB
        //{
        //	get { return _NotesFromDB; }
        //	set
        //	{
        //		if (_NotesFromDB != value)
        //		{
        //			_NotesFromDB = value;
        //			NotifyPropertyChanged("NotesFromDB");
        //		}
        //	}
        //}
        //#endregion

        //#region CurrentNoteForm
        //private PhysicalPersonNoteViewModel _CurrentNoteForm = new PhysicalPersonNoteViewModel();

        //public PhysicalPersonNoteViewModel CurrentNoteForm
        //{
        //	get { return _CurrentNoteForm; }
        //	set
        //	{
        //		if (_CurrentNoteForm != value)
        //		{
        //			_CurrentNoteForm = value;
        //			NotifyPropertyChanged("CurrentNoteForm");
        //		}
        //	}
        //}
        //#endregion

        //#region CurrentNoteDG
        //private PhysicalPersonNoteViewModel _CurrentNoteDG;

        //public PhysicalPersonNoteViewModel CurrentNoteDG
        //{
        //	get { return _CurrentNoteDG; }
        //	set
        //	{
        //		if (_CurrentNoteDG != value)
        //		{
        //			_CurrentNoteDG = value;
        //			NotifyPropertyChanged("CurrentNoteDG");
        //		}
        //	}
        //}
        //#endregion

        //#region NoteDataLoading
        //private bool _NoteDataLoading;

        //public bool NoteDataLoading
        //{
        //	get { return _NoteDataLoading; }
        //	set
        //	{
        //		if (_NoteDataLoading != value)
        //		{
        //			_NoteDataLoading = value;
        //			NotifyPropertyChanged("NoteDataLoading");
        //		}
        //	}
        //}
        //#endregion


        //#region PhysicalPersonDocumentsFromDB
        //private ObservableCollection<PhysicalPersonDocumentViewModel> _PhysicalPersonDocumentsFromDB;

        //      public ObservableCollection<PhysicalPersonDocumentViewModel> PhysicalPersonDocumentsFromDB
        //      {
        //          get { return _PhysicalPersonDocumentsFromDB; }
        //          set
        //          {
        //              if (_PhysicalPersonDocumentsFromDB != value)
        //              {
        //                  _PhysicalPersonDocumentsFromDB = value;
        //                  NotifyPropertyChanged("PhysicalPersonDocumentsFromDB");
        //              }
        //          }
        //      }
        //      #endregion

        //      #region CurrentPhysicalPersonDocument
        //      private PhysicalPersonDocumentViewModel _CurrentPhysicalPersonDocument;

        //      public PhysicalPersonDocumentViewModel CurrentPhysicalPersonDocument
        //      {
        //          get { return _CurrentPhysicalPersonDocument; }
        //          set
        //          {
        //              if (_CurrentPhysicalPersonDocument != value)
        //              {
        //                  _CurrentPhysicalPersonDocument = value;
        //                  NotifyPropertyChanged("CurrentPhysicalPersonDocument");
        //              }
        //          }
        //      }
        //      #endregion

        //      #region PhysicalPersonDocumentDataLoading
        //      private bool _PhysicalPersonDocumentDataLoading;

        //      public bool PhysicalPersonDocumentDataLoading
        //      {
        //          get { return _PhysicalPersonDocumentDataLoading; }
        //          set
        //          {
        //              if (_PhysicalPersonDocumentDataLoading != value)
        //              {
        //                  _PhysicalPersonDocumentDataLoading = value;
        //                  NotifyPropertyChanged("PhysicalPersonDocumentDataLoading");
        //              }
        //          }
        //      }
        //      #endregion


        //      #region PhysicalPersonCardsFromDB
        //      private ObservableCollection<PhysicalPersonCardViewModel> _PhysicalPersonCardsFromDB;

        //      public ObservableCollection<PhysicalPersonCardViewModel> PhysicalPersonCardsFromDB
        //      {
        //          get { return _PhysicalPersonCardsFromDB; }
        //          set
        //          {
        //              if (_PhysicalPersonCardsFromDB != value)
        //              {
        //                  _PhysicalPersonCardsFromDB = value;
        //                  NotifyPropertyChanged("PhysicalPersonCardsFromDB");
        //              }
        //          }
        //      }
        //      #endregion

        //      #region PhysicalPersonCardDataLoading
        //      private bool _PhysicalPersonCardDataLoading;

        //      public bool PhysicalPersonCardDataLoading
        //      {
        //          get { return _PhysicalPersonCardDataLoading; }
        //          set
        //          {
        //              if (_PhysicalPersonCardDataLoading != value)
        //              {
        //                  _PhysicalPersonCardDataLoading = value;
        //                  NotifyPropertyChanged("PhysicalPersonCardDataLoading");
        //              }
        //          }
        //      }
        //      #endregion



        //      #region Pagination data
        //      int currentPage = 1;
        //int itemsPerPage = 50;
        //int totalItems = 0;

        //#region PaginationDisplay
        //private string _PaginationDisplay;

        //public string PaginationDisplay
        //{
        //	get { return _PaginationDisplay; }
        //	set
        //	{
        //		if (_PaginationDisplay != value)
        //		{
        //			_PaginationDisplay = value;
        //			NotifyPropertyChanged("PaginationDisplay");
        //		}
        //	}
        //}
        //#endregion
        //#endregion


        //#region RefreshButtonContent
        //private string _RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

        //      public string RefreshButtonContent
        //{
        //	get { return _RefreshButtonContent; }
        //	set
        //	{
        //		if (_RefreshButtonContent != value)
        //		{
        //			_RefreshButtonContent = value;
        //			NotifyPropertyChanged("RefreshButtonContent");
        //		}
        //	}
        //}
        //#endregion

        //#region RefreshButtonEnabled
        //private bool _RefreshButtonEnabled = true;

        //public bool RefreshButtonEnabled
        //{
        //	get { return _RefreshButtonEnabled; }
        //	set
        //	{
        //		if (_RefreshButtonEnabled != value)
        //		{
        //			_RefreshButtonEnabled = value;
        //			NotifyPropertyChanged("RefreshButtonEnabled");
        //		}
        //	}
        //}
        //#endregion

        //#endregion

        //#region Constructor

        //public PhysicalPerson_List()
        //{
        //	// Get required services
        //	this.physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();
        //          this.physicalPersonItemService = DependencyResolver.Kernel.Get<IPhysicalPersonItemService>();
        //          this.physicalPersonLicenceService = DependencyResolver.Kernel.Get<IPhysicalPersonLicenceService>();
        //          this.physicalPersonProfessionService = DependencyResolver.Kernel.Get<IPhysicalPersonProfessionService>();
        //          this.physicalPersonDocumentService = DependencyResolver.Kernel.Get<IPhysicalPersonDocumentService>();
        //          this.physicalPersonCardService = DependencyResolver.Kernel.Get<IPhysicalPersonCardService>();
        //          this.physicalPersonNoteService = DependencyResolver.Kernel.Get<IPhysicalPersonNoteService>();

        //          InitializeComponent();

        //	this.DataContext = this;

        //	Thread displayThread = new Thread(() => SyncData());
        //	displayThread.IsBackground = true;
        //	displayThread.Start();
        //}

        //#endregion

        //#region Display data

        //private void btnRefresh_Click(object sender, RoutedEventArgs e)
        //{
        //	currentPage = 1;

        //	Thread syncThread = new Thread(() =>
        //	{
        //		SyncData();

        //		MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sinhronizovaniUzvičnik"));
        //          });
        //	syncThread.IsBackground = true;
        //	syncThread.Start();
        //}

        //private void btnSearch_Click(object sender, RoutedEventArgs e)
        //{
        //	currentPage = 1;

        //	Thread displayThread = new Thread(() => DisplayData());
        //	displayThread.IsBackground = true;
        //	displayThread.Start();
        //}

        //public void DisplayData()
        //{
        //	PhysicalPersonDataLoading = true;

        //	PhysicalPersonListResponse response = new PhysicalPersonSQLiteRepository()
        //		.GetPhysicalPersonsByPage(MainWindow.CurrentCompanyId, PhysicalPersonSearchObject, currentPage, itemsPerPage);

        //	if (response.Success)
        //	{
        //		PhysicalPersonsFromDB = new ObservableCollection<PhysicalPersonViewModel>(response.PhysicalPersons ?? new List<PhysicalPersonViewModel>());
        //		totalItems = response.TotalItems;
        //	}
        //	else
        //	{
        //		PhysicalPersonsFromDB = new ObservableCollection<PhysicalPersonViewModel>();
        //		totalItems = 0;
        //		MainWindow.ErrorMessage = response.Message;
        //	}

        //	int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
        //	int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

        //	PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

        //	PhysicalPersonDataLoading = false;
        //}

        //      private void DisplayPhysicalPersonItemData()
        //      {
        //          PhysicalPersonItemDataLoading = true;

        //          PhysicalPersonItemListResponse response = new PhysicalPersonItemSQLiteRepository()
        //              .GetPhysicalPersonItemsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

        //          if (response.Success)
        //          {
        //              PhysicalPersonItemsFromDB = new ObservableCollection<PhysicalPersonItemViewModel>(
        //                  response.PhysicalPersonItems ?? new List<PhysicalPersonItemViewModel>());
        //          }
        //          else
        //          {
        //              PhysicalPersonItemsFromDB = new ObservableCollection<PhysicalPersonItemViewModel>();
        //              MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_prilikom_učitavanja_podatakaUzvičnik"));
        //          }

        //          PhysicalPersonItemDataLoading = false;
        //      }

        //      private void DisplayProfessionData()
        //      {
        //          LoadingProfessions = true;

        //          PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionSQLiteRepository()
        //              .GetPhysicalPersonProfessionsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

        //          if (response.Success)
        //          {
        //              PhysicalPersonProfessionsFromDB = new ObservableCollection<PhysicalPersonProfessionViewModel>(
        //                  response.PhysicalPersonProfessions ?? new List<PhysicalPersonProfessionViewModel>());
        //          }
        //          else
        //          {
        //              PhysicalPersonProfessionsFromDB = new ObservableCollection<PhysicalPersonProfessionViewModel>();
        //          }

        //          LoadingProfessions = false;
        //      }

        //private void DisplayPhysicalPersonNoteData()
        //{
        //	NoteDataLoading = true;

        //	PhysicalPersonNoteListResponse response = new PhysicalPersonNoteSQLiteRepository()
        //		.GetPhysicalPersonNotesByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

        //	if (response.Success)
        //	{
        //		NotesFromDB = new ObservableCollection<PhysicalPersonNoteViewModel>(
        //			response.PhysicalPersonNotes ?? new List<PhysicalPersonNoteViewModel>());
        //	}
        //	else
        //	{
        //		NotesFromDB = new ObservableCollection<PhysicalPersonNoteViewModel>();
        //	}

        //	NoteDataLoading = false;
        //}

        //private void DisplayLicenceData()
        //      {
        //          LoadingLicences = true;

        //          PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceSQLiteRepository()
        //              .GetPhysicalPersonLicencesByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

        //          if (response.Success)
        //          {
        //              PhysicalPersonLicencesFromDB = new ObservableCollection<PhysicalPersonLicenceViewModel>(
        //                  response.PhysicalPersonLicences ?? new List<PhysicalPersonLicenceViewModel>());
        //          }
        //          else
        //          {
        //              PhysicalPersonLicencesFromDB = new ObservableCollection<PhysicalPersonLicenceViewModel>();
        //          }

        //          LoadingLicences = false;
        //      }

        //      private void DisplayDocumentData()
        //      {
        //          PhysicalPersonDocumentDataLoading = true;

        //          PhysicalPersonDocumentListResponse response = new PhysicalPersonDocumentSQLiteRepository()
        //              .GetPhysicalPersonDocumentsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

        //          if (response.Success)
        //          {
        //              PhysicalPersonDocumentsFromDB = new ObservableCollection<PhysicalPersonDocumentViewModel>(
        //                  response.PhysicalPersonDocuments ?? new List<PhysicalPersonDocumentViewModel>());
        //          }
        //          else
        //          {
        //              PhysicalPersonDocumentsFromDB = new ObservableCollection<PhysicalPersonDocumentViewModel>();
        //          }

        //          PhysicalPersonDocumentDataLoading = false;
        //      }

        //      private void DisplayCardData()
        //      {
        //          PhysicalPersonCardDataLoading = true;

        //          PhysicalPersonCardListResponse response = new PhysicalPersonCardSQLiteRepository()
        //              .GetPhysicalPersonCardsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

        //          if (response.Success)
        //          {
        //              PhysicalPersonCardsFromDB = new ObservableCollection<PhysicalPersonCardViewModel>(
        //                  response.PhysicalPersonCards ?? new List<PhysicalPersonCardViewModel>());
        //          }
        //          else
        //          {
        //              PhysicalPersonCardsFromDB = new ObservableCollection<PhysicalPersonCardViewModel>();
        //          }

        //          PhysicalPersonCardDataLoading = false;
        //      }

        //      private void SyncData()
        //{
        //	RefreshButtonEnabled = false;

        //	RefreshButtonContent = ((string)Application.Current.FindResource("Fizička_lica_TriTacke"));
        //          new PhysicalPersonSQLiteRepository().Sync(physicalPersonService);

        //          RefreshButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
        //          new PhysicalPersonItemSQLiteRepository().Sync(physicalPersonItemService);
        //          new PhysicalPersonLicenceSQLiteRepository().Sync(physicalPersonLicenceService);
        //          new PhysicalPersonProfessionSQLiteRepository().Sync(physicalPersonProfessionService);
        //          new PhysicalPersonDocumentSQLiteRepository().Sync(physicalPersonDocumentService);
        //          new PhysicalPersonCardSQLiteRepository().Sync(physicalPersonCardService);
        //          new PhysicalPersonNoteSQLiteRepository().Sync(physicalPersonNoteService);

        //          DisplayData();

        //	RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
        //          RefreshButtonEnabled = true;
        //}

        //#endregion

        //#region Add, edit and delete methods

        //private void btnAdd_Click(object sender, RoutedEventArgs e)
        //{
        //	PhysicalPersonViewModel physicalPerson = new PhysicalPersonViewModel();
        //	physicalPerson.Identifier = Guid.NewGuid();

        //	PhysicalPerson_AddEdit addEditForm = new PhysicalPerson_AddEdit(physicalPerson, true);
        //	addEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncData);
        //	FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_fizičkim_licima")), 95, addEditForm);
        //}

        //private void btnEdit_Click(object sender, RoutedEventArgs e)
        //{
        //	if (CurrentPhysicalPerson == null)
        //	{
        //		MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_fizičko_lice_za_izmenuUzvičnik"));
        //              return;
        //	}

        //	PhysicalPerson_AddEdit addEditForm = new PhysicalPerson_AddEdit(CurrentPhysicalPerson, false);
        //	addEditForm.PhysicalPersonCreatedUpdated += new PhysicalPersonHandler(SyncData);
        //	FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_fizičkom_licu")), 95, addEditForm);
        //}

        //private void btnDelete_Click(object sender, RoutedEventArgs e)
        //{
        //	if (CurrentPhysicalPerson == null)
        //	{
        //		MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_fizičko_lice_za_brisanjeUzvičnik"));
        //              return;
        //	}

        //	SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //	// Create confirmation window
        //	DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("banka", CurrentPhysicalPerson.Name + CurrentPhysicalPerson.Code);
        //	var showDialog = deleteConfirmationForm.ShowDialog();
        //	if (showDialog != null && showDialog.Value)
        //	{
        //		PhysicalPersonResponse response = physicalPersonService.Delete(CurrentPhysicalPerson.Identifier);
        //		if (!response.Success)
        //		{
        //			MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
        //                  SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //			return;
        //		}

        //		response = new PhysicalPersonSQLiteRepository().Delete(CurrentPhysicalPerson.Identifier);
        //		if (!response.Success)
        //		{
        //			MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
        //                  SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //			return;
        //		}

        //		MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Banka_je_uspešno_obrisanaUzvičnik"));

        //              Thread displayThread = new Thread(() => SyncData());
        //		displayThread.IsBackground = true;
        //		displayThread.Start();
        //	}

        //	SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //      #endregion

        //      #region Display documents

        //      private void btnShowDocument_Click(object sender, RoutedEventArgs e)
        //      {
        //          try
        //          {
        //              System.Diagnostics.Process process = new System.Diagnostics.Process();
        //              //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
        //              Uri pdf = new Uri(CurrentPhysicalPersonDocument.Path, UriKind.RelativeOrAbsolute);
        //              process.StartInfo.FileName = pdf.LocalPath;
        //              process.Start();
        //          }
        //          catch (Exception error)
        //          {
        //              MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        //          }
        //      }

        //      #endregion

        //      #region Pagination

        //      private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        //{
        //	if (currentPage > 1)
        //	{
        //		currentPage = 1;

        //		Thread displayThread = new Thread(() => DisplayData());
        //		displayThread.IsBackground = true;
        //		displayThread.Start();
        //	}
        //}

        //private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        //{
        //	if (currentPage > 1)
        //	{
        //		currentPage--;

        //		Thread displayThread = new Thread(() => DisplayData());
        //		displayThread.IsBackground = true;
        //		displayThread.Start();
        //	}
        //}

        //private void btnNextPage_Click(object sender, RoutedEventArgs e)
        //{
        //	if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
        //	{
        //		currentPage++;

        //		Thread displayThread = new Thread(() => DisplayData());
        //		displayThread.IsBackground = true;
        //		displayThread.Start();
        //	}
        //}

        //private void btnLastPage_Click(object sender, RoutedEventArgs e)
        //{
        //	int lastPage = (int)Math.Ceiling((double)this.totalItems / this.itemsPerPage);
        //	if (currentPage < lastPage)
        //	{
        //		currentPage = lastPage;

        //		Thread displayThread = new Thread(() => DisplayData());
        //		displayThread.IsBackground = true;
        //		displayThread.Start();
        //	}
        //}

        //#endregion

        //#region INotifyPropertyChanged implementation
        //public event PropertyChangedEventHandler PropertyChanged;


        //// This method is called by the Set accessor of each property.
        //// The CallerMemberName attribute that is applied to the optional propertyName
        //// parameter causes the property name of the caller to be substituted as an argument.
        //private void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        //{
        //	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //#endregion

        //private void btnExcel_Click(object sender, RoutedEventArgs e)
        //{
        //          try
        //          {
        //              var personResponse = new PhysicalPersonSQLiteRepository().GetPhysicalPersonsByPage(MainWindow.CurrentCompanyId, PhysicalPersonSearchObject, 1, Int32.MaxValue);
        //              PhysicalPersonsExcelReport.Show(personResponse.PhysicalPersons);
        //          }
        //          catch(Exception ex)
        //          {
        //              MainWindow.ErrorMessage = ex.Message;
        //          }
        //}

        //      private void btnPhysicalPersonExcel_Click(object sender, RoutedEventArgs e)
        //      {
        //          try
        //          {
        //              PhysicalPersonExcelReport.Show(CurrentPhysicalPerson);
        //          }
        //          catch (Exception ex)
        //          {
        //              MainWindow.ErrorMessage = ex.Message;
        //          }
        //      }
        //      private void txtSearchByPhysicalPersonCode_TextChanged(object sender, TextChangedEventArgs e)
        //      {
        //          //PhysicalPersonListResponse response = new PhysicalPersonSQLiteRepository()
        //          //     .GetPhysicalPersonsByPage(MainWindow.CurrentCompanyId, PhysicalPersonSearchObject, currentPage, itemsPerPage);

        //          //if (response.Success)
        //          //{
        //          //    List<PhysicalPersonViewModel> filteredItems;
        //          //    if (!String.IsNullOrEmpty(txtSearchByPhysicalPersonCode.Text))
        //          //        filteredItems = response.PhysicalPersons.Where(x => x.PhysicalPersonCode.Contains(txtSearchByPhysicalPersonCode.Text))?.ToList();
        //          //    else
        //          //        filteredItems = response.PhysicalPersons;

        //          //    PhysicalPersonsFromDB = new ObservableCollection<PhysicalPersonViewModel>(
        //          //        filteredItems ?? new List<PhysicalPersonViewModel>());
        //          //}
        //          //else
        //          //{
        //          //    PhysicalPersonsFromDB = new ObservableCollection<PhysicalPersonViewModel>();
        //          //    MainWindow.ErrorMessage = "Greška prilikom učitavanja podataka!";
        //          //}
        //          Thread th = new Thread(() => DisplayData());
        //          th.IsBackground = true;
        //          th.Start();
        //      }
        //  }
    }
}
