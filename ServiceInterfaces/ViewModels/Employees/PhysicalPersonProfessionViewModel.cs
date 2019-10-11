using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Employees
{
    public class PhysicalPersonProfessionViewModel : BaseEntityViewModel
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

        #region Profession
        private ProfessionViewModel _Profession;

        public ProfessionViewModel Profession
        {
            get { return _Profession; }
            set
            {
                if (_Profession != value)
                {
                    _Profession = value;
                    NotifyPropertyChanged("Profession");
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

        #region ItemStatus
        private int _ItemStatus;

        public int ItemStatus
        {
            get { return _ItemStatus; }
            set
            {
                if (_ItemStatus != value)
                {
                    _ItemStatus = value;
                    NotifyPropertyChanged("ItemStatus");
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
