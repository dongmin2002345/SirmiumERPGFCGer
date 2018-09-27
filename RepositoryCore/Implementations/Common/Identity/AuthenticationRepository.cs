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
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private ApplicationDbContext context;

        public AuthenticationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public User Authenticate(string username, string passwordHash)
        {
            User user = context.Users

                //.Include(o => o.CompanyUsers)
                //.ThenInclude(o2 => o2.User)
                //.Include(o => o.CompanyUsers)
                //.ThenInclude(o2 => o2.Company)

                .Include("CompanyUsers")
                .Include("CompanyUsers.Company")
                .Include("CompanyUsers.User")

                .FirstOrDefault(x =>
                    x.Username == username &&
                    x.PasswordHash == passwordHash);

            //var tmpToRet = user.CompanyUsers.Where(x => x.CompanyId == companyId && x.UserId == user.Id).Select(x => x.User).FirstOrDefault();

            //if (tmpToRet != null)
            //{
            //    var businessPartner = context.BusinessPartners
            //        .Include(x => x.BusinessPartnerUsers)
            //        .ThenInclude(x => x.User)
            //        .FirstOrDefault(x => x.BusinessPartnerUsers != null && x.BusinessPartnerUsers.FirstOrDefault(y => y.User.Id == tmpToRet.Id) != null);

            //    tmpToRet.BusinessPartner = businessPartner;
            //}
            //return tmpToRet;
            return user;
        }
    }
}