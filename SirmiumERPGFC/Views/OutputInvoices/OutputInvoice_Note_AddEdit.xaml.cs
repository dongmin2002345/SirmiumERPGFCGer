﻿using Ninject;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.OutputInvoices;
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

namespace SirmiumERPGFC.Views.OutputInvoices
{
    /// <summary>
    /// Interaction logic for OutputInvoice_Note_AddEdit.xaml
    /// </summary>
    public partial class OutputInvoice_Note_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IOutputInvoiceService outputInvoiceService;
        IOutputInvoiceNoteService outputInvoiceNoteService;
        #endregion


        #region Event
        public event OutputInvoiceHandler OutputInvoiceCreatedUpdated;
        #endregion


        #region CurrentOutputInvoice
        private OutputInvoiceViewModel _CurrentOutputInvoice = new OutputInvoiceViewModel();

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

        public OutputInvoice_Note_AddEdit(OutputInvoiceViewModel outputInvoice)
        {
            outputInvoiceService = DependencyResolver.Kernel.Get<IOutputInvoiceService>();
            outputInvoiceNoteService = DependencyResolver.Kernel.Get<IOutputInvoiceNoteService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentOutputInvoice = outputInvoice;
            CurrentOutputInvoiceNoteForm = new OutputInvoiceNoteViewModel();
            CurrentOutputInvoiceNoteForm.Identifier = Guid.NewGuid();
            CurrentOutputInvoiceNoteForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayOutputInvoiceNoteData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayOutputInvoiceNoteData()
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

        private void DgOutputInvoiceNotes_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentOutputInvoiceNoteForm.Note == null)
            {
                MainWindow.ErrorMessage = "Obavezno polje: Napomena";
                return;
            }

            #endregion

            new OutputInvoiceNoteSQLiteRepository().Delete(CurrentOutputInvoiceNoteForm.Identifier);

            CurrentOutputInvoiceNoteForm.OutputInvoice = CurrentOutputInvoice;

            CurrentOutputInvoiceNoteForm.IsSynced = false;
            CurrentOutputInvoiceNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentOutputInvoiceNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new OutputInvoiceNoteSQLiteRepository().Create(CurrentOutputInvoiceNoteForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentOutputInvoiceNoteForm = new OutputInvoiceNoteViewModel();
                CurrentOutputInvoiceNoteForm.Identifier = Guid.NewGuid();
                CurrentOutputInvoiceNoteForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentOutputInvoiceNoteForm = new OutputInvoiceNoteViewModel();
            CurrentOutputInvoiceNoteForm.Identifier = Guid.NewGuid();
            CurrentOutputInvoiceNoteForm.ItemStatus = ItemStatus.Added;

            OutputInvoiceCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayOutputInvoiceNoteData());
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
            CurrentOutputInvoiceNoteForm = new OutputInvoiceNoteViewModel();
            CurrentOutputInvoiceNoteForm.Identifier = CurrentOutputInvoiceNoteDG.Identifier;
            CurrentOutputInvoiceNoteForm.ItemStatus = ItemStatus.Edited;

            CurrentOutputInvoiceNoteForm.Note = CurrentOutputInvoiceNoteDG.Note;
            CurrentOutputInvoiceNoteForm.NoteDate = CurrentOutputInvoiceNoteDG.NoteDate;

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new OutputInvoiceNoteSQLiteRepository().SetStatusDeleted(CurrentOutputInvoiceNoteDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = "Stavka je uspešno obrisana!";

                CurrentOutputInvoiceNoteForm = new OutputInvoiceNoteViewModel();
                CurrentOutputInvoiceNoteForm.Identifier = Guid.NewGuid();
                CurrentOutputInvoiceNoteForm.ItemStatus = ItemStatus.Added;

                CurrentOutputInvoiceNoteDG = null;

                OutputInvoiceCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayOutputInvoiceNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentOutputInvoiceNoteForm = new OutputInvoiceNoteViewModel();
            CurrentOutputInvoiceNoteForm.Identifier = Guid.NewGuid();
            CurrentOutputInvoiceNoteForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (OutputInvoiceNotesFromDB == null || OutputInvoiceNotesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = "Ne postoje stavke za proknjižavanje!";
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = " Čuvanje u toku... ";
                SubmitButtonEnabled = false;

                CurrentOutputInvoice.OutputInvoiceNotes = OutputInvoiceNotesFromDB;
                OutputInvoiceResponse response = outputInvoiceService.Create(CurrentOutputInvoice);
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

                    OutputInvoiceCreatedUpdated();

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
            OutputInvoiceCreatedUpdated();

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
