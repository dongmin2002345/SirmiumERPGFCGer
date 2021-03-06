﻿using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.ObjectModel;

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


        #region ConstructionSiteCode
        private string _ConstructionSiteCode;

        public string ConstructionSiteCode
        {
            get { return _ConstructionSiteCode; }
            set
            {
                if (_ConstructionSiteCode != value)
                {
                    _ConstructionSiteCode = value;
                    NotifyPropertyChanged("ConstructionSiteCode");
                }
            }
        }
        #endregion

        #region ConstructionSiteName
        private string _ConstructionSiteName;

        public string ConstructionSiteName
        {
            get { return _ConstructionSiteName; }
            set
            {
                if (_ConstructionSiteName != value)
                {
                    _ConstructionSiteName = value;
                    NotifyPropertyChanged("ConstructionSiteName");
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

        #region IsVisaApplied
        private bool _IsVisaApplied;

        public bool IsVisaApplied
        {
            get { return _IsVisaApplied; }
            set
            {
                if (_IsVisaApplied != value)
                {
                    _IsVisaApplied = value;
                    NotifyPropertyChanged("IsVisaApplied");
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

        #region PassportMup
        private string _PassportMup;

        public string PassportMup
        {
            get { return _PassportMup; }
            set
            {
                if (_PassportMup != value)
                {
                    _PassportMup = value;
                    NotifyPropertyChanged("PassportMup");
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


        #region ResidenceCountry
        private CountryViewModel _ResidenceCountry;

        public CountryViewModel ResidenceCountry
        {
            get { return _ResidenceCountry; }
            set
            {
                if (_ResidenceCountry != value)
                {
                    _ResidenceCountry = value;
                    NotifyPropertyChanged("ResidenceCountry");
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

        #region VisaDate
        private DateTime? _VisaDate;

        public DateTime? VisaDate
        {
            get { return _VisaDate; }
            set
            {
                if (_VisaDate != value)
                {
                    _VisaDate = value;
                    NotifyPropertyChanged("VisaDate");
                }
            }
        }
        #endregion

        #region VisaValidFrom
        private DateTime? _VisaValidFrom;

        public DateTime? VisaValidFrom
        {
            get { return _VisaValidFrom; }
            set
            {
                if (_VisaValidFrom != value)
                {
                    _VisaValidFrom = value;
                    NotifyPropertyChanged("VisaValidFrom");
                }
            }
        }
        #endregion

        #region VisaValidTo
        private DateTime? _VisaValidTo;

        public DateTime? VisaValidTo
        {
            get { return _VisaValidTo; }
            set
            {
                if (_VisaValidTo != value)
                {
                    _VisaValidTo = value;
                    NotifyPropertyChanged("VisaValidTo");
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

        #region EmployeeProfessions
        private ObservableCollection<EmployeeProfessionItemViewModel> _EmployeeProfessions;

        public ObservableCollection<EmployeeProfessionItemViewModel> EmployeeProfessions
        {
            get { return _EmployeeProfessions; }
            set
            {
                if (_EmployeeProfessions != value)
                {
                    _EmployeeProfessions = value;
                    NotifyPropertyChanged("EmployeeProfessions");
                }
            }
        }
        #endregion

        #region EmployeeLicences
        private ObservableCollection<EmployeeLicenceItemViewModel> _EmployeeLicences;

        public ObservableCollection<EmployeeLicenceItemViewModel> EmployeeLicences
        {
            get { return _EmployeeLicences; }
            set
            {
                if (_EmployeeLicences != value)
                {
                    _EmployeeLicences = value;
                    NotifyPropertyChanged("EmployeeLicences");
                }
            }
        }
        #endregion

        #region EmployeeDocuments
        private ObservableCollection<EmployeeDocumentViewModel> _EmployeeDocuments;

        public ObservableCollection<EmployeeDocumentViewModel> EmployeeDocuments
        {
            get { return _EmployeeDocuments; }
            set
            {
                if (_EmployeeDocuments != value)
                {
                    _EmployeeDocuments = value;
                    NotifyPropertyChanged("EmployeeDocuments");
                }
            }
        }
        #endregion

        #region EmployeeNotes
        private ObservableCollection<EmployeeNoteViewModel> _EmployeeNotes;

        public ObservableCollection<EmployeeNoteViewModel> EmployeeNotes
        {
            get { return _EmployeeNotes; }
            set
            {
                if (_EmployeeNotes != value)
                {
                    _EmployeeNotes = value;
                    NotifyPropertyChanged("EmployeeNotes");
                }
            }
        }
        #endregion

        #region EmployeeCards
        private ObservableCollection<EmployeeCardViewModel> _EmployeeCards;

        public ObservableCollection<EmployeeCardViewModel> EmployeeCards
        {
            get { return _EmployeeCards; }
            set
            {
                if (_EmployeeCards != value)
                {
                    _EmployeeCards = value;
                    NotifyPropertyChanged("EmployeeCards");
                }
            }
        }
        #endregion

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

        #region Search_ConstructionSite
        private string _Search_ConstructionSite;

        public string Search_ConstructionSite
        {
            get { return _Search_ConstructionSite; }
            set
            {
                if (_Search_ConstructionSite != value)
                {
                    _Search_ConstructionSite = value;
                    NotifyPropertyChanged("Search_ConstructionSite");
                }
            }
        }
        #endregion

        #region Search_EmployeeCode
        private string _Search_EmployeeCode;

        public string Search_EmployeeCode
        {
            get { return _Search_EmployeeCode; }
            set
            {
                if (_Search_EmployeeCode != value)
                {
                    _Search_EmployeeCode = value;
                    NotifyPropertyChanged("Search_EmployeeCode");
                }
            }
        }
        #endregion


        #region AddPopupOpened
        private bool _AddPopupOpened;

        public bool AddPopupOpened
        {
            get { return _AddPopupOpened; }
            set
            {
                if (_AddPopupOpened != value)
                {
                    _AddPopupOpened = value;
                    NotifyPropertyChanged("AddPopupOpened");
                }
            }
        }
        #endregion

        #region ContractStartDate
        private DateTime _ContractStartDate = DateTime.Now;

        public DateTime ContractStartDate
        {
            get { return _ContractStartDate; }
            set
            {
                if (_ContractStartDate != value)
                {
                    _ContractStartDate = value;
                    NotifyPropertyChanged("ContractStartDate");
                }
            }
        }
        #endregion

        #region ContractEndDate
        private DateTime _ContractEndDate = DateTime.Now.AddDays(1);

        public DateTime ContractEndDate
        {
            get { return _ContractEndDate; }
            set
            {
                if (_ContractEndDate != value)
                {
                    _ContractEndDate = value;
                    NotifyPropertyChanged("ContractEndDate");
                }
            }
        }
        #endregion

    }
}
