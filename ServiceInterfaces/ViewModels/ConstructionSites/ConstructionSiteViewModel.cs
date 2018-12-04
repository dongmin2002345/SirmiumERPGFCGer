using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Locations;
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

    }
}


