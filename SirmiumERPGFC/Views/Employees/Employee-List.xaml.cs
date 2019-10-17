using Newtonsoft.Json;
using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
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
    public delegate void EmployeeHandler();

    public partial class Employee_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IEmployeeService employeeService;
        IEmployeeItemService employeeItemService;
        IEmployeeLicenceService employeeLicenceService;
        IEmployeeProfessionService employeeProfessionService;
        IEmployeeDocumentService employeeDocumentService;
        IEmployeeCardService employeeCardService;
        IEmployeeNoteService employeeNoteService;
        #endregion

        #region EmployeeSearchObject
        private EmployeeViewModel _EmployeeSearchObject = new EmployeeViewModel();

        public EmployeeViewModel EmployeeSearchObject
        {
            get { return _EmployeeSearchObject; }
            set
            {
                if (_EmployeeSearchObject != value)
                {
                    _EmployeeSearchObject = value;
                    NotifyPropertyChanged("EmployeeSearchObject");
                }
            }
        }
        #endregion

        #region EmployeesFromDB
        private ObservableCollection<EmployeeViewModel> _EmployeesFromDB;

        public ObservableCollection<EmployeeViewModel> EmployeesFromDB
        {
            get { return _EmployeesFromDB; }
            set
            {
                if (_EmployeesFromDB != value)
                {
                    _EmployeesFromDB = value;
                    NotifyPropertyChanged("EmployeesFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployee
        private EmployeeViewModel _CurrentEmployee;

        public EmployeeViewModel CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set
            {
                if (_CurrentEmployee != value)
                {
                    _CurrentEmployee = value;
                    NotifyPropertyChanged("CurrentEmployee");


                    if (_CurrentEmployee != null)
                    {
                        Thread displayItemThread = new Thread(() =>
                        {
                            DisplayEmployeeItemData();
                            DisplayEmployeeProfessionData();
                            DisplayEmployeeLicenceData();
                            DisplayEmployeeDocumentData();
                            DisplayEmployeeCardData();
                            DisplayEmployeeNoteData();

                        });
                        displayItemThread.IsBackground = true;
                        displayItemThread.Start();
                    }
                    else
                        EmployeeItemsFromDB = new ObservableCollection<EmployeeItemViewModel>();
                }
            }
        }
        #endregion

        #region EmployeeDataLoading
        private bool _EmployeeDataLoading = true;

        public bool EmployeeDataLoading
        {
            get { return _EmployeeDataLoading; }
            set
            {
                if (_EmployeeDataLoading != value)
                {
                    _EmployeeDataLoading = value;
                    NotifyPropertyChanged("EmployeeDataLoading");
                }
            }
        }
        #endregion



        #region EmployeeDocumentsFromDB
        private ObservableCollection<EmployeeDocumentViewModel> _EmployeeDocumentsFromDB;

        public ObservableCollection<EmployeeDocumentViewModel> EmployeeDocumentsFromDB
        {
            get { return _EmployeeDocumentsFromDB; }
            set
            {
                if (_EmployeeDocumentsFromDB != value)
                {
                    _EmployeeDocumentsFromDB = value;
                    NotifyPropertyChanged("EmployeeDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeDocument
        private EmployeeDocumentViewModel _CurrentEmployeeDocument;

        public EmployeeDocumentViewModel CurrentEmployeeDocument
        {
            get { return _CurrentEmployeeDocument; }
            set
            {
                if (_CurrentEmployeeDocument != value)
                {
                    _CurrentEmployeeDocument = value;
                    NotifyPropertyChanged("CurrentEmployeeDocument");
                }
            }
        }
        #endregion

        #region EmployeeDocumentDataLoading
        private bool _EmployeeDocumentDataLoading;

        public bool EmployeeDocumentDataLoading
        {
            get { return _EmployeeDocumentDataLoading; }
            set
            {
                if (_EmployeeDocumentDataLoading != value)
                {
                    _EmployeeDocumentDataLoading = value;
                    NotifyPropertyChanged("EmployeeDocumentDataLoading");
                }
            }
        }
        #endregion

        #region EmployeeNotesFromDB
        private ObservableCollection<EmployeeNoteViewModel> _EmployeeNotesFromDB;

        public ObservableCollection<EmployeeNoteViewModel> EmployeeNotesFromDB
        {
            get { return _EmployeeNotesFromDB; }
            set
            {
                if (_EmployeeNotesFromDB != value)
                {
                    _EmployeeNotesFromDB = value;
                    NotifyPropertyChanged("EmployeeNotesFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeNoteForm
        private EmployeeNoteViewModel _CurrentEmployeeNoteForm = new EmployeeNoteViewModel();

        public EmployeeNoteViewModel CurrentEmployeeNoteForm
        {
            get { return _CurrentEmployeeNoteForm; }
            set
            {
                if (_CurrentEmployeeNoteForm != value)
                {
                    _CurrentEmployeeNoteForm = value;
                    NotifyPropertyChanged("CurrentEmployeeNoteForm");
                }
            }
        }
        #endregion

        #region CurrentEmployeeNoteDG
        private EmployeeNoteViewModel _CurrentEmployeeNoteDG;

        public EmployeeNoteViewModel CurrentEmployeeNoteDG
        {
            get { return _CurrentEmployeeNoteDG; }
            set
            {
                if (_CurrentEmployeeNoteDG != value)
                {
                    _CurrentEmployeeNoteDG = value;
                    NotifyPropertyChanged("CurrentEmployeeNoteDG");
                }
            }
        }
        #endregion

        #region EmployeeNoteDataLoading
        private bool _EmployeeNoteDataLoading;

        public bool EmployeeNoteDataLoading
        {
            get { return _EmployeeNoteDataLoading; }
            set
            {
                if (_EmployeeNoteDataLoading != value)
                {
                    _EmployeeNoteDataLoading = value;
                    NotifyPropertyChanged("EmployeeNoteDataLoading");
                }
            }
        }
        #endregion

        #region EmployeeCardsFromDB
        private ObservableCollection<EmployeeCardViewModel> _EmployeeCardsFromDB;

        public ObservableCollection<EmployeeCardViewModel> EmployeeCardsFromDB
        {
            get { return _EmployeeCardsFromDB; }
            set
            {
                if (_EmployeeCardsFromDB != value)
                {
                    _EmployeeCardsFromDB = value;
                    NotifyPropertyChanged("EmployeeCardsFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeCard
        private EmployeeCardViewModel _CurrentEmployeeCard;

        public EmployeeCardViewModel CurrentEmployeeCard
        {
            get { return _CurrentEmployeeCard; }
            set
            {
                if (_CurrentEmployeeCard != value)
                {
                    _CurrentEmployeeCard = value;
                    NotifyPropertyChanged("CurrentEmployeeCard");
                }
            }
        }
        #endregion

        #region EmployeeCardDataLoading
        private bool _EmployeeCardDataLoading;

        public bool EmployeeCardDataLoading
        {
            get { return _EmployeeCardDataLoading; }
            set
            {
                if (_EmployeeCardDataLoading != value)
                {
                    _EmployeeCardDataLoading = value;
                    NotifyPropertyChanged("EmployeeCardDataLoading");
                }
            }
        }
        #endregion


        #region EmployeeProfessionsFromDB
        private ObservableCollection<EmployeeProfessionItemViewModel> _EmployeeProfessionsFromDB;

        public ObservableCollection<EmployeeProfessionItemViewModel> EmployeeProfessionsFromDB
        {
            get { return _EmployeeProfessionsFromDB; }
            set
            {
                if (_EmployeeProfessionsFromDB != value)
                {
                    _EmployeeProfessionsFromDB = value;
                    NotifyPropertyChanged("EmployeeProfessionsFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeProfession
        private EmployeeProfessionItemViewModel _CurrentEmployeeProfession;

        public EmployeeProfessionItemViewModel CurrentEmployeeProfession
        {
            get { return _CurrentEmployeeProfession; }
            set
            {
                if (_CurrentEmployeeProfession != value)
                {
                    _CurrentEmployeeProfession = value;
                    NotifyPropertyChanged("CurrentEmployeeProfession");
                }
            }
        }
        #endregion

        #region EmployeeProfessionDataLoading
        private bool _EmployeeProfessionDataLoading;

        public bool EmployeeProfessionDataLoading
        {
            get { return _EmployeeProfessionDataLoading; }
            set
            {
                if (_EmployeeProfessionDataLoading != value)
                {
                    _EmployeeProfessionDataLoading = value;
                    NotifyPropertyChanged("EmployeeProfessionDataLoading");
                }
            }
        }
        #endregion


        #region EmployeeLicencesFromDB
        private ObservableCollection<EmployeeLicenceItemViewModel> _EmployeeLicencesFromDB;

        public ObservableCollection<EmployeeLicenceItemViewModel> EmployeeLicencesFromDB
        {
            get { return _EmployeeLicencesFromDB; }
            set
            {
                if (_EmployeeLicencesFromDB != value)
                {
                    _EmployeeLicencesFromDB = value;
                    NotifyPropertyChanged("EmployeeLicencesFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeLicence
        private EmployeeLicenceItemViewModel _CurrentEmployeeLicence;

        public EmployeeLicenceItemViewModel CurrentEmployeeLicence
        {
            get { return _CurrentEmployeeLicence; }
            set
            {
                if (_CurrentEmployeeLicence != value)
                {
                    _CurrentEmployeeLicence = value;
                    NotifyPropertyChanged("CurrentEmployeeLicence");
                }
            }
        }
        #endregion

        #region EmployeeLicenceDataLoading
        private bool _EmployeeLicenceDataLoading;

        public bool EmployeeLicenceDataLoading
        {
            get { return _EmployeeLicenceDataLoading; }
            set
            {
                if (_EmployeeLicenceDataLoading != value)
                {
                    _EmployeeLicenceDataLoading = value;
                    NotifyPropertyChanged("EmployeeLicenceDataLoading");
                }
            }
        }
        #endregion


        #region EmployeeItemsFromDB
        private ObservableCollection<EmployeeItemViewModel> _EmployeeItemsFromDB;

        public ObservableCollection<EmployeeItemViewModel> EmployeeItemsFromDB
        {
            get { return _EmployeeItemsFromDB; }
            set
            {
                if (_EmployeeItemsFromDB != value)
                {
                    _EmployeeItemsFromDB = value;
                    NotifyPropertyChanged("EmployeeItemsFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeItem
        private EmployeeItemViewModel _CurrentEmployeeItem;

        public EmployeeItemViewModel CurrentEmployeeItem
        {
            get { return _CurrentEmployeeItem; }
            set
            {
                if (_CurrentEmployeeItem != value)
                {
                    _CurrentEmployeeItem = value;
                    NotifyPropertyChanged("CurrentEmployeeItem");
                }
            }
        }
        #endregion

        #region EmployeeItemDataLoading
        private bool _EmployeeItemDataLoading;

        public bool EmployeeItemDataLoading
        {
            get { return _EmployeeItemDataLoading; }
            set
            {
                if (_EmployeeItemDataLoading != value)
                {
                    _EmployeeItemDataLoading = value;
                    NotifyPropertyChanged("EmployeeProfessionDataLoading");
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

        public Employee_List()
        {
            // Get required services
            employeeService = DependencyResolver.Kernel.Get<IEmployeeService>();
            employeeCardService = DependencyResolver.Kernel.Get<IEmployeeCardService>();
            employeeDocumentService = DependencyResolver.Kernel.Get<IEmployeeDocumentService>();
            employeeNoteService = DependencyResolver.Kernel.Get<IEmployeeNoteService>();
            employeeProfessionService = DependencyResolver.Kernel.Get<IEmployeeProfessionService>();
            employeeLicenceService = DependencyResolver.Kernel.Get<IEmployeeLicenceService>();
            employeeItemService = DependencyResolver.Kernel.Get<IEmployeeItemService>();
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

        private void txtSearchByEmployeeCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            Thread th = new Thread(() => DisplayData());
            th.IsBackground = true;
            th.Start();
        }

        public void DisplayData()
        {
            EmployeeDataLoading = true;

            EmployeeListResponse response = new EmployeeSQLiteRepository()
                .GetEmployeesByPage(MainWindow.CurrentCompanyId, EmployeeSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                EmployeesFromDB = new ObservableCollection<EmployeeViewModel>(response.Employees ?? new List<EmployeeViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                EmployeesFromDB = new ObservableCollection<EmployeeViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            EmployeeDataLoading = false;
        }

        private void DisplayEmployeeNoteData()
        {
            EmployeeNoteDataLoading = true;

            EmployeeNoteListResponse response = new EmployeeNoteSQLiteRepository()
                .GetEmployeeNotesByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

            if (response.Success)
            {
                EmployeeNotesFromDB = new ObservableCollection<EmployeeNoteViewModel>(
                    response.EmployeeNotes ?? new List<EmployeeNoteViewModel>());
            }
            else
            {
                EmployeeNotesFromDB = new ObservableCollection<EmployeeNoteViewModel>();
            }

            EmployeeNoteDataLoading = false;
        }
        private void DisplayEmployeeDocumentData()
        {
            EmployeeDocumentDataLoading = true;

            EmployeeDocumentListResponse response = new EmployeeDocumentSQLiteRepository()
                .GetEmployeeDocumentsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

            if (response.Success)
            {
                EmployeeDocumentsFromDB = new ObservableCollection<EmployeeDocumentViewModel>(
                    response.EmployeeDocuments ?? new List<EmployeeDocumentViewModel>());
            }
            else
            {
                EmployeeDocumentsFromDB = new ObservableCollection<EmployeeDocumentViewModel>();
            }

            EmployeeDocumentDataLoading = false;
        }

        private void DisplayEmployeeCardData()
        {
            EmployeeCardDataLoading = true;

            EmployeeCardListResponse response = new EmployeeCardSQLiteRepository()
                .GetEmployeeCardsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

            if (response.Success)
            {
                EmployeeCardsFromDB = new ObservableCollection<EmployeeCardViewModel>(
                    response.EmployeeCards ?? new List<EmployeeCardViewModel>());
            }
            else
            {
                EmployeeCardsFromDB = new ObservableCollection<EmployeeCardViewModel>();
            }

            EmployeeCardDataLoading = false;
        }

        private void DisplayEmployeeProfessionData()
        {
            EmployeeProfessionDataLoading = true;

            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemSQLiteRepository()
                .GetEmployeeProfessionsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

            if (response.Success)
            {
                EmployeeProfessionsFromDB = new ObservableCollection<EmployeeProfessionItemViewModel>(
                    response.EmployeeProfessionItems ?? new List<EmployeeProfessionItemViewModel>());
            }
            else
            {
                EmployeeProfessionsFromDB = new ObservableCollection<EmployeeProfessionItemViewModel>();
            }

            EmployeeProfessionDataLoading = false;
        }

        private void DisplayEmployeeLicenceData()
        {
            EmployeeLicenceDataLoading = true;

            EmployeeLicenceItemListResponse response = new EmployeeLicenceItemSQLiteRepository()
                .GetEmployeeLicencesByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

            if (response.Success)
            {
                EmployeeLicencesFromDB = new ObservableCollection<EmployeeLicenceItemViewModel>(
                    response.EmployeeLicenceItems ?? new List<EmployeeLicenceItemViewModel>());
            }
            else
            {
                EmployeeLicencesFromDB = new ObservableCollection<EmployeeLicenceItemViewModel>();
            }

            EmployeeLicenceDataLoading = false;
        }

        private void DisplayEmployeeItemData()
        {
            EmployeeItemDataLoading = true;

            EmployeeItemListResponse response = new EmployeeItemSQLiteRepository()
                .GetEmployeeItemsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

            if (response.Success)
            {
                EmployeeItemsFromDB = new ObservableCollection<EmployeeItemViewModel>(
                    response.EmployeeItems ?? new List<EmployeeItemViewModel>());
            }
            else
            {
                EmployeeItemsFromDB = new ObservableCollection<EmployeeItemViewModel>();
            }

            EmployeeItemDataLoading = false;
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Radnici_TriTacke"));
            new EmployeeSQLiteRepository().Sync(employeeService, (synced, toSync) =>
            {
                SyncButtonContent = " Radnici (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new EmployeeNoteSQLiteRepository().Sync(employeeNoteService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new EmployeeDocumentSQLiteRepository().Sync(employeeDocumentService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new EmployeeCardSQLiteRepository().Sync(employeeCardService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new EmployeeProfessionItemSQLiteRepository().Sync(employeeProfessionService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new EmployeeLicenceItemSQLiteRepository().Sync(employeeLicenceService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = " Stavke ... ";
            new EmployeeItemSQLiteRepository().Sync(employeeItemService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });
            DisplayData();
            CurrentEmployee = null;
            EmployeeNotesFromDB = new ObservableCollection<EmployeeNoteViewModel>();
            EmployeeDocumentsFromDB = new ObservableCollection<EmployeeDocumentViewModel>();
            EmployeeCardsFromDB = new ObservableCollection<EmployeeCardViewModel>();
            EmployeeProfessionsFromDB = new ObservableCollection<EmployeeProfessionItemViewModel>();
            EmployeeLicencesFromDB = new ObservableCollection<EmployeeLicenceItemViewModel>();
            EmployeeItemsFromDB = new ObservableCollection<EmployeeItemViewModel>();
            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }
        private void SyncNoteData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new EmployeeNoteSQLiteRepository().Sync(employeeNoteService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeNoteData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }

        private void SyncDocumentData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new EmployeeDocumentSQLiteRepository().Sync(employeeDocumentService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeDocumentData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }

        private void SyncCardData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new EmployeeCardSQLiteRepository().Sync(employeeCardService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeCardData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }

        private void SyncProfessionData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new EmployeeProfessionItemSQLiteRepository().Sync(employeeProfessionService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeProfessionData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }

        private void SyncLicenceData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new EmployeeLicenceItemSQLiteRepository().Sync(employeeLicenceService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeLicenceData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }

        private void SyncItemData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = " Stavke ... ";
            new EmployeeItemSQLiteRepository().Sync(employeeItemService, (synced, toSync) =>
            {
                SyncButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeItemData();

            SyncButtonContent = " OSVEŽI ";
            SyncButtonEnabled = true;
        }
        private void dgEmployees_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgEmployeeCards_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgEmployeeNotes_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgEmployeeDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgEmployeeProfessions_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgEmployeeLicences_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void DgEmployeeItems_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Add, edit, delete, lock and cancel

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EmployeeViewModel Employee = new EmployeeViewModel();
            Employee.Identifier = Guid.NewGuid();
            
            Employee_List_AddEdit addEditForm = new Employee_List_AddEdit(Employee, true, false);
            addEditForm.EmployeeCreatedUpdated += new EmployeeHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_radnicima")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_izmenuUzvičnik"));
                return;
            }

            Employee_List_AddEdit addEditForm = new Employee_List_AddEdit(CurrentEmployee, false);
            addEditForm.EmployeeCreatedUpdated += new EmployeeHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_radnicima")), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_brisanjeUzvičnik"));
                return;
            }

            // Delete data
            var result = employeeService.Delete(CurrentEmployee.Identifier);
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

            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = "Morate odabrati radnika!";
                return;
            }

            #endregion

            Employee_Card_AddEdit EmployeeCardAddEditForm = new Employee_Card_AddEdit(CurrentEmployee);
            EmployeeCardAddEditForm.EmployeeCreatedUpdated += new EmployeeHandler(SyncCardData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Istorija_radnika")), 95, EmployeeCardAddEditForm);
        }

        private void BtnAddDocuments_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = "Morate odabrati radnika!";
                return;
            }

            #endregion

            Employee_Document_AddEdit EmployeeDocumentAddEditForm = new Employee_Document_AddEdit(CurrentEmployee);
            EmployeeDocumentAddEditForm.EmployeeCreatedUpdated += new EmployeeHandler(SyncDocumentData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Fajlovi_za_radnika")), 95, EmployeeDocumentAddEditForm);
        }

        private void BtnAddNotes_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = "Morate odabrati radnika!";
                return;
            }

            #endregion

            Employee_Note_AddEdit EmployeeNoteAddEditForm = new Employee_Note_AddEdit(CurrentEmployee);
            EmployeeNoteAddEditForm.EmployeeCreatedUpdated += new EmployeeHandler(SyncNoteData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Napomena")), 95, EmployeeNoteAddEditForm);
        }
        private void BtnAddProfessions_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = "Morate odabrati radnika!";
                return;
            }

            #endregion

            Employee_Profession_AddEdit EmployeeProfessionAddEditForm = new Employee_Profession_AddEdit(CurrentEmployee);
            EmployeeProfessionAddEditForm.EmployeeCreatedUpdated += new EmployeeHandler(SyncProfessionData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Zanimanja")), 95, EmployeeProfessionAddEditForm);
        }
        private void BtnAddLicences_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = "Morate odabrati radnika!";
                return;
            }

            #endregion

            Employee_Licence_AddEdit EmployeeLicenceAddEditForm = new Employee_Licence_AddEdit(CurrentEmployee);
            EmployeeLicenceAddEditForm.EmployeeCreatedUpdated += new EmployeeHandler(SyncLicenceData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Dozvole")), 95, EmployeeLicenceAddEditForm);
        }
        private void BtnAddItems_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = "Morate odabrati radnika!";
                return;
            }

            #endregion

            Employee_Item_AddEdit EmployeeItemAddEditForm = new Employee_Item_AddEdit(CurrentEmployee);
            EmployeeItemAddEditForm.EmployeeCreatedUpdated += new EmployeeHandler(SyncItemData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Članovi_porodice")), 95, EmployeeItemAddEditForm);
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
                EmployeesExcelReport.Show(EmployeesFromDB.ToList());
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
                Uri pdf = new Uri(CurrentEmployeeDocument.Path, UriKind.RelativeOrAbsolute);
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
        public void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var employeeResponse = new EmployeeSQLiteRepository().GetEmployeesByPage(MainWindow.CurrentCompanyId, EmployeeSearchObject, 1, Int32.MaxValue);
                EmployeesExcelReport.Show(employeeResponse.Employees);
            }
            catch(Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }

        private void btnEmployeeExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmployeeExcelReport.Show(CurrentEmployee);
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }

        private void btnExcelMS0129_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentEmployee == null)
                {
                    MainWindow.WarningMessage = "Morate odabrati radnika";
                    return;
                }
                MS0129Report.Show(CurrentEmployee);
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }
        
    }
}

