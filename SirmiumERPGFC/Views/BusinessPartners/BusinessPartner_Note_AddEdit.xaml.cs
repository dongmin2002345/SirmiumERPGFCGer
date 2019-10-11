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
    /// Interaction logic for BusinessPartner_Note_AddEdit.xaml
    /// </summary>
    public partial class BusinessPartner_Note_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerService businessPartnerService;
        IBusinessPartnerNoteService businessPartnerNoteService;
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


        #region BusinessPartnerNotesFromDB
        private ObservableCollection<BusinessPartnerNoteViewModel> _BusinessPartnerNotesFromDB;

        public ObservableCollection<BusinessPartnerNoteViewModel> BusinessPartnerNotesFromDB
        {
            get { return _BusinessPartnerNotesFromDB; }
            set
            {
                if (_BusinessPartnerNotesFromDB != value)
                {
                    _BusinessPartnerNotesFromDB = value;
                    NotifyPropertyChanged("BusinessPartnerNotesFromDB");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerNoteForm
        private BusinessPartnerNoteViewModel _CurrentBusinessPartnerNoteForm = new BusinessPartnerNoteViewModel() { NoteDate = DateTime.Now };

        public BusinessPartnerNoteViewModel CurrentBusinessPartnerNoteForm
        {
            get { return _CurrentBusinessPartnerNoteForm; }
            set
            {
                if (_CurrentBusinessPartnerNoteForm != value)
                {
                    _CurrentBusinessPartnerNoteForm = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerNoteForm");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerNoteDG
        private BusinessPartnerNoteViewModel _CurrentBusinessPartnerNoteDG;

        public BusinessPartnerNoteViewModel CurrentBusinessPartnerNoteDG
        {
            get { return _CurrentBusinessPartnerNoteDG; }
            set
            {
                if (_CurrentBusinessPartnerNoteDG != value)
                {
                    _CurrentBusinessPartnerNoteDG = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerNoteDG");
                }
            }
        }
        #endregion

        #region BusinessPartnerNoteDataLoading
        private bool _BusinessPartnerNoteDataLoading;

        public bool BusinessPartnerNoteDataLoading
        {
            get { return _BusinessPartnerNoteDataLoading; }
            set
            {
                if (_BusinessPartnerNoteDataLoading != value)
                {
                    _BusinessPartnerNoteDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerNoteDataLoading");
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

        public BusinessPartner_Note_AddEdit(BusinessPartnerViewModel businessPartner)
        {
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            businessPartnerNoteService = DependencyResolver.Kernel.Get<IBusinessPartnerNoteService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentBusinessPartner = businessPartner;
            CurrentBusinessPartnerNoteForm = new BusinessPartnerNoteViewModel();
            CurrentBusinessPartnerNoteForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerNoteForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayBusinessPartnerNoteData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayBusinessPartnerNoteData()
        {
            BusinessPartnerNoteDataLoading = true;

            BusinessPartnerNoteListResponse response = new BusinessPartnerNoteSQLiteRepository()
                .GetBusinessPartnerNotesByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                BusinessPartnerNotesFromDB = new ObservableCollection<BusinessPartnerNoteViewModel>(
                    response.BusinessPartnerNotes ?? new List<BusinessPartnerNoteViewModel>());
            }
            else
            {
                BusinessPartnerNotesFromDB = new ObservableCollection<BusinessPartnerNoteViewModel>();
            }

            BusinessPartnerNoteDataLoading = false;
        }

        private void DgBusinessPartnerNotes_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentBusinessPartnerNoteForm.Note == null)
            {
                MainWindow.ErrorMessage = "Obavezno polje: Napomena";
                return;
            }

            #endregion

            new BusinessPartnerNoteSQLiteRepository().Delete(CurrentBusinessPartnerNoteForm.Identifier);

            CurrentBusinessPartnerNoteForm.BusinessPartner = CurrentBusinessPartner;

            CurrentBusinessPartnerNoteForm.IsSynced = false;
            CurrentBusinessPartnerNoteForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentBusinessPartnerNoteForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new BusinessPartnerNoteSQLiteRepository().Create(CurrentBusinessPartnerNoteForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentBusinessPartnerNoteForm = new BusinessPartnerNoteViewModel();
                CurrentBusinessPartnerNoteForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerNoteForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentBusinessPartnerNoteForm = new BusinessPartnerNoteViewModel();
            CurrentBusinessPartnerNoteForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerNoteForm.ItemStatus = ItemStatus.Added;

            BusinessPartnerCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayBusinessPartnerNoteData());
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
            CurrentBusinessPartnerNoteForm = new BusinessPartnerNoteViewModel();
            CurrentBusinessPartnerNoteForm.Identifier = CurrentBusinessPartnerNoteDG.Identifier;
            CurrentBusinessPartnerNoteForm.ItemStatus = ItemStatus.Edited;

            CurrentBusinessPartnerNoteForm.Note = CurrentBusinessPartnerNoteDG.Note;
            CurrentBusinessPartnerNoteForm.NoteDate = CurrentBusinessPartnerNoteDG.NoteDate;

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new BusinessPartnerNoteSQLiteRepository().SetStatusDeleted(CurrentBusinessPartnerNoteDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentBusinessPartnerNoteForm = new BusinessPartnerNoteViewModel();
                CurrentBusinessPartnerNoteForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerNoteForm.ItemStatus = ItemStatus.Added;

                CurrentBusinessPartnerNoteDG = null;

                BusinessPartnerCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayBusinessPartnerNoteData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerNoteForm = new BusinessPartnerNoteViewModel();
            CurrentBusinessPartnerNoteForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerNoteForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (BusinessPartnerNotesFromDB == null || BusinessPartnerNotesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = "Ne postoje stavke za proknjižavanje!";
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentBusinessPartner.BusinessPartnerNotes = BusinessPartnerNotesFromDB;
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

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion
    }
}
