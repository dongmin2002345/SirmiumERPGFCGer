using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using Microsoft.EntityFrameworkCore;
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

            if (context.Users.Where(x => x.Identifier != Guid.Empty).Count() == 0 || context.CompanyUsers.Count() == 0)
            {

                if (context.Users.Count(x => x.Email == "admin@admin.com") == 0)
                {
                    saltedHashBytes = Encoding.UTF8.GetBytes("Secret123$" + "Admin");
                    algorithm = new SHA256Managed();
                    byte[] hash = algorithm.ComputeHash(saltedHashBytes);
                    string password = Convert.ToBase64String(hash);

                    var userForDb = new User()
                    {
                        Identifier = Guid.NewGuid(),
                        Username = "Admin",
                        FirstName = "Petar",
                        LastName = "Petrovic",
                        PasswordHash = password,
                        Email = "admin@admin.com",
                        Active = true,
                        UpdatedAt = DateTime.Now,
                        CreatedAt = DateTime.Now
                    };
                    context.Users.Add(userForDb);
                    context.SaveChanges();
                }
                if (context.Users.FirstOrDefault(x => x.Email == "admin@admin.com").Identifier == Guid.Empty)
                {
                    var user = context.Users.FirstOrDefault(x => x.Email == "admin@admin.com");
                    user.Identifier = Guid.NewGuid();
                    user.UpdatedAt = DateTime.Now;
                    context.SaveChanges();
                }

                if (context.CompanyUsers.Include(x => x.User).Where(x => x.Identifier != Guid.Empty && x.User.Identifier != Guid.Empty).Count() == 0)
                {
                    if (context.CompanyUsers.Include(x => x.User).Count(x => x.User.Email == "admin@admin.com") == 0)
                    {
                        var companyUser = new CompanyUser()
                        {
                            Identifier = Guid.NewGuid(),
                            UserId = context.Users.FirstOrDefault(x => x.Email == "admin@admin.com")?.Id ?? null,
                            CompanyId = context.Companies.FirstOrDefault()?.Id,
                            RolesCSV = "Admin",
                            UpdatedAt = DateTime.Now,
                            Active = true
                        };
                        context.CompanyUsers.Add(companyUser);
                        context.SaveChanges();
                    }
                }
            }
        }

        public void SeedCompanyData()
        {
            try
            {
                Company company = context.Companies.FirstOrDefault();

                if (company == null)
                {
                    context.Companies.Add(new Company()
                    {
                        Code = "KOR-1",
                        Identifier = Guid.NewGuid(),
                        Name = "MS Accounting Office",
                        PIBNumber = "000000",
                        PDVNumber = "18",
                        Address = "Primer adresa",
                        Email = "mail@firma.com",
                        IndustryCode = "1001",
                        IndustryName = "Farma",
                        PIONumber = "1001010",
                        WebSite = "http://farma.rs",
                        IdentificationNumber = "313123123132131232",
                        Active = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
