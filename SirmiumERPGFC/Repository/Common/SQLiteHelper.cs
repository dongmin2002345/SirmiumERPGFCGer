using Microsoft.Data.Sqlite;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Sectors;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Common
{
    public class SQLiteHelper
    {
        public static void AddColumnIfNotExists(string table, string column, string columnType)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                // make sure table exists
                String commandString = string.Format("SELECT COUNT(*) AS CNTREC FROM pragma_table_info('{0}') WHERE name='{1}'", table, column);
                SqliteCommand sqlCommand = new SqliteCommand(commandString, db);
                var reader = sqlCommand.ExecuteReader();

                bool hascol = true;
                if (reader.Read())
                {
                    //does column exists?
                    hascol = reader.GetInt32(0) > 0;
                    reader.Close();
                }
                reader.Close();

                if (!hascol)
                {
                    commandString = string.Format("ALTER TABLE '{0}' ADD COLUMN '{1}' {2};", table, column, columnType);
                    sqlCommand = new SqliteCommand(commandString, db);
                    reader = sqlCommand.ExecuteReader();
                }
            }
        }


        public static int GetInt(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter++;
                return 0;
            }
            else
                return query.GetInt32(counter++);
        }

        public static int? GetIntNullable(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter++;
                return null;
            }
            else
                return query.GetInt32(counter++);
        }

        public static Guid GetGuid(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter++;
                return Guid.Empty;
            }
            else
                return query.GetGuid(counter++);
        }

        public static string GetString(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter++;
                return "";
            }
            else
                return query.GetString(counter++);
        }

        public static DateTime GetDateTime(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter++;
                return DateTime.Now;
            }
            else
                return query.GetDateTime(counter++);
        }

        public static DateTime? GetDateTimeNullable(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter++;
                return null;
            }
            else
                return query.GetDateTime(counter++);
        }

        public static double GetDouble(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter++;
                return 0;
            }
            else
                return query.GetDouble(counter++);
        }

        public static double? GetDoubleNullable(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter++;
                return null;
            }
            else
                return query.GetDouble(counter++);
        }

        public static decimal GetDecimal(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter++;
                return 0;
            }
            else
                return query.GetDecimal(counter++);
        }

        public static decimal? GetDecimalNullable(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter++;
                return null;
            }
            else
                return query.GetDecimal(counter++);
        }

        public static bool GetBoolean(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter++;
                return false;
            }
            else
                return query.GetBoolean(counter++);
        }

        public static UserViewModel GetCreatedBy(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 2;
                return null;
            }
            else
                return new UserViewModel()
                {
                    Id = query.GetInt32(counter++),
                    FullName = query.GetString(counter++)
                };
        }

        public static CompanyViewModel GetCompany(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 2;
                return null;
            }
            else
                return new CompanyViewModel()
                {
                    Id = query.GetInt32(counter++),
                    CompanyName = query.GetString(counter++)
                };
        }

        public static CityViewModel GetCity(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new CityViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    ZipCode = query.GetString(counter++),
                    Name = query.GetString(counter++)
                };
        }

        public static RegionViewModel GetRegion(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new RegionViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    RegionCode = query.GetString(counter++),
                    Name = query.GetString(counter++)
                };
        }


        public static MunicipalityViewModel GetMunicipality(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new MunicipalityViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    MunicipalityCode = query.GetString(counter++),
                    Name = query.GetString(counter++)
                };
        }

        public static CountryViewModel GetCountry(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new CountryViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.GetString(counter++),
                    Name = query.GetString(counter++)
                };
        }

        public static SectorViewModel GetSector(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new SectorViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.GetString(counter++),
                    Name = query.GetString(counter++)
                };
        }

        public static AgencyViewModel GetAgency(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new AgencyViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.GetString(counter++),
                    Name = query.GetString(counter++)
                };
        }

        public static FamilyMemberViewModel GetFamilyMember(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new FamilyMemberViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.GetString(counter++),
                    Name = query.GetString(counter++)
                };
        }

        public static BusinessPartnerViewModel GetBusinessPartner(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new BusinessPartnerViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.GetString(counter++),
                    Name = query.GetString(counter++)//query.IsDBNull(counter++)? query.GetString(counter-1) : 
                };
        }

        public static BusinessPartnerTypeViewModel GetBusinessPartnerType(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new BusinessPartnerTypeViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.GetString(counter++),
                    Name = query.GetString(counter++)
                };
        }

        public static EmployeeViewModel GetEmployee(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
            {
                var viewModel = new EmployeeViewModel();
                viewModel.Id = query.GetInt32(counter++);
                viewModel.Identifier = query.GetGuid(counter++);
                viewModel.Code = query.IsDBNull(counter++) ? "" : query.GetString(counter-1);
                viewModel.Name = query.GetString(counter++);


                return viewModel;
            }
        }

        public static ProfessionViewModel GetProfession(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new ProfessionViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.GetString(counter++),
                    Name = query.GetString(counter++)
                };
        }

        public static LicenceTypeViewModel GetLicence(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 5;
                return null;
            }
            else
                return new LicenceTypeViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.GetString(counter++),
                    Category = query.GetString(counter++),
                    Description = query.GetString(counter++)
                };
        }
    }
}

