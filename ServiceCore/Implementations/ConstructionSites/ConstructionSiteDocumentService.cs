using DataMapper.Mappers.ConstructionSites;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.ConstructionSites
{
    public class ConstructionSiteDocumentService : IConstructionSiteDocumentService
    {
        IUnitOfWork unitOfWork;

        public ConstructionSiteDocumentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ConstructionSiteDocumentListResponse Sync(SyncConstructionSiteDocumentRequest request)
        {
            ConstructionSiteDocumentListResponse response = new ConstructionSiteDocumentListResponse();
            try
            {
                response.ConstructionSiteDocuments = new List<ConstructionSiteDocumentViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.ConstructionSiteDocuments.AddRange(unitOfWork.GetConstructionSiteDocumentRepository()
                        .GetConstructionSiteDocumentsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToConstructionSiteDocumentViewModelList() ?? new List<ConstructionSiteDocumentViewModel>());
                }
                else
                {
                    response.ConstructionSiteDocuments.AddRange(unitOfWork.GetConstructionSiteDocumentRepository()
                        .GetConstructionSiteDocuments(request.CompanyId)
                        ?.ConvertToConstructionSiteDocumentViewModelList() ?? new List<ConstructionSiteDocumentViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ConstructionSiteDocuments = new List<ConstructionSiteDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
