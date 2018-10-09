using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Locations;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Individuals;
using DomainCore.Common.OutputInvoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using DomainCore.Common.Sectors;
using DomainCore.Common.Professions;
using DomainCore.Banks;
using DomainCore.ConstructionSites;
using DomainCore.Employees;
using DomainCore.Employees;

namespace RepositoryCore.Context
{
    public class TraceLogger : ILogger
    {
        private readonly string categoryName;

        public TraceLogger(string categoryName) => this.categoryName = categoryName;

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Trace.WriteLine($"{DateTime.Now.ToString("o")} {logLevel} {eventId.Id} {this.categoryName}");
            Trace.WriteLine(formatter(state, exception));
        }
    }

    public class TraceLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new TraceLogger(categoryName);
        }

        public void Dispose()
        {

        }
    }


    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<BusinessPartner> BusinessPartners { get; set; }

        public DbSet<Individual> Individuals { get; set; }

        public DbSet<OutputInvoice> OutputInvoices { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<Sector> Sectors { get; set; }

        public DbSet<Profession> Professions { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<ConstructionSite> ConstructionSites { get; set; }


		public DbSet<Bank> Banks { get; set; }
		public DbSet<LicenceType> LicenceTypes { get; set; }



		//protected override void OnModelCreating(ModelBuilder modelBuilder)

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // User mapping
            //modelBuilder.Entity<BusinessPartner>().HasOne(bp => bp.).WithOne(u => u.BusinessPartner);
            //modelBuilder.Entity<User>().HasMany(u => u.CreatedCompanies).WithOne(c => c.CreatedBy);

            //// Identity settings
            //modelBuilder.Entity<Company>().HasOne(x => x.City).WithMany(y => y.Companies);
            //modelBuilder.Entity<Company>().HasOne(x => x.Municipality).WithMany(y => y.Companies);
            //modelBuilder.Entity<Company>().HasOne(x => x.Country).WithMany(y => y.Companies);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //LoggerFactory loggerFactory = new LoggerFactory();
            //loggerFactory.AddProvider(new TraceLoggerProvider());
            //optionsBuilder.UseLoggerFactory(loggerFactory);


            //optionsBuilder.UseLazyLoadingProxies();
        }

    }
}

