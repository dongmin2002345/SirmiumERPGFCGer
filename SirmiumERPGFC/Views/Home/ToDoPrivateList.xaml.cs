using Ninject;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.ToDos;
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

namespace SirmiumERPGFC.Views.Home
{
    public partial class ToDoPrivateList : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IToDoService toDoService;
        #endregion


        #region ToDosFromDB
        private ObservableCollection<ToDoViewModel> _ToDosFromDB;

        public ObservableCollection<ToDoViewModel> ToDosFromDB
        {
            get { return _ToDosFromDB; }
            set
            {
                if (_ToDosFromDB != value)
                {
                    _ToDosFromDB = value;
                    NotifyPropertyChanged("ToDosFromDB");
                }
            }
        }
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

                    if (_CurrentToDo != null)
                    {
                        txtDescription.Text = _CurrentToDo.Description;
                    }
                    else
                        txtDescription.Text = "";
                }
            }
        }
        #endregion


        #region RefreshButtonContent
        private string _RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

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

        public ToDoPrivateList()
        {
            toDoService = DependencyResolver.Kernel.Get<IToDoService>();

            InitializeComponent();

            this.DataContext = this;
        }

        #region Display data

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Thread syncThread = new Thread(() =>
            {
                SyncData();

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sinhronizovaniUzvičnik"));
            });
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        public void DisplayData()
        {
            ToDoListResponse response = new ToDoSQLiteRepository()
               .GetPrivateToDos(MainWindow.CurrentCompanyId, "");
            
            if (response.Success)
            {
                ToDosFromDB = new ObservableCollection<ToDoViewModel>(response.ToDos?.OrderBy(x => x.ToDoDate)?.ToList() ?? new List<ToDoViewModel>());
            }
            else
            {
                ToDosFromDB = new ObservableCollection<ToDoViewModel>();
                MainWindow.ErrorMessage = response.Message;
            }
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = ((string)Application.Current.FindResource("Podsetnici_TriTacke"));
            new ToDoSQLiteRepository().Sync(toDoService);

            DisplayData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
        }

        #endregion

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentToDo == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_brisanjeUzvičnik"));
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("stavku ", CurrentToDo.Name);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                ToDoResponse response = toDoService.Delete(CurrentToDo.Identifier);
                if (!response.Success)
                {
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
                }

                response = new ToDoSQLiteRepository().Delete(CurrentToDo.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                Thread displayThread = new Thread(() => SyncData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ToDoViewModel toDo = new ToDoViewModel();
            toDo.Identifier = Guid.NewGuid();
            toDo.ToDoDate = DateTime.Now;
            toDo.IsPrivate = true;

            ToDoAddEdit addEditForm = new ToDoAddEdit(toDo, true, true);
            addEditForm.ToDoCreatedUpdated += new ToDoHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podsetnik", 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ToDoAddEdit addEditForm = new ToDoAddEdit(CurrentToDo, true, false);
            addEditForm.ToDoCreatedUpdated += new ToDoHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podsetnik", 95, addEditForm);
        }

        private void btnShowDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentToDo.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            Thread displayThread = new Thread(() => SyncData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }
    }
}
