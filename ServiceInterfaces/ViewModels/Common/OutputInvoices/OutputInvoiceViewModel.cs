using ServiceInterfaces.ViewModels.Base;
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
        private int _Code;

        public int Code
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

        #region Construction
        private string _Construction;

        public string Construction
        {
            get { return _Construction; }
            set
            {
                if (_Construction != value)
                {
                    _Construction = value;
                    NotifyPropertyChanged("Construction");
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

        #region BusinessPartner
        private string _BusinessPartner;

        public string BusinessPartner
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

        #region InvoiceType
        private string _InvoiceType;

        public string InvoiceType
        {
            get { return _InvoiceType; }
            set
            {
                if (_InvoiceType != value)
                {
                    _InvoiceType = value;
                    NotifyPropertyChanged("InvoiceType");
                }
            }
        }
        #endregion

        #region Quantity
        private decimal _Quantity;

        public decimal Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    NotifyPropertyChanged("Quantity");
                }
            }
        }
        #endregion

        #region TrafficDate
        private DateTime _TrafficDate = DateTime.Now;

        public DateTime TrafficDate
        {
            get { return _TrafficDate; }
            set
            {
                if (_TrafficDate != value)
                {
                    _TrafficDate = value;
                    NotifyPropertyChanged("TrafficDate");
                }
            }
        }
        #endregion

        #region Price
        private decimal _Price;

        public decimal Price
        {
            get { return _Price; }
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    NotifyPropertyChanged("Price");
                }
            }
        }
        #endregion

        #region Rebate
        private decimal _Rebate;

        public decimal Rebate
        {
            get { return _Rebate; }
            set
            {
                if (_Rebate != value)
                {
                    _Rebate = value;
                    NotifyPropertyChanged("Rebate");
                }
            }
        }
        #endregion

        #region RebateValue
        private decimal _RebateValue;

        public decimal RebateValue
        {
            get { return _RebateValue; }
            set
            {
                if (_RebateValue != value)
                {
                    _RebateValue = value;
                    NotifyPropertyChanged("RebateValue");
                }
            }
        }
        #endregion

        #region Base
        private decimal _Base;

        public decimal Base
        {
            get { return _Base; }
            set
            {
                if (_Base != value)
                {
                    _Base = value;
                    NotifyPropertyChanged("Base");
                }
            }
        }
        #endregion

        #region PDV
        private decimal _PDV;

        public decimal PDV
        {
            get { return _PDV; }
            set
            {
                if (_PDV != value)
                {
                    _PDV = value;
                    NotifyPropertyChanged("PDV");
                }
            }
        }
        #endregion

        #region Total
        private decimal _Total;

        public decimal Total
        {
            get { return _Total; }
            set
            {
                if (_Total != value)
                {
                    _Total = value;
                    NotifyPropertyChanged("Total");
                }
            }
        }
        #endregion

        #region Search

        #region SearchBy_Code
        private string _SearchBy_Code;

        public string SearchBy_Code
        {
            get { return _SearchBy_Code; }
            set
            {
                if (_SearchBy_Code != value)
                {
                    _SearchBy_Code = value;
                    NotifyPropertyChanged("SearchBy_Code");
                }
            }
        }
        #endregion

        #region SearchBy_Construction
        private string _SearchBy_Construction;

        public string SearchBy_Construction
        {
            get { return _SearchBy_Construction; }
            set
            {
                if (_SearchBy_Construction != value)
                {
                    _SearchBy_Construction = value;
                    NotifyPropertyChanged("SearchBy_Construction");
                }
            }
        }
        #endregion

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

        #region SearchBy_DateFrom
        private DateTime? _SearchBy_DateFrom;

        public DateTime? SearchBy_DateFrom
        {
            get { return _SearchBy_DateFrom; }
            set
            {
                if (_SearchBy_DateFrom != value)
                {
                    _SearchBy_DateFrom = value;
                    NotifyPropertyChanged("SearchBy_DateFrom");
                }
            }
        }
        #endregion

        #region SearchBy_DateTo
        private DateTime? _SearchBy_DateTo;

        public DateTime? SearchBy_DateTo
        {
            get { return _SearchBy_DateTo; }
            set
            {
                if (_SearchBy_DateTo != value)
                {
                    _SearchBy_DateTo = value;
                    NotifyPropertyChanged("SearchBy_DateTo");
                }
            }
        }
        #endregion


        #endregion

    }
}
