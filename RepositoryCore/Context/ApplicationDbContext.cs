using DomainCore.Banks;
using DomainCore.CalendarAssignments;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.CallCentars;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.InputInvoices;
using DomainCore.Common.Invoices;
using DomainCore.Common.Locations;
using DomainCore.Common.OutputInvoices;
using DomainCore.Common.Phonebooks;
using DomainCore.Common.Prices;
using DomainCore.Common.Professions;
using DomainCore.Common.Sectors;
using DomainCore.Common.Shipments;
using DomainCore.Common.TaxAdministrations;
using DomainCore.Common.ToDos;
using DomainCore.ConstructionSites;
using DomainCore.Employees;
using DomainCore.Limitations;
using DomainCore.Statuses;
using DomainCore.Vats;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

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
        public DbSet<CompanyUser> CompanyUsers { get; set; }

        public DbSet<ToDo> ToDos { get; set; }

        public DbSet<BusinessPartner> BusinessPartners { get; set; }
        public DbSet<BusinessPartnerLocation> BusinessPartnerLocations { get; set; }
        public DbSet<BusinessPartnerPhone> BusinessPartnerPhones { get; set; }
        public DbSet<BusinessPartnerInstitution> BusinessPartnerInstitutions { get; set; }
        public DbSet<BusinessPartnerBank> BusinessPartnerBanks { get; set; }
        public DbSet<BusinessPartnerOrganizationUnit> BusinessPartnerOrganizationUnits { get; set; }
        public DbSet<BusinessPartnerType> BusinessPartnerTypes { get; set; }
        public DbSet<BusinessPartnerBusinessPartnerType> BusinessPartnerBusinessPartnerTypes { get; set; }
        public DbSet<BusinessPartnerDocument> BusinessPartnerDocuments { get; set; }
        public DbSet<BusinessPartnerNote> BusinessPartnerNotes { get; set; }

        public DbSet<OutputInvoice> OutputInvoices { get; set; }
        public DbSet<OutputInvoiceNote> OutputInvoiceNotes { get; set; }
        public DbSet<InputInvoice> InputInvoices { get; set; }
        public DbSet<InputInvoiceNote> InputInvoiceNotes { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Agency> Agencies { get; set; }

        public DbSet<Profession> Professions { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }

		public DbSet<PhysicalPerson> PhysicalPersons { get; set; }
        public DbSet<PhysicalPersonItem> PhysicalPersonItems { get; set; }
        public DbSet<PhysicalPersonNote> PhysicalPersonNotes { get; set; }
        public DbSet<PhysicalPersonCard> PhysicalPersonCards { get; set; }
        public DbSet<PhysicalPersonDocument> PhysicalPersonDocuments { get; set; }
        public DbSet<PhysicalPersonLicence> PhysicalPersonLicences { get; set; }
        public DbSet<PhysicalPersonProfession> PhysicalPersonProfessions { get; set; }

		public DbSet<InputInvoiceDocument> InputInvoiceDocuments { get; set; }
		public DbSet<OutputInvoiceDocument> OutputInvoiceDocuments { get; set; }

		public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeItem> EmployeeItems { get; set; }
        public DbSet<EmployeeNote> EmployeeNotes { get; set; }
        public DbSet<EmployeeCard> EmployeeCards { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public DbSet<EmployeeLicence> EmployeeLicences { get; set; }
        public DbSet<EmployeeProfession> EmployeeProfessions { get; set; }
        public DbSet<EmployeeByConstructionSite> EmployeeByConstructionSites { get; set; }

        public DbSet<EmployeeByBusinessPartner> EmployeeByBusinessPartners { get; set; }

        public DbSet<BusinessPartnerByConstructionSite> BusinessPartnerByConstructionSites { get; set; }

        public DbSet<ConstructionSite> ConstructionSites { get; set; }
        public DbSet<ConstructionSiteCalculation> ConstructionSiteCalculations { get; set; }
        public DbSet<ConstructionSiteDocument> ConstructionSiteDocuments { get; set; }
        public DbSet<ConstructionSiteNote> ConstructionSiteNotes { get; set; }


        public DbSet<Bank> Banks { get; set; }
		public DbSet<LicenceType> LicenceTypes { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<TaxAdministration> TaxAdministrations { get; set; }

        public DbSet<Limitation> Limitations { get; set; }
        public DbSet<LimitationEmail> LimitationEmails { get; set; }

        public DbSet<Vat> Vats { get; set; }
        public DbSet<ServiceDelivery> ServiceDeliverys { get; set; }
        public DbSet<Status> Statuses { get; set; }

        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentDocument> ShipmentDocuments { get; set; }

        public DbSet<Phonebook> Phonebooks { get; set; }
        public DbSet<PhonebookPhone> PhonebookPhones { get; set; }
        public DbSet<PhonebookDocument> PhonebookDocuments { get; set; }
        public DbSet<PhonebookNote> PhonebookNotes { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        public DbSet<CallCentar> CallCentars { get; set; }

        public DbSet<CalendarAssignment> CalendarAssignments { get; set; }

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

