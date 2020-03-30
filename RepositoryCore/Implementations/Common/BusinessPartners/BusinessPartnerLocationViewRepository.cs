using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
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
    public class BusinessPartnerLocationViewRepository : IBusinessPartnerLocationRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public BusinessPartnerLocationViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<BusinessPartnerLocation> GetBusinessPartnerLocations(int companyId)
        {
            List<BusinessPartnerLocation> BusinessPartnerLocations = new List<BusinessPartnerLocation>();

            string queryString =
                "SELECT BusinessPartnerLocationId, BusinessPartnerLocationIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Address, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "ItemStatus, Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerLocations " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerLocation businessPartnerLocation;
                    while (reader.Read())
                    {
                        businessPartnerLocation = new BusinessPartnerLocation();
                        businessPartnerLocation.Id = Int32.Parse(reader["BusinessPartnerLocationId"].ToString());
                        businessPartnerLocation.Identifier = Guid.Parse(reader["BusinessPartnerLocationIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerLocation.BusinessPartner = new BusinessPartner();
                            businessPartnerLocation.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerLocation.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerLocation.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Address"] != DBNull.Value)
                            businessPartnerLocation.Address = reader["Address"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Country = new Country();
                            businessPartnerLocation.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerLocation.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerLocation.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            businessPartnerLocation.City = new City();
                            businessPartnerLocation.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.City.ZipCode = reader["CityZipCode"].ToString();
                            businessPartnerLocation.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Municipality = new Municipality();
                            businessPartnerLocation.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            businessPartnerLocation.Municipality.Code = reader["MunicipalityCode"].ToString();
                            businessPartnerLocation.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["RegionId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Region = new Region();
                            businessPartnerLocation.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            businessPartnerLocation.Region.Code = reader["RegionCode"].ToString();
                            businessPartnerLocation.Region.Name = reader["RegionName"].ToString();
                        }
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerLocation.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerLocation.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerLocation.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerLocation.CreatedBy = new User();
                            businessPartnerLocation.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerLocation.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Company = new Company();
                            businessPartnerLocation.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerLocation.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerLocation.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerLocations.Add(businessPartnerLocation);
                    }
                }
            }
            return BusinessPartnerLocations;
        }

        public List<BusinessPartnerLocation> GetBusinessPartnerLocationssByBusinessPartner(int businessPartnerId)
        {
            List<BusinessPartnerLocation> BusinessPartnerLocations = new List<BusinessPartnerLocation>();

            string queryString =
                "SELECT BusinessPartnerLocationId, BusinessPartnerLocationIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Address, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "ItemStatus, Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerLocations " +
                "WHERE BusinessPartnerId = @BusinessPartnerId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@BusinessPartnerId", businessPartnerId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerLocation businessPartnerLocation;
                    while (reader.Read())
                    {
                        businessPartnerLocation = new BusinessPartnerLocation();
                        businessPartnerLocation.Id = Int32.Parse(reader["BusinessPartnerLocationId"].ToString());
                        businessPartnerLocation.Identifier = Guid.Parse(reader["BusinessPartnerLocationIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerLocation.BusinessPartner = new BusinessPartner();
                            businessPartnerLocation.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerLocation.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerLocation.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Address"] != DBNull.Value)
                            businessPartnerLocation.Address = reader["Address"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Country = new Country();
                            businessPartnerLocation.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerLocation.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerLocation.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            businessPartnerLocation.City = new City();
                            businessPartnerLocation.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.City.ZipCode = reader["CityZipCode"].ToString();
                            businessPartnerLocation.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Municipality = new Municipality();
                            businessPartnerLocation.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            businessPartnerLocation.Municipality.Code = reader["MunicipalityCode"].ToString();
                            businessPartnerLocation.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["RegionId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Region = new Region();
                            businessPartnerLocation.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            businessPartnerLocation.Region.Code = reader["RegionCode"].ToString();
                            businessPartnerLocation.Region.Name = reader["RegionName"].ToString();
                        }
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerLocation.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerLocation.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerLocation.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerLocation.CreatedBy = new User();
                            businessPartnerLocation.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerLocation.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Company = new Company();
                            businessPartnerLocation.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerLocation.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerLocation.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerLocations.Add(businessPartnerLocation);
                    }
                }
            }
            return BusinessPartnerLocations;

            //List<BusinessPartnerLocation> businessPartnerLocationes = context.BusinessPartnerLocations
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Country)
            //    .Include(x => x.City)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.Region)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.BusinessPartnerId == businessPartnerId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return businessPartnerLocationes;
        }

        public List<BusinessPartnerLocation> GetBusinessPartnerLocationsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerLocation> BusinessPartnerLocations = new List<BusinessPartnerLocation>();

            string queryString =
                "SELECT BusinessPartnerLocationId, BusinessPartnerLocationIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Address, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "ItemStatus, Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerLocations " +
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
                    BusinessPartnerLocation businessPartnerLocation;
                    while (reader.Read())
                    {
                        businessPartnerLocation = new BusinessPartnerLocation();
                        businessPartnerLocation.Id = Int32.Parse(reader["BusinessPartnerLocationId"].ToString());
                        businessPartnerLocation.Identifier = Guid.Parse(reader["BusinessPartnerLocationIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerLocation.BusinessPartner = new BusinessPartner();
                            businessPartnerLocation.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerLocation.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerLocation.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Address"] != DBNull.Value)
                            businessPartnerLocation.Address = reader["Address"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Country = new Country();
                            businessPartnerLocation.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerLocation.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerLocation.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            businessPartnerLocation.City = new City();
                            businessPartnerLocation.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.City.ZipCode = reader["CityZipCode"].ToString();
                            businessPartnerLocation.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Municipality = new Municipality();
                            businessPartnerLocation.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            businessPartnerLocation.Municipality.Code = reader["MunicipalityCode"].ToString();
                            businessPartnerLocation.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["RegionId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Region = new Region();
                            businessPartnerLocation.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            businessPartnerLocation.Region.Code = reader["RegionCode"].ToString();
                            businessPartnerLocation.Region.Name = reader["RegionName"].ToString();
                        }
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerLocation.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerLocation.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerLocation.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerLocation.CreatedBy = new User();
                            businessPartnerLocation.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerLocation.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerLocation.Company = new Company();
                            businessPartnerLocation.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerLocation.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerLocation.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerLocations.Add(businessPartnerLocation);
                    }
                }
            }
            return BusinessPartnerLocations;

            //List<BusinessPartnerLocation> businessPartnerLocationes = context.BusinessPartnerLocations
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Country)
            //    .Include(x => x.City)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.Region)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return businessPartnerLocationes;
        }

       
        public BusinessPartnerLocation Create(BusinessPartnerLocation businessPartnerLocation)
        {
            if (context.BusinessPartnerLocations.Where(x => x.Identifier != null && x.Identifier == businessPartnerLocation.Identifier).Count() == 0)
            {
                businessPartnerLocation.Id = 0;

                businessPartnerLocation.Active = true;
                businessPartnerLocation.CreatedAt = DateTime.Now;
                businessPartnerLocation.UpdatedAt = DateTime.Now;
                context.BusinessPartnerLocations.Add(businessPartnerLocation);
                return businessPartnerLocation;
            }
            else
            {
                // Load businessPartnerLocation that will be updated
                BusinessPartnerLocation dbEntry = context.BusinessPartnerLocations
                    .FirstOrDefault(x => x.Identifier == businessPartnerLocation.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerLocation.BusinessPartnerId ?? null;
                    dbEntry.CountryId = businessPartnerLocation.CountryId ?? null;
                    dbEntry.CityId = businessPartnerLocation.CityId ?? null;
                    dbEntry.MunicipalityId = businessPartnerLocation.MunicipalityId ?? null;
                    dbEntry.RegionId = businessPartnerLocation.RegionId ?? null;
                    dbEntry.CompanyId = businessPartnerLocation.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerLocation.CreatedById ?? null;

                    // Set properties
                    dbEntry.Address = businessPartnerLocation.Address;
                    dbEntry.ItemStatus = businessPartnerLocation.ItemStatus;
                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerLocation Delete(Guid identifier)
        {
            BusinessPartnerLocation dbEntry = context.BusinessPartnerLocations
               .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerLocation))
                    .Select(x => x.Entity as BusinessPartnerLocation))
                .FirstOrDefault(x => x.Identifier == identifier);
            if (dbEntry != null)
            {
                dbEntry.Active = false;
                dbEntry.UpdatedAt = DateTime.Now;
            }
            return dbEntry;
        }
    }
}
