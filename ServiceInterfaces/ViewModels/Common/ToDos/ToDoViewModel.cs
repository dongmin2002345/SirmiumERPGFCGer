using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.ToDos
{
    public class ToDoViewModel : BaseEntityViewModel
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

        #region User
        private UserViewModel _User;

        public UserViewModel User
        {
            get { return _User; }
            set
            {
                if (_User != value)
                {
                    _User = value;
                    NotifyPropertyChanged("User");
                }
            }
        }
        #endregion

        #region ToDoStatus
        private ToDoStatusViewModel _ToDoStatus;

        public ToDoStatusViewModel ToDoStatus
        {
            get { return _ToDoStatus; }
            set
            {
                if (_ToDoStatus != value)
                {
                    _ToDoStatus = value;
                    NotifyPropertyChanged("ToDoStatus");
                }
            }
        }
        #endregion


        #region ToDoDate
        private DateTime _ToDoDate;

        public DateTime ToDoDate
        {
            get { return _ToDoDate; }
            set
            {
                if (_ToDoDate != value)
                {
                    _ToDoDate = value;
                    NotifyPropertyChanged("ToDoDate");
                }
            }
        }
        #endregion

        #region IsPrivate
        private bool _IsPrivate;

        public bool IsPrivate
        {
            get { return _IsPrivate; }
            set
            {
                if (_IsPrivate != value)
                {
                    _IsPrivate = value;
                    NotifyPropertyChanged("IsPrivate");
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
