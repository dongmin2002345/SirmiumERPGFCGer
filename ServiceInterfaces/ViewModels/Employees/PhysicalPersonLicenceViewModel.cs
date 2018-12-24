using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Employees
{
    public class PhysicalPersonLicenceViewModel : BaseEntityViewModel
    {
        #region PhysicalPerson
        private PhysicalPersonViewModel _PhysicalPerson;

        public PhysicalPersonViewModel PhysicalPerson
        {
            get { return _PhysicalPerson; }
            set
            {
                if (_PhysicalPerson != value)
                {
                    _PhysicalPerson = value;
                    NotifyPropertyChanged("PhysicalPerson");
                }
            }
        }
        #endregion

        #region Licence
        private LicenceTypeViewModel _Licence;

        public LicenceTypeViewModel Licence
        {
            get { return _Licence; }
            set
            {
                if (_Licence != value)
                {
                    _Licence = value;
                    NotifyPropertyChanged("Licence");
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

        #region ValidFrom
        private DateTime? _ValidFrom;

        public DateTime? ValidFrom
        {
            get { return _ValidFrom; }
            set
            {
                if (_ValidFrom != value)
                {
                    _ValidFrom = value;
                    NotifyPropertyChanged("ValidFrom");
                }
            }
        }
        #endregion

        #region ValidTo
        private DateTime? _ValidTo;

        public DateTime? ValidTo
        {
            get { return _ValidTo; }
            set
            {
                if (_ValidTo != value)
                {
                    _ValidTo = value;
                    NotifyPropertyChanged("ValidTo");
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
