using GlobalValidations;
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
        private string _CompanyCode;

        public string CompanyCode
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
        [requiredField_Validator]
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

        [Required(ErrorMessage = "Obavezno polje: Adresa")]
        [requiredField_Validator]
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

        #region IdentificationNumber
        private string _IdentificationNumber;

        [Required(ErrorMessage = "Obavezno polje: Matični broj")]
        [requiredField_Validator]
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

        [Required(ErrorMessage = "Obavezno polje: Poreski broj (PIB)")]
        [requiredField_Validator]
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

        [Required(ErrorMessage = "Obavezno polje: PDV broj")]
        [requiredField_Validator]
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


        #region Search_Code
        private string _Search_Code;

        public string Search_Code
        {
            get { return _Search_Code; }
            set
            {
                if (_Search_Code != value)
                {
                    _Search_Code = value;
                    NotifyPropertyChanged("Search_Code");
                }
            }
        }
        #endregion

        #region Search_Name
        private string _Search_Name;

        public string Search_Name
        {
            get { return _Search_Name; }
            set
            {
                if (_Search_Name != value)
                {
                    _Search_Name = value;
                    NotifyPropertyChanged("Search_Name");
                }
            }
        }
        #endregion

        #region Search_Pib
        private string _Search_Pib;

        public string Search_Pib
        {
            get { return _Search_Pib; }
            set
            {
                if (_Search_Pib != value)
                {
                    _Search_Pib = value;
                    NotifyPropertyChanged("Search_Pib");
                }
            }
        }
        #endregion

        #region Search_City
        private string _Search_City;

        public string Search_City
        {
            get { return _Search_City; }
            set
            {
                if (_Search_City != value)
                {
                    _Search_City = value;
                    NotifyPropertyChanged("Search_City");
                }
            }
        }
        #endregion

    }
}
