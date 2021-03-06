﻿using Ninject;
using ServiceInterfaces.Abstractions.Common.TaxAdministrations;
using ServiceInterfaces.Messages.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.TaxAdministrations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.TaxAdministrations
{
    public delegate void TaxAdministrationHandler();

    public partial class TaxAdministrationList : UserControl, INotifyPropertyChanged
    {

        #region Attributes
        ITaxAdministrationService taxAdministrationService;

        #region TaxAdministrationsFromDB
        private ObservableCollection<TaxAdministrationViewModel> _TaxAdministrationsFromDB;

        public ObservableCollection<TaxAdministrationViewModel> TaxAdministrationsFromDB
        {
            get { return _TaxAdministrationsFromDB; }
            set
            {
                if (_TaxAdministrationsFromDB != value)
                {
                    _TaxAdministrationsFromDB = value;
                    NotifyPropertyChanged("TaxAdministrationsFromDB");
                }
            }
        }
        #endregion

        #region CurrentTaxAdministration
        private TaxAdministrationViewModel _CurrentTaxAdministration;

        public TaxAdministrationViewModel CurrentTaxAdministration
        {
            get { return _CurrentTaxAdministration; }
            set
            {
                if (_CurrentTaxAdministration != value)
                {
                    _CurrentTaxAdministration = value;
                    NotifyPropertyChanged("CurrentTaxAdministration");
                }
            }
        }
        #endregion

        #region TaxAdministrationSearchObject
        private TaxAdministrationViewModel _TaxAdministrationSearchObject = new TaxAdministrationViewModel();

        public TaxAdministrationViewModel TaxAdministrationSearchObject
        {
            get { return _TaxAdministrationSearchObject; }
            set
            {
                if (_TaxAdministrationSearchObject != value)
                {
                    _TaxAdministrationSearchObject = value;
                    NotifyPropertyChanged("TaxAdministrationSearchObject");
                }
            }
        }
        #endregion

        #region TaxAdministrationDataLoading
        private bool _TaxAdministrationDataLoading = true;

        public bool TaxAdministrationDataLoading
        {
            get { return _TaxAdministrationDataLoading; }
            set
            {
                if (_TaxAdministrationDataLoading != value)
                {
                    _TaxAdministrationDataLoading = value;
                    NotifyPropertyChanged("TaxAdministrationDataLoading");
                }
            }
        }
        #endregion

        #region Pagination data
        int currentPage = 1;
        int itemsPerPage = 50;
        int totalItems = 0;

        #region PaginationDisplay
        private string _PaginationDisplay;

        public string PaginationDisplay
        {
            get { return _PaginationDisplay; }
            set
            {
                if (_PaginationDisplay != value)
                {
                    _PaginationDisplay = value;
                    NotifyPropertyChanged("PaginationDisplay");
                }
            }
        }
        #endregion
        #endregion


        #region RefreshButtonContent
        private string _RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

        public string RefreshButtonContent
        {
            get { return _RefreshButtonContent; }
            set
            {
                if (_RefreshButtonContent != value)
                {
                    _RefreshButtonContent = value;
                    NotifyPropertyChanged("RefreshButtonContent");
                }
            }
        }
        #endregion

        #region RefreshButtonEnabled
        private bool _RefreshButtonEnabled = true;

        public bool RefreshButtonEnabled
        {
            get { return _RefreshButtonEnabled; }
            set
            {
                if (_RefreshButtonEnabled != value)
                {
                    _RefreshButtonEnabled = value;
                    NotifyPropertyChanged("RefreshButtonEnabled");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public TaxAdministrationList()
        {
            // Get required services
            this.taxAdministrationService = DependencyResolver.Kernel.Get<ITaxAdministrationService>();

            InitializeComponent();

            this.DataContext = this;

            Thread displayThread = new Thread(() => SyncData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread syncThread = new Thread(() =>
            {
                SyncData();

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sinhronizovaniUzvičnik"));
            });
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        public void DisplayData()
        {
            TaxAdministrationDataLoading = true;

            TaxAdministrationListResponse response = new TaxAdministrationSQLiteRepository()
                .GetTaxAdministrationsByPage(MainWindow.CurrentCompanyId, TaxAdministrationSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                TaxAdministrationsFromDB = new ObservableCollection<TaxAdministrationViewModel>(response.TaxAdministrations ?? new List<TaxAdministrationViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                TaxAdministrationsFromDB = new ObservableCollection<TaxAdministrationViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " von " + totalItems;

            TaxAdministrationDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = ((string)Application.Current.FindResource("Poreska_Uprava_tritacke"));
            new TaxAdministrationSQLiteRepository().Sync(taxAdministrationService);

            DisplayData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
        }

        private void dgTaxAdministrations_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            TaxAdministrationViewModel TaxAdministration = new TaxAdministrationViewModel();
            TaxAdministration.Identifier = Guid.NewGuid();

            TaxAdministrationAddEdit addEditForm = new TaxAdministrationAddEdit(TaxAdministration, true);
            addEditForm.TaxAdministrationCreatedUpdated += new TaxAdministrationHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Poreska_Uprava")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTaxAdministration == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_izmenuUzvičnik"));
                return;
            }

            TaxAdministrationAddEdit addEditForm = new TaxAdministrationAddEdit(CurrentTaxAdministration, false);
            addEditForm.TaxAdministrationCreatedUpdated += new TaxAdministrationHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Poreska_Uprava")), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTaxAdministration == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_brisanjeUzvičnik"));
                return;
            }

            // Delete data
            var result = taxAdministrationService.Delete(CurrentTaxAdministration.Identifier);
            if (result.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_obrisaniUzvičnik"));

                Thread displayThread = new Thread(() => SyncData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
            {
                MainWindow.ErrorMessage = result.Message;
            }
        }

        #endregion

        #region Pagination

        private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage = 1;

                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnLastPage_Click(object sender, RoutedEventArgs e)
        {
            int lastPage = (int)Math.Ceiling((double)this.totalItems / this.itemsPerPage);
            if (currentPage < lastPage)
            {
                currentPage = lastPage;

                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
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

        //private void btnPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    //OutputInvoicesExcelReport.Show(OutputInvoicesFromDB.ToList());
        //}

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }
}
