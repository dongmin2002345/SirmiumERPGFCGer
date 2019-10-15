using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.Identity
{
    public class UserViewModel : BaseEntityViewModel
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

        #region Username
        private string _Username;

        [Required(ErrorMessage = "Obavezno polje: Korisničko ime")]
        public string Username
        {
            get { return _Username; }
            set
            {
                if (_Username != value)
                {
                    _Username = value;
                    NotifyPropertyChanged("Username");
                }
            }
        }
        #endregion

        #region FirstName
        private string _FirstName;

        [Required(ErrorMessage = "Obavezno polje: Ime")]
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    NotifyPropertyChanged("FirstName");
                }
            }
        }

        public UserViewModel ConvertToUserViewModelLite()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region LastName
        private string _LastName;

        [Required(ErrorMessage = "Obavezno polje: Prezime")]
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    NotifyPropertyChanged("LastName");
                }
            }
        }
        #endregion

        #region FullName
        private string _FullName;

        public string FullName
        {
            get { return _FullName; }
            set
            {
                if (_FullName != value)
                {
                    _FullName = value;
                    NotifyPropertyChanged("FullName");
                }
            }
        }
        #endregion

        #region Password
        private string _Password;

        //[Required(ErrorMessage = "Obavezno polje: Lozinka")]
        //[requiredField_Validator]
        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    NotifyPropertyChanged("Password");
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

        #region Companies
        private List<CompanyViewModel> _Companies;

        public List<CompanyViewModel> Companies
        {
            get { return _Companies; }
            set
            {
                if (_Companies != value)
                {
                    _Companies = value;
                    NotifyPropertyChanged("Companies");
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

        #region CompanyUsers
        private List<CompanyUserViewModel> _CompanyUsers;

        public List<CompanyUserViewModel> CompanyUsers
        {
            get { return _CompanyUsers; }
            set
            {
                if (_CompanyUsers != value)
                {
                    _CompanyUsers = value;
                    NotifyPropertyChanged("CompanyUsers");
                }
            }
        }
        #endregion

        #region PasswordHash
        private string _PasswordHash;

        public string PasswordHash
        {
            get { return _PasswordHash; }
            set
            {
                if (_PasswordHash != value)
                {
                    _PasswordHash = value;
                    NotifyPropertyChanged("PasswordHash");
                }
            }
        }
        #endregion


        #region Search_UserName
        private string _Search_UserName;

        public string Search_UserName
        {
            get { return _Search_UserName; }
            set
            {
                if (_Search_UserName != value)
                {
                    _Search_UserName = value;
                    NotifyPropertyChanged("Search_UserName");
                }
            }
        }
        #endregion

        #region Search_Email
        private string _Search_Email;

        public string Search_Email
        {
            get { return _Search_Email; }
            set
            {
                if (_Search_Email != value)
                {
                    _Search_Email = value;
                    NotifyPropertyChanged("Search_Email");
                }
            }
        }
        #endregion
    }
}
