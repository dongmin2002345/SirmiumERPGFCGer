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
    /// Interaction logic for Phonebook_Document_AddEdit.xaml
    /// </summary>
    public partial class Phonebook_Document_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhonebookService PhonebookService;
        IPhonebookDocumentService PhonebookDocumentService;
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


        #region PhonebookDocumentsFromDB
        private ObservableCollection<PhonebookDocumentViewModel> _PhonebookDocumentsFromDB;

        public ObservableCollection<PhonebookDocumentViewModel> PhonebookDocumentsFromDB
        {
            get { return _PhonebookDocumentsFromDB; }
            set
            {
                if (_PhonebookDocumentsFromDB != value)
                {
                    _PhonebookDocumentsFromDB = value;
                    NotifyPropertyChanged("PhonebookDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhonebookDocumentForm
        private PhonebookDocumentViewModel _CurrentPhonebookDocumentForm = new PhonebookDocumentViewModel() { CreateDate = DateTime.Now };

        public PhonebookDocumentViewModel CurrentPhonebookDocumentForm
        {
            get { return _CurrentPhonebookDocumentForm; }
            set
            {
                if (_CurrentPhonebookDocumentForm != value)
                {
                    _CurrentPhonebookDocumentForm = value;
                    NotifyPropertyChanged("CurrentPhonebookDocumentForm");
                }
            }
        }
        #endregion

        #region CurrentPhonebookDocumentDG
        private PhonebookDocumentViewModel _CurrentPhonebookDocumentDG;

        public PhonebookDocumentViewModel CurrentPhonebookDocumentDG
        {
            get { return _CurrentPhonebookDocumentDG; }
            set
            {
                if (_CurrentPhonebookDocumentDG != value)
                {
                    _CurrentPhonebookDocumentDG = value;
                    NotifyPropertyChanged("CurrentPhonebookDocumentDG");
                }
            }
        }
        #endregion

        #region PhonebookDocumentDataLoading
        private bool _PhonebookDocumentDataLoading;

        public bool PhonebookDocumentDataLoading
        {
            get { return _PhonebookDocumentDataLoading; }
            set
            {
                if (_PhonebookDocumentDataLoading != value)
                {
                    _PhonebookDocumentDataLoading = value;
                    NotifyPropertyChanged("PhonebookDocumentDataLoading");
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

        public Phonebook_Document_AddEdit(PhonebookViewModel Phonebook)
        {
            PhonebookService = DependencyResolver.Kernel.Get<IPhonebookService>();
            PhonebookDocumentService = DependencyResolver.Kernel.Get<IPhonebookDocumentService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentPhonebook = Phonebook;
            CurrentPhonebookDocumentForm = new PhonebookDocumentViewModel();
            CurrentPhonebookDocumentForm.Identifier = Guid.NewGuid();
            CurrentPhonebookDocumentForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayPhonebookDocumentData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddDocument.Focus();
        }

        #endregion

        #region Display data

        public void DisplayPhonebookDocumentData()
        {
            PhonebookDocumentDataLoading = true;

            PhonebookDocumentListResponse response = new PhonebookDocumentSQLiteRepository()
                .GetPhonebookDocumentsByPhonebook(MainWindow.CurrentCompanyId, CurrentPhonebook.Identifier);

            if (response.Success)
            {
                PhonebookDocumentsFromDB = new ObservableCollection<PhonebookDocumentViewModel>(
                    response.PhonebookDocuments ?? new List<PhonebookDocumentViewModel>());
            }
            else
            {
                PhonebookDocumentsFromDB = new ObservableCollection<PhonebookDocumentViewModel>();
            }

            PhonebookDocumentDataLoading = false;
        }

        private void DgPhonebookDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentPhonebookDocumentForm.Name == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;


                CurrentPhonebookDocumentForm.Phonebook = CurrentPhonebook;


                CurrentPhonebookDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentPhonebookDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                new PhonebookDocumentSQLiteRepository().Delete(CurrentPhonebookDocumentForm.Identifier);
                var response = new PhonebookDocumentSQLiteRepository().Create(CurrentPhonebookDocumentForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentPhonebookDocumentForm = new PhonebookDocumentViewModel();
                    CurrentPhonebookDocumentForm.Identifier = Guid.NewGuid();
                    CurrentPhonebookDocumentForm.ItemStatus = ItemStatus.Added;
                    CurrentPhonebookDocumentForm.IsSynced = false;
                    return;
                }

                CurrentPhonebookDocumentForm = new PhonebookDocumentViewModel();
                CurrentPhonebookDocumentForm.Identifier = Guid.NewGuid();
                CurrentPhonebookDocumentForm.ItemStatus = ItemStatus.Added;
                CurrentPhonebookDocumentForm.IsSynced = false;
                PhonebookCreatedUpdated();

                DisplayPhonebookDocumentData();

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
            CurrentPhonebookDocumentForm = new PhonebookDocumentViewModel();
            CurrentPhonebookDocumentForm.Identifier = CurrentPhonebookDocumentDG.Identifier;
            CurrentPhonebookDocumentForm.ItemStatus = ItemStatus.Edited;

            CurrentPhonebookDocumentForm.IsSynced = CurrentPhonebookDocumentDG.IsSynced;
            CurrentPhonebookDocumentForm.Name = CurrentPhonebookDocumentDG.Name;
            CurrentPhonebookDocumentForm.CreateDate = CurrentPhonebookDocumentDG.CreateDate;
            CurrentPhonebookDocumentForm.Path = CurrentPhonebookDocumentDG.Path;
            CurrentPhonebookDocumentForm.UpdatedAt = CurrentPhonebookDocumentDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new PhonebookDocumentSQLiteRepository().SetStatusDeleted(CurrentPhonebookDocumentDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentPhonebookDocumentForm = new PhonebookDocumentViewModel();
                CurrentPhonebookDocumentForm.Identifier = Guid.NewGuid();
                CurrentPhonebookDocumentForm.ItemStatus = ItemStatus.Added;

                CurrentPhonebookDocumentDG = null;

                PhonebookCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayPhonebookDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhonebookDocumentForm = new PhonebookDocumentViewModel();
            CurrentPhonebookDocumentForm.Identifier = Guid.NewGuid();
            CurrentPhonebookDocumentForm.ItemStatus = ItemStatus.Added;
        }

        private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
            string[] fileNames = dialog.FileNames;

            if (fileNames.Length > 0)
                CurrentPhonebookDocumentForm.Path = fileNames[0];
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

            if (PhonebookDocumentsFromDB == null || PhonebookDocumentsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentPhonebook.PhonebookDocuments = PhonebookDocumentsFromDB;
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
