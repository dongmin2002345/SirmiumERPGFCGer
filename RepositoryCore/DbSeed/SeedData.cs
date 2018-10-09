using DomainCore.Common.Companies;
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

        public void SeedCompanyData()
        {
            try
            {
                Company company = context.Companies.FirstOrDefault();

                if (company == null)
                {
                    context.Companies.Add(new Company()
                    {
                        Code = 1,
                        Identifier = Guid.NewGuid(),
                        Name = "MS Accounting Office",
                        PIBNumber = "000000",
                        PDVNumber = "18",
                        Address = "Primer adresa",
                        BankAccountName = "Ime korisnika racuna",
                        BankAccountNo = "111-123-1321",
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
