using DataMapper.Mappers.Vats;
using DomainCore.Vats;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Vats;
using ServiceInterfaces.Messages.Vats;
using ServiceInterfaces.ViewModels.Vats;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Vats
{
    public class VatService : IVatService
    {
        private IUnitOfWork unitOfWork;

        public VatService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public VatListResponse GetVats(int companyId)
        {
            VatListResponse response = new VatListResponse();
            try
            {
                response.Vats = unitOfWork.GetVatRepository().GetVats(companyId)
                    .ConvertToVatViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Vats = new List<VatViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public VatResponse Create(VatViewModel re)
        {
            VatResponse response = new VatResponse();
            try
            {
                Vat addedVat = unitOfWork.GetVatRepository().Create(re.ConvertToVat());

                unitOfWork.Save();

                response.Vat = addedVat.ConvertToVatViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Vat = new VatViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public VatResponse Delete(Guid identifier)
        {
            VatResponse response = new VatResponse();
            try
            {
                Vat deletedVat = unitOfWork.GetVatRepository().Delete(identifier);

                unitOfWork.Save();

                response.Vat = deletedVat.ConvertToVatViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Vat = new VatViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public VatListResponse Sync(SyncVatRequest request)
        {
            VatListResponse response = new VatListResponse();
            try
            {
                response.Vats = new List<VatViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Vats.AddRange(unitOfWork.GetVatRepository()
                        .GetVatsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToVatViewModelList() ?? new List<VatViewModel>());
                }
                else
                {
                    response.Vats.AddRange(unitOfWork.GetVatRepository()
                        .GetVats(request.CompanyId)
                        ?.ConvertToVatViewModelList() ?? new List<VatViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Vats = new List<VatViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
