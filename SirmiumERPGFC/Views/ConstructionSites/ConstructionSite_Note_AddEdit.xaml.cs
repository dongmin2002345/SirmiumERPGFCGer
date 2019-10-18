using Ninject;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.ConstructionSites;
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

namespace SirmiumERPGFC.Views.ConstructionSites
{
    /// <summary>
    /// Interaction logic for ConstructionSite_Note_AddEdit.xaml
    /// </summary>
    public partial class ConstructionSite_Note_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IConstructionSiteService constructionSiteService;
        IConstructionSiteNoteService constructionSiteNoteService;
        #endregion


        #region Event
        public event ConstructionSiteHandler ConstructionSiteCreatedUpdated;
        #endregion


        #region CurrentConstructionSite
        private ConstructionSiteViewModel _CurrentConstructionSite = new ConstructionSiteViewModel();

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

        public ConstructionSite_Note_AddEdit(ConstructionSiteViewModel constructionSite)
        {
            constructionSiteService = DependencyResolver.Kernel.Get<IConstructionSiteService>();
            constructionSiteNoteService = DependencyResolver.Kernel.Get<IConstructionSiteNoteService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentConstructionSite = constructionSite;
            CurrentConstructionSiteNoteForm = new ConstructionSiteNoteViewModel();
            CurrentConstructionSiteNoteForm.Identifier = Guid.NewGuid();
            CurrentConstructionSiteNoteForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayConstructionSiteNoteData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayConstructionSiteNoteData()
        {
            ConstructionSiteNoteDataLoading = true;

            ConstructionSiteNoteListResponse response = new ConstructionSiteNoteSQLiteRepository()
                .GetConstructionSiteNotesByConstructionSite(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier);

            if (response.Success)
            {
                ConstructionSiteNotesFromDB = new ObservableCollection<ConstructionSiteNoteViewModel>(
                    response.ConstructionSiteNotes ?? new List<ConstructionSiteNoteViewModel>());
            }
            else
            {
                ConstructionSiteNotesFromDB = new ObservableCollection<ConstructionSiteNoteViewModel>();
            }

            ConstructionSiteNoteDataLoading = false;
        }

        private void DgConstructionSiteNotes_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentConstructionSiteNoteForm.Note == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Član_porodice"));
                return;
            }

            #endregion
            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;
                CurrentConstructionSiteNoteForm.ConstructionSite = CurrentConstructionSite;

                CurrentConstructionSiteNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentConstructionSiteNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                new ConstructionSiteNoteSQLiteRepository().Delete(CurrentConstructionSiteNoteForm.Identifier);

                var response = new ConstructionSiteNoteSQLiteRepository().Create(CurrentConstructionSiteNoteForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentConstructionSiteNoteForm = new ConstructionSiteNoteViewModel();
                    CurrentConstructionSiteNoteForm.Identifier = Guid.NewGuid();
                    CurrentConstructionSiteNoteForm.ItemStatus = ItemStatus.Added;
                    CurrentConstructionSiteNoteForm.IsSynced = false;

                    return;
                }

                CurrentConstructionSiteNoteForm = new ConstructionSiteNoteViewModel();
                CurrentConstructionSiteNoteForm.Identifier = Guid.NewGuid();
                CurrentConstructionSiteNoteForm.ItemStatus = ItemStatus.Added;
                CurrentConstructionSiteNoteForm.IsSynced = false;

                ConstructionSiteCreatedUpdated();
                DisplayConstructionSiteNoteData();

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
            CurrentConstructionSiteNoteForm = new ConstructionSiteNoteViewModel();
            CurrentConstructionSiteNoteForm.Identifier = CurrentConstructionSiteNoteDG.Identifier;
            CurrentConstructionSiteNoteForm.ItemStatus = ItemStatus.Edited;

            CurrentConstructionSiteNoteForm.Note = CurrentConstructionSiteNoteDG.Note;
            CurrentConstructionSiteNoteForm.NoteDate = CurrentConstructionSiteNoteDG.NoteDate;
            CurrentConstructionSiteNoteForm.IsSynced = CurrentConstructionSiteNoteDG.IsSynced;
            CurrentConstructionSiteNoteForm.UpdatedAt = CurrentConstructionSiteNoteDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new ConstructionSiteNoteSQLiteRepository().SetStatusDeleted(CurrentConstructionSiteNoteDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentConstructionSiteNoteForm = new ConstructionSiteNoteViewModel();
                CurrentConstructionSiteNoteForm.Identifier = Guid.NewGuid();
                CurrentConstructionSiteNoteForm.ItemStatus = ItemStatus.Added;

                CurrentConstructionSiteNoteDG = null;

                ConstructionSiteCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayConstructionSiteNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentConstructionSiteNoteForm = new ConstructionSiteNoteViewModel();
            CurrentConstructionSiteNoteForm.Identifier = Guid.NewGuid();
            CurrentConstructionSiteNoteForm.ItemStatus = ItemStatus.Added;

        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (ConstructionSiteNotesFromDB == null || ConstructionSiteNotesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Ne_postoje_stavke_za_proknjižavanje"));
                return; 
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentConstructionSite.ConstructionSiteNotes = ConstructionSiteNotesFromDB;
                ConstructionSiteResponse response = constructionSiteService.Create(CurrentConstructionSite);
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
            ConstructionSiteCreatedUpdated();

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
