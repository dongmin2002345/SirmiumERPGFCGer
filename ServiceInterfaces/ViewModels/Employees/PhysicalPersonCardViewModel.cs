using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Employees
{
    public class PhysicalPersonCardViewModel : BaseEntityViewModel
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

        #region CardDate
        private DateTime? _CardDate;

        public DateTime? CardDate
        {
            get { return _CardDate; }
            set
            {
                if (_CardDate != value)
                {
                    _CardDate = value;
                    NotifyPropertyChanged("CardDate");
                }
            }
        }
        #endregion

        #region Description
        private string _Description;

        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }
        #endregion

        #region PlusMinus
        private string _PlusMinus;

        public string PlusMinus
        {
            get { return _PlusMinus; }
            set
            {
                if (_PlusMinus != value)
                {
                    _PlusMinus = value;
                    NotifyPropertyChanged("PlusMinus");
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
