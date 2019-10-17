using Ninject;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
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
    /// Interaction logic for InputInvoice_Document_AddEdit.xaml
    /// </summary>
    public partial class InputInvoice_Document_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IInputInvoiceService inputInvoiceService;
        IInputInvoiceDocumentService inputInvoiceDocumentService;
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


        #region InputInvoiceDocumentsFromDB
        private ObservableCollection<InputInvoiceDocumentViewModel> _InputInvoiceDocumentsFromDB;

        public ObservableCollection<InputInvoiceDocumentViewModel> InputInvoiceDocumentsFromDB
        {
            get { return _InputInvoiceDocumentsFromDB; }
            set
            {
                if (_InputInvoiceDocumentsFromDB != value)
                {
                    _InputInvoiceDocumentsFromDB = value;
                    NotifyPropertyChanged("InputInvoiceDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentInputInvoiceDocumentForm
        private InputInvoiceDocumentViewModel _CurrentInputInvoiceDocumentForm = new InputInvoiceDocumentViewModel() { CreateDate = DateTime.Now };

        public InputInvoiceDocumentViewModel CurrentInputInvoiceDocumentForm
        {
            get { return _CurrentInputInvoiceDocumentForm; }
            set
            {
                if (_CurrentInputInvoiceDocumentForm != value)
                {
                    _CurrentInputInvoiceDocumentForm = value;
                    NotifyPropertyChanged("CurrentInputInvoiceDocumentForm");
                }
            }
        }
        #endregion

        #region CurrentInputInvoiceDocumentDG
        private InputInvoiceDocumentViewModel _CurrentInputInvoiceDocumentDG;

        public InputInvoiceDocumentViewModel CurrentInputInvoiceDocumentDG
        {
            get { return _CurrentInputInvoiceDocumentDG; }
            set
            {
                if (_CurrentInputInvoiceDocumentDG != value)
                {
                    _CurrentInputInvoiceDocumentDG = value;
                    NotifyPropertyChanged("CurrentInputInvoiceDocumentDG");
                }
            }
        }
        #endregion

        #region InputInvoiceDocumentDataLoading
        private bool _InputInvoiceDocumentDataLoading;

        public bool InputInvoiceDocumentDataLoading
        {
            get { return _InputInvoiceDocumentDataLoading; }
            set
            {
                if (_InputInvoiceDocumentDataLoading != value)
                {
                    _InputInvoiceDocumentDataLoading = value;
                    NotifyPropertyChanged("InputInvoiceDocumentDataLoading");
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

        public InputInvoice_Document_AddEdit(InputInvoiceViewModel inputInvoice)
        {
            inputInvoiceService = DependencyResolver.Kernel.Get<IInputInvoiceService>();
            inputInvoiceDocumentService = DependencyResolver.Kernel.Get<IInputInvoiceDocumentService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentInputInvoice = inputInvoice;
            CurrentInputInvoiceDocumentForm = new InputInvoiceDocumentViewModel();
            CurrentInputInvoiceDocumentForm.Identifier = Guid.NewGuid();
            CurrentInputInvoiceDocumentForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayInputInvoiceDocumentData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddDocument.Focus();
        }

        #endregion

        #region Display data

        public void DisplayInputInvoiceDocumentData()
        {
            InputInvoiceDocumentDataLoading = true;

            InputInvoiceDocumentListResponse response = new InputInvoiceDocumentSQLiteRepository()
                .GetInputInvoiceDocumentsByInputInvoice(MainWindow.CurrentCompanyId, CurrentInputInvoice.Identifier);

            if (response.Success)
            {
                InputInvoiceDocumentsFromDB = new ObservableCollection<InputInvoiceDocumentViewModel>(
                    response.InputInvoiceDocuments ?? new List<InputInvoiceDocumentViewModel>());
            }
            else
            {
                InputInvoiceDocumentsFromDB = new ObservableCollection<InputInvoiceDocumentViewModel>();
            }

            InputInvoiceDocumentDataLoading = false;
        }

        private void DgInputInvoiceDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentInputInvoiceDocumentForm.Name == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv"));
                return;
            }

            #endregion

            new InputInvoiceDocumentSQLiteRepository().Delete(CurrentInputInvoiceDocumentForm.Identifier);

            CurrentInputInvoiceDocumentForm.InputInvoice = CurrentInputInvoice;

            CurrentInputInvoiceDocumentForm.IsSynced = false;
            CurrentInputInvoiceDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentInputInvoiceDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new InputInvoiceDocumentSQLiteRepository().Create(CurrentInputInvoiceDocumentForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentInputInvoiceDocumentForm = new InputInvoiceDocumentViewModel();
                CurrentInputInvoiceDocumentForm.Identifier = Guid.NewGuid();
                CurrentInputInvoiceDocumentForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentInputInvoiceDocumentForm = new InputInvoiceDocumentViewModel();
            CurrentInputInvoiceDocumentForm.Identifier = Guid.NewGuid();
            CurrentInputInvoiceDocumentForm.ItemStatus = ItemStatus.Added;

            InputInvoiceCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayInputInvoiceDocumentData());
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
            CurrentInputInvoiceDocumentForm = new InputInvoiceDocumentViewModel();
            CurrentInputInvoiceDocumentForm.Identifier = CurrentInputInvoiceDocumentDG.Identifier;
            CurrentInputInvoiceDocumentForm.ItemStatus = ItemStatus.Edited;

            CurrentInputInvoiceDocumentForm.Name = CurrentInputInvoiceDocumentDG.Name;
            CurrentInputInvoiceDocumentForm.CreateDate = CurrentInputInvoiceDocumentDG.CreateDate;
            CurrentInputInvoiceDocumentForm.Path = CurrentInputInvoiceDocumentDG.Path;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new InputInvoiceDocumentSQLiteRepository().SetStatusDeleted(CurrentInputInvoiceDocumentDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentInputInvoiceDocumentForm = new InputInvoiceDocumentViewModel();
                CurrentInputInvoiceDocumentForm.Identifier = Guid.NewGuid();
                CurrentInputInvoiceDocumentForm.ItemStatus = ItemStatus.Added;

                CurrentInputInvoiceDocumentDG = null;

                InputInvoiceCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayInputInvoiceDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentInputInvoiceDocumentForm = new InputInvoiceDocumentViewModel();
            CurrentInputInvoiceDocumentForm.Identifier = Guid.NewGuid();
            CurrentInputInvoiceDocumentForm.ItemStatus = ItemStatus.Added;
        }

        private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
            string[] fileNames = dialog.FileNames;

            if (fileNames.Length > 0)
                CurrentInputInvoiceDocumentForm.Path = fileNames[0];
        }

        private void btnChooseDocument_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDIalog = new System.Windows.Forms.OpenFileDialog();

            fileDIalog.Multiselect = true;
            fileDIalog.FileOk += FileDIalog_FileOk;
            fileDIalog.Filter = "All Files (*.*)|*.*";
            fileDIalog.ShowDialog();
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (InputInvoiceDocumentsFromDB == null || InputInvoiceDocumentsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentInputInvoice.InputInvoiceDocuments = InputInvoiceDocumentsFromDB;
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
