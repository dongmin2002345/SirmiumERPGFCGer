using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Employees
{
    public class EmployeeByBusinessPartnerViewModel : BaseEntityViewModel
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

        #region StartDate
        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set
            {
                if (_StartDate != value)
                {
                    _StartDate = value;
                    NotifyPropertyChanged("StartDate");
                }
            }
        }
        #endregion

        #region EndDate
        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
            set
            {
                if (_EndDate != value)
                {
                    _EndDate = value;
                    NotifyPropertyChanged("EndDate");
                }
            }
        }
        #endregion

        #region RealEndDate
        private DateTime? _RealEndDate;

        public DateTime? RealEndDate
        {
            get { return _RealEndDate; }
            set
            {
                if (_RealEndDate != value)
                {
                    _RealEndDate = value;
                    NotifyPropertyChanged("RealEndDate");
                }
            }
        }
        #endregion


        #region Employee
        private EmployeeViewModel _Employee;

        public EmployeeViewModel Employee
        {
            get { return _Employee; }
            set
            {
                if (_Employee != value)
                {
                    _Employee = value;
                    NotifyPropertyChanged("Employee");
                }
            }
        }
        #endregion

        #region EmployeeCount
        private int _EmployeeCount;

        public int EmployeeCount
        {
            get { return _EmployeeCount; }
            set
            {
                if (_EmployeeCount != value)
                {
                    _EmployeeCount = value;
                    NotifyPropertyChanged("EmployeeCount");
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


        #region Search_Employee
        private EmployeeViewModel _Search_Employee;

        public EmployeeViewModel Search_Employee
        {
            get { return _Search_Employee; }
            set
            {
                if (_Search_Employee != value)
                {
                    _Search_Employee = value;
                    NotifyPropertyChanged("Search_Employee");
                }
            }
        }
        #endregion
    }
}
