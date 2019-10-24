using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Prices;
using ServiceInterfaces.ViewModels.Vats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.Invoices
{
    public class InvoiceViewModel : BaseEntityViewModel, INotifyPropertyChanged
    {
        #region Code
        private string _Code;

        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    NotifyPropertyChanged("Code");
                }
            }
        }
        #endregion

        #region InvoiceNumber
        private string _InvoiceNumber;

        public string InvoiceNumber
        {
            get { return _InvoiceNumber; }
            set
            {
                if (_InvoiceNumber != value)
                {
                    _InvoiceNumber = value;
                    NotifyPropertyChanged("InvoiceNumber");
                }
            }
        }
        #endregion

        #region InvoiceDate
        private DateTime _InvoiceDate = DateTime.Now;

        public DateTime InvoiceDate
        {
            get { return _InvoiceDate; }
            set
            {
                if (_InvoiceDate != value)
                {
                    _InvoiceDate = value;
                    NotifyPropertyChanged("InvoiceDate");
                }
            }
        }
        #endregion

        #region DateOfSupplyOfGoods
        private DateTime _DateOfSupplyOfGoods = DateTime.Now;

        public DateTime DateOfSupplyOfGoods
        {
            get { return _DateOfSupplyOfGoods; }
            set
            {
                if (_DateOfSupplyOfGoods != value)
                {
                    _DateOfSupplyOfGoods = value;
                    NotifyPropertyChanged("DateOfSupplyOfGoods");
                }
            }
        }
        #endregion

        #region BusinessPartner
        private BusinessPartnerViewModel _BusinessPartner;

        public BusinessPartnerViewModel BusinessPartner
        {
            get { return _BusinessPartner; }
            set
            {
                if (_BusinessPartner != value)
                {
                    _BusinessPartner = value;
                    NotifyPropertyChanged("BusinessPartner");
                }
            }
        }
        #endregion

        #region City
        private CityViewModel _City;

        public CityViewModel City
        {
            get { return _City; }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    NotifyPropertyChanged("City");
                }
            }
        }
        #endregion

        #region Municipality
        private MunicipalityViewModel _Municipality;

        public MunicipalityViewModel Municipality
        {
            get { return _Municipality; }
            set
            {
                if (_Municipality != value)
                {
                    _Municipality = value;
                    NotifyPropertyChanged("Municipality");
                }
            }
        }
        #endregion

        #region Discount
        private DiscountViewModel _Discount;

        public DiscountViewModel Discount
        {
            get { return _Discount; }
            set
            {
                if (_Discount != value)
                {
                    _Discount = value;
                    NotifyPropertyChanged("Discount");
                }
            }
        }
        #endregion

        #region Vat
        private VatViewModel _Vat;

        public VatViewModel Vat
        {
            get { return _Vat; }
            set
            {
                if (_Vat != value)
                {
                    _Vat = value;
                    NotifyPropertyChanged("Vat");
                }
            }
        }
        #endregion

        #region Customer
        private string _Customer;

        public string Customer
        {
            get { return _Customer; }
            set
            {
                if (_Customer != value)
                {
                    _Customer = value;
                    NotifyPropertyChanged("Customer");
                }
            }
        }
        #endregion

        #region PIB
        private string _PIB;

        public string PIB
        {
            get { return _PIB; }
            set
            {
                if (_PIB != value)
                {
                    _PIB = value;
                    NotifyPropertyChanged("PIB");
                }
            }
        }
        #endregion

        #region BPName
        private string _BPName;

        public string BPName
        {
            get { return _BPName; }
            set
            {
                if (_BPName != value)
                {
                    _BPName = value;
                    NotifyPropertyChanged("BPName");
                }
            }
        }
        #endregion

        #region Address
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    _Address = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }
        #endregion


        #region Currency
        private DateTime _Currency = DateTime.Now;

        public DateTime Currency
        {
            get { return _Currency; }
            set
            {
                if (_Currency != value)
                {
                    _Currency = value;
                    NotifyPropertyChanged("Currency");
                    //DateOfPayment = InvoiceDate.AddDays(Currency);
                }
            }
        }
        #endregion


        #region InvoiceItems
        private ObservableCollection<InvoiceItemViewModel> _InvoiceItems;

        public ObservableCollection<InvoiceItemViewModel> InvoiceItems
        {
            get { return _InvoiceItems; }
            set
            {
                if (_InvoiceItems != value)
                {
                    _InvoiceItems = value;
                    NotifyPropertyChanged("InvoiceItems");
                }
            }
        }
        #endregion

        #region IsSynced
        private bool _IsSynced;

        public bool IsSynced
        {
            get { return _IsSynced; }
            set
            {
                if (_IsSynced != value)
                {
                    _IsSynced = value;
                    NotifyPropertyChanged("IsSynced");
                }
            }
        }
        #endregion

        #region Search

        #region SearchBy_BusinessPartner
        private string _SearchBy_BusinessPartner;

        public string SearchBy_BusinessPartner
        {
            get { return _SearchBy_BusinessPartner; }
            set
            {
                if (_SearchBy_BusinessPartner != value)
                {
                    _SearchBy_BusinessPartner = value;
                    NotifyPropertyChanged("SearchBy_BusinessPartner");
                }
            }
        }
        #endregion

        #region SearchBy_InvoiceNumber
        private string _SearchBy_InvoiceNumber;

        public string SearchBy_InvoiceNumber
        {
            get { return _SearchBy_InvoiceNumber; }
            set
            {
                if (_SearchBy_InvoiceNumber != value)
                {
                    _SearchBy_InvoiceNumber = value;
                    NotifyPropertyChanged("SearchBy_InvoiceNumber");
                }
            }
        }
        #endregion

        //#region SearchBy_DateFrom
        //private DateTime? _SearchBy_InvoiceDateFrom;

        //public DateTime? SearchBy_InvoiceDateFrom
        //{
        //    get { return _SearchBy_InvoiceDateFrom; }
        //    set
        //    {
        //        if (_SearchBy_InvoiceDateFrom != value)
        //        {
        //            _SearchBy_InvoiceDateFrom = value;
        //            NotifyPropertyChanged("SearchBy_InvoiceDateFrom");
        //        }
        //    }
        //}
        //#endregion

        //#region SearchBy_DateTo
        //private DateTime? _SearchBy_InvoiceDateTo;

        //public DateTime? SearchBy_InvoiceDateTo
        //{
        //    get { return _SearchBy_InvoiceDateTo; }
        //    set
        //    {
        //        if (_SearchBy_InvoiceDateTo != value)
        //        {
        //            _SearchBy_InvoiceDateTo = value;
        //            NotifyPropertyChanged("SearchBy_InvoiceDateTo");
        //        }
        //    }
        //}
        //#endregion

        #endregion

    }
}
