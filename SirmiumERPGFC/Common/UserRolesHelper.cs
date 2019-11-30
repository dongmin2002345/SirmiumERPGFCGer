using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Common
{
    public static class UserRolesHelper
    {
        public static List<UserRoleViewModel> GetAvailableRoles()
        {
            return new List<UserRoleViewModel>()
            {
                new UserRoleViewModel() { Name = "Admin" },
                new UserRoleViewModel() { Name = "Korisnik" },
                new UserRoleViewModel() { Name = "CallCentar" },
            };
        }
    }
}
