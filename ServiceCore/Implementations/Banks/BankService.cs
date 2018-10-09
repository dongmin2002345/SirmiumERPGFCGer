using DataMapper.Mappers.Banks;
using DomainCore.Banks;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Banks;
using ServiceInterfaces.ViewModels.Banks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Banks
{
    public class BankService : IBankService
	{
		private IUnitOfWork unitOfWork;

		public BankService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public BankListResponse GetBanks(int companyId)
		{
			BankListResponse response = new BankListResponse();
			try
			{
				response.Banks = unitOfWork.GetBankRepository().GetBanks(companyId)
					.ConvertToBankViewModelList();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Banks = new List<BankViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public BankListResponse GetBanksNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			BankListResponse response = new BankListResponse();
			try
			{
				if (lastUpdateTime != null)
				{
					response.Banks = unitOfWork.GetBankRepository()
						.GetBanksNewerThen(companyId, (DateTime)lastUpdateTime)
						.ConvertToBankViewModelList();
				}
				else
				{
					response.Banks = unitOfWork.GetBankRepository()
						.GetBanks(companyId)
						.ConvertToBankViewModelList();
				}
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Banks = new List<BankViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}


		public BankResponse Create(BankViewModel bank)
		{
			BankResponse response = new BankResponse();
			try
			{
				Bank addedBank = unitOfWork.GetBankRepository().Create(bank.ConvertToBank());
				unitOfWork.Save();

				response.Bank = addedBank.ConvertToBankViewModel();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Bank = new BankViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}


		public BankResponse Delete(Guid identifier)
		{
			BankResponse response = new BankResponse();
			try
			{
				Bank deletedBank = unitOfWork.GetBankRepository().Delete(identifier);

				unitOfWork.Save();

				response.Bank = deletedBank.ConvertToBankViewModel();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Bank = new BankViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public BankListResponse Sync(SyncBankRequest request)
		{
			BankListResponse response = new BankListResponse();
			try
			{
				response.Banks = new List<BankViewModel>();

				if (request.LastUpdatedAt != null)
				{
					response.Banks.AddRange(unitOfWork.GetBankRepository()
						.GetBanksNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
						?.ConvertToBankViewModelList() ?? new List<BankViewModel>());
				}
				else
				{
					response.Banks.AddRange(unitOfWork.GetBankRepository()
						.GetBanks(request.CompanyId)
						?.ConvertToBankViewModelList() ?? new List<BankViewModel>());
				}

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Banks = new List<BankViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
