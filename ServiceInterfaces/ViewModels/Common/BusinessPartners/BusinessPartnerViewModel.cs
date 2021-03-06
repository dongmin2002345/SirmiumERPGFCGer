﻿using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Vats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.BusinessPartners
{
    public class BusinessPartnerViewModel : BaseEntityViewModel, INotifyPropertyChanged
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


        #region PIB
        private string _PIB;

        public string PIB
        {
            get { return _PIB; }
            set
            {
                if (_PIB != value)
                {
                    _PIB = value;
                    NotifyPropertyChanged("PIB");
                }
            }
        }
        #endregion

        #region PIO
        private string _PIO;

        public string PIO
        {
            get { return _PIO; }
            set
            {
                if (_PIO != value)
                {
                    _PIO = value;
                    NotifyPropertyChanged("PIO");
                }
            }
        }
        #endregion


        #region IdentificationNumber
        private string _IdentificationNumber;

        public string IdentificationNumber
        {
            get { return _IdentificationNumber; }
            set
            {
                if (_IdentificationNumber != value)
                {
                    _IdentificationNumber = value;
                    NotifyPropertyChanged("IdentificationNumber");
                }
            }
        }
        #endregion


        #region Customer
        private string _Customer;

        public string Customer
        {
            get { return _Customer; }
            set
            {
                if (_Customer != value)
                {
                    _Customer = value;
                    NotifyPropertyChanged("Customer");
                }
            }
        }
        #endregion

        #region DueDate
        private int _DueDate;

        public int DueDate
        {
            get { return _DueDate; }
            set
            {
                if (_DueDate != value)
                {
                    _DueDate = value;
                    NotifyPropertyChanged("DueDate");
                }
            }
        }
        #endregion


        #region WebSite
        private string _WebSite;

        public string WebSite
        {
            get { return _WebSite; }
            set
            {
                if (_WebSite != value)
                {
                    _WebSite = value;
                    NotifyPropertyChanged("WebSite");
                }
            }
        }
        #endregion

        #region ContactPerson
        private string _ContactPerson;

        public string ContactPerson
        {
            get { return _ContactPerson; }
            set
            {
                if (_ContactPerson != value)
                {
                    _ContactPerson = value;
                    NotifyPropertyChanged("ContactPerson");
                }
            }
        }
        #endregion


        #region IsInPDV
        private bool _IsInPDV;

        public bool IsInPDV
        {
            get { return _IsInPDV; }
            set
            {
                if (_IsInPDV != value)
                {
                    _IsInPDV = value;
                    NotifyPropertyChanged("IsInPDV");
                }
            }
        }
        #endregion


        #region JBKJS
        private string _JBKJS;

        public string JBKJS
        {
            get { return _JBKJS; }
            set
            {
                if (_JBKJS != value)
                {
                    _JBKJS = value;
                    NotifyPropertyChanged("JBKJS");
                }
            }
        }
        #endregion

        #region CountrySrb
        private CountryViewModel _CountrySrb;

        public CountryViewModel CountrySrb
        {
            get { return _CountrySrb; }
            set
            {
                if (_CountrySrb != value)
                {
                    _CountrySrb = value;
                    NotifyPropertyChanged("CountrySrb");
                }
            }
        }
        #endregion

        #region CitySrb
        private CityViewModel _CitySrb;

        public CityViewModel CitySrb
        {
            get { return _CitySrb; }
            set
            {
                if (_CitySrb != value)
                {
                    _CitySrb = value;
                    NotifyPropertyChanged("CitySrb");
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



        #region GER 

        #region NameGer
        private string _NameGer;

        public string NameGer
        {
            get { return _NameGer; }
            set
            {
                if (_NameGer != value)
                {
                    _NameGer = value;
                    NotifyPropertyChanged("NameGer");
                }
            }
        }
        #endregion

        #region IsInPDVGer
        private bool _IsInPDVGer;

        public bool IsInPDVGer
        {
            get { return _IsInPDVGer; }
            set
            {
                if (_IsInPDVGer != value)
                {
                    _IsInPDVGer = value;
                    NotifyPropertyChanged("IsInPDVGer");
                }
            }
        }
        #endregion

        #region TaxAdministration
        private TaxAdministrationViewModel _TaxAdministration;

        public TaxAdministrationViewModel TaxAdministration
        {
            get { return _TaxAdministration; }
            set
            {
                if (_TaxAdministration != value)
                {
                    _TaxAdministration = value;
                    NotifyPropertyChanged("TaxAdministration");
                }
            }
        }
        #endregion

        #region IBAN
        private string _IBAN;

        public string IBAN
        {
            get { return _IBAN; }
            set
            {
                if (_IBAN != value)
                {
                    _IBAN = value;
                    NotifyPropertyChanged("IBAN");
                }
            }
        }
        #endregion

        #region BetriebsNumber
        private string _BetriebsNumber;

        public string BetriebsNumber
        {
            get { return _BetriebsNumber; }
            set
            {
                if (_BetriebsNumber != value)
                {
                    _BetriebsNumber = value;
                    NotifyPropertyChanged("BetriebsNumber");
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

        #region AddressGer
        private string _AddressGer;

        public string AddressGer
        {
            get { return _AddressGer; }
            set
            {
                if (_AddressGer != value)
                {
                    _AddressGer = value;
                    NotifyPropertyChanged("AddressGer");
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

        #region Agency
        private AgencyViewModel _Agency;

        public AgencyViewModel Agency
        {
            get { return _Agency; }
            set
            {
                if (_Agency != value)
                {
                    _Agency = value;
                    NotifyPropertyChanged("Agency");
                }
            }
        }
        #endregion

        #region TaxNr
        private string _TaxNr;

        public string TaxNr
        {
            get { return _TaxNr; }
            set
            {
                if (_TaxNr != value)
                {
                    _TaxNr = value;
                    NotifyPropertyChanged("TaxNr");
                }
            }
        }
        #endregion

        #region CommercialNr
        private string _CommercialNr;

        public string CommercialNr
        {
            get { return _CommercialNr; }
            set
            {
                if (_CommercialNr != value)
                {
                    _CommercialNr = value;
                    NotifyPropertyChanged("CommercialNr");
                }
            }
        }
        #endregion

        #region ContactPersonGer
        private string _ContactPersonGer;

        public string ContactPersonGer
        {
            get { return _ContactPersonGer; }
            set
            {
                if (_ContactPersonGer != value)
                {
                    _ContactPersonGer = value;
                    NotifyPropertyChanged("ContactPersonGer");
                }
            }
        }
        #endregion


        #region VatDeductionFrom
        private DateTime? _VatDeductionFrom;

        public DateTime? VatDeductionFrom
        {
            get { return _VatDeductionFrom; }
            set
            {
                if (_VatDeductionFrom != value)
                {
                    _VatDeductionFrom = value;
                    NotifyPropertyChanged("VatDeductionFrom");
                }
            }
        }
        #endregion

        #region VatDeductionTo
        private DateTime? _VatDeductionTo;

        public DateTime? VatDeductionTo
        {
            get { return _VatDeductionTo; }
            set
            {
                if (_VatDeductionTo != value)
                {
                    _VatDeductionTo = value;
                    NotifyPropertyChanged("VatDeductionTo");
                }
            }
        }
        #endregion


        #endregion

        #region Vat
        private VatViewModel _Vat;

        public VatViewModel Vat
        {
            get { return _Vat; }
            set
            {
                if (_Vat != value)
                {
                    _Vat = value;
                    NotifyPropertyChanged("Vat");
                }
            }
        }
        #endregion

        #region Discount
        private DiscountViewModel _Discount;

        public DiscountViewModel Discount
        {
            get { return _Discount; }
            set
            {
                if (_Discount != value)
                {
                    _Discount = value;
                    NotifyPropertyChanged("Discount");
                }
            }
        }
        #endregion

        #region Locations
        private ObservableCollection<BusinessPartnerLocationViewModel> _Locations;

        public ObservableCollection<BusinessPartnerLocationViewModel> Locations
        {
            get { return _Locations; }
            set
            {
                if (_Locations != value)
                {
                    _Locations = value;
                    NotifyPropertyChanged("Locations");
                }
            }
        }
        #endregion

        #region Phones
        private ObservableCollection<BusinessPartnerPhoneViewModel> _Phones;

        public ObservableCollection<BusinessPartnerPhoneViewModel> Phones
        {
            get { return _Phones; }
            set
            {
                if (_Phones != value)
                {
                    _Phones = value;
                    NotifyPropertyChanged("Phones");
                }
            }
        }
        #endregion

        #region Institutions
        private ObservableCollection<BusinessPartnerInstitutionViewModel> _Institutions;

        public ObservableCollection<BusinessPartnerInstitutionViewModel> Institutions
        {
            get { return _Institutions; }
            set
            {
                if (_Institutions != value)
                {
                    _Institutions = value;
                    NotifyPropertyChanged("Institutions");
                }
            }
        }
        #endregion

        #region Banks
        private ObservableCollection<BusinessPartnerBankViewModel> _Banks;

        public ObservableCollection<BusinessPartnerBankViewModel> Banks
        {
            get { return _Banks; }
            set
            {
                if (_Banks != value)
                {
                    _Banks = value;
                    NotifyPropertyChanged("Banks");
                }
            }
        }
        #endregion

        #region BusinessPartnerTypes
        private ObservableCollection<BusinessPartnerTypeViewModel> _BusinessPartnerTypes;

        public ObservableCollection<BusinessPartnerTypeViewModel> BusinessPartnerTypes
        {
            get { return _BusinessPartnerTypes; }
            set
            {
                if (_BusinessPartnerTypes != value)
                {
                    _BusinessPartnerTypes = value;
                    NotifyPropertyChanged("BusinessPartnerTypes");
                }
            }
        }
        #endregion

        #region BusinessPartnerDocuments
        private ObservableCollection<BusinessPartnerDocumentViewModel> _BusinessPartnerDocuments;

        public ObservableCollection<BusinessPartnerDocumentViewModel> BusinessPartnerDocuments
        {
            get { return _BusinessPartnerDocuments; }
            set
            {
                if (_BusinessPartnerDocuments != value)
                {
                    _BusinessPartnerDocuments = value;
                    NotifyPropertyChanged("BusinessPartnerDocuments");
                }
            }
        }
        #endregion

        #region BusinessPartnerNotes
        private ObservableCollection<BusinessPartnerNoteViewModel> _BusinessPartnerNotes;

        public ObservableCollection<BusinessPartnerNoteViewModel> BusinessPartnerNotes
        {
            get { return _BusinessPartnerNotes; }
            set
            {
                if (_BusinessPartnerNotes != value)
                {
                    _BusinessPartnerNotes = value;
                    NotifyPropertyChanged("BusinessPartnerNotes");
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

        #region Search_PIB
        private string _Search_PIB;

        public string Search_PIB
        {
            get { return _Search_PIB; }
            set
            {
                if (_Search_PIB != value)
                {
                    _Search_PIB = value;
                    NotifyPropertyChanged("Search_PIB");
                }
            }
        }
        #endregion

        #region Search_TaxNr
        private string _Search_TaxNr;

        public string Search_TaxNr
        {
            get { return _Search_TaxNr; }
            set
            {
                if (_Search_TaxNr != value)
                {
                    _Search_TaxNr = value;
                    NotifyPropertyChanged("Search_TaxNr");
                }
            }
        }
        #endregion


        #region Search_Agency
        private string _Search_Agency;

        public string Search_Agency
        {
            get { return _Search_Agency; }
            set
            {
                if (_Search_Agency != value)
                {
                    _Search_Agency = value;
                    NotifyPropertyChanged("Search_Agency");
                }
            }
        }
        #endregion


        #region PdvType
        private int? _PdvType;

        public int? PdvType
        {
            get { return _PdvType; }
            set
            {
                if (_PdvType != value)
                {
                    _PdvType = value;
                    NotifyPropertyChanged("PdvType");
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

        #region MaxNumOfEmployees
        private int _MaxNumOfEmployees;

        public int MaxNumOfEmployees
        {
            get { return _MaxNumOfEmployees; }
            set
            {
                if (_MaxNumOfEmployees != value)
                {
                    _MaxNumOfEmployees = value;
                    NotifyPropertyChanged("MaxNumOfEmployees");
                }
            }
        }
        #endregion

    }
}
