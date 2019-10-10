using DataMapper.Mappers.Common.OutputInvoices;
using DomainCore.Common.OutputInvoices;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Implementations.Common.OutputInvoices
{
    public class OutputInvoiceService : IOutputInvoiceService
    {
        IUnitOfWork unitOfWork;

        public OutputInvoiceService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public OutputInvoiceListResponse GetOutputInvoices(int companyId)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                response.OutputInvoices = unitOfWork.GetOutputInvoiceRepository().GetOutputInvoices(companyId)
               .ConvertToOutputInvoiceViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.OutputInvoices = new List<OutputInvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public OutputInvoiceListResponse GetOutputInvoicesNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.OutputInvoices = unitOfWork.GetOutputInvoiceRepository()
                        .GetOutputInvoicesNewerThan(companyId, (DateTime)lastUpdateTime)
                        .ConvertToOutputInvoiceViewModelList();
                }
                else
                {
                    response.OutputInvoices = unitOfWork.GetOutputInvoiceRepository()
                        .GetOutputInvoices(companyId)
                        .ConvertToOutputInvoiceViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.OutputInvoices = new List<OutputInvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public OutputInvoiceResponse Create(OutputInvoiceViewModel outputInvoice)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                //Backup items
                List<OutputInvoiceNoteViewModel> outputInvoiceNotes = outputInvoice
                    .OutputInvoiceNotes?.ToList() ?? new List<OutputInvoiceNoteViewModel>();
                outputInvoice.OutputInvoiceNotes = null;

                List<OutputInvoiceDocumentViewModel> outputInvoiceDocuments = outputInvoice
                   .OutputInvoiceDocuments?.ToList() ?? new List<OutputInvoiceDocumentViewModel>();
                outputInvoice.OutputInvoiceDocuments = null;

                OutputInvoice createdOutputInvoice = unitOfWork.GetOutputInvoiceRepository()
                    .Create(outputInvoice.ConvertToOutputInvoice());

                // Update items
                if (outputInvoiceNotes != null && outputInvoiceNotes.Count > 0)
                {
                    foreach (OutputInvoiceNoteViewModel item in outputInvoiceNotes
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<OutputInvoiceNoteViewModel>())
                    {
                        item.OutputInvoice = new OutputInvoiceViewModel() { Id = createdOutputInvoice.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetOutputInvoiceNoteRepository().Create(item.ConvertToOutputInvoiceNote());
                    }

                    foreach (OutputInvoiceNoteViewModel item in outputInvoiceNotes
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<OutputInvoiceNoteViewModel>())
                    {
                        item.OutputInvoice = new OutputInvoiceViewModel() { Id = createdOutputInvoice.Id };
                        unitOfWork.GetOutputInvoiceNoteRepository().Create(item.ConvertToOutputInvoiceNote());

                        unitOfWork.GetOutputInvoiceNoteRepository().Delete(item.Identifier);
                    }

                }

                // Update items
                if (outputInvoiceDocuments != null && outputInvoiceDocuments.Count > 0)
                {
                    foreach (OutputInvoiceDocumentViewModel item in outputInvoiceDocuments
                       .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<OutputInvoiceDocumentViewModel>())
                    {
                        item.OutputInvoice = new OutputInvoiceViewModel() { Id = createdOutputInvoice.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetOutputInvoiceDocumentRepository().Create(item.ConvertToOutputInvoiceDocument());
                    }

                    foreach (OutputInvoiceDocumentViewModel item in outputInvoiceDocuments
                       .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<OutputInvoiceDocumentViewModel>())
                    {
                        item.OutputInvoice = new OutputInvoiceViewModel() { Id = createdOutputInvoice.Id };
                        unitOfWork.GetOutputInvoiceDocumentRepository().Create(item.ConvertToOutputInvoiceDocument());

                        unitOfWork.GetOutputInvoiceDocumentRepository().Delete(item.Identifier);
                    }
                }

                unitOfWork.Save();

                response.OutputInvoice = createdOutputInvoice.ConvertToOutputInvoiceViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.OutputInvoice = new OutputInvoiceViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public OutputInvoiceResponse Delete(Guid identifier)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                OutputInvoice deletedOutputInvoice = unitOfWork.GetOutputInvoiceRepository().Delete(identifier);

                unitOfWork.Save();

                response.OutputInvoice = deletedOutputInvoice.ConvertToOutputInvoiceViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.OutputInvoice = new OutputInvoiceViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public OutputInvoiceListResponse Sync(SyncOutputInvoiceRequest request)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                response.OutputInvoices = new List<OutputInvoiceViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.OutputInvoices.AddRange(unitOfWork.GetOutputInvoiceRepository()
                        .GetOutputInvoicesNewerThan(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToOutputInvoiceViewModelList() ?? new List<OutputInvoiceViewModel>());
                }
                else
                {
                    response.OutputInvoices.AddRange(unitOfWork.GetOutputInvoiceRepository()
                        .GetOutputInvoices(request.CompanyId)
                        ?.ConvertToOutputInvoiceViewModelList() ?? new List<OutputInvoiceViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.OutputInvoices = new List<OutputInvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        //public OutputInvoiceListResponse GetOutputInvoicesByPage(int currentPage = 1, int itemsPerPage = 50, string filterString = "")
        //{
        //    OutputInvoiceListResponse response = new OutputInvoiceListResponse();
        //    try
        //    {
        //        response.OutputInvoicesByPage = unitOfWork.GetOutputInvoiceRepository()
        //            .GetOutputInvoicesByPages(currentPage, itemsPerPage, filterString)
        //            .ConvertToOutputInvoiceViewModelList();
        //        response.TotalItems = unitOfWork.GetOutputInvoiceRepository().GetOutputInvoicesCount(filterString);
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.OutputInvoicesByPage = new List<OutputInvoiceViewModel>();
        //        response.Success = false;
        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}

        //public OutputInvoiceListResponse GetOutputInvoicesForPopup(string filterString)
        //{
        //    OutputInvoiceListResponse response = new OutputInvoiceListResponse();
        //    try
        //    {
        //        response.OutputInvoices = unitOfWork.GetOutputInvoiceRepository()
        //            .GetOutputInvoicesForPopup(filterString)
        //            .ConvertToOutputInvoiceViewModelList();
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.OutputInvoices = new List<OutputInvoiceViewModel>();
        //        response.Success = false;
        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}

        //public OutputInvoiceResponse GetOutputInvoice(int id)
        //{
        //    OutputInvoiceResponse response = new OutputInvoiceResponse();
        //    try
        //    {
        //        response.OutputInvoice = unitOfWork.GetOutputInvoiceRepository().GetOutputInvoice(id)
        //            .ConvertToOutputInvoiceViewModel();
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.OutputInvoice = new OutputInvoiceViewModel();
        //        response.Success = false;
        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}

        //public OutputInvoiceResponse GetNewCodeValue()
        //{
        //    OutputInvoiceResponse response = new OutputInvoiceResponse();
        //    try
        //    {
        //        response.OutputInvoiceCode = unitOfWork.GetOutputInvoiceRepository().GetNewCodeValue();
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.OutputInvoice = new OutputInvoiceViewModel();
        //        response.Success = false;
        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}

        //public OutputInvoiceResponse Create(OutputInvoiceViewModel oi)
        //{
        //    OutputInvoiceResponse response = new OutputInvoiceResponse();
        //    try
        //    {
        //        //List<OutputInvoiceItem> outputInvoiceItems = oi.ConvertToOutputInvoice().InvoiceItems;
        //        //oi.SumOfBase = outputInvoiceItems.Sum(x => x.Base);
        //        //oi.SumOfPdvValue = outputInvoiceItems.Sum(x => x.Pdv);
        //        //oi.SumOfTotalValue = outputInvoiceItems.Sum(x => x.TotalPrice);
        //        //oi.OutputInvoiceSubItems = null;

        //        OutputInvoice addedOutPutInvoice = unitOfWork.GetOutputInvoiceRepository().Create(oi.ConvertToOutputInvoice());

        //        //unitOfWork.GetOutputInvoiceItemRepository().UpdateOutputInvoiceItems(outputInvoiceItems ?? new List<OutputInvoiceItem>(), addedOutPutInvoice);

        //        unitOfWork.Save();

        //        response.OutputInvoice = addedOutPutInvoice.ConvertToOutputInvoiceViewModel();
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.OutputInvoice = new OutputInvoiceViewModel();
        //        response.Success = false;
        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}

        //public OutputInvoiceResponse Update(OutputInvoiceViewModel oi)
        //{
        //    OutputInvoiceResponse response = new OutputInvoiceResponse();
        //    try
        //    {
        //        //List<OutputInvoiceItem> outputInvoiceItems = oi.ConvertToOutputInvoice().InvoiceItems;
        //        //oi.SumOfBase = outputInvoiceItems.Sum(x => x.Base);
        //        //oi.SumOfPdvValue = outputInvoiceItems.Sum(x => x.Pdv);
        //        //oi.SumOfTotalValue = outputInvoiceItems.Sum(x => x.TotalPrice);
        //        //oi.OutputInvoiceSubItems = null;

        //        OutputInvoice updatedOutputInvoice = unitOfWork.GetOutputInvoiceRepository().Update(oi.ConvertToOutputInvoice());

        //        //unitOfWork.GetOutputInvoiceItemRepository().UpdateOutputInvoiceItems(outputInvoiceItems ?? new List<OutputInvoiceItem>(), updatedOutputInvoice);

        //        unitOfWork.Save();

        //        response.OutputInvoice = updatedOutputInvoice.ConvertToOutputInvoiceViewModel();
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.OutputInvoice = new OutputInvoiceViewModel();
        //        response.Success = false;
        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}

        //public OutputInvoiceResponse SetInvoiceLock(int id, bool locked)
        //{
        //    OutputInvoiceResponse response = new OutputInvoiceResponse();
        //    try
        //    {
        //        response.OutputInvoice = unitOfWork.GetOutputInvoiceRepository().SetInvoiceLock(id, locked)
        //            .ConvertToOutputInvoiceViewModel();
        //        unitOfWork.Save();
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.OutputInvoice = new OutputInvoiceViewModel();
        //        response.Success = false;
        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}

        //public OutputInvoiceResponse Delete(int id)
        //{
        //    OutputInvoiceResponse response = new OutputInvoiceResponse();
        //    try
        //    {
        //        OutputInvoice deletedOutputInvoice = unitOfWork.GetOutputInvoiceRepository().Delete(id);

        //        unitOfWork.Save();

        //        response.OutputInvoice = deletedOutputInvoice.ConvertToOutputInvoiceViewModel();
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.OutputInvoice = new OutputInvoiceViewModel();
        //        response.Success = false;
        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}

        //public OutputInvoiceResponse CancelOutputInvoice(int id)
        //{
        //    OutputInvoiceResponse response = new OutputInvoiceResponse();
        //    try
        //    {
        //        OutputInvoice outputInvoice = unitOfWork.GetOutputInvoiceRepository().CancelOutputInvoice(id);
        //        if (outputInvoice != null && outputInvoice.Active != false)
        //            unitOfWork.Save();

        //        response.OutputInvoice = outputInvoice.ConvertToOutputInvoiceViewModel();
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.OutputInvoice = new OutputInvoiceViewModel();
        //        response.Success = false;
        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}
    }
}

