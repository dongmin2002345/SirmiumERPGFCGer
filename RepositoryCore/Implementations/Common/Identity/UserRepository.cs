using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Identity;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Identity
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<User> GetUsers(int companyId)
        {
            List<User> Users = context.Users
                .Where(x => x.Active == true)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return Users;
        }

        public List<User> GetUsersNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<User> Users = context.Users
                .Where(x => x.UpdatedAt > lastUpdateTime)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return Users;
        }

        private string GetNewCodeValue()
        {
            int count = context.Users
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(User))
                    .Select(x => x.Entity as User))
                    .Count();
            if (count == 0)
                return "KORISNIK-00001";
            else
            {
                string activeCode = context.Users
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(User))
                        .Select(x => x.Entity as User))
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("KORISNIK-", ""));
                    return "KORISNIK-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public User Create(User user)
        {
            if (context.Users.Where(x => x.Identifier != null && x.Identifier == user.Identifier).Count() == 0)
            {
                user.Id = 0;

                user.Code = GetNewCodeValue();
                user.Active = true;

                user.UpdatedAt = DateTime.Now;
                user.CreatedAt = DateTime.Now;
                context.Users.Add(user);
                return user;
            }
            else
            {
                // Load user that will be updated
                User dbEntry = context.Users
                .FirstOrDefault(x => x.Identifier == user.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    // Set properties
                    dbEntry.Code = user.Code;
                    dbEntry.Username = user.Username;
                    dbEntry.FirstName = user.FirstName;
                    dbEntry.LastName = user.LastName;
                    dbEntry.PasswordHash = user.PasswordHash;
                    dbEntry.Email = user.Email;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public User Delete(Guid identifier)
        {
            // Load User that will be deleted
            User dbEntry = context.Users
                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

            if (dbEntry != null)
            {
                // Set activity
                dbEntry.Active = false;
                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }
    }
}

