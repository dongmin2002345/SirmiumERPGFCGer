using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Employees
{
    public class PhysicalPersonDocumentViewModel : BaseEntityViewModel
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

        #region CreateDate
        private DateTime? _CreateDate;

        public DateTime? CreateDate
        {
            get { return _CreateDate; }
            set
            {
                if (_CreateDate != value)
                {
                    _CreateDate = value;
                    NotifyPropertyChanged("CreateDate");
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
