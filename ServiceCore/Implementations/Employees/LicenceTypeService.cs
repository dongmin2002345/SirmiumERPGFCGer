using DataMapper.Mappers.Employees;
using DomainCore.Employees;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Employees
{
   public class LicenceTypeService : ILicenceTypeService
    {
		private IUnitOfWork unitOfWork;

		public LicenceTypeService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public LicenceTypeListResponse GetLicenceTypes(int companyId)
		{
			LicenceTypeListResponse response = new LicenceTypeListResponse();
			try
			{
				response.LicenceTypes = unitOfWork.GetLicenceTypeRepository().GetLicenceTypes(companyId)
					.ConvertToLicenceTypeViewModelList();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.LicenceTypes = new List<LicenceTypeViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public LicenceTypeListResponse GetLicenceTypesNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			LicenceTypeListResponse response = new LicenceTypeListResponse();
			try
			{
				if (lastUpdateTime != null)
				{
					response.LicenceTypes = unitOfWork.GetLicenceTypeRepository()
						.GetLicenceTypesNewerThen(companyId, (DateTime)lastUpdateTime)
						.ConvertToLicenceTypeViewModelList();
				}
				else
				{
					response.LicenceTypes = unitOfWork.GetLicenceTypeRepository()
						.GetLicenceTypes(companyId)
						.ConvertToLicenceTypeViewModelList();
				}
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.LicenceTypes = new List<LicenceTypeViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}


		public LicenceTypeResponse Create(LicenceTypeViewModel licenceType)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();
			try
			{
				LicenceType addedLicenceType = unitOfWork.GetLicenceTypeRepository().Create(licenceType.ConvertToLicenceType());
				unitOfWork.Save();

				response.LicenceType = addedLicenceType.ConvertToLicenceTypeViewModel();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.LicenceType = new LicenceTypeViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}


		public LicenceTypeResponse Delete(Guid identifier)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();
			try
			{
				LicenceType deletedLicenceType = unitOfWork.GetLicenceTypeRepository().Delete(identifier);

				unitOfWork.Save();

				response.LicenceType = deletedLicenceType.ConvertToLicenceTypeViewModel();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.LicenceType = new LicenceTypeViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public LicenceTypeListResponse Sync(SyncLicenceTypeRequest request)
		{
			LicenceTypeListResponse response = new LicenceTypeListResponse();
			try
			{
				response.LicenceTypes = new List<LicenceTypeViewModel>();

				if (request.LastUpdatedAt != null)
				{
					response.LicenceTypes.AddRange(unitOfWork.GetLicenceTypeRepository()
						.GetLicenceTypesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
						?.ConvertToLicenceTypeViewModelList() ?? new List<LicenceTypeViewModel>());
				}
				else
				{
					response.LicenceTypes.AddRange(unitOfWork.GetLicenceTypeRepository()
						.GetLicenceTypes(request.CompanyId)
						?.ConvertToLicenceTypeViewModelList() ?? new List<LicenceTypeViewModel>());
				}

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.LicenceTypes = new List<LicenceTypeViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
