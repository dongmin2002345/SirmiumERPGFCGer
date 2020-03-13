using Ninject;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Shipments;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Shipments;
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

namespace SirmiumERPGFC.Views.Shipments
{
    /// <summary>
    /// Interaction logic for Shipment_Document_AddEdit.xaml
    /// </summary>
    public partial class Shipment_Document_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IShipmentService ShipmentService;
        IShipmentDocumentService ShipmentDocumentService;
        #endregion


        #region Event
        public event ShipmentHandler ShipmentCreatedUpdated;
        #endregion


        #region CurrentShipment
        private ShipmentViewModel _CurrentShipment = new ShipmentViewModel();

        public ShipmentViewModel CurrentShipment
        {
            get { return _CurrentShipment; }
            set
            {
                if (_CurrentShipment != value)
                {
                    _CurrentShipment = value;
                    NotifyPropertyChanged("CurrentShipment");
                }
            }
        }
        #endregion


        #region ShipmentDocumentsFromDB
        private ObservableCollection<ShipmentDocumentViewModel> _ShipmentDocumentsFromDB;

        public ObservableCollection<ShipmentDocumentViewModel> ShipmentDocumentsFromDB
        {
            get { return _ShipmentDocumentsFromDB; }
            set
            {
                if (_ShipmentDocumentsFromDB != value)
                {
                    _ShipmentDocumentsFromDB = value;
                    NotifyPropertyChanged("ShipmentDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentShipmentDocumentForm
        private ShipmentDocumentViewModel _CurrentShipmentDocumentForm = new ShipmentDocumentViewModel() { CreateDate = DateTime.Now };

        public ShipmentDocumentViewModel CurrentShipmentDocumentForm
        {
            get { return _CurrentShipmentDocumentForm; }
            set
            {
                if (_CurrentShipmentDocumentForm != value)
                {
                    _CurrentShipmentDocumentForm = value;
                    NotifyPropertyChanged("CurrentShipmentDocumentForm");
                }
            }
        }
        #endregion

        #region CurrentShipmentDocumentDG
        private ShipmentDocumentViewModel _CurrentShipmentDocumentDG;

        public ShipmentDocumentViewModel CurrentShipmentDocumentDG
        {
            get { return _CurrentShipmentDocumentDG; }
            set
            {
                if (_CurrentShipmentDocumentDG != value)
                {
                    _CurrentShipmentDocumentDG = value;
                    NotifyPropertyChanged("CurrentShipmentDocumentDG");
                }
            }
        }
        #endregion

        #region ShipmentDocumentDataLoading
        private bool _ShipmentDocumentDataLoading;

        public bool ShipmentDocumentDataLoading
        {
            get { return _ShipmentDocumentDataLoading; }
            set
            {
                if (_ShipmentDocumentDataLoading != value)
                {
                    _ShipmentDocumentDataLoading = value;
                    NotifyPropertyChanged("ShipmentDocumentDataLoading");
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

        public Shipment_Document_AddEdit(ShipmentViewModel Shipment)
        {
            ShipmentService = DependencyResolver.Kernel.Get<IShipmentService>();
            ShipmentDocumentService = DependencyResolver.Kernel.Get<IShipmentDocumentService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentShipment = Shipment;
            CurrentShipmentDocumentForm = new ShipmentDocumentViewModel();
            CurrentShipmentDocumentForm.Identifier = Guid.NewGuid();
            CurrentShipmentDocumentForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayShipmentDocumentData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddDocument.Focus();
        }

        #endregion

        #region Display data

        public void DisplayShipmentDocumentData()
        {
            ShipmentDocumentDataLoading = true;

            ShipmentDocumentListResponse response = new ShipmentDocumentSQLiteRepository()
                .GetShipmentDocumentsByShipment(MainWindow.CurrentCompanyId, CurrentShipment.Identifier);

            if (response.Success)
            {
                ShipmentDocumentsFromDB = new ObservableCollection<ShipmentDocumentViewModel>(
                    response.ShipmentDocuments ?? new List<ShipmentDocumentViewModel>());
            }
            else
            {
                ShipmentDocumentsFromDB = new ObservableCollection<ShipmentDocumentViewModel>();
            }

            ShipmentDocumentDataLoading = false;
        }

        private void DgShipmentDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentShipmentDocumentForm.Name == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;


                CurrentShipmentDocumentForm.Shipment = CurrentShipment;


                CurrentShipmentDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentShipmentDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                new ShipmentDocumentSQLiteRepository().Delete(CurrentShipmentDocumentForm.Identifier);
                var response = new ShipmentDocumentSQLiteRepository().Create(CurrentShipmentDocumentForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentShipmentDocumentForm = new ShipmentDocumentViewModel();
                    CurrentShipmentDocumentForm.Identifier = Guid.NewGuid();
                    CurrentShipmentDocumentForm.ItemStatus = ItemStatus.Added;
                    CurrentShipmentDocumentForm.IsSynced = false;
                    return;
                }

                CurrentShipmentDocumentForm = new ShipmentDocumentViewModel();
                CurrentShipmentDocumentForm.Identifier = Guid.NewGuid();
                CurrentShipmentDocumentForm.ItemStatus = ItemStatus.Added;
                CurrentShipmentDocumentForm.IsSynced = false;
                ShipmentCreatedUpdated();

                DisplayShipmentDocumentData();

                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        txtDocumentName.Focus();
                    })
                );

                SubmitButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnEditDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentShipmentDocumentForm = new ShipmentDocumentViewModel();
            CurrentShipmentDocumentForm.Identifier = CurrentShipmentDocumentDG.Identifier;
            CurrentShipmentDocumentForm.ItemStatus = ItemStatus.Edited;

            CurrentShipmentDocumentForm.IsSynced = CurrentShipmentDocumentDG.IsSynced;
            CurrentShipmentDocumentForm.Name = CurrentShipmentDocumentDG.Name;
            CurrentShipmentDocumentForm.CreateDate = CurrentShipmentDocumentDG.CreateDate;
            CurrentShipmentDocumentForm.Path = CurrentShipmentDocumentDG.Path;
            CurrentShipmentDocumentForm.UpdatedAt = CurrentShipmentDocumentDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new ShipmentDocumentSQLiteRepository().SetStatusDeleted(CurrentShipmentDocumentDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentShipmentDocumentForm = new ShipmentDocumentViewModel();
                CurrentShipmentDocumentForm.Identifier = Guid.NewGuid();
                CurrentShipmentDocumentForm.ItemStatus = ItemStatus.Added;

                CurrentShipmentDocumentDG = null;

                ShipmentCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayShipmentDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentShipmentDocumentForm = new ShipmentDocumentViewModel();
            CurrentShipmentDocumentForm.Identifier = Guid.NewGuid();
            CurrentShipmentDocumentForm.ItemStatus = ItemStatus.Added;
        }

        private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
            string[] fileNames = dialog.FileNames;

            if (fileNames.Length > 0)
            {
                CurrentShipmentDocumentForm.Path = fileNames[0];
                if (!String.IsNullOrEmpty(CurrentShipmentDocumentForm.Path))
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(CurrentShipmentDocumentForm.Path);
                    CurrentShipmentDocumentForm.Name = fileName;
                }
            }
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

            if (ShipmentDocumentsFromDB == null || ShipmentDocumentsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() =>
            {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentShipment.ShipmentDocuments = ShipmentDocumentsFromDB;
                ShipmentResponse response = ShipmentService.Create(CurrentShipment);
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

                    ShipmentCreatedUpdated();

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
            ShipmentCreatedUpdated();

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
