using DataMapper.Mappers.Common.Companies;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DataMapper.Mappers.Common.Identity
{
    public static class UserMapper
    {
        public static List<UserViewModel> ConvertToUserViewModelList(this IEnumerable<User> users)
        {
            List<UserViewModel> userViewModels = new List<UserViewModel>();
            foreach (User user in users)
            {
                userViewModels.Add(user.ConvertToUserViewModel());
            }
            return userViewModels;
        }

        public static UserViewModel ConvertToUserViewModel(this User user)
        {
            UserViewModel userViewModel = new UserViewModel()
            {
                Id = user.Id,
                Identifier = user.Identifier,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.PasswordHash,
                Email = user.Email,
                IsActive = user.Active,

                UpdatedAt = user.UpdatedAt,
                CreatedAt = user.CreatedAt
            };
            return userViewModel;
        }

        public static UserViewModel ConvertToUserViewModelLite(this User user)
        {
            UserViewModel userViewModel = new UserViewModel()
            {
                Id = user.Id,
                Identifier = user.Identifier,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.PasswordHash,
                Email = user.Email,
                IsActive = user.Active,
                UpdatedAt = user.UpdatedAt,
                CreatedAt = user.CreatedAt
            };
            return userViewModel;
        }

        public static User ConvertToUser(this UserViewModel userViewModel)
        {

            string rolesCSV = "";
            foreach (String role in userViewModel.Roles)
            {
                rolesCSV += role + ",";
            }
            if (rolesCSV.Length > 0)
                rolesCSV = rolesCSV.Substring(0, rolesCSV.Length - 1);

            //var companies = new List<CompanyUsers>();

            //if (userViewModel.Companies != null)
            //{
            //    foreach (var item in userViewModel.Companies)
            //    {
            //        companies.Add(new CompanyUsers()
            //        {
            //            Company = item?.ConvertToCompany()
            //        });
            //    }
            //}

            var userPassword = (String.IsNullOrEmpty(userViewModel.Password) == false ? CalculateHash(userViewModel.Password, userViewModel.Username) : null);

            User user = new User()
            {
                Id = userViewModel.Id,
                Identifier = userViewModel.Identifier,
                Username = userViewModel.Username,
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                PasswordHash = userPassword,
                Email = userViewModel.Email,
            };

            return user;
        }

        public static string CalculateHash(string password, string username)
        {
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(password + username);
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            return Convert.ToBase64String(hash);
        }
    }
}
