﻿using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.Sectors
{
    public class AgencyViewModel : BaseEntityViewModel, INotifyPropertyChanged
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

        #region InternalCode
        private string _InternalCode;

        public string InternalCode
        {
            get { return _InternalCode; }
            set
            {
                if (_InternalCode != value)
                {
                    _InternalCode = value;
                    NotifyPropertyChanged("InternalCode");
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

        #region Sector
        private SectorViewModel _Sector;

        public SectorViewModel Sector
        {
            get { return _Sector; }
            set
            {
                if (_Sector != value)
                {
                    _Sector = value;
                    NotifyPropertyChanged("Sector");
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

        #region Search_Code
        private string _Search_Code;

        public string Search_Code
        {
            get { return _Search_Code; }
            set
            {
                if (_Search_Code != value)
                {
                    _Search_Code = value;
                    NotifyPropertyChanged("Search_Code");
                }
            }
        }
        #endregion

        #region Search_InternalCode
        private string _Search_InternalCode;

        public string Search_InternalCode
        {
            get { return _Search_InternalCode; }
            set
            {
                if (_Search_InternalCode != value)
                {
                    _Search_InternalCode = value;
                    NotifyPropertyChanged("Search_InternalCode");
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
