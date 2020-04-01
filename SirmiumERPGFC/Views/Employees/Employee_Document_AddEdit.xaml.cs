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
using SirmiumERPGFC.ViewComponents.Dialogs;
using SirmiumERPGFC.Views.Home;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SirmiumERPGFC.Views.Employees
{
    public partial class Employee_Document_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IEmployeeService employeeService;
        IEmployeeDocumentService employeeDocumentService;
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

        #region CurrentEmployeeDocumentForm
        private EmployeeDocumentViewModel _CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel() { CreateDate = DateTime.Now };

        public EmployeeDocumentViewModel CurrentEmployeeDocumentForm
        {
            get { return _CurrentEmployeeDocumentForm; }
            set
            {
                if (_CurrentEmployeeDocumentForm != value)
                {
                    _CurrentEmployeeDocumentForm = value;
                    NotifyPropertyChanged("CurrentEmployeeDocumentForm");
                }
            }
        }
        #endregion

        #region CurrentEmployeeDocumentDG
        private EmployeeDocumentViewModel _CurrentEmployeeDocumentDG;

        public EmployeeDocumentViewModel CurrentEmployeeDocumentDG
        {
            get { return _CurrentEmployeeDocumentDG; }
            set
            {
                if (_CurrentEmployeeDocumentDG != value)
                {
                    _CurrentEmployeeDocumentDG = value;
                    NotifyPropertyChanged("CurrentEmployeeDocumentDG");
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

        public Employee_Document_AddEdit(EmployeeViewModel employee)
        {
            employeeService = DependencyResolver.Kernel.Get<IEmployeeService>();
            employeeDocumentService = DependencyResolver.Kernel.Get<IEmployeeDocumentService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentEmployee = employee;
            CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
            CurrentEmployeeDocumentForm.Identifier = Guid.NewGuid();
            CurrentEmployeeDocumentForm.CreateDate = DateTime.Now;
            CurrentEmployeeDocumentForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayEmployeeDocumentData());
            displayThread.IsBackground = true;
            displayThread.Start();

            txtDocumentName.Focus();
        }

        #endregion

        #region Display data

        public void DisplayEmployeeDocumentData()
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

        private void DgEmployeeDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
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

        private void btnAddDocument_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeDocumentForm.Name == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;


                CurrentEmployeeDocumentForm.Employee = CurrentEmployee;


                CurrentEmployeeDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentEmployeeDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                new EmployeeDocumentSQLiteRepository().Delete(CurrentEmployeeDocumentForm.Identifier);
                var response = new EmployeeDocumentSQLiteRepository().Create(CurrentEmployeeDocumentForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
                    CurrentEmployeeDocumentForm.Identifier = Guid.NewGuid();
                    CurrentEmployeeDocumentForm.CreateDate = DateTime.Now;
                    CurrentEmployeeDocumentForm.ItemStatus = ItemStatus.Added;
                    CurrentEmployeeDocumentForm.IsSynced = false;
                    return;
                }

                CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
                CurrentEmployeeDocumentForm.Identifier = Guid.NewGuid();
                CurrentEmployeeDocumentForm.CreateDate = DateTime.Now;
                CurrentEmployeeDocumentForm.ItemStatus = ItemStatus.Added;
                CurrentEmployeeDocumentForm.IsSynced = false;
                EmployeeCreatedUpdated();

                DisplayEmployeeDocumentData();

                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        txtDocumentName.Focus();
                    })
                );

                SubmitButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }
        private void btnEditDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
            CurrentEmployeeDocumentForm.Identifier = CurrentEmployeeDocumentDG.Identifier;
            CurrentEmployeeDocumentForm.ItemStatus = ItemStatus.Edited;

            CurrentEmployeeDocumentForm.IsSynced = CurrentEmployeeDocumentDG.IsSynced;
            CurrentEmployeeDocumentForm.Name = CurrentEmployeeDocumentDG.Name;
            CurrentEmployeeDocumentForm.CreateDate = CurrentEmployeeDocumentDG.CreateDate;
            CurrentEmployeeDocumentForm.Path = CurrentEmployeeDocumentDG.Path;
            CurrentEmployeeDocumentForm.UpdatedAt = CurrentEmployeeDocumentDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new EmployeeDocumentSQLiteRepository().SetStatusDeleted(CurrentEmployeeDocumentDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
                CurrentEmployeeDocumentForm.Identifier = Guid.NewGuid();
                CurrentEmployeeDocumentForm.CreateDate = DateTime.Now;
                CurrentEmployeeDocumentForm.ItemStatus = ItemStatus.Added;

                CurrentEmployeeDocumentDG = null;

                EmployeeCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayEmployeeDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
            CurrentEmployeeDocumentForm.Identifier = Guid.NewGuid();
            CurrentEmployeeDocumentForm.CreateDate = DateTime.Now;
            CurrentEmployeeDocumentForm.ItemStatus = ItemStatus.Added;
        }

        private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
            string[] fileNames = dialog.FileNames;

            if (fileNames.Length > 0)
            
                CurrentEmployeeDocumentForm.Path = fileNames[0];
                
        }

        private void btnChooseDocument_Click(object sender, RoutedEventArgs e)
        {
            DocumentSelectDialog dcpDialog = new DocumentSelectDialog();

            bool? result = dcpDialog.ShowDialog();

            if (result == true)
            {
                CurrentEmployeeDocumentForm.Path = dcpDialog?.SelectedDocument?.FullPath;
                CurrentEmployeeDocumentForm.Name = System.IO.Path.GetFileNameWithoutExtension(CurrentEmployeeDocumentForm?.Path ?? "");
            }
            //System.Windows.Forms.OpenFileDialog fileDIalog = new System.Windows.Forms.OpenFileDialog();

            //fileDIalog.Multiselect = true;
            //fileDIalog.FileOk += FileDIalog_FileOk;
            //fileDIalog.Filter = "All Files (*.*)|*.*";
            //fileDIalog.ShowDialog();
        }

        private void btnScahner_Click(object sender, RoutedEventArgs e)
        {
            Scanner_Window window = new Scanner_Window();
            bool? result = window.ShowDialog();

            if (result == true)
            {
                var path = window?.SelectedDocument;

                CurrentEmployeeDocumentForm.Path = path;
                var file = new FileInfo(CurrentEmployeeDocumentForm.Path);
                CurrentEmployeeDocumentForm.Name = System.IO.Path.GetFileNameWithoutExtension(CurrentEmployeeDocumentForm?.Path ?? "");
            }
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (EmployeeDocumentsFromDB == null || EmployeeDocumentsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentEmployee.EmployeeDocuments = EmployeeDocumentsFromDB;
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
