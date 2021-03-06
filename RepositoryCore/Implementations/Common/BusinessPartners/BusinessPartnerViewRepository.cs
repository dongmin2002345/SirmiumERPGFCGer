﻿using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using DomainCore.Common.Prices;
using DomainCore.Common.Sectors;
using DomainCore.Common.TaxAdministrations;
using DomainCore.Vats;
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
                "PIB, PIO, IdentificationNumber, DueDate, WebSite, ContactPerson, IsInPDV, JBKJS, NameGer, IsInPDVGer, " +

                "CountrySrbId, CountrySrbIdentifier, CountrySrbCode, CountrySrbName, " +
                "CitySrbId, CitySrbIdentifier, CitySrbZipCode, CitySrbName, " +
                "Address, " +
                "TaxAdministrationId, TaxAdministrationIdentifier, TaxAdministrationCode, TaxAdministrationName, " +
                "IBAN, BetriebsNumber, Customer, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "AddressGer, " +
                "SectorId, SectorIdentifier, SectorCode, SectorName, " +
                "AgencyId, AgencyIdentifier, AgencyCode, AgencyName, " +
                "VatId, VatIdentifier, VatCode, VatDescription, VatAmount, " +
                "DiscountId, DiscountIdentifier, DiscountCode, DiscountName, DiscountAmount, " +
                "TaxNr, CommercialNr, ContactPersonGer, VatDeductionFrom, VatDeductionTo, PdvType, Path, " +
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
                        businessPartner.Code = reader["BusinessPartnerCode"]?.ToString();
                        businessPartner.InternalCode = reader["BusinessPartnerInternalCode"]?.ToString();
                        if (reader["BusinessPartnerName"] != DBNull.Value)
                            businessPartner.Name = reader["BusinessPartnerName"].ToString();

                        if (reader["PIB"] != DBNull.Value)
                            businessPartner.PIB = reader["PIB"].ToString();
                        if (reader["PIO"] != DBNull.Value)
                            businessPartner.PIO = reader["PIO"].ToString();
                        if (reader["IdentificationNumber"] != DBNull.Value)
                            businessPartner.IdentificationNumber = reader["IdentificationNumber"].ToString();
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

                        if (reader["CountrySrbId"] != DBNull.Value)
                        {
                            businessPartner.CountrySrb = new Country();
                            businessPartner.CountrySrbId = Int32.Parse(reader["CountrySrbId"].ToString());
                            businessPartner.CountrySrb.Id = Int32.Parse(reader["CountrySrbId"].ToString());
                            businessPartner.CountrySrb.Identifier = Guid.Parse(reader["CountrySrbIdentifier"].ToString());
                            businessPartner.CountrySrb.Mark = reader["CountrySrbCode"].ToString();
                            businessPartner.CountrySrb.Name = reader["CountrySrbName"].ToString();
                        }

                        if (reader["CitySrbId"] != DBNull.Value)
                        {
                            businessPartner.CitySrb = new City();
                            businessPartner.CitySrbId = Int32.Parse(reader["CitySrbId"].ToString());
                            businessPartner.CitySrb.Id = Int32.Parse(reader["CitySrbId"].ToString());
                            businessPartner.CitySrb.Identifier = Guid.Parse(reader["CitySrbIdentifier"].ToString());
                            businessPartner.CitySrb.ZipCode = reader["CitySrbZipCode"].ToString();
                            businessPartner.CitySrb.Name = reader["CitySrbName"].ToString();
                        }

                        if (reader["Address"] != DBNull.Value)
                            businessPartner.Address = reader["Address"].ToString();

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
                        if (reader["Customer"] != DBNull.Value)
                            businessPartner.Customer = reader["Customer"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartner.Country = new Country();
                            businessPartner.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartner.Country.Mark = reader["CountryCode"].ToString();
                            businessPartner.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            businessPartner.City = new City();
                            businessPartner.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartner.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartner.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartner.City.ZipCode = reader["CityZipCode"].ToString();
                            businessPartner.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["AddressGer"] != DBNull.Value)
                            businessPartner.AddressGer = reader["AddressGer"].ToString();

                        if (reader["SectorId"] != DBNull.Value)
                        {
                            businessPartner.Sector = new Sector();
                            businessPartner.SectorId = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Id = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Identifier = Guid.Parse(reader["SectorIdentifier"].ToString());
                            businessPartner.Sector.SecondCode = reader["SectorCode"].ToString();
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

                        if (reader["VatId"] != DBNull.Value)
                        {
                            businessPartner.Vat = new Vat();
                            businessPartner.VatId = Int32.Parse(reader["VatId"].ToString());
                            businessPartner.Vat.Id = Int32.Parse(reader["VatId"].ToString());
                            businessPartner.Vat.Identifier = Guid.Parse(reader["VatIdentifier"].ToString());
                            businessPartner.Vat.Code = reader["VatCode"].ToString();
                            businessPartner.Vat.Description = reader["VatDescription"].ToString();
                            businessPartner.Vat.Amount = decimal.Parse(reader["VatAmount"].ToString());
                        }

                        if (reader["DiscountId"] != DBNull.Value)
                        {
                            businessPartner.Discount = new Discount();
                            businessPartner.DiscountId = Int32.Parse(reader["DiscountId"].ToString());
                            businessPartner.Discount.Id = Int32.Parse(reader["DiscountId"].ToString());
                            businessPartner.Discount.Identifier = Guid.Parse(reader["DiscountIdentifier"].ToString());
                            businessPartner.Discount.Code = reader["DiscountCode"].ToString();
                            businessPartner.Discount.Name = reader["DiscountName"].ToString();
                            businessPartner.Discount.Amount = decimal.Parse(reader["DiscountAmount"].ToString());
                        }

                        if (reader["TaxNr"] != DBNull.Value)
                            businessPartner.TaxNr = reader["TaxNr"].ToString();
                        if (reader["CommercialNr"] != DBNull.Value)
                            businessPartner.CommercialNr = reader["CommercialNr"].ToString();
                        if (reader["ContactPersonGer"] != DBNull.Value)
                            businessPartner.ContactPersonGer = reader["ContactPersonGer"].ToString();
                        if (reader["VatDeductionFrom"] != DBNull.Value)
                            businessPartner.VatDeductionFrom = DateTime.Parse(reader["VatDeductionFrom"].ToString());
                        if (reader["VatDeductionTo"] != DBNull.Value)
                            businessPartner.VatDeductionTo = DateTime.Parse(reader["VatDeductionTo"].ToString());

                        if (reader["PdvType"] != DBNull.Value)
                            businessPartner.PdvType = Int32.Parse(reader["PdvType"].ToString());

                        if (reader["Path"] != DBNull.Value)
                            businessPartner.Path = reader["Path"].ToString();

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
                "PIB, PIO, IdentificationNumber, DueDate, WebSite, ContactPerson, IsInPDV, JBKJS, NameGer, IsInPDVGer, " +

                "CountrySrbId, CountrySrbIdentifier, CountrySrbCode, CountrySrbName, " +
                "CitySrbId, CitySrbIdentifier, CitySrbZipCode, CitySrbName, " +
                "Address, " +

                "TaxAdministrationId, TaxAdministrationIdentifier, TaxAdministrationCode, TaxAdministrationName, " +
                "IBAN, BetriebsNumber, Customer, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "AddressGer, " +
                "SectorId, SectorIdentifier, SectorCode, SectorName, " +
                "AgencyId, AgencyIdentifier, AgencyCode, AgencyName, " +
                "VatId, VatIdentifier, VatCode, VatDescription, VatAmount, " +
                "DiscountId, DiscountIdentifier, DiscountCode, DiscountName, DiscountAmount, " +
                "TaxNr, CommercialNr, ContactPersonGer, VatDeductionFrom, VatDeductionTo, PdvType, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartners " +
                "WHERE CompanyId = @CompanyId " +
                "AND CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));";

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
                        if (reader["IdentificationNumber"] != DBNull.Value)
                            businessPartner.IdentificationNumber = reader["IdentificationNumber"].ToString();
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

                        if (reader["CountrySrbId"] != DBNull.Value)
                        {
                            businessPartner.CountrySrb = new Country();
                            businessPartner.CountrySrbId = Int32.Parse(reader["CountrySrbId"].ToString());
                            businessPartner.CountrySrb.Id = Int32.Parse(reader["CountrySrbId"].ToString());
                            businessPartner.CountrySrb.Identifier = Guid.Parse(reader["CountrySrbIdentifier"].ToString());
                            businessPartner.CountrySrb.Mark = reader["CountrySrbCode"].ToString();
                            businessPartner.CountrySrb.Name = reader["CountrySrbName"].ToString();
                        }

                        if (reader["CitySrbId"] != DBNull.Value)
                        {
                            businessPartner.CitySrb = new City();
                            businessPartner.CitySrbId = Int32.Parse(reader["CitySrbId"].ToString());
                            businessPartner.CitySrb.Id = Int32.Parse(reader["CitySrbId"].ToString());
                            businessPartner.CitySrb.Identifier = Guid.Parse(reader["CitySrbIdentifier"].ToString());
                            businessPartner.CitySrb.ZipCode = reader["CitySrbZipCode"].ToString();
                            businessPartner.CitySrb.Name = reader["CitySrbName"].ToString();
                        }

                        if (reader["Address"] != DBNull.Value)
                            businessPartner.Address = reader["Address"].ToString();

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
                        if (reader["Customer"] != DBNull.Value)
                            businessPartner.Customer = reader["Customer"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartner.Country = new Country();
                            businessPartner.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartner.Country.Mark = reader["CountryCode"].ToString();
                            businessPartner.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            businessPartner.City = new City();
                            businessPartner.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartner.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartner.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartner.City.ZipCode = reader["CityZipCode"].ToString();
                            businessPartner.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["AddressGer"] != DBNull.Value)
                            businessPartner.AddressGer = reader["AddressGer"].ToString();

                        if (reader["SectorId"] != DBNull.Value)
                        {
                            businessPartner.Sector = new Sector();
                            businessPartner.SectorId = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Id = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Identifier = Guid.Parse(reader["SectorIdentifier"].ToString());
                            businessPartner.Sector.SecondCode = reader["SectorCode"].ToString();
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

                        if (reader["VatId"] != DBNull.Value)
                        {
                            businessPartner.Vat = new Vat();
                            businessPartner.VatId = Int32.Parse(reader["VatId"].ToString());
                            businessPartner.Vat.Id = Int32.Parse(reader["VatId"].ToString());
                            businessPartner.Vat.Identifier = Guid.Parse(reader["VatIdentifier"].ToString());
                            businessPartner.Vat.Code = reader["VatCode"].ToString();
                            businessPartner.Vat.Description = reader["VatDescription"].ToString();
                            businessPartner.Vat.Amount = decimal.Parse(reader["VatAmount"].ToString());

                        }

                        if (reader["DiscountId"] != DBNull.Value)
                        {
                            businessPartner.Discount = new Discount();
                            businessPartner.DiscountId = Int32.Parse(reader["DiscountId"].ToString());
                            businessPartner.Discount.Id = Int32.Parse(reader["DiscountId"].ToString());
                            businessPartner.Discount.Identifier = Guid.Parse(reader["DiscountIdentifier"].ToString());
                            businessPartner.Discount.Code = reader["DiscountCode"].ToString();
                            businessPartner.Discount.Name = reader["DiscountName"].ToString();
                            businessPartner.Discount.Amount = decimal.Parse(reader["DiscountAmount"].ToString());
                        }

                        if (reader["TaxNr"] != DBNull.Value)
                            businessPartner.TaxNr = reader["TaxNr"].ToString();
                        if (reader["CommercialNr"] != DBNull.Value)
                            businessPartner.CommercialNr = reader["CommercialNr"].ToString();
                        if (reader["ContactPersonGer"] != DBNull.Value)
                            businessPartner.ContactPersonGer = reader["ContactPersonGer"].ToString();
                        if (reader["VatDeductionFrom"] != DBNull.Value)
                            businessPartner.VatDeductionFrom = DateTime.Parse(reader["VatDeductionFrom"].ToString());
                        if (reader["VatDeductionTo"] != DBNull.Value)
                            businessPartner.VatDeductionTo = DateTime.Parse(reader["VatDeductionTo"].ToString());

                        if (reader["PdvType"] != DBNull.Value)
                            businessPartner.PdvType = Int32.Parse(reader["PdvType"].ToString());
                        if (reader["Path"] != DBNull.Value)
                            businessPartner.Path = reader["Path"].ToString();
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
                "PIB, PIO, IdentificationNumber, DueDate, WebSite, ContactPerson, IsInPDV, JBKJS, NameGer, IsInPDVGer, " +

                "CountrySrbId, CountrySrbIdentifier, CountrySrbCode, CountrySrbName, " +
                "CitySrbId, CitySrbIdentifier, CitySrbZipCode, CitySrbName, " +
                "Address, " +

                "TaxAdministrationId, TaxAdministrationIdentifier, TaxAdministrationCode, TaxAdministrationName, " +
                "IBAN, BetriebsNumber, Customer, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "AddressGer, " +
                "SectorId, SectorIdentifier, SectorCode, SectorName, " +
                "AgencyId, AgencyIdentifier, AgencyCode, AgencyName, " +
                "VatId, VatIdentifier, VatCode, VatDescription, VatAmount, " +
                "DiscountId, DiscountIdentifier, DiscountCode, DiscountName, DiscountAmount, " +
                "TaxNr, CommercialNr, ContactPersonGer, VatDeductionFrom, VatDeductionTo, PdvType, Path, " +
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
                        if (reader["IdentificationNumber"] != DBNull.Value)
                            businessPartner.IdentificationNumber = reader["IdentificationNumber"].ToString();
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

                        if (reader["CountrySrbId"] != DBNull.Value)
                        {
                            businessPartner.CountrySrb = new Country();
                            businessPartner.CountrySrbId = Int32.Parse(reader["CountrySrbId"].ToString());
                            businessPartner.CountrySrb.Id = Int32.Parse(reader["CountrySrbId"].ToString());
                            businessPartner.CountrySrb.Identifier = Guid.Parse(reader["CountrySrbIdentifier"].ToString());
                            businessPartner.CountrySrb.Mark = reader["CountrySrbCode"].ToString();
                            businessPartner.CountrySrb.Name = reader["CountrySrbName"].ToString();
                        }

                        if (reader["CitySrbId"] != DBNull.Value)
                        {
                            businessPartner.CitySrb = new City();
                            businessPartner.CitySrbId = Int32.Parse(reader["CitySrbId"].ToString());
                            businessPartner.CitySrb.Id = Int32.Parse(reader["CitySrbId"].ToString());
                            businessPartner.CitySrb.Identifier = Guid.Parse(reader["CitySrbIdentifier"].ToString());
                            businessPartner.CitySrb.ZipCode = reader["CitySrbZipCode"].ToString();
                            businessPartner.CitySrb.Name = reader["CitySrbName"].ToString();
                        }

                        if (reader["Address"] != DBNull.Value)
                            businessPartner.Address = reader["Address"].ToString();

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
                        if (reader["Customer"] != DBNull.Value)
                            businessPartner.Customer = reader["Customer"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartner.Country = new Country();
                            businessPartner.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartner.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartner.Country.Mark = reader["CountryCode"].ToString();
                            businessPartner.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            businessPartner.City = new City();
                            businessPartner.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartner.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartner.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartner.City.ZipCode = reader["CityZipCode"].ToString();
                            businessPartner.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["AddressGer"] != DBNull.Value)
                            businessPartner.AddressGer = reader["AddressGer"].ToString();

                        if (reader["SectorId"] != DBNull.Value)
                        {
                            businessPartner.Sector = new Sector();
                            businessPartner.SectorId = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Id = Int32.Parse(reader["SectorId"].ToString());
                            businessPartner.Sector.Identifier = Guid.Parse(reader["SectorIdentifier"].ToString());
                            businessPartner.Sector.SecondCode = reader["SectorCode"].ToString();
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

                        if (reader["VatId"] != DBNull.Value)
                        {
                            businessPartner.Vat = new Vat();
                            businessPartner.VatId = Int32.Parse(reader["VatId"].ToString());
                            businessPartner.Vat.Id = Int32.Parse(reader["VatId"].ToString());
                            businessPartner.Vat.Identifier = Guid.Parse(reader["VatIdentifier"].ToString());
                            businessPartner.Vat.Code = reader["VatCode"].ToString();
                            businessPartner.Vat.Description = reader["VatDescription"].ToString();
                            businessPartner.Vat.Amount = decimal.Parse(reader["VatAmount"].ToString());
                        }

                        if (reader["DiscountId"] != DBNull.Value)
                        {
                            businessPartner.Discount = new Discount();
                            businessPartner.DiscountId = Int32.Parse(reader["DiscountId"].ToString());
                            businessPartner.Discount.Id = Int32.Parse(reader["DiscountId"].ToString());
                            businessPartner.Discount.Identifier = Guid.Parse(reader["DiscountIdentifier"].ToString());
                            businessPartner.Discount.Code = reader["DiscountCode"].ToString();
                            businessPartner.Discount.Name = reader["DiscountName"].ToString();
                            businessPartner.Discount.Amount = decimal.Parse(reader["DiscountAmount"].ToString());
                        }

                        if (reader["TaxNr"] != DBNull.Value)
                            businessPartner.TaxNr = reader["TaxNr"].ToString();
                        if (reader["CommercialNr"] != DBNull.Value)
                            businessPartner.CommercialNr = reader["CommercialNr"].ToString();
                        if (reader["ContactPersonGer"] != DBNull.Value)
                            businessPartner.ContactPersonGer = reader["ContactPersonGer"].ToString();
                        if (reader["VatDeductionFrom"] != DBNull.Value)
                            businessPartner.VatDeductionFrom = DateTime.Parse(reader["VatDeductionFrom"].ToString());
                        if (reader["VatDeductionTo"] != DBNull.Value)
                            businessPartner.VatDeductionTo = DateTime.Parse(reader["VatDeductionTo"].ToString());

                        if (reader["PdvType"] != DBNull.Value)
                            businessPartner.PdvType = Int32.Parse(reader["PdvType"].ToString());
                        if (reader["Path"] != DBNull.Value)
                            businessPartner.Path = reader["Path"].ToString();
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
                    throw new Exception("Firma sa datom šifrom već postoji u bazi! / Eine Firma mit einem bestimmten Code ist bereits in der Datenbank vorhanden");

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
                    dbEntry.CountrySrbId = businessPartner.CountrySrbId ?? null;
                    dbEntry.CitySrbId = businessPartner.CitySrbId ?? null;
                    dbEntry.CityId = businessPartner.CityId ?? null;
                    dbEntry.CountryId = businessPartner.CountryId ?? null;
                    dbEntry.SectorId = businessPartner.SectorId ?? null;
                    dbEntry.AgencyId = businessPartner.AgencyId ?? null;
                    dbEntry.CompanyId = businessPartner.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartner.CreatedById ?? null;
                    dbEntry.TaxAdministrationId = businessPartner.TaxAdministrationId ?? null;
                    dbEntry.VatId = businessPartner.VatId ?? null;
                    dbEntry.DiscountId = businessPartner.DiscountId ?? null;

                    // Set properties
                    dbEntry.Code = businessPartner.Code;
                    dbEntry.InternalCode = businessPartner.InternalCode;
                    dbEntry.Name = businessPartner.Name;

                    dbEntry.PIB = businessPartner.PIB;
                    dbEntry.PIO = businessPartner.PIO;
                    dbEntry.IdentificationNumber = businessPartner.IdentificationNumber;
                    dbEntry.Address = businessPartner.Address;

                    // Set GER properties
                    dbEntry.NameGer = businessPartner.NameGer;
                    dbEntry.AddressGer = businessPartner.AddressGer;
                    dbEntry.IsInPDVGer = businessPartner.IsInPDVGer;
                    dbEntry.IBAN = businessPartner.IBAN;
                    dbEntry.BetriebsNumber = businessPartner.BetriebsNumber;
                    dbEntry.Customer = businessPartner.Customer;

                    dbEntry.TaxNr = businessPartner.TaxNr;
                    dbEntry.CommercialNr = businessPartner.CommercialNr;
                    dbEntry.ContactPersonGer = businessPartner.ContactPersonGer;

                    dbEntry.DueDate = businessPartner.DueDate;

                    dbEntry.WebSite = businessPartner.WebSite;
                    dbEntry.ContactPerson = businessPartner.ContactPerson;

                    dbEntry.IsInPDV = businessPartner.IsInPDV;

                    dbEntry.JBKJS = businessPartner.JBKJS;

                    dbEntry.VatDeductionFrom = businessPartner.VatDeductionFrom;
                    dbEntry.VatDeductionTo = businessPartner.VatDeductionTo;

                    dbEntry.PdvType = businessPartner.PdvType;
                    dbEntry.Path = businessPartner.Path;

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
