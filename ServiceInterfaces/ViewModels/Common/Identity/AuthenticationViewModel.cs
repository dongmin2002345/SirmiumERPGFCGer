using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.Identity
{
    public class AuthenticationViewModel : BaseEntityViewModel, INotifyPropertyChanged
    {
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

        #region CompanyID
        private int _CompanyID;

        public int CompanyID
        {
            get { return _CompanyID; }
            set
            {
                if (_CompanyID != value)
                {
                    _CompanyID = value;
                    NotifyPropertyChanged("CompanyID");
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
