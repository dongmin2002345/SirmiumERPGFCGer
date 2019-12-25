using Ninject;
using ServiceInterfaces.Abstractions.Common.Invoices;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.Invoices;
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

namespace SirmiumERPGFC.Views.Invoices
{
    /// <summary>
    /// Interaction logic for Invoice_Item_AddEdit.xaml
    /// </summary>
    public partial class Invoice_Item_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IInvoiceService invoiceService;
        #endregion


        #region Event
        public event InvoiceHandler InvoiceCreatedUpdated;
        #endregion


        #region CurrentInvoice
        private InvoiceViewModel _CurrentInvoice = new InvoiceViewModel();

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


        #region InvoiceItemsFromDB
        private ObservableCollection<InvoiceItemViewModel> _InvoiceItemsFromDB;

        public ObservableCollection<InvoiceItemViewModel> InvoiceItemsFromDB
        {
            get { return _InvoiceItemsFromDB; }
            set
            {
                if (_InvoiceItemsFromDB != value)
                {
                    _InvoiceItemsFromDB = value;
                    NotifyPropertyChanged("InvoiceItemsFromDB");
                }
            }
        }
        #endregion

        #region CurrentInvoiceItemForm
        private InvoiceItemViewModel _CurrentInvoiceItemForm = new InvoiceItemViewModel();

        public InvoiceItemViewModel CurrentInvoiceItemForm
        {
            get { return _CurrentInvoiceItemForm; }
            set
            {
                if (_CurrentInvoiceItemForm != value)
                {
                    _CurrentInvoiceItemForm = value;
                    NotifyPropertyChanged("CurrentInvoiceItemForm");
                }
            }
        }
        #endregion

        #region CurrentInvoiceItemDG
        private InvoiceItemViewModel _CurrentInvoiceItemDG;

        public InvoiceItemViewModel CurrentInvoiceItemDG
        {
            get { return _CurrentInvoiceItemDG; }
            set
            {
                if (_CurrentInvoiceItemDG != value)
                {
                    _CurrentInvoiceItemDG = value;
                    NotifyPropertyChanged("CurrentInvoiceItemDG");
                }
            }
        }
        #endregion

        #region InvoiceItemDataLoading
        private bool _InvoiceItemDataLoading;

        public bool InvoiceItemDataLoading
        {
            get { return _InvoiceItemDataLoading; }
            set
            {
                if (_InvoiceItemDataLoading != value)
                {
                    _InvoiceItemDataLoading = value;
                    NotifyPropertyChanged("InvoiceItemDataLoading");
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

        public Invoice_Item_AddEdit(InvoiceViewModel invoice)
        {
            invoiceService = DependencyResolver.Kernel.Get<IInvoiceService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentInvoice = invoice;
            CurrentInvoiceItemForm = new InvoiceItemViewModel();
            CurrentInvoiceItemForm.Identifier = Guid.NewGuid();
            CurrentInvoiceItemForm.ItemStatus = ItemStatus.Added;
            if (CurrentInvoice.Vat?.Amount != null)
            {
                CurrentInvoiceItemForm.PDVPercent = CurrentInvoice.Vat.Amount;
            }
            if (CurrentInvoice.Discount?.Amount != null)
            {
                CurrentInvoiceItemForm.Discount = CurrentInvoice.Discount.Amount;
            }
            if (CurrentInvoice.CurrencyExchangeRate != null)
                CurrentInvoiceItemForm.ExchangeRate = CurrentInvoice.CurrencyExchangeRate;

            Thread displayThread = new Thread(() => DisplayInvoiceItemData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayInvoiceItemData()
        {
            InvoiceItemDataLoading = true;

            InvoiceItemListResponse response = new InvoiceItemSQLiteRepository()
                .GetInvoiceItemsByInvoice(MainWindow.CurrentCompanyId, CurrentInvoice.Identifier);

            if (response.Success)
            {
                InvoiceItemsFromDB = new ObservableCollection<InvoiceItemViewModel>(
                    response.InvoiceItems ?? new List<InvoiceItemViewModel>());
            }
            else
            {
                InvoiceItemsFromDB = new ObservableCollection<InvoiceItemViewModel>();
            }

            InvoiceItemDataLoading = false;
        }

        private void DgInvoiceItems_LoadingRow(object sender, DataGridRowEventArgs e)
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

            //if (CurrentInvoiceItemForm.Note == null)
            //{
            //    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Napomena"));
            //    return;
            //}

            #endregion
            Thread th = new Thread(() =>
            {
                SubmitButtonEnabled = false;
                CurrentInvoiceItemForm.Invoice = CurrentInvoice;

                CurrentInvoiceItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentInvoiceItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                new InvoiceItemSQLiteRepository().Delete(CurrentInvoiceItemForm.Identifier);

                var response = new InvoiceItemSQLiteRepository().Create(CurrentInvoiceItemForm);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = response.Message;

                    CurrentInvoiceItemForm = new InvoiceItemViewModel();
                    CurrentInvoiceItemForm.Identifier = Guid.NewGuid();
                    CurrentInvoiceItemForm.ItemStatus = ItemStatus.Added;
                    CurrentInvoiceItemForm.IsSynced = false;
                    if (CurrentInvoice.Vat?.Amount != null)
                    {
                        CurrentInvoiceItemForm.PDVPercent = CurrentInvoice.Vat.Amount;
                    }
                    if (CurrentInvoice.Discount?.Amount != null)
                    {
                        CurrentInvoiceItemForm.Discount = CurrentInvoice.Discount.Amount;
                    }
                    if (CurrentInvoice.CurrencyExchangeRate != null)
                        CurrentInvoiceItemForm.ExchangeRate = CurrentInvoice.CurrencyExchangeRate;
                    return;
                }
                CurrentInvoiceItemForm = new InvoiceItemViewModel() { Discount = CurrentInvoice?.Discount?.Amount ?? 0 };
                CurrentInvoiceItemForm.Identifier = Guid.NewGuid();
                CurrentInvoiceItemForm.ItemStatus = ItemStatus.Added;
                CurrentInvoiceItemForm.IsSynced = false;
                if (CurrentInvoice.Vat?.Amount != null)
                {
                    CurrentInvoiceItemForm.PDVPercent = CurrentInvoice.Vat.Amount;
                }
                if (CurrentInvoice.Discount?.Amount != null)
                {
                    CurrentInvoiceItemForm.Discount = CurrentInvoice.Discount.Amount;
                }
                if (CurrentInvoice.CurrencyExchangeRate != null)
                    CurrentInvoiceItemForm.ExchangeRate = CurrentInvoice.CurrencyExchangeRate;

                //PriceWithPDV * Discount / 100


                InvoiceCreatedUpdated();

                DisplayInvoiceItemData();

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

    

        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentInvoiceItemForm = new InvoiceItemViewModel();
            CurrentInvoiceItemForm.Identifier = CurrentInvoiceItemDG.Identifier;
            CurrentInvoiceItemForm.ItemStatus = ItemStatus.Edited;

            CurrentInvoiceItemForm.IsSynced = CurrentInvoiceItemDG.IsSynced;
            CurrentInvoiceItemForm.Code = CurrentInvoiceItemDG.Code;
            CurrentInvoiceItemForm.Name = CurrentInvoiceItemDG.Name;
            CurrentInvoiceItemForm.UnitOfMeasure = CurrentInvoiceItemDG.UnitOfMeasure;
            CurrentInvoiceItemForm.Quantity = CurrentInvoiceItemDG.Quantity;
            CurrentInvoiceItemForm.PriceWithPDV = CurrentInvoiceItemDG.PriceWithPDV;
            CurrentInvoiceItemForm.PriceWithoutPDV = CurrentInvoiceItemDG.PriceWithoutPDV;
            CurrentInvoiceItemForm.Discount = CurrentInvoiceItemDG.Discount;

            CurrentInvoiceItemForm.CurrencyCode = CurrentInvoiceItemDG.CurrencyCode;
            CurrentInvoiceItemForm.PDVPercent = CurrentInvoiceItemDG.PDVPercent;
            CurrentInvoiceItemForm.PDV = CurrentInvoiceItemDG.PDV;
            CurrentInvoiceItemForm.Amount = CurrentInvoiceItemDG.Amount;
            CurrentInvoiceItemForm.CurrencyPriceWithPDV = CurrentInvoiceItemDG.CurrencyPriceWithPDV;
            CurrentInvoiceItemForm.ExchangeRate = CurrentInvoice?.CurrencyExchangeRate;

            CurrentInvoiceItemForm.UpdatedAt = CurrentInvoiceItemDG.UpdatedAt;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new InvoiceItemSQLiteRepository().SetStatusDeleted(CurrentInvoiceItemDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Stavka_je_uspešno_obrisanaUzvičnik"));

                CurrentInvoiceItemForm = new InvoiceItemViewModel() { Discount = CurrentInvoice?.Discount?.Amount ?? 0 };
                CurrentInvoiceItemForm.Identifier = Guid.NewGuid();
                CurrentInvoiceItemForm.ItemStatus = ItemStatus.Added;
                CurrentInvoiceItemForm.IsSynced = false;
                if (CurrentInvoice.Vat?.Amount != null)
                {
                    CurrentInvoiceItemForm.PDVPercent = CurrentInvoice.Vat.Amount;
                }
                if (CurrentInvoice.Discount?.Amount != null)
                {
                    CurrentInvoiceItemForm.Discount = CurrentInvoice.Discount.Amount;
                }
                if (CurrentInvoice.CurrencyExchangeRate != null)
                    CurrentInvoiceItemForm.ExchangeRate = CurrentInvoice.CurrencyExchangeRate;

                CurrentInvoiceItemDG = null;

                InvoiceCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayInvoiceItemData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentInvoiceItemForm = new InvoiceItemViewModel();
            CurrentInvoiceItemForm.Identifier = Guid.NewGuid();
            CurrentInvoiceItemForm.ItemStatus = ItemStatus.Added;
            if (CurrentInvoice.Vat?.Amount != null)
            {
                CurrentInvoiceItemForm.PDVPercent = CurrentInvoice.Vat.Amount;
            }
            if (CurrentInvoice.Discount?.Amount != null)
            {
                CurrentInvoiceItemForm.Discount = CurrentInvoice.Discount.Amount;
            }
            if (CurrentInvoice.CurrencyExchangeRate != null)
                CurrentInvoiceItemForm.ExchangeRate = CurrentInvoice.CurrencyExchangeRate;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (InvoiceItemsFromDB == null || InvoiceItemsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Ne_postoje_stavke_za_proknjižavanje"));
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;


                CurrentInvoice.TotalPrice = (double)InvoiceItemsFromDB.Sum(x => x.Amount);
                CurrentInvoice.TotalPDV = (double)InvoiceItemsFromDB.Sum(x => x.PDV);
                CurrentInvoice.TotalRebate = (double)InvoiceItemsFromDB.Sum(x => x.TotalDiscount);


                var response = new InvoiceSQLiteRepository().Create(CurrentInvoice);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                }

                CurrentInvoice.InvoiceItems = InvoiceItemsFromDB;
                response = invoiceService.Create(CurrentInvoice);
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
            InvoiceCreatedUpdated();

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

        //private void txtPriceWithoutPDV_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if(CurrentInvoiceItemForm.PriceWithoutPDV != 0)
        //    {
        //        CurrentInvoiceItemForm.Amount = CurrentInvoiceItemForm.PriceWithoutPDV * CurrentInvoiceItemForm.Quantity
        //            + (CurrentInvoiceItemForm.PriceWithoutPDV * CurrentInvoiceItemForm.Quantity * CurrentInvoiceItemForm.PDV / 100);
        //        CurrentInvoiceItemForm.PriceWithPDV = CurrentInvoiceItemForm.Amount/ CurrentInvoiceItemForm.Quantity;
        //    }
        //}
    }
}
