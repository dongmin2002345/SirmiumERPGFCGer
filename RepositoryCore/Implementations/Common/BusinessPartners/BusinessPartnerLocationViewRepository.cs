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
                "CityId, CityIdentifier, CityCode, CityName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
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

                        if (reader["BusinessPartnerId"] != null)
                        {
                            businessPartnerLocation.BusinessPartner = new BusinessPartner();
                            businessPartnerLocation.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerLocation.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerLocation.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Address"] != null)
                            businessPartnerLocation.Address = reader["Address"].ToString();

                        if (reader["CountryId"] != null)
                        {
                            businessPartnerLocation.Country = new Country();
                            businessPartnerLocation.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerLocation.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerLocation.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != null)
                        {
                            businessPartnerLocation.City = new City();
                            businessPartnerLocation.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.City.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["MunicipalityId"] != null)
                        {
                            businessPartnerLocation.Municipality = new Municipality();
                            businessPartnerLocation.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.Municipality.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.Municipality.Name = reader["CityName"].ToString();
                        }

                        if (reader["RegionId"] != null)
                        {
                            businessPartnerLocation.Region = new Region();
                            businessPartnerLocation.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.Region.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.Region.Name = reader["CityName"].ToString();
                        }

                        businessPartnerLocation.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerLocation.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            businessPartnerLocation.CreatedBy = new User();
                            businessPartnerLocation.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerLocation.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return businessPartnerLocationes;
        }

        public List<BusinessPartnerLocation> GetBusinessPartnerLocationssByBusinessPartner(int businessPartnerId)
        {
            List<BusinessPartnerLocation> BusinessPartnerLocations = new List<BusinessPartnerLocation>();

            string queryString =
                "SELECT BusinessPartnerLocationId, BusinessPartnerLocationIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Address, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityCode, CityName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
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

                        if (reader["BusinessPartnerId"] != null)
                        {
                            businessPartnerLocation.BusinessPartner = new BusinessPartner();
                            businessPartnerLocation.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerLocation.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerLocation.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Address"] != null)
                            businessPartnerLocation.Address = reader["Address"].ToString();

                        if (reader["CountryId"] != null)
                        {
                            businessPartnerLocation.Country = new Country();
                            businessPartnerLocation.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerLocation.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerLocation.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != null)
                        {
                            businessPartnerLocation.City = new City();
                            businessPartnerLocation.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.City.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["MunicipalityId"] != null)
                        {
                            businessPartnerLocation.Municipality = new Municipality();
                            businessPartnerLocation.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.Municipality.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.Municipality.Name = reader["CityName"].ToString();
                        }

                        if (reader["RegionId"] != null)
                        {
                            businessPartnerLocation.Region = new Region();
                            businessPartnerLocation.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.Region.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.Region.Name = reader["CityName"].ToString();
                        }

                        businessPartnerLocation.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerLocation.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            businessPartnerLocation.CreatedBy = new User();
                            businessPartnerLocation.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerLocation.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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
                "CityId, CityIdentifier, CityCode, CityName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerLocations " +
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
                    BusinessPartnerLocation businessPartnerLocation;
                    while (reader.Read())
                    {
                        businessPartnerLocation = new BusinessPartnerLocation();
                        businessPartnerLocation.Id = Int32.Parse(reader["BusinessPartnerLocationId"].ToString());
                        businessPartnerLocation.Identifier = Guid.Parse(reader["BusinessPartnerLocationIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != null)
                        {
                            businessPartnerLocation.BusinessPartner = new BusinessPartner();
                            businessPartnerLocation.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerLocation.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerLocation.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Address"] != null)
                            businessPartnerLocation.Address = reader["Address"].ToString();

                        if (reader["CountryId"] != null)
                        {
                            businessPartnerLocation.Country = new Country();
                            businessPartnerLocation.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerLocation.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerLocation.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != null)
                        {
                            businessPartnerLocation.City = new City();
                            businessPartnerLocation.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.City.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["MunicipalityId"] != null)
                        {
                            businessPartnerLocation.Municipality = new Municipality();
                            businessPartnerLocation.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.Municipality.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.Municipality.Name = reader["CityName"].ToString();
                        }

                        if (reader["RegionId"] != null)
                        {
                            businessPartnerLocation.Region = new Region();
                            businessPartnerLocation.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.Region.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.Region.Name = reader["CityName"].ToString();
                        }

                        businessPartnerLocation.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerLocation.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            businessPartnerLocation.CreatedBy = new User();
                            businessPartnerLocation.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerLocation.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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

        public BusinessPartnerLocation GetBusinessPartnerLocation(int id)
        {
            BusinessPartnerLocation businessPartnerLocation = new BusinessPartnerLocation();

            string queryString =
                "SELECT BusinessPartnerLocationId, BusinessPartnerLocationIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Address, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityCode, CityName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerLocations " +
                "WHERE BusinessPartnerLocationId = @BusinessPartnerLocationId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@BusinessPartnerLocationId", id));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        businessPartnerLocation = new BusinessPartnerLocation();
                        businessPartnerLocation.Id = Int32.Parse(reader["BusinessPartnerLocationId"].ToString());
                        businessPartnerLocation.Identifier = Guid.Parse(reader["BusinessPartnerLocationIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != null)
                        {
                            businessPartnerLocation.BusinessPartner = new BusinessPartner();
                            businessPartnerLocation.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerLocation.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerLocation.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerLocation.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Address"] != null)
                            businessPartnerLocation.Address = reader["Address"].ToString();

                        if (reader["CountryId"] != null)
                        {
                            businessPartnerLocation.Country = new Country();
                            businessPartnerLocation.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerLocation.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerLocation.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerLocation.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != null)
                        {
                            businessPartnerLocation.City = new City();
                            businessPartnerLocation.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerLocation.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.City.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["MunicipalityId"] != null)
                        {
                            businessPartnerLocation.Municipality = new Municipality();
                            businessPartnerLocation.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerLocation.Municipality.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.Municipality.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.Municipality.Name = reader["CityName"].ToString();
                        }

                        if (reader["RegionId"] != null)
                        {
                            businessPartnerLocation.Region = new Region();
                            businessPartnerLocation.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            businessPartnerLocation.Region.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerLocation.Region.Code = reader["CityCode"].ToString();
                            businessPartnerLocation.Region.Name = reader["CityName"].ToString();
                        }

                        businessPartnerLocation.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerLocation.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            businessPartnerLocation.CreatedBy = new User();
                            businessPartnerLocation.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerLocation.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerLocation.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            businessPartnerLocation.Company = new Company();
                            businessPartnerLocation.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerLocation.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerLocation.Company.Name = reader["CompanyName"].ToString();
                        }

                    }
                }
            }
            return businessPartnerLocation;

            //return context.BusinessPartnerLocations
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Country)
            //    .Include(x => x.City)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.Region)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public BusinessPartnerLocation Create(BusinessPartnerLocation businessPartnerLocation)
        {
            if (context.BusinessPartnerLocations.Where(x => x.Identifier != null && x.Identifier == businessPartnerLocation.Identifier).Count() == 0)
            {
                businessPartnerLocation.Id = 0;

                businessPartnerLocation.Active = true;

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

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerLocation Delete(Guid identifier)
        {
            BusinessPartnerLocation dbEntry = context.BusinessPartnerLocations
                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

            if (dbEntry != null)
            {
                dbEntry.Active = false;
                dbEntry.UpdatedAt = DateTime.Now;
            }
            return dbEntry;
        }
    }
}
