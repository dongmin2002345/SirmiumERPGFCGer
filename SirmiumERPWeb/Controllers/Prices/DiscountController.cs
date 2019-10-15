using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;

namespace SirmiumERPWeb.Controllers.Prices
{
    public class DiscountController : Controller
    {
        IDiscountService discountService { get; set; }

        public DiscountController(IServiceProvider provider)
        {
            discountService = provider.GetRequiredService<IDiscountService>();
        }

        [HttpGet]
        public JsonResult GetDiscounts(int companyId)
        {
            DiscountListResponse response = new DiscountListResponse();
            try
            {
                response = discountService.GetDiscounts(companyId);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }

            JsonResult result = Json(response, new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize
            });
            return result;
        }

        [HttpPost]
        public JsonResult Create([FromBody] DiscountViewModel c)
        {
            DiscountResponse response = new DiscountResponse();
            try
            {
                response = this.discountService.Create(c);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }

            JsonResult result = Json(response, new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize
            });
            return result;
        }

        [HttpPost]
        public JsonResult Delete([FromBody]DiscountViewModel discount)
        {
            DiscountResponse response = new DiscountResponse();
            try
            {
                response = this.discountService.Delete(discount.Identifier);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }

            JsonResult result = Json(response, new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize
            });
            return result;
        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncDiscountRequest request)
        {
            DiscountListResponse response = new DiscountListResponse();
            try
            {
                response = this.discountService.Sync(request);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            JsonResult result = Json(response, new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize
            });
            return result;
        }
    }
}
