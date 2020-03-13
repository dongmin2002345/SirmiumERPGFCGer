using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

namespace SirmiumERPGFC.Views.BusinessPartners
{
    /// <summary>
    /// Interaction logic for BusinessPartner_Document_AddEdit.xaml
    /// </summary>
    public partial class BusinessPartner_Document_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerService businessPartnerService;
        IBusinessPartnerDocumentService businessPartnerDocumentService;
        #endregion


        #region Event
        public event BusinessPartnerHandler BusinessPartnerCreatedUpdated;
        #endregion


        #region CurrentBusinessPartner
        private BusinessPartnerViewModel _CurrentBusinessPartner = new BusinessPartnerViewModel();

        public BusinessPartnerViewModel CurrentBusinessPartner
        {
            get { return _CurrentBusinessPartner; }
            set
            {
                if (_CurrentBusinessPartner != value)
                {
                    _CurrentBusinessPartner = value;
                    NotifyPropertyChanged("CurrentBusinessPartner");
                }
            }
        }
        #endregion


        #region BusinessPartnerDocumentsFromDB
        private ObservableCollection<BusinessPartnerDocumentViewModel> _BusinessPartnerDocumentsFromDB;

        public ObservableCollection<BusinessPartnerDocumentViewModel> BusinessPartnerDocumentsFromDB
        {
            get { return _BusinessPartnerDocumentsFromDB; }
            set
            {
                if (_BusinessPartnerDocumentsFromDB != value)
                {
                    _BusinessPartnerDocumentsFromDB = value;
                    NotifyPropertyChanged("BusinessPartnerDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerDocumentForm
        private BusinessPartnerDocumentViewModel _CurrentBusinessPartnerDocumentForm = new BusinessPartnerDocumentViewModel() { CreateDate = DateTime.Now };

        public BusinessPartnerDocumentViewModel CurrentBusinessPartnerDocumentForm
        {
            get { return _CurrentBusinessPartnerDocumentForm; }
            set
            {
                if (_CurrentBusinessPartnerDocumentForm != value)
                {
                    _CurrentBusinessPartnerDocumentForm = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerDocumentForm");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerDocumentDG
        private BusinessPartnerDocumentViewModel _CurrentBusinessPartnerDocumentDG;

        public BusinessPartnerDocumentViewModel CurrentBusinessPartnerDocumentDG
        {
            get { return _CurrentBusinessPartnerDocumentDG; }
            set
            {
                if (_CurrentBusinessPartnerDocumentDG != value)
                {
                    _CurrentBusinessPartnerDocumentDG = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerDocumentDG");
                }
            }
        }
        #endregion

        #region BusinessPartnerDocumentDataLoading
        private bool _BusinessPartnerDocumentDataLoading;

        public bool BusinessPartnerDocumentDataLoading
        {
            get { return _BusinessPartnerDocumentDataLoading; }
            set
            {
                if (_BusinessPartnerDocumentDataLoading != value)
                {
                    _BusinessPartnerDocumentDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerDocumentDataLoading");
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

        public BusinessPartner_Document_AddEdit(BusinessPartnerViewModel businessPartner)
        {
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            businessPartnerDocumentService = DependencyResolver.Kernel.Get<IBusinessPartnerDocumentService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentBusinessPartner = businessPartner;
            CurrentBusinessPartnerDocumentForm = new BusinessPartnerDocumentViewModel();
            CurrentBusinessPartnerDocumentForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerDocumentForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayBusinessPartnerDocumentData());
            displayThread.IsBackground = true;
            displayThread.Start();

            txtBusinessPartnerDocumentName.Focus();
        }

        #endregion

        #region Display data

        public void DisplayBusinessPartnerDocumentData()
        {
            BusinessPartnerDocumentDataLoading = true;

            BusinessPartnerDocumentListResponse response = new BusinessPartnerDocumentSQLiteRepository()
                .GetBusinessPartnerDocumentsByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                BusinessPartnerDocumentsFromDB = new ObservableCollection<BusinessPartnerDocumentViewModel>(
                    response.BusinessPartnerDocuments ?? new List<BusinessPartnerDocumentViewModel>());
            }
            else
            {
                BusinessPartnerDocumentsFromDB = new ObservableCollection<BusinessPartnerDocumentViewModel>();
            }

            BusinessPartnerDocumentDataLoading = false;
        }

        private void DgBusinessPartnerDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentBusinessPartnerDocumentForm.Name == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;


                CurrentBusinessPartnerDocumentForm.BusinessPartner = CurrentBusinessPartner;


                CurrentBusinessPartnerDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentBusinessPartnerDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                new BusinessPartnerDocumentSQLiteRepository().Delete(CurrentBusinessPartnerDocumentForm.Identifier);
                var response = new BusinessPartnerDocumentSQLiteRepository().Create(CurrentBusinessPartnerDocumentForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentBusinessPartnerDocumentForm = new BusinessPartnerDocumentViewModel();
                    CurrentBusinessPartnerDocumentForm.Identifier = Guid.NewGuid();
                    CurrentBusinessPartnerDocumentForm.ItemStatus = ItemStatus.Added;
                    CurrentBusinessPartnerDocumentForm.IsSynced = false;
                    return;
                }

                CurrentBusinessPartnerDocumentForm = new BusinessPartnerDocumentViewModel();
                CurrentBusinessPartnerDocumentForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerDocumentForm.ItemStatus = ItemStatus.Added;
                CurrentBusinessPartnerDocumentForm.IsSynced = false;
                BusinessPartnerCreatedUpdated();

                DisplayBusinessPartnerDocumentData();

                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        txtBusinessPartnerDocumentName.Focus();
                    })
                );

                SubmitButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnEditDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerDocumentForm = new BusinessPartnerDocumentViewModel();
            CurrentBusinessPartnerDocumentForm.Identifier = CurrentBusinessPartnerDocumentDG.Identifier;
            CurrentBusinessPartnerDocumentForm.ItemStatus = ItemStatus.Edited;

            CurrentBusinessPartnerDocumentForm.IsSynced = CurrentBusinessPartnerDocumentDG.IsSynced;
            CurrentBusinessPartnerDocumentForm.Name = CurrentBusinessPartnerDocumentDG.Name;
            CurrentBusinessPartnerDocumentForm.CreateDate = CurrentBusinessPartnerDocumentDG.CreateDate;
            CurrentBusinessPartnerDocumentForm.Path = CurrentBusinessPartnerDocumentDG.Path;
            CurrentBusinessPartnerDocumentForm.UpdatedAt = CurrentBusinessPartnerDocumentDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new BusinessPartnerDocumentSQLiteRepository().SetStatusDeleted(CurrentBusinessPartnerDocumentDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentBusinessPartnerDocumentForm = new BusinessPartnerDocumentViewModel();
                CurrentBusinessPartnerDocumentForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerDocumentForm.ItemStatus = ItemStatus.Added;

                CurrentBusinessPartnerDocumentDG = null;

                BusinessPartnerCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayBusinessPartnerDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerDocumentForm = new BusinessPartnerDocumentViewModel();
            CurrentBusinessPartnerDocumentForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerDocumentForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (BusinessPartnerDocumentsFromDB == null || BusinessPartnerDocumentsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentBusinessPartner.BusinessPartnerDocuments = BusinessPartnerDocumentsFromDB;
                BusinessPartnerResponse response = businessPartnerService.Create(CurrentBusinessPartner);
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

                    BusinessPartnerCreatedUpdated();

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
            BusinessPartnerCreatedUpdated();

            FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
            dialog.Multiselect = false;
            string[] fileNames = dialog.FileNames;

            if (fileNames.Length > 0)
            {
                CurrentBusinessPartnerDocumentForm.Path = fileNames[0];
                if (!String.IsNullOrEmpty(CurrentBusinessPartnerDocumentForm.Path))
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(CurrentBusinessPartnerDocumentForm.Path);
                    CurrentBusinessPartnerDocumentForm.Name = fileName;
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

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion
    }
}
