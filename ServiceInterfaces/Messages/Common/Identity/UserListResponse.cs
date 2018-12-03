using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Identity
{
    public class UserListResponse : BaseResponse
    {
        public List<UserViewModel> Users { get; set; }
        public int TotalItems { get; set; }
    }
}
