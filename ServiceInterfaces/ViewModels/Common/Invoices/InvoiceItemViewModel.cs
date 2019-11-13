using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.Invoices
{
    public class InvoiceItemViewModel : BaseEntityViewModel
    {
        #region Invoice
        private InvoiceViewModel _Invoice;

        public InvoiceViewModel Invoice
        {
            get { return _Invoice; }
            set
            {
                if (_Invoice != value)
                {
                    _Invoice = value;
                    NotifyPropertyChanged("Invoice");
                }
            }
        }
        #endregion

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

        #region Name
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }
        #endregion

        #region UnitOfMeasure
        private string _UnitOfMeasure;

        public string UnitOfMeasure
        {
            get { return _UnitOfMeasure; }
            set
            {
                if (_UnitOfMeasure != value)
                {
                    _UnitOfMeasure = value;
                    NotifyPropertyChanged("UnitOfMeasure");
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
                    basePrice = PriceWithoutPDV * Quantity;
                    PriceWithPDV = basePrice + (basePrice * PDV / 100);
                    rabat = PriceWithPDV * Discount / 100;
                    Amount = PriceWithPDV - rabat;
                    
                }
            }
        }
        #endregion

        #region PriceWithPDV
        private decimal _PriceWithPDV;

        public decimal PriceWithPDV
        {
            get { return _PriceWithPDV; }
            set
            {
                if (_PriceWithPDV != value)
                {
                    _PriceWithPDV = value;
                    NotifyPropertyChanged("PriceWithPDV");
                }
            }
        }
        #endregion
        private decimal basePrice;
        private decimal rabat;

        #region PriceWithoutPDV
        private decimal _PriceWithoutPDV;

        public decimal PriceWithoutPDV
        {
            get { return _PriceWithoutPDV; }
            set
            {
                if (_PriceWithoutPDV != value)
                {
                    _PriceWithoutPDV = value;
                    NotifyPropertyChanged("PriceWithoutPDV");
                    CalculateFieldsAfterPrice();
                }
            }
        }

        private void CalculateFieldsAfterPrice()
        {
            var calculatedBase = PriceWithoutPDV * Quantity;
            basePrice = calculatedBase;
            PriceWithPDV = basePrice + (basePrice * PDVPercent / 100);
            PDV = PriceWithPDV - basePrice;
            rabat = PriceWithPDV * Discount / 100;
            Amount = PriceWithPDV - rabat;
        }
        #endregion


        #region Discount
        private decimal _Discount;

        public decimal Discount
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

        #region PDVPercent
        private decimal _PDVPercent;

        public decimal PDVPercent
        {
            get { return _PDVPercent; }
            set
            {
                if (_PDVPercent != value)
                {
                    _PDVPercent = value;
                    NotifyPropertyChanged("PDVPercent");

                    CalculateFieldsAfterPrice();
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

        #region Amount
        private decimal _Amount;

        public decimal Amount
        {
            get { return _Amount; }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }
        #endregion

        #region ItemStatus
        private int _ItemStatus;

        public int ItemStatus
        {
            get { return _ItemStatus; }
            set
            {
                if (_ItemStatus != value)
                {
                    _ItemStatus = value;
                    NotifyPropertyChanged("ItemStatus");
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
    }
}
