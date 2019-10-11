using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.BusinessPartners
{
    public class BusinessPartnerPhoneViewModel : BaseEntityViewModel
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

        #region Mobile
        private string _Mobile;

        public string Mobile
        {
            get { return _Mobile; }
            set
            {
                if (_Mobile != value)
                {
                    _Mobile = value;
                    NotifyPropertyChanged("Mobile");
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

        #region ContactPersonFirstName
        private string _ContactPersonFirstName;

        public string ContactPersonFirstName
        {
            get { return _ContactPersonFirstName; }
            set
            {
                if (_ContactPersonFirstName != value)
                {
                    _ContactPersonFirstName = value;
                    NotifyPropertyChanged("ContactPersonFirstName");
                }
            }
        }
        #endregion

        #region ContactPersonLastName
        private string _ContactPersonLastName;

        public string ContactPersonLastName
        {
            get { return _ContactPersonLastName; }
            set
            {
                if (_ContactPersonLastName != value)
                {
                    _ContactPersonLastName = value;
                    NotifyPropertyChanged("ContactPersonLastName");
                }
            }
        }
        #endregion


        #region Birthday
        private DateTime? _Birthday;

        public DateTime? Birthday
        {
            get { return _Birthday; }
            set
            {
                if (_Birthday != value)
                {
                    _Birthday = value;
                    NotifyPropertyChanged("Birthday");
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