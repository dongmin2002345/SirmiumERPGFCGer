using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.BusinessPartners
{
    public class BusinessPartnerInstitutionViewModel : BaseEntityViewModel
    {
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


        #region Institution
        private string _Institution;

        public string Institution
        {
            get { return _Institution; }
            set
            {
                if (_Institution != value)
                {
                    _Institution = value;
                    NotifyPropertyChanged("Institution");
                }
            }
        }
        #endregion

        #region Username
        private string _Username;

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

        #region Password
        private string _Password;

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

        #region ContactPerson
        private string _ContactPerson;

        public string ContactPerson
        {
            get { return _ContactPerson; }
            set
            {
                if (_ContactPerson != value)
                {
                    _ContactPerson = value;
                    NotifyPropertyChanged("ContactPerson");
                }
            }
        }
        #endregion

        #region Phone
        private string _Phone;

        public string Phone
        {
            get { return _Phone; }
            set
            {
                if (_Phone != value)
                {
                    _Phone = value;
                    NotifyPropertyChanged("Phone");
                }
            }
        }
        #endregion

        #region Fax
        private string _Fax;

        public string Fax
        {
            get { return _Fax; }
            set
            {
                if (_Fax != value)
                {
                    _Fax = value;
                    NotifyPropertyChanged("Fax");
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
