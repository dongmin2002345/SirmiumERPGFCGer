using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Statuses;
using System;
using System.Collections.ObjectModel;

namespace ServiceInterfaces.ViewModels.ConstructionSites
{
    public class ConstructionSiteViewModel : BaseEntityViewModel
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


        #region Address
        private string _Address = null;

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

        #region Status
        private StatusViewModel _Status;

        public StatusViewModel Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }
        #endregion

        #region StatusDate
        private DateTime _StatusDate = DateTime.Now;

        public DateTime StatusDate
        {
            get { return _StatusDate; }
            set
            {
                if (_StatusDate != value)
                {
                    _StatusDate = value;
                    NotifyPropertyChanged("StatusDate");
                }
            }
        }
        #endregion

        #region MaxWorkers
        private int _MaxWorkers;

        public int MaxWorkers
        {
            get { return _MaxWorkers; }
            set
            {
                if (_MaxWorkers != value)
                {
                    _MaxWorkers = value;
                    NotifyPropertyChanged("MaxWorkers");
                }
            }
        }
        #endregion


        #region ProContractDate
        private DateTime _ProContractDate;

        public DateTime ProContractDate
        {
            get { return _ProContractDate; }
            set
            {
                if (_ProContractDate != value)
                {
                    _ProContractDate = value;
                    NotifyPropertyChanged("ProContractDate");
                }
            }
        }
        #endregion

        #region ContractStart
        private DateTime _ContractStart;

        public DateTime ContractStart
        {
            get { return _ContractStart; }
            set
            {
                if (_ContractStart != value)
                {
                    _ContractStart = value;
                    NotifyPropertyChanged("ContractStart");
                }
            }
        }
        #endregion

        #region ContractExpiration
        private DateTime _ContractExpiration = DateTime.Now;

        public DateTime ContractExpiration
        {
            get { return _ContractExpiration; }
            set
            {
                if (_ContractExpiration != value)
                {
                    _ContractExpiration = value;
                    NotifyPropertyChanged("ContractExpiration");
                }
            }
        }
        #endregion


        #region ConstructionSiteDocuments
        private ObservableCollection<ConstructionSiteDocumentViewModel> _ConstructionSiteDocuments;

        public ObservableCollection<ConstructionSiteDocumentViewModel> ConstructionSiteDocuments
        {
            get { return _ConstructionSiteDocuments; }
            set
            {
                if (_ConstructionSiteDocuments != value)
                {
                    _ConstructionSiteDocuments = value;
                    NotifyPropertyChanged("ConstructionSiteDocuments");
                }
            }
        }
        #endregion

        #region ConstructionSiteNotes
        private ObservableCollection<ConstructionSiteNoteViewModel> _ConstructionSiteNotes;

        public ObservableCollection<ConstructionSiteNoteViewModel> ConstructionSiteNotes
        {
            get { return _ConstructionSiteNotes; }
            set
            {
                if (_ConstructionSiteNotes != value)
                {
                    _ConstructionSiteNotes = value;
                    NotifyPropertyChanged("ConstructionSiteNotes");
                }
            }
        }
        #endregion

        #region ConstructionSiteCalculations
        private ObservableCollection<ConstructionSiteCalculationViewModel> _ConstructionSiteCalculations;

        public ObservableCollection<ConstructionSiteCalculationViewModel> ConstructionSiteCalculations
        {
            get { return _ConstructionSiteCalculations; }
            set
            {
                if (_ConstructionSiteCalculations != value)
                {
                    _ConstructionSiteCalculations = value;
                    NotifyPropertyChanged("ConstructionSiteCalculations");
                }
            }
        }
        #endregion

        #region PaymentDate
        private DateTime _PaymentDate = DateTime.Now;

        public DateTime PaymentDate
        {
            get { return _PaymentDate; }
            set
            {
                if (_PaymentDate != value)
                {
                    _PaymentDate = value;
                    NotifyPropertyChanged("PaymentDate");
                }
            }
        }
        #endregion

        #region Path
        private string _Path;

        public string Path
        {
            get { return _Path; }
            set
            {
                if (_Path != value)
                {
                    _Path = value;
                    NotifyPropertyChanged("Path");
                }
            }
        }
        #endregion

        #region PaymentValue
        private decimal _PaymentValue;

        public decimal PaymentValue
        {
            get { return _PaymentValue; }
            set
            {
                if (_PaymentValue != value)
                {
                    _PaymentValue = value;
                    NotifyPropertyChanged("PaymentValue");
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

        #region Search_City
        private string _Search_City;

        public string Search_City
        {
            get { return _Search_City; }
            set
            {
                if (_Search_City != value)
                {
                    _Search_City = value;
                    NotifyPropertyChanged("Search_City");
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

        #region Search_BusinessPartnerName
        private string _Search_BusinessPartnerName;

        public string Search_BusinessPartnerName
        {
            get { return _Search_BusinessPartnerName; }
            set
            {
                if (_Search_BusinessPartnerName != value)
                {
                    _Search_BusinessPartnerName = value;
                    NotifyPropertyChanged("Search_BusinessPartnerName");
                }
            }
        }
        #endregion

    }
}


