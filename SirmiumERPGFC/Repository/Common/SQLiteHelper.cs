using Microsoft.Data.Sqlite;
using ServiceInterfaces.ViewModels.Banks;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Common.ToDos;
using ServiceInterfaces.ViewModels.ConstructionSites;
using ServiceInterfaces.ViewModels.Employees;
using ServiceInterfaces.ViewModels.Statuses;
using ServiceInterfaces.ViewModels.Vats;
using System;

namespace SirmiumERPGFC.Repository.Common
{
    public class SQLiteHelper
    {
        public static string SqLiteTableName = "Filename=SirmiumERPGFC.db";

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
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    ZipCode = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1)
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
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    RegionCode = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1)
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
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    MunicipalityCode = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1)
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
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1)
                };
        }

        public static PhonebookViewModel GetPhonebook(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new PhonebookViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1)
                };
        }

        public static ServiceDeliveryViewModel GetServiceDelivery(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 5;
                return null;
            }
            else
                return new ServiceDeliveryViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    URL = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1)
                };
        }

        public static InputInvoiceViewModel GetInputInvoice(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 3;
                return null;
            }
            else
                return new InputInvoiceViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                };
        }

        public static ShipmentViewModel GetShipment(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 3;
                return null;
            }
            else
                return new ShipmentViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                };
        }

        public static ShipmentViewModel GetFullShipment(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new ShipmentViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                   
                    ShipmentNumber = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                };
        }

        public static OutputInvoiceViewModel GetOutputInvoice(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 3;
                return null;
            }
            else
                return new OutputInvoiceViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                };
        }

        public static BankViewModel GetBank(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new BankViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1)
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
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1)
                };
        }

        public static BusinessPartnerViewModel GetBusinessPartner(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 6;
                return null;
            }
            else
                return new BusinessPartnerViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    InternalCode = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    NameGer = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1)
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
                counter += 5;
                return null;
            }
            else
            {
                var viewModel = new EmployeeViewModel();
                viewModel.Id = query.GetInt32(counter++);
                viewModel.Identifier = query.GetGuid(counter++);
                viewModel.Code = query.IsDBNull(counter++) ? "" : query.GetString(counter-1);
                viewModel.Name = query.GetString(counter++);
                viewModel.EmployeeCode = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);


                return viewModel;
            }
        }

        public static PhysicalPersonViewModel GetPhysicalPerson(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
            {
                var viewModel = new PhysicalPersonViewModel();
                viewModel.Id = query.GetInt32(counter++);
                viewModel.Identifier = query.GetGuid(counter++);
                viewModel.Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);
                viewModel.Name = query.GetString(counter++);


                return viewModel;
            }
        }


        public static ConstructionSiteViewModel GetConstructionSite(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
            {
                var viewModel = new ConstructionSiteViewModel();
                viewModel.Id = query.GetInt32(counter++);
                viewModel.Identifier = query.GetGuid(counter++);
                viewModel.Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);
                viewModel.Name = query.GetString(counter++);


                return viewModel;
            }
        }

        public static ConstructionSiteViewModel GetConstructionSiteWithInternalCode(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 5;
                return null;
            }
            else
            {
                var viewModel = new ConstructionSiteViewModel();
                viewModel.Id = query.GetInt32(counter++);
                viewModel.Identifier = query.GetGuid(counter++);
                viewModel.Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);
                viewModel.Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);
                viewModel.InternalCode = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);

                return viewModel;
            }
        }

        public static ToDoStatusViewModel GetToDoStatus(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
            {
                var viewModel = new ToDoStatusViewModel();
                viewModel.Id = query.GetInt32(counter++);
                viewModel.Identifier = query.GetGuid(counter++);
                viewModel.Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);
                viewModel.Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);
                return viewModel;
            };
        }

        public static UserViewModel GetUser(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 5;
                return null;
            }
            else
                {
                     var viewModel = new UserViewModel();
                     viewModel.Id = query.GetInt32(counter++);
                     viewModel.Identifier = query.GetGuid(counter++);
                     viewModel.Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);
                     viewModel.FirstName = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);
                     viewModel.LastName = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);

                return viewModel;
                };
        }

        public static UserViewModel GetCompanyUser(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 3;
                return null;
            }
            else
                return new UserViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    FullName = query.GetString(counter++)
                };
        }

        public static TaxAdministrationViewModel GetTaxAdministration(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
            {
                var viewModel = new TaxAdministrationViewModel();
                viewModel.Id = query.GetInt32(counter++);
                viewModel.Identifier = query.GetGuid(counter++);
                viewModel.Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1);
                viewModel.Name = query.GetString(counter++);


                return viewModel;
            }
        }

        public static ProfessionViewModel GetProfession(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 5;
                return null;
            }
            else
                return new ProfessionViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    Name = query.GetString(counter++),
                    SecondCode = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
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

        public static StatusViewModel GetStatus(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 4;
                return null;
            }
            else
                return new StatusViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.IsDBNull(counter++) ? Guid.Empty : query.GetGuid(counter - 1),
                    Code = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1),
                    Name = query.IsDBNull(counter++) ? "" : query.GetString(counter - 1)
                };
        }

        public static VatViewModel GetVat(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 5;
                return null;
            }
            else
                return new VatViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.GetString(counter++),
                    Description = query.GetString(counter++),
                    Amount = query.GetDecimal(counter++),
                };
        }

        public static DiscountViewModel GetDiscount(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 5;
                return null;
            }
            else
                return new DiscountViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.GetString(counter++),
                    Name = query.GetString(counter++),
                    Amount = query.GetDecimal(counter++),
                };
        }

        public static InvoiceViewModel GetInvoice(SqliteDataReader query, ref int counter)
        {
            if (query.IsDBNull(counter))
            {
                counter += 3;
                return null;
            }
            else
                return new InvoiceViewModel()
                {
                    Id = query.GetInt32(counter++),
                    Identifier = query.GetGuid(counter++),
                    Code = query.GetString(counter++),
                };
        }
    }
}

