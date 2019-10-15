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
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppCommonCode.Converters;

namespace SirmiumERPGFC.Views.Employees
{
    public partial class Employee_List_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IEmployeeService employeeService;
       
        #endregion

        #region Events
        public event EmployeeHandler EmployeeCreatedUpdated;
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
                }
            }
        }
        #endregion

        #region EmployeeDataLoading
        private bool _EmployeeDataLoading;

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

        #region SubmitButtonContent
        private string _SubmitButtonContent = " PROKNJIŽI ";

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


        #region IsInPdvOptions
        public ObservableCollection<String> IsInPdvOptions
        {
            get { return new ObservableCollection<String>(new List<string>() { (string)Application.Current.FindResource("DA"), (string)Application.Current.FindResource("NE") }); }
        }
        #endregion


        #region ItemsEnabled
        private bool _ItemsEnabled;

        public bool ItemsEnabled
        {
            get { return _ItemsEnabled; }
            set
            {
                if (_ItemsEnabled != value)
                {
                    _ItemsEnabled = value;
                    NotifyPropertyChanged("ItemsEnabled");
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

        #endregion

        #region Constructor

        public Employee_List_AddEdit(EmployeeViewModel employeeViewModel, bool itemsEnabled, bool isPopup = false)
        {
            employeeService = DependencyResolver.Kernel.Get<IEmployeeService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentEmployee = employeeViewModel;
            ItemsEnabled = itemsEnabled;
            IsPopup = isPopup;



        }
        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployee?.EmployeeCode == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Sifra";
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = " Čuvanje u toku... ";
                SubmitButtonEnabled = false;

                CurrentEmployee.IsSynced = false;
                CurrentEmployee.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentEmployee.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                EmployeeResponse response = new EmployeeSQLiteRepository().Create(CurrentEmployee);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod čuvanja podataka!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;
                }

                response = employeeService.Create(CurrentEmployee);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu. Greška kod čuvanja na serveru!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;

                    new EmployeeSQLiteRepository().Sync(employeeService);

                    EmployeeCreatedUpdated();

                    Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            FlyoutHelper.CloseFlyout(this);
                        })
                    );
                }
            });
            td.IsBackground = true;
            td.Start();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
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

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "") //[CallerMemberName]
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion

        //#region Attributes

        //#region Services
        //IEmployeeService EmployeeService;
        //#endregion

        //#region Event
        //public event EmployeeHandler EmployeeCreated;
        //#endregion


        //#region CurrentEmployee
        //private EmployeeViewModel _CurrentEmployee = new EmployeeViewModel();

        //public EmployeeViewModel CurrentEmployee
        //{
        //    get { return _CurrentEmployee; }
        //    set
        //    {
        //        if (_CurrentEmployee != value)
        //        {
        //            _CurrentEmployee = value;
        //            NotifyPropertyChanged("CurrentEmployee");
        //        }
        //    }
        //}
        //#endregion


        //#region EmployeeItemsFromDB
        //private ObservableCollection<EmployeeItemViewModel> _EmployeeItemsFromDB;

        //public ObservableCollection<EmployeeItemViewModel> EmployeeItemsFromDB
        //{
        //    get { return _EmployeeItemsFromDB; }
        //    set
        //    {
        //        if (_EmployeeItemsFromDB != value)
        //        {
        //            _EmployeeItemsFromDB = value;
        //            NotifyPropertyChanged("EmployeeItemsFromDB");
        //        }
        //    }
        //}
        //#endregion

        //#region CurrentEmployeeItemForm
        //private EmployeeItemViewModel _CurrentEmployeeItemForm = new EmployeeItemViewModel();

        //public EmployeeItemViewModel CurrentEmployeeItemForm
        //{
        //    get { return _CurrentEmployeeItemForm; }
        //    set
        //    {
        //        if (_CurrentEmployeeItemForm != value)
        //        {
        //            _CurrentEmployeeItemForm = value;
        //            NotifyPropertyChanged("CurrentEmployeeItemForm");
        //        }
        //    }
        //}
        //#endregion

        //#region CurrentEmployeeItemDG
        //private EmployeeItemViewModel _CurrentEmployeeItemDG;

        //public EmployeeItemViewModel CurrentEmployeeItemDG
        //{
        //    get { return _CurrentEmployeeItemDG; }
        //    set
        //    {
        //        if (_CurrentEmployeeItemDG != value)
        //        {
        //            _CurrentEmployeeItemDG = value;
        //            NotifyPropertyChanged("CurrentEmployeeItemDG");
        //        }
        //    }
        //}
        //#endregion

        //#region EmployeeItemDataLoading
        //private bool _EmployeeItemDataLoading;

        //public bool EmployeeItemDataLoading
        //{
        //    get { return _EmployeeItemDataLoading; }
        //    set
        //    {
        //        if (_EmployeeItemDataLoading != value)
        //        {
        //            _EmployeeItemDataLoading = value;
        //            NotifyPropertyChanged("EmployeeItemDataLoading");
        //        }
        //    }
        //}
        //#endregion


        //#region EmployeeProfessionItemsFromDB
        //private ObservableCollection<EmployeeProfessionItemViewModel> _EmployeeProfessionItemsFromDB;

        //public ObservableCollection<EmployeeProfessionItemViewModel> EmployeeProfessionItemsFromDB
        //{
        //    get { return _EmployeeProfessionItemsFromDB; }
        //    set
        //    {
        //        if (_EmployeeProfessionItemsFromDB != value)
        //        {
        //            _EmployeeProfessionItemsFromDB = value;
        //            NotifyPropertyChanged("EmployeeProfessionItemsFromDB");
        //        }
        //    }
        //}
        //#endregion

        //#region CurrentEmployeeProfessionItemForm
        //private EmployeeProfessionItemViewModel _CurrentEmployeeProfessionItemForm;

        //public EmployeeProfessionItemViewModel CurrentEmployeeProfessionItemForm
        //{
        //    get { return _CurrentEmployeeProfessionItemForm; }
        //    set
        //    {
        //        if (_CurrentEmployeeProfessionItemForm != value)
        //        {
        //            _CurrentEmployeeProfessionItemForm = value;
        //            NotifyPropertyChanged("CurrentEmployeeProfessionItemForm");
        //        }
        //    }
        //}
        //#endregion

        //#region CurrentProfessionDG
        //private EmployeeProfessionItemViewModel _CurrentProfessionDG;

        //public EmployeeProfessionItemViewModel CurrentProfessionDG
        //{
        //    get { return _CurrentProfessionDG; }
        //    set
        //    {
        //        if (_CurrentProfessionDG != value)
        //        {
        //            _CurrentProfessionDG = value;
        //            NotifyPropertyChanged("CurrentProfessionDG");
        //        }
        //    }
        //}
        //#endregion

        //#region LoadingProfessionItems
        //private bool _LoadingProfessionItems;

        //public bool LoadingProfessionItems
        //{
        //    get { return _LoadingProfessionItems; }
        //    set
        //    {
        //        if (_LoadingProfessionItems != value)
        //        {
        //            _LoadingProfessionItems = value;
        //            NotifyPropertyChanged("LoadingProfessionItems");
        //        }
        //    }
        //}
        //#endregion


        //#region EmployeeLicenceItemsFromDB
        //private ObservableCollection<EmployeeLicenceItemViewModel> _EmployeeLicenceItemsFromDB;

        //public ObservableCollection<EmployeeLicenceItemViewModel> EmployeeLicenceItemsFromDB
        //{
        //    get { return _EmployeeLicenceItemsFromDB; }
        //    set
        //    {
        //        if (_EmployeeLicenceItemsFromDB != value)
        //        {
        //            _EmployeeLicenceItemsFromDB = value;
        //            NotifyPropertyChanged("EmployeeLicenceItemsFromDB");
        //        }
        //    }
        //}
        //#endregion

        //#region CurrentEmployeeLicenceItemForm
        //private EmployeeLicenceItemViewModel _CurrentEmployeeLicenceItemForm;

        //public EmployeeLicenceItemViewModel CurrentEmployeeLicenceItemForm
        //{
        //    get { return _CurrentEmployeeLicenceItemForm; }
        //    set
        //    {
        //        if (_CurrentEmployeeLicenceItemForm != value)
        //        {
        //            _CurrentEmployeeLicenceItemForm = value;
        //            NotifyPropertyChanged("CurrentEmployeeLicenceItemForm");
        //        }
        //    }
        //}
        //#endregion

        //#region CurrentLicenceDG
        //private EmployeeLicenceItemViewModel _CurrentLicenceDG;

        //public EmployeeLicenceItemViewModel CurrentLicenceDG
        //{
        //    get { return _CurrentLicenceDG; }
        //    set
        //    {
        //        if (_CurrentLicenceDG != value)
        //        {
        //            _CurrentLicenceDG = value;
        //            NotifyPropertyChanged("CurrentLicenceDG");
        //        }
        //    }
        //}
        //#endregion

        //#region LoadingLicenceItems
        //private bool _LoadingLicenceItems;

        //public bool LoadingLicenceItems
        //{
        //    get { return _LoadingLicenceItems; }
        //    set
        //    {
        //        if (_LoadingLicenceItems != value)
        //        {
        //            _LoadingLicenceItems = value;
        //            NotifyPropertyChanged("LoadingLicenceItems");
        //        }
        //    }
        //}
        //#endregion


        //#region EmployeeDocumentsFromDB
        //private ObservableCollection<EmployeeDocumentViewModel> _EmployeeDocumentsFromDB;

        //public ObservableCollection<EmployeeDocumentViewModel> EmployeeDocumentsFromDB
        //{
        //    get { return _EmployeeDocumentsFromDB; }
        //    set
        //    {
        //        if (_EmployeeDocumentsFromDB != value)
        //        {
        //            _EmployeeDocumentsFromDB = value;
        //            NotifyPropertyChanged("EmployeeDocumentsFromDB");
        //        }
        //    }
        //}
        //#endregion

        //#region CurrentEmployeeDocumentForm
        //private EmployeeDocumentViewModel _CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel() { CreateDate = DateTime.Now };

        //public EmployeeDocumentViewModel CurrentEmployeeDocumentForm
        //{
        //    get { return _CurrentEmployeeDocumentForm; }
        //    set
        //    {
        //        if (_CurrentEmployeeDocumentForm != value)
        //        {
        //            _CurrentEmployeeDocumentForm = value;
        //            NotifyPropertyChanged("CurrentEmployeeDocumentForm");
        //        }
        //    }
        //}
        //#endregion

        //#region CurrentEmployeeDocumentDG
        //private EmployeeDocumentViewModel _CurrentEmployeeDocumentDG;

        //public EmployeeDocumentViewModel CurrentEmployeeDocumentDG
        //{
        //    get { return _CurrentEmployeeDocumentDG; }
        //    set
        //    {
        //        if (_CurrentEmployeeDocumentDG != value)
        //        {
        //            _CurrentEmployeeDocumentDG = value;
        //            NotifyPropertyChanged("CurrentEmployeeDocumentDG");
        //        }
        //    }
        //}
        //#endregion

        //#region EmployeeDocumentDataLoading
        //private bool _EmployeeDocumentDataLoading;

        //public bool EmployeeDocumentDataLoading
        //{
        //    get { return _EmployeeDocumentDataLoading; }
        //    set
        //    {
        //        if (_EmployeeDocumentDataLoading != value)
        //        {
        //            _EmployeeDocumentDataLoading = value;
        //            NotifyPropertyChanged("EmployeeDocumentDataLoading");
        //        }
        //    }
        //}
        //#endregion


        //#region EmployeeNotesFromDB
        //private ObservableCollection<EmployeeNoteViewModel> _EmployeeNotesFromDB;

        //public ObservableCollection<EmployeeNoteViewModel> EmployeeNotesFromDB
        //{
        //    get { return _EmployeeNotesFromDB; }
        //    set
        //    {
        //        if (_EmployeeNotesFromDB != value)
        //        {
        //            _EmployeeNotesFromDB = value;
        //            NotifyPropertyChanged("EmployeeNotesFromDB");
        //        }
        //    }
        //}
        //#endregion

        //#region CurrentEmployeeNoteForm
        //private EmployeeNoteViewModel _CurrentEmployeeNoteForm = new EmployeeNoteViewModel() { NoteDate = DateTime.Now };

        //public EmployeeNoteViewModel CurrentEmployeeNoteForm
        //{
        //    get { return _CurrentEmployeeNoteForm; }
        //    set
        //    {
        //        if (_CurrentEmployeeNoteForm != value)
        //        {
        //            _CurrentEmployeeNoteForm = value;
        //            NotifyPropertyChanged("CurrentEmployeeNoteForm");
        //        }
        //    }
        //}
        //#endregion

        //#region CurrentEmployeeNoteDG
        //private EmployeeNoteViewModel _CurrentEmployeeNoteDG;

        //public EmployeeNoteViewModel CurrentEmployeeNoteDG
        //{
        //    get { return _CurrentEmployeeNoteDG; }
        //    set
        //    {
        //        if (_CurrentEmployeeNoteDG != value)
        //        {
        //            _CurrentEmployeeNoteDG = value;
        //            NotifyPropertyChanged("CurrentEmployeeNoteDG");
        //        }
        //    }
        //}
        //#endregion

        //#region EmployeeNoteDataLoading
        //private bool _EmployeeNoteDataLoading;

        //public bool EmployeeNoteDataLoading
        //{
        //    get { return _EmployeeNoteDataLoading; }
        //    set
        //    {
        //        if (_EmployeeNoteDataLoading != value)
        //        {
        //            _EmployeeNoteDataLoading = value;
        //            NotifyPropertyChanged("EmployeeNoteDataLoading");
        //        }
        //    }
        //}
        //#endregion


        //#region GenderOptions
        //public ObservableCollection<String> GenderOptions
        //{
        //    get
        //    {
        //        return new ObservableCollection<String>(new List<string>() {
        //                   GenderConverter.Choose,
        //                   GenderConverter.ChooseM,
        //                   GenderConverter.ChooseF});
        //    }
        //}
        //#endregion

        //#region SaveButtonContent
        //private string _SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));

        //public string SaveButtonContent
        //{
        //    get { return _SaveButtonContent; }
        //    set
        //    {
        //        if (_SaveButtonContent != value)
        //        {
        //            _SaveButtonContent = value;
        //            NotifyPropertyChanged("SaveButtonContent");
        //        }
        //    }
        //}
        //#endregion

        //#region SaveButtonEnabled
        //private bool _SaveButtonEnabled = true;

        //public bool SaveButtonEnabled
        //{
        //    get { return _SaveButtonEnabled; }
        //    set
        //    {
        //        if (_SaveButtonEnabled != value)
        //        {
        //            _SaveButtonEnabled = value;
        //            NotifyPropertyChanged("SaveButtonEnabled");
        //        }
        //    }
        //}
        //#endregion


        //#region SubmitButtonContent
        //private string _SubmitButtonContent = ((string)Application.Current.FindResource("Sačuvaj_i_proknjiži"));

        //public string SubmitButtonContent
        //{
        //    get { return _SubmitButtonContent; }
        //    set
        //    {
        //        if (_SubmitButtonContent != value)
        //        {
        //            _SubmitButtonContent = value;
        //            NotifyPropertyChanged("SubmitButtonContent");
        //        }
        //    }
        //}
        //#endregion

        //#region SubmitButtonEnabled
        //private bool _SubmitButtonEnabled = true;

        //public bool SubmitButtonEnabled
        //{
        //    get { return _SubmitButtonEnabled; }
        //    set
        //    {
        //        if (_SubmitButtonEnabled != value)
        //        {
        //            _SubmitButtonEnabled = value;
        //            NotifyPropertyChanged("SubmitButtonEnabled");
        //        }
        //    }
        //}
        //#endregion


        //#region IsCreateProcess
        //private bool _IsCreateProcess;

        //public bool IsCreateProcess
        //{
        //    get { return _IsCreateProcess; }
        //    set
        //    {
        //        if (_IsCreateProcess != value)
        //        {
        //            _IsCreateProcess = value;
        //            NotifyPropertyChanged("IsCreateProcess");
        //        }
        //    }
        //}
        //#endregion

        //#region IsHeaderCreated
        //private bool _IsHeaderCreated;

        //public bool IsHeaderCreated
        //{
        //    get { return _IsHeaderCreated; }
        //    set
        //    {
        //        if (_IsHeaderCreated != value)
        //        {
        //            _IsHeaderCreated = value;
        //            NotifyPropertyChanged("IsHeaderCreated");
        //        }
        //    }
        //}
        //#endregion

        //#region IsPopup
        //private bool _IsPopup;

        //public bool IsPopup
        //{
        //    get { return _IsPopup; }
        //    set
        //    {
        //        if (_IsPopup != value)
        //        {
        //            _IsPopup = value;
        //            NotifyPropertyChanged("IsPopup");
        //        }
        //    }
        //}
        //#endregion

        //#endregion

        //#region Constructor

        //public Employee_List_AddEdit(EmployeeViewModel Employee, bool isCreateProcess, bool isPopup)
        //{
        //    // Load required services
        //    EmployeeService = DependencyResolver.Kernel.Get<IEmployeeService>();

        //    InitializeComponent();

        //    this.DataContext = this;

        //    CurrentEmployee = Employee;

        //    CurrentEmployeeItemForm = new EmployeeItemViewModel();
        //    CurrentEmployeeProfessionItemForm = new EmployeeProfessionItemViewModel();
        //    CurrentEmployeeLicenceItemForm = new EmployeeLicenceItemViewModel();

        //    IsCreateProcess = isCreateProcess;
        //    IsHeaderCreated = !isCreateProcess;
        //    IsPopup = isPopup;

        //    Thread displayThread = new Thread(() =>
        //    {
        //        DisplayEmployeeItemData();
        //        DisplayProfessionItemData();
        //        DisplayLicenceItemData();
        //        DisplayDocumentData();
        //        DisplayEmployeeNoteData();
        //    });
        //    displayThread.IsBackground = true;
        //    displayThread.Start();
        //}

        //#endregion

        //#region Display data

        //private void DisplayEmployeeItemData()
        //{
        //    EmployeeItemDataLoading = true;

        //    EmployeeItemListResponse response = new EmployeeItemSQLiteRepository()
        //        .GetEmployeeItemsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

        //    if (response.Success)
        //    {
        //        EmployeeItemsFromDB = new ObservableCollection<EmployeeItemViewModel>(
        //            response.EmployeeItems ?? new List<EmployeeItemViewModel>());
        //    }
        //    else
        //    {
        //        EmployeeItemsFromDB = new ObservableCollection<EmployeeItemViewModel>();
        //    }

        //    EmployeeItemDataLoading = false;
        //}


        //private void DisplayProfessionItemData()
        //{
        //    LoadingProfessionItems = true;

        //    EmployeeProfessionItemListResponse response = new EmployeeProfessionItemSQLiteRepository()
        //        .GetEmployeeProfessionsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

        //    if (response.Success)
        //    {
        //        EmployeeProfessionItemsFromDB = new ObservableCollection<EmployeeProfessionItemViewModel>(
        //            response.EmployeeProfessionItems ?? new List<EmployeeProfessionItemViewModel>());
        //    }
        //    else
        //    {
        //        EmployeeProfessionItemsFromDB = new ObservableCollection<EmployeeProfessionItemViewModel>();
        //    }

        //    LoadingProfessionItems = false;
        //}


        //private void DisplayLicenceItemData()
        //{
        //    LoadingLicenceItems = true;

        //    EmployeeLicenceItemListResponse response = new EmployeeLicenceItemSQLiteRepository()
        //        .GetEmployeeLicencesByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

        //    if (response.Success)
        //    {
        //        EmployeeLicenceItemsFromDB = new ObservableCollection<EmployeeLicenceItemViewModel>(
        //            response.EmployeeLicenceItems ?? new List<EmployeeLicenceItemViewModel>());
        //    }
        //    else
        //    {
        //        EmployeeLicenceItemsFromDB = new ObservableCollection<EmployeeLicenceItemViewModel>();
        //    }

        //    LoadingLicenceItems = false;
        //}

        //private void DisplayDocumentData()
        //{
        //    EmployeeDocumentDataLoading = true;

        //    EmployeeDocumentListResponse response = new EmployeeDocumentSQLiteRepository()
        //        .GetEmployeeDocumentsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

        //    if (response.Success)
        //    {
        //        EmployeeDocumentsFromDB = new ObservableCollection<EmployeeDocumentViewModel>(
        //            response.EmployeeDocuments ?? new List<EmployeeDocumentViewModel>());
        //    }
        //    else
        //    {
        //        EmployeeDocumentsFromDB = new ObservableCollection<EmployeeDocumentViewModel>();
        //    }

        //    EmployeeDocumentDataLoading = false;
        //}

        //private void DisplayEmployeeNoteData()
        //{
        //    EmployeeNoteDataLoading = true;

        //    EmployeeNoteListResponse response = new EmployeeNoteSQLiteRepository()
        //        .GetEmployeeNotesByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

        //    if (response.Success)
        //    {
        //        EmployeeNotesFromDB = new ObservableCollection<EmployeeNoteViewModel>(
        //            response.EmployeeNotes ?? new List<EmployeeNoteViewModel>());
        //    }
        //    else
        //    {
        //        EmployeeNotesFromDB = new ObservableCollection<EmployeeNoteViewModel>();
        //    }

        //    EmployeeNoteDataLoading = false;
        //}

        //#endregion

        //#region Save header

        //private void btnSaveHeader_Click(object sender, RoutedEventArgs e)
        //{
        //    IsHeaderCreated = false;

        //    //#region Validation

        //    //if (CurrentEmployee.InputNoteDate == null)
        //    //{
        //    //    MainWindow.WarningMessage = "Obavezno polje: Datum prijema";
        //    //    return;
        //    //}

        //    //if (CurrentEmployee.Supplier == null)
        //    //{
        //    //    MainWindow.WarningMessage = "Obavezno polje: Dobavljač";
        //    //    return;
        //    //}

        //    //if (CurrentEmployee.Country == null)
        //    //{
        //    //    MainWindow.WarningMessage = "Obavezno polje: Država";
        //    //    return;
        //    //}

        //    //if (CurrentEmployee.ReceivedWeight == null)
        //    //{
        //    //    MainWindow.WarningMessage = "Obavezno polje: Težina kod prijema";
        //    //    return;
        //    //}

        //    //if (CurrentEmployee.FarmWeight == null)
        //    //{
        //    //    MainWindow.WarningMessage = "Obavezno polje: Težina na farmi";
        //    //    return;
        //    //}

        //    //if (CurrentEmployee.Quantity == null)
        //    //{
        //    //    MainWindow.WarningMessage = "Obavezno polje: Količina / Broj grla";
        //    //    return;
        //    //}

        //    //#endregion

        //    CurrentEmployee.IsSynced = false;
        //    CurrentEmployee.UpdatedAt = DateTime.Now;
        //    CurrentEmployee.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
        //    CurrentEmployee.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

        //    var sqLite = new EmployeeSQLiteRepository();
        //    sqLite.Delete(CurrentEmployee.Identifier);
        //    var response = sqLite.Create(CurrentEmployee);
        //    if (response.Success)
        //    {
        //        MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Zaglavlje_je_uspešno_sačuvanoUzvičnik"));
        //        IsHeaderCreated = true;

        //        popCountry2.txtCountry.Focus();
        //    }
        //    else
        //        MainWindow.ErrorMessage = response.Message;
        //}

        //#endregion

        //#region Add, edit, delete and cancel item

        //private void btnAddItem_Click(object sender, RoutedEventArgs e)
        //{
        //    #region Validation

        //    if (String.IsNullOrEmpty(CurrentEmployeeItemForm.Name))
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Ime"));
        //        return;
        //    }

        //    if (CurrentEmployeeItemForm.DateOfBirth == null)
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Datum_rodjenja"));
        //        return;
        //    }

        //    #endregion

        //    // IF update process, first delete item
        //    new EmployeeItemSQLiteRepository().Delete(CurrentEmployeeItemForm.Identifier);

        //    CurrentEmployeeItemForm.Employee = CurrentEmployee;
        //    CurrentEmployeeItemForm.Identifier = Guid.NewGuid();
        //    CurrentEmployeeItemForm.IsSynced = false;
        //    CurrentEmployeeItemForm.UpdatedAt = DateTime.Now;
        //    CurrentEmployeeItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
        //    CurrentEmployeeItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

        //    var response = new EmployeeItemSQLiteRepository().Create(CurrentEmployeeItemForm);
        //    if (response.Success)
        //    {
        //        CurrentEmployeeItemForm = new EmployeeItemViewModel();

        //        Thread displayThread = new Thread(() => DisplayEmployeeItemData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();

        //        popFamilyMember.txtFamilyMember.Focus();
        //    }
        //    else
        //        MainWindow.ErrorMessage = response.Message;
        //}

        //private void btnEdit_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentEmployeeItemForm = CurrentEmployeeItemDG;
        //}

        //private void btnDelete_Click(object sender, RoutedEventArgs e)
        //{
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku radnika", "");
        //    var showDialog = deleteConfirmationForm.ShowDialog();
        //    if (showDialog != null && showDialog.Value)
        //    {
        //        new EmployeeItemSQLiteRepository().Delete(CurrentEmployeeItemDG.Identifier);

        //        MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_radnika_je_uspešno_obrisanaUzvičnik"));

        //        Thread displayThread = new Thread(() => DisplayEmployeeItemData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }

        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //private void btnCancelItem_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentEmployeeItemForm = new EmployeeItemViewModel();
        //}

        //#endregion

        //#region Add, edit, delete and cancel document

        //private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        //{
        //    System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
        //    string[] fileNames = dialog.FileNames;

        //    if (fileNames.Length > 0)
        //        CurrentEmployeeDocumentForm.Path = fileNames[0];
        //}

        //private void btnChooseDocument_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.Forms.OpenFileDialog fileDIalog = new System.Windows.Forms.OpenFileDialog();

        //    fileDIalog.Multiselect = true;
        //    fileDIalog.FileOk += FileDIalog_FileOk;
        //    fileDIalog.Filter = "Image Files | *.pdf";
        //    fileDIalog.ShowDialog();
        //}

        //private void btnAddDocument_Click(object sender, RoutedEventArgs e)
        //{
        //    #region Validation

        //    if (String.IsNullOrEmpty(CurrentEmployeeDocumentForm.Name))
        //    {
        //        MainWindow.WarningMessage = "Obavezno polje: Naziv";
        //        return;
        //    }

        //    if (String.IsNullOrEmpty(CurrentEmployeeDocumentForm.Path))
        //    {
        //        MainWindow.WarningMessage = "Obavezno polje: Putanja";
        //        return;
        //    }

        //    if (CurrentEmployeeDocumentForm.CreateDate == null)
        //    {
        //        MainWindow.WarningMessage = "Obavezno polje: Datum kreiranja";
        //        return;
        //    }

        //    #endregion

        //    // IF update process, first delete item
        //    new EmployeeDocumentSQLiteRepository().Delete(CurrentEmployeeDocumentForm.Identifier);

        //    CurrentEmployeeDocumentForm.Employee = CurrentEmployee;
        //    CurrentEmployeeDocumentForm.Identifier = Guid.NewGuid();
        //    CurrentEmployeeDocumentForm.IsSynced = false;
        //    CurrentEmployeeDocumentForm.UpdatedAt = DateTime.Now;
        //    CurrentEmployeeDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
        //    CurrentEmployeeDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

        //    var response = new EmployeeDocumentSQLiteRepository().Create(CurrentEmployeeDocumentForm);
        //    if (response.Success)
        //    {
        //        CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
        //        CurrentEmployeeDocumentForm.CreateDate = DateTime.Now;

        //        Thread displayThread = new Thread(() => DisplayDocumentData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();

        //        txtDocumentName.Focus();
        //    }
        //    else
        //        MainWindow.ErrorMessage = response.Message;
        //}

        //private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
        //    CurrentEmployeeDocumentForm.CreateDate = DateTime.Now;
        //}

        //private void btnEditDocument_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentEmployeeDocumentForm = CurrentEmployeeDocumentDG;
        //}

        //private void btnDeleteDocument_Click(object sender, RoutedEventArgs e)
        //{
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("dokument", "");
        //    var showDialog = deleteConfirmationForm.ShowDialog();
        //    if (showDialog != null && showDialog.Value)
        //    {
        //        new EmployeeDocumentSQLiteRepository().Delete(CurrentEmployeeDocumentDG.Identifier);

        //        MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Dokument_je_uspešno_obrisanUzvičnik"));

        //        Thread displayThread = new Thread(() => DisplayDocumentData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }

        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //#endregion

        //#region Add, edit, delete and cancel note

        //private void btnAddNote_Click(object sender, RoutedEventArgs e)
        //{
        //    #region Validation

        //    if (String.IsNullOrEmpty(CurrentEmployeeNoteForm.Note))
        //    {
        //        MainWindow.WarningMessage = "Obavezno polje: Napomena";
        //        return;
        //    }

        //    if (CurrentEmployeeNoteForm.NoteDate == null)
        //    {
        //        MainWindow.WarningMessage = "Obavezno polje: Datum napomene";
        //        return;
        //    }

        //    #endregion

        //    // IF update process, first delete item
        //    new EmployeeNoteSQLiteRepository().Delete(CurrentEmployeeNoteForm.Identifier);

        //    CurrentEmployeeNoteForm.Employee = CurrentEmployee;
        //    CurrentEmployeeNoteForm.Identifier = Guid.NewGuid();
        //    CurrentEmployeeNoteForm.IsSynced = false;
        //    CurrentEmployeeNoteForm.UpdatedAt = DateTime.Now;
        //    CurrentEmployeeNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
        //    CurrentEmployeeNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

        //    var response = new EmployeeNoteSQLiteRepository().Create(CurrentEmployeeNoteForm);
        //    if (response.Success)
        //    {
        //        CurrentEmployeeNoteForm = new EmployeeNoteViewModel();

        //        Thread displayThread = new Thread(() => DisplayEmployeeNoteData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();

        //        txtNote.Focus();
        //    }
        //    else
        //        MainWindow.ErrorMessage = response.Message;
        //}

        //private void btnEditNote_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentEmployeeNoteForm = CurrentEmployeeNoteDG;
        //}

        //private void btnDeleteNote_Click(object sender, RoutedEventArgs e)
        //{
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku radnika", "");
        //    var showDialog = deleteConfirmationForm.ShowDialog();
        //    if (showDialog != null && showDialog.Value)
        //    {
        //        new EmployeeNoteSQLiteRepository().Delete(CurrentEmployeeNoteDG.Identifier);

        //        MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_radnika_je_uspešno_obrisanaUzvičnik"));

        //        Thread displayThread = new Thread(() => DisplayEmployeeNoteData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }

        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentEmployeeNoteForm = new EmployeeNoteViewModel();
        //}

        //#endregion

        //#region Submit and Cancel button

        //private void btnSubmit_Click(object sender, RoutedEventArgs e)
        //{
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    SubmitConfirmation submitConfirmationForm = new SubmitConfirmation();
        //    var showDialog = submitConfirmationForm.ShowDialog();
        //    if (showDialog != null && showDialog.Value)
        //    {
        //        // Save header for any new change
        //        btnSaveHeader_Click(sender, e);

        //        #region Validation

        //        if (!IsHeaderCreated)
        //        {
        //            MainWindow.WarningMessage = "Zaglavlje nije sačuvano";
        //            return;
        //        }

        //        #endregion

        //        Thread th = new Thread(() =>
        //        {
        //            SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
        //            SubmitButtonEnabled = false;

        //            CurrentEmployee.EmployeeItems = EmployeeItemsFromDB;
        //            CurrentEmployee.EmployeeLicences = EmployeeLicenceItemsFromDB;
        //            CurrentEmployee.EmployeeProfessions = EmployeeProfessionItemsFromDB;
        //            CurrentEmployee.EmployeeDocuments = EmployeeDocumentsFromDB;
        //            CurrentEmployee.EmployeeNotes = EmployeeNotesFromDB;

        //            EmployeeResponse response = EmployeeService.Create(CurrentEmployee);

                    //if (response.Success)
                    //{
                    //    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    //    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    //    SubmitButtonEnabled = true;

        //                EmployeeCreated();

        //                CurrentEmployee = new EmployeeViewModel();
        //                CurrentEmployee.Identifier = Guid.NewGuid();

        //                Application.Current.Dispatcher.BeginInvoke(
        //                    System.Windows.Threading.DispatcherPriority.Normal,
        //                    new Action(() =>
        //                    {
        //                        if (IsPopup)
        //                            FlyoutHelper.CloseFlyoutPopup(this);
        //                        else
        //                            FlyoutHelper.CloseFlyout(this);
        //                    })
        //                );
        //            }
        //            else
        //            {
        //                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_čuvanja_na_serveruUzvičnik")) + response.Message;

        //                SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
        //                SubmitButtonEnabled = true;
        //            }
        //        });
        //        th.IsBackground = true;
        //        th.Start();
        //    }
        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //private void btnCancel_Click(object sender, RoutedEventArgs e)
        //{
        //    EmployeeCreated();

        //    if (IsPopup)
        //        FlyoutHelper.CloseFlyoutPopup(this);
        //    else
        //        FlyoutHelper.CloseFlyout(this);
        //}

        //#endregion

        //#region Mouse wheel event 

        //private void PreviewMouseWheelEv(object sender, System.Windows.Input.MouseWheelEventArgs e)
        //{
        //    if (!e.Handled)
        //    {
        //        e.Handled = true;
        //        var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
        //        eventArg.RoutedEvent = UIElement.MouseWheelEvent;
        //        eventArg.Source = sender;
        //        var parent = ((Control)sender).Parent as UIElement;
        //        parent.RaiseEvent(eventArg);
        //    }
        //}

        //#endregion

        //#region INotifyPropertyChanged implementation
        //public event PropertyChangedEventHandler PropertyChanged;


        //// This method is called by the Set accessor of each property.
        //// The CallerMemberName attribute that is applied to the optional propertyName
        //// parameter causes the property name of the caller to be substituted as an argument.
        //private void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
        //#endregion

        //private void btnAddProfessionItem_Click(object sender, RoutedEventArgs e)
        //{
        //    #region Validation

        //    if (CurrentEmployeeProfessionItemForm.Profession == null)
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv_zanimanja"));
        //        return;
        //    }

        //    if (CurrentEmployeeProfessionItemForm.Country == null)
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Država"));
        //        return;
        //    }

        //    #endregion
        //    // IF update process, first delete item
        //    new EmployeeProfessionItemSQLiteRepository().Delete(CurrentEmployeeProfessionItemForm.Identifier);

        //    CurrentEmployeeProfessionItemForm.Employee = CurrentEmployee;
        //    CurrentEmployeeProfessionItemForm.Identifier = Guid.NewGuid();
        //    CurrentEmployeeProfessionItemForm.IsSynced = false;
        //    CurrentEmployeeProfessionItemForm.UpdatedAt = DateTime.Now;
        //    CurrentEmployeeProfessionItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
        //    CurrentEmployeeProfessionItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

        //    var response = new EmployeeProfessionItemSQLiteRepository().Create(CurrentEmployeeProfessionItemForm);
        //    if (response.Success)
        //    {
        //        CurrentEmployeeProfessionItemForm = new EmployeeProfessionItemViewModel();

        //        Thread displayThread = new Thread(() => DisplayProfessionItemData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();

        //        popCountry2.txtCountry.Focus();
        //    }
        //    else
        //        MainWindow.ErrorMessage = response.Message;
        //}

        //private void btnCancelProfessionItem_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentEmployeeProfessionItemForm = new EmployeeProfessionItemViewModel();
        //}

        //private void btnAddDItem_Click(object sender, RoutedEventArgs e)
        //{
        //    #region Validation

        //    if (CurrentEmployeeLicenceItemForm.Licence == null)
        //    {
        //        MainWindow.WarningMessage = "Obavezno polje: Vrsta dozvole";
        //        return;
        //    }

        //    if (CurrentEmployeeLicenceItemForm.Country == null)
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Država"));
        //        return;
        //    }

        //    #endregion
        //    // IF update process, first delete item
        //    new EmployeeLicenceItemSQLiteRepository().Delete(CurrentEmployeeLicenceItemForm.Identifier);

        //    CurrentEmployeeLicenceItemForm.Employee = CurrentEmployee;
        //    CurrentEmployeeLicenceItemForm.Identifier = Guid.NewGuid();
        //    CurrentEmployeeLicenceItemForm.IsSynced = false;
        //    CurrentEmployeeLicenceItemForm.UpdatedAt = DateTime.Now;
        //    CurrentEmployeeLicenceItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
        //    CurrentEmployeeLicenceItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

        //    var response = new EmployeeLicenceItemSQLiteRepository().Create(CurrentEmployeeLicenceItemForm);
        //    if (response.Success)
        //    {
        //        CurrentEmployeeLicenceItemForm = new EmployeeLicenceItemViewModel();

        //        Thread displayThread = new Thread(() => DisplayLicenceItemData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();

        //        popCountry3.txtCountry.Focus();
        //    }
        //    else
        //        MainWindow.ErrorMessage = response.Message;
        //}

        //private void btnCancelDItem_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentEmployeeLicenceItemForm = new EmployeeLicenceItemViewModel();
        //}

        //private void btnEditProfession_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentEmployeeProfessionItemForm = new EmployeeProfessionItemViewModel() {
        //        Company = CurrentProfessionDG.Company,
        //        Country = CurrentProfessionDG.Country,
        //        CreatedAt = CurrentProfessionDG.CreatedAt,
        //        CreatedBy = CurrentProfessionDG.CreatedBy,
        //        Employee = CurrentProfessionDG.Employee,
        //        Id = CurrentProfessionDG.Id,
        //        Identifier = CurrentProfessionDG.Identifier,
        //        IsActive = CurrentProfessionDG.IsActive,
        //        IsSynced = CurrentProfessionDG.IsSynced,
        //        Profession = CurrentProfessionDG.Profession,
        //        UpdatedAt = CurrentProfessionDG.UpdatedAt
        //    };
        //}

        //private void btnDeleteProfession_Click(object sender, RoutedEventArgs e)
        //{
        //    if (CurrentProfessionDG == null)
        //        return;

        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku radnika", "");
        //    var showDialog = deleteConfirmationForm.ShowDialog();
        //    if (showDialog != null && showDialog.Value)
        //    {
        //        new EmployeeProfessionItemSQLiteRepository().Delete(CurrentProfessionDG.Identifier);

        //        MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_radnika_je_uspešno_obrisanaUzvičnik"));

        //        Thread displayThread = new Thread(() => DisplayProfessionItemData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }

        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //private void btnEditLicence_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentEmployeeLicenceItemForm = new EmployeeLicenceItemViewModel()
        //    {
        //        Company = CurrentLicenceDG.Company,
        //        Country = CurrentLicenceDG.Country,
        //        CreatedAt = CurrentLicenceDG.CreatedAt,
        //        CreatedBy = CurrentLicenceDG.CreatedBy,
        //        Employee = CurrentLicenceDG.Employee,
        //        ValidFrom = CurrentLicenceDG.ValidFrom,
        //        ValidTo = CurrentLicenceDG.ValidTo,
        //        Id = CurrentLicenceDG.Id,
        //        Identifier = CurrentLicenceDG.Identifier,
        //        IsActive = CurrentLicenceDG.IsActive,
        //        IsSynced = CurrentLicenceDG.IsSynced,
        //        Licence = CurrentLicenceDG.Licence,
        //        UpdatedAt = CurrentLicenceDG.UpdatedAt
        //    };
        //}

        //private void btnDeleteLicence_Click(object sender, RoutedEventArgs e)
        //{
        //    if (CurrentLicenceDG == null)
        //        return;

        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku radnika", "");
        //    var showDialog = deleteConfirmationForm.ShowDialog();
        //    if (showDialog != null && showDialog.Value)
        //    {
        //        new EmployeeLicenceItemSQLiteRepository().Delete(CurrentLicenceDG.Identifier);

        //        MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_radnika_je_uspešno_obrisanaUzvičnik"));

        //        Thread displayThread = new Thread(() => DisplayLicenceItemData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }

        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

    }
}
