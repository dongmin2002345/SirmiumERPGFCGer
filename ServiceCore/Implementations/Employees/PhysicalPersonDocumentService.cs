using DataMapper.Mappers.Employees;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Employees
{
    public class PhysicalPersonDocumentService : IPhysicalPersonDocumentService
    {
        IUnitOfWork unitOfWork;

        public PhysicalPersonDocumentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PhysicalPersonDocumentListResponse Sync(SyncPhysicalPersonDocumentRequest request)
        {
            PhysicalPersonDocumentListResponse response = new PhysicalPersonDocumentListResponse();
            try
            {
                response.PhysicalPersonDocuments = new List<PhysicalPersonDocumentViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.PhysicalPersonDocuments.AddRange(unitOfWork.GetPhysicalPersonDocumentRepository()
                        .GetPhysicalPersonDocumentsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToPhysicalPersonDocumentViewModelList() ?? new List<PhysicalPersonDocumentViewModel>());
                }
                else
                {
                    response.PhysicalPersonDocuments.AddRange(unitOfWork.GetPhysicalPersonDocumentRepository()
                        .GetPhysicalPersonDocuments(request.CompanyId)
                        ?.ConvertToPhysicalPersonDocumentViewModelList() ?? new List<PhysicalPersonDocumentViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonDocuments = new List<PhysicalPersonDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}