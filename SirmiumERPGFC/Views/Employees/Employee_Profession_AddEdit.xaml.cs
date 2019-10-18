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
    /// Interaction logic for Employee_Profession_AddEdit.xaml
    /// </summary>
    public partial class Employee_Profession_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IEmployeeService employeeService;
        IEmployeeProfessionService employeeProfessionService;
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

        #region CurrentEmployeeProfessionForm
        private EmployeeProfessionItemViewModel _CurrentEmployeeProfessionForm = new EmployeeProfessionItemViewModel();

        public EmployeeProfessionItemViewModel CurrentEmployeeProfessionForm
        {
            get { return _CurrentEmployeeProfessionForm; }
            set
            {
                if (_CurrentEmployeeProfessionForm != value)
                {
                    _CurrentEmployeeProfessionForm = value;
                    NotifyPropertyChanged("CurrentEmployeeProfessionForm");
                }
            }
        }
        #endregion

        #region CurrentEmployeeProfessionDG
        private EmployeeProfessionItemViewModel _CurrentEmployeeProfessionDG;

        public EmployeeProfessionItemViewModel CurrentEmployeeProfessionDG
        {
            get { return _CurrentEmployeeProfessionDG; }
            set
            {
                if (_CurrentEmployeeProfessionDG != value)
                {
                    _CurrentEmployeeProfessionDG = value;
                    NotifyPropertyChanged("CurrentEmployeeProfessionDG");
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

        public Employee_Profession_AddEdit(EmployeeViewModel employee)
        {
            employeeService = DependencyResolver.Kernel.Get<IEmployeeService>();
            employeeProfessionService = DependencyResolver.Kernel.Get<IEmployeeProfessionService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentEmployee = employee;
            CurrentEmployeeProfessionForm = new EmployeeProfessionItemViewModel();
            CurrentEmployeeProfessionForm.Identifier = Guid.NewGuid();
            CurrentEmployeeProfessionForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayEmployeeProfessionData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddEmployee.Focus();
        }

        #endregion

        #region Display data

        public void DisplayEmployeeProfessionData()
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

        private void DgEmployeeProfessions_LoadingRow(object sender, DataGridRowEventArgs e)
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

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeProfessionForm.Profession.Name == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_polje_naziv_profesije"));
                return;
            }

            #endregion

            new EmployeeProfessionItemSQLiteRepository().Delete(CurrentEmployeeProfessionForm.Identifier);

            CurrentEmployeeProfessionForm.Employee = CurrentEmployee;

            CurrentEmployeeProfessionForm.IsSynced = false;
            CurrentEmployeeProfessionForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentEmployeeProfessionForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new EmployeeProfessionItemSQLiteRepository().Create(CurrentEmployeeProfessionForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentEmployeeProfessionForm = new EmployeeProfessionItemViewModel();
                CurrentEmployeeProfessionForm.Identifier = Guid.NewGuid();
                CurrentEmployeeProfessionForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentEmployeeProfessionForm = new EmployeeProfessionItemViewModel();
            CurrentEmployeeProfessionForm.Identifier = Guid.NewGuid();
            CurrentEmployeeProfessionForm.ItemStatus = ItemStatus.Added;

            EmployeeCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayEmployeeProfessionData());
            displayThread.IsBackground = true;
            displayThread.Start();

            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    popCountry.Focus();
                })
            );

            SubmitButtonEnabled = true;
        }

        private void btnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeProfessionForm = new EmployeeProfessionItemViewModel();
            CurrentEmployeeProfessionForm.Identifier = CurrentEmployeeProfessionDG.Identifier;
            CurrentEmployeeProfessionForm.ItemStatus = ItemStatus.Edited;

            CurrentEmployeeProfessionForm.Country = CurrentEmployeeProfessionDG.Country;
            CurrentEmployeeProfessionForm.Profession = CurrentEmployeeProfessionDG.Profession;
            
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new EmployeeProfessionItemSQLiteRepository().SetStatusDeleted(CurrentEmployeeProfessionDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentEmployeeProfessionForm = new EmployeeProfessionItemViewModel();
                CurrentEmployeeProfessionForm.Identifier = Guid.NewGuid();
                CurrentEmployeeProfessionForm.ItemStatus = ItemStatus.Added;

                CurrentEmployeeProfessionDG = null;

                EmployeeCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayEmployeeProfessionData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelEmployee_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeProfessionForm = new EmployeeProfessionItemViewModel();
            CurrentEmployeeProfessionForm.Identifier = Guid.NewGuid();
            CurrentEmployeeProfessionForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (EmployeeProfessionsFromDB == null || EmployeeProfessionsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentEmployee.EmployeeProfessions = EmployeeProfessionsFromDB;
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
