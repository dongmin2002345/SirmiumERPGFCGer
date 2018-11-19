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
                            DisplayProfessionItemData();
                            DisplayLicenceItemData();
                            DisplayDocumentData();
                            DisplayCardData();
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
                    NotifyPropertyChanged("EmployeeItemDataLoading");
                }
            }
        }
        #endregion


        #region EmployeeProfessionItemsFromDB
        private ObservableCollection<EmployeeProfessionItemViewModel> _EmployeeProfessionItemsFromDB;

        public ObservableCollection<EmployeeProfessionItemViewModel> EmployeeProfessionItemsFromDB
        {
            get { return _EmployeeProfessionItemsFromDB; }
            set
            {
                if (_EmployeeProfessionItemsFromDB != value)
                {
                    _EmployeeProfessionItemsFromDB = value;
                    NotifyPropertyChanged("EmployeeProfessionItemsFromDB");
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


        #region EmployeeLicenceItemsFromDB
        private ObservableCollection<EmployeeLicenceItemViewModel> _EmployeeLicenceItemsFromDB;

        public ObservableCollection<EmployeeLicenceItemViewModel> EmployeeLicenceItemsFromDB
        {
            get { return _EmployeeLicenceItemsFromDB; }
            set
            {
                if (_EmployeeLicenceItemsFromDB != value)
                {
                    _EmployeeLicenceItemsFromDB = value;
                    NotifyPropertyChanged("EmployeeLicenceItemsFromDB");
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
        private string _RefreshButtonContent = " Osveži ";

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

        public Employee_List()
        {
            // Get required service
            this.employeeService = DependencyResolver.Kernel.Get<IEmployeeService>();
            this.employeeItemService = DependencyResolver.Kernel.Get<IEmployeeItemService>();
            this.employeeLicenceService = DependencyResolver.Kernel.Get<IEmployeeLicenceService>();
            this.employeeProfessionService = DependencyResolver.Kernel.Get<IEmployeeProfessionService>();
            this.employeeDocumentService = DependencyResolver.Kernel.Get<IEmployeeDocumentService>();
            this.employeeCardService = DependencyResolver.Kernel.Get<IEmployeeCardService>();

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

                MainWindow.SuccessMessage = "Podaci su uspešno sinhronizovani!";
            });
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => PopulateData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        private void PopulateData()
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
                MainWindow.ErrorMessage = "Greška prilikom učitavanja podataka!";
            }

            EmployeeItemDataLoading = false;
        }

        private void DisplayProfessionItemData()
        {
            LoadingProfessionItems = true;

            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemSQLiteRepository()
                .GetEmployeeProfessionsByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

            if (response.Success)
            {
                EmployeeProfessionItemsFromDB = new ObservableCollection<EmployeeProfessionItemViewModel>(
                    response.EmployeeProfessionItems ?? new List<EmployeeProfessionItemViewModel>());
            }
            else
            {
                EmployeeProfessionItemsFromDB = new ObservableCollection<EmployeeProfessionItemViewModel>();
            }

            LoadingProfessionItems = false;
        }

        private void DisplayLicenceItemData()
        {
            LoadingLicenceItems = true;

            EmployeeLicenceItemListResponse response = new EmployeeLicenceItemSQLiteRepository()
                .GetEmployeeLicencesByEmployee(MainWindow.CurrentCompanyId, CurrentEmployee.Identifier);

            if (response.Success)
            {
                EmployeeLicenceItemsFromDB = new ObservableCollection<EmployeeLicenceItemViewModel>(
                    response.EmployeeLicenceItems ?? new List<EmployeeLicenceItemViewModel>());
            }
            else
            {
                EmployeeLicenceItemsFromDB = new ObservableCollection<EmployeeLicenceItemViewModel>();
            }

            LoadingLicenceItems = false;
        }

        private void DisplayDocumentData()
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

        private void DisplayCardData()
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

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Radnici ... ";
            new EmployeeSQLiteRepository().Sync(employeeService);

            RefreshButtonContent = " Stavke ... ";
            new EmployeeItemSQLiteRepository().Sync(employeeItemService);
            new EmployeeLicenceItemSQLiteRepository().Sync(employeeLicenceService);
            new EmployeeProfessionItemSQLiteRepository().Sync(employeeProfessionService);
            new EmployeeDocumentSQLiteRepository().Sync(employeeDocumentService);
            new EmployeeCardSQLiteRepository().Sync(employeeCardService);

            PopulateData();

            RefreshButtonContent = " Osveži ";
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EmployeeViewModel Employee = new EmployeeViewModel();
            Employee.Identifier = Guid.NewGuid();
            Employee.Gender = 1;

            Employee_List_AddEdit EmployeeAddEditForm = new Employee_List_AddEdit(Employee, true, false);
            EmployeeAddEditForm.EmployeeCreated += new EmployeeHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o radnicima", 95, EmployeeAddEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = "Morate odabrati stavku za izmenu!";
                return;
            }

            Employee_List_AddEdit EmployeeAddEditForm = new Employee_List_AddEdit(CurrentEmployee, false, false);
            EmployeeAddEditForm.EmployeeCreated += new EmployeeHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o radnicima", 90, EmployeeAddEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEmployee == null)
            {
                MainWindow.WarningMessage = "Morate odabrati stavku za brisanje!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku", CurrentEmployee.Code.ToString());
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                EmployeeResponse response = employeeService.Delete(CurrentEmployee.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                }

                var result = new EmployeeSQLiteRepository().Delete(CurrentEmployee.Identifier);
                if (result.Success)
                {
                    MainWindow.SuccessMessage = "Podaci su uspešno obrisani!";

                    Thread displayThread = new Thread(() => PopulateData());
                    displayThread.IsBackground = true;
                    displayThread.Start();
                }
                else
                {
                    MainWindow.ErrorMessage = result.Message;
                }
            }

            // Remove blur effects
            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        #endregion

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

        #region Pagination

        private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage = 1;

                Thread displayThread = new Thread(() => PopulateData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => PopulateData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => PopulateData());
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

                Thread displayThread = new Thread(() => PopulateData());
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

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            EmployeesExcelReport.Show(EmployeesFromDB.ToList());
        }
    }
}

