using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Identity
{
    public class CustomIdentity : IIdentity
    {
        public int Id { get; private set; }
        public Guid UserIdentifier { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public int CompanyId { get; private set; }
        public Guid CompanyIdentifier { get; private set; }
        public string CompanyName { get; private set; }
        public UserViewModel CompanyUser { get; private set; }
        public CompanyUserViewModel CompanyUserData { get; private set; }
        public List<string> Roles { get; private set; }

        public CustomIdentity(int id, string firstName, string lastName, Guid userIdentifier, string email,
            int companyId, Guid companyIdentifier, string companyName, UserViewModel user, CompanyUserViewModel compUser)
        {
            Id = id;
            Name = firstName;
            LastName = lastName;
            UserIdentifier = userIdentifier;
            Email = email;
            CompanyId = companyId;
            CompanyIdentifier = companyIdentifier;
            CompanyName = companyName;
            CompanyUser = user;
            CompanyUserData = compUser;
            Roles = compUser?.UserRoles?.Select(x => x.Name)?.ToList() ?? new List<string>();
        }

        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
    }
}
