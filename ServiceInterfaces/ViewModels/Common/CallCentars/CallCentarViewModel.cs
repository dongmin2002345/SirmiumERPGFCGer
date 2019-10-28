using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.CallCentars
{
    public class CallCentarViewModel : BaseEntityViewModel
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

        #region ReceivingDate
        private DateTime _ReceivingDate = DateTime.Now;

        public DateTime ReceivingDate
        {
            get { return _ReceivingDate; }
            set
            {
                if (_ReceivingDate != value)
                {
                    _ReceivingDate = value;
                    NotifyPropertyChanged("ReceivingDate");
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

        #region Comment
        private string _Comment;

        public string Comment
        {
            get { return _Comment; }
            set
            {
                if (_Comment != value)
                {
                    _Comment = value;
                    NotifyPropertyChanged("Comment");
                }
            }
        }
        #endregion

        #region EndingDate
        private DateTime _EndingDate = DateTime.Now;

        public DateTime EndingDate
        {
            get { return _EndingDate; }
            set
            {
                if (_EndingDate != value)
                {
                    _EndingDate = value;
                    NotifyPropertyChanged("EndingDate");
                }
            }
        }
        #endregion

        #region IsSelected
        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    NotifyPropertyChanged("IsSelected");
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
