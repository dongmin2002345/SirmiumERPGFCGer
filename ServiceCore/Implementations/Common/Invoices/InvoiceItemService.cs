using DataMapper.Mappers.Common.Invoices;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Invoices;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Invoices
{
    public class InvoiceItemService : IInvoiceItemService
    {
        IUnitOfWork unitOfWork;

        public InvoiceItemService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public InvoiceItemListResponse Sync(SyncInvoiceItemRequest request)
        {
            InvoiceItemListResponse response = new InvoiceItemListResponse();
            try
            {
                response.InvoiceItems = new List<InvoiceItemViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.InvoiceItems.AddRange(unitOfWork.GetInvoiceItemRepository()
                        .GetInvoiceItemsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToInvoiceItemViewModelList() ?? new List<InvoiceItemViewModel>());
                }
                else
                {
                    response.InvoiceItems.AddRange(unitOfWork.GetInvoiceItemRepository()
                        .GetInvoiceItems(request.CompanyId)
                        ?.ConvertToInvoiceItemViewModelList() ?? new List<InvoiceItemViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.InvoiceItems = new List<InvoiceItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
