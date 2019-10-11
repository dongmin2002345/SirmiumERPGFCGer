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
    /// Interaction logic for PhysicalPerson_Profession_AddEdit.xaml
    /// </summary>
    public partial class PhysicalPerson_Profession_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhysicalPersonService physicalPersonService;
        IPhysicalPersonProfessionService physicalPersonProfessionService;
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


        #region PhysicalPersonProfessionsFromDB
        private ObservableCollection<PhysicalPersonProfessionViewModel> _PhysicalPersonProfessionsFromDB;

        public ObservableCollection<PhysicalPersonProfessionViewModel> PhysicalPersonProfessionsFromDB
        {
            get { return _PhysicalPersonProfessionsFromDB; }
            set
            {
                if (_PhysicalPersonProfessionsFromDB != value)
                {
                    _PhysicalPersonProfessionsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonProfessionsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonProfessionForm
        private PhysicalPersonProfessionViewModel _CurrentPhysicalPersonProfessionForm = new PhysicalPersonProfessionViewModel();

        public PhysicalPersonProfessionViewModel CurrentPhysicalPersonProfessionForm
        {
            get { return _CurrentPhysicalPersonProfessionForm; }
            set
            {
                if (_CurrentPhysicalPersonProfessionForm != value)
                {
                    _CurrentPhysicalPersonProfessionForm = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonProfessionForm");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonProfessionDG
        private PhysicalPersonProfessionViewModel _CurrentPhysicalPersonProfessionDG;

        public PhysicalPersonProfessionViewModel CurrentPhysicalPersonProfessionDG
        {
            get { return _CurrentPhysicalPersonProfessionDG; }
            set
            {
                if (_CurrentPhysicalPersonProfessionDG != value)
                {
                    _CurrentPhysicalPersonProfessionDG = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonProfessionDG");
                }
            }
        }
        #endregion

        #region PhysicalPersonProfessionDataLoading
        private bool _PhysicalPersonProfessionDataLoading;

        public bool PhysicalPersonProfessionDataLoading
        {
            get { return _PhysicalPersonProfessionDataLoading; }
            set
            {
                if (_PhysicalPersonProfessionDataLoading != value)
                {
                    _PhysicalPersonProfessionDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonProfessionDataLoading");
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

        public PhysicalPerson_Profession_AddEdit(PhysicalPersonViewModel physicalPerson)
        {
            physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();
            physicalPersonProfessionService = DependencyResolver.Kernel.Get<IPhysicalPersonProfessionService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentPhysicalPerson = physicalPerson;
            CurrentPhysicalPersonProfessionForm = new PhysicalPersonProfessionViewModel();
            CurrentPhysicalPersonProfessionForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonProfessionForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayPhysicalPersonProfessionData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayPhysicalPersonProfessionData()
        {
            PhysicalPersonProfessionDataLoading = true;

            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionSQLiteRepository()
                .GetPhysicalPersonProfessionsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonProfessionsFromDB = new ObservableCollection<PhysicalPersonProfessionViewModel>(
                    response.PhysicalPersonProfessions ?? new List<PhysicalPersonProfessionViewModel>());
            }
            else
            {
                PhysicalPersonProfessionsFromDB = new ObservableCollection<PhysicalPersonProfessionViewModel>();
            }

            PhysicalPersonProfessionDataLoading = false;
        }

        private void DgPhysicalPersonProfessions_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentPhysicalPersonProfessionForm.Country.Name == null)
            {
                MainWindow.ErrorMessage = "Obavezno polje: Drzava!";
                return;
            }

            #endregion

            new PhysicalPersonProfessionSQLiteRepository().Delete(CurrentPhysicalPersonProfessionForm.Identifier);

            CurrentPhysicalPersonProfessionForm.PhysicalPerson = CurrentPhysicalPerson;

            CurrentPhysicalPersonProfessionForm.IsSynced = false;
            CurrentPhysicalPersonProfessionForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentPhysicalPersonProfessionForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new PhysicalPersonProfessionSQLiteRepository().Create(CurrentPhysicalPersonProfessionForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentPhysicalPersonProfessionForm = new PhysicalPersonProfessionViewModel();
                CurrentPhysicalPersonProfessionForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonProfessionForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentPhysicalPersonProfessionForm = new PhysicalPersonProfessionViewModel();
            CurrentPhysicalPersonProfessionForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonProfessionForm.ItemStatus = ItemStatus.Added;

            PhysicalPersonCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayPhysicalPersonProfessionData());
            displayThread.IsBackground = true;
            displayThread.Start();

            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    //txtNote.Focus();
                })
            );

            SubmitButtonEnabled = true;
        }

        private void btnEditNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonProfessionForm = new PhysicalPersonProfessionViewModel();
            CurrentPhysicalPersonProfessionForm.Identifier = CurrentPhysicalPersonProfessionDG.Identifier;
            CurrentPhysicalPersonProfessionForm.ItemStatus = ItemStatus.Edited;

            CurrentPhysicalPersonProfessionForm.Country = CurrentPhysicalPersonProfessionDG.Country;
            CurrentPhysicalPersonProfessionForm.Profession = CurrentPhysicalPersonProfessionDG.Profession;

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new PhysicalPersonProfessionSQLiteRepository().SetStatusDeleted(CurrentPhysicalPersonProfessionDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = "Stavka je uspešno obrisana!";

                CurrentPhysicalPersonProfessionForm = new PhysicalPersonProfessionViewModel();
                CurrentPhysicalPersonProfessionForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonProfessionForm.ItemStatus = ItemStatus.Added;

                CurrentPhysicalPersonProfessionDG = null;

                PhysicalPersonCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayPhysicalPersonProfessionData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonProfessionForm = new PhysicalPersonProfessionViewModel();
            CurrentPhysicalPersonProfessionForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonProfessionForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (PhysicalPersonProfessionsFromDB == null || PhysicalPersonProfessionsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = "Ne postoje stavke za proknjižavanje!";
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = " Čuvanje u toku... ";
                SubmitButtonEnabled = false;

                CurrentPhysicalPerson.PhysicalPersonProfessions = PhysicalPersonProfessionsFromDB;
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

