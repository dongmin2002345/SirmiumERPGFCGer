using Ninject;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.ConstructionSites;
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
using WpfAppCommonCode.Converters;

namespace SirmiumERPGFC.Views.ConstructionSites
{
    /// <summary>
    /// Interaction logic for ConstructionSite_List_AddEdit.xaml
    /// </summary>
    public partial class ConstructionSite_List_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IConstructionSiteService constructionSiteService;
        #endregion

        #region Event
        public event ConstructionSiteHandler ConstructionSiteCreatedUpdated;
        #endregion

        #region CurrentConstructionSite
        private ConstructionSiteViewModel _CurrentConstructionSite;

        public ConstructionSiteViewModel CurrentConstructionSite
        {
            get { return _CurrentConstructionSite; }
            set
            {
                if (_CurrentConstructionSite != value)
                {
                    _CurrentConstructionSite = value;
                    NotifyPropertyChanged("CurrentConstructionSite");
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
                    StatusConverter.UObradi,
                    StatusConverter.Odbijen,
                    StatusConverter.Odobren });
            }
        }
        #endregion


        #region ConstructionSiteDocumentsFromDB
        private ObservableCollection<ConstructionSiteDocumentViewModel> _ConstructionSiteDocumentsFromDB;

        public ObservableCollection<ConstructionSiteDocumentViewModel> ConstructionSiteDocumentsFromDB
        {
            get { return _ConstructionSiteDocumentsFromDB; }
            set
            {
                if (_ConstructionSiteDocumentsFromDB != value)
                {
                    _ConstructionSiteDocumentsFromDB = value;
                    NotifyPropertyChanged("ConstructionSiteDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteDocumentForm
        private ConstructionSiteDocumentViewModel _CurrentConstructionSiteDocumentForm = new ConstructionSiteDocumentViewModel() { CreateDate = DateTime.Now };

        public ConstructionSiteDocumentViewModel CurrentConstructionSiteDocumentForm
        {
            get { return _CurrentConstructionSiteDocumentForm; }
            set
            {
                if (_CurrentConstructionSiteDocumentForm != value)
                {
                    _CurrentConstructionSiteDocumentForm = value;
                    NotifyPropertyChanged("CurrentConstructionSiteDocumentForm");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteDocumentDG
        private ConstructionSiteDocumentViewModel _CurrentConstructionSiteDocumentDG;

        public ConstructionSiteDocumentViewModel CurrentConstructionSiteDocumentDG
        {
            get { return _CurrentConstructionSiteDocumentDG; }
            set
            {
                if (_CurrentConstructionSiteDocumentDG != value)
                {
                    _CurrentConstructionSiteDocumentDG = value;
                    NotifyPropertyChanged("CurrentConstructionSiteDocumentDG");
                }
            }
        }
        #endregion

        #region ConstructionSiteDocumentDataLoading
        private bool _ConstructionSiteDocumentDataLoading;

        public bool ConstructionSiteDocumentDataLoading
        {
            get { return _ConstructionSiteDocumentDataLoading; }
            set
            {
                if (_ConstructionSiteDocumentDataLoading != value)
                {
                    _ConstructionSiteDocumentDataLoading = value;
                    NotifyPropertyChanged("ConstructionSiteDocumentDataLoading");
                }
            }
        }
        #endregion


        #region ConstructionSiteNotesFromDB
        private ObservableCollection<ConstructionSiteNoteViewModel> _ConstructionSiteNotesFromDB;

        public ObservableCollection<ConstructionSiteNoteViewModel> ConstructionSiteNotesFromDB
        {
            get { return _ConstructionSiteNotesFromDB; }
            set
            {
                if (_ConstructionSiteNotesFromDB != value)
                {
                    _ConstructionSiteNotesFromDB = value;
                    NotifyPropertyChanged("ConstructionSiteNotesFromDB");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteNoteForm
        private ConstructionSiteNoteViewModel _CurrentConstructionSiteNoteForm = new ConstructionSiteNoteViewModel() { NoteDate = DateTime.Now };

        public ConstructionSiteNoteViewModel CurrentConstructionSiteNoteForm
        {
            get { return _CurrentConstructionSiteNoteForm; }
            set
            {
                if (_CurrentConstructionSiteNoteForm != value)
                {
                    _CurrentConstructionSiteNoteForm = value;
                    NotifyPropertyChanged("CurrentConstructionSiteNoteForm");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteNoteDG
        private ConstructionSiteNoteViewModel _CurrentConstructionSiteNoteDG;

        public ConstructionSiteNoteViewModel CurrentConstructionSiteNoteDG
        {
            get { return _CurrentConstructionSiteNoteDG; }
            set
            {
                if (_CurrentConstructionSiteNoteDG != value)
                {
                    _CurrentConstructionSiteNoteDG = value;
                    NotifyPropertyChanged("CurrentConstructionSiteNoteDG");
                }
            }
        }
        #endregion

        #region ConstructionSiteNoteDataLoading
        private bool _ConstructionSiteNoteDataLoading;

        public bool ConstructionSiteNoteDataLoading
        {
            get { return _ConstructionSiteNoteDataLoading; }
            set
            {
                if (_ConstructionSiteNoteDataLoading != value)
                {
                    _ConstructionSiteNoteDataLoading = value;
                    NotifyPropertyChanged("ConstructionSiteNoteDataLoading");
                }
            }
        }
        #endregion

        #region ConstructionSiteCalculationsFromDB
        private ObservableCollection<ConstructionSiteCalculationViewModel> _ConstructionSiteCalculationsFromDB;

        public ObservableCollection<ConstructionSiteCalculationViewModel> ConstructionSiteCalculationsFromDB
        {
            get { return _ConstructionSiteCalculationsFromDB; }
            set
            {
                if (_ConstructionSiteCalculationsFromDB != value)
                {
                    _ConstructionSiteCalculationsFromDB = value;
                    NotifyPropertyChanged("ConstructionSiteCalculationsFromDB");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteCalculationForm
        private ConstructionSiteCalculationViewModel _CurrentConstructionSiteCalculationForm = new ConstructionSiteCalculationViewModel();

        public ConstructionSiteCalculationViewModel CurrentConstructionSiteCalculationForm
        {
            get { return _CurrentConstructionSiteCalculationForm; }
            set
            {
                if (_CurrentConstructionSiteCalculationForm != value)
                {
                    _CurrentConstructionSiteCalculationForm = value;
                    NotifyPropertyChanged("CurrentConstructionSiteCalculationForm");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteCalculationDG
        private ConstructionSiteCalculationViewModel _CurrentConstructionSiteCalculationDG;

        public ConstructionSiteCalculationViewModel CurrentConstructionSiteCalculationDG
        {
            get { return _CurrentConstructionSiteCalculationDG; }
            set
            {
                if (_CurrentConstructionSiteCalculationDG != value)
                {
                    _CurrentConstructionSiteCalculationDG = value;
                    NotifyPropertyChanged("CurrentConstructionSiteCalculationDG");
                }
            }
        }
        #endregion

        #region ConstructionSiteCalculationDataLoading
        private bool _ConstructionSiteCalculationDataLoading;

        public bool ConstructionSiteCalculationDataLoading
        {
            get { return _ConstructionSiteCalculationDataLoading; }
            set
            {
                if (_ConstructionSiteCalculationDataLoading != value)
                {
                    _ConstructionSiteCalculationDataLoading = value;
                    NotifyPropertyChanged("ConstructionSiteCalculationDataLoading");
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

        public ConstructionSite_List_AddEdit(ConstructionSiteViewModel constructionSiteViewModel, bool isCreateProcess, bool isPopup = false)
        {
            constructionSiteService = DependencyResolver.Kernel.Get<IConstructionSiteService>();

            // Initialize form components
            InitializeComponent();

            this.DataContext = this;

            IsHeaderCreated = !isCreateProcess;

            CurrentConstructionSite = constructionSiteViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;

            //Thread displayThread = new Thread(() =>
            //{
            //    DisplayDocumentData();
            //    DisplayConstructionSiteNoteData();
            //});
            //displayThread.IsBackground = true;
            //displayThread.Start();
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentConstructionSite?.Name == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv_gradilista"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentConstructionSite.IsSynced = false;
                CurrentConstructionSite.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentConstructionSite.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                ConstructionSiteResponse response = new ConstructionSiteSQLiteRepository().Create(CurrentConstructionSite);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                }

                response = constructionSiteService.Create(CurrentConstructionSite);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik")) + response.Message;
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;

                    new ConstructionSiteSQLiteRepository().Sync(constructionSiteService);

                    ConstructionSiteCreatedUpdated();

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
            FlyoutHelper.CloseFlyout(this);
        }

        #endregion
        //private void DisplayDocumentData()
        //{
        //    ConstructionSiteDocumentDataLoading = true;

        //    ConstructionSiteDocumentListResponse response = new ConstructionSiteDocumentSQLiteRepository()
        //        .GetConstructionSiteDocumentsByConstructionSite(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier);

        //    if (response.Success)
        //    {
        //        ConstructionSiteDocumentsFromDB = new ObservableCollection<ConstructionSiteDocumentViewModel>(
        //            response.ConstructionSiteDocuments ?? new List<ConstructionSiteDocumentViewModel>());
        //    }
        //    else
        //    {
        //        ConstructionSiteDocumentsFromDB = new ObservableCollection<ConstructionSiteDocumentViewModel>();
        //    }

        //    ConstructionSiteDocumentDataLoading = false;
        //}

        //private void DisplayConstructionSiteNoteData()
        //{
        //    ConstructionSiteNoteDataLoading = true;

        //    ConstructionSiteNoteListResponse response = new ConstructionSiteNoteSQLiteRepository()
        //        .GetConstructionSiteNotesByConstructionSite(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier);

        //    if (response.Success)
        //    {
        //        ConstructionSiteNotesFromDB = new ObservableCollection<ConstructionSiteNoteViewModel>(
        //            response.ConstructionSiteNotes ?? new List<ConstructionSiteNoteViewModel>());
        //    }
        //    else
        //    {
        //        ConstructionSiteNotesFromDB = new ObservableCollection<ConstructionSiteNoteViewModel>();
        //    }

        //    ConstructionSiteNoteDataLoading = false;
        //}

        //#region Cancel and Save buttons

        //private void btnSaveHeader_Click(object sender, RoutedEventArgs e)
        //{
        //    #region Validation

        //    //if (CurrentConstructionSite.Code == null)
        //    //{
        //    //    MainWindow.WarningMessage = "Obavezno polje: Sifra";
        //    //    return;
        //    //}

        //    if (String.IsNullOrEmpty(CurrentConstructionSite.Name))
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv_gradilista"));
        //        return;
        //    }

        //    #endregion

        //    CurrentConstructionSite.IsSynced = false;
        //    CurrentConstructionSite.UpdatedAt = DateTime.Now;
        //    CurrentConstructionSite.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
        //    CurrentConstructionSite.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

        //    ConstructionSiteResponse response = new ConstructionSiteSQLiteRepository().Delete(CurrentConstructionSite.Identifier);
        //    response = new ConstructionSiteSQLiteRepository().Create(CurrentConstructionSite);
        //    if (response.Success)
        //    {
        //        MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Zaglavlje_je_uspešno_sačuvanoUzvičnik"));
        //        IsHeaderCreated = true;

        //        txtDocumentName.Focus();
        //    }
        //    else
        //        MainWindow.ErrorMessage = response.Message;
        //}

        //private void btnSave_Click(object sender, RoutedEventArgs e)
        //{
        //    #region Validation

        //    //if (CurrentConstructionSite.Code == null)
        //    //{
        //    //    MainWindow.WarningMessage = "Obavezno polje: Sifra";
        //    //    return;
        //    //}

        //    if (String.IsNullOrEmpty(CurrentConstructionSite.Name))
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv_gradilista"));
        //        return;
        //    }

        //    #endregion

        //    Thread th = new Thread(() =>
        //    {
        //        SaveButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
        //        SaveButtonEnabled = false;

        //        CurrentConstructionSite.IsSynced = false;
        //        CurrentConstructionSite.UpdatedAt = DateTime.Now;
        //        CurrentConstructionSite.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
        //        CurrentConstructionSite.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

        //        ConstructionSiteResponse response = new ConstructionSiteSQLiteRepository().Delete(CurrentConstructionSite.Identifier);
        //        response = new ConstructionSiteSQLiteRepository().Create(CurrentConstructionSite);
        //        if (!response.Success)
        //        {
        //            MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
        //            SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));
        //            SaveButtonEnabled = true;
        //            return;
        //        }

        //        CurrentConstructionSite.ConstructionSiteDocuments = ConstructionSiteDocumentsFromDB;
        //        CurrentConstructionSite.ConstructionSiteNotes = ConstructionSiteNotesFromDB;

        //        response = constructionSiteService.Create(CurrentConstructionSite);
        //        if (!response.Success)
        //        {
        //            MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik")) + response.Message;
        //            SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));
        //            SaveButtonEnabled = true;
        //        }

        //        if (response.Success)
        //        {
        //            new ConstructionSiteSQLiteRepository().UpdateSyncStatus(response.ConstructionSite.Identifier, response.ConstructionSite.Code, response.ConstructionSite.UpdatedAt, response.ConstructionSite.Id, true);
        //            MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
        //            SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));
        //            SaveButtonEnabled = true;

        //            ConstructionSiteCreatedUpdated();

        //            //if (IsCreateProcess)
        //            //{
        //            //    CurrentConstructionSite = new ConstructionSiteViewModel();
        //            //    CurrentConstructionSite.Identifier = Guid.NewGuid();

        //            //    Application.Current.Dispatcher.BeginInvoke(
        //            //        System.Windows.Threading.DispatcherPriority.Normal,
        //            //        new Action(() =>
        //            //        {
        //            //            txtCode.Focus();
        //            //        })
        //            //    );
        //            //}
        //            //else
        //            //{
        //                Application.Current.Dispatcher.BeginInvoke(
        //                    System.Windows.Threading.DispatcherPriority.Normal,
        //                    new Action(() =>
        //                    {
        //                        if (IsPopup)
        //                            FlyoutHelper.CloseFlyoutPopup(this);
        //                        else
        //                            FlyoutHelper.CloseFlyout(this);
        //                    })
        //                );
        //            //}
        //        }

        //    });
        //    th.IsBackground = true;
        //    th.Start();
        //}

        //private void btnCancel_Click(object sender, RoutedEventArgs e)
        //{
        //    if (IsPopup)
        //        FlyoutHelper.CloseFlyoutPopup(this);
        //    else
        //        FlyoutHelper.CloseFlyout(this);
        //}

        //#endregion

        //#region Add, edit, delete and cancel document

        //private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        //{
        //    System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
        //    string[] fileNames = dialog.FileNames;

        //    if (fileNames.Length > 0)
        //        CurrentConstructionSiteDocumentForm.Path = fileNames[0];
        //}

        //private void btnChooseDocument_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.Forms.OpenFileDialog fileDIalog = new System.Windows.Forms.OpenFileDialog();

        //    fileDIalog.Multiselect = true;
        //    fileDIalog.FileOk += FileDIalog_FileOk;
        //    fileDIalog.Filter = "Image Files | *.pdf";
        //    fileDIalog.ShowDialog();
        //}

        //private void btnAddDocument_Click(object sender, RoutedEventArgs e)
        //{
        //    #region Validation

        //    if (String.IsNullOrEmpty(CurrentConstructionSiteDocumentForm.Name))
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv"));
        //        return;
        //    }

        //    if (String.IsNullOrEmpty(CurrentConstructionSiteDocumentForm.Path))
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Putanja"));
        //        return;
        //    }

        //    if (CurrentConstructionSiteDocumentForm.CreateDate == null)
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Datum_kreiranja"));
        //        return;
        //    }

        //    #endregion

        //    // IF update process, first delete item
        //    new ConstructionSiteDocumentSQLiteRepository().Delete(CurrentConstructionSiteDocumentForm.Identifier);

        //    CurrentConstructionSiteDocumentForm.ConstructionSite = CurrentConstructionSite;
        //    CurrentConstructionSiteDocumentForm.Identifier = Guid.NewGuid();
        //    CurrentConstructionSiteDocumentForm.IsSynced = false;
        //    CurrentConstructionSiteDocumentForm.UpdatedAt = DateTime.Now;
        //    CurrentConstructionSiteDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
        //    CurrentConstructionSiteDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

        //    var response = new ConstructionSiteDocumentSQLiteRepository().Create(CurrentConstructionSiteDocumentForm);
        //    if (response.Success)
        //    {
        //        CurrentConstructionSiteDocumentForm = new ConstructionSiteDocumentViewModel();
        //        CurrentConstructionSiteDocumentForm.CreateDate = DateTime.Now;

        //        Thread displayThread = new Thread(() => DisplayDocumentData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();

        //        txtDocumentName.Focus();
        //    }
        //    else
        //        MainWindow.ErrorMessage = response.Message;
        //}

        //private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentConstructionSiteDocumentForm = new ConstructionSiteDocumentViewModel();
        //    CurrentConstructionSiteDocumentForm.CreateDate = DateTime.Now;
        //}

        //private void btnEditDocument_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentConstructionSiteDocumentForm = CurrentConstructionSiteDocumentDG;
        //}

        //private void btnDeleteDocument_Click(object sender, RoutedEventArgs e)
        //{
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("dokument", "");
        //    var showDialog = deleteConfirmationForm.ShowDialog();
        //    if (showDialog != null && showDialog.Value)
        //    {
        //        new ConstructionSiteDocumentSQLiteRepository().Delete(CurrentConstructionSiteDocumentDG.Identifier);

        //        MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Dokument_je_uspešno_obrisanUzvičnik"));

        //        Thread displayThread = new Thread(() => DisplayDocumentData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }

        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //#endregion

        //#region Add, edit, delete and cancel note

        //private void btnAddNote_Click(object sender, RoutedEventArgs e)
        //{
        //    #region Validation

        //    if (String.IsNullOrEmpty(CurrentConstructionSiteNoteForm.Note))
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Napomena"));
        //        return;
        //    }

        //    if (CurrentConstructionSiteNoteForm.NoteDate == null)
        //    {
        //        MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Datum_napomene"));
        //        return;
        //    }

        //    #endregion

        //    // IF update process, first delete item
        //    new ConstructionSiteNoteSQLiteRepository().Delete(CurrentConstructionSiteNoteForm.Identifier);

        //    CurrentConstructionSiteNoteForm.ConstructionSite = CurrentConstructionSite;
        //    CurrentConstructionSiteNoteForm.Identifier = Guid.NewGuid();
        //    CurrentConstructionSiteNoteForm.IsSynced = false;
        //    CurrentConstructionSiteNoteForm.UpdatedAt = DateTime.Now;
        //    CurrentConstructionSiteNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
        //    CurrentConstructionSiteNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

        //    var response = new ConstructionSiteNoteSQLiteRepository().Create(CurrentConstructionSiteNoteForm);
        //    if (response.Success)
        //    {
        //        CurrentConstructionSiteNoteForm = new ConstructionSiteNoteViewModel();
        //        CurrentConstructionSiteNoteForm.NoteDate = DateTime.Now;

        //        Thread displayThread = new Thread(() => DisplayConstructionSiteNoteData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();

        //        txtNote.Focus();
        //    }
        //    else
        //        MainWindow.ErrorMessage = response.Message;
        //}

        //private void btnEditNote_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentConstructionSiteNoteForm = CurrentConstructionSiteNoteDG;
        //}

        //private void btnDeleteNote_Click(object sender, RoutedEventArgs e)
        //{
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation(((string)Application.Current.FindResource("stavku_radnika")), "");
        //    var showDialog = deleteConfirmationForm.ShowDialog();
        //    if (showDialog != null && showDialog.Value)
        //    {
        //        new ConstructionSiteNoteSQLiteRepository().Delete(CurrentConstructionSiteNoteDG.Identifier);

        //        MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_radnika_je_uspešno_obrisanaUzvičnik"));

        //        Thread displayThread = new Thread(() => DisplayConstructionSiteNoteData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }

        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentConstructionSiteNoteForm = new ConstructionSiteNoteViewModel();
        //    CurrentConstructionSiteNoteForm.NoteDate = DateTime.Now;
        //}

        //#endregion

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

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
