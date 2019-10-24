using Ninject;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Phonebooks;
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

namespace SirmiumERPGFC.Views.Phonebooks
{
    /// <summary>
    /// Interaction logic for Phonebook_Note_AddEdit.xaml
    /// </summary>
    public partial class Phonebook_Note_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhonebookService PhonebookService;
        IPhonebookNoteService PhonebookNoteService;
        #endregion


        #region Event
        public event PhonebookHandler PhonebookCreatedUpdated;
        #endregion


        #region CurrentPhonebook
        private PhonebookViewModel _CurrentPhonebook = new PhonebookViewModel();

        public PhonebookViewModel CurrentPhonebook
        {
            get { return _CurrentPhonebook; }
            set
            {
                if (_CurrentPhonebook != value)
                {
                    _CurrentPhonebook = value;
                    NotifyPropertyChanged("CurrentPhonebook");
                }
            }
        }
        #endregion


        #region PhonebookNotesFromDB
        private ObservableCollection<PhonebookNoteViewModel> _PhonebookNotesFromDB;

        public ObservableCollection<PhonebookNoteViewModel> PhonebookNotesFromDB
        {
            get { return _PhonebookNotesFromDB; }
            set
            {
                if (_PhonebookNotesFromDB != value)
                {
                    _PhonebookNotesFromDB = value;
                    NotifyPropertyChanged("PhonebookNotesFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhonebookNoteForm
        private PhonebookNoteViewModel _CurrentPhonebookNoteForm = new PhonebookNoteViewModel() { NoteDate = DateTime.Now };

        public PhonebookNoteViewModel CurrentPhonebookNoteForm
        {
            get { return _CurrentPhonebookNoteForm; }
            set
            {
                if (_CurrentPhonebookNoteForm != value)
                {
                    _CurrentPhonebookNoteForm = value;
                    NotifyPropertyChanged("CurrentPhonebookNoteForm");
                }
            }
        }
        #endregion

        #region CurrentPhonebookNoteDG
        private PhonebookNoteViewModel _CurrentPhonebookNoteDG;

        public PhonebookNoteViewModel CurrentPhonebookNoteDG
        {
            get { return _CurrentPhonebookNoteDG; }
            set
            {
                if (_CurrentPhonebookNoteDG != value)
                {
                    _CurrentPhonebookNoteDG = value;
                    NotifyPropertyChanged("CurrentPhonebookNoteDG");
                }
            }
        }
        #endregion

        #region PhonebookNoteDataLoading
        private bool _PhonebookNoteDataLoading;

        public bool PhonebookNoteDataLoading
        {
            get { return _PhonebookNoteDataLoading; }
            set
            {
                if (_PhonebookNoteDataLoading != value)
                {
                    _PhonebookNoteDataLoading = value;
                    NotifyPropertyChanged("PhonebookNoteDataLoading");
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

        public Phonebook_Note_AddEdit(PhonebookViewModel Phonebook)
        {
            PhonebookService = DependencyResolver.Kernel.Get<IPhonebookService>();
            PhonebookNoteService = DependencyResolver.Kernel.Get<IPhonebookNoteService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentPhonebook = Phonebook;
            CurrentPhonebookNoteForm = new PhonebookNoteViewModel();
            CurrentPhonebookNoteForm.Identifier = Guid.NewGuid();
            CurrentPhonebookNoteForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayPhonebookNoteData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayPhonebookNoteData()
        {
            PhonebookNoteDataLoading = true;

            PhonebookNoteListResponse response = new PhonebookNoteSQLiteRepository()
                .GetPhonebookNotesByPhonebook(MainWindow.CurrentCompanyId, CurrentPhonebook.Identifier);

            if (response.Success)
            {
                PhonebookNotesFromDB = new ObservableCollection<PhonebookNoteViewModel>(
                    response.PhonebookNotes ?? new List<PhonebookNoteViewModel>());
            }
            else
            {
                PhonebookNotesFromDB = new ObservableCollection<PhonebookNoteViewModel>();
            }

            PhonebookNoteDataLoading = false;
        }

        private void DgPhonebookNotes_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentPhonebookNoteForm.Note == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Napomena"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;


                CurrentPhonebookNoteForm.Phonebook = CurrentPhonebook;


                CurrentPhonebookNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentPhonebookNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                new PhonebookNoteSQLiteRepository().Delete(CurrentPhonebookNoteForm.Identifier);
                var response = new PhonebookNoteSQLiteRepository().Create(CurrentPhonebookNoteForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentPhonebookNoteForm = new PhonebookNoteViewModel();
                    CurrentPhonebookNoteForm.Identifier = Guid.NewGuid();
                    CurrentPhonebookNoteForm.ItemStatus = ItemStatus.Added;
                    CurrentPhonebookNoteForm.IsSynced = false;
                    return;
                }

                CurrentPhonebookNoteForm = new PhonebookNoteViewModel();
                CurrentPhonebookNoteForm.Identifier = Guid.NewGuid();
                CurrentPhonebookNoteForm.ItemStatus = ItemStatus.Added;
                CurrentPhonebookNoteForm.IsSynced = false;
                PhonebookCreatedUpdated();

                DisplayPhonebookNoteData();

                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        txtNote.Focus();
                    })
                );

                SubmitButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }


        private void btnEditNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhonebookNoteForm = new PhonebookNoteViewModel();
            CurrentPhonebookNoteForm.Identifier = CurrentPhonebookNoteDG.Identifier;
            CurrentPhonebookNoteForm.ItemStatus = ItemStatus.Edited;

            CurrentPhonebookNoteForm.IsSynced = CurrentPhonebookNoteDG.IsSynced;
            CurrentPhonebookNoteForm.Note = CurrentPhonebookNoteDG.Note;
            CurrentPhonebookNoteForm.NoteDate = CurrentPhonebookNoteDG.NoteDate;
            CurrentPhonebookNoteForm.UpdatedAt = CurrentPhonebookNoteDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new PhonebookNoteSQLiteRepository().SetStatusDeleted(CurrentPhonebookNoteDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentPhonebookNoteForm = new PhonebookNoteViewModel();
                CurrentPhonebookNoteForm.Identifier = Guid.NewGuid();
                CurrentPhonebookNoteForm.ItemStatus = ItemStatus.Added;

                CurrentPhonebookNoteDG = null;

                PhonebookCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayPhonebookNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhonebookNoteForm = new PhonebookNoteViewModel();
            CurrentPhonebookNoteForm.Identifier = Guid.NewGuid();
            CurrentPhonebookNoteForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (PhonebookNotesFromDB == null || PhonebookNotesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentPhonebook.PhonebookNotes = PhonebookNotesFromDB;
                PhonebookResponse response = PhonebookService.Create(CurrentPhonebook);
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

                    PhonebookCreatedUpdated();

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
            PhonebookCreatedUpdated();

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
