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

        #region Buyer
        private BusinessPartnerViewModel _Buyer;

        public BusinessPartnerViewModel Buyer
        {
            get { return _Buyer; }
            set
            {
                if (_Buyer != value)
                {
                    _Buyer = value;
                    NotifyPropertyChanged("Buyer");
                }
            }
        }
        #endregion

        #region BuyerName
        private string _BuyerName;

        public string BuyerName
        {
            get { return _BuyerName; }
            set
            {
                if (_BuyerName != value)
                {
                    _BuyerName = value;
                    NotifyPropertyChanged("BuyerName");
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

        #region DueDate
        private DateTime _DueDate = DateTime.Now;

        public DateTime DueDate
        {
            get { return _DueDate; }
            set
            {
                if (_DueDate != value)
                {
                    _DueDate = value;
                    NotifyPropertyChanged("DueDate");
                }
            }
        }
        #endregion

        #region DateOfPayment
        private DateTime? _DateOfPayment;

        public DateTime? DateOfPayment
        {
            get { return _DateOfPayment; }
            set
            {
                if (_DateOfPayment != value)
                {
                    _DateOfPayment = value;
                    NotifyPropertyChanged("DateOfPayment");
                }
            }
        }
        #endregion

        #region Status
        private int _Status;

        public int Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }
        #endregion

        #region StatusDate
        private DateTime _StatusDate = DateTime.Now;

        public DateTime StatusDate
        {
            get { return _StatusDate; }
            set
            {
                if (_StatusDate != value)
                {
                    _StatusDate = value;
                    NotifyPropertyChanged("StatusDate");
                }
            }
        }
        #endregion

        #region Description
        private string _Description;

        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }
        #endregion

        #region CurrencyCode
        private string _CurrencyCode = "EUR";

        public string CurrencyCode
        {
            get { return _CurrencyCode; }
            set
            {
                if (_CurrencyCode != value)
                {
                    _CurrencyCode = value;
                    NotifyPropertyChanged("CurrencyCode");
                }
            }
        }
        #endregion

        #region CurrencyExchangeRate
        private double? _CurrencyExchangeRate;

        public double? CurrencyExchangeRate
        {
            get { return _CurrencyExchangeRate; }
            set
            {
                if (_CurrencyExchangeRate != value)
                {
                    _CurrencyExchangeRate = value;
                    NotifyPropertyChanged("CurrencyExchangeRate");
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

        #region PdvType
        private int? _PdvType;

        public int? PdvType
        {
            get { return _PdvType; }
            set
            {
                if (_PdvType != value)
                {
                    _PdvType = value;

                    if (_PdvType == 2)
                    {
                        TotalPDV = 0;
                        Vat = null;
                    }
                    NotifyPropertyChanged("PdvType");
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

        #region TotalPrice
        private double _TotalPrice;

        public double TotalPrice
        {
            get { return _TotalPrice; }
            set
            {
                if (_TotalPrice != value)
                {
                    _TotalPrice = value;
                    NotifyPropertyChanged("TotalPrice");
                }
            }
        }
        #endregion

        #region TotalPDV
        private double _TotalPDV;

        public double TotalPDV
        {
            get { return _TotalPDV; }
            set
            {
                if (_TotalPDV != value)
                {
                    _TotalPDV = value;
                    NotifyPropertyChanged("TotalPDV");
                }
            }
        }
        #endregion

        #region TotalRebate
        private double _TotalRebate;

        public double TotalRebate
        {
            get { return _TotalRebate; }
            set
            {
                if (_TotalRebate != value)
                {
                    _TotalRebate = value;
                    NotifyPropertyChanged("TotalRebate");
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

        #region SearchBy_DateFrom
        private DateTime? _SearchBy_InvoiceDateFrom;

        public DateTime? SearchBy_InvoiceDateFrom
        {
            get { return _SearchBy_InvoiceDateFrom; }
            set
            {
                if (_SearchBy_InvoiceDateFrom != value)
                {
                    _SearchBy_InvoiceDateFrom = value;
                    NotifyPropertyChanged("SearchBy_InvoiceDateFrom");
                }
            }
        }
        #endregion

        #region SearchBy_DateTo
        private DateTime? _SearchBy_InvoiceDateTo;

        public DateTime? SearchBy_InvoiceDateTo
        {
            get { return _SearchBy_InvoiceDateTo; }
            set
            {
                if (_SearchBy_InvoiceDateTo != value)
                {
                    _SearchBy_InvoiceDateTo = value;
                    NotifyPropertyChanged("SearchBy_InvoiceDateTo");
                }
            }
        }
        #endregion

        #region SearchBy_DateFrom
        private DateTime? _SearchBy_DateOfPaymentFrom;

        public DateTime? SearchBy_DateOfPaymentFrom
        {
            get { return _SearchBy_DateOfPaymentFrom; }
            set
            {
                if (_SearchBy_DateOfPaymentFrom != value)
                {
                    _SearchBy_DateOfPaymentFrom = value;
                    NotifyPropertyChanged("SearchBy_DateOfPaymentFrom");
                }
            }
        }
        #endregion

        #region SearchBy_DateTo
        private DateTime? _SearchBy_DateOfPaymentTo;

        public DateTime? SearchBy_DateOfPaymentTo
        {
            get { return _SearchBy_DateOfPaymentTo; }
            set
            {
                if (_SearchBy_DateOfPaymentTo != value)
                {
                    _SearchBy_DateOfPaymentTo = value;
                    NotifyPropertyChanged("SearchBy_DateOfPaymentTo");
                }
            }
        }
        #endregion

        #endregion

    }
}
