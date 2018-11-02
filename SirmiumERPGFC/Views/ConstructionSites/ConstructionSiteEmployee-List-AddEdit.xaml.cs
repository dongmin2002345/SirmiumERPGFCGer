﻿using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.ConstructionSites;
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

namespace SirmiumERPGFC.Views.ConstructionSites
{
    public partial class ConstructionSiteEmployee_List_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region 
        IEmployeeByConstructionSiteService employeeByConstructionSiteService;
        IEmployeeByConstructionSiteHistoryService employeeByConstructionSiteHistoryService;
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

        #region ContractStartDate
        private DateTime _ContractStartDate = DateTime.Now;

        public DateTime ContractStartDate
        {
            get { return _ContractStartDate; }
            set
            {
                if (_ContractStartDate != value)
                {
                    _ContractStartDate = value;
                    NotifyPropertyChanged("ContractStartDate");
                }
            }
        }
        #endregion

        #region ContractEndDate
        private DateTime _ContractEndDate = DateTime.Now;

        public DateTime ContractEndDate
        {
            get { return _ContractEndDate; }
            set
            {
                if (_ContractEndDate != value)
                {
                    _ContractEndDate = value;
                    NotifyPropertyChanged("ContractEndDate");
                }
            }
        }
        #endregion
        

        #region EmployeesNotOnConstructionSiteFromDB
        private ObservableCollection<EmployeeViewModel> _EmployeesNotOnConstructionSiteFromDB;

        public ObservableCollection<EmployeeViewModel> EmployeesNotOnConstructionSiteFromDB
        {
            get { return _EmployeesNotOnConstructionSiteFromDB; }
            set
            {
                if (_EmployeesNotOnConstructionSiteFromDB != value)
                {
                    _EmployeesNotOnConstructionSiteFromDB = value;
                    NotifyPropertyChanged("EmployeesNotOnConstructionSiteFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeNotOnConstructionSite
        private EmployeeViewModel _CurrentEmployeeNotOnConstructionSite;

        public EmployeeViewModel CurrentEmployeeNotOnConstructionSite
        {
            get { return _CurrentEmployeeNotOnConstructionSite; }
            set
            {
                if (_CurrentEmployeeNotOnConstructionSite != value)
                {
                    _CurrentEmployeeNotOnConstructionSite = value;
                    NotifyPropertyChanged("CurrentEmployeeNotOnConstructionSite");
                }
            }
        }
        #endregion

        #region EmployeeNotOnConstructionSiteSearchObject
        private EmployeeViewModel _EmployeeNotOnConstructionSiteSearchObject = new EmployeeViewModel();

        public EmployeeViewModel EmployeeNotOnConstructionSiteSearchObject
        {
            get { return _EmployeeNotOnConstructionSiteSearchObject; }
            set
            {
                if (_EmployeeNotOnConstructionSiteSearchObject != value)
                {
                    _EmployeeNotOnConstructionSiteSearchObject = value;
                    NotifyPropertyChanged("EmployeeNotOnConstructionSiteSearchObject");
                }
            }
        }
        #endregion

        #region EmployeeNotOnConstructionSiteDataLoading
        private bool _EmployeeNotOnConstructionSiteDataLoading;

        public bool EmployeeNotOnConstructionSiteDataLoading
        {
            get { return _EmployeeNotOnConstructionSiteDataLoading; }
            set
            {
                if (_EmployeeNotOnConstructionSiteDataLoading != value)
                {
                    _EmployeeNotOnConstructionSiteDataLoading = value;
                    NotifyPropertyChanged("EmployeeNotOnConstructionSiteDataLoading");
                }
            }
        }
        #endregion


        #region EmployeesOnConstructionSiteFromDB
        private ObservableCollection<EmployeeViewModel> _EmployeesOnConstructionSiteFromDB;

        public ObservableCollection<EmployeeViewModel> EmployeesOnConstructionSiteFromDB
        {
            get { return _EmployeesOnConstructionSiteFromDB; }
            set
            {
                if (_EmployeesOnConstructionSiteFromDB != value)
                {
                    _EmployeesOnConstructionSiteFromDB = value;
                    NotifyPropertyChanged("EmployeesOnConstructionSiteFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeOnConstructionSite
        private EmployeeViewModel _CurrentEmployeeOnConstructionSite;

        public EmployeeViewModel CurrentEmployeeOnConstructionSite
        {
            get { return _CurrentEmployeeOnConstructionSite; }
            set
            {
                if (_CurrentEmployeeOnConstructionSite != value)
                {
                    _CurrentEmployeeOnConstructionSite = value;
                    NotifyPropertyChanged("CurrentEmployeeOnConstructionSite");
                }
            }
        }
        #endregion

        #region EmployeeOnConstructionSiteSearchObject
        private EmployeeViewModel _EmployeeOnConstructionSiteSearchObject = new EmployeeViewModel();

        public EmployeeViewModel EmployeeOnConstructionSiteSearchObject
        {
            get { return _EmployeeOnConstructionSiteSearchObject; }
            set
            {
                if (_EmployeeOnConstructionSiteSearchObject != value)
                {
                    _EmployeeOnConstructionSiteSearchObject = value;
                    NotifyPropertyChanged("EmployeeOnConstructionSiteSearchObject");
                }
            }
        }
        #endregion

        #region EmployeeOnConstructionSiteDataLoading
        private bool _EmployeeOnConstructionSiteDataLoading;

        public bool EmployeeOnConstructionSiteDataLoading
        {
            get { return _EmployeeOnConstructionSiteDataLoading; }
            set
            {
                if (_EmployeeOnConstructionSiteDataLoading != value)
                {
                    _EmployeeOnConstructionSiteDataLoading = value;
                    NotifyPropertyChanged("EmployeeOnConstructionSiteDataLoading");
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


        #endregion

        #region Constructor

        public ConstructionSiteEmployee_List_AddEdit(ConstructionSiteViewModel constructionSiteViewModel)
        {
            employeeByConstructionSiteService = DependencyResolver.Kernel.Get<IEmployeeByConstructionSiteService>();
            employeeByConstructionSiteHistoryService = DependencyResolver.Kernel.Get<IEmployeeByConstructionSiteHistoryService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentConstructionSite = constructionSiteViewModel;

            Thread displayThread = new Thread(() =>
            {
                DisplayEmployeesNotOnConstructionSiteData();
                DisplayEmployeesOnConstructionSiteData();
            });
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void btnSearchEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        public void DisplayEmployeesOnConstructionSiteData()
        {
            EmployeeOnConstructionSiteDataLoading = true;

            EmployeeListResponse response = new EmployeeSQLiteRepository()
                .GetEmployeesOnConstructionSiteByPage(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier, EmployeeOnConstructionSiteSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                EmployeesOnConstructionSiteFromDB = new ObservableCollection<EmployeeViewModel>(response?.Employees ?? new List<EmployeeViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                EmployeesOnConstructionSiteFromDB = new ObservableCollection<EmployeeViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            EmployeeOnConstructionSiteDataLoading = false;
        }

        public void DisplayEmployeesNotOnConstructionSiteData()
        {
            EmployeeNotOnConstructionSiteDataLoading = true;

            EmployeeListResponse response = new EmployeeSQLiteRepository()
                .GetEmployeesNotOnConstructionSiteByPage(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier, EmployeeOnConstructionSiteSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                EmployeesNotOnConstructionSiteFromDB = new ObservableCollection<EmployeeViewModel>(response?.Employees ?? new List<EmployeeViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                EmployeesNotOnConstructionSiteFromDB = new ObservableCollection<EmployeeViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            EmployeeNotOnConstructionSiteDataLoading = false;
        }

        #endregion

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeNotOnConstructionSite == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Radnik bez gradilišta";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                EmployeeByConstructionSiteViewModel employeeByConstructionSite = new EmployeeByConstructionSiteViewModel()
                {
                    Identifier = Guid.NewGuid(),
                    Employee = CurrentEmployeeNotOnConstructionSite,
                    ConstructionSite = CurrentConstructionSite,
                    StartDate = ContractStartDate,
                    Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                    CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId }
                };

                EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteSQLiteRepository().Create(employeeByConstructionSite);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    return;
                }

                response = employeeByConstructionSiteService.Create(employeeByConstructionSite);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    return;
                }


                DisplayEmployeesNotOnConstructionSiteData();
                DisplayEmployeesOnConstructionSiteData();
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentEmployeeOnConstructionSite == null)
            {
                MainWindow.WarningMessage = "Obavezno polje: Radnik na odabranom gradilištu";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                EmployeeByConstructionSiteListResponse listResponse = new EmployeeByConstructionSiteSQLiteRepository().GetByConstructionSite(CurrentConstructionSite.Identifier);
                EmployeeByConstructionSiteViewModel employeeByConstructionSite = listResponse.EmployeeByConstructionSites.FirstOrDefault(x => x.Employee.Identifier == CurrentEmployeeOnConstructionSite.Identifier);
                EmployeeByConstructionSiteResponse response = employeeByConstructionSiteService.Delete(employeeByConstructionSite.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    return;
                }

                response = new EmployeeByConstructionSiteSQLiteRepository().Delete(CurrentEmployeeOnConstructionSite.Identifier, CurrentConstructionSite.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                    return;
                }

                //response = employeeByConstructionSiteService.Create(employeeByConstructionSite);
                //if (!response.Success)
                //{
                //    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                //    return;
                //}


                DisplayEmployeesNotOnConstructionSiteData();
                DisplayEmployeesOnConstructionSiteData();
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.CloseFlyout(this);
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        public void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
