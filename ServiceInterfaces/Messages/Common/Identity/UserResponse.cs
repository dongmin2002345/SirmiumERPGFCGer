using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Identity
{
    public class UserResponse : BaseResponse
    {
        public UserViewModel User { get; set; }
    }
}
