using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.ComponentModel;

namespace ServiceInterfaces.ViewModels.Base
{
    public class BaseEntityViewModel : INotifyPropertyChanged
    {
        #region Id
        private int _Id;

        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }
        #endregion

        #region Identifier
        private Guid _Identifier;

        public Guid Identifier
        {
            get { return _Identifier; }
            set
            {
                if (_Identifier != value)
                {
                    _Identifier = value;
                    NotifyPropertyChanged("Identifier");
                }
            }
        }
        #endregion


        #region CreatedBy
        private UserViewModel _CreatedBy;

        public UserViewModel CreatedBy
        {
            get { return _CreatedBy; }
            set
            {
                if (_CreatedBy != value)
                {
                    _CreatedBy = value;
                    NotifyPropertyChanged("CreatedBy");
                }
            }
        }
        #endregion

        #region Company
        private CompanyViewModel _Company;

        public CompanyViewModel Company
        {
            get { return _Company; }
            set
            {
                if (_Company != value)
                {
                    _Company = value;
                    NotifyPropertyChanged("Company");
                }
            }
        }
        #endregion

        #region UpdatedAt
        private DateTime? _UpdatedAt;

        public DateTime? UpdatedAt
        {
            get { return _UpdatedAt; }
            set
            {
                if (_UpdatedAt != value)
                {
                    _UpdatedAt = value;
                    NotifyPropertyChanged("UpdatedAt");
                }
            }
        }
        #endregion

        #region CreatedAt
        private DateTime? _CreatedAt;

        public DateTime? CreatedAt
        {
            get { return _CreatedAt; }
            set
            {
                if (_CreatedAt != value)
                {
                    _CreatedAt = value;
                    NotifyPropertyChanged("CreatedAt");
                }
            }
        }
        #endregion


        #region IsActive
        private bool _IsActive = false;

        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                if (_IsActive != value)
                {
                    _IsActive = value;
                    NotifyPropertyChanged("IsActive");
                }
            }
        }
        #endregion


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        public void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
