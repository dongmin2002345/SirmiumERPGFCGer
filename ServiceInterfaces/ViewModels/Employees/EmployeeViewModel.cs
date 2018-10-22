﻿using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Employees
{
    public class EmployeeViewModel : BaseEntityViewModel
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

        #region EmployeeCode
        private string _EmployeeCode;

        public string EmployeeCode
        {
            get { return _EmployeeCode; }
            set
            {
                if (_EmployeeCode != value)
                {
                    _EmployeeCode = value;
                    NotifyPropertyChanged("EmployeeCode");
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


        #region DateOfBirth
        private DateTime? _DateOfBirth = DateTime.Now;

        public DateTime? DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                if (_DateOfBirth != value)
                {
                    _DateOfBirth = value;
                    NotifyPropertyChanged("DateOfBirth");
                }
            }
        }
        #endregion

        #region Gender
        private int _Gender;

        public int Gender
        {
            get { return _Gender; }
            set
            {
                if (_Gender != value)
                {
                    _Gender = value;
                    NotifyPropertyChanged("Gender");
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

        #region City
        private CityViewModel _City;

        public CityViewModel City
        {
            get { return _City; }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    NotifyPropertyChanged("City");
                }
            }
        }
        #endregion

        #region Address
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    _Address = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }
        #endregion


        #region PassportCountry
        private CountryViewModel _PassportCountry;

        public CountryViewModel PassportCountry
        {
            get { return _PassportCountry; }
            set
            {
                if (_PassportCountry != value)
                {
                    _PassportCountry = value;
                    NotifyPropertyChanged("PassportCountry");
                }
            }
        }
        #endregion

        #region PassportCity
        private CityViewModel _PassportCity;

        public CityViewModel PassportCity
        {
            get { return _PassportCity; }
            set
            {
                if (_PassportCity != value)
                {
                    _PassportCity = value;
                    NotifyPropertyChanged("PassportCity");
                }
            }
        }
        #endregion

        #region Passport
        private string _Passport;

        public string Passport
        {
            get { return _Passport; }
            set
            {
                if (_Passport != value)
                {
                    _Passport = value;
                    NotifyPropertyChanged("Passport");
                }
            }
        }
        #endregion

        #region VisaFrom
        private DateTime? _VisaFrom = DateTime.Now;

        public DateTime? VisaFrom
        {
            get { return _VisaFrom; }
            set
            {
                if (_VisaFrom != value)
                {
                    _VisaFrom = value;
                    NotifyPropertyChanged("VisaFrom");
                }
            }
        }
        #endregion

        #region VisaTo
        private DateTime? _VisaTo = DateTime.Now;

        public DateTime? VisaTo
        {
            get { return _VisaTo; }
            set
            {
                if (_VisaTo != value)
                {
                    _VisaTo = value;
                    NotifyPropertyChanged("VisaTo");
                }
            }
        }
        #endregion

        #region ResidenceCity
        private CityViewModel _ResidenceCity;

        public CityViewModel ResidenceCity
        {
            get { return _ResidenceCity; }
            set
            {
                if (_ResidenceCity != value)
                {
                    _ResidenceCity = value;
                    NotifyPropertyChanged("ResidenceCity");
                }
            }
        }
        #endregion

        #region ResidenceAddress
        private string _ResidenceAddress;

        public string ResidenceAddress
        {
            get { return _ResidenceAddress; }
            set
            {
                if (_ResidenceAddress != value)
                {
                    _ResidenceAddress = value;
                    NotifyPropertyChanged("ResidenceAddress");
                }
            }
        }
        #endregion



        #region Interest
        private string _Interest;

        public string Interest
        {
            get { return _Interest; }
            set
            {
                if (_Interest != value)
                {
                    _Interest = value;
                    NotifyPropertyChanged("Interest");
                }
            }
        }
        #endregion

        #region License
        private string _License;

        public string License
        {
            get { return _License; }
            set
            {
                if (_License != value)
                {
                    _License = value;
                    NotifyPropertyChanged("License");
                }
            }
        }
        #endregion



        #region EmbassyDate
        private DateTime? _EmbassyDate = DateTime.Now;

        public DateTime? EmbassyDate
        {
            get { return _EmbassyDate; }
            set
            {
                if (_EmbassyDate != value)
                {
                    _EmbassyDate = value;
                    NotifyPropertyChanged("EmbassyDate");
                }
            }
        }
        #endregion

        #region WorkPermitFrom
        private DateTime? _WorkPermitFrom = DateTime.Now;

        public DateTime? WorkPermitFrom
        {
            get { return _WorkPermitFrom; }
            set
            {
                if (_WorkPermitFrom != value)
                {
                    _WorkPermitFrom = value;
                    NotifyPropertyChanged("WorkPermitFrom");
                }
            }
        }
        #endregion

        #region WorkPermitTo
        private DateTime? _WorkPermitTo = DateTime.Now;

        public DateTime? WorkPermitTo
        {
            get { return _WorkPermitTo; }
            set
            {
                if (_WorkPermitTo != value)
                {
                    _WorkPermitTo = value;
                    NotifyPropertyChanged("WorkPermitTo");
                }
            }
        }
        #endregion

        #region EmployeeItems
        private ObservableCollection<EmployeeItemViewModel>  _EmployeeItems;

        public ObservableCollection<EmployeeItemViewModel>  EmployeeItems
        {
            get { return _EmployeeItems; }
            set
            {
                if (_EmployeeItems != value)
                {
                    _EmployeeItems = value;
                    NotifyPropertyChanged("EmployeeItems");
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

        
        #region SearchBy_Name
        private string _SearchBy_Name;

        public string SearchBy_Name
        {
            get { return _SearchBy_Name; }
            set
            {
                if (_SearchBy_Name != value)
                {
                    _SearchBy_Name = value;
                    NotifyPropertyChanged("SearchBy_Name");
                }
            }
        }
        #endregion

        #region SearchBy_SurName
        private string _SearchBy_SurName;

        public string SearchBy_SurName
        {
            get { return _SearchBy_SurName; }
            set
            {
                if (_SearchBy_SurName != value)
                {
                    _SearchBy_SurName = value;
                    NotifyPropertyChanged("SearchBy_SurName");
                }
            }
        }
        #endregion

        #region SearchBy_Passport
        private string _SearchBy_Passport;

        public string SearchBy_Passport
        {
            get { return _SearchBy_Passport; }
            set
            {
                if (_SearchBy_Passport != value)
                {
                    _SearchBy_Passport = value;
                    NotifyPropertyChanged("SearchBy_Passport");
                }
            }
        }
        #endregion

        #region SearchBy_Interest
        private string _SearchBy_Interest;

        public string SearchBy_Interest
        {
            get { return _SearchBy_Interest; }
            set
            {
                if (_SearchBy_Interest != value)
                {
                    _SearchBy_Interest = value;
                    NotifyPropertyChanged("SearchBy_Interest");
                }
            }
        }
        #endregion
    }
}
