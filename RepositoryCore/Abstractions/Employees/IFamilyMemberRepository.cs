using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Employees
{
    public interface IFamilyMemberRepository
    {
        List<FamilyMember> GetFamilyMembers(int companyId);
        List<FamilyMember> GetFamilyMembersNewerThen(int companyId, DateTime lastUpdateTime);

        FamilyMember Create(FamilyMember familyMember);
        FamilyMember Delete(Guid identifier);
    }
}
