using Ninject;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using ServiceWebApi.Implementations.Common.OutputInvoices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Identity;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.OutputInvoices;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using WpfAppCommonCode.Converters;

namespace SirmiumERPGFC.Views.OutputInvoices
{
    /// <summary>
    /// Interaction logic for BusinessPartnerAddEdit.xaml
    /// </summary>
    public partial class OutputInvoiceAddEdit : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Event for handling output invoice create and update
        /// </summary>
        public event OutputInvoiceHandler OutputInvoiceCreatedUpdated;

        /// <summary>
        /// Service for accessing output invoice
        /// </summary>
        IOutputInvoiceService outputInvoiceService;

        #region CurrentOutputInvoice
        private OutputInvoiceViewModel _CurrentOutputInvoice;

        public OutputInvoiceViewModel CurrentOutputInvoice
        {
            get { return _CurrentOutputInvoice; }
            set
            {
                if (_CurrentOutputInvoice != value)
                {
                    _CurrentOutputInvoice = value;
                    NotifyPropertyChanged("CurrentOutputInvoice");
                }
            }
        }
        #endregion

        #region StatusOptions
        public ObservableCollection<String> StatusOptions
        {
            get
            {
                return new ObservableCollection<String>(new List<string>() {
                           ChooseStatusConverter.Choose,
                           ChooseStatusConverter.ChooseO,
                           ChooseStatusConverter.ChooseB,
                           ChooseStatusConverter.ChooseM,
                });
            }
        }
        #endregion


        #region OutputInvoiceNotesFromDB
        private ObservableCollection<OutputInvoiceNoteViewModel> _OutputInvoiceNotesFromDB;

        public ObservableCollection<OutputInvoiceNoteViewModel> OutputInvoiceNotesFromDB
        {
            get { return _OutputInvoiceNotesFromDB; }
            set
            {
                if (_OutputInvoiceNotesFromDB != value)
                {
                    _OutputInvoiceNotesFromDB = value;
                    NotifyPropertyChanged("OutputInvoiceNotesFromDB");
                }
            }
        }
        #endregion

        #region CurrentOutputInvoiceNoteForm
        private OutputInvoiceNoteViewModel _CurrentOutputInvoiceNoteForm = new OutputInvoiceNoteViewModel() { NoteDate = DateTime.Now };

        public OutputInvoiceNoteViewModel CurrentOutputInvoiceNoteForm
        {
            get { return _CurrentOutputInvoiceNoteForm; }
            set
            {
                if (_CurrentOutputInvoiceNoteForm != value)
                {
                    _CurrentOutputInvoiceNoteForm = value;
                    NotifyPropertyChanged("CurrentOutputInvoiceNoteForm");
                }
            }
        }
        #endregion

        #region CurrentOutputInvoiceNoteDG
        private OutputInvoiceNoteViewModel _CurrentOutputInvoiceNoteDG;

        public OutputInvoiceNoteViewModel CurrentOutputInvoiceNoteDG
        {
            get { return _CurrentOutputInvoiceNoteDG; }
            set
            {
                if (_CurrentOutputInvoiceNoteDG != value)
                {
                    _CurrentOutputInvoiceNoteDG = value;
                    NotifyPropertyChanged("CurrentOutputInvoiceNoteDG");
                }
            }
        }
        #endregion

        #region OutputInvoiceNoteDataLoading
        private bool _OutputInvoiceNoteDataLoading;

        public bool OutputInvoiceNoteDataLoading
        {
            get { return _OutputInvoiceNoteDataLoading; }
            set
            {
                if (_OutputInvoiceNoteDataLoading != value)
                {
                    _OutputInvoiceNoteDataLoading = value;
                    NotifyPropertyChanged("OutputInvoiceNoteDataLoading");
                }
            }
        }
        #endregion

        #region OutputInvoiceDocumentsFromDB
        private ObservableCollection<OutputInvoiceDocumentViewModel> _OutputInvoiceDocumentsFromDB;

        public ObservableCollection<OutputInvoiceDocumentViewModel> OutputInvoiceDocumentsFromDB
        {
            get { return _OutputInvoiceDocumentsFromDB; }
            set
            {
                if (_OutputInvoiceDocumentsFromDB != value)
                {
                    _OutputInvoiceDocumentsFromDB = value;
                    NotifyPropertyChanged("OutputInvoiceDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentOutputInvoiceDocumentForm
        private OutputInvoiceDocumentViewModel _CurrentOutputInvoiceDocumentForm = new OutputInvoiceDocumentViewModel() { CreateDate = DateTime.Now };

        public OutputInvoiceDocumentViewModel CurrentOutputInvoiceDocumentForm
        {
            get { return _CurrentOutputInvoiceDocumentForm; }
            set
            {
                if (_CurrentOutputInvoiceDocumentForm != value)
                {
                    _CurrentOutputInvoiceDocumentForm = value;
                    NotifyPropertyChanged("CurrentOutputInvoiceDocumentForm");
                }
            }
        }
        #endregion

        #region CurrentOutputInvoiceDocumentDG
        private OutputInvoiceDocumentViewModel _CurrentOutputInvoiceDocumentDG;

        public OutputInvoiceDocumentViewModel CurrentOutputInvoiceDocumentDG
        {
            get { return _CurrentOutputInvoiceDocumentDG; }
            set
            {
                if (_CurrentOutputInvoiceDocumentDG != value)
                {
                    _CurrentOutputInvoiceDocumentDG = value;
                    NotifyPropertyChanged("CurrentOutputInvoiceDocumentDG");
                }
            }
        }
        #endregion

        #region OutputInvoiceDocumentDataLoading
        private bool _OutputInvoiceDocumentDataLoading;

        public bool OutputInvoiceDocumentDataLoading
        {
            get { return _OutputInvoiceDocumentDataLoading; }
            set
            {
                if (_OutputInvoiceDocumentDataLoading != value)
                {
                    _OutputInvoiceDocumentDataLoading = value;
                    NotifyPropertyChanged("OutputInvoiceDocumentDataLoading");
                }
            }
        }
        #endregion


        #region IsHeaderCreated
        private bool _IsHeaderCreated;

        public bool IsHeaderCreated
        {
            get { return _IsHeaderCreated; }
            set
            {
                if (_IsHeaderCreated != value)
                {
                    _IsHeaderCreated = value;
                    NotifyPropertyChanged("IsHeaderCreated");
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



        #region Constructor

        public OutputInvoiceAddEdit(OutputInvoiceViewModel OutputInvoiceViewModel, bool isCreateProcess, bool isPopup = false)
        {
            // Initialize service
            outputInvoiceService = DependencyResolver.Kernel.Get<IOutputInvoiceService>();

            InitializeComponent();

            this.DataContext = this;

            IsHeaderCreated = !isCreateProcess;

            CurrentOutputInvoice = OutputInvoiceViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;

            Thread displayThread = new Thread(() =>
            {
                DisplayOutputInvoiceNoteData();
            });
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void DisplayOutputInvoiceNoteData()
        {
            OutputInvoiceNoteDataLoading = true;

            OutputInvoiceNoteListResponse response = new OutputInvoiceNoteSQLiteRepository()
                .GetOutputInvoiceNotesByOutputInvoice(MainWindow.CurrentCompanyId, CurrentOutputInvoice.Identifier);

            if (response.Success)
            {
                OutputInvoiceNotesFromDB = new ObservableCollection<OutputInvoiceNoteViewModel>(
                    response.OutputInvoiceNotes ?? new List<OutputInvoiceNoteViewModel>());
            }
            else
            {
                OutputInvoiceNotesFromDB = new ObservableCollection<OutputInvoiceNoteViewModel>();
            }

            OutputInvoiceNoteDataLoading = false;
        }

        private void DisplayOutputInvoiceDocumentData()
        {
            OutputInvoiceDocumentDataLoading = true;

            OutputInvoiceDocumentListResponse response = new OutputInvoiceDocumentSQLiteRepository()
                .GetOutputInvoiceDocumentsByOutputInvoice(MainWindow.CurrentCompanyId, CurrentOutputInvoice.Identifier);

            if (response.Success)
            {
                OutputInvoiceDocumentsFromDB = new ObservableCollection<OutputInvoiceDocumentViewModel>(
                    response.OutputInvoiceDocuments ?? new List<OutputInvoiceDocumentViewModel>());
            }
            else
            {
                OutputInvoiceDocumentsFromDB = new ObservableCollection<OutputInvoiceDocumentViewModel>();
            }

            OutputInvoiceDocumentDataLoading = false;
        }

        #endregion

        #region Save and Cancel button

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentOutputInvoice.Supplier))
            {
                MainWindow.WarningMessage = "Obavezno polje: Naziv dobavljača";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SaveButtonContent = " Čuvanje u toku... ";
                SaveButtonEnabled = false;

                CurrentOutputInvoice.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentOutputInvoice.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentOutputInvoice.IsSynced = false;

                OutputInvoiceResponse response = new OutputInvoiceSQLiteRepository().Delete(CurrentOutputInvoice.Identifier);
                response = new OutputInvoiceSQLiteRepository().Create(CurrentOutputInvoice);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                    return;
                }

                CurrentOutputInvoice.OutputInvoiceNotes = OutputInvoiceNotesFromDB;
                CurrentOutputInvoice.OutputInvoiceDocuments = OutputInvoiceDocumentsFromDB;

                response = outputInvoiceService.Create(CurrentOutputInvoice);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new OutputInvoiceSQLiteRepository().UpdateSyncStatus(
                        response.OutputInvoice.Identifier,
                        response.OutputInvoice.Code,
                        response.OutputInvoice.UpdatedAt,
                        response.OutputInvoice.Id, 
                        true);

                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;

                    OutputInvoiceCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentOutputInvoice = new OutputInvoiceViewModel();
                        CurrentOutputInvoice.Identifier = Guid.NewGuid();

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txtOutoutInvoiceCode.Focus();
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
                                    FlyoutHelper.CloseFlyout(this);
                                else
                                    FlyoutHelper.CloseFlyout(this);
                            })
                        );
                    }
                }

            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (IsPopup)
                FlyoutHelper.CloseFlyout(this);
            else
                FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        #region Add, edit, delete and cancel note

        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentOutputInvoiceNoteForm.Note))
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Napomena"));
                return;
            }

            if (CurrentOutputInvoiceNoteForm.NoteDate == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Datum_napomene"));
                return;
            }

            #endregion

            // IF update process, first delete item
            new OutputInvoiceNoteSQLiteRepository().Delete(CurrentOutputInvoiceNoteForm.Identifier);

            CurrentOutputInvoiceNoteForm.OutputInvoice = CurrentOutputInvoice;
            CurrentOutputInvoiceNoteForm.Identifier = Guid.NewGuid();
            CurrentOutputInvoiceNoteForm.IsSynced = false;
            CurrentOutputInvoiceNoteForm.UpdatedAt = DateTime.Now;
            CurrentOutputInvoiceNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentOutputInvoiceNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new OutputInvoiceNoteSQLiteRepository().Create(CurrentOutputInvoiceNoteForm);
            if (response.Success)
            {
                CurrentOutputInvoiceNoteForm = new OutputInvoiceNoteViewModel();

                Thread displayThread = new Thread(() => DisplayOutputInvoiceNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtNote.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnEditNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentOutputInvoiceNoteForm = CurrentOutputInvoiceNoteDG;
        }

        private void btnDeleteNote_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation(((string)Application.Current.FindResource("stavku_radnika")), "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new OutputInvoiceNoteSQLiteRepository().Delete(CurrentOutputInvoiceNoteDG.Identifier);

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_radnika_je_uspešno_obrisanaUzvičnik"));

                Thread displayThread = new Thread(() => DisplayOutputInvoiceNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentOutputInvoiceNoteForm = new OutputInvoiceNoteViewModel();
        }

        #endregion

        #region Add, edit, delete and cancel document

        private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
            string[] fileNames = dialog.FileNames;

            if (fileNames.Length > 0)
                CurrentOutputInvoiceDocumentForm.Path = fileNames[0];
        }

        private void btnChooseDocument_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDIalog = new System.Windows.Forms.OpenFileDialog();

            fileDIalog.Multiselect = true;
            fileDIalog.FileOk += FileDIalog_FileOk;
            fileDIalog.Filter = "Image Files | *.pdf";
            fileDIalog.ShowDialog();
        }

        private void btnAddDocument_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentOutputInvoiceDocumentForm.Name))
            {
                MainWindow.WarningMessage = "Obavezno polje: Naziv";
                return;
            }

            if (String.IsNullOrEmpty(CurrentOutputInvoiceDocumentForm.Path))
            {
                MainWindow.WarningMessage = "Obavezno polje: Putanja";
                return;
            }

            if (CurrentOutputInvoiceDocumentForm.CreateDate == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Datum kreiranja";
                return;
            }

            #endregion

            // IF update process, first delete item
            new OutputInvoiceDocumentSQLiteRepository().Delete(CurrentOutputInvoiceDocumentForm.Identifier);

            CurrentOutputInvoiceDocumentForm.OutputInvoice = CurrentOutputInvoice;
            CurrentOutputInvoiceDocumentForm.Identifier = Guid.NewGuid();
            CurrentOutputInvoiceDocumentForm.IsSynced = false;
            CurrentOutputInvoiceDocumentForm.UpdatedAt = DateTime.Now;
            CurrentOutputInvoiceDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentOutputInvoiceDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new OutputInvoiceDocumentSQLiteRepository().Create(CurrentOutputInvoiceDocumentForm);
            if (response.Success)
            {
                CurrentOutputInvoiceDocumentForm = new OutputInvoiceDocumentViewModel();
                CurrentOutputInvoiceDocumentForm.CreateDate = DateTime.Now;

                Thread displayThread = new Thread(() => DisplayOutputInvoiceDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();

                txtDocumentName.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentOutputInvoiceDocumentForm = new OutputInvoiceDocumentViewModel();
            CurrentOutputInvoiceDocumentForm.CreateDate = DateTime.Now;
        }

        private void btnEditDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentOutputInvoiceDocumentForm = CurrentOutputInvoiceDocumentDG;
        }

        private void btnDeleteDocument_Click(object sender, RoutedEventArgs e)
        {
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("dokument", "");
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                new OutputInvoiceDocumentSQLiteRepository().Delete(CurrentOutputInvoiceDocumentDG.Identifier);

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Dokument_je_uspešno_obrisanUzvičnik"));

                Thread displayThread = new Thread(() => DisplayOutputInvoiceDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        #endregion

        #region Mouse wheel event 

        private void PreviewMouseWheelEv(object sender, System.Windows.Input.MouseWheelEventArgs e)
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

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "") //[CallerMemberName]
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion

       

        private void btnSaveHeader_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentOutputInvoice.InvoiceNumber))
            {
                MainWindow.WarningMessage = "Obavezno polje: Broj fakture";
                return;
            }

            #endregion

            CurrentOutputInvoice.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentOutputInvoice.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            CurrentOutputInvoice.IsSynced = false;

            OutputInvoiceResponse response = new OutputInvoiceSQLiteRepository().Delete(CurrentOutputInvoice.Identifier);
            response = new OutputInvoiceSQLiteRepository().Create(CurrentOutputInvoice);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Zaglavlje_je_uspešno_sačuvanoUzvičnik"));
                IsHeaderCreated = true;

                txtNote.Focus();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }
    }
}
