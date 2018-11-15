using DataMapper.Mappers.Common.InputInvoices;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.InputInvoices
{
    public class InputInvoiceService : IInputInvoiceService
	{
		IUnitOfWork unitOfWork;

		public InputInvoiceService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public InputInvoiceListResponse GetInputInvoices(int companyId)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			try
			{
				response.InputInvoices = unitOfWork.GetInputInvoiceRepository().GetInputInvoices(companyId)
			   .ConvertToInputInvoiceViewModelList();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.InputInvoices = new List<InputInvoiceViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceListResponse GetInputInvoicesNewerThan(int companyId, DateTime? lastUpdateTime)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			try
			{
				if (lastUpdateTime != null)
				{
					response.InputInvoices = unitOfWork.GetInputInvoiceRepository()
						.GetInputInvoicesNewerThan(companyId, (DateTime)lastUpdateTime)
						.ConvertToInputInvoiceViewModelList();
				}
				else
				{
					response.InputInvoices = unitOfWork.GetInputInvoiceRepository()
						.GetInputInvoices(companyId)
						.ConvertToInputInvoiceViewModelList();
				}
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.InputInvoices = new List<InputInvoiceViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceResponse Create(InputInvoiceViewModel re)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();
			try
			{
				DomainCore.Common.InputInvoices.InputInvoice addedInputInvoice = unitOfWork.GetInputInvoiceRepository().Create(re.ConvertToInputInvoice());
				unitOfWork.Save();
				response.InputInvoice = addedInputInvoice.ConvertToInputInvoiceViewModel();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.InputInvoice = new InputInvoiceViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceResponse Delete(Guid identifier)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();
			try
			{
				DomainCore.Common.InputInvoices.InputInvoice deletedInputInvoice = unitOfWork.GetInputInvoiceRepository().Delete(identifier);

				unitOfWork.Save();

				response.InputInvoice = deletedInputInvoice.ConvertToInputInvoiceViewModel();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.InputInvoice = new InputInvoiceViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public InputInvoiceListResponse Sync(SyncInputInvoiceRequest request)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			try
			{
				response.InputInvoices = new List<InputInvoiceViewModel>();

				if (request.LastUpdatedAt != null)
				{
					response.InputInvoices.AddRange(unitOfWork.GetInputInvoiceRepository()
						.GetInputInvoicesNewerThan(request.CompanyId, (DateTime)request.LastUpdatedAt)
						?.ConvertToInputInvoiceViewModelList() ?? new List<InputInvoiceViewModel>());
				}
				else
				{
					response.InputInvoices.AddRange(unitOfWork.GetInputInvoiceRepository()
						.GetInputInvoices(request.CompanyId)
						?.ConvertToInputInvoiceViewModelList() ?? new List<InputInvoiceViewModel>());
				}

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.InputInvoices = new List<InputInvoiceViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
