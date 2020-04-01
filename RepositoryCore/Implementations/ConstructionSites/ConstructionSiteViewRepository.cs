using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using DomainCore.Common.Shipments;
using DomainCore.ConstructionSites;
using DomainCore.Statuses;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.ConstructionSites;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.ConstructionSites
{
    public class ConstructionSiteViewRepository : IConstructionSiteRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public ConstructionSiteViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<ConstructionSite> GetConstructionSites(int companyId)
        {
            List<ConstructionSite> ConstructionSites = new List<ConstructionSite>();

            string queryString =
                "SELECT ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteInternalCode, ConstructionSiteName, " +
                "NamePartner, AddressPartner, " +
                "CityPartnerId, CityPartnerIdentifier, CityPartnerZipCode, CityPartnerName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
                "StatusId, StatusIdentifier, StatusCode, StatusName, " +
                "StatusDate, Address, MaxWorkers, ProContractDate, ContractStart, ContractExpiration, PaymentDate, Path, PaymentValue, " +
                "ShipmentId, ShipmentIdentifier, ShipmentCode, ShipmentNumber, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vConstructionSites " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ConstructionSite constructionSite;
                    while (reader.Read())
                    {
                        constructionSite = new ConstructionSite();
                        constructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                        constructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                        constructionSite.Code = reader["ConstructionSiteCode"]?.ToString();
                        constructionSite.InternalCode = reader["ConstructionSiteInternalCode"]?.ToString();
                        constructionSite.Name = reader["ConstructionSiteName"].ToString();
                        if (reader["NamePartner"] != DBNull.Value)
                            constructionSite.NamePartner = reader["NamePartner"].ToString();
                        if (reader["AddressPartner"] != DBNull.Value)
                            constructionSite.AddressPartner = reader["AddressPartner"].ToString();

                        if (reader["CityPartnerId"] != DBNull.Value)
                        {
                            constructionSite.CityPartner = new City();
                            constructionSite.CityPartnerId = Int32.Parse(reader["CityPartnerId"].ToString());
                            constructionSite.CityPartner.Id = Int32.Parse(reader["CityPartnerId"].ToString());
                            constructionSite.CityPartner.Identifier = Guid.Parse(reader["CityPartnerIdentifier"].ToString());
                            constructionSite.CityPartner.ZipCode = reader["CityPartnerZipCode"].ToString();
                            constructionSite.CityPartner.Name = reader["CityPartnerName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            constructionSite.City = new City();
                            constructionSite.CityId = Int32.Parse(reader["CityId"].ToString());
                            constructionSite.City.Id = Int32.Parse(reader["CityId"].ToString());
                            constructionSite.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            constructionSite.City.ZipCode = reader["CityZipCode"].ToString();
                            constructionSite.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            constructionSite.Country = new Country();
                            constructionSite.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            constructionSite.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            constructionSite.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            constructionSite.Country.Mark = reader["CountryCode"].ToString();
                            constructionSite.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            constructionSite.BusinessPartner = new BusinessPartner();
                            constructionSite.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            constructionSite.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            constructionSite.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            constructionSite.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            constructionSite.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                            constructionSite.BusinessPartner.InternalCode = reader["BusinessPartnerInternalCode"].ToString();
                            constructionSite.BusinessPartner.NameGer = reader["BusinessPartnerNameGer"].ToString();
                        }

                        if (reader["StatusId"] != DBNull.Value)
                        {
                            constructionSite.Status = new Status();
                            constructionSite.StatusId = Int32.Parse(reader["StatusId"].ToString());
                            constructionSite.Status.Id = Int32.Parse(reader["StatusId"].ToString());
                            constructionSite.Status.Identifier = Guid.Parse(reader["StatusIdentifier"].ToString());
                            constructionSite.Status.Code = reader["StatusCode"].ToString();
                            constructionSite.Status.Name = reader["StatusName"].ToString();
                        }
                        if (reader["StatusDate"] != DBNull.Value)
                            constructionSite.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());
                        if (reader["Address"] != DBNull.Value)
                            constructionSite.Address = reader["Address"].ToString();
                        if (reader["MaxWorkers"] != DBNull.Value)
                            constructionSite.MaxWorkers = Int32.Parse(reader["MaxWorkers"].ToString());
                        if (reader["ProContractDate"] != DBNull.Value)
                            constructionSite.ProContractDate = DateTime.Parse(reader["ProContractDate"].ToString());
                        if (reader["ContractStart"] != DBNull.Value)
                            constructionSite.ContractStart = DateTime.Parse(reader["ContractStart"].ToString());
                        if (reader["ContractExpiration"] != DBNull.Value)
                            constructionSite.ContractExpiration = DateTime.Parse(reader["ContractExpiration"].ToString());

                        if (reader["PaymentDate"] != DBNull.Value)
                            constructionSite.PaymentDate = DateTime.Parse(reader["PaymentDate"].ToString());
                        if (reader["Path"] != DBNull.Value)
                            constructionSite.Path = reader["Path"].ToString();
                        if (reader["PaymentValue"] != DBNull.Value)
                            constructionSite.PaymentValue = decimal.Parse(reader["PaymentValue"].ToString());

                        if (reader["ShipmentId"] != DBNull.Value)
                        {
                            constructionSite.Shipment = new Shipment();
                            constructionSite.ShipmentId = Int32.Parse(reader["ShipmentId"].ToString());
                            constructionSite.Shipment.Id = Int32.Parse(reader["ShipmentId"].ToString());
                            constructionSite.Shipment.Identifier = Guid.Parse(reader["ShipmentIdentifier"].ToString());
                            constructionSite.Shipment.Code = reader["ShipmentCode"].ToString();
                            constructionSite.Shipment.ShipmentNumber = reader["ShipmentNumber"].ToString();
                        }

                        constructionSite.Active = bool.Parse(reader["Active"].ToString());
                        constructionSite.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            constructionSite.CreatedBy = new User();
                            constructionSite.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSite.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSite.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSite.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            constructionSite.Company = new Company();
                            constructionSite.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSite.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSite.Company.Name = reader["CompanyName"].ToString();
                        }

                        ConstructionSites.Add(constructionSite);
                    }
                }
            }
            return ConstructionSites;

            //List<ConstructionSite> constructionSites = context.ConstructionSites
            //    .Include(x => x.City)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return constructionSites;
        }

        public ConstructionSite GetConstructionSite(int constructionSiteId)
        {
            ConstructionSite constructionSite = new ConstructionSite();

            string queryString =
                "SELECT ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteInternalCode, ConstructionSiteName, " +
                "NamePartner, AddressPartner, " +
                "CityPartnerId, CityPartnerIdentifier, CityPartnerZipCode, CityPartnerName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
                "StatusId, StatusIdentifier, StatusCode, StatusName, " +
                "StatusDate, Address, MaxWorkers, ProContractDate, ContractStart, ContractExpiration, PaymentDate, Path, PaymentValue, " +
                "ShipmentId, ShipmentIdentifier, ShipmentCode, ShipmentNumber, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vConstructionSites " +
                "WHERE ConstructionSiteId = @ConstructionSiteId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@ConstructionSiteId", constructionSiteId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        constructionSite = new ConstructionSite();
                        constructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                        constructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                        constructionSite.Code = reader["ConstructionSiteCode"]?.ToString();
                        constructionSite.InternalCode = reader["ConstructionSiteInternalCode"]?.ToString();
                        constructionSite.Name = reader["ConstructionSiteName"].ToString();

                        if (reader["NamePartner"] != DBNull.Value)
                            constructionSite.NamePartner = reader["NamePartner"].ToString();
                        if (reader["AddressPartner"] != DBNull.Value)
                            constructionSite.AddressPartner = reader["AddressPartner"].ToString();

                        if (reader["CityPartnerId"] != DBNull.Value)
                        {
                            constructionSite.CityPartner = new City();
                            constructionSite.CityPartnerId = Int32.Parse(reader["CityPartnerId"].ToString());
                            constructionSite.CityPartner.Id = Int32.Parse(reader["CityPartnerId"].ToString());
                            constructionSite.CityPartner.Identifier = Guid.Parse(reader["CityPartnerIdentifier"].ToString());
                            constructionSite.CityPartner.ZipCode = reader["CityPartnerZipCode"].ToString();
                            constructionSite.CityPartner.Name = reader["CityPartnerName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            constructionSite.City = new City();
                            constructionSite.CityId = Int32.Parse(reader["CityId"].ToString());
                            constructionSite.City.Id = Int32.Parse(reader["CityId"].ToString());
                            constructionSite.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            constructionSite.City.ZipCode = reader["CityZipCode"].ToString();
                            constructionSite.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            constructionSite.Country = new Country();
                            constructionSite.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            constructionSite.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            constructionSite.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            constructionSite.Country.Mark = reader["CountryCode"].ToString();
                            constructionSite.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            constructionSite.BusinessPartner = new BusinessPartner();
                            constructionSite.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            constructionSite.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            constructionSite.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            constructionSite.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            constructionSite.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                            constructionSite.BusinessPartner.InternalCode = reader["BusinessPartnerInternalCode"].ToString();
                            constructionSite.BusinessPartner.NameGer = reader["BusinessPartnerNameGer"].ToString();
                        }

                        if (reader["StatusId"] != DBNull.Value)
                        {
                            constructionSite.Status = new Status();
                            constructionSite.StatusId = Int32.Parse(reader["StatusId"].ToString());
                            constructionSite.Status.Id = Int32.Parse(reader["StatusId"].ToString());
                            constructionSite.Status.Identifier = Guid.Parse(reader["StatusIdentifier"].ToString());
                            constructionSite.Status.Code = reader["StatusCode"].ToString();
                            constructionSite.Status.Name = reader["StatusName"].ToString();
                        }
                        if (reader["StatusDate"] != DBNull.Value)
                            constructionSite.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());
                        if (reader["Address"] != DBNull.Value)
                            constructionSite.Address = reader["Address"].ToString();
                        if (reader["MaxWorkers"] != DBNull.Value)
                            constructionSite.MaxWorkers = Int32.Parse(reader["MaxWorkers"].ToString());
                        if (reader["ProContractDate"] != DBNull.Value)
                            constructionSite.ProContractDate = DateTime.Parse(reader["ProContractDate"].ToString());
                        if (reader["ContractStart"] != DBNull.Value)
                            constructionSite.ContractStart = DateTime.Parse(reader["ContractStart"].ToString());
                        if (reader["ContractExpiration"] != DBNull.Value)
                            constructionSite.ContractExpiration = DateTime.Parse(reader["ContractExpiration"].ToString());

                        if (reader["PaymentDate"] != DBNull.Value)
                            constructionSite.PaymentDate = DateTime.Parse(reader["PaymentDate"].ToString());
                        if (reader["Path"] != DBNull.Value)
                            constructionSite.Path = reader["Path"].ToString();
                        if (reader["PaymentValue"] != DBNull.Value)
                            constructionSite.PaymentValue = decimal.Parse(reader["PaymentValue"].ToString());

                        if (reader["ShipmentId"] != DBNull.Value)
                        {
                            constructionSite.Shipment = new Shipment();
                            constructionSite.ShipmentId = Int32.Parse(reader["ShipmentId"].ToString());
                            constructionSite.Shipment.Id = Int32.Parse(reader["ShipmentId"].ToString());
                            constructionSite.Shipment.Identifier = Guid.Parse(reader["ShipmentIdentifier"].ToString());
                            constructionSite.Shipment.Code = reader["ShipmentCode"].ToString();
                            constructionSite.Shipment.ShipmentNumber = reader["ShipmentNumber"].ToString();
                        }

                        constructionSite.Active = bool.Parse(reader["Active"].ToString());
                        constructionSite.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            constructionSite.CreatedBy = new User();
                            constructionSite.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSite.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSite.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSite.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            constructionSite.Company = new Company();
                            constructionSite.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSite.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSite.Company.Name = reader["CompanyName"].ToString();
                        }

                    }
                }
            }
            return constructionSite;

            //ConstructionSite constructionSite = context.ConstructionSites
            //    .Include(x => x.City)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .FirstOrDefault(x => x.Id == constructionSiteId);

            //return constructionSite;
        }

        public List<ConstructionSite> GetConstructionSitesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<ConstructionSite> ConstructionSites = new List<ConstructionSite>();

            string queryString =
                "SELECT ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteInternalCode, ConstructionSiteName, " +
                "NamePartner, AddressPartner, " +
                "CityPartnerId, CityPartnerIdentifier, CityPartnerZipCode, CityPartnerName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
                "StatusId, StatusIdentifier, StatusCode, StatusName, " +
                "StatusDate, Address, MaxWorkers, ProContractDate, ContractStart, ContractExpiration, PaymentDate, Path, PaymentValue, " +
                "ShipmentId, ShipmentIdentifier, ShipmentCode, ShipmentNumber, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vConstructionSites " +
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
                    ConstructionSite constructionSite;
                    while (reader.Read())
                    {
                        constructionSite = new ConstructionSite();
                        constructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                        constructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                        constructionSite.Code = reader["ConstructionSiteCode"]?.ToString();
                        constructionSite.InternalCode = reader["ConstructionSiteInternalCode"]?.ToString();
                        constructionSite.Name = reader["ConstructionSiteName"].ToString();

                        if (reader["NamePartner"] != DBNull.Value)
                            constructionSite.NamePartner = reader["NamePartner"].ToString();
                        if (reader["AddressPartner"] != DBNull.Value)
                            constructionSite.AddressPartner = reader["AddressPartner"].ToString();

                        if (reader["CityPartnerId"] != DBNull.Value)
                        {
                            constructionSite.CityPartner = new City();
                            constructionSite.CityPartnerId = Int32.Parse(reader["CityPartnerId"].ToString());
                            constructionSite.CityPartner.Id = Int32.Parse(reader["CityPartnerId"].ToString());
                            constructionSite.CityPartner.Identifier = Guid.Parse(reader["CityPartnerIdentifier"].ToString());
                            constructionSite.CityPartner.ZipCode = reader["CityPartnerZipCode"].ToString();
                            constructionSite.CityPartner.Name = reader["CityPartnerName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            constructionSite.City = new City();
                            constructionSite.CityId = Int32.Parse(reader["CityId"].ToString());
                            constructionSite.City.Id = Int32.Parse(reader["CityId"].ToString());
                            constructionSite.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            constructionSite.City.ZipCode = reader["CityZipCode"].ToString();
                            constructionSite.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            constructionSite.Country = new Country();
                            constructionSite.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            constructionSite.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            constructionSite.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            constructionSite.Country.Mark = reader["CountryCode"].ToString();
                            constructionSite.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            constructionSite.BusinessPartner = new BusinessPartner();
                            constructionSite.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            constructionSite.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            constructionSite.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            constructionSite.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            constructionSite.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                            constructionSite.BusinessPartner.InternalCode = reader["BusinessPartnerInternalCode"].ToString();
                            constructionSite.BusinessPartner.NameGer = reader["BusinessPartnerNameGer"].ToString();
                        }

                        if (reader["StatusId"] != DBNull.Value)
                        {
                            constructionSite.Status = new Status();
                            constructionSite.StatusId = Int32.Parse(reader["StatusId"].ToString());
                            constructionSite.Status.Id = Int32.Parse(reader["StatusId"].ToString());
                            constructionSite.Status.Identifier = Guid.Parse(reader["StatusIdentifier"].ToString());
                            constructionSite.Status.Code = reader["StatusCode"].ToString();
                            constructionSite.Status.Name = reader["StatusName"].ToString();
                        }
                        if (reader["StatusDate"] != DBNull.Value)
                            constructionSite.StatusDate = DateTime.Parse(reader["StatusDate"].ToString());
                        if (reader["Address"] != DBNull.Value)
                            constructionSite.Address = reader["Address"].ToString();
                        if (reader["MaxWorkers"] != DBNull.Value)
                            constructionSite.MaxWorkers = Int32.Parse(reader["MaxWorkers"].ToString());
                        if (reader["ProContractDate"] != DBNull.Value)
                            constructionSite.ProContractDate = DateTime.Parse(reader["ProContractDate"].ToString());
                        if (reader["ContractStart"] != DBNull.Value)
                            constructionSite.ContractStart = DateTime.Parse(reader["ContractStart"].ToString());
                        if (reader["ContractExpiration"] != DBNull.Value)
                            constructionSite.ContractExpiration = DateTime.Parse(reader["ContractExpiration"].ToString());

                        if (reader["PaymentDate"] != DBNull.Value)
                            constructionSite.PaymentDate = DateTime.Parse(reader["PaymentDate"].ToString());
                        if (reader["Path"] != DBNull.Value)
                            constructionSite.Path = reader["Path"].ToString();
                        if (reader["PaymentValue"] != DBNull.Value)
                            constructionSite.PaymentValue = decimal.Parse(reader["PaymentValue"].ToString());

                        if (reader["ShipmentId"] != DBNull.Value)
                        {
                            constructionSite.Shipment = new Shipment();
                            constructionSite.ShipmentId = Int32.Parse(reader["ShipmentId"].ToString());
                            constructionSite.Shipment.Id = Int32.Parse(reader["ShipmentId"].ToString());
                            constructionSite.Shipment.Identifier = Guid.Parse(reader["ShipmentIdentifier"].ToString());
                            constructionSite.Shipment.Code = reader["ShipmentCode"].ToString();
                            constructionSite.Shipment.ShipmentNumber = reader["ShipmentNumber"].ToString();
                        }

                        constructionSite.Active = bool.Parse(reader["Active"].ToString());
                        constructionSite.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            constructionSite.CreatedBy = new User();
                            constructionSite.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSite.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSite.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSite.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            constructionSite.Company = new Company();
                            constructionSite.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSite.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSite.Company.Name = reader["CompanyName"].ToString();
                        }

                        ConstructionSites.Add(constructionSite);
                    }
                }
            }
            return ConstructionSites;

            //List<ConstructionSite> constructionSites = context.ConstructionSites
            //    .Include(x => x.City)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return constructionSites;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.ConstructionSites
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(ConstructionSite))
                    .Select(x => x.Entity as ConstructionSite))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "GRA-00001";
            else
            {
                string activeCode = context.ConstructionSites
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(ConstructionSite))
                        .Select(x => x.Entity as ConstructionSite))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("GRA-", ""));
                    return "GRA-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public ConstructionSite Create(ConstructionSite constructionSite)
        {
            if (context.ConstructionSites.Where(x => x.Identifier != null && x.Identifier == constructionSite.Identifier).Count() == 0)
            {
                //if (context.ConstructionSites.Where(x => x.InternalCode == constructionSite.InternalCode).Count() > 0)
                    //throw new Exception("Gradilište sa datom šifrom već postoji u bazi! / Eine Baustelle mit diesem Code existiert bereits in der Datenbank!");

                constructionSite.Id = 0;

                constructionSite.Code = GetNewCodeValue(constructionSite.CompanyId ?? 0);
                constructionSite.Active = true;

                constructionSite.UpdatedAt = DateTime.Now;
                constructionSite.CreatedAt = DateTime.Now;

                context.ConstructionSites.Add(constructionSite);
                return constructionSite;
            }
            else
            {
                // Load constructionSite that will be updated
                ConstructionSite dbEntry = context.ConstructionSites
                .FirstOrDefault(x => x.Identifier == constructionSite.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CityId = constructionSite.CityId ?? null;
                    dbEntry.CityPartnerId = constructionSite.CityPartnerId ?? null;
                    dbEntry.CountryId = constructionSite.CountryId ?? null;
                    dbEntry.BusinessPartnerId = constructionSite.BusinessPartnerId ?? null;
                    dbEntry.StatusId = constructionSite.StatusId ?? null;
                    dbEntry.ShipmentId = constructionSite.ShipmentId ?? null;
                    dbEntry.CompanyId = constructionSite.CompanyId ?? null;
                    dbEntry.CreatedById = constructionSite.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = constructionSite.Code;
                    dbEntry.InternalCode = constructionSite.InternalCode;
                    dbEntry.Name = constructionSite.Name;
                    dbEntry.NamePartner = constructionSite.NamePartner;
                    dbEntry.AddressPartner = constructionSite.AddressPartner;
                    dbEntry.StatusDate = constructionSite.StatusDate;
                    dbEntry.Address = constructionSite.Address;
                    dbEntry.MaxWorkers = constructionSite.MaxWorkers;
                    dbEntry.ProContractDate = constructionSite.ProContractDate;
                    dbEntry.ContractStart = constructionSite.ContractStart;
                    dbEntry.ContractExpiration = constructionSite.ContractExpiration;

                    dbEntry.PaymentDate = constructionSite.PaymentDate;
                    dbEntry.Path = constructionSite.Path;
                    dbEntry.PaymentValue = constructionSite.PaymentValue;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public ConstructionSite Delete(Guid identifier)
        {
            // Load ConstructionSite that will be deleted
            ConstructionSite dbEntry = context.ConstructionSites
                .FirstOrDefault(x => x.Identifier == identifier);

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
