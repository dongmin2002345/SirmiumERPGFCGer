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
    /// Interaction logic for Employee_Item_AddEdit.xaml
    /// </summary>
    public partial class Employee_Item_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IEmployeeService employeeService;
        IEmployeeItemService employeeItemService;
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

        public Employee_Item_AddEdit(EmployeeViewModel employee)
        {
            employeeService = DependencyResolver.Kernel.Get<IEmployeeService>();
            employeeItemService = DependencyResolver.Kernel.Get<IEmployeeItemService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentEmployee = employee;
            CurrentEmployeeItemForm = new EmployeeItemViewModel();
            CurrentEmployeeItemForm.Identifier = Guid.NewGuid();
            CurrentEmployeeItemForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayEmployeeItemData());
            displayThread.IsBackground = true;
            displayThread.Start();

            txtName.Focus();
        }

        #endregion

        #region Display data

        public void DisplayEmployeeItemData()
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

        private void DgEmployeeItems_LoadingRow(object sender, DataGridRowEventArgs e)
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

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeItemForm.FamilyMember.Name == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_clanporodice"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;


                CurrentEmployeeItemForm.Employee = CurrentEmployee;


                CurrentEmployeeItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentEmployeeItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                new EmployeeItemSQLiteRepository().Delete(CurrentEmployeeItemForm.Identifier);
                var response = new EmployeeItemSQLiteRepository().Create(CurrentEmployeeItemForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentEmployeeItemForm = new EmployeeItemViewModel();
                    CurrentEmployeeItemForm.Identifier = Guid.NewGuid();
                    CurrentEmployeeItemForm.ItemStatus = ItemStatus.Added;
                    CurrentEmployeeItemForm.IsSynced = false;
                    return;
                }

                CurrentEmployeeItemForm = new EmployeeItemViewModel();
                CurrentEmployeeItemForm.Identifier = Guid.NewGuid();
                CurrentEmployeeItemForm.ItemStatus = ItemStatus.Added;
                CurrentEmployeeItemForm.IsSynced = false;
                EmployeeCreatedUpdated();

                DisplayEmployeeItemData();

                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        txtName.Focus();
                    })
                );

                SubmitButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeItemForm = new EmployeeItemViewModel();
            CurrentEmployeeItemForm.Identifier = CurrentEmployeeItemDG.Identifier;
            CurrentEmployeeItemForm.ItemStatus = ItemStatus.Edited;


            CurrentEmployeeItemForm.IsSynced = CurrentEmployeeItemDG.IsSynced;
            CurrentEmployeeItemForm.FamilyMember = CurrentEmployeeItemDG.FamilyMember;
            CurrentEmployeeItemForm.Name = CurrentEmployeeItemDG.Name;
            CurrentEmployeeItemForm.DateOfBirth = CurrentEmployeeItemDG.DateOfBirth;
            CurrentEmployeeItemForm.Passport = CurrentEmployeeItemDG.Passport;
            CurrentEmployeeItemForm.EmbassyDate = CurrentEmployeeItemDG.EmbassyDate;
            CurrentEmployeeItemForm.UpdatedAt = CurrentEmployeeItemDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new EmployeeItemSQLiteRepository().SetStatusDeleted(CurrentEmployeeItemDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentEmployeeItemForm = new EmployeeItemViewModel();
                CurrentEmployeeItemForm.Identifier = Guid.NewGuid();
                CurrentEmployeeItemForm.ItemStatus = ItemStatus.Added;

                CurrentEmployeeItemDG = null;

                EmployeeCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayEmployeeItemData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeItemForm = new EmployeeItemViewModel();
            CurrentEmployeeItemForm.Identifier = Guid.NewGuid();
            CurrentEmployeeItemForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (EmployeeItemsFromDB == null || EmployeeItemsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentEmployee.EmployeeItems = EmployeeItemsFromDB;
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
