using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Companies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.Identity
{
    public class UserViewModel : BaseEntityViewModel
    {
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

        #region Roles
        private List<string> _Roles;

        public List<string> Roles
        {
            get { return _Roles; }
            set
            {
                if (_Roles != value)
                {
                    _Roles = value;
                    NotifyPropertyChanged("Roles");
                }
            }
        }
        #endregion

        #region RolesCSV
        private string _RolesCSV;

        public string RolesCSV
        {
            get { return _RolesCSV; }
            set
            {
                if (_RolesCSV != value)
                {
                    _RolesCSV = value;
                    NotifyPropertyChanged("RolesCSV");
                }
            }
        }
        #endregion

    }
}
