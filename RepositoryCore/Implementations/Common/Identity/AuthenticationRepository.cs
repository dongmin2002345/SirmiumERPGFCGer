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
                .FirstOrDefault(x =>
                    x.Username == username &&
                    x.PasswordHash == passwordHash && x.Active == true);

            return user;
        }
    }
}