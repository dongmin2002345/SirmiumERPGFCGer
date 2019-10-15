using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Vats;
using ServiceInterfaces.Messages.Vats;
using ServiceInterfaces.ViewModels.Vats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Vats
{
    public class VatService : IVatService
    {
        public VatListResponse GetVats(int companyId)
        {
            VatListResponse response = new VatListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<VatViewModel>, VatListResponse>("GetVats", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Vats = new List<VatViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public VatResponse Create(VatViewModel Vat)
        {
            VatResponse response = new VatResponse();
            try
            {
                response = WpfApiHandler.SendToApi<VatViewModel, VatResponse>(Vat, "Create");
            }
            catch (Exception ex)
            {
                response.Vat = new VatViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public VatResponse Delete(Guid identifier)
        {
            VatResponse response = new VatResponse();
            try
            {
                VatViewModel Vat = new VatViewModel();
                Vat.Identifier = identifier;
                response = WpfApiHandler.SendToApi<VatViewModel, VatResponse>(Vat, "Delete");
            }
            catch (Exception ex)
            {
                response.Vat = new VatViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public VatListResponse Sync(SyncVatRequest request)
        {
            VatListResponse response = new VatListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncVatRequest, VatViewModel, VatListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Vats = new List<VatViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

