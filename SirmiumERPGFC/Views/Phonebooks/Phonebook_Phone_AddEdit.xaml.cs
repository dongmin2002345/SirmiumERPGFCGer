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
    /// Interaction logic for Phonebook_Phone_AddEdit.xaml
    /// </summary>
    public partial class Phonebook_Phone_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhonebookService PhonebookService;
        IPhonebookPhoneService PhonebookPhoneService;
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


        #region PhonebookPhonesFromDB
        private ObservableCollection<PhonebookPhoneViewModel> _PhonebookPhonesFromDB;

        public ObservableCollection<PhonebookPhoneViewModel> PhonebookPhonesFromDB
        {
            get { return _PhonebookPhonesFromDB; }
            set
            {
                if (_PhonebookPhonesFromDB != value)
                {
                    _PhonebookPhonesFromDB = value;
                    NotifyPropertyChanged("PhonebookPhonesFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhonebookPhoneForm
        private PhonebookPhoneViewModel _CurrentPhonebookPhoneForm = new PhonebookPhoneViewModel();

        public PhonebookPhoneViewModel CurrentPhonebookPhoneForm
        {
            get { return _CurrentPhonebookPhoneForm; }
            set
            {
                if (_CurrentPhonebookPhoneForm != value)
                {
                    _CurrentPhonebookPhoneForm = value;
                    NotifyPropertyChanged("CurrentPhonebookPhoneForm");
                }
            }
        }
        #endregion

        #region CurrentPhonebookPhoneDG
        private PhonebookPhoneViewModel _CurrentPhonebookPhoneDG;

        public PhonebookPhoneViewModel CurrentPhonebookPhoneDG
        {
            get { return _CurrentPhonebookPhoneDG; }
            set
            {
                if (_CurrentPhonebookPhoneDG != value)
                {
                    _CurrentPhonebookPhoneDG = value;
                    NotifyPropertyChanged("CurrentPhonebookPhoneDG");
                }
            }
        }
        #endregion

        #region PhonebookPhoneDataLoading
        private bool _PhonebookPhoneDataLoading;

        public bool PhonebookPhoneDataLoading
        {
            get { return _PhonebookPhoneDataLoading; }
            set
            {
                if (_PhonebookPhoneDataLoading != value)
                {
                    _PhonebookPhoneDataLoading = value;
                    NotifyPropertyChanged("PhonebookPhoneDataLoading");
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

        public Phonebook_Phone_AddEdit(PhonebookViewModel Phonebook)
        {
            PhonebookService = DependencyResolver.Kernel.Get<IPhonebookService>();
            PhonebookPhoneService = DependencyResolver.Kernel.Get<IPhonebookPhoneService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentPhonebook = Phonebook;
            CurrentPhonebookPhoneForm = new PhonebookPhoneViewModel();
            CurrentPhonebookPhoneForm.Identifier = Guid.NewGuid();
            CurrentPhonebookPhoneForm.ItemStatus = ItemStatus.Added;
            CurrentPhonebookPhoneForm.IsSynced = false;

            Thread displayThread = new Thread(() => DisplayPhonebookPhoneData());
            displayThread.IsBackground = true;
            displayThread.Start();

            txtName.Focus();
        }

        #endregion

        #region Display data

        public void DisplayPhonebookPhoneData()
        {
            PhonebookPhoneDataLoading = true;

            PhonebookPhoneListResponse response = new PhonebookPhoneSQLiteRepository()
                .GetPhonebookPhonesByPhonebook(MainWindow.CurrentCompanyId, CurrentPhonebook.Identifier);

            if (response.Success)
            {
                PhonebookPhonesFromDB = new ObservableCollection<PhonebookPhoneViewModel>(
                    response.PhonebookPhones ?? new List<PhonebookPhoneViewModel>());
            }
            else
            {
                PhonebookPhonesFromDB = new ObservableCollection<PhonebookPhoneViewModel>();
            }

            PhonebookPhoneDataLoading = false;
        }

        private void DgPhonebookPhones_LoadingRow(object sender, DataGridRowEventArgs e)
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

        private void btnAddPhone_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhonebookPhoneForm.Name == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Ime"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;


                CurrentPhonebookPhoneForm.Phonebook = CurrentPhonebook;


                CurrentPhonebookPhoneForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentPhonebookPhoneForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                new PhonebookPhoneSQLiteRepository().Delete(CurrentPhonebookPhoneForm.Identifier);
                var response = new PhonebookPhoneSQLiteRepository().Create(CurrentPhonebookPhoneForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentPhonebookPhoneForm = new PhonebookPhoneViewModel();
                    CurrentPhonebookPhoneForm.Identifier = Guid.NewGuid();
                    CurrentPhonebookPhoneForm.ItemStatus = ItemStatus.Added;
                    CurrentPhonebookPhoneForm.IsSynced = false;
                    return;
                }

                CurrentPhonebookPhoneForm = new PhonebookPhoneViewModel();
                CurrentPhonebookPhoneForm.Identifier = Guid.NewGuid();
                CurrentPhonebookPhoneForm.ItemStatus = ItemStatus.Added;
                CurrentPhonebookPhoneForm.IsSynced = false;
                PhonebookCreatedUpdated();

                DisplayPhonebookPhoneData();

                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        txtName.Focus();
                    })
                );
                SubmitButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }
        private void btnEditPhone_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhonebookPhoneForm = new PhonebookPhoneViewModel();
            CurrentPhonebookPhoneForm.Identifier = CurrentPhonebookPhoneDG.Identifier;
            CurrentPhonebookPhoneForm.ItemStatus = ItemStatus.Edited;

            CurrentPhonebookPhoneForm.IsSynced = CurrentPhonebookPhoneDG.IsSynced;
            CurrentPhonebookPhoneForm.Name = CurrentPhonebookPhoneDG.Name;
            CurrentPhonebookPhoneForm.SurName = CurrentPhonebookPhoneDG.SurName;

            CurrentPhonebookPhoneForm.PhoneNumber = CurrentPhonebookPhoneDG.PhoneNumber;
            CurrentPhonebookPhoneForm.Email = CurrentPhonebookPhoneDG.Email;
  
            CurrentPhonebookPhoneForm.UpdatedAt = CurrentPhonebookPhoneDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;

                var response = new PhonebookPhoneSQLiteRepository().SetStatusDeleted(CurrentPhonebookPhoneDG.Identifier);
                if (response.Success)
                {
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                    PhonebookCreatedUpdated();

                    DisplayPhonebookPhoneData();

                    CurrentPhonebookPhoneForm = new PhonebookPhoneViewModel();
                    CurrentPhonebookPhoneForm.Identifier = Guid.NewGuid();
                    CurrentPhonebookPhoneForm.ItemStatus = ItemStatus.Added;
                    CurrentPhonebookPhoneForm.IsSynced = false;

                    CurrentPhonebookPhoneDG = null;
                }
                else
                    MainWindow.ErrorMessage = response.Message;

                SubmitButtonEnabled = true;
            });
            th.Start();
        }

        private void btnCancelPhone_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhonebookPhoneForm = new PhonebookPhoneViewModel();
            CurrentPhonebookPhoneForm.Identifier = Guid.NewGuid();
            CurrentPhonebookPhoneForm.ItemStatus = ItemStatus.Added;
            CurrentPhonebookPhoneForm.IsSynced = false;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (PhonebookPhonesFromDB == null || PhonebookPhonesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_uneti_osnovne_podatkeUzvičnik"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentPhonebook.PhonebookPhones = PhonebookPhonesFromDB;
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
