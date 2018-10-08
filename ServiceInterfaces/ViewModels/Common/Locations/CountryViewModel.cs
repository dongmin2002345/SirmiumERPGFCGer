using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.Locations
{
    public class CountryViewModel : BaseEntityViewModel, INotifyPropertyChanged
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

        #region AlfaCode
        private string _AlfaCode;

        public string AlfaCode
        {
            get { return _AlfaCode; }
            set
            {
                if (_AlfaCode != value)
                {
                    _AlfaCode = value;
                    NotifyPropertyChanged("AlfaCode");
                }
            }
        }
        #endregion

        #region NumericCode
        private string _NumericCode;

        public string NumericCode
        {
            get { return _NumericCode; }
            set
            {
                if (_NumericCode != value)
                {
                    _NumericCode = value;
                    NotifyPropertyChanged("NumericCode");
                }
            }
        }
        #endregion

        #region Mark
        private string _Mark;

        public string Mark
        {
            get { return _Mark; }
            set
            {
                if (_Mark != value)
                {
                    _Mark = value;
                    NotifyPropertyChanged("Mark");
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


        #region Search_AlfaCode
        private string _Search_AlfaCode;

        public string Search_AlfaCode
        {
            get { return _Search_AlfaCode; }
            set
            {
                if (_Search_AlfaCode != value)
                {
                    _Search_AlfaCode = value;
                    NotifyPropertyChanged("Search_AlfaCode");
                }
            }
        }
        #endregion

        #region Search_NumericCode
        private string _Search_NumericCode;

        public string Search_NumericCode
        {
            get { return _Search_NumericCode; }
            set
            {
                if (_Search_NumericCode != value)
                {
                    _Search_NumericCode = value;
                    NotifyPropertyChanged("Search_NumericCode");
                }
            }
        }
        #endregion

        #region Search_Mark
        private string _Search_Mark;

        public string Search_Mark
        {
            get { return _Search_Mark; }
            set
            {
                if (_Search_Mark != value)
                {
                    _Search_Mark = value;
                    NotifyPropertyChanged("Search_Mark");
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
