using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Employees;
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
    /// <summary>
    /// Interaction logic for Employee_Licence_AddEdit.xaml
    /// </summary>
    public partial class Employee_Licence_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IEmployeeService employeeService;
        IEmployeeLicenceService employeeLicenceService;
        #endregion


        #region Event
        public event EmployeeHandler EmployeeCreatedUpdated;
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

        #region CurrentEmployeeLicenceForm
        private EmployeeLicenceItemViewModel _CurrentEmployeeLicenceForm = new EmployeeLicenceItemViewModel();

        public EmployeeLicenceItemViewModel CurrentEmployeeLicenceForm
        {
            get { return _CurrentEmployeeLicenceForm; }
            set
            {
                if (_CurrentEmployeeLicenceForm != value)
                {
                    _CurrentEmployeeLicenceForm = value;
                    NotifyPropertyChanged("CurrentEmployeeLicenceForm");
                }
            }
        }
        #endregion

        #region CurrentEmployeeLicenceDG
        private EmployeeLicenceItemViewModel _CurrentEmployeeLicenceDG;

        public EmployeeLicenceItemViewModel CurrentEmployeeLicenceDG
        {
            get { return _CurrentEmployeeLicenceDG; }
            set
            {
                if (_CurrentEmployeeLicenceDG != value)
                {
                    _CurrentEmployeeLicenceDG = value;
                    NotifyPropertyChanged("CurrentEmployeeLicenceDG");
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




        #region SubmitButtonContent
        private string _SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));

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

        #endregion

        #region Constructor

        public Employee_Licence_AddEdit(EmployeeViewModel employee)
        {
            employeeService = DependencyResolver.Kernel.Get<IEmployeeService>();
            employeeLicenceService = DependencyResolver.Kernel.Get<IEmployeeLicenceService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentEmployee = employee;
            CurrentEmployeeLicenceForm = new EmployeeLicenceItemViewModel();
            CurrentEmployeeLicenceForm.Identifier = Guid.NewGuid();
            CurrentEmployeeLicenceForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayEmployeeLicenceData());
            displayThread.IsBackground = true;
            displayThread.Start();

            ValidFrom.Focus();
        }

        #endregion

        #region Display data

        public void DisplayEmployeeLicenceData()
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

        private void DgEmployeeLicences_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void dg_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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

        #region Add, Edit and Delete 

        private void btnAddLicence_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeLicenceForm.Licence.Description == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Licence"));
                return;
            }

            #endregion

            new EmployeeLicenceItemSQLiteRepository().Delete(CurrentEmployeeLicenceForm.Identifier);

            CurrentEmployeeLicenceForm.Employee = CurrentEmployee;

            CurrentEmployeeLicenceForm.IsSynced = false;
            CurrentEmployeeLicenceForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentEmployeeLicenceForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new EmployeeLicenceItemSQLiteRepository().Create(CurrentEmployeeLicenceForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentEmployeeLicenceForm = new EmployeeLicenceItemViewModel();
                CurrentEmployeeLicenceForm.Identifier = Guid.NewGuid();
                CurrentEmployeeLicenceForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentEmployeeLicenceForm = new EmployeeLicenceItemViewModel();
            CurrentEmployeeLicenceForm.Identifier = Guid.NewGuid();
            CurrentEmployeeLicenceForm.ItemStatus = ItemStatus.Added;

            EmployeeCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayEmployeeLicenceData());
            displayThread.IsBackground = true;
            displayThread.Start();

            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    ValidFrom.Focus();
                })
            );

            SubmitButtonEnabled = true;
        }

        private void btnEditLicence_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeLicenceForm = new EmployeeLicenceItemViewModel();
            CurrentEmployeeLicenceForm.Identifier = CurrentEmployeeLicenceDG.Identifier;
            CurrentEmployeeLicenceForm.ItemStatus = ItemStatus.Edited;

            CurrentEmployeeLicenceForm.Country = CurrentEmployeeLicenceDG.Country;
            CurrentEmployeeLicenceForm.Licence = CurrentEmployeeLicenceDG.Licence;
            CurrentEmployeeLicenceForm.ValidFrom = CurrentEmployeeLicenceDG.ValidFrom;
            CurrentEmployeeLicenceForm.ValidTo = CurrentEmployeeLicenceDG.ValidTo;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new EmployeeLicenceItemSQLiteRepository().SetStatusDeleted(CurrentEmployeeLicenceDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentEmployeeLicenceForm = new EmployeeLicenceItemViewModel();
                CurrentEmployeeLicenceForm.Identifier = Guid.NewGuid();
                CurrentEmployeeLicenceForm.ItemStatus = ItemStatus.Added;

                CurrentEmployeeLicenceDG = null;

                EmployeeCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayEmployeeLicenceData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelLicence_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeLicenceForm = new EmployeeLicenceItemViewModel();
            CurrentEmployeeLicenceForm.Identifier = Guid.NewGuid();
            CurrentEmployeeLicenceForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (EmployeeLicencesFromDB == null || EmployeeLicencesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentEmployee.EmployeeLicences = EmployeeLicencesFromDB;
                EmployeeResponse response = employeeService.Create(CurrentEmployee);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_čuvanja_na_serveruUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;

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
            EmployeeCreatedUpdated();

            FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion
    }
}
