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
    /// Interaction logic for PhysicalPerson_Document_AddEdit.xaml
    /// </summary>
    public partial class PhysicalPerson_Document_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhysicalPersonService physicalPersonService;
        IPhysicalPersonDocumentService physicalPersonNoteService;
        #endregion


        #region Event
        public event PhysicalPersonHandler PhysicalPersonCreatedUpdated;
        #endregion


        #region CurrentPhysicalPerson
        private PhysicalPersonViewModel _CurrentPhysicalPerson = new PhysicalPersonViewModel();

        public PhysicalPersonViewModel CurrentPhysicalPerson
        {
            get { return _CurrentPhysicalPerson; }
            set
            {
                if (_CurrentPhysicalPerson != value)
                {
                    _CurrentPhysicalPerson = value;
                    NotifyPropertyChanged("CurrentPhysicalPerson");
                }
            }
        }
        #endregion


        #region PhysicalPersonDocumentsFromDB
        private ObservableCollection<PhysicalPersonDocumentViewModel> _PhysicalPersonDocumentsFromDB;

        public ObservableCollection<PhysicalPersonDocumentViewModel> PhysicalPersonDocumentsFromDB
        {
            get { return _PhysicalPersonDocumentsFromDB; }
            set
            {
                if (_PhysicalPersonDocumentsFromDB != value)
                {
                    _PhysicalPersonDocumentsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonDocumentForm
        private PhysicalPersonDocumentViewModel _CurrentPhysicalPersonDocumentForm = new PhysicalPersonDocumentViewModel() { CreateDate = DateTime.Now };

        public PhysicalPersonDocumentViewModel CurrentPhysicalPersonDocumentForm
        {
            get { return _CurrentPhysicalPersonDocumentForm; }
            set
            {
                if (_CurrentPhysicalPersonDocumentForm != value)
                {
                    _CurrentPhysicalPersonDocumentForm = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonDocumentForm");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonDocumentDG
        private PhysicalPersonDocumentViewModel _CurrentPhysicalPersonDocumentDG;

        public PhysicalPersonDocumentViewModel CurrentPhysicalPersonDocumentDG
        {
            get { return _CurrentPhysicalPersonDocumentDG; }
            set
            {
                if (_CurrentPhysicalPersonDocumentDG != value)
                {
                    _CurrentPhysicalPersonDocumentDG = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonDocumentDG");
                }
            }
        }
        #endregion

        #region PhysicalPersonDocumentDataLoading
        private bool _PhysicalPersonDocumentDataLoading;

        public bool PhysicalPersonDocumentDataLoading
        {
            get { return _PhysicalPersonDocumentDataLoading; }
            set
            {
                if (_PhysicalPersonDocumentDataLoading != value)
                {
                    _PhysicalPersonDocumentDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonDocumentDataLoading");
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

        public PhysicalPerson_Document_AddEdit(PhysicalPersonViewModel physicalPerson)
        {
            physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();
            physicalPersonNoteService = DependencyResolver.Kernel.Get<IPhysicalPersonDocumentService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentPhysicalPerson = physicalPerson;
            CurrentPhysicalPersonDocumentForm = new PhysicalPersonDocumentViewModel();
            CurrentPhysicalPersonDocumentForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonDocumentForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayPhysicalPersonDocumentData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayPhysicalPersonDocumentData()
        {
            PhysicalPersonDocumentDataLoading = true;

            PhysicalPersonDocumentListResponse response = new PhysicalPersonDocumentSQLiteRepository()
                .GetPhysicalPersonDocumentsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonDocumentsFromDB = new ObservableCollection<PhysicalPersonDocumentViewModel>(
                    response.PhysicalPersonDocuments ?? new List<PhysicalPersonDocumentViewModel>());
            }
            else
            {
                PhysicalPersonDocumentsFromDB = new ObservableCollection<PhysicalPersonDocumentViewModel>();
            }

            PhysicalPersonDocumentDataLoading = false;
        }

        private void DgPhysicalPersonDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentPhysicalPersonDocumentForm.Name == null)
            {
                MainWindow.ErrorMessage = "Obavezno polje: Naziv";
                return;
            }

            #endregion

            new PhysicalPersonDocumentSQLiteRepository().Delete(CurrentPhysicalPersonDocumentForm.Identifier);

            CurrentPhysicalPersonDocumentForm.PhysicalPerson = CurrentPhysicalPerson;

            CurrentPhysicalPersonDocumentForm.IsSynced = false;
            CurrentPhysicalPersonDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentPhysicalPersonDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new PhysicalPersonDocumentSQLiteRepository().Create(CurrentPhysicalPersonDocumentForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentPhysicalPersonDocumentForm = new PhysicalPersonDocumentViewModel();
                CurrentPhysicalPersonDocumentForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonDocumentForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentPhysicalPersonDocumentForm = new PhysicalPersonDocumentViewModel();
            CurrentPhysicalPersonDocumentForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonDocumentForm.ItemStatus = ItemStatus.Added;

            PhysicalPersonCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayPhysicalPersonDocumentData());
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
            CurrentPhysicalPersonDocumentForm = new PhysicalPersonDocumentViewModel();
            CurrentPhysicalPersonDocumentForm.Identifier = CurrentPhysicalPersonDocumentDG.Identifier;
            CurrentPhysicalPersonDocumentForm.ItemStatus = ItemStatus.Edited;

            CurrentPhysicalPersonDocumentForm.Name = CurrentPhysicalPersonDocumentDG.Name;
            CurrentPhysicalPersonDocumentForm.Path = CurrentPhysicalPersonDocumentDG.Path;
            CurrentPhysicalPersonDocumentForm.CreateDate = CurrentPhysicalPersonDocumentDG.CreateDate;

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new PhysicalPersonDocumentSQLiteRepository().SetStatusDeleted(CurrentPhysicalPersonDocumentDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = "Stavka je uspešno obrisana!";

                CurrentPhysicalPersonDocumentForm = new PhysicalPersonDocumentViewModel();
                CurrentPhysicalPersonDocumentForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonDocumentForm.ItemStatus = ItemStatus.Added;

                CurrentPhysicalPersonDocumentDG = null;

                PhysicalPersonCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayPhysicalPersonDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonDocumentForm = new PhysicalPersonDocumentViewModel();
            CurrentPhysicalPersonDocumentForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonDocumentForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (PhysicalPersonDocumentsFromDB == null || PhysicalPersonDocumentsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = "Ne postoje stavke za proknjižavanje!";
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = " Čuvanje u toku... ";
                SubmitButtonEnabled = false;

                CurrentPhysicalPerson.PhysicalPersonDocuments = PhysicalPersonDocumentsFromDB;
                PhysicalPersonResponse response = physicalPersonService.Create(CurrentPhysicalPerson);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod čuvanja podataka!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;

                    PhysicalPersonCreatedUpdated();

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
            PhysicalPersonCreatedUpdated();

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
        private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
            string[] fileNames = dialog.FileNames;

            if (fileNames.Length > 0)
                CurrentPhysicalPersonDocumentForm.Path = fileNames[0];
        }
        private void btnChooseDocument_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDIalog = new System.Windows.Forms.OpenFileDialog();

            fileDIalog.Multiselect = true;
            fileDIalog.FileOk += FileDIalog_FileOk;
            fileDIalog.Filter = "All Files (*.*)|*.*";
            fileDIalog.ShowDialog();
        }
    }
}
