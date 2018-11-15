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
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        IUnitOfWork unitOfWork;

        public EmployeeDocumentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmployeeDocumentListResponse Sync(SyncEmployeeDocumentRequest request)
        {
            EmployeeDocumentListResponse response = new EmployeeDocumentListResponse();
            try
            {
                response.EmployeeDocuments = new List<EmployeeDocumentViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.EmployeeDocuments.AddRange(unitOfWork.GetEmployeeDocumentRepository()
                        .GetEmployeeDocumentsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToEmployeeDocumentViewModelList() ?? new List<EmployeeDocumentViewModel>());
                }
                else
                {
                    response.EmployeeDocuments.AddRange(unitOfWork.GetEmployeeDocumentRepository()
                        .GetEmployeeDocuments(request.CompanyId)
                        ?.ConvertToEmployeeDocumentViewModelList() ?? new List<EmployeeDocumentViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeDocuments = new List<EmployeeDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
