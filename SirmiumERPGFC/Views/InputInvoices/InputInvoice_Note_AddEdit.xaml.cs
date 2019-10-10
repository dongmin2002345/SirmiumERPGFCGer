using Ninject;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using ServiceWebApi.Implementations.Common.InputInvoices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.InputInvoices;
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

namespace SirmiumERPGFC.Views.InputInvoices
{
    /// <summary>
    /// Interaction logic for InputInvoice_Note_AddEdit.xaml
    /// </summary>
    public partial class InputInvoice_Note_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IInputInvoiceService inputInvoiceService;
        IInputInvoiceNoteService inputInvoiceNoteService;
        #endregion


        #region Event
        public event InputInvoiceHandler InputInvoiceCreatedUpdated;
        #endregion


        #region CurrentInputInvoice
        private InputInvoiceViewModel _CurrentInputInvoice = new InputInvoiceViewModel();

        public InputInvoiceViewModel CurrentInputInvoice
        {
            get { return _CurrentInputInvoice; }
            set
            {
                if (_CurrentInputInvoice != value)
                {
                    _CurrentInputInvoice = value;
                    NotifyPropertyChanged("CurrentInputInvoice");
                }
            }
        }
        #endregion


        #region InputInvoiceNotesFromDB
        private ObservableCollection<InputInvoiceNoteViewModel> _InputInvoiceNotesFromDB;

        public ObservableCollection<InputInvoiceNoteViewModel> InputInvoiceNotesFromDB
        {
            get { return _InputInvoiceNotesFromDB; }
            set
            {
                if (_InputInvoiceNotesFromDB != value)
                {
                    _InputInvoiceNotesFromDB = value;
                    NotifyPropertyChanged("InputInvoiceNotesFromDB");
                }
            }
        }
        #endregion

        #region CurrentInputInvoiceNoteForm
        private InputInvoiceNoteViewModel _CurrentInputInvoiceNoteForm = new InputInvoiceNoteViewModel() { NoteDate = DateTime.Now };

        public InputInvoiceNoteViewModel CurrentInputInvoiceNoteForm
        {
            get { return _CurrentInputInvoiceNoteForm; }
            set
            {
                if (_CurrentInputInvoiceNoteForm != value)
                {
                    _CurrentInputInvoiceNoteForm = value;
                    NotifyPropertyChanged("CurrentInputInvoiceNoteForm");
                }
            }
        }
        #endregion

        #region CurrentInputInvoiceNoteDG
        private InputInvoiceNoteViewModel _CurrentInputInvoiceNoteDG;

        public InputInvoiceNoteViewModel CurrentInputInvoiceNoteDG
        {
            get { return _CurrentInputInvoiceNoteDG; }
            set
            {
                if (_CurrentInputInvoiceNoteDG != value)
                {
                    _CurrentInputInvoiceNoteDG = value;
                    NotifyPropertyChanged("CurrentInputInvoiceNoteDG");
                }
            }
        }
        #endregion

        #region InputInvoiceNoteDataLoading
        private bool _InputInvoiceNoteDataLoading;

        public bool InputInvoiceNoteDataLoading
        {
            get { return _InputInvoiceNoteDataLoading; }
            set
            {
                if (_InputInvoiceNoteDataLoading != value)
                {
                    _InputInvoiceNoteDataLoading = value;
                    NotifyPropertyChanged("InputInvoiceNoteDataLoading");
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

        public InputInvoice_Note_AddEdit(InputInvoiceViewModel inputInvoice)
        {
            inputInvoiceService = DependencyResolver.Kernel.Get<IInputInvoiceService>();
            inputInvoiceNoteService = DependencyResolver.Kernel.Get<IInputInvoiceNoteService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentInputInvoice = inputInvoice;
            CurrentInputInvoiceNoteForm = new InputInvoiceNoteViewModel();
            CurrentInputInvoiceNoteForm.Identifier = Guid.NewGuid();
            CurrentInputInvoiceNoteForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayInputInvoiceNoteData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayInputInvoiceNoteData()
        {
            InputInvoiceNoteDataLoading = true;

            InputInvoiceNoteListResponse response = new InputInvoiceNoteSQLiteRepository()
                .GetInputInvoiceNotesByInputInvoice(MainWindow.CurrentCompanyId, CurrentInputInvoice.Identifier);

            if (response.Success)
            {
                InputInvoiceNotesFromDB = new ObservableCollection<InputInvoiceNoteViewModel>(
                    response.InputInvoiceNotes ?? new List<InputInvoiceNoteViewModel>());
            }
            else
            {
                InputInvoiceNotesFromDB = new ObservableCollection<InputInvoiceNoteViewModel>();
            }

            InputInvoiceNoteDataLoading = false;
        }

        private void DgInputInvoiceNotes_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentInputInvoiceNoteForm.Note == null)
            {
                MainWindow.ErrorMessage = "Obavezno polje: Napomena";
                return;
            }

            #endregion

            new InputInvoiceNoteSQLiteRepository().Delete(CurrentInputInvoiceNoteForm.Identifier);

            CurrentInputInvoiceNoteForm.InputInvoice = CurrentInputInvoice;

            CurrentInputInvoiceNoteForm.IsSynced = false;
            CurrentInputInvoiceNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentInputInvoiceNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new InputInvoiceNoteSQLiteRepository().Create(CurrentInputInvoiceNoteForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentInputInvoiceNoteForm = new InputInvoiceNoteViewModel();
                CurrentInputInvoiceNoteForm.Identifier = Guid.NewGuid();
                CurrentInputInvoiceNoteForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentInputInvoiceNoteForm = new InputInvoiceNoteViewModel();
            CurrentInputInvoiceNoteForm.Identifier = Guid.NewGuid();
            CurrentInputInvoiceNoteForm.ItemStatus = ItemStatus.Added;

            InputInvoiceCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayInputInvoiceNoteData());
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
            CurrentInputInvoiceNoteForm = new InputInvoiceNoteViewModel();
            CurrentInputInvoiceNoteForm.Identifier = CurrentInputInvoiceNoteDG.Identifier;
            CurrentInputInvoiceNoteForm.ItemStatus = ItemStatus.Edited;

            CurrentInputInvoiceNoteForm.Note = CurrentInputInvoiceNoteDG.Note;
            CurrentInputInvoiceNoteForm.NoteDate = CurrentInputInvoiceNoteDG.NoteDate;
            
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new InputInvoiceNoteSQLiteRepository().SetStatusDeleted(CurrentInputInvoiceNoteDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentInputInvoiceNoteForm = new InputInvoiceNoteViewModel();
                CurrentInputInvoiceNoteForm.Identifier = Guid.NewGuid();
                CurrentInputInvoiceNoteForm.ItemStatus = ItemStatus.Added;

                CurrentInputInvoiceNoteDG = null;

                InputInvoiceCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayInputInvoiceNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentInputInvoiceNoteForm = new InputInvoiceNoteViewModel();
            CurrentInputInvoiceNoteForm.Identifier = Guid.NewGuid();
            CurrentInputInvoiceNoteForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (InputInvoiceNotesFromDB == null || InputInvoiceNotesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = "Ne postoje stavke za proknjižavanje!";
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentInputInvoice.InputInvoiceNotes = InputInvoiceNotesFromDB;
                InputInvoiceResponse response = inputInvoiceService.Create(CurrentInputInvoice);
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

                    InputInvoiceCreatedUpdated();

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
            InputInvoiceCreatedUpdated();

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
