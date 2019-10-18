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
    /// Interaction logic for Employee_Note_AddEdit.xaml
    /// </summary>
    public partial class Employee_Note_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IEmployeeService employeeService;
        IEmployeeNoteService employeeNoteService;
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
        private EmployeeNoteViewModel _CurrentEmployeeNoteForm = new EmployeeNoteViewModel() { NoteDate = DateTime.Now };

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

        public Employee_Note_AddEdit(EmployeeViewModel employee)
        {
            employeeService = DependencyResolver.Kernel.Get<IEmployeeService>();
            employeeNoteService = DependencyResolver.Kernel.Get<IEmployeeNoteService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentEmployee = employee;
            CurrentEmployeeNoteForm = new EmployeeNoteViewModel();
            CurrentEmployeeNoteForm.Identifier = Guid.NewGuid();
            CurrentEmployeeNoteForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayEmployeeNoteData());
            displayThread.IsBackground = true;
            displayThread.Start();

            txtNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayEmployeeNoteData()
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

        private void DgEmployeeNotes_LoadingRow(object sender, DataGridRowEventArgs e)
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

        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeNoteForm.Note == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Napomena"));
                return;
            }

            #endregion

            new EmployeeNoteSQLiteRepository().Delete(CurrentEmployeeNoteForm.Identifier);

            CurrentEmployeeNoteForm.Employee = CurrentEmployee;

            CurrentEmployeeNoteForm.IsSynced = false;
            CurrentEmployeeNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentEmployeeNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new EmployeeNoteSQLiteRepository().Create(CurrentEmployeeNoteForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentEmployeeNoteForm = new EmployeeNoteViewModel();
                CurrentEmployeeNoteForm.Identifier = Guid.NewGuid();
                CurrentEmployeeNoteForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentEmployeeNoteForm = new EmployeeNoteViewModel();
            CurrentEmployeeNoteForm.Identifier = Guid.NewGuid();
            CurrentEmployeeNoteForm.ItemStatus = ItemStatus.Added;

            EmployeeCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayEmployeeNoteData());
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

        private void btnEditNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeNoteForm = new EmployeeNoteViewModel();
            CurrentEmployeeNoteForm.Identifier = CurrentEmployeeNoteDG.Identifier;
            CurrentEmployeeNoteForm.ItemStatus = ItemStatus.Edited;

            CurrentEmployeeNoteForm.Note = CurrentEmployeeNoteDG.Note;
            CurrentEmployeeNoteForm.NoteDate = CurrentEmployeeNoteDG.NoteDate;
           
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new EmployeeNoteSQLiteRepository().SetStatusDeleted(CurrentEmployeeNoteDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentEmployeeNoteForm = new EmployeeNoteViewModel();
                CurrentEmployeeNoteForm.Identifier = Guid.NewGuid();
                CurrentEmployeeNoteForm.ItemStatus = ItemStatus.Added;

                CurrentEmployeeNoteDG = null;

                EmployeeCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayEmployeeNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeNoteForm = new EmployeeNoteViewModel();
            CurrentEmployeeNoteForm.Identifier = Guid.NewGuid();
            CurrentEmployeeNoteForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (EmployeeNotesFromDB == null || EmployeeNotesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentEmployee.EmployeeNotes = EmployeeNotesFromDB;
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
