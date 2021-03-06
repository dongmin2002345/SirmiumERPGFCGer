﻿using Ninject;
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
    /// Interaction logic for PhysicalPerson_Card_AddEdit.xaml
    /// </summary>
    public partial class PhysicalPerson_Card_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhysicalPersonService physicalPersonService;
        IPhysicalPersonCardService physicalPersonNoteService;
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


        #region PhysicalPersonCardsFromDB
        private ObservableCollection<PhysicalPersonCardViewModel> _PhysicalPersonCardsFromDB;

        public ObservableCollection<PhysicalPersonCardViewModel> PhysicalPersonCardsFromDB
        {
            get { return _PhysicalPersonCardsFromDB; }
            set
            {
                if (_PhysicalPersonCardsFromDB != value)
                {
                    _PhysicalPersonCardsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonCardsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonCardForm
        private PhysicalPersonCardViewModel _CurrentPhysicalPersonCardForm = new PhysicalPersonCardViewModel() { CardDate = DateTime.Now };

        public PhysicalPersonCardViewModel CurrentPhysicalPersonCardForm
        {
            get { return _CurrentPhysicalPersonCardForm; }
            set
            {
                if (_CurrentPhysicalPersonCardForm != value)
                {
                    _CurrentPhysicalPersonCardForm = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonCardForm");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonCardDG
        private PhysicalPersonCardViewModel _CurrentPhysicalPersonCardDG;

        public PhysicalPersonCardViewModel CurrentPhysicalPersonCardDG
        {
            get { return _CurrentPhysicalPersonCardDG; }
            set
            {
                if (_CurrentPhysicalPersonCardDG != value)
                {
                    _CurrentPhysicalPersonCardDG = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonCardDG");
                }
            }
        }
        #endregion

        #region PhysicalPersonCardDataLoading
        private bool _PhysicalPersonCardDataLoading;

        public bool PhysicalPersonCardDataLoading
        {
            get { return _PhysicalPersonCardDataLoading; }
            set
            {
                if (_PhysicalPersonCardDataLoading != value)
                {
                    _PhysicalPersonCardDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonCardDataLoading");
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

        public PhysicalPerson_Card_AddEdit(PhysicalPersonViewModel physicalPerson)
        {
            physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();
            physicalPersonNoteService = DependencyResolver.Kernel.Get<IPhysicalPersonCardService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentPhysicalPerson = physicalPerson;
            CurrentPhysicalPersonCardForm = new PhysicalPersonCardViewModel();
            CurrentPhysicalPersonCardForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonCardForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayPhysicalPersonCardData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayPhysicalPersonCardData()
        {
            PhysicalPersonCardDataLoading = true;

            PhysicalPersonCardListResponse response = new PhysicalPersonCardSQLiteRepository()
                .GetPhysicalPersonCardsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonCardsFromDB = new ObservableCollection<PhysicalPersonCardViewModel>(
                    response.PhysicalPersonCards ?? new List<PhysicalPersonCardViewModel>());
            }
            else
            {
                PhysicalPersonCardsFromDB = new ObservableCollection<PhysicalPersonCardViewModel>();
            }

            PhysicalPersonCardDataLoading = false;
        }

        private void DgPhysicalPersonCards_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentPhysicalPersonCardForm.Description == null)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Opis"));
                return;
            }

            #endregion
            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;
                CurrentPhysicalPersonCardForm.PhysicalPerson = CurrentPhysicalPerson;

                CurrentPhysicalPersonCardForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentPhysicalPersonCardForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                new PhysicalPersonCardSQLiteRepository().Delete(CurrentPhysicalPersonCardForm.Identifier);

                var response = new PhysicalPersonCardSQLiteRepository().Create(CurrentPhysicalPersonCardForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentPhysicalPersonCardForm = new PhysicalPersonCardViewModel();
                    CurrentPhysicalPersonCardForm.Identifier = Guid.NewGuid();
                    CurrentPhysicalPersonCardForm.ItemStatus = ItemStatus.Added;
                    CurrentPhysicalPersonCardForm.IsSynced = false;

                    return;
                }

                CurrentPhysicalPersonCardForm = new PhysicalPersonCardViewModel();
                CurrentPhysicalPersonCardForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonCardForm.ItemStatus = ItemStatus.Added;
                CurrentPhysicalPersonCardForm.IsSynced = false;

                PhysicalPersonCreatedUpdated();
                DisplayPhysicalPersonCardData();

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
            CurrentPhysicalPersonCardForm = new PhysicalPersonCardViewModel();
            CurrentPhysicalPersonCardForm.Identifier = CurrentPhysicalPersonCardDG.Identifier;
            CurrentPhysicalPersonCardForm.ItemStatus = ItemStatus.Edited;

            CurrentPhysicalPersonCardForm.IsSynced = CurrentPhysicalPersonCardDG.IsSynced;
            CurrentPhysicalPersonCardForm.Description = CurrentPhysicalPersonCardDG.Description;
            CurrentPhysicalPersonCardForm.CardDate = CurrentPhysicalPersonCardDG.CardDate;
            CurrentPhysicalPersonCardForm.UpdatedAt = CurrentPhysicalPersonCardDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new PhysicalPersonCardSQLiteRepository().SetStatusDeleted(CurrentPhysicalPersonCardDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentPhysicalPersonCardForm = new PhysicalPersonCardViewModel();
                CurrentPhysicalPersonCardForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonCardForm.ItemStatus = ItemStatus.Added;

                CurrentPhysicalPersonCardDG = null;

                PhysicalPersonCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayPhysicalPersonCardData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonCardForm = new PhysicalPersonCardViewModel();
            CurrentPhysicalPersonCardForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonCardForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (PhysicalPersonCardsFromDB == null || PhysicalPersonCardsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Ne_postoje_stavke_za_proknjižavanje"));
                return;
            }

            #endregion

            Thread th = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentPhysicalPerson.PhysicalPersonCards = PhysicalPersonCardsFromDB;
                PhysicalPersonResponse response = physicalPersonService.Create(CurrentPhysicalPerson);
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
            th.IsBackground = true;
            th.Start();
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
