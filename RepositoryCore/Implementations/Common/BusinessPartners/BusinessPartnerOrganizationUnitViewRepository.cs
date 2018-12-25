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
    public class BusinessPartnerOrganizationUnitViewRepository : IBusinessPartnerOrganizationUnitRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public BusinessPartnerOrganizationUnitViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<BusinessPartnerOrganizationUnit> GetBusinessPartnerOrganizationUnits(int companyId)
        {
            List<BusinessPartnerOrganizationUnit> BusinessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnit>();

            string queryString =
                "SELECT BusinessPartnerOrganizationUnitId, BusinessPartnerOrganizationUnitIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Code, Name, Address, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityCode, CityName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "ContactPerson, Phone, Mobile, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerOrganizationUnits " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerOrganizationUnit businessPartnerOrganizationUnit;
                    while (reader.Read())
                    {
                        businessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnit();
                        businessPartnerOrganizationUnit.Id = Int32.Parse(reader["BusinessPartnerOrganizationUnitId"].ToString());
                        businessPartnerOrganizationUnit.Identifier = Guid.Parse(reader["BusinessPartnerOrganizationUnitIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.BusinessPartner = new BusinessPartner();
                            businessPartnerOrganizationUnit.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerOrganizationUnit.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Code"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Code = reader["Code"].ToString();
                        if (reader["Name"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Name = reader["Name"].ToString();
                        if (reader["Address"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Address = reader["Address"].ToString();
                        
                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Country = new Country();
                            businessPartnerOrganizationUnit.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerOrganizationUnit.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerOrganizationUnit.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerOrganizationUnit.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerOrganizationUnit.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.City = new City();
                            businessPartnerOrganizationUnit.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerOrganizationUnit.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerOrganizationUnit.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerOrganizationUnit.City.Code = reader["CityCode"].ToString();
                            businessPartnerOrganizationUnit.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Municipality = new Municipality();
                            businessPartnerOrganizationUnit.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Code = reader["MunicipalityCode"].ToString();
                            businessPartnerOrganizationUnit.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["ContactPerson"] != DBNull.Value)
                            businessPartnerOrganizationUnit.ContactPerson = reader["ContactPerson"].ToString();
                        if (reader["Phone"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Phone = reader["Phone"].ToString();
                        if (reader["Mobile"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Mobile = reader["Mobile"].ToString();

                        businessPartnerOrganizationUnit.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerOrganizationUnit.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.CreatedBy = new User();
                            businessPartnerOrganizationUnit.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerOrganizationUnit.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerOrganizationUnit.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerOrganizationUnit.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Company = new Company();
                            businessPartnerOrganizationUnit.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerOrganizationUnit.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerOrganizationUnit.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerOrganizationUnits.Add(businessPartnerOrganizationUnit);
                    }
                }
            }
            return BusinessPartnerOrganizationUnits;

            //List<BusinessPartnerOrganizationUnit> businessPartnerOrganizationUnits = context.BusinessPartnerOrganizationUnits
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Country)
            //    .Include(x => x.City)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return businessPartnerOrganizationUnits;
        }

        public List<BusinessPartnerOrganizationUnit> GetBusinessPartnerOrganizationUnitsByBusinessPartner(int businessPartnerId)
        {
            List<BusinessPartnerOrganizationUnit> BusinessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnit>();

            string queryString =
                "SELECT BusinessPartnerOrganizationUnitId, BusinessPartnerOrganizationUnitIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Code, Name, Address, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityCode, CityName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "ContactPerson, Phone, Mobile, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerOrganizationUnits " +
                "WHERE BusinessPartnerId = @BusinessPartnerId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@BusinessPartnerId", businessPartnerId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerOrganizationUnit businessPartnerOrganizationUnit;
                    while (reader.Read())
                    {
                        businessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnit();
                        businessPartnerOrganizationUnit.Id = Int32.Parse(reader["BusinessPartnerOrganizationUnitId"].ToString());
                        businessPartnerOrganizationUnit.Identifier = Guid.Parse(reader["BusinessPartnerOrganizationUnitIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.BusinessPartner = new BusinessPartner();
                            businessPartnerOrganizationUnit.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerOrganizationUnit.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Code"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Code = reader["Code"].ToString();
                        if (reader["Name"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Name = reader["Name"].ToString();
                        if (reader["Address"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Address = reader["Address"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Country = new Country();
                            businessPartnerOrganizationUnit.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerOrganizationUnit.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerOrganizationUnit.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerOrganizationUnit.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerOrganizationUnit.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.City = new City();
                            businessPartnerOrganizationUnit.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerOrganizationUnit.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerOrganizationUnit.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerOrganizationUnit.City.Code = reader["CityCode"].ToString();
                            businessPartnerOrganizationUnit.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Municipality = new Municipality();
                            businessPartnerOrganizationUnit.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Code = reader["MunicipalityCode"].ToString();
                            businessPartnerOrganizationUnit.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["ContactPerson"] != DBNull.Value)
                            businessPartnerOrganizationUnit.ContactPerson = reader["ContactPerson"].ToString();
                        if (reader["Phone"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Phone = reader["Phone"].ToString();
                        if (reader["Mobile"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Mobile = reader["Mobile"].ToString();

                        businessPartnerOrganizationUnit.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerOrganizationUnit.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.CreatedBy = new User();
                            businessPartnerOrganizationUnit.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerOrganizationUnit.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerOrganizationUnit.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerOrganizationUnit.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Company = new Company();
                            businessPartnerOrganizationUnit.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerOrganizationUnit.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerOrganizationUnit.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerOrganizationUnits.Add(businessPartnerOrganizationUnit);
                    }
                }
            }
            return BusinessPartnerOrganizationUnits;

            //List<BusinessPartnerOrganizationUnit> businessPartnerOrganizationUnits = context.BusinessPartnerOrganizationUnits
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Country)
            //    .Include(x => x.City)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.BusinessPartnerId == businessPartnerId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return businessPartnerOrganizationUnits;
        }

        public List<BusinessPartnerOrganizationUnit> GetBusinessPartnerOrganizationUnitsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerOrganizationUnit> BusinessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnit>();

            string queryString =
                "SELECT BusinessPartnerOrganizationUnitId, BusinessPartnerOrganizationUnitIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Code, Name, Address, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityCode, CityName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "ContactPerson, Phone, Mobile, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerOrganizationUnits " +
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
                    BusinessPartnerOrganizationUnit businessPartnerOrganizationUnit;
                    while (reader.Read())
                    {
                        businessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnit();
                        businessPartnerOrganizationUnit.Id = Int32.Parse(reader["BusinessPartnerOrganizationUnitId"].ToString());
                        businessPartnerOrganizationUnit.Identifier = Guid.Parse(reader["BusinessPartnerOrganizationUnitIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.BusinessPartner = new BusinessPartner();
                            businessPartnerOrganizationUnit.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerOrganizationUnit.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Code"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Code = reader["Code"].ToString();
                        if (reader["Name"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Name = reader["Name"].ToString();
                        if (reader["Address"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Address = reader["Address"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Country = new Country();
                            businessPartnerOrganizationUnit.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerOrganizationUnit.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerOrganizationUnit.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerOrganizationUnit.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerOrganizationUnit.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.City = new City();
                            businessPartnerOrganizationUnit.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerOrganizationUnit.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerOrganizationUnit.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerOrganizationUnit.City.Code = reader["CityCode"].ToString();
                            businessPartnerOrganizationUnit.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Municipality = new Municipality();
                            businessPartnerOrganizationUnit.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Code = reader["MunicipalityCode"].ToString();
                            businessPartnerOrganizationUnit.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["ContactPerson"] != DBNull.Value)
                            businessPartnerOrganizationUnit.ContactPerson = reader["ContactPerson"].ToString();
                        if (reader["Phone"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Phone = reader["Phone"].ToString();
                        if (reader["Mobile"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Mobile = reader["Mobile"].ToString();

                        businessPartnerOrganizationUnit.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerOrganizationUnit.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.CreatedBy = new User();
                            businessPartnerOrganizationUnit.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerOrganizationUnit.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerOrganizationUnit.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerOrganizationUnit.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Company = new Company();
                            businessPartnerOrganizationUnit.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerOrganizationUnit.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerOrganizationUnit.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerOrganizationUnits.Add(businessPartnerOrganizationUnit);
                    }
                }
            }
            return BusinessPartnerOrganizationUnits;

            //List<BusinessPartnerOrganizationUnit> businessPartnerOrganizationUnits = context.BusinessPartnerOrganizationUnits
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Country)
            //    .Include(x => x.City)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return businessPartnerOrganizationUnits;
        }

        public BusinessPartnerOrganizationUnit GetBusinessPartnerOrganizationUnit(int id)
        {
            BusinessPartnerOrganizationUnit businessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnit();

            string queryString =
                "SELECT BusinessPartnerOrganizationUnitId, BusinessPartnerOrganizationUnitIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Code, Name, Address, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "CityId, CityIdentifier, CityCode, CityName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "ContactPerson, Phone, Mobile, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerOrganizationUnits " +
                "WHERE BusinessPartnerOrganizationUnitId = @BusinessPartnerOrganizationUnitId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@BusinessPartnerOrganizationUnitId", id));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        businessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnit();
                        businessPartnerOrganizationUnit.Id = Int32.Parse(reader["BusinessPartnerOrganizationUnitId"].ToString());
                        businessPartnerOrganizationUnit.Identifier = Guid.Parse(reader["BusinessPartnerOrganizationUnitIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.BusinessPartner = new BusinessPartner();
                            businessPartnerOrganizationUnit.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerOrganizationUnit.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerOrganizationUnit.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Code"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Code = reader["Code"].ToString();
                        if (reader["Name"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Name = reader["Name"].ToString();
                        if (reader["Address"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Address = reader["Address"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Country = new Country();
                            businessPartnerOrganizationUnit.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerOrganizationUnit.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            businessPartnerOrganizationUnit.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            businessPartnerOrganizationUnit.Country.Code = reader["CountryCode"].ToString();
                            businessPartnerOrganizationUnit.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.City = new City();
                            businessPartnerOrganizationUnit.CityId = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerOrganizationUnit.City.Id = Int32.Parse(reader["CityId"].ToString());
                            businessPartnerOrganizationUnit.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            businessPartnerOrganizationUnit.City.Code = reader["CityCode"].ToString();
                            businessPartnerOrganizationUnit.City.Name = reader["CityName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Municipality = new Municipality();
                            businessPartnerOrganizationUnit.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            businessPartnerOrganizationUnit.Municipality.Code = reader["MunicipalityCode"].ToString();
                            businessPartnerOrganizationUnit.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["ContactPerson"] != DBNull.Value)
                            businessPartnerOrganizationUnit.ContactPerson = reader["ContactPerson"].ToString();
                        if (reader["Phone"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Phone = reader["Phone"].ToString();
                        if (reader["Mobile"] != DBNull.Value)
                            businessPartnerOrganizationUnit.Mobile = reader["Mobile"].ToString();

                        businessPartnerOrganizationUnit.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerOrganizationUnit.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.CreatedBy = new User();
                            businessPartnerOrganizationUnit.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerOrganizationUnit.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerOrganizationUnit.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerOrganizationUnit.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerOrganizationUnit.Company = new Company();
                            businessPartnerOrganizationUnit.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerOrganizationUnit.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerOrganizationUnit.Company.Name = reader["CompanyName"].ToString();
                        }

                    }
                }
            }
            return businessPartnerOrganizationUnit;

            //return context.BusinessPartnerOrganizationUnits
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Country)
            //    .Include(x => x.City)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public BusinessPartnerOrganizationUnit Create(BusinessPartnerOrganizationUnit businessPartnerOrganizationUnit)
        {
            if (context.BusinessPartnerOrganizationUnits.Where(x => x.Identifier != null && x.Identifier == businessPartnerOrganizationUnit.Identifier).Count() == 0)
            {
                businessPartnerOrganizationUnit.Id = 0;

                businessPartnerOrganizationUnit.Active = true;

                context.BusinessPartnerOrganizationUnits.Add(businessPartnerOrganizationUnit);
                return businessPartnerOrganizationUnit;
            }
            else
            {
                // Load businessPartnerOrganizationUnit that will be updated
                BusinessPartnerOrganizationUnit dbEntry = context.BusinessPartnerOrganizationUnits
                    .FirstOrDefault(x => x.Identifier == businessPartnerOrganizationUnit.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerOrganizationUnit.BusinessPartnerId ?? null;
                    dbEntry.CountryId = businessPartnerOrganizationUnit.CountryId ?? null;
                    dbEntry.CityId = businessPartnerOrganizationUnit.CityId ?? null;
                    dbEntry.MunicipalityId = businessPartnerOrganizationUnit.MunicipalityId ?? null;
                    dbEntry.CompanyId = businessPartnerOrganizationUnit.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerOrganizationUnit.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = businessPartnerOrganizationUnit.Code;
                    dbEntry.Name = businessPartnerOrganizationUnit.Name;
                    dbEntry.Address = businessPartnerOrganizationUnit.Address;
                    dbEntry.ContactPerson = businessPartnerOrganizationUnit.ContactPerson;
                    dbEntry.Phone = businessPartnerOrganizationUnit.Phone;
                    dbEntry.Mobile = businessPartnerOrganizationUnit.Mobile;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerOrganizationUnit Delete(Guid identifier)
        {
            BusinessPartnerOrganizationUnit dbEntry = context.BusinessPartnerOrganizationUnits
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
