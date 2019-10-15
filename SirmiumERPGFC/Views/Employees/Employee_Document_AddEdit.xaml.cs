﻿using Ninject;
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
    /// Interaction logic for Employee_Document_AddEdit.xaml
    /// </summary>
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
                MainWindow.ErrorMessage = "Obavezno polje: Naziv";
                return;
            }

            #endregion

            new EmployeeDocumentSQLiteRepository().Delete(CurrentEmployeeDocumentForm.Identifier);

            CurrentEmployeeDocumentForm.Employee = CurrentEmployee;

            CurrentEmployeeDocumentForm.IsSynced = false;
            CurrentEmployeeDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentEmployeeDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new EmployeeDocumentSQLiteRepository().Create(CurrentEmployeeDocumentForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
                CurrentEmployeeDocumentForm.Identifier = Guid.NewGuid();
                CurrentEmployeeDocumentForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
            CurrentEmployeeDocumentForm.Identifier = Guid.NewGuid();
            CurrentEmployeeDocumentForm.ItemStatus = ItemStatus.Added;

            EmployeeCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayEmployeeDocumentData());
            displayThread.IsBackground = true;
            displayThread.Start();

            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    txtDocumentName.Focus();
                })
            );

            SubmitButtonEnabled = true;
        }

        private void btnEditDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
            CurrentEmployeeDocumentForm.Identifier = CurrentEmployeeDocumentDG.Identifier;
            CurrentEmployeeDocumentForm.ItemStatus = ItemStatus.Edited;

            CurrentEmployeeDocumentForm.Name = CurrentEmployeeDocumentDG.Name;
            CurrentEmployeeDocumentForm.CreateDate = CurrentEmployeeDocumentDG.CreateDate;
            CurrentEmployeeDocumentForm.Path = CurrentEmployeeDocumentDG.Path;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new EmployeeDocumentSQLiteRepository().SetStatusDeleted(CurrentEmployeeDocumentDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentEmployeeDocumentForm = new EmployeeDocumentViewModel();
                CurrentEmployeeDocumentForm.Identifier = Guid.NewGuid();
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
            System.Windows.Forms.OpenFileDialog fileDIalog = new System.Windows.Forms.OpenFileDialog();

            fileDIalog.Multiselect = true;
            fileDIalog.FileOk += FileDIalog_FileOk;
            fileDIalog.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            fileDIalog.ShowDialog();
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (EmployeeDocumentsFromDB == null || EmployeeDocumentsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = "Ne postoje stavke za proknjižavanje!";
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