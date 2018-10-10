using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Locations;

namespace SirmiumERPWeb.Controllers.Administrations
{
    public class SeedDataController : Controller
    {
        ICountryService countryService;

        public SeedDataController(IServiceProvider provider)
        {
            countryService = provider.GetRequiredService<ICountryService>();
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

    }
}