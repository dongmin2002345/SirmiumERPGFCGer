using DataMapper.Mappers.Common.Identity;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Messages.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Identity
{
    public class AuthenticationService : IAuthenticationService
    {
        private IUnitOfWork unitOfWork;

        public AuthenticationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public UserResponse Authenticate(string username, string password)
        {
            UserResponse response = new UserResponse();
            try
            {
                response.User = unitOfWork.GetAuthenticationRepository().Authenticate(username, password)
                    .ConvertToUserViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.User = null;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
