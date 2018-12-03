using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.Companies
{
    public class CompanyUserViewModel : BaseEntityViewModel
    {
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

        #region Company
        private CompanyViewModel _Company;

        public new CompanyViewModel Company
        {
            get { return _Company; }
            set
            {
                if (_Company != value)
                {
                    _Company = value;
                    NotifyPropertyChanged("Company");
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

        #region UserRoles
        private List<UserRoleViewModel> _UserRoles;

        public List<UserRoleViewModel> UserRoles
        {
            get { return _UserRoles; }
            set
            {
                if (_UserRoles != value)
                {
                    _UserRoles = value;
                    NotifyPropertyChanged("UserRoles");
                }
            }
        }
        #endregion
    }
}
