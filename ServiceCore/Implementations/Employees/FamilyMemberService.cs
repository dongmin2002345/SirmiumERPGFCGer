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
    public class FamilyMemberService : IFamilyMemberService
    {
        private IUnitOfWork unitOfWork;

        public FamilyMemberService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public FamilyMemberListResponse GetFamilyMembers(int companyId)
        {
            FamilyMemberListResponse response = new FamilyMemberListResponse();
            try
            {
                response.FamilyMembers = unitOfWork.GetFamilyMemberRepository().GetFamilyMembers(companyId)
                    .ConvertToFamilyMemberViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.FamilyMembers = new List<FamilyMemberViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public FamilyMemberListResponse GetFamilyMembersNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            FamilyMemberListResponse response = new FamilyMemberListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.FamilyMembers = unitOfWork.GetFamilyMemberRepository()
                        .GetFamilyMembersNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToFamilyMemberViewModelList();
                }
                else
                {
                    response.FamilyMembers = unitOfWork.GetFamilyMemberRepository()
                        .GetFamilyMembers(companyId)
                        .ConvertToFamilyMemberViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.FamilyMembers = new List<FamilyMemberViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public FamilyMemberResponse Create(FamilyMemberViewModel familyMember)
        {
            FamilyMemberResponse response = new FamilyMemberResponse();
            try
            {
                FamilyMember addedFamilyMember = unitOfWork.GetFamilyMemberRepository().Create(familyMember.ConvertToFamilyMember());
                unitOfWork.Save();

                response.FamilyMember = addedFamilyMember.ConvertToFamilyMemberViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.FamilyMember = new FamilyMemberViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public FamilyMemberResponse Delete(Guid identifier)
        {
            FamilyMemberResponse response = new FamilyMemberResponse();
            try
            {
                FamilyMember deletedFamilyMember = unitOfWork.GetFamilyMemberRepository().Delete(identifier);

                unitOfWork.Save();

                response.FamilyMember = deletedFamilyMember.ConvertToFamilyMemberViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.FamilyMember = new FamilyMemberViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public FamilyMemberListResponse Sync(SyncFamilyMemberRequest request)
        {
            FamilyMemberListResponse response = new FamilyMemberListResponse();
            try
            {
                response.FamilyMembers = new List<FamilyMemberViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.FamilyMembers.AddRange(unitOfWork.GetFamilyMemberRepository()
                        .GetFamilyMembersNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToFamilyMemberViewModelList() ?? new List<FamilyMemberViewModel>());
                }
                else
                {
                    response.FamilyMembers.AddRange(unitOfWork.GetFamilyMemberRepository()
                        .GetFamilyMembers(request.CompanyId)
                        ?.ConvertToFamilyMemberViewModelList() ?? new List<FamilyMemberViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.FamilyMembers = new List<FamilyMemberViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

