using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.Locations
{
    public class CityViewModel : BaseEntityViewModel, INotifyPropertyChanged
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

        #region ZipCode
        private string _ZipCode = null;

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

        #region Country
        private CountryViewModel _Country;

        public CountryViewModel Country
        {
            get { return _Country; }
            set
            {
                if (_Country != value)
                {
                    _Country = value;
                    NotifyPropertyChanged("Country");
                }
            }
        }
        #endregion

        #region Region
        private RegionViewModel _Region;

        public RegionViewModel Region
        {
            get { return _Region; }
            set
            {
                if (_Region != value)
                {
                    _Region = value;
                    NotifyPropertyChanged("Region");
                }
            }
        }
        #endregion

        #region Municipality
        private MunicipalityViewModel _Municipality;

        public MunicipalityViewModel Municipality
        {
            get { return _Municipality; }
            set
            {
                if (_Municipality != value)
                {
                    _Municipality = value;
                    NotifyPropertyChanged("Municipality");
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

        #region Search_ZipCode
        private string _Search_ZipCode;

        public string Search_ZipCode
        {
            get { return _Search_ZipCode; }
            set
            {
                if (_Search_ZipCode != value)
                {
                    _Search_ZipCode = value;
                    NotifyPropertyChanged("Search_ZipCode");
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

        #region Search_Municipality
        private string _Search_Municipality;

        public string Search_Municipality
        {
            get { return _Search_Municipality; }
            set
            {
                if (_Search_Municipality != value)
                {
                    _Search_Municipality = value;
                    NotifyPropertyChanged("Search_Municipality");
                }
            }
        }
        #endregion

        #region Search_Region
        private string _Search_Region;

        public string Search_Region
        {
            get { return _Search_Region; }
            set
            {
                if (_Search_Region != value)
                {
                    _Search_Region = value;
                    NotifyPropertyChanged("Search_Region");
                }
            }
        }
        #endregion
    }
}
