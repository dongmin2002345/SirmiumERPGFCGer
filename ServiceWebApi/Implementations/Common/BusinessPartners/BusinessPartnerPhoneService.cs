﻿using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerPhoneService : IBusinessPartnerPhoneService
    {
        public BusinessPartnerPhoneListResponse Sync(SyncBusinessPartnerPhoneRequest request)
        {
            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerPhoneRequest, BusinessPartnerPhoneViewModel, BusinessPartnerPhoneListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerPhones = new List<BusinessPartnerPhoneViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
