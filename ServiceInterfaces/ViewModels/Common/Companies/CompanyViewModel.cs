using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.Companies
{
    public class CompanyViewModel : BaseEntityViewModel
    {

        #region CompanyCode
        private int _CompanyCode;

        [Required(ErrorMessage = "Obavezno polje: Sifra")]
        public int CompanyCode
        {
            get { return _CompanyCode; }
            set
            {
                if (_CompanyCode != value)
                {
                    _CompanyCode = value;
                    NotifyPropertyChanged("CompanyCode");
                }
            }
        }
        #endregion

        #region CompanyName
        private string _CompanyName;

        [Required(ErrorMessage = "Obavezno polje: Naziv")]
        public string CompanyName
        {
            get { return _CompanyName; }
            set
            {
                if (_CompanyName != value)
                {
                    _CompanyName = value;
                    NotifyPropertyChanged("CompanyName");
                }
            }
        }
        #endregion

        #region ZipCode
        private string _ZipCode;

        public string ZipCode
        {
            get { return _ZipCode; }
            set
            {
                if (_ZipCode != value)
                {
                    _ZipCode = value;
                    NotifyPropertyChanged("ZipCode");
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

        #region BankAccountNo
        private string _BankAccountNo;

        public string BankAccountNo
        {
            get { return _BankAccountNo; }
            set
            {
                if (_BankAccountNo != value)
                {
                    _BankAccountNo = value;
                    NotifyPropertyChanged("BankAccountNo");
                }
            }
        }
        #endregion

        #region BankAccountName
        private string _BankAccountName;

        public string BankAccountName
        {
            get { return _BankAccountName; }
            set
            {
                if (_BankAccountName != value)
                {
                    _BankAccountName = value;
                    NotifyPropertyChanged("BankAccountName");
                }
            }
        }
        #endregion

        #region IdentificationNumber
        private string _IdentificationNumber;

        public string IdentificationNumber
        {
            get { return _IdentificationNumber; }
            set
            {
                if (_IdentificationNumber != value)
                {
                    _IdentificationNumber = value;
                    NotifyPropertyChanged("IdentificationNumber");
                }
            }
        }
        #endregion

        #region PIBNumber
        private string _PIBNumber;

        [Required(ErrorMessage = "Obavezno polje: PIB")]
        public string PIBNumber
        {
            get { return _PIBNumber; }
            set
            {
                if (_PIBNumber != value)
                {
                    _PIBNumber = value;
                    NotifyPropertyChanged("PIBNumber");
                }
            }
        }
        #endregion

        #region PIONumber
        private string _PIONumber;

        public string PIONumber
        {
            get { return _PIONumber; }
            set
            {
                if (_PIONumber != value)
                {
                    _PIONumber = value;
                    NotifyPropertyChanged("PIONumber");
                }
            }
        }
        #endregion
        #region PDVNumber
        private string _PDVNumber;

        [Required(ErrorMessage = "Obavezno polje: PDV")]
        public string PDVNumber
        {
            get { return _PDVNumber; }
            set
            {
                if (_PDVNumber != value)
                {
                    _PDVNumber = value;
                    NotifyPropertyChanged("PDVNumber");
                }
            }
        }
        #endregion

        #region IndustryCode
        private string _IndustryCode;

        public string IndustryCode
        {
            get { return _IndustryCode; }
            set
            {
                if (_IndustryCode != value)
                {
                    _IndustryCode = value;
                    NotifyPropertyChanged("IndustryCode");
                }
            }
        }
        #endregion

        #region IndustryName
        private string _IndustryName;

        public string IndustryName
        {
            get { return _IndustryName; }
            set
            {
                if (_IndustryName != value)
                {
                    _IndustryName = value;
                    NotifyPropertyChanged("IndustryName");
                }
            }
        }
        #endregion

        #region Email
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }
        #endregion

        #region WebSite
        private string _WebSite;

        public string WebSite
        {
            get { return _WebSite; }
            set
            {
                if (_WebSite != value)
                {
                    _WebSite = value;
                    NotifyPropertyChanged("WebSite");
                }
            }
        }
        #endregion

        #region IsSelected
        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    NotifyPropertyChanged("IsSelected");
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
