using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Employees
{
    public class FamilyMemberService : IFamilyMemberService
    {
        public FamilyMemberListResponse GetFamilyMembers(int companyId)
        {
            FamilyMemberListResponse response = new FamilyMemberListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<FamilyMemberViewModel>, FamilyMemberListResponse>("GetFamilyMembers", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
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
                response = WpfApiHandler.GetFromApi<List<FamilyMemberViewModel>, FamilyMemberListResponse>("GetFamilyMembersNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.FamilyMembers = new List<FamilyMemberViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public FamilyMemberResponse Create(FamilyMemberViewModel FamilyMember)
        {
            FamilyMemberResponse response = new FamilyMemberResponse();
            try
            {
                response = WpfApiHandler.SendToApi<FamilyMemberViewModel, FamilyMemberResponse>(FamilyMember, "Create");
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
                FamilyMemberViewModel re = new FamilyMemberViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<FamilyMemberViewModel, FamilyMemberResponse>(re, "Delete");
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
                response = WpfApiHandler.SendToApi<SyncFamilyMemberRequest, FamilyMemberViewModel, FamilyMemberListResponse>(request, "Sync");
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
