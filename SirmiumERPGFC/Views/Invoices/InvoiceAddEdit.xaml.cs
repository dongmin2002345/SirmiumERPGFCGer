﻿using Ninject;
using ServiceInterfaces.Abstractions.Common.Invoices;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Invoices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Invoices;
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
using WpfAppCommonCode.Converters;

namespace SirmiumERPGFC.Views.Invoices
{
    /// <summary>
    /// Interaction logic for InvoiceAddEdit.xaml
    /// </summary>
    public partial class InvoiceAddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IInvoiceService outputInvoiceService;
        #endregion

        #region Events
        public event InvoiceHandler InvoiceCreatedUpdated;
        #endregion

        #region CurrentInvoice
        private InvoiceViewModel _CurrentInvoice;

        public InvoiceViewModel CurrentInvoice
        {
            get { return _CurrentInvoice; }
            set
            {
                if (_CurrentInvoice != value)
                {
                    _CurrentInvoice = value;
                    NotifyPropertyChanged("CurrentInvoice");
                }
            }
        }
        #endregion


        #region StatusOptions
        public ObservableCollection<String> StatusOptions
        {
            get
            {
                return new ObservableCollection<String>(new List<string>() {
                           ChooseStatusConverter.Choose,
                           ChooseStatusConverter.ChooseO,
                           ChooseStatusConverter.ChooseB,
                           ChooseStatusConverter.ChooseM,
                });
            }
        }
        #endregion


        #region CurrentBusinessPartnerInvoice
        private BusinessPartnerViewModel _CurrentBusinessPartnerInvoice;

        public BusinessPartnerViewModel CurrentBusinessPartnerInvoice
        {
            get { return _CurrentBusinessPartnerInvoice; }
            set
            {
                if (_CurrentBusinessPartnerInvoice != value)
                {
                    _CurrentBusinessPartnerInvoice = value;
                    CurrentInvoice.Buyer = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerInvoice");

                    if (CurrentInvoice != null)
                    {
                        if (_CurrentBusinessPartnerInvoice?.Customer != null)
                        {
                            CurrentInvoice.BuyerName = _CurrentBusinessPartnerInvoice?.Customer;
                            ////CurrentInvoice.PIB = _CurrentBusinessPartnerInvoice?.PIB;
                            ////CurrentInvoice.BPName = _CurrentBusinessPartnerInvoice?.Name;
                            ////CurrentInvoice.IsInPDV = _CurrentBusinessPartnerInvoice?.IsInPDV ?? false;
                            //CurrentInvoice.Address = _CurrentBusinessPartnerInvoice?.Address;
                            //CurrentInvoice.City.ZipCode = _CurrentBusinessPartnerInvoice?.City.ZipCode;
                            //CurrentInvoice.City = _CurrentBusinessPartnerInvoice?.City;
                            CurrentInvoice.Vat = _CurrentBusinessPartnerInvoice?.Vat;
                            CurrentInvoice.Discount = _CurrentBusinessPartnerInvoice?.Discount;

                            CurrentInvoice.PdvType = _CurrentBusinessPartnerInvoice?.PdvType;
                            CurrentInvoice.Discount = _CurrentBusinessPartnerInvoice?.Discount;
                            CurrentInvoice.Address = _CurrentBusinessPartnerInvoice?.TaxAdministration?.Address1;
                            CurrentInvoice.DueDate = DateTime.Now.AddDays(_CurrentBusinessPartnerInvoice.DueDate);
                        }
                    }
                }
            }
        }
        #endregion



        #region PdvTypeOptions

        public ObservableCollection<string> PdvTypeOptions
        {
            get
            {
                return new ObservableCollection<string>(new List<string>() {
                    WpfAppCommonCode.Converters.PdvTypeConverter.SA_PDV,
                    WpfAppCommonCode.Converters.PdvTypeConverter.BEZ_PDV,
                    WpfAppCommonCode.Converters.PdvTypeConverter.NIJE_OBVEZNIK,
                });
            }
        }
        #endregion

        #region IsHeaderCreated
        private bool _IsHeaderCreated;

        public bool IsHeaderCreated
        {
            get { return _IsHeaderCreated; }
            set
            {
                if (_IsHeaderCreated != value)
                {
                    _IsHeaderCreated = value;
                    NotifyPropertyChanged("IsHeaderCreated");
                }
            }
        }
        #endregion

        #region IsCreateProcess
        private bool _IsCreateProcess;

        public bool IsCreateProcess
        {
            get { return _IsCreateProcess; }
            set
            {
                if (_IsCreateProcess != value)
                {
                    _IsCreateProcess = value;
                    NotifyPropertyChanged("IsCreateProcess");
                }
            }
        }
        #endregion

        #region IsPopup
        private bool _IsPopup;

        public bool IsPopup
        {
            get { return _IsPopup; }
            set
            {
                if (_IsPopup != value)
                {
                    _IsPopup = value;
                    NotifyPropertyChanged("IsPopup");
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

        public InvoiceAddEdit(InvoiceViewModel invoiceViewModel, bool isCreateProcess, bool isPopup = false)
        {
            // Initialize service
            outputInvoiceService = DependencyResolver.Kernel.Get<IInvoiceService>();

            InitializeComponent();

            this.DataContext = this;

            IsHeaderCreated = !isCreateProcess;

            CurrentInvoice = invoiceViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;

            //CurrentInvoice.PdvType = null;
            CurrentBusinessPartnerInvoice = CurrentInvoice.Buyer;
            if(CurrentInvoice?.PdvType == 2)
            {
                CurrentInvoice.Vat = null;
                CurrentInvoice.TotalPDV = 0;
            }

            if (CurrentBusinessPartnerInvoice != null)
            {
                CurrentBusinessPartnerInvoice.Customer = CurrentInvoice?.BuyerName;
            }
            //CurrentBusinessPartnerInvoice.PIB = CurrentInvoice?.PIB;
            //CurrentBusinessPartnerInvoice.Name = CurrentInvoice?.BPName;
            //CurrentBusinessPartnerInvoice.IsInPDV = CurrentInvoice?.IsInPDV ?? false;
            //CurrentBusinessPartnerInvoice.Vat = CurrentInvoice?.Vat;
            //CurrentBusinessPartnerInvoice.Discount = CurrentInvoice?.Discount;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentBusinessPartnerInvoice == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_polje_poslovni_partner"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;
                CurrentInvoice.Buyer = CurrentBusinessPartnerInvoice;
                CurrentInvoice.IsSynced = false;
                CurrentInvoice.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentInvoice.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                var itemsResponse = new InvoiceItemSQLiteRepository().GetInvoiceItemsByInvoice(MainWindow.CurrentCompanyId, CurrentInvoice.Identifier);

                if(itemsResponse.InvoiceItems != null)
                {
                    CurrentInvoice.InvoiceItems = new ObservableCollection<InvoiceItemViewModel>(itemsResponse?.InvoiceItems ?? new List<InvoiceItemViewModel>());
                    foreach(var item in CurrentInvoice.InvoiceItems)
                    {
                        item.PDVPercent = CurrentInvoice.Vat?.Amount ?? 0;
                        item.ItemStatus = ItemStatus.Edited;
                        new InvoiceItemSQLiteRepository().Delete(item.Identifier);
                        new InvoiceItemSQLiteRepository().Create(item);
                    }
                }

                CurrentInvoice.TotalPrice = CurrentInvoice.InvoiceItems.Sum(x => (double)x.PriceWithPDV);

                InvoiceResponse response = new InvoiceSQLiteRepository().Create(CurrentInvoice);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                }

                response = outputInvoiceService.Create(CurrentInvoice);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik")) + response.Message;
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;

                    new InvoiceSQLiteRepository().Sync(outputInvoiceService);

                    InvoiceCreatedUpdated();

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
            FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        #region Mouse wheel event 

        private void PreviewMouseWheelEv(object sender, System.Windows.Input.MouseWheelEventArgs e)
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

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "") //[CallerMemberName]
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion


    }
}
