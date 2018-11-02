using Newtonsoft.Json;
using ServiceInterfaces.ViewModels.Banks;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Sectors;
using ServiceInterfaces.ViewModels.ConstructionSites;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Text;

namespace ApiExtension.Sender
{
    public class WebClientWithTimeout : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest wr = base.GetWebRequest(address);
            wr.Timeout = 30000; // timeout in milliseconds (ms)
            return wr;
        }
    }

    public class WpfApiHandler
    {
        //public static string BaseApiUrl = "http://192.168.0.3:5001/api";
        //public static string BaseApiUrl = "http://192.168.0.10:5001/api";
        //public static string BaseApiUrl = "http://192.168.0.250:5001/api";
        //public static string BaseApiUrl = "http://192.168.0.250:5002/api";// for 2018
        //public static string BaseApiUrl = "http://192.168.0.10:5002/api";// for 2018
        //private static string BaseApiUrl = "http://212.200.54.246:5001/api";
        public static string BaseApiUrl = "http://localhost:5001/api";
        //private static string BaseApiUrl = "http://localhost:22632/api";// Zeljko Tepic localhost address

        private static bool _baseAddressLoadedFromConfig = false;

        public static string ApiMethod { get; set; }
        public static string ObjectType { get; set; }

        public static Dictionary<Type, string> routes = new Dictionary<Type, string>()
        {
            #region Common

                #region Companies
                { typeof(CompanyViewModel), "Company" },
                { typeof(List<CompanyViewModel>), "Company" },
                #endregion
            
                #region Identity
                { typeof(UserViewModel), "User" },
                { typeof(List<UserViewModel>), "User" },

                { typeof(AuthenticationViewModel), "Authentication" },
                { typeof(List<AuthenticationViewModel>), "Authentication" },
                #endregion

                #region BusinessPartners

                { typeof(BusinessPartnerViewModel), "BusinessPartner" },
                { typeof(List<BusinessPartnerViewModel>), "BusinessPartner" },

                { typeof(BusinessPartnerTypeViewModel), "BusinessPartnerType" },
                { typeof(List<BusinessPartnerTypeViewModel>), "BusinessPartnerType" },

                { typeof(BusinessPartnerPhoneViewModel), "BusinessPartnerPhone" },
                { typeof(List<BusinessPartnerPhoneViewModel>), "BusinessPartnerPhone" },

                { typeof(BusinessPartnerOrganizationUnitViewModel), "BusinessPartnerOrganizationUnit" },
                { typeof(List<BusinessPartnerOrganizationUnitViewModel>), "BusinessPartnerOrganizationUnit" },

                { typeof(BusinessPartnerLocationViewModel), "BusinessPartnerLocation" },
                { typeof(List<BusinessPartnerLocationViewModel>), "BusinessPartnerLocation" },

                #endregion

                #region OutputInvoices

                { typeof(OutputInvoiceViewModel), "OutputInvoice" },
                { typeof(List<OutputInvoiceViewModel>), "OutputInvoice" },

                #endregion

                #region Locations

                { typeof(CityViewModel), "City" },
                { typeof(List<CityViewModel>), "City" },

                { typeof(RegionViewModel), "Region" },
                { typeof(List<RegionViewModel>), "Region" },

                { typeof(MunicipalityViewModel), "Municipality" },
                { typeof(List<MunicipalityViewModel>), "Municipality" },

                { typeof(CountryViewModel), "Country" },
                { typeof(List<CountryViewModel>), "Country" },

                #endregion

			    #region Sectors

                { typeof(SectorViewModel), "Sector" },
                { typeof(List<SectorViewModel>), "Sector" },

                { typeof(AgencyViewModel), "Agency" },
                { typeof(List<AgencyViewModel>), "Agency" },

                #endregion

                #region Professions

                { typeof(ProfessionViewModel), "Profession" },
                { typeof(List<ProfessionViewModel>), "Profession" },

			#endregion

                #region Employees
                
                { typeof(EmployeeViewModel), "Employee" },
                { typeof(List<EmployeeViewModel>), "Employee" },

                { typeof(EmployeeItemViewModel), "EmployeeItem" },
                { typeof(List<EmployeeItemViewModel>), "EmployeeItem" },

                { typeof(FamilyMemberViewModel), "FamilyMember" },
                { typeof(List<FamilyMemberViewModel>), "FamilyMember" },

                { typeof(EmployeeLicenceItemViewModel), "EmployeeLicence" },
                { typeof(List<EmployeeLicenceItemViewModel>), "EmployeeLicence" },

                { typeof(EmployeeProfessionItemViewModel), "EmployeeProfession" },
                { typeof(List<EmployeeProfessionItemViewModel>), "EmployeeProfession" },

                { typeof(EmployeeByConstructionSiteViewModel), "EmployeeByConstructionSite" },
                { typeof(List<EmployeeByConstructionSiteViewModel>), "EmployeeByConstructionSite" },

                { typeof(EmployeeByConstructionSiteHistoryViewModel), "EmployeeByConstructionSiteHistory" },
                { typeof(List<EmployeeByConstructionSiteHistoryViewModel>), "EmployeeByConstructionSiteHistory" },

			    #endregion
                
                #region ConstructionSites

                { typeof(ConstructionSiteViewModel), "ConstructionSite" },
                { typeof(List<ConstructionSiteViewModel>), "ConstructionSite" },

			    #endregion


            #endregion

			#region Banks
			{ typeof(BankViewModel), "Bank" },
                { typeof(List<BankViewModel>), "Bank" },
                #endregion

			#region LicenceTypes
				 { typeof(LicenceTypeViewModel), "LicenceType" },
                { typeof(List<LicenceTypeViewModel>), "LicenceType" },
#endregion
		};

        static WpfApiHandler()
        {
            WpfApiHandler._loadBaseAddressFromConfigFile();
        }

        private static void _loadBaseAddressFromConfigFile()
        {
            if (WpfApiHandler._baseAddressLoadedFromConfig == false)
            {
                try
                {
                    //var appConfig = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
                    //string tmpBaseAddress = appConfig.AppSettings.Settings["BaseApiUrl"].Value;

                    ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                    map.ExeConfigFilename = Assembly.GetEntryAssembly().Location + ".config";
                    Configuration libConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                    AppSettingsSection section = (libConfig.GetSection("appSettings") as AppSettingsSection);
                    string tmpBaseAddress = section.Settings["BaseApiUrl"]?.Value;

                    if (!string.IsNullOrWhiteSpace(tmpBaseAddress))
                    {
                        WpfApiHandler.BaseApiUrl = tmpBaseAddress;
                    }

                }
                catch (Exception ex)
                {

                }
                WpfApiHandler._baseAddressLoadedFromConfig = true;
            }
        }

        public static string GetReportUrlByType(object obj, int id)
        {
            var realObject = routes[obj.GetType()];

            string apiUrl = BaseApiUrl + "/" + realObject + "/Get" + realObject + "ForReport?id=" + id;

            return apiUrl;
        }

        public static Tout SendToApi<Tin, Tout>(Tin obj, string Action = null)
        {
            return SendToApi<Tin, Tin, Tout>(obj, Action);
        }

        public static Tout SendToApi<Tin, TIdentificator, Tout>(Tin obj, string Action = null)
        {
            Tout response = default(Tout);
            string Type = routes[typeof(TIdentificator)];
            if (Type != null)
                ObjectType = Type;
            if (Action != null)
                ApiMethod = Action;

            PropertyInfo messageProp;
            PropertyInfo successProp;
            string apiUrl = BaseApiUrl + "/" + ObjectType + "/" + ApiMethod;

            string jsonResponse = "";
            var values = JsonConvert.SerializeObject(obj,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    //StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            using (var client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                client.Proxy = null;
                Uri uri;
                try
                {
                    Uri.TryCreate(apiUrl, UriKind.Absolute, out uri);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    //Dictionary<string, string> nvc = new Dictionary<string, string>();
                    //nvc.Add("Object", values);
                    //nvc.Add("Type", ObjectType);
                    //nvc.Add("Method", ApiMethod);

                    //client.UploadStringAsync(uri, "POST", values);

                    //client.UploadValuesCompleted += Client_UploadValuesCompleted;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    jsonResponse = client.UploadString(apiUrl, "POST", values);

                    response = JsonConvert.DeserializeObject<Tout>(jsonResponse);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Dogodila se greska: " + ex.Message);
                }
            }
            if (response == null)
            {
                response = Activator.CreateInstance<Tout>();
                messageProp = response.GetType().GetProperty("Message");
                successProp = response.GetType().GetProperty("Success");
                messageProp.SetValue(response, "Dogodila se greška pri učitavanju podataka, proverite mrežu!", null);
                successProp.SetValue(response, false, null);
            }
            return response;
        }

        public static T GetFromApi<T>(string Action, Dictionary<string, string> parameters = null)
        {
            return GetFromApi<T, T>(Action, parameters);
        }

        public static Tout GetFromApi<Tin, Tout>(string Action, Dictionary<string, string> parameters = null)
        {
            return GetFromApi<Tin, Tin, Tout>(Action, parameters);
        }

        public static Tout GetFromApi<Tin, TIdentificator, Tout>(string Action, Dictionary<string, string> parameters = null)
        {
            PropertyInfo messageProp;
            PropertyInfo successProp;// = response.GetType().GetProperty("Success");


            Object lockObject = new object();

            Tout response = default(Tout);
            string Type = routes[typeof(TIdentificator)];
            if (Type != null)
                ObjectType = Type;
            if (Action != null)
                ApiMethod = Action;

            string apiUrl = BaseApiUrl + "/" + ObjectType + "/" + ApiMethod + "?";
            string jsonResponse = "";
            using (WebClient client = new WebClientWithTimeout() { Encoding = Encoding.UTF8 })
            {
                client.Proxy = null;
                Uri uri;
                try
                {
                    //Uri.TryCreate(apiUrl, UriKind.Absolute, out uri);
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var item in parameters)
                        {
                            if (!string.IsNullOrEmpty(item.Key))
                            {
                                apiUrl += item.Key + "=" + item.Value + "&";
                            }
                        }
                    }

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";

                    //client.UploadStringAsync(uri, "POST", values);

                    //client.UploadValuesCompleted += Client_UploadValuesCompleted;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    jsonResponse = client.DownloadString(apiUrl); //, "Get"); //, JsonConvert.SerializeObject(parameters));
                    response = JsonConvert.DeserializeObject<Tout>(jsonResponse);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Dogodila se greska: " + ex.Message);
                }
            }
            if (response == null)
            {
                response = Activator.CreateInstance<Tout>();
                messageProp = response.GetType().GetProperty("Message");
                successProp = response.GetType().GetProperty("Success");
                messageProp.SetValue(response, "Dogodila se greška pri učitavanju podataka, proverite mrežu!", null);
                successProp.SetValue(response, false, null);
            }
            return response;
        }
        /*
        private static void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            if(e.Result != null)
            {
                string tmp = Encoding.UTF8.GetString(e.Result);
                MessageBox.Show(tmp);
            }
        }*/
    }
}
