using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IFamilyMemberService
    {
        FamilyMemberListResponse GetFamilyMembers(int companyId);
        FamilyMemberListResponse GetFamilyMembersNewerThen(int companyId, DateTime? lastUpdateTime);

        FamilyMemberResponse Create(FamilyMemberViewModel familyMember);
        FamilyMemberResponse Delete(Guid identifier);

        FamilyMemberListResponse Sync(SyncFamilyMemberRequest request);
    }
}
