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
    /// Interaction logic for Employee_Card_AddEdit.xaml
    /// </summary>
    public partial class Employee_Card_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IEmployeeService employeeService;
        IEmployeeCardService employeeCardService;
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

        #region CurrentEmployeeCardForm
        private EmployeeCardViewModel _CurrentEmployeeCardForm = new EmployeeCardViewModel() { CardDate = DateTime.Now };

        public EmployeeCardViewModel CurrentEmployeeCardForm
        {
            get { return _CurrentEmployeeCardForm; }
            set
            {
                if (_CurrentEmployeeCardForm != value)
                {
                    _CurrentEmployeeCardForm = value;
                    NotifyPropertyChanged("CurrentEmployeeCardForm");
                }
            }
        }
        #endregion

        #region CurrentEmployeeCardDG
        private EmployeeCardViewModel _CurrentEmployeeCardDG;

        public EmployeeCardViewModel CurrentEmployeeCardDG
        {
            get { return _CurrentEmployeeCardDG; }
            set
            {
                if (_CurrentEmployeeCardDG != value)
                {
                    _CurrentEmployeeCardDG = value;
                    NotifyPropertyChanged("CurrentEmployeeCardDG");
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

        public Employee_Card_AddEdit(EmployeeViewModel employee)
        {
            employeeService = DependencyResolver.Kernel.Get<IEmployeeService>();
            employeeCardService = DependencyResolver.Kernel.Get<IEmployeeCardService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentEmployee = employee;
            CurrentEmployeeCardForm = new EmployeeCardViewModel();
            CurrentEmployeeCardForm.Identifier = Guid.NewGuid();
            CurrentEmployeeCardForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayEmployeeCardData());
            displayThread.IsBackground = true;
            displayThread.Start();

            txtNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayEmployeeCardData()
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

        private void DgEmployeeCards_LoadingRow(object sender, DataGridRowEventArgs e)
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

        private void btnAddCard_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeCardForm.Description == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Opis"));
                return;
            }

            #endregion

            new EmployeeCardSQLiteRepository().Delete(CurrentEmployeeCardForm.Identifier);

            CurrentEmployeeCardForm.Employee = CurrentEmployee;

            CurrentEmployeeCardForm.IsSynced = false;
            CurrentEmployeeCardForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentEmployeeCardForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new EmployeeCardSQLiteRepository().Create(CurrentEmployeeCardForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentEmployeeCardForm = new EmployeeCardViewModel();
                CurrentEmployeeCardForm.Identifier = Guid.NewGuid();
                CurrentEmployeeCardForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentEmployeeCardForm = new EmployeeCardViewModel();
            CurrentEmployeeCardForm.Identifier = Guid.NewGuid();
            CurrentEmployeeCardForm.ItemStatus = ItemStatus.Added;

            EmployeeCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayEmployeeCardData());
            displayThread.IsBackground = true;
            displayThread.Start();

            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    txtNote.Focus();
                })
            );

            SubmitButtonEnabled = true;
        }

        private void btnEditCard_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeCardForm = new EmployeeCardViewModel();
            CurrentEmployeeCardForm.Identifier = CurrentEmployeeCardDG.Identifier;
            CurrentEmployeeCardForm.ItemStatus = ItemStatus.Edited;

            CurrentEmployeeCardForm.Description = CurrentEmployeeCardDG.Description;
            CurrentEmployeeCardForm.CardDate = CurrentEmployeeCardDG.CardDate;
            CurrentEmployeeCardForm.PlusMinus = CurrentEmployeeCardDG.PlusMinus;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new EmployeeCardSQLiteRepository().SetStatusDeleted(CurrentEmployeeCardDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentEmployeeCardForm = new EmployeeCardViewModel();
                CurrentEmployeeCardForm.Identifier = Guid.NewGuid();
                CurrentEmployeeCardForm.ItemStatus = ItemStatus.Added;

                CurrentEmployeeCardDG = null;

                EmployeeCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayEmployeeCardData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelCard_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeCardForm = new EmployeeCardViewModel();
            CurrentEmployeeCardForm.Identifier = Guid.NewGuid();
            CurrentEmployeeCardForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (EmployeeCardsFromDB == null || EmployeeCardsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentEmployee.EmployeeCards = EmployeeCardsFromDB;
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
