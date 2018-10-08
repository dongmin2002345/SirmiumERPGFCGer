using DataMapper.Mappers.Common.Professions;
using DomainCore.Common.Professions;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Professions;
using ServiceInterfaces.Messages.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Professions
{
    public class ProfessionService : IProfessionService
    {
        private IUnitOfWork unitOfWork;

        public ProfessionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ProfessionListResponse GetProfessions(int companyId)
        {
            ProfessionListResponse response = new ProfessionListResponse();
            try
            {
                response.Professions = unitOfWork.GetProfessionRepository().GetProfessions(companyId)
                    .ConvertToProfessionViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Professions = new List<ProfessionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ProfessionListResponse GetProfessionsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            ProfessionListResponse response = new ProfessionListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Professions = unitOfWork.GetProfessionRepository()
                        .GetProfessionsNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToProfessionViewModelList();
                }
                else
                {
                    response.Professions = unitOfWork.GetProfessionRepository()
                        .GetProfessions(companyId)
                        .ConvertToProfessionViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Professions = new List<ProfessionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public ProfessionResponse Create(ProfessionViewModel city)
        {
            ProfessionResponse response = new ProfessionResponse();
            try
            {
                Profession addedProfession = unitOfWork.GetProfessionRepository().Create(city.ConvertToProfession());
                unitOfWork.Save();

                response.Profession = addedProfession.ConvertToProfessionViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Profession = new ProfessionViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public ProfessionResponse Delete(Guid identifier)
        {
            ProfessionResponse response = new ProfessionResponse();
            try
            {
                Profession deletedProfession = unitOfWork.GetProfessionRepository().Delete(identifier);

                unitOfWork.Save();

                response.Profession = deletedProfession.ConvertToProfessionViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Profession = new ProfessionViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ProfessionListResponse Sync(SyncProfessionRequest request)
        {
            ProfessionListResponse response = new ProfessionListResponse();
            try
            {
                response.Professions = new List<ProfessionViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Professions.AddRange(unitOfWork.GetProfessionRepository()
                        .GetProfessionsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToProfessionViewModelList() ?? new List<ProfessionViewModel>());
                }
                else
                {
                    response.Professions.AddRange(unitOfWork.GetProfessionRepository()
                        .GetProfessions(request.CompanyId)
                        ?.ConvertToProfessionViewModelList() ?? new List<ProfessionViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Professions = new List<ProfessionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
