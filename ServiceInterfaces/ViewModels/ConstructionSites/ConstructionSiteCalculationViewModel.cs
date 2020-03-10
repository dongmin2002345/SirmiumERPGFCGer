using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.ConstructionSites
{
    public class ConstructionSiteCalculationViewModel : BaseEntityViewModel
    {
        #region ConstructionSite
        private ConstructionSiteViewModel _ConstructionSite;

        public ConstructionSiteViewModel ConstructionSite
        {
            get { return _ConstructionSite; }
            set
            {
                if (_ConstructionSite != value)
                {
                    _ConstructionSite = value;
                    NotifyPropertyChanged("ConstructionSite");
                }
            }
        }
        #endregion

        #region StatusDate
        private DateTime _StatusDate;

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
    
        #region NumOfEmployees
        private int _NumOfEmployees;

        public int NumOfEmployees
        {
            get { return _NumOfEmployees; }
            set
            {
                if (_NumOfEmployees != value)
                {
                    _NumOfEmployees = value;
                    NotifyPropertyChanged("NumOfEmployees");
                }
            }
        }
        #endregion

        #region EmployeePrice
        private decimal _EmployeePrice;

        public decimal EmployeePrice
        {
            get { return _EmployeePrice; }
            set
            {
                if (_EmployeePrice != value)
                {
                    _EmployeePrice = value;
                    NotifyPropertyChanged("EmployeePrice");
                }
            }
        }
        #endregion

        #region NumOfMonths
        private int _NumOfMonths;

        public int NumOfMonths
        {
            get { return _NumOfMonths; }
            set
            {
                if (_NumOfMonths != value)
                {
                    _NumOfMonths = value;
                    NotifyPropertyChanged("NumOfMonths");
                }
            }
        }
        #endregion


        #region OldValue
        private decimal _OldValue;

        public decimal OldValue
        {
            get { return _OldValue; }
            set
            {
                if (_OldValue != value)
                {
                    _OldValue = value;
                    NotifyPropertyChanged("OldValue");
                }
            }
        }
        #endregion

        #region ValueDifference
        private decimal _ValueDifference;

        public decimal ValueDifference
        {
            get { return _ValueDifference; }
            set
            {
                if (_ValueDifference != value)
                {
                    _ValueDifference = value;
                    NotifyPropertyChanged("ValueDifference");
                }
            }
        }
        #endregion

        #region NewValue
        private decimal _NewValue;

        public decimal NewValue
        {
            get { return _NewValue; }
            set
            {
                if (_NewValue != value)
                {
                    _NewValue = value;
                    NotifyPropertyChanged("NewValue");
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
