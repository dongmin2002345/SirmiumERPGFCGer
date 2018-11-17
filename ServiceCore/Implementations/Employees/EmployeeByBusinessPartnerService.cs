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
    public class EmployeeByBusinessPartnerService : IEmployeeByBusinessPartnerService
    {
        IUnitOfWork unitOfWork;

        public EmployeeByBusinessPartnerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmployeeByBusinessPartnerListResponse GetEmployeeByBusinessPartners(int companyId)
        {
            EmployeeByBusinessPartnerListResponse response = new EmployeeByBusinessPartnerListResponse();
            try
            {
                response.EmployeeByBusinessPartners = unitOfWork.GetEmployeeByBusinessPartnerRepository().GetEmployeeByBusinessPartners(companyId)
               .ConvertToEmployeeByBusinessPartnerViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartners = new List<EmployeeByBusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerListResponse GetEmployeeByBusinessPartnersNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByBusinessPartnerListResponse response = new EmployeeByBusinessPartnerListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.EmployeeByBusinessPartners = unitOfWork.GetEmployeeByBusinessPartnerRepository()
                        .GetEmployeeByBusinessPartnersNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToEmployeeByBusinessPartnerViewModelList();
                }
                else
                {
                    response.EmployeeByBusinessPartners = unitOfWork.GetEmployeeByBusinessPartnerRepository()
                        .GetEmployeeByBusinessPartners(companyId)
                        .ConvertToEmployeeByBusinessPartnerViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartners = new List<EmployeeByBusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerResponse Create(EmployeeByBusinessPartnerViewModel re)
        {
            EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerResponse();
            try
            {
                EmployeeByBusinessPartner addedEmployeeByBusinessPartner = unitOfWork.GetEmployeeByBusinessPartnerRepository()
                    .Create(re.ConvertToEmployeeByBusinessPartner());

                EmployeeCard ec = new EmployeeCard()
                {
                    Identifier = Guid.NewGuid(),
                    EmployeeId = re.Employee.Id,
                    CardDate = DateTime.Now,
                    Description = "Radnik " + re.Employee?.Name + " je sklopio ugovor sa firmom " + re.BusinessPartner?.Name, 
                    CreatedById = re.CreatedBy?.Id,
                    CompanyId = re.Company?.Id,
                    CreatedAt = DateTime.Now, 
                    UpdatedAt = DateTime.Now
                };
                unitOfWork.GetEmployeeCardRepository().Create(ec);

                unitOfWork.Save();

                response.EmployeeByBusinessPartner = addedEmployeeByBusinessPartner.ConvertToEmployeeByBusinessPartnerViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartner = new EmployeeByBusinessPartnerViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerResponse Delete(EmployeeByBusinessPartnerViewModel employeeByBysinessPartner)
        {
            EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerResponse();
            try
            {
                EmployeeByBusinessPartner deletedEmployeeByBusinessPartner = unitOfWork.GetEmployeeByBusinessPartnerRepository()
                    .Delete(employeeByBysinessPartner.ConvertToEmployeeByBusinessPartner());

                EmployeeCard ec = new EmployeeCard()
                {
                    Identifier = Guid.NewGuid(),
                    EmployeeId = deletedEmployeeByBusinessPartner.EmployeeId,
                    CardDate = DateTime.Now,
                    Description = "Radnik " + deletedEmployeeByBusinessPartner.Employee?.Name + " je raskinuo ugovor sa firmom " + deletedEmployeeByBusinessPartner.BusinessPartner?.Name,
                    CreatedById = deletedEmployeeByBusinessPartner.CreatedById,
                    CompanyId = deletedEmployeeByBusinessPartner.CompanyId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                unitOfWork.GetEmployeeCardRepository().Create(ec);

                unitOfWork.Save();

                response.EmployeeByBusinessPartner = deletedEmployeeByBusinessPartner.ConvertToEmployeeByBusinessPartnerViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartner = new EmployeeByBusinessPartnerViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerListResponse Sync(SyncEmployeeByBusinessPartnerRequest request)
        {
            EmployeeByBusinessPartnerListResponse response = new EmployeeByBusinessPartnerListResponse();
            try
            {
                response.EmployeeByBusinessPartners = new List<EmployeeByBusinessPartnerViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.EmployeeByBusinessPartners.AddRange(unitOfWork.GetEmployeeByBusinessPartnerRepository()
                        .GetEmployeeByBusinessPartnersNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToEmployeeByBusinessPartnerViewModelList() ?? new List<EmployeeByBusinessPartnerViewModel>());
                }
                else
                {
                    response.EmployeeByBusinessPartners.AddRange(unitOfWork.GetEmployeeByBusinessPartnerRepository()
                        .GetEmployeeByBusinessPartners(request.CompanyId)
                        ?.ConvertToEmployeeByBusinessPartnerViewModelList() ?? new List<EmployeeByBusinessPartnerViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartners = new List<EmployeeByBusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
