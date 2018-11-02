using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Employees
{
    public class EmployeeByBusinessPartnerHistoryViewModel : BaseEntityViewModel
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

        #region BussinesPartner
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
    }
}
