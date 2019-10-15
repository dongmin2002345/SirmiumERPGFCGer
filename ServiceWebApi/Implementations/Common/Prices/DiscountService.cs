using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Prices
{
    public class DiscountService : IDiscountService
    {
        public DiscountListResponse GetDiscounts(int companyId)
        {
            DiscountListResponse response = new DiscountListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<DiscountViewModel>, DiscountListResponse>("GetDiscounts", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Discounts = new List<DiscountViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DiscountResponse Create(DiscountViewModel discount)
        {
            DiscountResponse response = new DiscountResponse();
            try
            {
                response = WpfApiHandler.SendToApi<DiscountViewModel, DiscountResponse>(discount, "Create");
            }
            catch (Exception ex)
            {
                response.Discount = new DiscountViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DiscountResponse Delete(Guid identifier)
        {
            DiscountResponse response = new DiscountResponse();
            try
            {
                DiscountViewModel discount = new DiscountViewModel();
                discount.Identifier = identifier;
                response = WpfApiHandler.SendToApi<DiscountViewModel, DiscountResponse>(discount, "Delete");
            }
            catch (Exception ex)
            {
                response.Discount = new DiscountViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DiscountListResponse Sync(SyncDiscountRequest request)
        {
            DiscountListResponse response = new DiscountListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncDiscountRequest, DiscountViewModel, DiscountListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Discounts = new List<DiscountViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

