using DataMapper.Mappers.Common.BusinessPartners;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerNoteService : IBusinessPartnerNoteService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerNoteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public BusinessPartnerNoteListResponse Sync(SyncBusinessPartnerNoteRequest request)
        {
            BusinessPartnerNoteListResponse response = new BusinessPartnerNoteListResponse();
            try
            {
                response.BusinessPartnerNotes = new List<BusinessPartnerNoteViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartnerNotes.AddRange(unitOfWork.GetBusinessPartnerNoteRepository()
                        .GetBusinessPartnerNotesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerNoteViewModelList() ?? new List<BusinessPartnerNoteViewModel>());
                }
                else
                {
                    response.BusinessPartnerNotes.AddRange(unitOfWork.GetBusinessPartnerNoteRepository()
                        .GetBusinessPartnerNotes(request.CompanyId)
                        ?.ConvertToBusinessPartnerNoteViewModelList() ?? new List<BusinessPartnerNoteViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerNotes = new List<BusinessPartnerNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
