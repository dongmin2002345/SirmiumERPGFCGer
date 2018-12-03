using DomainCore.Common.Companies;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Companies;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Companies
{
    public class CompanyUserRepository : ICompanyUserRepository
    {
        ApplicationDbContext context;

        public CompanyUserRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public List<CompanyUser> GetCompanyUsers()
        {
            var companyUsers = context.CompanyUsers
                .Include(x => x.Company)
                .Include(x => x.User)
                .Where(x => x.Active == true)
                .ToList();

            return companyUsers;
        }

        public List<CompanyUser> GetCompanyUsersNewerThan(DateTime dateFrom)
        {
            var companyUsers = context.CompanyUsers
                .Include(x => x.Company)
                .Include(x => x.User)
                .Where(x => x.UpdatedAt > dateFrom)
                .ToList();

            return companyUsers;
        }

        public List<CompanyUser> GetCompanyUsersByUser(int userId)
        {
            var companyUsers = context.CompanyUsers
                .Include(x => x.Company)
                .Include(x => x.User)
                .Where(x => x.UserId == userId && x.Active == true)
                .ToList();

            return companyUsers;
        }

        public CompanyUser Create(CompanyUser compUser)
        {
            if (compUser.Id < 1)
            {
                compUser.Id = 0;

                compUser.Active = true;
                compUser.UpdatedAt = DateTime.Now;

                context.CompanyUsers.Add(compUser);
                return compUser;
            }
            else
            {
                var dbItem = context.CompanyUsers
                    .FirstOrDefault(x => x.Id == compUser.Id);

                if (dbItem != null)
                {
                    dbItem.CompanyId = compUser.CompanyId;
                    dbItem.UserId = compUser.UserId;
                    dbItem.RolesCSV = compUser.RolesCSV;

                    dbItem.UpdatedAt = DateTime.Now;
                }
                return dbItem;
            }
        }

        public CompanyUser Delete(Guid identifier)
        {
            var dbItem = context.CompanyUsers
                .FirstOrDefault(x => x.Identifier == identifier);

            if (dbItem != null)
            {
                dbItem.Active = false;

                dbItem.UpdatedAt = DateTime.Now;
            }
            return dbItem;
        }
    }
}
