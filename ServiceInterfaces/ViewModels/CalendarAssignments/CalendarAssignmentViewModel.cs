using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.ViewModels.CalendarAssignments
{
    public class CalendarAssignmentViewModel : BaseEntityViewModel
    {
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

        #region AssignedTo
        private UserViewModel _AssignedTo;

        public UserViewModel AssignedTo
        {
            get { return _AssignedTo; }
            set
            {
                if (_AssignedTo != value)
                {
                    _AssignedTo = value;
                    NotifyPropertyChanged("AssignedTo");
                }
            }
        }
        #endregion

        #region Date
        private DateTime _Date;

        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    NotifyPropertyChanged("Date");
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
