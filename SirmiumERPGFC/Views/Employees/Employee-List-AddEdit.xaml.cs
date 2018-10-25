using Ninject;
using ServiceInterfaces.Abstractions.Common.Individuals;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Common.Individuals;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Individuals;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Identity;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using WpfAppCommonCode.Converters;

namespace SirmiumERPGFC.Views.Employees
{
    public partial class Employee_List_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IEmployeeService EmployeeService;
        #endregion

        #region Event
        public event EmployeeHandler EmployeeCreated;
        #endregion

        #region CurrentEmployee
        private EmployeeViewModel _CurrentEmployee = new EmployeeViewModel();

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

        #region CurrentEmployeeItemForm
        private EmployeeItemViewModel _CurrentEmployeeItemForm = new EmployeeItemViewModel();

        public EmployeeItemViewModel CurrentEmployeeItemForm
        {
            get { return _CurrentEmployeeItemForm; }
            set
            {
                if (_CurrentEmployeeItemForm != value)
                {
                    _CurrentEmployeeItemForm = value;
                    NotifyPropertyChanged("CurrentEmployeeItemForm");
                }
            }
        }
        #endregion

        #region CurrentEmployeeItemDG
        private EmployeeItemViewModel _CurrentEmployeeItemDG;

        public EmployeeItemViewModel CurrentEmployeeItemDG
        {
            get { return _CurrentEmployeeItemDG; }
            set
            {
                if (_CurrentEmployeeItemDG != value)
                {
                    _CurrentEmployeeItemDG = value;
                    NotifyPropertyChanged("CurrentEmployeeItemDG");
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

        #region CurrentEmployeeProfessionItemForm
        private EmployeeProfessionItemViewModel _CurrentEmployeeProfessionItemForm;

        public EmployeeProfessionItemViewModel CurrentEmployeeProfessionItemForm
        {
            get { return _CurrentEmployeeProfessionItemForm; }
            set
            {
                if (_CurrentEmployeeProfessionItemForm != value)
                {
                    _CurrentEmployeeProfessionItemForm = value;
                    NotifyPropertyChanged("CurrentEmployeeProfessionItemForm");
                }
            }
        }
        #endregion

        #region CurrentProfessionDG
        private EmployeeProfessionItemViewModel _CurrentProfessionDG;

        public EmployeeProfessionItemViewModel CurrentProfessionDG
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

        #region CurrentEmployeeLicenceItemForm
        private EmployeeLicenceItemViewModel _CurrentEmployeeLicenceItemForm;

        public EmployeeLicenceItemViewModel CurrentEmployeeLicenceItemForm
        {
            get { return _CurrentEmployeeLicenceItemForm; }
            set
            {
                if (_CurrentEmployeeLicenceItemForm != value)
                {
                    _CurrentEmployeeLicenceItemForm = value;
                    NotifyPropertyChanged("CurrentEmployeeLicenceItemForm");
                }
            }
        }
        #endregion

        #region CurrentLicenceDG
        private EmployeeLicenceItemViewModel _CurrentLicenceDG;

        public EmployeeLicenceItemViewModel CurrentLicenceDG
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

        #endregion

        #region Constructor

        public Employee_List_AddEdit(EmployeeViewModel Employee, bool isCreateProcess, bool isPopup)
        {
            // Load required services
            EmployeeService = DependencyResolver.Kernel.Get<IEmployeeService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentEmployee = Employee;
            if(CurrentEmployee.Identifier == Guid.Empty)
                CurrentEmployee.Identifier = new Guid();

            CurrentEmployeeItemForm = new EmployeeItemViewModel();
            CurrentEmployeeProfessionItemForm = new EmployeeProfessionItemViewModel();
            CurrentEmployeeLicenceItemForm = new EmployeeLicenceItemViewModel();

            IsCreateProcess = isCreateProcess;
            IsHeaderCreated = !isCreateProcess;
            IsPopup = isPopup;

            Thread displayThread = new Thread(() =>
            {
                DisplayItemData();
            });
            displayThread.IsBackground = true;
            displayThread.Start();

            Thread displayThread2 = new Thread(() =>
            {
                DisplayProfessionItemData();
            });
            displayThread2.IsBackground = true;
            displayThread2.Start();

            Thread displayThread3 = new Thread(() =>
            {
                DisplayLicenceItemData();
            });
            displayThread3.IsBackground = true;
            displayThread3.Start();
        }

        #endregion

        #region Display data

        private void DisplayItemData()
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

        #endregion

        #region Save header

        private void btnSaveHeader_Click(object sender, RoutedEventArgs e)
        {
            IsHeaderCreated = false;

            //#region Validation

            //if (CurrentEmployee.InputNoteDate == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Datum prijema";
            //    return;
            //}

            //if (CurrentEmployee.Supplier == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Dobavljač";
            //    return;
            //}

            //if (CurrentEmployee.Country == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Država";
            //    return;
            //}

            //if (CurrentEmployee.ReceivedWeight == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Težina kod prijema";
            //    return;
            //}

            //if (CurrentEmployee.FarmWeight == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Težina na farmi";
            //    return;
            //}

            //if (CurrentEmployee.Quantity == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Količina / Broj grla";
            //    return;
            //}

            //#endregion

            CurrentEmployee.IsSynced = false;
            CurrentEmployee.UpdatedAt = DateTime.Now;
            CurrentEmployee.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentEmployee.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var sqLite = new EmployeeSQLiteRepository();
            sqLite.Delete(CurrentEmployee.Identifier);
            var response = sqLite.Create(CurrentEmployee);
            if (response.Success)
            {
                MainWindow.SuccessMessage = "Zaglavlje je uspešno sačuvano!";
                IsHeaderCreated = true;

                txtName.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        #endregion

        #region Add, edit, delete and cancel item

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            //#region Validation

            //if (String.IsNullOrEmpty(CurrentEmployeeItemForm.Name))
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Ime";
            //    return;
            //}

            //if (String.IsNullOrEmpty(CurrentEmployeeItemForm.EarTag))
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Ušna markica";
            //    return;
            //}

            //if (CurrentEmployeeItemForm.DateOfBirth == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Datum rođenja";
            //    return;
            //}

            //if (CurrentEmployeeItemForm.Gender == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Pol";
            //    return;
            //}

            //#endregion

            // IF update process, first delete item
            new EmployeeItemSQLiteRepository().Delete(CurrentEmployeeItemForm.Identifier);

            CurrentEmployeeItemForm.Employee = CurrentEmployee;
            CurrentEmployeeItemForm.Identifier = Guid.NewGuid();
            CurrentEmployeeItemForm.IsSynced = false;
            CurrentEmployeeItemForm.UpdatedAt = DateTime.Now;
            CurrentEmployeeItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentEmployeeItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new EmployeeItemSQLiteRepository().Create(CurrentEmployeeItemForm);
            if (response.Success)
            {
                CurrentEmployeeItemForm = new EmployeeItemViewModel();

                Thread displayThread = new Thread(() => DisplayItemData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtName.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeItemForm = CurrentEmployeeItemDG;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku radnika", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new EmployeeItemSQLiteRepository().Delete(CurrentEmployeeItemDG.Identifier);

                MainWindow.SuccessMessage = "Stavka radnika je uspešno obrisana!";

                Thread displayThread = new Thread(() => DisplayItemData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancelItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeItemForm = new EmployeeItemViewModel();
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

                    CurrentEmployee.EmployeeItems = EmployeeItemsFromDB;
                    CurrentEmployee.EmployeeLicences = EmployeeLicenceItemsFromDB;
                    CurrentEmployee.EmployeeProfessions = EmployeeProfessionItemsFromDB;

                    EmployeeResponse response = EmployeeService.Create(CurrentEmployee);

                    if (response.Success)
                    {
                        new EmployeeSQLiteRepository().UpdateSyncStatus(CurrentEmployee.Identifier, response.Employee.Id, true);
                        MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                        SubmitButtonContent = " Proknjiži ";
                        SubmitButtonEnabled = true;

                        EmployeeCreated();

                        CurrentEmployee = new EmployeeViewModel();
                        CurrentEmployee.Identifier = Guid.NewGuid();

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
                        MainWindow.ErrorMessage = "Greška kod čuvanja na serveru!";

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
            EmployeeCreated();

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
            // IF update process, first delete item
            new EmployeeProfessionItemSQLiteRepository().Delete(CurrentEmployeeProfessionItemForm.Identifier);

            CurrentEmployeeProfessionItemForm.Employee = CurrentEmployee;
            CurrentEmployeeProfessionItemForm.Identifier = Guid.NewGuid();
            CurrentEmployeeProfessionItemForm.IsSynced = false;
            CurrentEmployeeProfessionItemForm.UpdatedAt = DateTime.Now;
            CurrentEmployeeProfessionItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentEmployeeProfessionItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new EmployeeProfessionItemSQLiteRepository().Create(CurrentEmployeeProfessionItemForm);
            if (response.Success)
            {
                CurrentEmployeeProfessionItemForm = new EmployeeProfessionItemViewModel();

                Thread displayThread = new Thread(() => DisplayProfessionItemData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtName.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelProfessionItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeProfessionItemForm = new EmployeeProfessionItemViewModel();
        }

        private void btnAddDItem_Click(object sender, RoutedEventArgs e)
        {
            // IF update process, first delete item
            new EmployeeLicenceItemSQLiteRepository().Delete(CurrentEmployeeLicenceItemForm.Identifier);

            CurrentEmployeeLicenceItemForm.Employee = CurrentEmployee;
            CurrentEmployeeLicenceItemForm.Identifier = Guid.NewGuid();
            CurrentEmployeeLicenceItemForm.IsSynced = false;
            CurrentEmployeeLicenceItemForm.UpdatedAt = DateTime.Now;
            CurrentEmployeeLicenceItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentEmployeeLicenceItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new EmployeeLicenceItemSQLiteRepository().Create(CurrentEmployeeLicenceItemForm);
            if (response.Success)
            {
                CurrentEmployeeLicenceItemForm = new EmployeeLicenceItemViewModel();

                Thread displayThread = new Thread(() => DisplayLicenceItemData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtName.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeLicenceItemForm = new EmployeeLicenceItemViewModel();
        }

        private void btnEditProfession_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeProfessionItemForm = new EmployeeProfessionItemViewModel() {
                Company = CurrentProfessionDG.Company,
                Country = CurrentProfessionDG.Country,
                CreatedAt = CurrentProfessionDG.CreatedAt,
                CreatedBy = CurrentProfessionDG.CreatedBy,
                Employee = CurrentProfessionDG.Employee,
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

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku radnika", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new EmployeeProfessionItemSQLiteRepository().Delete(CurrentProfessionDG.Identifier);

                MainWindow.SuccessMessage = "Stavka radnika je uspešno obrisana!";

                Thread displayThread = new Thread(() => DisplayProfessionItemData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnEditLicence_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeLicenceItemForm = new EmployeeLicenceItemViewModel()
            {
                Company = CurrentLicenceDG.Company,
                Country = CurrentLicenceDG.Country,
                CreatedAt = CurrentLicenceDG.CreatedAt,
                CreatedBy = CurrentLicenceDG.CreatedBy,
                Employee = CurrentLicenceDG.Employee,
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

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku radnika", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new EmployeeLicenceItemSQLiteRepository().Delete(CurrentLicenceDG.Identifier);

                MainWindow.SuccessMessage = "Stavka radnika je uspešno obrisana!";

                Thread displayThread = new Thread(() => DisplayLicenceItemData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }
    }
}
