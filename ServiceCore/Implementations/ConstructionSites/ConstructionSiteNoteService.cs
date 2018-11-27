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
    public class ConstructionSiteNoteService : IConstructionSiteNoteService
    {
        IUnitOfWork unitOfWork;

        public ConstructionSiteNoteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ConstructionSiteNoteListResponse Sync(SyncConstructionSiteNoteRequest request)
        {
            ConstructionSiteNoteListResponse response = new ConstructionSiteNoteListResponse();
            try
            {
                response.ConstructionSiteNotes = new List<ConstructionSiteNoteViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.ConstructionSiteNotes.AddRange(unitOfWork.GetConstructionSiteNoteRepository()
                        .GetConstructionSiteNotesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToConstructionSiteNoteViewModelList() ?? new List<ConstructionSiteNoteViewModel>());
                }
                else
                {
                    response.ConstructionSiteNotes.AddRange(unitOfWork.GetConstructionSiteNoteRepository()
                        .GetConstructionSiteNotes(request.CompanyId)
                        ?.ConvertToConstructionSiteNoteViewModelList() ?? new List<ConstructionSiteNoteViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ConstructionSiteNotes = new List<ConstructionSiteNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
