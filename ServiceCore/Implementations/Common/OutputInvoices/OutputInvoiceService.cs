using DataMapper.Mappers.Common.OutputInvoices;
using DomainCore.Common.OutputInvoices;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
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

        public OutputInvoiceListResponse GetOutputInvoicesByPage(int currentPage = 1, int itemsPerPage = 50, string filterString = "")
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                response.OutputInvoicesByPage = unitOfWork.GetOutputInvoiceRepository()
                    .GetOutputInvoicesByPages(currentPage, itemsPerPage, filterString)
                    .ConvertToOutputInvoiceViewModelList();
                response.TotalItems = unitOfWork.GetOutputInvoiceRepository().GetOutputInvoicesCount(filterString);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.OutputInvoicesByPage = new List<OutputInvoiceViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public OutputInvoiceListResponse GetOutputInvoicesForPopup(string filterString)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                response.OutputInvoices = unitOfWork.GetOutputInvoiceRepository()
                    .GetOutputInvoicesForPopup(filterString)
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

        public OutputInvoiceResponse GetOutputInvoice(int id)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                response.OutputInvoice = unitOfWork.GetOutputInvoiceRepository().GetOutputInvoice(id)
                    .ConvertToOutputInvoiceViewModel();
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

        public OutputInvoiceResponse GetNewCodeValue()
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                response.OutputInvoiceCode = unitOfWork.GetOutputInvoiceRepository().GetNewCodeValue();
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

        public OutputInvoiceResponse Create(OutputInvoiceViewModel oi)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                //List<OutputInvoiceItem> outputInvoiceItems = oi.ConvertToOutputInvoice().InvoiceItems;
                //oi.SumOfBase = outputInvoiceItems.Sum(x => x.Base);
                //oi.SumOfPdvValue = outputInvoiceItems.Sum(x => x.Pdv);
                //oi.SumOfTotalValue = outputInvoiceItems.Sum(x => x.TotalPrice);
                //oi.OutputInvoiceSubItems = null;

                OutputInvoice addedOutPutInvoice = unitOfWork.GetOutputInvoiceRepository().Create(oi.ConvertToOutputInvoice());

                //unitOfWork.GetOutputInvoiceItemRepository().UpdateOutputInvoiceItems(outputInvoiceItems ?? new List<OutputInvoiceItem>(), addedOutPutInvoice);

                unitOfWork.Save();

                response.OutputInvoice = addedOutPutInvoice.ConvertToOutputInvoiceViewModel();
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

        public OutputInvoiceResponse Update(OutputInvoiceViewModel oi)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                //List<OutputInvoiceItem> outputInvoiceItems = oi.ConvertToOutputInvoice().InvoiceItems;
                //oi.SumOfBase = outputInvoiceItems.Sum(x => x.Base);
                //oi.SumOfPdvValue = outputInvoiceItems.Sum(x => x.Pdv);
                //oi.SumOfTotalValue = outputInvoiceItems.Sum(x => x.TotalPrice);
                //oi.OutputInvoiceSubItems = null;

                OutputInvoice updatedOutputInvoice = unitOfWork.GetOutputInvoiceRepository().Update(oi.ConvertToOutputInvoice());

                //unitOfWork.GetOutputInvoiceItemRepository().UpdateOutputInvoiceItems(outputInvoiceItems ?? new List<OutputInvoiceItem>(), updatedOutputInvoice);

                unitOfWork.Save();

                response.OutputInvoice = updatedOutputInvoice.ConvertToOutputInvoiceViewModel();
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

        public OutputInvoiceResponse SetInvoiceLock(int id, bool locked)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                response.OutputInvoice = unitOfWork.GetOutputInvoiceRepository().SetInvoiceLock(id, locked)
                    .ConvertToOutputInvoiceViewModel();
                unitOfWork.Save();
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

        public OutputInvoiceResponse Delete(int id)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                OutputInvoice deletedOutputInvoice = unitOfWork.GetOutputInvoiceRepository().Delete(id);

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

        public OutputInvoiceResponse CancelOutputInvoice(int id)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                OutputInvoice outputInvoice = unitOfWork.GetOutputInvoiceRepository().CancelOutputInvoice(id);
                if (outputInvoice != null && outputInvoice.Active != false)
                    unitOfWork.Save();

                response.OutputInvoice = outputInvoice.ConvertToOutputInvoiceViewModel();
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
    }
}

