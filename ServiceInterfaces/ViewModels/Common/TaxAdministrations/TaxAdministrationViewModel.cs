using ServiceInterfaces.ViewModels.Banks;
using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.TaxAdministrations
{
    public class TaxAdministrationViewModel : BaseEntityViewModel, INotifyPropertyChanged
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

        #region SecondCode
        private string _SecondCode;

        public string SecondCode
        {
            get { return _SecondCode; }
            set
            {
                if (_SecondCode != value)
                {
                    _SecondCode = value;
                    NotifyPropertyChanged("SecondCode");
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

        #region Bank1
        private BankViewModel _Bank1;

        public BankViewModel Bank1
        {
            get { return _Bank1; }
            set
            {
                if (_Bank1 != value)
                {
                    _Bank1 = value;
                    NotifyPropertyChanged("Bank1");
                }
            }
        }
        #endregion

        #region Bank2
        private BankViewModel _Bank2;

        public BankViewModel Bank2
        {
            get { return _Bank2; }
            set
            {
                if (_Bank2 != value)
                {
                    _Bank2 = value;
                    NotifyPropertyChanged("Bank2");
                }
            }
        }
        #endregion

        #region Address1
        private string _Address1;

        public string Address1
        {
            get { return _Address1; }
            set
            {
                if (_Address1 != value)
                {
                    _Address1 = value;
                    NotifyPropertyChanged("Address1");
                }
            }
        }
        #endregion

        #region Address2
        private string _Address2;

        public string Address2
        {
            get { return _Address2; }
            set
            {
                if (_Address2 != value)
                {
                    _Address2 = value;
                    NotifyPropertyChanged("Address2");
                }
            }
        }
        #endregion

        #region Address3
        private string _Address3;

        public string Address3
        {
            get { return _Address3; }
            set
            {
                if (_Address3 != value)
                {
                    _Address3 = value;
                    NotifyPropertyChanged("Address3");
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

        #region IBAN1
        private int _IBAN1;

        public int IBAN1
        {
            get { return _IBAN1; }
            set
            {
                if (_IBAN1 != value)
                {
                    _IBAN1 = value;
                    NotifyPropertyChanged("IBAN1");
                }
            }
        }
        #endregion

        #region SWIFT
        private int _SWIFT;

        public int SWIFT
        {
            get { return _SWIFT; }
            set
            {
                if (_SWIFT != value)
                {
                    _SWIFT = value;
                    NotifyPropertyChanged("SWIFT");
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

        #region SearchBy_Name
        private string _SearchBy_Name;

        public string SearchBy_Name
        {
            get { return _SearchBy_Name; }
            set
            {
                if (_SearchBy_Name != value)
                {
                    _SearchBy_Name = value;
                    NotifyPropertyChanged("SearchBy_Name");
                }
            }
        }
        #endregion

        #region SearchBy_City
        private string _SearchBy_City;

        public string SearchBy_City
        {
            get { return _SearchBy_City; }
            set
            {
                if (_SearchBy_City != value)
                {
                    _SearchBy_City = value;
                    NotifyPropertyChanged("SearchBy_City");
                }
            }
        }
        #endregion


        #region SearchBy_Bank1
        private string _SearchBy_Bank1;

        public string SearchBy_Bank1
        {
            get { return _SearchBy_Bank1; }
            set
            {
                if (_SearchBy_Bank1 != value)
                {
                    _SearchBy_Bank1 = value;
                    NotifyPropertyChanged("SearchBy_Bank1");
                }
            }
        }
        #endregion

        //#region SearchBy_Bank2
        //private string _SearchBy_Bank2;

        //public string SearchBy_Bank2
        //{
        //    get { return _SearchBy_Bank2; }
        //    set
        //    {
        //        if (_SearchBy_Bank2 != value)
        //        {
        //            _SearchBy_Bank2 = value;
        //            NotifyPropertyChanged("SearchBy_Bank2");
        //        }
        //    }
        //}
        //#endregion

        #region SearchBy_IBAN1
        private string _SearchBy_IBAN1;

        public string SearchBy_IBAN1
        {
            get { return _SearchBy_IBAN1; }
            set
            {
                if (_SearchBy_IBAN1 != value)
                {
                    _SearchBy_IBAN1 = value;
                    NotifyPropertyChanged("SearchBy_IBAN1");
                }
            }
        }
        #endregion

        #region SearchBy_SWIFT
        private string _SearchBy_SWIFT;

        public string SearchBy_SWIFT
        {
            get { return _SearchBy_SWIFT; }
            set
            {
                if (_SearchBy_SWIFT != value)
                {
                    _SearchBy_SWIFT = value;
                    NotifyPropertyChanged("SearchBy_SWIFT");
                }
            }
        }
        #endregion


        #endregion

    }
}
