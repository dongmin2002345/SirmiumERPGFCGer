using Microsoft.Reporting.WinForms;
using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.RdlcReports.Employees;
using SirmiumERPGFC.Reports.Employees;
using SirmiumERPGFC.Repository.Employees;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using WpfAppCommonCode.Converters;
using iTextSharp.text.pdf;
using System.Reflection;
using SirmiumERPGFC.Repository.BusinessPartners;
using SirmiumERPGFC.Repository.ConstructionSites;

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
        IEmployeeAttachmentService employeeAttachmentService;
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
                            DisplayEmployeeAttachmentsData();

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


        #region EmployeeAttachmentsFromDB
        private ObservableCollection<EmployeeAttachmentViewModel> _EmployeeAttachmentsFromDB;

        public ObservableCollection<EmployeeAttachmentViewModel> EmployeeAttachmentsFromDB
        {
            get { return _EmployeeAttachmentsFromDB; }
            set
            {
                if (_EmployeeAttachmentsFromDB != value)
                {
                    _EmployeeAttachmentsFromDB = value;
                    NotifyPropertyChanged("EmployeeAttachmentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeAttachment
        private EmployeeAttachmentViewModel _CurrentEmployeeAttachment;

        public EmployeeAttachmentViewModel CurrentEmployeeAttachment
        {
            get { return _CurrentEmployeeAttachment; }
            set
            {
                if (_CurrentEmployeeAttachment != value)
                {
                    _CurrentEmployeeAttachment = value;
                    NotifyPropertyChanged("CurrentEmployeeAttachment");
                }
            }
        }
        #endregion

        #region EmployeeAttachmentDataLoading
        private bool _EmployeeAttachmentDataLoading;

        public bool EmployeeAttachmentDataLoading
        {
            get { return _EmployeeAttachmentDataLoading; }
            set
            {
                if (_EmployeeAttachmentDataLoading != value)
                {
                    _EmployeeAttachmentDataLoading = value;
                    NotifyPropertyChanged("EmployeeAttachmentDataLoading");
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
            employeeAttachmentService = DependencyResolver.Kernel.Get<IEmployeeAttachmentService>();
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

        private void DisplayEmployeeAttachmentsData()
        {
            EmployeeAttachmentDataLoading = true;

            EmployeeAttachmentListResponse response = new EmployeeAttachmentSQLiteRepository()
                .GetEmployeeAttachmentsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

            if (response.Success)
            {
                EmployeeAttachmentsFromDB = new ObservableCollection<EmployeeAttachmentViewModel>(
                    response.EmployeeAttachments ?? new List<EmployeeAttachmentViewModel>());

                if (EmployeeAttachmentsFromDB.Count == 0)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                EmployeeAttachmentsFromDB.Add(new EmployeeAttachmentViewModel()
                                {
                                    Identifier = Guid.NewGuid(),
                                    Code = "Prilog " + (i + 1).ToString(),
                                    Employee = CurrentEmployee,
                                    IsActive = false,
                                    IsSynced = false,
                                    Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                                    CreatedBy = new UserViewModel()
                                    {
                                        Id = MainWindow.CurrentUserId,
                                        FullName = MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName
                                    }
                                });
                            }
                        })
                    );
                }
            }
            else
            {
                EmployeeAttachmentsFromDB = new ObservableCollection<EmployeeAttachmentViewModel>();
            }
            EmployeeAttachmentDataLoading = false;
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Radnici_TriTacke"));
            new EmployeeSQLiteRepository().Sync(employeeService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("RADNICI")) + "(" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeNoteSQLiteRepository().Sync(employeeNoteService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeDocumentSQLiteRepository().Sync(employeeDocumentService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeCardSQLiteRepository().Sync(employeeCardService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeProfessionItemSQLiteRepository().Sync(employeeProfessionService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeLicenceItemSQLiteRepository().Sync(employeeLicenceService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeItemSQLiteRepository().Sync(employeeItemService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            SyncButtonContent = ((string)Application.Current.FindResource("RadnikPrilozi_TriTacke"));
            new EmployeeAttachmentSQLiteRepository().Sync(employeeAttachmentService, (synced, toSync) => {
                SyncButtonContent = ((string)Application.Current.FindResource("RadnikPrilozi")) + "(" + synced + " / " + toSync + ")... ";
            });
            DisplayData();
            CurrentEmployee = null;
            EmployeeNotesFromDB = new ObservableCollection<EmployeeNoteViewModel>();
            EmployeeDocumentsFromDB = new ObservableCollection<EmployeeDocumentViewModel>();
            EmployeeCardsFromDB = new ObservableCollection<EmployeeCardViewModel>();
            EmployeeProfessionsFromDB = new ObservableCollection<EmployeeProfessionItemViewModel>();
            EmployeeLicencesFromDB = new ObservableCollection<EmployeeLicenceItemViewModel>();
            EmployeeItemsFromDB = new ObservableCollection<EmployeeItemViewModel>();
            EmployeeAttachmentsFromDB = new ObservableCollection<EmployeeAttachmentViewModel>();
            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }
        private void SyncNoteData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeNoteSQLiteRepository().Sync(employeeNoteService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeNoteData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncDocumentData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeDocumentSQLiteRepository().Sync(employeeDocumentService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeDocumentData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncCardData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeCardSQLiteRepository().Sync(employeeCardService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeCardData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncProfessionData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeProfessionItemSQLiteRepository().Sync(employeeProfessionService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeProfessionData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncLicenceData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeLicenceItemSQLiteRepository().Sync(employeeLicenceService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeLicenceData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncItemData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Stavke_TriTacke"));
            new EmployeeItemSQLiteRepository().Sync(employeeItemService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("Stavke")) + "(" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeItemData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        private void SyncEmployeeAttachmentsData()
        {
            SyncButtonEnabled = false;
            SyncButtonContent = ((string)Application.Current.FindResource("RadnikPrilozi_TriTacke"));
            new EmployeeAttachmentSQLiteRepository().Sync(employeeAttachmentService, (synced, toSync) =>
            {
                SyncButtonContent = ((string)Application.Current.FindResource("RadnikPrilozi")) + "(" + synced + " / " + toSync + ")... ";
            });

            DisplayEmployeeAttachmentsData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
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

        private void DgEmployeeAttachments_LoadingRow(object sender, DataGridRowEventArgs e)
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
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_radnikaUzvičnik"));
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
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_radnikaUzvičnik"));
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
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_radnikaUzvičnik"));
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
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_radnikaUzvičnik"));
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
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_radnikaUzvičnik"));
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
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_radnikaUzvičnik"));
                return;
            }

            #endregion

            Employee_Item_AddEdit EmployeeItemAddEditForm = new Employee_Item_AddEdit(CurrentEmployee);
            EmployeeItemAddEditForm.EmployeeCreatedUpdated += new EmployeeHandler(SyncItemData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Članovi_porodice")), 95, EmployeeItemAddEditForm);
        }
        private void btnSaveAttachments_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_radnikaUzvičnik"));
                return;
            }

            #endregion

            SaveChangesOnEmployeeAttachments();
        }

        void SaveChangesOnEmployeeAttachments()
        {
            Thread td = new Thread(() => {
                var attachments = EmployeeAttachmentsFromDB.ToList();

                foreach(var attachment in attachments)
                {
                    attachment.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                    attachment.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                    new EmployeeAttachmentSQLiteRepository().Delete(attachment.Identifier);
                    var localResponse = new EmployeeAttachmentSQLiteRepository().Create(attachment);
                    if (!localResponse.Success)
                    {
                        MainWindow.ErrorMessage = localResponse.Message;
                        return;
                    }
                }

                var response = employeeAttachmentService.CreateList(attachments);

                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;
                }
                else
                {
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    SyncEmployeeAttachmentsData();
                }
            });
            td.IsBackground = true;
            td.Start();
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
            //try
            //{
            //    var employeeResponse = new EmployeeSQLiteRepository().GetEmployeesByPage(MainWindow.CurrentCompanyId, EmployeeSearchObject, 1, Int32.MaxValue);
            //    EmployeesExcelReport.Show(employeeResponse.Employees);
            //}
            //catch(Exception ex)
            //{
            //    MainWindow.ErrorMessage = ex.Message;
            //}
            Employee_ReportWindow reportWindow = new Employee_ReportWindow(CurrentEmployee);
            reportWindow.Show();
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

        private void BtnPrintEmployeeReport_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_racunUzvičnik"));
                return;
            }

            #endregion

            rdlcEmployeeReport.LocalReport.DataSources.Clear();

            List<EmployeeReportViewModel> employee = new List<EmployeeReportViewModel>()
            {
                new EmployeeReportViewModel()
                {

                    EmployeeCode = CurrentEmployee?.EmployeeCode ?? "",
                    Name = CurrentEmployee?.Name ?? "",
                    SurName = CurrentEmployee?.SurName ?? "",
                    DateOfBirth = CurrentEmployee?.DateOfBirth?.ToString("dd.MM.yyyy") ?? "",
                    Gender = CurrentEmployee?.Gender.ToString() ?? "",
                    CountryName = CurrentEmployee?.Country?.Name ?? "",
                    RegionName = CurrentEmployee?.Region?.Name ?? "",
                    MunicipalityName = CurrentEmployee?.Municipality?.Name ?? "",
                    CityName = CurrentEmployee?.City?.Name ?? "",
                    Address = CurrentEmployee?.Address ?? "",
                    ConstructionSiteCode = CurrentEmployee?.ConstructionSiteCode ?? "",
                    //podaci o pasošu
                    PassportCountryName = CurrentEmployee?.PassportCountry?.Name ?? "",
                    PassportCityName = CurrentEmployee?.PassportCity?.Name ?? "",
                    Passport = CurrentEmployee?.Passport ?? "",
                    VisaFrom = CurrentEmployee?.VisaFrom?.ToString("dd.MM.yyyy") ?? "",
                    VisaTo = CurrentEmployee?.VisaTo?.ToString("dd.MM.yyyy") ?? "",
                    //Podaci o prebivalištu
                    ResidenceCountryName = CurrentEmployee?.ResidenceCountry?.Name ?? "",
                    ResidenceCityName = CurrentEmployee?.ResidenceCity?.Name ?? "",
                    ResidenceAddress = CurrentEmployee?.ResidenceAddress ?? "",
                    //Podaci o radnoj dozvoli
                    EmbassyDate = CurrentEmployee?.EmbassyDate?.ToString("dd.MM.yyyy") ?? "",
                    VisaDate = CurrentEmployee?.VisaDate?.ToString("dd.MM.yyyy") ?? "",
                    VisaValidFrom = CurrentEmployee?.VisaValidFrom?.ToString("dd.MM.yyyy") ?? "",
                    VisaValidTo = CurrentEmployee?.VisaValidTo?.ToString("dd.MM.yyyy") ?? "",
                    WorkPermitFrom = CurrentEmployee?.WorkPermitFrom?.ToString("dd.MM.yyyy") ?? "",
                    WorkPermitTo = CurrentEmployee?.WorkPermitTo?.ToString("dd.MM.yyyy") ?? ""
                }
            };
            var rpdsModel = new ReportDataSource()
            {
                Name = "DataSet1",
                Value = employee
            };
            rdlcEmployeeReport.LocalReport.DataSources.Add(rpdsModel);

            List<EmployeeReportViewModel> employeeProfession = new List<EmployeeReportViewModel>();
            List<EmployeeProfessionItemViewModel> ProfessionItems = new EmployeeProfessionItemSQLiteRepository().GetEmployeeProfessionsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier).EmployeeProfessionItems;
            int counter = 1;
            foreach (var ProfessionItem in ProfessionItems)
            {
                employeeProfession.Add(new EmployeeReportViewModel()
                {
                    OrderNumberProfession = counter++,
                    ProfessionCountryName = ProfessionItem?.Country?.Name ?? "",
                    ProfessionName = ProfessionItem?.Profession?.Name ?? "",
                 
                });
            }
            var rpdsModelProfession = new ReportDataSource()
            {
                Name = "DataSet2",
                Value = employeeProfession
            };
            rdlcEmployeeReport.LocalReport.DataSources.Add(rpdsModelProfession);

            List<EmployeeReportViewModel> employeeLicence = new List<EmployeeReportViewModel>();
            List<EmployeeLicenceItemViewModel> LicenceItems = new EmployeeLicenceItemSQLiteRepository().GetEmployeeLicencesByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier).EmployeeLicenceItems;
            int Licencecounter = 1;
            foreach (var LicenceItem in LicenceItems)
            {
                employeeLicence.Add(new EmployeeReportViewModel()
                {
                    OrderNumberLicence = Licencecounter++,
                    LicenceCountryName = LicenceItem?.Country?.Name ?? "",
                    LicenceCode = LicenceItem?.Licence?.Code ?? "",
                    LicenceValidFrom = LicenceItem?.ValidFrom?.ToString("dd.MM.yyyy") ?? "",
                    LicenceValidTo = LicenceItem?.ValidTo?.ToString("dd.MM.yyyy") ?? ""

                });
            }
            var rpdsModelLicence = new ReportDataSource()
            {
                Name = "DataSet5",
                Value = employeeLicence
            };
            rdlcEmployeeReport.LocalReport.DataSources.Add(rpdsModelLicence);

            List<EmployeeReportViewModel> employeeNote = new List<EmployeeReportViewModel>();
            List<EmployeeNoteViewModel> NoteItems = new EmployeeNoteSQLiteRepository().GetEmployeeNotesByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier).EmployeeNotes;
            int Notecounter = 1;
            foreach (var NoteItem in NoteItems)
            {
                employeeNote.Add(new EmployeeReportViewModel()
                {
                    OrderNumberNote = Notecounter++,
                    Note = NoteItem?.Note ?? "",
                    NoteDAte = NoteItem?.NoteDate.ToString("dd.MM.yyyy") ?? ""

                });
            }
            var rpdsModelNote = new ReportDataSource()
            {
                Name = "DataSet3",
                Value = employeeNote
            };
            rdlcEmployeeReport.LocalReport.DataSources.Add(rpdsModelNote);

            List<EmployeeReportViewModel> employeeCard = new List<EmployeeReportViewModel>();
            List<EmployeeCardViewModel> CardItems = new EmployeeCardSQLiteRepository().GetEmployeeCardsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier).EmployeeCards;
            int Cardcounter = 1;
            foreach (var CardItem in CardItems)
            {
                employeeCard.Add(new EmployeeReportViewModel()
                {
                    OrderNumberCard = Cardcounter++,
                    CardDescription = CardItem?.Description ?? "",
                    CardDate = CardItem?.CardDate?.ToString("dd.MM.yyyy") ?? ""

                });
            }
            var rpdsModelCard = new ReportDataSource()
            {
                Name = "DataSet4",
                Value = employeeCard
            };
            rdlcEmployeeReport.LocalReport.DataSources.Add(rpdsModelCard);

            List<EmployeeReportViewModel> employeeItem = new List<EmployeeReportViewModel>();
            List<EmployeeItemViewModel> Items = new EmployeeItemSQLiteRepository().GetEmployeeItemsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier).EmployeeItems;
            int Itemcounter = 1;
            foreach (var Item in Items)
            {
                employeeItem.Add(new EmployeeReportViewModel()
                {
                    OrderNumberFamilyMember = Itemcounter++,
                    FamilyMemberName = Item?.FamilyMember?.Name ?? "",
                    ItemName = Item?.Name ?? "",
                    ItemsDateOfBirth = Item?.DateOfBirth?.ToString("dd.MM.yyyy") ?? "",
                    ItemPassport = Item?.Passport ?? "",
                    ItemEmbassyDate = Item?.EmbassyDate?.ToString("dd.MM.yyyy") ?? ""
                });
            }
            var rpdsModelItem = new ReportDataSource()
            {
                Name = "DataSet6",
                Value = employeeItem
            };
            rdlcEmployeeReport.LocalReport.DataSources.Add(rpdsModelItem);


            
          
            //List<ReportParameter> reportParams = new List<ReportParameter>();
            //string parameterText = "Dana " + (CurrentInputInvoice?.InvoiceDate.ToString("dd.MM.yyyy") ?? "") + " na stočni depo klanice Bioesen primljeno je:";
            //reportParams.Add(new ReportParameter("txtInputInvoiceDate", parameterText));


            //var businessPartnerList = new List<InvoiceBusinessPartnerViewModel>();
            //businessPartnerList.Add(new InvoiceBusinessPartnerViewModel() { Name = "Pera peric " });
            //var businessPartnerModel = new ReportDataSource() { Name = "DataSet2", Value = businessPartnerList };
            //rdlcInputNoteReport.LocalReport.DataSources.Add(businessPartnerModel);

            string exeFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string ContentStart = System.IO.Path.Combine(exeFolder, @"RdlcReports\Employees\EmployeeReport.rdlc");
            rdlcEmployeeReport.LocalReport.ReportPath = ContentStart;
            // rdlcInputInvoiceReport.LocalReport.SetParameters(reportParams);
            rdlcEmployeeReport.SetDisplayMode(DisplayMode.PrintLayout);
            rdlcEmployeeReport.Refresh();
            rdlcEmployeeReport.ZoomMode = ZoomMode.Percent;
            rdlcEmployeeReport.ZoomPercent = 100;
            rdlcEmployeeReport.RefreshReport();
        }

        private void btnPrintEmployeePdf_Click(object sender, RoutedEventArgs e)
        {
            #region Validation 
            
            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = "No Current employee!";
                return;
            }

            #endregion

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SirmiumERPGFC.Resources.WAK NEU.pdf";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                using (var memstream = new MemoryStream())
                {
                    reader.BaseStream.CopyTo(memstream);
                    PdfReader pdfReader = new PdfReader(memstream.ToArray());

                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.FileName = "WAK NEU " + CurrentEmployee.Name + " " + CurrentEmployee.SurName; // Default file name
                    dlg.DefaultExt = ".pdf"; // Default file extension
                    dlg.Filter = "Text documents (.pdf)|*.pdf"; // Filter files by extension

                    // Show save file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result == true)
                    {
                        // Save document
                        string filename = dlg.FileName;

                        PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(filename, FileMode.Create));
                        AcroFields pdfFormFields = pdfStamper.AcroFields;
                        // set form pdfFormFields  
                        // The first worksheet and W-4 form  
                        pdfFormFields.SetField("1 Name", CurrentEmployee.Name);
                        pdfFormFields.SetField("3 Vorname", CurrentEmployee.SurName);
                        pdfFormFields.SetField("5 Geburtsdatum", CurrentEmployee.DateOfBirth?.ToString("dd.MM.yyyy") ?? "");

                        // Adresa firme u Nemackoj
                        var employeeBusinessPartner = new EmployeeByBusinessPartnerSQLiteRepository().GetByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier).EmployeeByBusinessPartners?.OrderByDescending(x => x.CreatedAt)?.FirstOrDefault()?.BusinessPartner;
                        var businessPartnerAddress = new BusinessPartnerLocationSQLiteRepository().GetBusinessPartnerLocationsByBusinessPartner(MainWindow.CurrentCompanyId, employeeBusinessPartner?.Identifier ?? Guid.Empty).BusinessPartnerLocations?.OrderBy(x => x.CreatedAt).FirstOrDefault();
                        pdfFormFields.SetField("7 Name und Anschrift des entsendenden Unternehmens bzw der Niederlassung im Bundesgebiet", businessPartnerAddress?.Address ?? "");
                        
                        pdfFormFields.SetField("9 PassNr oder PassersatzN", CurrentEmployee.Passport);
                        pdfFormFields.SetField("10 ausgestellt am", CurrentEmployee.VisaFrom?.ToString("dd.MM.yyyy") ?? "");
                        pdfFormFields.SetField("11 von BehördeStaat", CurrentEmployee.PassportMup);

                        pdfFormFields.SetField("13 von bis", CurrentEmployee.WorkPermitFrom?.ToString("dd.MM.yyyy") ?? "");
                        pdfFormFields.SetField("Text1", CurrentEmployee.WorkPermitTo?.ToString("dd.MM.yyyy") ?? "");


                        // Gradiliste
                        var employeeConstructionSite = new EmployeeByConstructionSiteSQLiteRepository().GetByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier).EmployeeByConstructionSites?.OrderByDescending(x => x.CreatedAt)?.FirstOrDefault()?.ConstructionSite;
                        employeeConstructionSite = new ConstructionSiteSQLiteRepository().GetConstructionSite(employeeConstructionSite?.Identifier ?? Guid.NewGuid())?.ConstructionSite;
                        pdfFormFields.SetField("15 im Rahmen des Werkvertrages vom", employeeConstructionSite.ContractStart.ToString("dd.MM.yyyy") ?? "");

                        pdfFormFields.SetField("Text8.0.0", employeeConstructionSite.InternalCode?.Length > 0 ? employeeConstructionSite.InternalCode.ToArray()[0].ToString() : "");
                        pdfFormFields.SetField("Text8.0.1", employeeConstructionSite.InternalCode?.Length > 1 ? employeeConstructionSite.InternalCode.ToArray()[1].ToString() : "");
                        pdfFormFields.SetField("Text8.0.2", employeeConstructionSite.InternalCode?.Length > 2 ? employeeConstructionSite.InternalCode.ToArray()[2].ToString() : "");
                        pdfFormFields.SetField("Text8.0.3", employeeConstructionSite.InternalCode?.Length > 3 ? employeeConstructionSite.InternalCode.ToArray()[3].ToString() : "");
                        pdfFormFields.SetField("Text8.0.4", employeeConstructionSite.InternalCode?.Length > 4 ? employeeConstructionSite.InternalCode.ToArray()[4].ToString() : "");
                        pdfFormFields.SetField("Text8.0.5", employeeConstructionSite.InternalCode?.Length > 5 ? employeeConstructionSite.InternalCode.ToArray()[5].ToString() : "");
                        pdfFormFields.SetField("Text8.0.6", employeeConstructionSite.InternalCode?.Length > 6 ? employeeConstructionSite.InternalCode.ToArray()[6].ToString() : "");
                        pdfFormFields.SetField("Text8.0.7", employeeConstructionSite.InternalCode?.Length > 7 ? employeeConstructionSite.InternalCode.ToArray()[7].ToString() : "");
                        pdfFormFields.SetField("Text8.0.8", employeeConstructionSite.InternalCode?.Length > 8 ? employeeConstructionSite.InternalCode.ToArray()[8].ToString() : "");
                        pdfFormFields.SetField("Text8.0.9", employeeConstructionSite.InternalCode?.Length > 9 ? employeeConstructionSite.InternalCode.ToArray()[9].ToString() : "");
                        pdfFormFields.SetField("Text8.0.10", employeeConstructionSite.InternalCode?.Length > 10 ? employeeConstructionSite.InternalCode.ToArray()[10].ToString() : "");
                        pdfFormFields.SetField("Text8.0.11", employeeConstructionSite.InternalCode?.Length > 11 ? employeeConstructionSite.InternalCode.ToArray()[11].ToString() : "");

                        pdfFormFields.SetField("17 Auftragnehmer ausländisches Unternehmen", businessPartnerAddress?.Address ?? "");
                        pdfFormFields.SetField("18 Auftraggeber", businessPartnerAddress?.Address ?? "");
                        pdfFormFields.SetField("19 BetriebsstätteBaustelle Anschrift Straße Nr PLZ Ort", employeeConstructionSite.Name);

                        //pdfFormFields.SetField("f1_03(0)", "1");
                        //pdfFormFields.SetField("f1_04(0)", "8");
                        //pdfFormFields.SetField("f1_05(0)", "0");
                        //pdfFormFields.SetField("f1_06(0)", "1");
                        //pdfFormFields.SetField("f1_07(0)", "16");
                        //pdfFormFields.SetField("f1_08(0)", "28");
                        //pdfFormFields.SetField("f1_09(0)", "Franklin A.");
                        //pdfFormFields.SetField("f1_10(0)", "Benefield");
                        //pdfFormFields.SetField("f1_11(0)", "532");
                        //pdfFormFields.SetField("f1_12(0)", "12");
                        //pdfFormFields.SetField("f1_13(0)", "1234");
                        //// The form's checkboxes  
                        //pdfFormFields.SetField("c1_01(0)", "0");
                        //pdfFormFields.SetField("c1_02(0)", "Yes");
                        //pdfFormFields.SetField("c1_03(0)", "0");
                        //pdfFormFields.SetField("c1_04(0)", "Yes");
                        //// The rest of the form pdfFormFields  
                        //pdfFormFields.SetField("f1_14(0)", "100 North Cujo Street");
                        //pdfFormFields.SetField("f1_15(0)", "Nome, AK  67201");
                        //pdfFormFields.SetField("f1_16(0)", "9");
                        //pdfFormFields.SetField("f1_17(0)", "10");
                        //pdfFormFields.SetField("f1_18(0)", "11");
                        //pdfFormFields.SetField("f1_19(0)", "Walmart, Nome, AK");
                        //pdfFormFields.SetField("f1_20(0)", "WAL666");
                        //pdfFormFields.SetField("f1_21(0)", "AB");
                        //pdfFormFields.SetField("f1_22(0)", "4321");
                        //// Second Worksheets pdfFormFields  
                        //// In order to map the fields, I just pass them a sequential  
                        //// number to mark them; once I know which field is which, I  
                        //// can pass the appropriate value  
                        //pdfFormFields.SetField("f2_01(0)", "1");
                        //pdfFormFields.SetField("f2_02(0)", "2");
                        //pdfFormFields.SetField("f2_03(0)", "3");
                        //pdfFormFields.SetField("f2_04(0)", "4");
                        //pdfFormFields.SetField("f2_05(0)", "5");
                        //pdfFormFields.SetField("f2_06(0)", "6");
                        //pdfFormFields.SetField("f2_07(0)", "7");
                        //pdfFormFields.SetField("f2_08(0)", "8");
                        //pdfFormFields.SetField("f2_09(0)", "9");
                        //pdfFormFields.SetField("f2_10(0)", "10");
                        //pdfFormFields.SetField("f2_11(0)", "11");
                        //pdfFormFields.SetField("f2_12(0)", "12");
                        //pdfFormFields.SetField("f2_13(0)", "13");
                        //pdfFormFields.SetField("f2_14(0)", "14");
                        //pdfFormFields.SetField("f2_15(0)", "15");
                        //pdfFormFields.SetField("f2_16(0)", "16");
                        //pdfFormFields.SetField("f2_17(0)", "17");
                        //pdfFormFields.SetField("f2_18(0)", "18");
                        //pdfFormFields.SetField("f2_19(0)", "19");
                        // report by reading values from completed PDF  
                        //string sTmp = "W-4 Completed for " + pdfFormFields.GetField("f1_09(0)") + " " + pdfFormFields.GetField("f1_10(0)");
                        //MessageBox.Show(sTmp, "Finished");
                        // flatten the form to remove editting options, set it to false  
                        // to leave the form open to subsequent manual edits  
                        pdfStamper.FormFlattening = false;
                        // close the pdf  
                        pdfStamper.Close();

                    }
                }
            }
        }
    }
}

