﻿using Ninject;
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
    /// Interaction logic for ConstructionSite_Calculation_AddEdit.xaml
    /// </summary>
    public partial class ConstructionSite_Calculation_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IConstructionSiteService constructionSiteService;
        IConstructionSiteCalculationService constructionSiteCalculationService;
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



        #region IsPaid
        private bool _IsPaid = false;

        public bool IsPaid
        {
            get { return _IsPaid; }
            set
            {
                if (_IsPaid != value)
                {
                    _IsPaid = value;
                    NotifyPropertyChanged("IsPaid");
                }
            }
        }
        #endregion

        #region IsRefunded
        private bool _IsRefunded = false;

        public bool IsRefunded
        {
            get { return _IsRefunded; }
            set
            {
                if (_IsRefunded != value)
                {
                    _IsRefunded = value;
                    NotifyPropertyChanged("Refunded");
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

        public ConstructionSite_Calculation_AddEdit(ConstructionSiteViewModel constructionSite)
        {
            constructionSiteService = DependencyResolver.Kernel.Get<IConstructionSiteService>();
            constructionSiteCalculationService = DependencyResolver.Kernel.Get<IConstructionSiteCalculationService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentConstructionSite = constructionSite;
            CurrentConstructionSiteCalculationForm = new ConstructionSiteCalculationViewModel();
            CurrentConstructionSiteCalculationForm.Identifier = Guid.NewGuid();
            CurrentConstructionSiteCalculationForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayConstructionSiteCalculationData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddCalculation.Focus();
        }

        #endregion

        #region Display data

        public void DisplayConstructionSiteCalculationData()
        {
            ConstructionSiteCalculationDataLoading = true;

            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationSQLiteRepository()
                .GetConstructionSiteCalculationsByConstructionSite(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier);

            if (response.Success)
            {
                ConstructionSiteCalculationsFromDB = new ObservableCollection<ConstructionSiteCalculationViewModel>(
                    response.ConstructionSiteCalculations ?? new List<ConstructionSiteCalculationViewModel>());
            }
            else
            {
                ConstructionSiteCalculationsFromDB = new ObservableCollection<ConstructionSiteCalculationViewModel>();
            }

            ConstructionSiteCalculationDataLoading = false;
        }

        private void DgConstructionSiteCalculations_LoadingRow(object sender, DataGridRowEventArgs e)
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

        private void btnAddCalculation_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            //if (CurrentConstructionSiteCalculationForm.Note == null)
            //{
            //    MainWindow.ErrorMessage = "Obavezno polje: Napomena";
            //    return;
            //}

            #endregion
            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;
                CurrentConstructionSiteCalculationForm.ConstructionSite = CurrentConstructionSite;
                CurrentConstructionSiteCalculationForm.IsPaid = IsPaid;
                CurrentConstructionSiteCalculationForm.IsRefunded = IsRefunded;

                CurrentConstructionSiteCalculationForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentConstructionSiteCalculationForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentConstructionSiteCalculationForm.NewValue =
                    CurrentConstructionSiteCalculationForm.NumOfEmployees *
                    CurrentConstructionSiteCalculationForm.EmployeePrice *
                    CurrentConstructionSiteCalculationForm.NumOfMonths;

                new ConstructionSiteCalculationSQLiteRepository().Delete(CurrentConstructionSiteCalculationForm.Identifier);
               
                var response = new ConstructionSiteCalculationSQLiteRepository().Create(CurrentConstructionSiteCalculationForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentConstructionSiteCalculationForm = new ConstructionSiteCalculationViewModel();
                    CurrentConstructionSiteCalculationForm.Identifier = Guid.NewGuid();
                    CurrentConstructionSiteCalculationForm.ItemStatus = ItemStatus.Added;
                    CurrentConstructionSiteCalculationForm.IsSynced = false;

                    return;
                }

                CurrentConstructionSiteCalculationForm = new ConstructionSiteCalculationViewModel();
                CurrentConstructionSiteCalculationForm.Identifier = Guid.NewGuid();
                CurrentConstructionSiteCalculationForm.ItemStatus = ItemStatus.Added;
                CurrentConstructionSiteCalculationForm.IsSynced = false;

                ConstructionSiteCreatedUpdated();
                DisplayConstructionSiteCalculationData();

                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        txtNumOfEmployees.Focus();
                    })
                );

                SubmitButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnEditCalculation_Click(object sender, RoutedEventArgs e)
        {
            CurrentConstructionSiteCalculationForm = new ConstructionSiteCalculationViewModel();
            CurrentConstructionSiteCalculationForm.Identifier = CurrentConstructionSiteCalculationDG.Identifier;
            CurrentConstructionSiteCalculationForm.ItemStatus = ItemStatus.Edited;

            CurrentConstructionSiteCalculationForm.NumOfEmployees = CurrentConstructionSiteCalculationDG.NumOfEmployees;
            CurrentConstructionSiteCalculationForm.EmployeePrice = CurrentConstructionSiteCalculationDG.EmployeePrice;
            CurrentConstructionSiteCalculationForm.NumOfMonths = CurrentConstructionSiteCalculationDG.NumOfMonths;
            CurrentConstructionSiteCalculationForm.DateCondition = CurrentConstructionSiteCalculationDG.DateCondition;
            CurrentConstructionSiteCalculationForm.IsSynced = CurrentConstructionSiteCalculationDG.IsSynced;
            CurrentConstructionSiteCalculationForm.UpdatedAt = CurrentConstructionSiteCalculationDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new ConstructionSiteCalculationSQLiteRepository().SetStatusDeleted(CurrentConstructionSiteCalculationDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentConstructionSiteCalculationForm = new ConstructionSiteCalculationViewModel();
                CurrentConstructionSiteCalculationForm.Identifier = Guid.NewGuid();
                CurrentConstructionSiteCalculationForm.ItemStatus = ItemStatus.Added;

                CurrentConstructionSiteCalculationDG = null;

                ConstructionSiteCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayConstructionSiteCalculationData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelCalculation_Click(object sender, RoutedEventArgs e)
        {
            CurrentConstructionSiteCalculationForm = new ConstructionSiteCalculationViewModel();
            CurrentConstructionSiteCalculationForm.Identifier = Guid.NewGuid();
            CurrentConstructionSiteCalculationForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (ConstructionSiteCalculationsFromDB == null || ConstructionSiteCalculationsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Ne_postoje_stavke_za_proknjižavanje"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentConstructionSite.ConstructionSiteCalculations = ConstructionSiteCalculationsFromDB;
                var test = CurrentConstructionSite;
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

        //private void cbIsPaid_CheckedChanged(object sender, RoutedEventArgs e)
        //{

        //}

        //private void cbIsFounded_CheckedChanged(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
