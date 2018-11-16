using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Employees
{
    public class EmployeeCardViewModel : BaseEntityViewModel
    {
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
