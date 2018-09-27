using DomainCore.Common.Identity;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RepositoryCore.DbSeed
{
    public class SeedData : ISeedData
    {
        private ApplicationDbContext context;

        public SeedData(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void SeedUserData()
        {
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes("Secret123$" + "Korisnik");
            HashAlgorithm algorithm = new SHA256Managed();

            if (context.Users.Count() == 0)
            {
                if (context.Users.Count(x => x.Email == "admin@admin.com") == 0)
                {
                    saltedHashBytes = Encoding.UTF8.GetBytes("Secret123$" + "Admin");
                    algorithm = new SHA256Managed();
                    byte[] hash = algorithm.ComputeHash(saltedHashBytes);
                    string password = Convert.ToBase64String(hash);
                    context.Users.Add(new User()
                    {

                        Username = "Admin",
                        FirstName = "Petar",
                        LastName = "Petrovic",
                        PasswordHash = password,
                        Email = "admin@admin.com",
                        //RolesCSV = "Admin",
                        Active = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
