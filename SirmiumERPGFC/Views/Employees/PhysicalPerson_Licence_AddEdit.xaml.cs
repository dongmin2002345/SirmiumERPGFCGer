using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Employees;
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

namespace SirmiumERPGFC.Views.Employees
{
    /// <summary>
    /// Interaction logic for PhysicalPerson_Licence_AddEdit.xaml
    /// </summary>
    public partial class PhysicalPerson_Licence_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhysicalPersonService physicalPersonService;
        IPhysicalPersonLicenceService physicalPersonLicenceService;
        #endregion


        #region Event
        public event PhysicalPersonHandler PhysicalPersonCreatedUpdated;
        #endregion


        #region CurrentPhysicalPerson
        private PhysicalPersonViewModel _CurrentPhysicalPerson = new PhysicalPersonViewModel();

        public PhysicalPersonViewModel CurrentPhysicalPerson
        {
            get { return _CurrentPhysicalPerson; }
            set
            {
                if (_CurrentPhysicalPerson != value)
                {
                    _CurrentPhysicalPerson = value;
                    NotifyPropertyChanged("CurrentPhysicalPerson");
                }
            }
        }
        #endregion


        #region PhysicalPersonLicencesFromDB
        private ObservableCollection<PhysicalPersonLicenceViewModel> _PhysicalPersonLicencesFromDB;

        public ObservableCollection<PhysicalPersonLicenceViewModel> PhysicalPersonLicencesFromDB
        {
            get { return _PhysicalPersonLicencesFromDB; }
            set
            {
                if (_PhysicalPersonLicencesFromDB != value)
                {
                    _PhysicalPersonLicencesFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonLicencesFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonLicenceForm
        private PhysicalPersonLicenceViewModel _CurrentPhysicalPersonLicenceForm = new PhysicalPersonLicenceViewModel();

        public PhysicalPersonLicenceViewModel CurrentPhysicalPersonLicenceForm
        {
            get { return _CurrentPhysicalPersonLicenceForm; }
            set
            {
                if (_CurrentPhysicalPersonLicenceForm != value)
                {
                    _CurrentPhysicalPersonLicenceForm = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonLicenceForm");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonLicenceDG
        private PhysicalPersonLicenceViewModel _CurrentPhysicalPersonLicenceDG;

        public PhysicalPersonLicenceViewModel CurrentPhysicalPersonLicenceDG
        {
            get { return _CurrentPhysicalPersonLicenceDG; }
            set
            {
                if (_CurrentPhysicalPersonLicenceDG != value)
                {
                    _CurrentPhysicalPersonLicenceDG = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonLicenceDG");
                }
            }
        }
        #endregion

        #region PhysicalPersonLicenceDataLoading
        private bool _PhysicalPersonLicenceDataLoading;

        public bool PhysicalPersonLicenceDataLoading
        {
            get { return _PhysicalPersonLicenceDataLoading; }
            set
            {
                if (_PhysicalPersonLicenceDataLoading != value)
                {
                    _PhysicalPersonLicenceDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonLicenceDataLoading");
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

        public PhysicalPerson_Licence_AddEdit(PhysicalPersonViewModel physicalPerson)
        {
            physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();
            physicalPersonLicenceService = DependencyResolver.Kernel.Get<IPhysicalPersonLicenceService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentPhysicalPerson = physicalPerson;
            CurrentPhysicalPersonLicenceForm = new PhysicalPersonLicenceViewModel();
            CurrentPhysicalPersonLicenceForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonLicenceForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayPhysicalPersonLicenceData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayPhysicalPersonLicenceData()
        {
            PhysicalPersonLicenceDataLoading = true;

            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceSQLiteRepository()
                .GetPhysicalPersonLicencesByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonLicencesFromDB = new ObservableCollection<PhysicalPersonLicenceViewModel>(
                    response.PhysicalPersonLicences ?? new List<PhysicalPersonLicenceViewModel>());
            }
            else
            {
                PhysicalPersonLicencesFromDB = new ObservableCollection<PhysicalPersonLicenceViewModel>();
            }

            PhysicalPersonLicenceDataLoading = false;
        }

        private void DgPhysicalPersonLicences_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentPhysicalPersonLicenceForm.Country.Name == null)
            {
                MainWindow.ErrorMessage = "Obavezno polje: Drzava!";
                return;
            }

            #endregion

            new PhysicalPersonLicenceSQLiteRepository().Delete(CurrentPhysicalPersonLicenceForm.Identifier);

            CurrentPhysicalPersonLicenceForm.PhysicalPerson = CurrentPhysicalPerson;

            CurrentPhysicalPersonLicenceForm.IsSynced = false;
            CurrentPhysicalPersonLicenceForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentPhysicalPersonLicenceForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new PhysicalPersonLicenceSQLiteRepository().Create(CurrentPhysicalPersonLicenceForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentPhysicalPersonLicenceForm = new PhysicalPersonLicenceViewModel();
                CurrentPhysicalPersonLicenceForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonLicenceForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentPhysicalPersonLicenceForm = new PhysicalPersonLicenceViewModel();
            CurrentPhysicalPersonLicenceForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonLicenceForm.ItemStatus = ItemStatus.Added;

            PhysicalPersonCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayPhysicalPersonLicenceData());
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
            CurrentPhysicalPersonLicenceForm = new PhysicalPersonLicenceViewModel();
            CurrentPhysicalPersonLicenceForm.Identifier = CurrentPhysicalPersonLicenceDG.Identifier;
            CurrentPhysicalPersonLicenceForm.ItemStatus = ItemStatus.Edited;

            CurrentPhysicalPersonLicenceForm.Country = CurrentPhysicalPersonLicenceDG.Country;
            CurrentPhysicalPersonLicenceForm.Licence = CurrentPhysicalPersonLicenceDG.Licence;
            CurrentPhysicalPersonLicenceForm.ValidFrom = CurrentPhysicalPersonLicenceDG.ValidFrom;
            CurrentPhysicalPersonLicenceForm.ValidTo = CurrentPhysicalPersonLicenceDG.ValidTo;

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new PhysicalPersonLicenceSQLiteRepository().SetStatusDeleted(CurrentPhysicalPersonLicenceDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = "Stavka je uspešno obrisana!";

                CurrentPhysicalPersonLicenceForm = new PhysicalPersonLicenceViewModel();
                CurrentPhysicalPersonLicenceForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonLicenceForm.ItemStatus = ItemStatus.Added;

                CurrentPhysicalPersonLicenceDG = null;

                PhysicalPersonCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayPhysicalPersonLicenceData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonLicenceForm = new PhysicalPersonLicenceViewModel();
            CurrentPhysicalPersonLicenceForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonLicenceForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (PhysicalPersonLicencesFromDB == null || PhysicalPersonLicencesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = "Ne postoje stavke za proknjižavanje!";
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = " Čuvanje u toku... ";
                SubmitButtonEnabled = false;

                CurrentPhysicalPerson.PhysicalPersonLicences = PhysicalPersonLicencesFromDB;
                PhysicalPersonResponse response = physicalPersonService.Create(CurrentPhysicalPerson);
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

                    PhysicalPersonCreatedUpdated();

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
            PhysicalPersonCreatedUpdated();

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
