using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.OutputInvoices
{
    public class OutputInvoiceViewModel : BaseEntityViewModel, INotifyPropertyChanged
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

        #region Supplier
        private string _Supplier;

        public string Supplier
        {
            get { return _Supplier; }
            set
            {
                if (_Supplier != value)
                {
                    _Supplier = value;
                    NotifyPropertyChanged("Supplier");
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
        //#region Construction
        //private string _Construction;

        //public string Construction
        //{
        //    get { return _Construction; }
        //    set
        //    {
        //        if (_Construction != value)
        //        {
        //            _Construction = value;
        //            NotifyPropertyChanged("Construction");
        //        }
        //    }
        //}
        //#endregion

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

        //#region InvoiceType
        //private string _InvoiceType;

        //public string InvoiceType
        //{
        //    get { return _InvoiceType; }
        //    set
        //    {
        //        if (_InvoiceType != value)
        //        {
        //            _InvoiceType = value;
        //            NotifyPropertyChanged("InvoiceType");
        //        }
        //    }
        //}
        //#endregion

        #region AmountNet
        private decimal _AmountNet;

        public decimal AmountNet
        {
            get { return _AmountNet; }
            set
            {
                if (_AmountNet != value)
                {
                    _AmountNet = value;
                    NotifyPropertyChanged("AmountNet");
                }
            }
        }
        #endregion

        #region PdvPercent
        private int _PdvPercent;

        public int PdvPercent
        {
            get { return _PdvPercent; }
            set
            {
                if (_PdvPercent != value)
                {
                    _PdvPercent = value;
                    NotifyPropertyChanged("PdvPercent");
                    Pdv = (AmountNet * PdvPercent) / 100;
                }
            }
        }
        #endregion

        #region Pdv
        private decimal _Pdv;

        public decimal Pdv
        {
            get { return _Pdv; }
            set
            {
                if (_Pdv != value)
                {
                    _Pdv = value;
                    NotifyPropertyChanged("Pdv");
                    AmountGross = AmountNet + Pdv;
                }
            }
        }
        #endregion

        #region AmountGross
        private decimal _AmountGross;

        public decimal AmountGross
        {
            get { return _AmountGross; }
            set
            {
                if (_AmountGross != value)
                {
                    _AmountGross = value;
                    NotifyPropertyChanged("AmountGross");
                }
            }
        }
        #endregion

        #region Currency
        private int _Currency;

        public int Currency
        {
            get { return _Currency; }
            set
            {
                if (_Currency != value)
                {
                    _Currency = value;
                    NotifyPropertyChanged("Currency");
                    DateOfPayment = InvoiceDate.AddDays(Currency);
                }
            }
        }
        #endregion

        #region DateOfPayment
        private DateTime _DateOfPayment = DateTime.Now;

        public DateTime DateOfPayment
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
        private string _Status;

        public string Status
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

        //#region TrafficDate
        //private DateTime _TrafficDate = DateTime.Now;

        //public DateTime TrafficDate
        //{
        //    get { return _TrafficDate; }
        //    set
        //    {
        //        if (_TrafficDate != value)
        //        {
        //            _TrafficDate = value;
        //            NotifyPropertyChanged("TrafficDate");
        //        }
        //    }
        //}
        //#endregion

        //#region Price
        //private decimal _Price;

        //public decimal Price
        //{
        //    get { return _Price; }
        //    set
        //    {
        //        if (_Price != value)
        //        {
        //            _Price = value;
        //            NotifyPropertyChanged("Price");
        //        }
        //    }
        //}
        //#endregion

        //#region Rebate
        //private decimal _Rebate;

        //public decimal Rebate
        //{
        //    get { return _Rebate; }
        //    set
        //    {
        //        if (_Rebate != value)
        //        {
        //            _Rebate = value;
        //            NotifyPropertyChanged("Rebate");
        //        }
        //    }
        //}
        //#endregion

        //#region RebateValue
        //private decimal _RebateValue;

        //public decimal RebateValue
        //{
        //    get { return _RebateValue; }
        //    set
        //    {
        //        if (_RebateValue != value)
        //        {
        //            _RebateValue = value;
        //            NotifyPropertyChanged("RebateValue");
        //        }
        //    }
        //}
        //#endregion

        //#region Base
        //private decimal _Base;

        //public decimal Base
        //{
        //    get { return _Base; }
        //    set
        //    {
        //        if (_Base != value)
        //        {
        //            _Base = value;
        //            NotifyPropertyChanged("Base");
        //        }
        //    }
        //}
        //#endregion

        //#region PDV
        //private decimal _PDV;

        //public decimal PDV
        //{
        //    get { return _PDV; }
        //    set
        //    {
        //        if (_PDV != value)
        //        {
        //            _PDV = value;
        //            NotifyPropertyChanged("PDV");
        //        }
        //    }
        //}
        //#endregion

        //#region Total
        //private decimal _Total;

        //public decimal Total
        //{
        //    get { return _Total; }
        //    set
        //    {
        //        if (_Total != value)
        //        {
        //            _Total = value;
        //            NotifyPropertyChanged("Total");
        //        }
        //    }
        //}
        //#endregion

        #region Search

        #region SearchBy_Supplier
        private string _SearchBy_Supplier;

        public string SearchBy_Supplier
        {
            get { return _SearchBy_Supplier; }
            set
            {
                if (_SearchBy_Supplier != value)
                {
                    _SearchBy_Supplier = value;
                    NotifyPropertyChanged("SearchBy_Supplier");
                }
            }
        }
        #endregion

        //#region SearchBy_Construction
        //private string _SearchBy_Construction;

        //public string SearchBy_Construction
        //{
        //    get { return _SearchBy_Construction; }
        //    set
        //    {
        //        if (_SearchBy_Construction != value)
        //        {
        //            _SearchBy_Construction = value;
        //            NotifyPropertyChanged("SearchBy_Construction");
        //        }
        //    }
        //}
        //#endregion

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
