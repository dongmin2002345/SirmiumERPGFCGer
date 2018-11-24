using Ninject;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.ToDos;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.ToDos;
using System;
using System.Collections.Generic;
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

namespace SirmiumERPGFC.Views.Home
{
    public partial class ToDoAddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IToDoService toDoService;
        #endregion


        #region Events
        public event ToDoHandler ToDoCreatedUpdated;
        #endregion


        #region CurrentToDo
        private ToDoViewModel _CurrentToDo;

        public ToDoViewModel CurrentToDo
        {
            get { return _CurrentToDo; }
            set
            {
                if (_CurrentToDo != value)
                {
                    _CurrentToDo = value;
                    NotifyPropertyChanged("CurrentToDo");
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

        #endregion

        #region Constructor

        public ToDoAddEdit(ToDoViewModel toDoViewModel, bool isCreateProcess, bool isPopup = false)
        {
            toDoService = DependencyResolver.Kernel.Get<IToDoService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentToDo = toDoViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;
        }

        #endregion

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            #region Validation

            if (String.IsNullOrEmpty(CurrentToDo.Name))
            {
                MainWindow.WarningMessage = "Obavezno polje: Ime grla";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SaveButtonContent = " Čuvanje u toku... ";
                SaveButtonEnabled = false;

                CurrentToDo.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentToDo.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentToDo.IsSynced = false;
                CurrentToDo.UpdatedAt = DateTime.Now;

                ToDoResponse response = new ToDoSQLiteRepository().Delete(CurrentToDo.Identifier);
                response = new ToDoSQLiteRepository().Create(CurrentToDo);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                    return;
                }

                response = toDoService.Create(CurrentToDo);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new ToDoSQLiteRepository().UpdateSyncStatus(response.ToDo.Identifier, response.ToDo.UpdatedAt, response.ToDo.Id, true);
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;

                    ToDoCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentToDo = new ToDoViewModel();
                        CurrentToDo.Identifier = Guid.NewGuid();
                        CurrentToDo.ToDoDate = DateTime.Now;

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txtName.Focus();
                            })
                        );
                    }
                    else
                    {
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
                }
            });
            th.IsBackground = true;
            th.Start();
            txtName.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (IsPopup)
                FlyoutHelper.CloseFlyoutPopup(this);
            else
                FlyoutHelper.CloseFlyout(this);
        }

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
    }
}
