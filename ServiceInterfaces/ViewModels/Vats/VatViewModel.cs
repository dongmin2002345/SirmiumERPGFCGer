using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.ViewModels.Vats
{
    public class VatViewModel : BaseEntityViewModel
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

        #region Search_Description
        private string _Search_Description;

        public string Search_Description
        {
            get { return _Search_Description; }
            set
            {
                if (_Search_Description != value)
                {
                    _Search_Description = value;
                    NotifyPropertyChanged("Search_Description");
                }
            }
        }
        #endregion
    }
}
