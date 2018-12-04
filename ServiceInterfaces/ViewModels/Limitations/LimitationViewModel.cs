using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Limitations
{
    public class LimitationViewModel : BaseEntityViewModel
    {
        #region ConstructionSiteLimit
        private int _ConstructionSiteLimit;

        public int ConstructionSiteLimit
        {
            get { return _ConstructionSiteLimit; }
            set
            {
                if (_ConstructionSiteLimit != value)
                {
                    _ConstructionSiteLimit = value;
                    NotifyPropertyChanged("ConstructionSiteLimit");
                }
            }
        }
        #endregion

        #region BusinessPartnerConstructionSiteLimit
        private int _BusinessPartnerConstructionSiteLimit;

        public int BusinessPartnerConstructionSiteLimit
        {
            get { return _BusinessPartnerConstructionSiteLimit; }
            set
            {
                if (_BusinessPartnerConstructionSiteLimit != value)
                {
                    _BusinessPartnerConstructionSiteLimit = value;
                    NotifyPropertyChanged("BusinessPartnerConstructionSiteLimit");
                }
            }
        }
        #endregion

        #region EmployeeConstructionSiteLimit
        private int _EmployeeConstructionSiteLimit;

        public int EmployeeConstructionSiteLimit
        {
            get { return _EmployeeConstructionSiteLimit; }
            set
            {
                if (_EmployeeConstructionSiteLimit != value)
                {
                    _EmployeeConstructionSiteLimit = value;
                    NotifyPropertyChanged("EmployeeConstructionSiteLimit");
                }
            }
        }
        #endregion

        #region EmployeeBusinessPartnerLimit
        private int _EmployeeBusinessPartnerLimit;

        public int EmployeeBusinessPartnerLimit
        {
            get { return _EmployeeBusinessPartnerLimit; }
            set
            {
                if (_EmployeeBusinessPartnerLimit != value)
                {
                    _EmployeeBusinessPartnerLimit = value;
                    NotifyPropertyChanged("EmployeeBusinessPartnerLimit");
                }
            }
        }
        #endregion

        #region EmployeeBirthdayLimit
        private int _EmployeeBirthdayLimit;

        public int EmployeeBirthdayLimit
        {
            get { return _EmployeeBirthdayLimit; }
            set
            {
                if (_EmployeeBirthdayLimit != value)
                {
                    _EmployeeBirthdayLimit = value;
                    NotifyPropertyChanged("EmployeeBirthdayLimit");
                }
            }
        }
        #endregion


        #region EmployeePassportLimit
        private int _EmployeePassportLimit;

        public int EmployeePassportLimit
        {
            get { return _EmployeePassportLimit; }
            set
            {
                if (_EmployeePassportLimit != value)
                {
                    _EmployeePassportLimit = value;
                    NotifyPropertyChanged("EmployeePassportLimit");
                }
            }
        }
        #endregion

        #region EmployeeEmbasyLimit
        private int _EmployeeEmbasyLimit;

        public int EmployeeEmbasyLimit
        {
            get { return _EmployeeEmbasyLimit; }
            set
            {
                if (_EmployeeEmbasyLimit != value)
                {
                    _EmployeeEmbasyLimit = value;
                    NotifyPropertyChanged("EmployeeEmbasyLimit");
                }
            }
        }
        #endregion

        #region EmployeeVisaTakeOffLimit
        private int _EmployeeVisaTakeOffLimit;

        public int EmployeeVisaTakeOffLimit
        {
            get { return _EmployeeVisaTakeOffLimit; }
            set
            {
                if (_EmployeeVisaTakeOffLimit != value)
                {
                    _EmployeeVisaTakeOffLimit = value;
                    NotifyPropertyChanged("EmployeeVisaTakeOffLimit");
                }
            }
        }
        #endregion

        #region EmployeeVisaLimit
        private int _EmployeeVisaLimit;

        public int EmployeeVisaLimit
        {
            get { return _EmployeeVisaLimit; }
            set
            {
                if (_EmployeeVisaLimit != value)
                {
                    _EmployeeVisaLimit = value;
                    NotifyPropertyChanged("EmployeeVisaLimit");
                }
            }
        }
        #endregion

        #region EmployeeWorkLicenceLimit
        private int _EmployeeWorkLicenceLimit;

        public int EmployeeWorkLicenceLimit
        {
            get { return _EmployeeWorkLicenceLimit; }
            set
            {
                if (_EmployeeWorkLicenceLimit != value)
                {
                    _EmployeeWorkLicenceLimit = value;
                    NotifyPropertyChanged("EmployeeWorkLicenceLimit");
                }
            }
        }
        #endregion

        #region EmployeeDriveLicenceLimit
        private int _EmployeeDriveLicenceLimit;

        public int EmployeeDriveLicenceLimit
        {
            get { return _EmployeeDriveLicenceLimit; }
            set
            {
                if (_EmployeeDriveLicenceLimit != value)
                {
                    _EmployeeDriveLicenceLimit = value;
                    NotifyPropertyChanged("EmployeeDriveLicenceLimit");
                }
            }
        }
        #endregion

        #region EmployeeEmbasyFamilyLimit
        private int _EmployeeEmbasyFamilyLimit;

        public int EmployeeEmbasyFamilyLimit
        {
            get { return _EmployeeEmbasyFamilyLimit; }
            set
            {
                if (_EmployeeEmbasyFamilyLimit != value)
                {
                    _EmployeeEmbasyFamilyLimit = value;
                    NotifyPropertyChanged("EmployeeEmbasyFamilyLimit");
                }
            }
        }
        #endregion


        #region PersonPassportLimit
        private int _PersonPassportLimit;

        public int PersonPassportLimit
        {
            get { return _PersonPassportLimit; }
            set
            {
                if (_PersonPassportLimit != value)
                {
                    _PersonPassportLimit = value;
                    NotifyPropertyChanged("PersonPassportLimit");
                }
            }
        }
        #endregion

        #region PersonEmbasyLimit
        private int _PersonEmbasyLimit;

        public int PersonEmbasyLimit
        {
            get { return _PersonEmbasyLimit; }
            set
            {
                if (_PersonEmbasyLimit != value)
                {
                    _PersonEmbasyLimit = value;
                    NotifyPropertyChanged("PersonEmbasyLimit");
                }
            }
        }
        #endregion

        #region PersonVisaTakeOffLimit
        private int _PersonVisaTakeOffLimit;

        public int PersonVisaTakeOffLimit
        {
            get { return _PersonVisaTakeOffLimit; }
            set
            {
                if (_PersonVisaTakeOffLimit != value)
                {
                    _PersonVisaTakeOffLimit = value;
                    NotifyPropertyChanged("PersonVisaTakeOffLimit");
                }
            }
        }
        #endregion

        #region PersonVisaLimit
        private int _PersonVisaLimit;

        public int PersonVisaLimit
        {
            get { return _PersonVisaLimit; }
            set
            {
                if (_PersonVisaLimit != value)
                {
                    _PersonVisaLimit = value;
                    NotifyPropertyChanged("PersonVisaLimit");
                }
            }
        }
        #endregion

        #region PersonWorkLicenceLimit
        private int _PersonWorkLicenceLimit;

        public int PersonWorkLicenceLimit
        {
            get { return _PersonWorkLicenceLimit; }
            set
            {
                if (_PersonWorkLicenceLimit != value)
                {
                    _PersonWorkLicenceLimit = value;
                    NotifyPropertyChanged("PersonWorkLicenceLimit");
                }
            }
        }
        #endregion

        #region PersonDriveLicenceLimit
        private int _PersonDriveLicenceLimit;

        public int PersonDriveLicenceLimit
        {
            get { return _PersonDriveLicenceLimit; }
            set
            {
                if (_PersonDriveLicenceLimit != value)
                {
                    _PersonDriveLicenceLimit = value;
                    NotifyPropertyChanged("PersonDriveLicenceLimit");
                }
            }
        }
        #endregion

        #region PersonEmbasyFamilyLimit
        private int _PersonEmbasyFamilyLimit;

        public int PersonEmbasyFamilyLimit
        {
            get { return _PersonEmbasyFamilyLimit; }
            set
            {
                if (_PersonEmbasyFamilyLimit != value)
                {
                    _PersonEmbasyFamilyLimit = value;
                    NotifyPropertyChanged("PersonEmbasyFamilyLimit");
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

    }
}
