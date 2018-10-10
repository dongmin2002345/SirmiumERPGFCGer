using ServiceInterfaces.ViewModels.Base;
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

        #region PDV
        private string _PDV;

        public string PDV
        {
            get { return _PDV; }
            set
            {
                if (_PDV != value)
                {
                    _PDV = value;
                    NotifyPropertyChanged("PDV");
                }
            }
        }
        #endregion

        #region IndustryCode
        private string _IndustryCode;

        public string IndustryCode
        {
            get { return _IndustryCode; }
            set
            {
                if (_IndustryCode != value)
                {
                    _IndustryCode = value;
                    NotifyPropertyChanged("IndustryCode");
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


        #region Rebate
        private decimal _Rebate;

        public decimal Rebate
        {
            get { return _Rebate; }
            set
            {
                if (_Rebate != value)
                {
                    _Rebate = value;
                    NotifyPropertyChanged("Rebate");
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

        #region OrganizationUnits
        private ObservableCollection<BusinessPartnerOrganizationUnitViewModel> _OrganizationUnits;

        public ObservableCollection<BusinessPartnerOrganizationUnitViewModel> OrganizationUnits
        {
            get { return _OrganizationUnits; }
            set
            {
                if (_OrganizationUnits != value)
                {
                    _OrganizationUnits = value;
                    NotifyPropertyChanged("OrganizationUnits");
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

    }
}
