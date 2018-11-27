using DataMapper.Mappers.ConstructionSites;
using DomainCore.ConstructionSites;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Implementations.ConstructionSites
{
    public class ConstructionSiteService : IConstructionSiteService
    {
        private IUnitOfWork unitOfWork;

        public ConstructionSiteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ConstructionSiteListResponse GetConstructionSites(int companyId)
        {
            ConstructionSiteListResponse response = new ConstructionSiteListResponse();
            try
            {
                response.ConstructionSites = unitOfWork.GetConstructionSiteRepository().GetConstructionSites(companyId)
                    .ConvertToConstructionSiteViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ConstructionSites = new List<ConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ConstructionSiteListResponse GetConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            ConstructionSiteListResponse response = new ConstructionSiteListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.ConstructionSites = unitOfWork.GetConstructionSiteRepository()
                        .GetConstructionSitesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToConstructionSiteViewModelList();
                }
                else
                {
                    response.ConstructionSites = unitOfWork.GetConstructionSiteRepository()
                        .GetConstructionSites(companyId)
                        .ConvertToConstructionSiteViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ConstructionSites = new List<ConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public ConstructionSiteResponse Create(ConstructionSiteViewModel constructionSite)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();
            try
            {
                // Backup items
                List<ConstructionSiteDocumentViewModel> constructionSiteDocuments = constructionSite.ConstructionSiteDocuments?.ToList() ?? new List<ConstructionSiteDocumentViewModel>();
                constructionSite.ConstructionSiteDocuments = null;
                List<ConstructionSiteNoteViewModel> constructionSiteNotes = constructionSite.ConstructionSiteNotes?.ToList() ?? new List<ConstructionSiteNoteViewModel>();
                constructionSite.ConstructionSiteNotes = null;

                ConstructionSite createdConstructionSite = unitOfWork.GetConstructionSiteRepository()
                    .Create(constructionSite.ConvertToConstructionSite());

                // Update items
                var ConstructionSiteDocumentsFromDB = unitOfWork.GetConstructionSiteDocumentRepository().GetConstructionSiteDocumentsByConstructionSite(createdConstructionSite.Id);
                foreach (var item in ConstructionSiteDocumentsFromDB)
                    if (!constructionSiteDocuments.Select(x => x.Identifier).Contains(item.Identifier))
                        unitOfWork.GetConstructionSiteDocumentRepository().Delete(item.Identifier);
                foreach (var item in constructionSiteDocuments)
                {
                    item.ConstructionSite = new ConstructionSiteViewModel() { Id = createdConstructionSite.Id };
                    unitOfWork.GetConstructionSiteDocumentRepository().Create(item.ConvertToConstructionSiteDocument());
                }

                var ConstructionSiteNotesFromDB = unitOfWork.GetConstructionSiteNoteRepository().GetConstructionSiteNotesByConstructionSite(createdConstructionSite.Id);
                foreach (var item in ConstructionSiteNotesFromDB)
                    if (!constructionSiteNotes.Select(x => x.Identifier).Contains(item.Identifier))
                        unitOfWork.GetConstructionSiteNoteRepository().Delete(item.Identifier);
                foreach (var item in constructionSiteNotes)
                {
                    item.ConstructionSite = new ConstructionSiteViewModel() { Id = createdConstructionSite.Id };
                    unitOfWork.GetConstructionSiteNoteRepository().Create(item.ConvertToConstructionSiteNote());
                }

                unitOfWork.Save();

                response.ConstructionSite = createdConstructionSite.ConvertToConstructionSiteViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ConstructionSite = new ConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public ConstructionSiteResponse Delete(Guid identifier)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();
            try
            {
                ConstructionSite deletedConstructionSite = unitOfWork.GetConstructionSiteRepository().Delete(identifier);

                unitOfWork.Save();

                response.ConstructionSite = deletedConstructionSite.ConvertToConstructionSiteViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ConstructionSite = new ConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ConstructionSiteListResponse Sync(SyncConstructionSiteRequest request)
        {
            ConstructionSiteListResponse response = new ConstructionSiteListResponse();
            try
            {
                response.ConstructionSites = new List<ConstructionSiteViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.ConstructionSites.AddRange(unitOfWork.GetConstructionSiteRepository()
                        .GetConstructionSitesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToConstructionSiteViewModelList() ?? new List<ConstructionSiteViewModel>());
                }
                else
                {
                    response.ConstructionSites.AddRange(unitOfWork.GetConstructionSiteRepository()
                        .GetConstructionSites(request.CompanyId)
                        ?.ConvertToConstructionSiteViewModelList() ?? new List<ConstructionSiteViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ConstructionSites = new List<ConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
