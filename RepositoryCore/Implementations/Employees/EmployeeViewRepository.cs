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
                "CityId, CityIdentifier, CityCode, CityName, " +
                "Address, " +
                "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                "PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
                "Passport, VisaFrom, VisaTo, " +
                "ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                "ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, " +
                "ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployees " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

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

                        if (reader["CountryId"] != null)
                        {
                            employee.Country = new Country();
                            employee.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employee.Country.Code = reader["CountryCode"].ToString();
                            employee.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["RegionId"] != null) //"RegionId, RegionIdentifier, RegionCode, RegionName, " +
                        {
                            employee.Region = new Region();
                            employee.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            employee.Region.Code = reader["RegionCode"].ToString();
                            employee.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["MunicipalityId"] != null) /*"MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +*/
                        {
                            employee.Municipality = new Municipality();
                            employee.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            employee.Municipality.Code = reader["MunicipalityCode"].ToString();
                            employee.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["CityId"] != null) // "CityId, CityIdentifier, CityCode, CityName, " +
                        {
                            employee.City = new City();
                            employee.CityId = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Id = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            employee.City.Code = reader["CityCode"].ToString();
                            employee.City.Name = reader["CityName"].ToString();
                        }


                        if (reader["Address"] != null)
                            employee.Address = reader["Address"].ToString();


                        if (reader["PassportCountryId"] != null) // "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                        {
                            employee.PassportCountry = new Country();
                            employee.PassportCountryId = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Id = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Identifier = Guid.Parse(reader["PassportCountryIdentifier"].ToString());
                            employee.PassportCountry.Code = reader["PassportCountryCode"].ToString();
                            employee.PassportCountry.Name = reader["PassportCountryName"].ToString();
                        }

                        if (reader["PassportCityId"] != null) // "PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
                        {
                            employee.PassportCity = new City();
                            employee.PassportCityId = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Id = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Identifier = Guid.Parse(reader["PassportCityIdentifier"].ToString());
                            employee.PassportCity.Code = reader["PassportCityCode"].ToString();
                            employee.PassportCity.Name = reader["PassportCityName"].ToString();
                        }

                        //"Passport, VisaFrom, Code, VisaTo, " +

                        if (reader["Passport"] != null)
                            employee.Passport = reader["Passport"].ToString();
                        if (reader["VisaFrom"] != null)
                            employee.VisaFrom = DateTime.Parse(reader["VisaFrom"].ToString());
                        if (reader["VisaTo"] != null)
                            employee.VisaTo = DateTime.Parse(reader["VisaTo"].ToString());

                        
                        if (reader["ResidenceCountryId"] != null) //"ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                        {
                            employee.ResidenceCountry = new Country();
                            employee.ResidenceCountryId = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Id = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Identifier = Guid.Parse(reader["ResidenceCountryIdentifier"].ToString());
                            employee.ResidenceCountry.Code = reader["ResidenceCountryCode"].ToString();
                            employee.ResidenceCountry.Name = reader["ResidenceCountryName"].ToString();
                        }

                        if (reader["ResidenceCityId"] != null) //"ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, " + 
                        {
                            employee.ResidenceCity = new City();
                            employee.ResidenceCityId = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Id = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Identifier = Guid.Parse(reader["ResidenceCityIdentifier"].ToString());
                            employee.ResidenceCity.Code = reader["ResidenceCityCode"].ToString();
                            employee.ResidenceCity.Name = reader["ResidenceCityName"].ToString();
                        }

                        //"ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +

                        if (reader["ResidenceAddress"] != null)
                            employee.ResidenceAddress = reader["ResidenceAddress"].ToString();
                        if (reader["EmbassyDate"] != null)
                            employee.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["VisaDate"] != null)
                            employee.VisaDate = DateTime.Parse(reader["VisaDate"].ToString());
                        if (reader["VisaValidFrom"] != null)
                            employee.VisaValidFrom = DateTime.Parse(reader["VisaValidFrom"].ToString());
                        if (reader["VisaValidTo"] != null)
                            employee.VisaValidTo = DateTime.Parse(reader["VisaValidTo"].ToString());
                        if (reader["WorkPermitFrom"] != null)
                            employee.WorkPermitFrom = DateTime.Parse(reader["WorkPermitFrom"].ToString());
                        if (reader["WorkPermitTo"] != null)
                            employee.WorkPermitTo = DateTime.Parse(reader["WorkPermitTo"].ToString());

                        //"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +

                        employee.Active = bool.Parse(reader["Active"].ToString());
                        employee.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employee.CreatedBy = new User();
                            employee.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employee.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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
                "CityId, CityIdentifier, CityCode, CityName, " +
                "Address, " +
                "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                "PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
                "Passport, VisaFrom, VisaTo, " +
                "ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                "ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, " +
                "ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vEmployees " +
                "WHERE EmployeeId = @EmployeeId AND Active = 1;";

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

                        if (reader["CountryId"] != null)
                        {
                            employee.Country = new Country();
                            employee.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employee.Country.Code = reader["CountryCode"].ToString();
                            employee.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["RegionId"] != null) //"RegionId, RegionIdentifier, RegionCode, RegionName, " +
                        {
                            employee.Region = new Region();
                            employee.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            employee.Region.Code = reader["RegionCode"].ToString();
                            employee.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["MunicipalityId"] != null) /*"MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +*/
                        {
                            employee.Municipality = new Municipality();
                            employee.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            employee.Municipality.Code = reader["MunicipalityCode"].ToString();
                            employee.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["CityId"] != null) // "CityId, CityIdentifier, CityCode, CityName, " +
                        {
                            employee.City = new City();
                            employee.CityId = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Id = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            employee.City.Code = reader["CityCode"].ToString();
                            employee.City.Name = reader["CityName"].ToString();
                        }


                        if (reader["Address"] != null)
                            employee.Address = reader["Address"].ToString();


                        if (reader["PassportCountryId"] != null) // "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                        {
                            employee.PassportCountry = new Country();
                            employee.PassportCountryId = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Id = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Identifier = Guid.Parse(reader["PassportCountryIdentifier"].ToString());
                            employee.PassportCountry.Code = reader["PassportCountryCode"].ToString();
                            employee.PassportCountry.Name = reader["PassportCountryName"].ToString();
                        }

                        if (reader["PassportCityId"] != null) // "PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
                        {
                            employee.PassportCity = new City();
                            employee.PassportCityId = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Id = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Identifier = Guid.Parse(reader["PassportCityIdentifier"].ToString());
                            employee.PassportCity.Code = reader["PassportCityCode"].ToString();
                            employee.PassportCity.Name = reader["PassportCityName"].ToString();
                        }

                        //"Passport, VisaFrom, Code, VisaTo, " +

                        if (reader["Passport"] != null)
                            employee.Passport = reader["Passport"].ToString();
                        if (reader["VisaFrom"] != null)
                            employee.VisaFrom = DateTime.Parse(reader["VisaFrom"].ToString());
                        if (reader["VisaTo"] != null)
                            employee.VisaTo = DateTime.Parse(reader["VisaTo"].ToString());


                        if (reader["ResidenceCountryId"] != null) //"ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                        {
                            employee.ResidenceCountry = new Country();
                            employee.ResidenceCountryId = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Id = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Identifier = Guid.Parse(reader["ResidenceCountryIdentifier"].ToString());
                            employee.ResidenceCountry.Code = reader["ResidenceCountryCode"].ToString();
                            employee.ResidenceCountry.Name = reader["ResidenceCountryName"].ToString();
                        }

                        if (reader["ResidenceCityId"] != null) //"ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, " + 
                        {
                            employee.ResidenceCity = new City();
                            employee.ResidenceCityId = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Id = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Identifier = Guid.Parse(reader["ResidenceCityIdentifier"].ToString());
                            employee.ResidenceCity.Code = reader["ResidenceCityCode"].ToString();
                            employee.ResidenceCity.Name = reader["ResidenceCityName"].ToString();
                        }

                        //"ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +

                        if (reader["ResidenceAddress"] != null)
                            employee.ResidenceAddress = reader["ResidenceAddress"].ToString();
                        if (reader["EmbassyDate"] != null)
                            employee.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["VisaDate"] != null)
                            employee.VisaDate = DateTime.Parse(reader["VisaDate"].ToString());
                        if (reader["VisaValidFrom"] != null)
                            employee.VisaValidFrom = DateTime.Parse(reader["VisaValidFrom"].ToString());
                        if (reader["VisaValidTo"] != null)
                            employee.VisaValidTo = DateTime.Parse(reader["VisaValidTo"].ToString());
                        if (reader["WorkPermitFrom"] != null)
                            employee.WorkPermitFrom = DateTime.Parse(reader["WorkPermitFrom"].ToString());
                        if (reader["WorkPermitTo"] != null)
                            employee.WorkPermitTo = DateTime.Parse(reader["WorkPermitTo"].ToString());

                        //"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +

                        employee.Active = bool.Parse(reader["Active"].ToString());
                        employee.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employee.CreatedBy = new User();
                            employee.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employee.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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

        public List<Employee> GetEmployeesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Employee> Employees = new List<Employee>();

            string queryString =
                "SELECT EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeEmployeeCode, EmployeeName, EmployeeSurName, EmployeeConstructionSiteCode, EmployeeConstructionSiteName, DateOfBirth, Gender, " +
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
                "FROM vEmployees " +
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
                        employee.DateOfBirth = DateTime.Parse(reader["DateOfBirth"]?.ToString());
                        employee.Gender = Int32.Parse(reader["Gender"].ToString());

                        if (reader["CountryId"] != null)
                        {
                            employee.Country = new Country();
                            employee.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            employee.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            employee.Country.Code = reader["CountryCode"].ToString();
                            employee.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["RegionId"] != null) //"RegionId, RegionIdentifier, RegionCode, RegionName, " +
                        {
                            employee.Region = new Region();
                            employee.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            employee.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            employee.Region.Code = reader["RegionCode"].ToString();
                            employee.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["MunicipalityId"] != null) /*"MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +*/
                        {
                            employee.Municipality = new Municipality();
                            employee.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            employee.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            employee.Municipality.Code = reader["MunicipalityCode"].ToString();
                            employee.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        if (reader["CityId"] != null) // "CityId, CityIdentifier, CityCode, CityName, " +
                        {
                            employee.City = new City();
                            employee.CityId = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Id = Int32.Parse(reader["CityId"].ToString());
                            employee.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                            employee.City.Code = reader["CityCode"].ToString();
                            employee.City.Name = reader["CityName"].ToString();
                        }


                        if (reader["Address"] != null)
                            employee.Address = reader["Address"].ToString();


                        if (reader["PassportCountryId"] != null) // "PassportCountryId, PassportCountryIdentifier, PassportCountryCode, PassportCountryName, " +
                        {
                            employee.PassportCountry = new Country();
                            employee.PassportCountryId = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Id = Int32.Parse(reader["PassportCountryId"].ToString());
                            employee.PassportCountry.Identifier = Guid.Parse(reader["PassportCountryIdentifier"].ToString());
                            employee.PassportCountry.Code = reader["PassportCountryCode"].ToString();
                            employee.PassportCountry.Name = reader["PassportCountryName"].ToString();
                        }

                        if (reader["PassportCityId"] != null) // "PassportCityId, PassportCityIdentifier, PassportCityCode, PassportCityName, " +
                        {
                            employee.PassportCity = new City();
                            employee.PassportCityId = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Id = Int32.Parse(reader["PassportCityId"].ToString());
                            employee.PassportCity.Identifier = Guid.Parse(reader["PassportCityIdentifier"].ToString());
                            employee.PassportCity.Code = reader["PassportCityCode"].ToString();
                            employee.PassportCity.Name = reader["PassportCityName"].ToString();
                        }

                        //"Passport, VisaFrom, Code, VisaTo, " +

                        if (reader["Passport"] != null)
                            employee.Passport = reader["Passport"].ToString();
                        if (reader["VisaFrom"] != null)
                            employee.VisaFrom = DateTime.Parse(reader["VisaFrom"].ToString());
                        if (reader["VisaTo"] != null)
                            employee.VisaTo = DateTime.Parse(reader["VisaTo"].ToString());


                        if (reader["ResidenceCountryId"] != null) //"ResidenceCountryId, ResidenceCountryIdentifier, ResidenceCountryCode, ResidenceCountryName, " +
                        {
                            employee.ResidenceCountry = new Country();
                            employee.ResidenceCountryId = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Id = Int32.Parse(reader["ResidenceCountryId"].ToString());
                            employee.ResidenceCountry.Identifier = Guid.Parse(reader["ResidenceCountryIdentifier"].ToString());
                            employee.ResidenceCountry.Code = reader["ResidenceCountryCode"].ToString();
                            employee.ResidenceCountry.Name = reader["ResidenceCountryName"].ToString();
                        }

                        if (reader["ResidenceCityId"] != null) //"ResidenceCityId, ResidenceCityIdentifier, ResidenceCityCode, ResidenceCityName, " + 
                        {
                            employee.ResidenceCity = new City();
                            employee.ResidenceCityId = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Id = Int32.Parse(reader["ResidenceCityId"].ToString());
                            employee.ResidenceCity.Identifier = Guid.Parse(reader["ResidenceCityIdentifier"].ToString());
                            employee.ResidenceCity.Code = reader["ResidenceCityCode"].ToString();
                            employee.ResidenceCity.Name = reader["ResidenceCityName"].ToString();
                        }

                        //"ResidenceAddress, EmbassyDate, VisaDate, VisaValidFrom, VisaValidTo, WorkPermitFrom, WorkPermitTo, " +

                        if (reader["ResidenceAddress"] != null)
                            employee.ResidenceAddress = reader["ResidenceAddress"].ToString();
                        if (reader["EmbassyDate"] != null)
                            employee.EmbassyDate = DateTime.Parse(reader["EmbassyDate"].ToString());
                        if (reader["VisaDate"] != null)
                            employee.VisaDate = DateTime.Parse(reader["VisaDate"].ToString());
                        if (reader["VisaValidFrom"] != null)
                            employee.VisaValidFrom = DateTime.Parse(reader["VisaValidFrom"].ToString());
                        if (reader["VisaValidTo"] != null)
                            employee.VisaValidTo = DateTime.Parse(reader["VisaValidTo"].ToString());
                        if (reader["WorkPermitFrom"] != null)
                            employee.WorkPermitFrom = DateTime.Parse(reader["WorkPermitFrom"].ToString());
                        if (reader["WorkPermitTo"] != null)
                            employee.WorkPermitTo = DateTime.Parse(reader["WorkPermitTo"].ToString());

                        //"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +

                        employee.Active = bool.Parse(reader["Active"].ToString());
                        employee.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            employee.CreatedBy = new User();
                            employee.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            employee.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            employee.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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
