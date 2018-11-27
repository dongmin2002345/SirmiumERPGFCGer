using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Abstractions.Common.Professions;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Abstractions.Common.TaxAdministrations;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Banks;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Employees;

namespace SirmiumERPWeb.Controllers.Administrations
{
    public class SeedDataController : Controller
    {
        ICountryService countryService;
        IRegionService regionService;
        IMunicipalityService municipalityService;
        ICityService cityService;
        IBankService bankService;
        IProfessionService professionService;
        ILicenceTypeService licenceTypeService;
        ISectorService sectorService;
        IAgencyService agencyService;
        ITaxAdministrationService taxAdministrationService;

        public SeedDataController(IServiceProvider provider)
        {
            countryService = provider.GetRequiredService<ICountryService>();
            regionService = provider.GetRequiredService<IRegionService>();
            municipalityService = provider.GetRequiredService<IMunicipalityService>();
            cityService = provider.GetRequiredService<ICityService>();
            bankService = provider.GetRequiredService<IBankService>();
            professionService = provider.GetRequiredService<IProfessionService>();
            licenceTypeService = provider.GetRequiredService<ILicenceTypeService>();
            sectorService = provider.GetRequiredService<ISectorService>();
            agencyService = provider.GetRequiredService<IAgencyService>();
            taxAdministrationService = provider.GetRequiredService<ITaxAdministrationService>();
        }

        [HttpPost]
        public JsonResult SeedCountries([FromBody]List<CountryViewModel> countries)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                foreach (var item in countries)
                {
                    countryService.Create(item);
                }
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult SeedBanks([FromBody]List<BankViewModel> banks)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                List<CountryViewModel> countries = countryService.GetCountries(banks.FirstOrDefault()?.Company?.Id ?? 0).Countries;
                foreach (var item in banks ?? new List<BankViewModel>())
                {
                    string mark = item?.Country.Mark;
                    if (mark == "NEM")
                        mark = "DEU";
                    item.Country = countries.FirstOrDefault(x => x.Mark == mark);
                    bankService.Create(item);
                }
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult SeedRegions([FromBody]List<RegionViewModel> regions)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                List<CountryViewModel> countries = countryService.GetCountries(regions.FirstOrDefault()?.Company?.Id ?? 0).Countries;
                foreach (var item in regions)
                {
                    string mark = item?.Country.Mark;
                    if (mark == "NEM")
                        mark = "DEU";
                    item.Country = countries.FirstOrDefault(x => x.Mark == mark);
                    regionService.Create(item);
                }
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult SeedMunicipalities([FromBody]List<MunicipalityViewModel> municipalities)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                int companyId = municipalities.FirstOrDefault()?.Company?.Id ?? 0;
                List<CountryViewModel> countries = countryService.GetCountries(companyId).Countries;
                List<RegionViewModel> regions = regionService.GetRegions(companyId).Regions;
                foreach (var item in municipalities)
                {
                    string mark = item?.Country.Mark;
                    if (mark == "NEM")
                        mark = "DEU";
                    item.Country = countries.FirstOrDefault(x => x.Mark == mark);
                    item.Region = regions.FirstOrDefault(x => x.RegionCode == item.Region.RegionCode);
                    municipalityService.Create(item);
                }
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult SeedCities([FromBody]List<CityViewModel> cities)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                int companyId = cities.FirstOrDefault()?.Company?.Id ?? 0;
                List<CountryViewModel> countries = countryService.GetCountries(companyId).Countries;
                List<RegionViewModel> regions = regionService.GetRegions(companyId).Regions;
                List<MunicipalityViewModel> municipalities = municipalityService.GetMunicipalities(companyId).Municipalities;
                foreach (var item in cities)
                {
                    string mark = item?.Country.Mark;
                    if (mark == "NEM")
                        mark = "DEU";
                    item.Country = countries.FirstOrDefault(x => x.Mark == mark);
                    item.Region = regions.FirstOrDefault(x => x.RegionCode == item.Region.RegionCode);
                    item.Municipality = municipalities.FirstOrDefault(x => x.MunicipalityCode == item.Municipality.MunicipalityCode);
                    cityService.Create(item);
                }
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult SeedProfessions([FromBody]List<ProfessionViewModel> professions)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                List<CountryViewModel> countries = countryService.GetCountries(professions.FirstOrDefault()?.Company?.Id ?? 0).Countries;
                foreach (var item in professions ?? new List<ProfessionViewModel>())
                {
                    string mark = item?.Country.Mark;
                    if (mark == "NEM")
                        mark = "DEU";
                    item.Country = countries.FirstOrDefault(x => x.Mark == mark);
                    professionService.Create(item);
                }
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult SeedLicenceTypes([FromBody]List<LicenceTypeViewModel> licenceTypes)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                foreach (var item in licenceTypes ?? new List<LicenceTypeViewModel>())
                {
                    licenceTypeService.Create(item);
                }
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult SeedSectors([FromBody]List<SectorViewModel> sectors)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                List<CountryViewModel> countries = countryService.GetCountries(sectors.FirstOrDefault()?.Company?.Id ?? 0).Countries;
                foreach (var item in sectors ?? new List<SectorViewModel>())
                {
                    string mark = item?.Country.Mark;
                    if (mark == "NEM")
                        mark = "DEU";
                    item.Country = countries.FirstOrDefault(x => x.Mark == mark);
                    sectorService.Create(item);
                }
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult SeedAgencies([FromBody]List<AgencyViewModel> agencies)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                int companyId = agencies.FirstOrDefault()?.Company?.Id ?? 0;
                List<CountryViewModel> countries = countryService.GetCountries(companyId).Countries;
                List<SectorViewModel> sectors = sectorService.GetSectors(companyId).Sectors;
                foreach (var item in agencies ?? new List<AgencyViewModel>())
                {
                    string mark = item?.Country.Mark;
                    if (mark == "NEM")
                        mark = "DEU";
                    item.Country = countries.FirstOrDefault(x => x.Mark == mark);
                    item.Sector = sectors.FirstOrDefault(x => x.SecondCode == item.Sector.SecondCode);
                    agencyService.Create(item);
                }
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult SeedTaxAdministrations([FromBody]List<TaxAdministrationViewModel> taxAdministrations)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                int companyId = taxAdministrations.FirstOrDefault()?.Company?.Id ?? 0;
                List<CityViewModel> cities = cityService.GetCities(companyId).Cities;
                List<BankViewModel> bank = bankService.GetBanks(companyId).Banks;
                foreach (var item in taxAdministrations ?? new List<TaxAdministrationViewModel>())
                {
                    item.City = cities.FirstOrDefault(x => x.Name == item.City?.Name);
                    item.Bank1 = bank.FirstOrDefault(x => x.Name == item.Bank1?.Name);
                    item.Bank2 = bank.FirstOrDefault(x => x.Name == item.Bank2?.Name);
                    taxAdministrationService.Create(item);
                }
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

    }
}