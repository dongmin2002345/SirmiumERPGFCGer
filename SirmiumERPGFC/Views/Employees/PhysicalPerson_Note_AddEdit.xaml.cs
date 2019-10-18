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
    /// Interaction logic for PhysicalPerson_Note_AddEdit.xaml
    /// </summary>
    public partial class PhysicalPerson_Note_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhysicalPersonService physicalPersonService;
        IPhysicalPersonNoteService physicalPersonNoteService;
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


        #region PhysicalPersonNotesFromDB
        private ObservableCollection<PhysicalPersonNoteViewModel> _PhysicalPersonNotesFromDB;

        public ObservableCollection<PhysicalPersonNoteViewModel> PhysicalPersonNotesFromDB
        {
            get { return _PhysicalPersonNotesFromDB; }
            set
            {
                if (_PhysicalPersonNotesFromDB != value)
                {
                    _PhysicalPersonNotesFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonNotesFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonNoteForm
        private PhysicalPersonNoteViewModel _CurrentPhysicalPersonNoteForm = new PhysicalPersonNoteViewModel() { NoteDate = DateTime.Now };

        public PhysicalPersonNoteViewModel CurrentPhysicalPersonNoteForm
        {
            get { return _CurrentPhysicalPersonNoteForm; }
            set
            {
                if (_CurrentPhysicalPersonNoteForm != value)
                {
                    _CurrentPhysicalPersonNoteForm = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonNoteForm");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonNoteDG
        private PhysicalPersonNoteViewModel _CurrentPhysicalPersonNoteDG;

        public PhysicalPersonNoteViewModel CurrentPhysicalPersonNoteDG
        {
            get { return _CurrentPhysicalPersonNoteDG; }
            set
            {
                if (_CurrentPhysicalPersonNoteDG != value)
                {
                    _CurrentPhysicalPersonNoteDG = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonNoteDG");
                }
            }
        }
        #endregion

        #region PhysicalPersonNoteDataLoading
        private bool _PhysicalPersonNoteDataLoading;

        public bool PhysicalPersonNoteDataLoading
        {
            get { return _PhysicalPersonNoteDataLoading; }
            set
            {
                if (_PhysicalPersonNoteDataLoading != value)
                {
                    _PhysicalPersonNoteDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonNoteDataLoading");
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

        public PhysicalPerson_Note_AddEdit(PhysicalPersonViewModel physicalPerson)
        {
            physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();
            physicalPersonNoteService = DependencyResolver.Kernel.Get<IPhysicalPersonNoteService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentPhysicalPerson = physicalPerson;
            CurrentPhysicalPersonNoteForm = new PhysicalPersonNoteViewModel();
            CurrentPhysicalPersonNoteForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonNoteForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayPhysicalPersonNoteData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayPhysicalPersonNoteData()
        {
            PhysicalPersonNoteDataLoading = true;

            PhysicalPersonNoteListResponse response = new PhysicalPersonNoteSQLiteRepository()
                .GetPhysicalPersonNotesByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonNotesFromDB = new ObservableCollection<PhysicalPersonNoteViewModel>(
                    response.PhysicalPersonNotes ?? new List<PhysicalPersonNoteViewModel>());
            }
            else
            {
                PhysicalPersonNotesFromDB = new ObservableCollection<PhysicalPersonNoteViewModel>();
            }

            PhysicalPersonNoteDataLoading = false;
        }

        private void DgPhysicalPersonNotes_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentPhysicalPersonNoteForm.Note == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Napomena"));
                return;
            }

            #endregion

            new PhysicalPersonNoteSQLiteRepository().Delete(CurrentPhysicalPersonNoteForm.Identifier);

            CurrentPhysicalPersonNoteForm.PhysicalPerson = CurrentPhysicalPerson;

            CurrentPhysicalPersonNoteForm.IsSynced = false;
            CurrentPhysicalPersonNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentPhysicalPersonNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new PhysicalPersonNoteSQLiteRepository().Create(CurrentPhysicalPersonNoteForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentPhysicalPersonNoteForm = new PhysicalPersonNoteViewModel();
                CurrentPhysicalPersonNoteForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonNoteForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentPhysicalPersonNoteForm = new PhysicalPersonNoteViewModel();
            CurrentPhysicalPersonNoteForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonNoteForm.ItemStatus = ItemStatus.Added;

            PhysicalPersonCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayPhysicalPersonNoteData());
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
            CurrentPhysicalPersonNoteForm = new PhysicalPersonNoteViewModel();
            CurrentPhysicalPersonNoteForm.Identifier = CurrentPhysicalPersonNoteDG.Identifier;
            CurrentPhysicalPersonNoteForm.ItemStatus = ItemStatus.Edited;

            CurrentPhysicalPersonNoteForm.Note = CurrentPhysicalPersonNoteDG.Note;
            CurrentPhysicalPersonNoteForm.NoteDate = CurrentPhysicalPersonNoteDG.NoteDate;

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new PhysicalPersonNoteSQLiteRepository().SetStatusDeleted(CurrentPhysicalPersonNoteDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentPhysicalPersonNoteForm = new PhysicalPersonNoteViewModel();
                CurrentPhysicalPersonNoteForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonNoteForm.ItemStatus = ItemStatus.Added;

                CurrentPhysicalPersonNoteDG = null;

                PhysicalPersonCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayPhysicalPersonNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonNoteForm = new PhysicalPersonNoteViewModel();
            CurrentPhysicalPersonNoteForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonNoteForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (PhysicalPersonNotesFromDB == null || PhysicalPersonNotesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Ne_postoje_stavke_za_proknjižavanje"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentPhysicalPerson.PhysicalPersonNotes = PhysicalPersonNotesFromDB;
                PhysicalPersonResponse response = physicalPersonService.Create(CurrentPhysicalPerson);
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
    }
}
