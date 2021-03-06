﻿using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.Locations
{
    public class MunicipalityViewModel : BaseEntityViewModel
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

        #region MunicipalityCode
        private string _MunicipalityCode;

        public string MunicipalityCode
        {
            get { return _MunicipalityCode; }
            set
            {
                if (_MunicipalityCode != value)
                {
                    _MunicipalityCode = value;
                    NotifyPropertyChanged("MunicipalityCode");
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

        #region Search_MunicipalityCode
        private string _Search_MunicipalityCode;

        public string Search_MunicipalityCode
        {
            get { return _Search_MunicipalityCode; }
            set
            {
                if (_Search_MunicipalityCode != value)
                {
                    _Search_MunicipalityCode = value;
                    NotifyPropertyChanged("Search_MunicipalityCode");
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

        #region Search_Country
        private string _Search_Country;

        public string Search_Country
        {
            get { return _Search_Country; }
            set
            {
                if (_Search_Country != value)
                {
                    _Search_Country = value;
                    NotifyPropertyChanged("Search_Country");
                }
            }
        }
        #endregion


    }
}
