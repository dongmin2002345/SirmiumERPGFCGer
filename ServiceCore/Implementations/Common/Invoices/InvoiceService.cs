using DataMapper.Mappers.Common.Invoices;
using DomainCore.Common.Invoices;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Invoices;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Implementations.Common.Invoices
{
    public class InvoiceService : IInvoiceService
    {
        IUnitOfWork unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public InvoiceListResponse GetInvoices(int companyId)
        {
            InvoiceListResponse response = new InvoiceListResponse();
            try
            {
                response.Invoices = unitOfWork.GetInvoiceRepository().GetInvoices(companyId)
               .ConvertToInvoiceViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Invoices = new List<InvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public InvoiceListResponse GetInvoicesNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            InvoiceListResponse response = new InvoiceListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Invoices = unitOfWork.GetInvoiceRepository()
                        .GetInvoicesNewerThan(companyId, (DateTime)lastUpdateTime)
                        .ConvertToInvoiceViewModelList();
                }
                else
                {
                    response.Invoices = unitOfWork.GetInvoiceRepository()
                        .GetInvoices(companyId)
                        .ConvertToInvoiceViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Invoices = new List<InvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public InvoiceResponse Create(InvoiceViewModel invoice)
        {
            InvoiceResponse response = new InvoiceResponse();
            try
            {
                //Backup items
                List<InvoiceItemViewModel> invoiceNotes = invoice
                    .InvoiceItems?.ToList() ?? new List<InvoiceItemViewModel>();
                invoice.InvoiceItems = null;


                Invoice createdInvoice = unitOfWork.GetInvoiceRepository()
                    .Create(invoice.ConvertToInvoice());

                // Update items
                if (invoiceNotes != null && invoiceNotes.Count > 0)
                {
                    foreach (InvoiceItemViewModel item in invoiceNotes
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<InvoiceItemViewModel>())
                    {
                        item.Invoice = new InvoiceViewModel() { Id = createdInvoice.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetInvoiceItemRepository().Create(item.ConvertToInvoiceItem());
                    }

                    foreach (InvoiceItemViewModel item in invoiceNotes
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<InvoiceItemViewModel>())
                    {
                        item.Invoice = new InvoiceViewModel() { Id = createdInvoice.Id };
                        unitOfWork.GetInvoiceItemRepository().Create(item.ConvertToInvoiceItem());

                        unitOfWork.GetInvoiceItemRepository().Delete(item.Identifier);
                    }

                }
                unitOfWork.Save();

                response.Invoice = createdInvoice.ConvertToInvoiceViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Invoice = new InvoiceViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public InvoiceResponse Delete(Guid identifier)
        {
            InvoiceResponse response = new InvoiceResponse();
            try
            {
                Invoice deletedInvoice = unitOfWork.GetInvoiceRepository().Delete(identifier);

                unitOfWork.Save();

                response.Invoice = deletedInvoice.ConvertToInvoiceViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Invoice = new InvoiceViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public InvoiceListResponse Sync(SyncInvoiceRequest request)
        {
            InvoiceListResponse response = new InvoiceListResponse();
            try
            {
                response.Invoices = new List<InvoiceViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Invoices.AddRange(unitOfWork.GetInvoiceRepository()
                        .GetInvoicesNewerThan(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToInvoiceViewModelList() ?? new List<InvoiceViewModel>());
                }
                else
                {
                    response.Invoices.AddRange(unitOfWork.GetInvoiceRepository()
                        .GetInvoices(request.CompanyId)
                        ?.ConvertToInvoiceViewModelList() ?? new List<InvoiceViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Invoices = new List<InvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
