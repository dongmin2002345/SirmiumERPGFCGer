using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using DomainCore.Common.Sectors;
using DomainCore.Common.TaxAdministrations;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerViewRepository : IBusinessPartnerRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public BusinessPartnerViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<BusinessPartner> GetBusinessPartners(int companyId)
        {
            List<BusinessPartner> BusinessPartners = new List<BusinessPartner>();

            string queryString =
                "SELECT BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerInternalCode, BusinessPartnerName, " +
                "PIB, PIO, PDV, IdentificationNumber, Rebate, DueDate, WebSite, ContactPerson, IsInPDV, JBKJS, NameGer, IsInPDVGer, " +
                "TaxAdministrationId, TaxAdministrationIdentifier, TaxAdministrationCode, TaxAdministrationName, " +
                "IBAN, BetriebsNumber, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "SectorId, SectorIdentifier, SectorCode, SectorName, " +
                "AgencyId, AgencyIdentifier, AgencyCode, AgencyName, " +
                "TaxNr, CommercialNr, ContactPersonGer, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartners " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartner businessPartner;
                    while (reader.Read())
                    {
                        businessPartner = new BusinessPartner();
                        businessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                        businessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                        businessPartner.Code = reader["BusinessPartnerCode"].ToString();
                        businessPartner.InternalCode = reader["BusinessPartnerInternalCode"]?.ToString();
                        if (reader["BusinessPartnerName"] != DBNull.Value)
                            businessPartner.Name = reader["BusinessPartnerName"].ToString();

                        if (reader["PIB"] != DBNull.Value)
                            businessPartner.PIB = reader["PIB"].ToString();
                        if (reader["PIO"] != DBNull.Value)
                            businessPartner.PIO = reader["PIO"].ToString();
                        if (reader["PDV"] != DBNull.Value)
                            businessPartner.PDV = reader["PDV"].ToString();
                        if (reader["IdentificationNumber"] != DBNull.Value)
                            businessPartner.IdentificationNumber = reader["IdentificationNumber"].ToString();
                        if (reader["Rebate"] != DBNull.Value)
                            businessPartner.Rebate = decimal.Parse(reader["Rebate"].ToString());
                        if (reader["DueDate"] != DBNull.Value)
                            businessPartner.DueDate = Int32.Parse(reader["DueDate"].ToString());
                        if (reader["WebSite"] != DBNull.Value)
                            businessPartner.WebSite = reader["WebSite"].ToString();
                        if (reader["ContactPerson"] != DBNull.Value)
                            businessPartner.ContactPerson = reader["ContactPerson"].ToString();
                        if (reader["IsInPDV"] != DBNull.Value)
                            businessPartner.IsInPDV = bool.Parse(reader["IsInPDV"].ToString());
                        if (reader["JBKJS"] != DBNull.Value)
                            businessPartner.JBKJS = reader["JBKJS"].ToString();
                        if (reader["NameGer"] != DBNull.Value)
                            businessPartner.NameGer = reader["NameGer"].ToString();
                        if (reader["IsInPDVGer"] != DBNull.Value)
                            businessPartner.IsInPDVGer = bool.Parse(reader["IsInPDVGer"].ToString());

                        if (reader["TaxAdministrationId"] != DBNull.Value)
                        {
                            businessPartner.TaxAdministration = new TaxAdministration();
                            businessPartner.TaxAdministrationId = Int32.Parse(reader["TaxAdministrationId"].ToString());
                            businessPartner.TaxAdministration.Id = Int32.Parse(reader["TaxAdministrationId"].ToString());
                            businessPartner.TaxAdministration.Identifier = Guid.Parse(reader["TaxAdministrationIdentifier"].ToString());
                            businessPartner.TaxAdministration.Code = reader["TaxAdministrationCode"].ToString();
                            businessPartner.TaxAdministration.Name = reader["TaxAdministrationName"].ToString();
                        }

                        if (reader["IBAN"] != DBNull.Value)
                            businessPartner.IBAN = reader["IBAN"].ToString();
                        if (reader["BetriebsNumber"] != DBNull.Value)
                            businessPartner.BetriebsNumber = reader["BetriebsNumber"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartner.Country = new Country();
                            businessPartner.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartner.Country.Code = reader["CountryCode"].ToString();
                            businessPartner.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["SectorId"] != DBNull.Value)
                        {
                            businessPartner.Sector = new Sector();
                            businessPartner.SectorId = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Id = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Identifier = Guid.Parse(reader["SectorIdentifier"].ToString());
                            businessPartner.Sector.Code = reader["SectorCode"].ToString();
                            businessPartner.Sector.Name = reader["SectorName"].ToString();
                        }

                        if (reader["AgencyId"] != DBNull.Value)
                        {
                            businessPartner.Agency = new Agency();
                            businessPartner.AgencyId = Int32.Parse(reader["AgencyId"].ToString());
                            businessPartner.Agency.Id = Int32.Parse(reader["AgencyId"].ToString());
                            businessPartner.Agency.Identifier = Guid.Parse(reader["AgencyIdentifier"].ToString());
                            businessPartner.Agency.Code = reader["AgencyCode"].ToString();
                            businessPartner.Agency.Name = reader["AgencyName"].ToString();
                        }

                        if (reader["TaxNr"] != DBNull.Value)
                            businessPartner.TaxNr = reader["TaxNr"].ToString();
                        if (reader["CommercialNr"] != DBNull.Value)
                            businessPartner.CommercialNr = reader["CommercialNr"].ToString();
                        if (reader["ContactPersonGer"] != DBNull.Value)
                            businessPartner.ContactPersonGer = reader["ContactPersonGer"].ToString();

                        businessPartner.Active = bool.Parse(reader["Active"].ToString());
                        businessPartner.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartner.CreatedBy = new User();
                            businessPartner.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartner.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartner.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartner.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartner.Company = new Company();
                            businessPartner.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartner.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartner.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartners.Add(businessPartner);
                    }
                }
            }
            return BusinessPartners;


            //List<BusinessPartner> businessPartners = context.BusinessPartners
            //    .Include(x => x.Country)
            //    .Include(x => x.Sector)
            //    .Include(x => x.Agency)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return businessPartners;
        }

        public List<BusinessPartner> GetBusinessPartnersNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartner> BusinessPartners = new List<BusinessPartner>();

            string queryString =
                "SELECT BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerInternalCode, BusinessPartnerName, " +
                "PIB, PIO, PDV, IdentificationNumber, Rebate, DueDate, WebSite, ContactPerson, IsInPDV, JBKJS, NameGer, IsInPDVGer, " +
                "TaxAdministrationId, TaxAdministrationIdentifier, TaxAdministrationCode, TaxAdministrationName, " +
                "IBAN, BetriebsNumber, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "SectorId, SectorIdentifier, SectorCode, SectorName, " +
                "AgencyId, AgencyIdentifier, AgencyCode, AgencyName, " +
                "TaxNr, CommercialNr, ContactPersonGer, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartners " +
                "WHERE CompanyId = @CompanyId AND UpdatedAt > @LastUpdateTime;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartner businessPartner;
                    while (reader.Read())
                    {
                        businessPartner = new BusinessPartner();
                        businessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                        businessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                        businessPartner.Code = reader["BusinessPartnerCode"].ToString();
                        businessPartner.InternalCode = reader["BusinessPartnerInternalCode"]?.ToString();
                        if (reader["BusinessPartnerName"] != DBNull.Value)
                            businessPartner.Name = reader["BusinessPartnerName"].ToString();

                        if (reader["PIB"] != DBNull.Value)
                            businessPartner.PIB = reader["PIB"].ToString();
                        if (reader["PIO"] != DBNull.Value)
                            businessPartner.PIO = reader["PIO"].ToString();
                        if (reader["PDV"] != DBNull.Value)
                            businessPartner.PDV = reader["PDV"].ToString();
                        if (reader["IdentificationNumber"] != DBNull.Value)
                            businessPartner.IdentificationNumber = reader["IdentificationNumber"].ToString();
                        if (reader["Rebate"] != DBNull.Value)
                            businessPartner.Rebate = decimal.Parse(reader["Rebate"].ToString());
                        if (reader["DueDate"] != DBNull.Value)
                            businessPartner.DueDate = Int32.Parse(reader["DueDate"].ToString());
                        if (reader["WebSite"] != DBNull.Value)
                            businessPartner.WebSite = reader["WebSite"].ToString();
                        if (reader["ContactPerson"] != DBNull.Value)
                            businessPartner.ContactPerson = reader["ContactPerson"].ToString();
                        if (reader["IsInPDV"] != DBNull.Value)
                            businessPartner.IsInPDV = bool.Parse(reader["IsInPDV"].ToString());
                        if (reader["JBKJS"] != DBNull.Value)
                            businessPartner.JBKJS = reader["JBKJS"].ToString();
                        if (reader["NameGer"] != DBNull.Value)
                            businessPartner.NameGer = reader["NameGer"].ToString();
                        if (reader["IsInPDVGer"] != DBNull.Value)
                            businessPartner.IsInPDVGer = bool.Parse(reader["IsInPDVGer"].ToString());

                        if (reader["TaxAdministrationId"] != DBNull.Value)
                        {
                            businessPartner.TaxAdministration = new TaxAdministration();
                            businessPartner.TaxAdministrationId = Int32.Parse(reader["TaxAdministrationId"].ToString());
                            businessPartner.TaxAdministration.Id = Int32.Parse(reader["TaxAdministrationId"].ToString());
                            businessPartner.TaxAdministration.Identifier = Guid.Parse(reader["TaxAdministrationIdentifier"].ToString());
                            businessPartner.TaxAdministration.Code = reader["TaxAdministrationCode"].ToString();
                            businessPartner.TaxAdministration.Name = reader["TaxAdministrationName"].ToString();
                        }

                        if (reader["IBAN"] != DBNull.Value)
                            businessPartner.IBAN = reader["IBAN"].ToString();
                        if (reader["BetriebsNumber"] != DBNull.Value)
                            businessPartner.BetriebsNumber = reader["BetriebsNumber"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartner.Country = new Country();
                            businessPartner.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartner.Country.Code = reader["CountryCode"].ToString();
                            businessPartner.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["SectorId"] != DBNull.Value)
                        {
                            businessPartner.Sector = new Sector();
                            businessPartner.SectorId = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Id = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Identifier = Guid.Parse(reader["SectorIdentifier"].ToString());
                            businessPartner.Sector.Code = reader["SectorCode"].ToString();
                            businessPartner.Sector.Name = reader["SectorName"].ToString();
                        }

                        if (reader["AgencyId"] != DBNull.Value)
                        {
                            businessPartner.Agency = new Agency();
                            businessPartner.AgencyId = Int32.Parse(reader["AgencyId"].ToString());
                            businessPartner.Agency.Id = Int32.Parse(reader["AgencyId"].ToString());
                            businessPartner.Agency.Identifier = Guid.Parse(reader["AgencyIdentifier"].ToString());
                            businessPartner.Agency.Code = reader["AgencyCode"].ToString();
                            businessPartner.Agency.Name = reader["AgencyName"].ToString();
                        }

                        if (reader["TaxNr"] != DBNull.Value)
                            businessPartner.TaxNr = reader["TaxNr"].ToString();
                        if (reader["CommercialNr"] != DBNull.Value)
                            businessPartner.CommercialNr = reader["CommercialNr"].ToString();
                        if (reader["ContactPersonGer"] != DBNull.Value)
                            businessPartner.ContactPersonGer = reader["ContactPersonGer"].ToString();

                        businessPartner.Active = bool.Parse(reader["Active"].ToString());
                        businessPartner.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartner.CreatedBy = new User();
                            businessPartner.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartner.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartner.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartner.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartner.Company = new Company();
                            businessPartner.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartner.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartner.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartners.Add(businessPartner);
                    }
                }
            }
            return BusinessPartners;


            //List<BusinessPartner> businessPartners = context.BusinessPartners
            //    .Include(x => x.Country)
            //    .Include(x => x.Sector)
            //    .Include(x => x.Agency)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return businessPartners;
        }

        public BusinessPartner GetBusinessPartner(int id)
        {
            BusinessPartner businessPartner = new BusinessPartner();

            string queryString =
                "SELECT BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerInternalCode, BusinessPartnerName, " +
                "PIB, PIO, PDV, IdentificationNumber, Rebate, DueDate, WebSite, ContactPerson, IsInPDV, JBKJS, NameGer, IsInPDVGer, " +
                "TaxAdministrationId, TaxAdministrationIdentifier, TaxAdministrationCode, TaxAdministrationName, " +
                "IBAN, BetriebsNumber, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "SectorId, SectorIdentifier, SectorCode, SectorName, " +
                "AgencyId, AgencyIdentifier, AgencyCode, AgencyName, " +
                "TaxNr, CommercialNr, ContactPersonGer, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartners " +
                "WHERE BusinessPartnerId = @BusinessPartnerId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@BusinessPartnerId", id));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        businessPartner = new BusinessPartner();
                        businessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                        businessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                        businessPartner.Code = reader["BusinessPartnerCode"].ToString();
                        businessPartner.InternalCode = reader["BusinessPartnerInternalCode"]?.ToString();
                        if (reader["BusinessPartnerName"] != DBNull.Value)
                            businessPartner.Name = reader["BusinessPartnerName"].ToString();

                        if (reader["PIB"] != DBNull.Value)
                            businessPartner.PIB = reader["PIB"].ToString();
                        if (reader["PIO"] != DBNull.Value)
                            businessPartner.PIO = reader["PIO"].ToString();
                        if (reader["PDV"] != DBNull.Value)
                            businessPartner.PDV = reader["PDV"].ToString();
                        if (reader["IdentificationNumber"] != DBNull.Value)
                            businessPartner.IdentificationNumber = reader["IdentificationNumber"].ToString();
                        if (reader["Rebate"] != DBNull.Value)
                            businessPartner.Rebate = decimal.Parse(reader["Rebate"].ToString());
                        if (reader["DueDate"] != DBNull.Value)
                            businessPartner.DueDate = Int32.Parse(reader["DueDate"].ToString());
                        if (reader["WebSite"] != DBNull.Value)
                            businessPartner.WebSite = reader["WebSite"].ToString();
                        if (reader["ContactPerson"] != DBNull.Value)
                            businessPartner.ContactPerson = reader["ContactPerson"].ToString();
                        if (reader["IsInPDV"] != DBNull.Value)
                            businessPartner.IsInPDV = bool.Parse(reader["IsInPDV"].ToString());
                        if (reader["JBKJS"] != DBNull.Value)
                            businessPartner.JBKJS = reader["JBKJS"].ToString();
                        if (reader["NameGer"] != DBNull.Value)
                            businessPartner.NameGer = reader["NameGer"].ToString();
                        if (reader["IsInPDVGer"] != DBNull.Value)
                            businessPartner.IsInPDVGer = bool.Parse(reader["IsInPDVGer"].ToString());

                        if (reader["TaxAdministrationId"] != DBNull.Value)
                        {
                            businessPartner.TaxAdministration = new TaxAdministration();
                            businessPartner.TaxAdministrationId = Int32.Parse(reader["TaxAdministrationId"].ToString());
                            businessPartner.TaxAdministration.Id = Int32.Parse(reader["TaxAdministrationId"].ToString());
                            businessPartner.TaxAdministration.Identifier = Guid.Parse(reader["TaxAdministrationIdentifier"].ToString());
                            businessPartner.TaxAdministration.Code = reader["TaxAdministrationCode"].ToString();
                            businessPartner.TaxAdministration.Name = reader["TaxAdministrationName"].ToString();
                        }

                        if (reader["IBAN"] != DBNull.Value)
                            businessPartner.IBAN = reader["IBAN"].ToString();
                        if (reader["BetriebsNumber"] != DBNull.Value)
                            businessPartner.BetriebsNumber = reader["BetriebsNumber"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartner.Country = new Country();
                            businessPartner.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartner.Country.Code = reader["CountryCode"].ToString();
                            businessPartner.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["SectorId"] != DBNull.Value)
                        {
                            businessPartner.Sector = new Sector();
                            businessPartner.SectorId = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Id = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Identifier = Guid.Parse(reader["SectorIdentifier"].ToString());
                            businessPartner.Sector.Code = reader["SectorCode"].ToString();
                            businessPartner.Sector.Name = reader["SectorName"].ToString();
                        }

                        if (reader["AgencyId"] != DBNull.Value)
                        {
                            businessPartner.Agency = new Agency();
                            businessPartner.AgencyId = Int32.Parse(reader["AgencyId"].ToString());
                            businessPartner.Agency.Id = Int32.Parse(reader["AgencyId"].ToString());
                            businessPartner.Agency.Identifier = Guid.Parse(reader["AgencyIdentifier"].ToString());
                            businessPartner.Agency.Code = reader["AgencyCode"].ToString();
                            businessPartner.Agency.Name = reader["AgencyName"].ToString();
                        }

                        if (reader["TaxNr"] != DBNull.Value)
                            businessPartner.TaxNr = reader["TaxNr"].ToString();
                        if (reader["CommercialNr"] != DBNull.Value)
                            businessPartner.CommercialNr = reader["CommercialNr"].ToString();
                        if (reader["ContactPersonGer"] != DBNull.Value)
                            businessPartner.ContactPersonGer = reader["ContactPersonGer"].ToString();

                        businessPartner.Active = bool.Parse(reader["Active"].ToString());
                        businessPartner.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartner.CreatedBy = new User();
                            businessPartner.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartner.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartner.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartner.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartner.Company = new Company();
                            businessPartner.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartner.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartner.Company.Name = reader["CompanyName"].ToString();
                        }

                    }
                }
            }
            return businessPartner;

            //return context.BusinessPartners
            //    .Include(x => x.Country)
            //    .Include(x => x.Sector)
            //    .Include(x => x.Agency)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //.FirstOrDefault(x => x.Id == id && x.Active == true);
        }


        private string GetNewCodeValue(int companyId)
        {
            int count = context.BusinessPartners
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartner))
                    .Select(x => x.Entity as BusinessPartner))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "BP/00001";
            else
            {
                string activeCode = context.BusinessPartners
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartner))
                        .Select(x => x.Entity as BusinessPartner))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("BP/", ""));
                    return "BP/" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }


        public BusinessPartner Create(BusinessPartner businessPartner)
        {
            if (context.BusinessPartners.Where(x => x.Identifier != null && x.Identifier == businessPartner.Identifier).Count() == 0)
            {
                if (context.BusinessPartners.Where(x => x.InternalCode == businessPartner.InternalCode).Count() > 0)
                    throw new Exception("Firma sa datom šifrom već postoji u bazi!");

                businessPartner.Id = 0;

                businessPartner.Active = true;

                businessPartner.Code = GetNewCodeValue(businessPartner.CompanyId ?? 0);

                businessPartner.CreatedAt = DateTime.Now;
                businessPartner.UpdatedAt = DateTime.Now;

                context.BusinessPartners.Add(businessPartner);
                return businessPartner;
            }
            else
            {
                // Load item that will be updated
                BusinessPartner dbEntry = context.BusinessPartners
                    .FirstOrDefault(x => x.Identifier == businessPartner.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CountryId = businessPartner.CountryId ?? null;
                    dbEntry.SectorId = businessPartner.SectorId ?? null;
                    dbEntry.AgencyId = businessPartner.AgencyId ?? null;
                    dbEntry.CompanyId = businessPartner.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartner.CreatedById ?? null;
                    dbEntry.TaxAdministrationId = businessPartner.TaxAdministrationId ?? null;

                    // Set properties
                    dbEntry.Code = businessPartner.Code;
                    dbEntry.InternalCode = businessPartner.InternalCode;
                    dbEntry.Name = businessPartner.Name;

                    dbEntry.PIB = businessPartner.PIB;
                    dbEntry.PIO = businessPartner.PIO;
                    dbEntry.PDV = businessPartner.PDV;
                    dbEntry.IdentificationNumber = businessPartner.IdentificationNumber;

                    // Set GER properties
                    dbEntry.NameGer = businessPartner.NameGer;

                    dbEntry.IsInPDVGer = businessPartner.IsInPDVGer;
                    dbEntry.IBAN = businessPartner.IBAN;
                    dbEntry.BetriebsNumber = businessPartner.BetriebsNumber;

                    dbEntry.TaxNr = businessPartner.TaxNr;
                    dbEntry.CommercialNr = businessPartner.CommercialNr;
                    dbEntry.ContactPersonGer = businessPartner.ContactPersonGer;

                    dbEntry.Rebate = businessPartner.Rebate;
                    dbEntry.DueDate = businessPartner.DueDate;

                    dbEntry.WebSite = businessPartner.WebSite;
                    dbEntry.ContactPerson = businessPartner.ContactPerson;

                    dbEntry.IsInPDV = businessPartner.IsInPDV;

                    dbEntry.JBKJS = businessPartner.JBKJS;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartner Delete(Guid identifier)
        {
            // Load BusinessPartner that will be deleted
            BusinessPartner dbEntry = context.BusinessPartners
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
