using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.Phonebooks
{
    public class PhonebookPhoneViewModel : BaseEntityViewModel, INotifyPropertyChanged
    {
        #region Phonebook
        private PhonebookViewModel _Phonebook;

        public PhonebookViewModel Phonebook
        {
            get { return _Phonebook; }
            set
            {
                if (_Phonebook != value)
                {
                    _Phonebook = value;
                    NotifyPropertyChanged("Phonebook");
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

        #region SurName
        private string _SurName;

        public string SurName
        {
            get { return _SurName; }
            set
            {
                if (_SurName != value)
                {
                    _SurName = value;
                    NotifyPropertyChanged("SurName");
                }
            }
        }
        #endregion

        #region PhoneNumber
        private string _PhoneNumber;

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set
            {
                if (_PhoneNumber != value)
                {
                    _PhoneNumber = value;
                    NotifyPropertyChanged("PhoneNumber");
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
