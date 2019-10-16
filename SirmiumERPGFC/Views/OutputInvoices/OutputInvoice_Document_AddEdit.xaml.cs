using Ninject;
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
    /// Interaction logic for OutputInvoice_Document_AddEdit.xaml
    /// </summary>
    public partial class OutputInvoice_Document_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IOutputInvoiceService outputInvoiceService;
        IOutputInvoiceDocumentService outputInvoiceDocumentService;
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

        public OutputInvoice_Document_AddEdit(OutputInvoiceViewModel outputInvoice)
        {
            outputInvoiceService = DependencyResolver.Kernel.Get<IOutputInvoiceService>();
            outputInvoiceDocumentService = DependencyResolver.Kernel.Get<IOutputInvoiceDocumentService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentOutputInvoice = outputInvoice;
            CurrentOutputInvoiceDocumentForm = new OutputInvoiceDocumentViewModel();
            CurrentOutputInvoiceDocumentForm.Identifier = Guid.NewGuid();
            CurrentOutputInvoiceDocumentForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayOutputInvoiceDocumentData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddDocument.Focus();
        }

        #endregion

        #region Display data

        public void DisplayOutputInvoiceDocumentData()
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

        private void DgOutputInvoiceDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentOutputInvoiceDocumentForm.Name == null)
            {
                MainWindow.ErrorMessage = "Obavezno polje: Napomena";
                return;
            }

            #endregion

            new OutputInvoiceDocumentSQLiteRepository().Delete(CurrentOutputInvoiceDocumentForm.Identifier);

            CurrentOutputInvoiceDocumentForm.OutputInvoice = CurrentOutputInvoice;

            CurrentOutputInvoiceDocumentForm.IsSynced = false;
            CurrentOutputInvoiceDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentOutputInvoiceDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new OutputInvoiceDocumentSQLiteRepository().Create(CurrentOutputInvoiceDocumentForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentOutputInvoiceDocumentForm = new OutputInvoiceDocumentViewModel();
                CurrentOutputInvoiceDocumentForm.Identifier = Guid.NewGuid();
                CurrentOutputInvoiceDocumentForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentOutputInvoiceDocumentForm = new OutputInvoiceDocumentViewModel();
            CurrentOutputInvoiceDocumentForm.Identifier = Guid.NewGuid();
            CurrentOutputInvoiceDocumentForm.ItemStatus = ItemStatus.Added;

            OutputInvoiceCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayOutputInvoiceDocumentData());
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
            CurrentOutputInvoiceDocumentForm = new OutputInvoiceDocumentViewModel();
            CurrentOutputInvoiceDocumentForm.Identifier = CurrentOutputInvoiceDocumentDG.Identifier;
            CurrentOutputInvoiceDocumentForm.ItemStatus = ItemStatus.Edited;

            CurrentOutputInvoiceDocumentForm.Name = CurrentOutputInvoiceDocumentDG.Name;
            CurrentOutputInvoiceDocumentForm.CreateDate = CurrentOutputInvoiceDocumentDG.CreateDate;
            CurrentOutputInvoiceDocumentForm.Path = CurrentOutputInvoiceDocumentDG.Path;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new OutputInvoiceDocumentSQLiteRepository().SetStatusDeleted(CurrentOutputInvoiceDocumentDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = "Stavka je uspešno obrisana!";

                CurrentOutputInvoiceDocumentForm = new OutputInvoiceDocumentViewModel();
                CurrentOutputInvoiceDocumentForm.Identifier = Guid.NewGuid();
                CurrentOutputInvoiceDocumentForm.ItemStatus = ItemStatus.Added;

                CurrentOutputInvoiceDocumentDG = null;

                OutputInvoiceCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayOutputInvoiceDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentOutputInvoiceDocumentForm = new OutputInvoiceDocumentViewModel();
            CurrentOutputInvoiceDocumentForm.Identifier = Guid.NewGuid();
            CurrentOutputInvoiceDocumentForm.ItemStatus = ItemStatus.Added;
        }

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
            fileDIalog.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            fileDIalog.ShowDialog();
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (OutputInvoiceDocumentsFromDB == null || OutputInvoiceDocumentsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = "Ne postoje stavke za proknjižavanje!";
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = " Čuvanje u toku... ";
                SubmitButtonEnabled = false;

                CurrentOutputInvoice.OutputInvoiceDocuments = OutputInvoiceDocumentsFromDB;
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
