using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Helpers
{
    public static class UserRoleHelper
    {
        public static List<string> ConvertToRoleList(this List<UserRoleViewModel> userRoles)
        {
            List<string> roles = new List<string>();
            if (userRoles != null)
            {
                foreach (var item in userRoles)
                {
                    roles.Add(item?.Name);
                }
            }

            return roles;
        }

        public static string ConvertToRole(this UserRoleViewModel role)
        {
            return role?.Name;
        }

        public static List<UserRoleViewModel> ConvertToRoleViewModelList(this List<string> userRoles)
        {
            List<UserRoleViewModel> roles = new List<UserRoleViewModel>();
            if (userRoles != null)
            {
                foreach (var item in userRoles)
                {
                    roles.Add(item?.ConvertToRoleViewModel());
                }
            }

            return roles;
        }

        public static UserRoleViewModel ConvertToRoleViewModel(this string role)
        {
            var viewModel = new UserRoleViewModel()
            {
                Name = role,
                IsChecked = true
            };
            return viewModel;
        }

        public static string RoleStringsToCSV(this List<string> roles)
        {
            string rolesCSV = "";
            foreach (String role in roles)
            {
                rolesCSV += role + ",";
            }
            if (rolesCSV.Length > 0)
                rolesCSV = rolesCSV.Substring(0, rolesCSV.Length - 1);

            return rolesCSV;
        }

        public static string RolesToCSV(this List<UserRoleViewModel> roles)
        {
            string rolesCSV = "";
            foreach (var role in roles)
            {
                rolesCSV += role?.Name + ",";
            }
            if (rolesCSV.Length > 0)
                rolesCSV = rolesCSV.Substring(0, rolesCSV.Length - 1);

            return rolesCSV;
        }

        public static List<string> RoleStringsFromCSV(this string csv)
        {
            var userRolesSplit = csv?.Split(',')?.ToList() ?? new List<string>();
            return userRolesSplit;
        }

        public static List<UserRoleViewModel> RolesFromCSV(this string csv)
        {
            var splitRoles = csv.RoleStringsFromCSV();
            var rolesToReturn = splitRoles?.ConvertToRoleViewModelList() ?? new List<UserRoleViewModel>();

            return rolesToReturn;
        }
    }
}
