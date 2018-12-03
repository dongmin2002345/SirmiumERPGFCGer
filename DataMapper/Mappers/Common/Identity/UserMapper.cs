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
                Code = user.Code,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.PasswordHash,
                Email = user.Email,

                CompanyUsers = user.CompanyUsers?.ConvertToCompanyUserViewModelList(),

                IsActive = user.Active,

                UpdatedAt = user.UpdatedAt,
            };
            return userViewModel;
        }

        public static List<UserViewModel> ConvertToUserViewModelListLite(this IEnumerable<User> users)
        {
            List<UserViewModel> userViewModels = new List<UserViewModel>();
            foreach (User user in users)
            {
                userViewModels.Add(user.ConvertToUserViewModelLite());
            }
            return userViewModels;
        }

        public static UserViewModel ConvertToUserViewModelLite(this User user)
        {
            UserViewModel userViewModel = new UserViewModel()
            {
                Id = user.Id,
                Identifier = user.Identifier,
                Code = user.Code,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.PasswordHash,
                Email = user.Email,
                IsActive = user.Active,

                UpdatedAt = user.UpdatedAt,
            };
            return userViewModel;
        }

        public static User ConvertToUser(this UserViewModel userViewModel)
        {
            User user = new User()
            {
                Id = userViewModel.Id,
                Identifier = userViewModel.Identifier,
                Code = userViewModel.Code,
                Username = userViewModel.Username,
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                PasswordHash = userViewModel.Password,
                Email = userViewModel.Email,
                Active = userViewModel.IsActive,
                // Roles = userViewModel.userRolesSplit, // ??
                CompanyUsers = userViewModel?.CompanyUsers?.ConvertToCompanyUserList(),

                //UpdatedAt = userViewModel.UpdatedAt
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
