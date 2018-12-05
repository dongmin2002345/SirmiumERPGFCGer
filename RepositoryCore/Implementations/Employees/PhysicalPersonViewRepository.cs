using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using DomainCore.Employees;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.PhysicalPersons
{
    public class PhysicalPersonViewRepository : IPhysicalPersonRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public PhysicalPersonViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

        }

        #region GET methods

        public List<PhysicalPerson> GetPhysicalPersons(int companyId)
        {
            List<PhysicalPerson> PhysicalPersons = new List<PhysicalPerson>();

            string queryString =
                "SELECT PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonPhysicalPersonCode, PhysicalPersonName, PhysicalPersonSurName, PhysicalPersonConstructionSiteCode, PhysicalPersonConstructionSiteName, DateOfBirth, Gender, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "CityId, CityIdentifier, CityCode, CityName, " +
                "Address, " +
                "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                "PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
                "Passport, VisaFrom, VisaTo, " +
                "ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                "ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, " +
                "ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vPhysicalPersons " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPerson physicalPerson;
                    while (reader.Read())
                    {
                        physicalPerson = new PhysicalPerson();
                        physicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                        physicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                        if (reader["PhysicalPersonCode"] != DBNull.Value)
                            physicalPerson.Code = reader["PhysicalPersonCode"]?.ToString();
                        if (reader["PhysicalPersonPhysicalPersonCode"] != DBNull.Value)
                            physicalPerson.PhysicalPersonCode = reader["PhysicalPersonPhysicalPersonCode"]?.ToString();
                        if (reader["PhysicalPersonName"] != DBNull.Value)
                            physicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        if (reader["PhysicalPersonSurName"] != DBNull.Value)
                            physicalPerson.SurName = reader["PhysicalPersonSurName"].ToString();
                        if (reader["PhysicalPersonConstructionSiteCode"] != DBNull.Value)
                            physicalPerson.ConstructionSiteCode = reader["PhysicalPersonConstructionSiteCode"]?.ToString();
                        if (reader["PhysicalPersonConstructionSiteName"] != DBNull.Value)
                            physicalPerson.ConstructionSiteName = reader["PhysicalPersonConstructionSiteName"]?.ToString();

                        if (reader["DateOfBirth"] != DBNull.Value)
                            physicalPerson.DateOfBirth = DateTime.Parse(reader["DateOfBirth"]?.ToString());
                        if (reader["Gender"] != DBNull.Value)
                            physicalPerson.Gender = Int32.Parse(reader["Gender"].ToString());

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            physicalPerson.Country = new Country();
                            physicalPerson.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            physicalPerson.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            physicalPerson.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            physicalPerson.Country.Code = reader["CountryCode"].ToString();
                            physicalPerson.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["RegionId"] != DBNull.Value)
                        {
                            physicalPerson.Region = new Region();
                            physicalPerson.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            physicalPerson.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            physicalPerson.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            physicalPerson.Region.Code = reader["RegionCode"].ToString();
                            physicalPerson.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value)
                        {
                            physicalPerson.Municipality = new Municipality();
                            physicalPerson.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            physicalPerson.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            physicalPerson.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            physicalPerson.Municipality.Code = reader["MunicipalityCode"].ToString();
                            physicalPerson.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            physicalPerson.City = new City();
                            physicalPerson.CityId = Int32.Parse(reader["CityId"].ToString());
                            physicalPerson.City.Id = Int32.Parse(reader["CityId"].ToString());
                            physicalPerson.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            physicalPerson.City.Code = reader["CityCode"].ToString();
                            physicalPerson.City.Name = reader["CityName"].ToString();
                        }


                        if (reader["Address"] != DBNull.Value)
                            physicalPerson.Address = reader["Address"].ToString();


                        if (reader["PassportCountryId"] != DBNull.Value)
                        {
                            physicalPerson.PassportCountry = new Country();
                            physicalPerson.PassportCountryId = Int32.Parse(reader["PassportCountryId"].ToString());
                            physicalPerson.PassportCountry.Id = Int32.Parse(reader["PassportCountryId"].ToString());
                            physicalPerson.PassportCountry.Identifier = Guid.Parse(reader["PassportCountryIdentifier"].ToString());
                            physicalPerson.PassportCountry.Code = reader["PassportCountryCode"].ToString();
                            physicalPerson.PassportCountry.Name = reader["PassportCountryName"].ToString();
                        }

                        if (reader["PassportCityId"] != DBNull.Value)
                        {
                            physicalPerson.PassportCity = new City();
                            physicalPerson.PassportCityId = Int32.Parse(reader["PassportCityId"].ToString());
                            physicalPerson.PassportCity.Id = Int32.Parse(reader["PassportCityId"].ToString());
                            physicalPerson.PassportCity.Identifier = Guid.Parse(reader["PassportCityIdentifier"].ToString());
                            physicalPerson.PassportCity.Code = reader["PassportCityCode"].ToString();
                            physicalPerson.PassportCity.Name = reader["PassportCityName"].ToString();
                        }


                        if (reader["Passport"] != DBNull.Value)
                            physicalPerson.Passport = reader["Passport"].ToString();
                        if (reader["VisaFrom"] != DBNull.Value)
                            physicalPerson.VisaFrom = DateTime.Parse(reader["VisaFrom"].ToString());
                        if (reader["VisaTo"] != DBNull.Value)
                            physicalPerson.VisaTo = DateTime.Parse(reader["VisaTo"].ToString());


                        if (reader["ResidenceCountryId"] != DBNull.Value)
                        {
                            physicalPerson.ResidenceCountry = new Country();
                            physicalPerson.ResidenceCountryId = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            physicalPerson.ResidenceCountry.Id = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            physicalPerson.ResidenceCountry.Identifier = Guid.Parse(reader["ResidenceCountryIdentifier"].ToString());
                            physicalPerson.ResidenceCountry.Code = reader["ResidenceCountryCode"].ToString();
                            physicalPerson.ResidenceCountry.Name = reader["ResidenceCountryName"].ToString();
                        }

                        if (reader["ResidenceCityId"] != DBNull.Value)
                        {
                            physicalPerson.ResidenceCity = new City();
                            physicalPerson.ResidenceCityId = Int32.Parse(reader["ResidenceCityId"].ToString());
                            physicalPerson.ResidenceCity.Id = Int32.Parse(reader["ResidenceCityId"].ToString());
                            physicalPerson.ResidenceCity.Identifier = Guid.Parse(reader["ResidenceCityIdentifier"].ToString());
                            physicalPerson.ResidenceCity.Code = reader["ResidenceCityCode"].ToString();
                            physicalPerson.ResidenceCity.Name = reader["ResidenceCityName"].ToString();
                        }


                        if (reader["ResidenceAddress"] != DBNull.Value)
                            physicalPerson.ResidenceAddress = reader["ResidenceAddress"].ToString();
                        if (reader["EmbassyDate"] != DBNull.Value)
                            physicalPerson.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["VisaDate"] != DBNull.Value)
                            physicalPerson.VisaDate = DateTime.Parse(reader["VisaDate"].ToString());
                        if (reader["VisaValidFrom"] != DBNull.Value)
                            physicalPerson.VisaValidFrom = DateTime.Parse(reader["VisaValidFrom"].ToString());
                        if (reader["VisaValidTo"] != DBNull.Value)
                            physicalPerson.VisaValidTo = DateTime.Parse(reader["VisaValidTo"].ToString());
                        if (reader["WorkPermitFrom"] != DBNull.Value)
                            physicalPerson.WorkPermitFrom = DateTime.Parse(reader["WorkPermitFrom"].ToString());
                        if (reader["WorkPermitTo"] != DBNull.Value)
                            physicalPerson.WorkPermitTo = DateTime.Parse(reader["WorkPermitTo"].ToString());


                        physicalPerson.Active = bool.Parse(reader["Active"].ToString());
                        physicalPerson.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPerson.CreatedBy = new User();
                            physicalPerson.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPerson.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPerson.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPerson.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPerson.Company = new Company();
                            physicalPerson.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPerson.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPerson.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersons.Add(physicalPerson);
                    }
                }
            }

            return PhysicalPersons;

            //return context.PhysicalPersons
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Include(x => x.Country)
            //    .Include(x => x.Region)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.City)
            //    .Include(x => x.PassportCountry)
            //    .Include(x => x.PassportCity)
            //    .Include(x => x.ResidenceCountry)
            //    .Include(x => x.ResidenceCity)
            //    .Where(x => x.Company.Id == companyId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();
        }

        public PhysicalPerson GetPhysicalPerson(int physicalPersonId)
        {
            PhysicalPerson physicalPerson = new PhysicalPerson();

            string queryString =
                "SELECT PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonPhysicalPersonCode, PhysicalPersonName, PhysicalPersonSurName, PhysicalPersonConstructionSiteCode, PhysicalPersonConstructionSiteName, DateOfBirth, Gender, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "CityId, CityIdentifier, CityCode, CityName, " +
                "Address, " +
                "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                "PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
                "Passport, VisaFrom, VisaTo, " +
                "ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                "ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, " +
                "ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vPhysicalPersons " +
                "WHERE PhysicalPersonId = @PhysicalPersonId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@PhysicalPersonId", physicalPersonId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        physicalPerson = new PhysicalPerson();
                        physicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                        physicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                        if (reader["PhysicalPersonCode"] != DBNull.Value)
                            physicalPerson.Code = reader["PhysicalPersonCode"]?.ToString();
                        if (reader["PhysicalPersonPhysicalPersonCode"] != DBNull.Value)
                            physicalPerson.PhysicalPersonCode = reader["PhysicalPersonPhysicalPersonCode"]?.ToString();
                        if (reader["PhysicalPersonName"] != DBNull.Value)
                            physicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        if (reader["PhysicalPersonSurName"] != DBNull.Value)
                            physicalPerson.SurName = reader["PhysicalPersonSurName"].ToString();
                        if (reader["PhysicalPersonConstructionSiteCode"] != DBNull.Value)
                            physicalPerson.ConstructionSiteCode = reader["PhysicalPersonConstructionSiteCode"]?.ToString();
                        if (reader["PhysicalPersonConstructionSiteName"] != DBNull.Value)
                            physicalPerson.ConstructionSiteName = reader["PhysicalPersonConstructionSiteName"]?.ToString();

                        if (reader["DateOfBirth"] != DBNull.Value)
                            physicalPerson.DateOfBirth = DateTime.Parse(reader["DateOfBirth"]?.ToString());
                        if (reader["Gender"] != DBNull.Value)
                            physicalPerson.Gender = Int32.Parse(reader["Gender"].ToString());

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            physicalPerson.Country = new Country();
                            physicalPerson.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            physicalPerson.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            physicalPerson.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            physicalPerson.Country.Code = reader["CountryCode"].ToString();
                            physicalPerson.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["RegionId"] != DBNull.Value)
                        {
                            physicalPerson.Region = new Region();
                            physicalPerson.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            physicalPerson.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            physicalPerson.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            physicalPerson.Region.Code = reader["RegionCode"].ToString();
                            physicalPerson.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value)
                        {
                            physicalPerson.Municipality = new Municipality();
                            physicalPerson.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            physicalPerson.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            physicalPerson.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            physicalPerson.Municipality.Code = reader["MunicipalityCode"].ToString();
                            physicalPerson.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value)
                        {
                            physicalPerson.City = new City();
                            physicalPerson.CityId = Int32.Parse(reader["CityId"].ToString());
                            physicalPerson.City.Id = Int32.Parse(reader["CityId"].ToString());
                            physicalPerson.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            physicalPerson.City.Code = reader["CityCode"].ToString();
                            physicalPerson.City.Name = reader["CityName"].ToString();
                        }


                        if (reader["Address"] != DBNull.Value)
                            physicalPerson.Address = reader["Address"].ToString();


                        if (reader["PassportCountryId"] != DBNull.Value)
                        {
                            physicalPerson.PassportCountry = new Country();
                            physicalPerson.PassportCountryId = Int32.Parse(reader["PassportCountryId"].ToString());
                            physicalPerson.PassportCountry.Id = Int32.Parse(reader["PassportCountryId"].ToString());
                            physicalPerson.PassportCountry.Identifier = Guid.Parse(reader["PassportCountryIdentifier"].ToString());
                            physicalPerson.PassportCountry.Code = reader["PassportCountryCode"].ToString();
                            physicalPerson.PassportCountry.Name = reader["PassportCountryName"].ToString();
                        }

                        if (reader["PassportCityId"] != DBNull.Value)
                        {
                            physicalPerson.PassportCity = new City();
                            physicalPerson.PassportCityId = Int32.Parse(reader["PassportCityId"].ToString());
                            physicalPerson.PassportCity.Id = Int32.Parse(reader["PassportCityId"].ToString());
                            physicalPerson.PassportCity.Identifier = Guid.Parse(reader["PassportCityIdentifier"].ToString());
                            physicalPerson.PassportCity.Code = reader["PassportCityCode"].ToString();
                            physicalPerson.PassportCity.Name = reader["PassportCityName"].ToString();
                        }


                        if (reader["Passport"] != DBNull.Value)
                            physicalPerson.Passport = reader["Passport"].ToString();
                        if (reader["VisaFrom"] != DBNull.Value)
                            physicalPerson.VisaFrom = DateTime.Parse(reader["VisaFrom"].ToString());
                        if (reader["VisaTo"] != DBNull.Value)
                            physicalPerson.VisaTo = DateTime.Parse(reader["VisaTo"].ToString());


                        if (reader["ResidenceCountryId"] != DBNull.Value)
                        {
                            physicalPerson.ResidenceCountry = new Country();
                            physicalPerson.ResidenceCountryId = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            physicalPerson.ResidenceCountry.Id = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            physicalPerson.ResidenceCountry.Identifier = Guid.Parse(reader["ResidenceCountryIdentifier"].ToString());
                            physicalPerson.ResidenceCountry.Code = reader["ResidenceCountryCode"].ToString();
                            physicalPerson.ResidenceCountry.Name = reader["ResidenceCountryName"].ToString();
                        }

                        if (reader["ResidenceCityId"] != DBNull.Value)
                        {
                            physicalPerson.ResidenceCity = new City();
                            physicalPerson.ResidenceCityId = Int32.Parse(reader["ResidenceCityId"].ToString());
                            physicalPerson.ResidenceCity.Id = Int32.Parse(reader["ResidenceCityId"].ToString());
                            physicalPerson.ResidenceCity.Identifier = Guid.Parse(reader["ResidenceCityIdentifier"].ToString());
                            physicalPerson.ResidenceCity.Code = reader["ResidenceCityCode"].ToString();
                            physicalPerson.ResidenceCity.Name = reader["ResidenceCityName"].ToString();
                        }


                        if (reader["ResidenceAddress"] != DBNull.Value)
                            physicalPerson.ResidenceAddress = reader["ResidenceAddress"].ToString();
                        if (reader["EmbassyDate"] != DBNull.Value)
                            physicalPerson.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["VisaDate"] != DBNull.Value)
                            physicalPerson.VisaDate = DateTime.Parse(reader["VisaDate"].ToString());
                        if (reader["VisaValidFrom"] != DBNull.Value)
                            physicalPerson.VisaValidFrom = DateTime.Parse(reader["VisaValidFrom"].ToString());
                        if (reader["VisaValidTo"] != DBNull.Value)
                            physicalPerson.VisaValidTo = DateTime.Parse(reader["VisaValidTo"].ToString());
                        if (reader["WorkPermitFrom"] != DBNull.Value)
                            physicalPerson.WorkPermitFrom = DateTime.Parse(reader["WorkPermitFrom"].ToString());
                        if (reader["WorkPermitTo"] != DBNull.Value)
                            physicalPerson.WorkPermitTo = DateTime.Parse(reader["WorkPermitTo"].ToString());


                        physicalPerson.Active = bool.Parse(reader["Active"].ToString());
                        physicalPerson.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPerson.CreatedBy = new User();
                            physicalPerson.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPerson.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPerson.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPerson.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPerson.Company = new Company();
                            physicalPerson.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPerson.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPerson.Company.Name = reader["CompanyName"].ToString();
                        }

                    }
                }
            }

            return physicalPerson;

            //return context.PhysicalPersons
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Include(x => x.Country)
            //    .Include(x => x.Region)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.City)
            //    .Include(x => x.PassportCountry)
            //    .Include(x => x.PassportCity)
            //    .Include(x => x.ResidenceCountry)
            //    .Include(x => x.ResidenceCity)
            //    .FirstOrDefault(x => x.Id == physicalPersonId && x.Active == true);
        }

        public List<PhysicalPerson> GetPhysicalPersonsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<PhysicalPerson> PhysicalPersons = new List<PhysicalPerson>();

            string queryString =
                "SELECT PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonPhysicalPersonCode, PhysicalPersonName, PhysicalPersonSurName, PhysicalPersonConstructionSiteCode, PhysicalPersonConstructionSiteName, DateOfBirth, Gender, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "CityId, CityIdentifier, CityCode, CityName, " +
                "Address, " +
                "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                "PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
                "Passport, VisaFrom, VisaTo, " +
                "ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                "ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, " +
                "ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vPhysicalPersons " +
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
                    PhysicalPerson physicalPerson;
                    while (reader.Read())
                    {
                        physicalPerson = new PhysicalPerson();
                        physicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                        physicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                        if (reader["PhysicalPersonCode"] != DBNull.Value)
                            physicalPerson.Code = reader["PhysicalPersonCode"]?.ToString();
                        if (reader["PhysicalPersonPhysicalPersonCode"] != DBNull.Value)
                            physicalPerson.PhysicalPersonCode = reader["PhysicalPersonPhysicalPersonCode"]?.ToString();
                        if (reader["PhysicalPersonName"] != DBNull.Value)
                            physicalPerson.Name = reader["PhysicalPersonName"].ToString();
                        if (reader["PhysicalPersonSurName"] != DBNull.Value)
                            physicalPerson.SurName = reader["PhysicalPersonSurName"].ToString();
                        if (reader["PhysicalPersonConstructionSiteCode"] != DBNull.Value)
                            physicalPerson.ConstructionSiteCode = reader["PhysicalPersonConstructionSiteCode"]?.ToString();
                        if (reader["PhysicalPersonConstructionSiteName"] != DBNull.Value)
                            physicalPerson.ConstructionSiteName = reader["PhysicalPersonConstructionSiteName"]?.ToString();

                        if (reader["DateOfBirth"] != DBNull.Value)
                            physicalPerson.DateOfBirth = DateTime.Parse(reader["DateOfBirth"]?.ToString());
                        if (reader["Gender"] != DBNull.Value)
                            physicalPerson.Gender = Int32.Parse(reader["Gender"].ToString());

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            physicalPerson.Country = new Country();
                            physicalPerson.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            physicalPerson.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            physicalPerson.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            physicalPerson.Country.Code = reader["CountryCode"].ToString();
                            physicalPerson.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["RegionId"] != DBNull.Value) 
                        {
                            physicalPerson.Region = new Region();
                            physicalPerson.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            physicalPerson.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            physicalPerson.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            physicalPerson.Region.Code = reader["RegionCode"].ToString();
                            physicalPerson.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value) 
                        {
                            physicalPerson.Municipality = new Municipality();
                            physicalPerson.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            physicalPerson.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            physicalPerson.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            physicalPerson.Municipality.Code = reader["MunicipalityCode"].ToString();
                            physicalPerson.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value) 
                        {
                            physicalPerson.City = new City();
                            physicalPerson.CityId = Int32.Parse(reader["CityId"].ToString());
                            physicalPerson.City.Id = Int32.Parse(reader["CityId"].ToString());
                            physicalPerson.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            physicalPerson.City.Code = reader["CityCode"].ToString();
                            physicalPerson.City.Name = reader["CityName"].ToString();
                        }


                        if (reader["Address"] != DBNull.Value)
                            physicalPerson.Address = reader["Address"].ToString();


                        if (reader["PassportCountryId"] != DBNull.Value) 
                        {
                            physicalPerson.PassportCountry = new Country();
                            physicalPerson.PassportCountryId = Int32.Parse(reader["PassportCountryId"].ToString());
                            physicalPerson.PassportCountry.Id = Int32.Parse(reader["PassportCountryId"].ToString());
                            physicalPerson.PassportCountry.Identifier = Guid.Parse(reader["PassportCountryIdentifier"].ToString());
                            physicalPerson.PassportCountry.Code = reader["PassportCountryCode"].ToString();
                            physicalPerson.PassportCountry.Name = reader["PassportCountryName"].ToString();
                        }

                        if (reader["PassportCityId"] != DBNull.Value) 
                        {
                            physicalPerson.PassportCity = new City();
                            physicalPerson.PassportCityId = Int32.Parse(reader["PassportCityId"].ToString());
                            physicalPerson.PassportCity.Id = Int32.Parse(reader["PassportCityId"].ToString());
                            physicalPerson.PassportCity.Identifier = Guid.Parse(reader["PassportCityIdentifier"].ToString());
                            physicalPerson.PassportCity.Code = reader["PassportCityCode"].ToString();
                            physicalPerson.PassportCity.Name = reader["PassportCityName"].ToString();
                        }


                        if (reader["Passport"] != DBNull.Value)
                            physicalPerson.Passport = reader["Passport"].ToString();
                        if (reader["VisaFrom"] != DBNull.Value)
                            physicalPerson.VisaFrom = DateTime.Parse(reader["VisaFrom"].ToString());
                        if (reader["VisaTo"] != DBNull.Value)
                            physicalPerson.VisaTo = DateTime.Parse(reader["VisaTo"].ToString());


                        if (reader["ResidenceCountryId"] != DBNull.Value) 
                        {
                            physicalPerson.ResidenceCountry = new Country();
                            physicalPerson.ResidenceCountryId = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            physicalPerson.ResidenceCountry.Id = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            physicalPerson.ResidenceCountry.Identifier = Guid.Parse(reader["ResidenceCountryIdentifier"].ToString());
                            physicalPerson.ResidenceCountry.Code = reader["ResidenceCountryCode"].ToString();
                            physicalPerson.ResidenceCountry.Name = reader["ResidenceCountryName"].ToString();
                        }

                        if (reader["ResidenceCityId"] != DBNull.Value) 
                        {
                            physicalPerson.ResidenceCity = new City();
                            physicalPerson.ResidenceCityId = Int32.Parse(reader["ResidenceCityId"].ToString());
                            physicalPerson.ResidenceCity.Id = Int32.Parse(reader["ResidenceCityId"].ToString());
                            physicalPerson.ResidenceCity.Identifier = Guid.Parse(reader["ResidenceCityIdentifier"].ToString());
                            physicalPerson.ResidenceCity.Code = reader["ResidenceCityCode"].ToString();
                            physicalPerson.ResidenceCity.Name = reader["ResidenceCityName"].ToString();
                        }


                        if (reader["ResidenceAddress"] != DBNull.Value)
                            physicalPerson.ResidenceAddress = reader["ResidenceAddress"].ToString();
                        if (reader["EmbassyDate"] != DBNull.Value)
                            physicalPerson.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["VisaDate"] != DBNull.Value)
                            physicalPerson.VisaDate = DateTime.Parse(reader["VisaDate"].ToString());
                        if (reader["VisaValidFrom"] != DBNull.Value)
                            physicalPerson.VisaValidFrom = DateTime.Parse(reader["VisaValidFrom"].ToString());
                        if (reader["VisaValidTo"] != DBNull.Value)
                            physicalPerson.VisaValidTo = DateTime.Parse(reader["VisaValidTo"].ToString());
                        if (reader["WorkPermitFrom"] != DBNull.Value)
                            physicalPerson.WorkPermitFrom = DateTime.Parse(reader["WorkPermitFrom"].ToString());
                        if (reader["WorkPermitTo"] != DBNull.Value)
                            physicalPerson.WorkPermitTo = DateTime.Parse(reader["WorkPermitTo"].ToString());


                        physicalPerson.Active = bool.Parse(reader["Active"].ToString());
                        physicalPerson.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            physicalPerson.CreatedBy = new User();
                            physicalPerson.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPerson.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            physicalPerson.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            physicalPerson.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            physicalPerson.Company = new Company();
                            physicalPerson.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPerson.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            physicalPerson.Company.Name = reader["CompanyName"].ToString();
                        }

                        PhysicalPersons.Add(physicalPerson);
                    }
                }
            }

            return PhysicalPersons;

            //return context.PhysicalPersons
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Include(x => x.Country)
            //    .Include(x => x.Region)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.City)
            //    .Include(x => x.PassportCountry)
            //    .Include(x => x.PassportCity)
            //    .Include(x => x.ResidenceCountry)
            //    .Include(x => x.ResidenceCity)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.PhysicalPersons
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhysicalPerson))
                    .Select(x => x.Entity as PhysicalPerson))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "RAD-00001";
            else
            {
                string activeCode = context.PhysicalPersons
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhysicalPerson))
                        .Select(x => x.Entity as PhysicalPerson))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("RAD-", ""));
                    return "RAD-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        #endregion

        #region CREATE methods

        public PhysicalPerson Create(PhysicalPerson physicalPerson)
        {
            if (context.PhysicalPersons.Where(x => x.Identifier != null && x.Identifier == physicalPerson.Identifier).Count() == 0)
            {
                physicalPerson.Id = 0;

                physicalPerson.Code = GetNewCodeValue(physicalPerson.CompanyId ?? 0);
                physicalPerson.Active = true;

                physicalPerson.UpdatedAt = DateTime.Now;
                physicalPerson.CreatedAt = DateTime.Now;

                context.PhysicalPersons.Add(physicalPerson);
                return physicalPerson;
            }
            else
            {
                // Load item that will be updated
                PhysicalPerson dbEntry = context.PhysicalPersons
                    .FirstOrDefault(x => x.Identifier == physicalPerson.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = physicalPerson.CompanyId ?? null;
                    dbEntry.CreatedById = physicalPerson.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = physicalPerson.Code;
                    dbEntry.PhysicalPersonCode = physicalPerson.PhysicalPersonCode;
                    dbEntry.Name = physicalPerson.Name;
                    dbEntry.SurName = physicalPerson.SurName;

                    dbEntry.ConstructionSiteCode = physicalPerson.ConstructionSiteCode;
                    dbEntry.ConstructionSiteName = physicalPerson.ConstructionSiteName;

                    dbEntry.DateOfBirth = physicalPerson.DateOfBirth;
                    dbEntry.Gender = physicalPerson.Gender;
                    dbEntry.CountryId = physicalPerson.CountryId;
                    dbEntry.RegionId = physicalPerson.RegionId;
                    dbEntry.MunicipalityId = physicalPerson.MunicipalityId;
                    dbEntry.CityId = physicalPerson.CityId;
                    dbEntry.Address = physicalPerson.Address;

                    dbEntry.Passport = physicalPerson.Passport;
                    dbEntry.VisaFrom = physicalPerson.VisaFrom;
                    dbEntry.VisaTo = physicalPerson.VisaTo;

                    dbEntry.PassportCountryId = physicalPerson.PassportCountryId;
                    dbEntry.PassportCityId = physicalPerson.PassportCityId;

                    dbEntry.ResidenceCountryId = physicalPerson.ResidenceCountryId;
                    dbEntry.ResidenceCityId = physicalPerson.ResidenceCityId;
                    dbEntry.ResidenceAddress = physicalPerson.ResidenceAddress;

                    dbEntry.EmbassyDate = physicalPerson.EmbassyDate;
                    dbEntry.VisaDate = physicalPerson.VisaDate;
                    dbEntry.VisaValidFrom = physicalPerson.VisaValidFrom;
                    dbEntry.VisaValidTo = physicalPerson.VisaValidTo;
                    dbEntry.WorkPermitFrom = physicalPerson.WorkPermitFrom;
                    dbEntry.WorkPermitTo = physicalPerson.WorkPermitTo;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public PhysicalPerson Delete(Guid identifier)
        {
            // Load PhysicalPerson that will be deleted
            PhysicalPerson dbEntry = context.PhysicalPersons
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

        #endregion
    }
}
