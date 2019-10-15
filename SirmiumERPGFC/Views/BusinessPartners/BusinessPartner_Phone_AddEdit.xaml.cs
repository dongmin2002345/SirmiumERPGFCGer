﻿using Ninject;
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
    /// Interaction logic for BusinessPartner_Phone_AddEdit.xaml
    /// </summary>
    public partial class BusinessPartner_Phone_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerService businessPartnerService;
        IBusinessPartnerPhoneService businessPartnerPhoneService;
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


        #region BusinessPartnerPhonesFromDB
        private ObservableCollection<BusinessPartnerPhoneViewModel> _BusinessPartnerPhonesFromDB;

        public ObservableCollection<BusinessPartnerPhoneViewModel> BusinessPartnerPhonesFromDB
        {
            get { return _BusinessPartnerPhonesFromDB; }
            set
            {
                if (_BusinessPartnerPhonesFromDB != value)
                {
                    _BusinessPartnerPhonesFromDB = value;
                    NotifyPropertyChanged("BusinessPartnerPhonesFromDB");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerPhoneForm
        private BusinessPartnerPhoneViewModel _CurrentBusinessPartnerPhoneForm = new BusinessPartnerPhoneViewModel();

        public BusinessPartnerPhoneViewModel CurrentBusinessPartnerPhoneForm
        {
            get { return _CurrentBusinessPartnerPhoneForm; }
            set
            {
                if (_CurrentBusinessPartnerPhoneForm != value)
                {
                    _CurrentBusinessPartnerPhoneForm = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerPhoneForm");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerPhoneDG
        private BusinessPartnerPhoneViewModel _CurrentBusinessPartnerPhoneDG;

        public BusinessPartnerPhoneViewModel CurrentBusinessPartnerPhoneDG
        {
            get { return _CurrentBusinessPartnerPhoneDG; }
            set
            {
                if (_CurrentBusinessPartnerPhoneDG != value)
                {
                    _CurrentBusinessPartnerPhoneDG = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerPhoneDG");
                }
            }
        }
        #endregion

        #region BusinessPartnerPhoneDataLoading
        private bool _BusinessPartnerPhoneDataLoading;

        public bool BusinessPartnerPhoneDataLoading
        {
            get { return _BusinessPartnerPhoneDataLoading; }
            set
            {
                if (_BusinessPartnerPhoneDataLoading != value)
                {
                    _BusinessPartnerPhoneDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerPhoneDataLoading");
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

        public BusinessPartner_Phone_AddEdit(BusinessPartnerViewModel businessPartner)
        {
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            businessPartnerPhoneService = DependencyResolver.Kernel.Get<IBusinessPartnerPhoneService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentBusinessPartner = businessPartner;
            CurrentBusinessPartnerPhoneForm = new BusinessPartnerPhoneViewModel();
            CurrentBusinessPartnerPhoneForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerPhoneForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayBusinessPartnerPhoneData());
            displayThread.IsBackground = true;
            displayThread.Start();

            txtPhoneContactPersonFirstName.Focus();
        }

        #endregion

        #region Display data

        public void DisplayBusinessPartnerPhoneData()
        {
            BusinessPartnerPhoneDataLoading = true;

            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneSQLiteRepository()
                .GetBusinessPartnerPhonesByBusinessPartner(MainWindow.CurrentCompanyId, CurrentBusinessPartner.Identifier);

            if (response.Success)
            {
                BusinessPartnerPhonesFromDB = new ObservableCollection<BusinessPartnerPhoneViewModel>(
                    response.BusinessPartnerPhones ?? new List<BusinessPartnerPhoneViewModel>());
            }
            else
            {
                BusinessPartnerPhonesFromDB = new ObservableCollection<BusinessPartnerPhoneViewModel>();
            }

            BusinessPartnerPhoneDataLoading = false;
        }

        private void DgBusinessPartnerPhones_LoadingRow(object sender, DataGridRowEventArgs e)
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

            if (CurrentBusinessPartnerPhoneForm.ContactPersonFirstName == null)
            {
                MainWindow.ErrorMessage = "Obavezno polje: Ime";
                return;
            }

            #endregion

            new BusinessPartnerPhoneSQLiteRepository().Delete(CurrentBusinessPartnerPhoneForm.Identifier);

            CurrentBusinessPartnerPhoneForm.BusinessPartner = CurrentBusinessPartner;

            CurrentBusinessPartnerPhoneForm.IsSynced = false;
            CurrentBusinessPartnerPhoneForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentBusinessPartnerPhoneForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new BusinessPartnerPhoneSQLiteRepository().Create(CurrentBusinessPartnerPhoneForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentBusinessPartnerPhoneForm = new BusinessPartnerPhoneViewModel();
                CurrentBusinessPartnerPhoneForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerPhoneForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentBusinessPartnerPhoneForm = new BusinessPartnerPhoneViewModel();
            CurrentBusinessPartnerPhoneForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerPhoneForm.ItemStatus = ItemStatus.Added;

            BusinessPartnerCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayBusinessPartnerPhoneData());
            displayThread.IsBackground = true;
            displayThread.Start();

            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    txtPhoneContactPersonFirstName.Focus();
                })
            );

            SubmitButtonEnabled = true;
        }

        private void btnEditPhone_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerPhoneForm = new BusinessPartnerPhoneViewModel();
            CurrentBusinessPartnerPhoneForm.Identifier = CurrentBusinessPartnerPhoneDG.Identifier;
            CurrentBusinessPartnerPhoneForm.ItemStatus = ItemStatus.Edited;

            CurrentBusinessPartnerPhoneForm.ContactPersonFirstName = CurrentBusinessPartnerPhoneDG.ContactPersonFirstName;
            CurrentBusinessPartnerPhoneForm.ContactPersonLastName = CurrentBusinessPartnerPhoneDG.ContactPersonLastName;
            CurrentBusinessPartnerPhoneForm.Description = CurrentBusinessPartnerPhoneDG.Description;
            CurrentBusinessPartnerPhoneForm.Mobile = CurrentBusinessPartnerPhoneDG.Mobile;
            CurrentBusinessPartnerPhoneForm.Phone = CurrentBusinessPartnerPhoneDG.Phone;
            CurrentBusinessPartnerPhoneForm.Fax = CurrentBusinessPartnerPhoneDG.Fax;
            CurrentBusinessPartnerPhoneForm.Email = CurrentBusinessPartnerPhoneDG.Email;
            CurrentBusinessPartnerPhoneForm.Birthday = CurrentBusinessPartnerPhoneDG.Birthday;
            CurrentBusinessPartnerPhoneForm.Path = CurrentBusinessPartnerPhoneDG.Path;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new BusinessPartnerPhoneSQLiteRepository().SetStatusDeleted(CurrentBusinessPartnerPhoneDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentBusinessPartnerPhoneForm = new BusinessPartnerPhoneViewModel();
                CurrentBusinessPartnerPhoneForm.Identifier = Guid.NewGuid();
                CurrentBusinessPartnerPhoneForm.ItemStatus = ItemStatus.Added;

                CurrentBusinessPartnerPhoneDG = null;

                BusinessPartnerCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayBusinessPartnerPhoneData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelPhone_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartnerPhoneForm = new BusinessPartnerPhoneViewModel();
            CurrentBusinessPartnerPhoneForm.Identifier = Guid.NewGuid();
            CurrentBusinessPartnerPhoneForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (BusinessPartnerPhonesFromDB == null || BusinessPartnerPhonesFromDB.Count == 0)
            {
                MainWindow.WarningMessage = "Ne postoje stavke za proknjižavanje!";
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentBusinessPartner.Phones = BusinessPartnerPhonesFromDB;
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

        private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
            string[] fileNames = dialog.FileNames;

            if (fileNames.Length > 0)
                CurrentBusinessPartnerPhoneForm.Path = fileNames[0];
        }

        private void btnChooseDocument_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDIalog = new System.Windows.Forms.OpenFileDialog();

            fileDIalog.Multiselect = true;
            fileDIalog.FileOk += FileDIalog_FileOk;
            fileDIalog.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            fileDIalog.ShowDialog();
        }

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion
    }
}
