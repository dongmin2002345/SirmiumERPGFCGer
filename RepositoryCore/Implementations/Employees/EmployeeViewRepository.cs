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

namespace RepositoryCore.Implementations.Employees
{
    public class EmployeeViewRepository : IEmployeeRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public EmployeeViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

        }

        #region GET methods

        public List<Employee> GetEmployees(int companyId)
        {
            List<Employee> Employees = new List<Employee>();

            string queryString =
                "SELECT EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeEmployeeCode, EmployeeName, EmployeeSurName, EmployeeConstructionSiteCode, EmployeeConstructionSiteName, DateOfBirth, Gender, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "Address, " +
                "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                "PassportCityId, PassportCityIdentifier, PassportCityZipCode, PassportCityName, " +
                "Passport, PassportMup, VisaFrom, VisaTo, " +
                "ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                "ResidenceCityId, ResidenceCityIdentifier, ResidenceCityZipCode, ResidenceCityName, " +
                "ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployees " +
                "WHERE CompanyId = @CompanyId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Employee employee;
                    while (reader.Read())
                    {
                        employee = new Employee();
                        employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                        employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                        employee.Code = reader["EmployeeCode"]?.ToString();
                        employee.EmployeeCode = reader["EmployeeEmployeeCode"]?.ToString();
                        employee.Name = reader["EmployeeName"].ToString();
                        employee.SurName = reader["EmployeeSurName"].ToString();
                        employee.ConstructionSiteCode = reader["EmployeeConstructionSiteCode"]?.ToString();
                        employee.ConstructionSiteName = reader["EmployeeConstructionSiteName"]?.ToString();
                        employee.DateOfBirth = DateTime.Parse( reader["DateOfBirth"]?.ToString());
                        employee.Gender = Int32.Parse(reader["Gender"].ToString());

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            employee.Country = new Country();
                            employee.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employee.Country.Mark = reader["CountryCode"].ToString();
                            employee.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["RegionId"] != DBNull.Value) //"RegionId, RegionIdentifier, RegionCode, RegionName, " +
                        {
                            employee.Region = new Region();
                            employee.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            employee.Region.RegionCode = reader["RegionCode"].ToString();
                            employee.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value) /*"MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +*/
                        {
                            employee.Municipality = new Municipality();
                            employee.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            employee.Municipality.MunicipalityCode = reader["MunicipalityCode"].ToString();
                            employee.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value) // "CityId, CityIdentifier, CityZipCode, CityName, " +
                        {
                            employee.City = new City();
                            employee.CityId = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Id = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            employee.City.ZipCode = reader["CityZipCode"].ToString();
                            employee.City.Name = reader["CityName"].ToString();
                        }


                        if (reader["Address"] != DBNull.Value)
                            employee.Address = reader["Address"].ToString();


                        if (reader["PassportCountryId"] != DBNull.Value) // "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                        {
                            employee.PassportCountry = new Country();
                            employee.PassportCountryId = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Id = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Identifier = Guid.Parse(reader["PassportCountryIdentifier"].ToString());
                            employee.PassportCountry.Mark = reader["PassportCountryCode"].ToString();
                            employee.PassportCountry.Name = reader["PassportCountryName"].ToString();
                        }

                        if (reader["PassportCityId"] != DBNull.Value) // "PassportCityId, PassportCityIdentifier, PassportCityZipCode, PassportCityName, " +
                        {
                            employee.PassportCity = new City();
                            employee.PassportCityId = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Id = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Identifier = Guid.Parse(reader["PassportCityIdentifier"].ToString());
                            employee.PassportCity.ZipCode = reader["PassportCityZipCode"].ToString();
                            employee.PassportCity.Name = reader["PassportCityName"].ToString();
                        }

                        //"Passport, VisaFrom, Code, VisaTo, " +

                        if (reader["Passport"] != DBNull.Value)
                            employee.Passport = reader["Passport"].ToString();
                        if (reader["PassportMup"] != DBNull.Value)
                            employee.PassportMup = reader["PassportMup"].ToString();
                        if (reader["VisaFrom"] != DBNull.Value)
                            employee.VisaFrom = DateTime.Parse(reader["VisaFrom"].ToString());
                        if (reader["VisaTo"] != DBNull.Value)
                            employee.VisaTo = DateTime.Parse(reader["VisaTo"].ToString());

                        
                        if (reader["ResidenceCountryId"] != DBNull.Value) //"ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                        {
                            employee.ResidenceCountry = new Country();
                            employee.ResidenceCountryId = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Id = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Identifier = Guid.Parse(reader["ResidenceCountryIdentifier"].ToString());
                            employee.ResidenceCountry.Mark = reader["ResidenceCountryCode"].ToString();
                            employee.ResidenceCountry.Name = reader["ResidenceCountryName"].ToString();
                        }

                        if (reader["ResidenceCityId"] != DBNull.Value) //"ResidenceCityId, ResidenceCityIdentifier, ResidenceCityZipCode, ResidenceCityName, " + 
                        {
                            employee.ResidenceCity = new City();
                            employee.ResidenceCityId = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Id = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Identifier = Guid.Parse(reader["ResidenceCityIdentifier"].ToString());
                            employee.ResidenceCity.ZipCode = reader["ResidenceCityZipCode"].ToString();
                            employee.ResidenceCity.Name = reader["ResidenceCityName"].ToString();
                        }

                        //"ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +

                        if (reader["ResidenceAddress"] != DBNull.Value)
                            employee.ResidenceAddress = reader["ResidenceAddress"].ToString();
                        if (reader["EmbassyDate"] != DBNull.Value)
                            employee.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["VisaDate"] != DBNull.Value)
                            employee.VisaDate = DateTime.Parse(reader["VisaDate"].ToString());
                        if (reader["VisaValidFrom"] != DBNull.Value)
                            employee.VisaValidFrom = DateTime.Parse(reader["VisaValidFrom"].ToString());
                        if (reader["VisaValidTo"] != DBNull.Value)
                            employee.VisaValidTo = DateTime.Parse(reader["VisaValidTo"].ToString());
                        if (reader["WorkPermitFrom"] != DBNull.Value)
                            employee.WorkPermitFrom = DateTime.Parse(reader["WorkPermitFrom"].ToString());
                        if (reader["WorkPermitTo"] != DBNull.Value)
                            employee.WorkPermitTo = DateTime.Parse(reader["WorkPermitTo"].ToString());

                        //"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +

                        employee.Active = bool.Parse(reader["Active"].ToString());
                        employee.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employee.CreatedBy = new User();
                            employee.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employee.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employee.Company = new Company();
                            employee.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employee.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employee.Company.Name = reader["CompanyName"].ToString();
                        }

                        Employees.Add(employee);
                    }
                }
            }

            return Employees;

            //return context.Employees
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

        public Employee GetEmployee(int employeeId)
        {
            Employee employee = new Employee();

            string queryString =
                "SELECT EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeEmployeeCode, EmployeeName, EmployeeSurName, EmployeeConstructionSiteCode, EmployeeConstructionSiteName, DateOfBirth, Gender, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "Address, " +
                "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                "PassportCityId, PassportCityIdentifier, PassportCityZipCode, PassportCityName, " +
                "Passport, PassportMup, VisaFrom, VisaTo, " +
                "ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                "ResidenceCityId, ResidenceCityIdentifier, ResidenceCityZipCode, ResidenceCityName, " +
                "ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployees " +
                "WHERE EmployeeId = @EmployeeId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@EmployeeId", employeeId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                        employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                        employee.Code = reader["EmployeeCode"]?.ToString();
                        employee.EmployeeCode = reader["EmployeeEmployeeCode"]?.ToString();
                        employee.Name = reader["EmployeeName"].ToString();
                        employee.SurName = reader["EmployeeSurName"].ToString();
                        employee.ConstructionSiteCode = reader["EmployeeConstructionSiteCode"]?.ToString();
                        employee.ConstructionSiteName = reader["EmployeeConstructionSiteName"]?.ToString();
                        employee.DateOfBirth = DateTime.Parse(reader["DateOfBirth"]?.ToString());
                        employee.Gender = Int32.Parse(reader["Gender"].ToString());

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            employee.Country = new Country();
                            employee.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employee.Country.Mark = reader["CountryCode"].ToString();
                            employee.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["RegionId"] != DBNull.Value) //"RegionId, RegionIdentifier, RegionCode, RegionName, " +
                        {
                            employee.Region = new Region();
                            employee.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            employee.Region.RegionCode = reader["RegionCode"].ToString();
                            employee.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value) /*"MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +*/
                        {
                            employee.Municipality = new Municipality();
                            employee.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            employee.Municipality.MunicipalityCode = reader["MunicipalityCode"].ToString();
                            employee.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value) // "CityId, CityIdentifier, CityZipCode, CityName, " +
                        {
                            employee.City = new City();
                            employee.CityId = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Id = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            employee.City.ZipCode = reader["CityZipCode"].ToString();
                            employee.City.Name = reader["CityName"].ToString();
                        }


                        if (reader["Address"] != null)
                            employee.Address = reader["Address"].ToString();


                        if (reader["PassportCountryId"] != DBNull.Value) // "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                        {
                            employee.PassportCountry = new Country();
                            employee.PassportCountryId = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Id = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Identifier = Guid.Parse(reader["PassportCountryIdentifier"].ToString());
                            employee.PassportCountry.Mark = reader["PassportCountryCode"].ToString();
                            employee.PassportCountry.Name = reader["PassportCountryName"].ToString();
                        }

                        if (reader["PassportCityId"] != DBNull.Value) // "PassportCityId, PassportCityIdentifier, PassportCityZipCode, PassportCityName, " +
                        {
                            employee.PassportCity = new City();
                            employee.PassportCityId = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Id = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Identifier = Guid.Parse(reader["PassportCityIdentifier"].ToString());
                            employee.PassportCity.ZipCode = reader["PassportCityZipCode"].ToString();
                            employee.PassportCity.Name = reader["PassportCityName"].ToString();
                        }

                        //"Passport, VisaFrom, Code, VisaTo, " +

                        if (reader["Passport"] != DBNull.Value)
                            employee.Passport = reader["Passport"].ToString();
                        if (reader["PassportMup"] != DBNull.Value)
                            employee.PassportMup = reader["PassportMup"].ToString();
                        if (reader["VisaFrom"] != DBNull.Value)
                            employee.VisaFrom = DateTime.Parse(reader["VisaFrom"].ToString());
                        if (reader["VisaTo"] != DBNull.Value)
                            employee.VisaTo = DateTime.Parse(reader["VisaTo"].ToString());


                        if (reader["ResidenceCountryId"] != DBNull.Value) //"ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                        {
                            employee.ResidenceCountry = new Country();
                            employee.ResidenceCountryId = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Id = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Identifier = Guid.Parse(reader["ResidenceCountryIdentifier"].ToString());
                            employee.ResidenceCountry.Mark = reader["ResidenceCountryCode"].ToString();
                            employee.ResidenceCountry.Name = reader["ResidenceCountryName"].ToString();
                        }

                        if (reader["ResidenceCityId"] != DBNull.Value) //"ResidenceCityId, ResidenceCityIdentifier, ResidenceCityZipCode, ResidenceCityName, " + 
                        {
                            employee.ResidenceCity = new City();
                            employee.ResidenceCityId = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Id = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Identifier = Guid.Parse(reader["ResidenceCityIdentifier"].ToString());
                            employee.ResidenceCity.ZipCode = reader["ResidenceCityZipCode"].ToString();
                            employee.ResidenceCity.Name = reader["ResidenceCityName"].ToString();
                        }

                        //"ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +

                        if (reader["ResidenceAddress"] != DBNull.Value)
                            employee.ResidenceAddress = reader["ResidenceAddress"].ToString();
                        if (reader["EmbassyDate"] != DBNull.Value)
                            employee.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["VisaDate"] != DBNull.Value)
                            employee.VisaDate = DateTime.Parse(reader["VisaDate"].ToString());
                        if (reader["VisaValidFrom"] != DBNull.Value)
                            employee.VisaValidFrom = DateTime.Parse(reader["VisaValidFrom"].ToString());
                        if (reader["VisaValidTo"] != DBNull.Value)
                            employee.VisaValidTo = DateTime.Parse(reader["VisaValidTo"].ToString());
                        if (reader["WorkPermitFrom"] != DBNull.Value)
                            employee.WorkPermitFrom = DateTime.Parse(reader["WorkPermitFrom"].ToString());
                        if (reader["WorkPermitTo"] != DBNull.Value)
                            employee.WorkPermitTo = DateTime.Parse(reader["WorkPermitTo"].ToString());

                        //"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +

                        employee.Active = bool.Parse(reader["Active"].ToString());
                        employee.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employee.CreatedBy = new User();
                            employee.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employee.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employee.Company = new Company();
                            employee.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employee.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employee.Company.Name = reader["CompanyName"].ToString();
                        }

                    }
                }
            }

            return employee;

            //return context.Employees
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
            //    .FirstOrDefault(x => x.Id == employeeId && x.Active == true);
        }

        public Employee GetEmployeeEntity(int employeeId)
        {
            return context.Employees
                .Include(x => x.Country)
                .Include(x => x.Region)
                .Include(x => x.Municipality)
                .Include(x => x.City)
                .Include(x => x.PassportCountry)
                .Include(x => x.PassportCity)
                .Include(x => x.ResidenceCountry)
                .Include(x => x.ResidenceCity)
                .FirstOrDefault(x => x.Id == employeeId && x.Active == true);
        }

        public List<Employee> GetEmployeesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Employee> Employees = new List<Employee>();

            string queryString =
                "SELECT EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeEmployeeCode, EmployeeName, EmployeeSurName, EmployeeConstructionSiteCode, EmployeeConstructionSiteName, DateOfBirth, Gender, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "CityId, CityIdentifier, CityZipCode, CityName, " +
                "Address, " +
                "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                "PassportCityId, PassportCityIdentifier, PassportCityZipCode, PassportCityName, " +
                "Passport, PassportMup, VisaFrom, VisaTo, " +
                "ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                "ResidenceCityId, ResidenceCityIdentifier, ResidenceCityZipCode, ResidenceCityName, " +
                "ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployees " +
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
                    Employee employee;
                    while (reader.Read())
                    {
                        employee = new Employee();
                        employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                        employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                        if (reader["EmployeeCode"] != DBNull.Value)
                            employee.Code = reader["EmployeeCode"]?.ToString();
                        if (reader["EmployeeEmployeeCode"] != DBNull.Value)
                            employee.EmployeeCode = reader["EmployeeEmployeeCode"]?.ToString();
                        if (reader["EmployeeName"] != DBNull.Value)
                            employee.Name = reader["EmployeeName"].ToString();
                        if (reader["EmployeeSurName"] != DBNull.Value)
                            employee.SurName = reader["EmployeeSurName"].ToString();
                        if (reader["EmployeeConstructionSiteCode"] != DBNull.Value)
                            employee.ConstructionSiteCode = reader["EmployeeConstructionSiteCode"]?.ToString();
                        if (reader["EmployeeConstructionSiteName"] != DBNull.Value)
                            employee.ConstructionSiteName = reader["EmployeeConstructionSiteName"]?.ToString();
                        if (reader["DateOfBirth"] != DBNull.Value)
                            employee.DateOfBirth = DateTime.Parse(reader["DateOfBirth"]?.ToString());
                        if (reader["Gender"] != DBNull.Value)
                            employee.Gender = Int32.Parse(reader["Gender"].ToString());

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            employee.Country = new Country();
                            employee.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employee.Country.Mark = reader["CountryCode"].ToString();
                            employee.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["RegionId"] != DBNull.Value) //"RegionId, RegionIdentifier, RegionCode, RegionName, " +
                        {
                            employee.Region = new Region();
                            employee.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            employee.Region.RegionCode = reader["RegionCode"].ToString();
                            employee.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value) /*"MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +*/
                        {
                            employee.Municipality = new Municipality();
                            employee.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            employee.Municipality.MunicipalityCode = reader["MunicipalityCode"].ToString();
                            employee.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["CityId"] != DBNull.Value) // "CityId, CityIdentifier, CityZipCode, CityName, " +
                        {
                            employee.City = new City();
                            employee.CityId = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Id = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            employee.City.ZipCode = reader["CityZipCode"].ToString();
                            employee.City.Name = reader["CityName"].ToString();
                        }


                        if (reader["Address"] != DBNull.Value)
                            employee.Address = reader["Address"].ToString();


                        if (reader["PassportCountryId"] != DBNull.Value) // "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                        {
                            employee.PassportCountry = new Country();
                            employee.PassportCountryId = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Id = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Identifier = Guid.Parse(reader["PassportCountryIdentifier"].ToString());
                            employee.PassportCountry.Mark = reader["PassportCountryCode"].ToString();
                            employee.PassportCountry.Name = reader["PassportCountryName"].ToString();
                        }

                        if (reader["PassportCityId"] != DBNull.Value) // "PassportCityId, PassportCityIdentifier, PassportCityZipCode, PassportCityName, " +
                        {
                            employee.PassportCity = new City();
                            employee.PassportCityId = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Id = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Identifier = Guid.Parse(reader["PassportCityIdentifier"].ToString());
                            employee.PassportCity.ZipCode = reader["PassportCityZipCode"].ToString();
                            employee.PassportCity.Name = reader["PassportCityName"].ToString();
                        }

                        //"Passport, VisaFrom, Code, VisaTo, " +

                        if (reader["Passport"] != DBNull.Value)
                            employee.Passport = reader["Passport"].ToString();
                        if (reader["PassportMup"] != DBNull.Value)
                            employee.PassportMup = reader["PassportMup"].ToString();
                        if (reader["VisaFrom"] != DBNull.Value)
                            employee.VisaFrom = DateTime.Parse(reader["VisaFrom"].ToString());
                        if (reader["VisaTo"] != DBNull.Value)
                            employee.VisaTo = DateTime.Parse(reader["VisaTo"].ToString());


                        if (reader["ResidenceCountryId"] != DBNull.Value) //"ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                        {
                            employee.ResidenceCountry = new Country();
                            employee.ResidenceCountryId = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Id = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Identifier = Guid.Parse(reader["ResidenceCountryIdentifier"].ToString());
                            employee.ResidenceCountry.Mark = reader["ResidenceCountryCode"].ToString();
                            employee.ResidenceCountry.Name = reader["ResidenceCountryName"].ToString();
                        }

                        if (reader["ResidenceCityId"] != DBNull.Value) //"ResidenceCityId, ResidenceCityIdentifier, ResidenceCityZipCode, ResidenceCityName, " + 
                        {
                            employee.ResidenceCity = new City();
                            employee.ResidenceCityId = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Id = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Identifier = Guid.Parse(reader["ResidenceCityIdentifier"].ToString());
                            employee.ResidenceCity.ZipCode = reader["ResidenceCityZipCode"].ToString();
                            employee.ResidenceCity.Name = reader["ResidenceCityName"].ToString();
                        }

                        //"ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +

                        if (reader["ResidenceAddress"] != DBNull.Value)
                            employee.ResidenceAddress = reader["ResidenceAddress"].ToString();
                        if (reader["EmbassyDate"] != DBNull.Value)
                            employee.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["VisaDate"] != DBNull.Value)
                            employee.VisaDate = DateTime.Parse(reader["VisaDate"].ToString());
                        if (reader["VisaValidFrom"] != DBNull.Value)
                            employee.VisaValidFrom = DateTime.Parse(reader["VisaValidFrom"].ToString());
                        if (reader["VisaValidTo"] != DBNull.Value)
                            employee.VisaValidTo = DateTime.Parse(reader["VisaValidTo"].ToString());
                        if (reader["WorkPermitFrom"] != DBNull.Value)
                            employee.WorkPermitFrom = DateTime.Parse(reader["WorkPermitFrom"].ToString());
                        if (reader["WorkPermitTo"] != DBNull.Value)
                            employee.WorkPermitTo = DateTime.Parse(reader["WorkPermitTo"].ToString());

                        //"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +

                        employee.Active = bool.Parse(reader["Active"].ToString());
                        employee.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            employee.CreatedBy = new User();
                            employee.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employee.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            employee.Company = new Company();
                            employee.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            employee.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            employee.Company.Name = reader["CompanyName"].ToString();
                        }

                        Employees.Add(employee);
                    }
                }
            }

            return Employees;

            //return context.Employees
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
            int count = context.Employees
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Employee))
                    .Select(x => x.Entity as Employee))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "RAD-00001";
            else
            {
                string activeCode = context.Employees
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Employee))
                        .Select(x => x.Entity as Employee))
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

        public Employee Create(Employee employee)
        {
            if (context.Employees.Where(x => x.Identifier != null && x.Identifier == employee.Identifier).Count() == 0)
            {
                if (context.Employees.Where(x => x.EmployeeCode == employee.EmployeeCode).Count() > 0)
                    throw new Exception("Radnik sa datom šifrom već postoji u bazi! / Arbeiter mit dem angegebenen Code ist bereits in der Datenbank vorhanden!");

                employee.Id = 0;

                employee.Code = GetNewCodeValue(employee.CompanyId ?? 0);
                employee.Active = true;

                employee.UpdatedAt = DateTime.Now;
                employee.CreatedAt = DateTime.Now;

                context.Employees.Add(employee);
                return employee;
            }
            else
            {
                // Load item that will be updated
                Employee dbEntry = context.Employees
                    .FirstOrDefault(x => x.Identifier == employee.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = employee.CompanyId ?? null;
                    dbEntry.CreatedById = employee.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = employee.Code;
                    dbEntry.EmployeeCode = employee.EmployeeCode;
                    dbEntry.Name = employee.Name;
                    dbEntry.SurName = employee.SurName;

                    dbEntry.ConstructionSiteCode = employee.ConstructionSiteCode;
                    dbEntry.ConstructionSiteName = employee.ConstructionSiteName;

                    dbEntry.DateOfBirth = employee.DateOfBirth;
                    dbEntry.Gender = employee.Gender;
                    dbEntry.CountryId = employee.CountryId;
                    dbEntry.RegionId = employee.RegionId;
                    dbEntry.MunicipalityId = employee.MunicipalityId;
                    dbEntry.CityId = employee.CityId;
                    dbEntry.Address = employee.Address;

                    dbEntry.Passport = employee.Passport;
                    dbEntry.PassportMup = employee.PassportMup;
                    dbEntry.VisaFrom = employee.VisaFrom;
                    dbEntry.VisaTo = employee.VisaTo;

                    dbEntry.PassportCountryId = employee.PassportCountryId;
                    dbEntry.PassportCityId = employee.PassportCityId;

                    dbEntry.ResidenceCountryId = employee.ResidenceCountryId;
                    dbEntry.ResidenceCityId = employee.ResidenceCityId;
                    dbEntry.ResidenceAddress = employee.ResidenceAddress;

                    dbEntry.EmbassyDate = employee.EmbassyDate;
                    dbEntry.VisaDate = employee.VisaDate;
                    dbEntry.VisaValidFrom = employee.VisaValidFrom;
                    dbEntry.VisaValidTo = employee.VisaValidTo;
                    dbEntry.WorkPermitFrom = employee.WorkPermitFrom;
                    dbEntry.WorkPermitTo = employee.WorkPermitTo;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Employee Delete(Guid identifier)
        {
            // Load Employee that will be deleted
            Employee dbEntry = context.Employees
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
