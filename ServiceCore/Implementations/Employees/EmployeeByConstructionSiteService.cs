using DataMapper.Mappers.Employees;
using DomainCore.ConstructionSites;
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
    public class EmployeeByConstructionSiteService : IEmployeeByConstructionSiteService
    {
        IUnitOfWork unitOfWork;

        public EmployeeByConstructionSiteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmployeeByConstructionSiteListResponse GetEmployeeByConstructionSites(int companyId)
        {
            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteListResponse();
            try
            {
                response.EmployeeByConstructionSites = unitOfWork.GetEmployeeByConstructionSiteRepository().GetEmployeeByConstructionSites(companyId)
               .ConvertToEmployeeByConstructionSiteViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSites = new List<EmployeeByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteListResponse GetEmployeeByConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.EmployeeByConstructionSites = unitOfWork.GetEmployeeByConstructionSiteRepository()
                        .GetEmployeeByConstructionSitesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToEmployeeByConstructionSiteViewModelList();
                }
                else
                {
                    response.EmployeeByConstructionSites = unitOfWork.GetEmployeeByConstructionSiteRepository()
                        .GetEmployeeByConstructionSites(companyId)
                        .ConvertToEmployeeByConstructionSiteViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSites = new List<EmployeeByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteResponse Create(EmployeeByConstructionSiteViewModel re)
        {
            EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteResponse();
            try
            {
                ConstructionSite constructionSite = unitOfWork.GetConstructionSiteRepository().GetConstructionSite(re.ConstructionSite.Id);

                Employee employee = unitOfWork.GetEmployeeRepository().GetEmployee(re.Employee.Id);
                employee.ConstructionSiteCode = constructionSite.InternalCode;
                employee.ConstructionSiteName = constructionSite.Name;
                employee.UpdatedAt = DateTime.Now;

                EmployeeByConstructionSite addedEmployeeByConstructionSite = unitOfWork.GetEmployeeByConstructionSiteRepository().Create(re.ConvertToEmployeeByConstructionSite());

                EmployeeCard ec = new EmployeeCard()
                {
                    Identifier = Guid.NewGuid(),
                    EmployeeId = re.Employee.Id,
                    CardDate = addedEmployeeByConstructionSite.StartDate,
                    Description = "Radnik " + re.Employee?.Name + " je krenuo da radi na gradilištu " + re.ConstructionSite?.Name + ". Radnik je na gradilištu od: " + addedEmployeeByConstructionSite.StartDate.ToString("dd.MM.yyyy") + " do " + addedEmployeeByConstructionSite.EndDate.ToString("dd.MM.yyyy"),
                    PlusMinus = "+",
                    CreatedById = re.CreatedBy?.Id,
                    CompanyId = re.Company?.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                unitOfWork.GetEmployeeCardRepository().Create(ec);

                unitOfWork.Save();

                response.EmployeeByConstructionSite = addedEmployeeByConstructionSite.ConvertToEmployeeByConstructionSiteViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSite = new EmployeeByConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteResponse Delete(EmployeeByConstructionSiteViewModel re)
        {
            EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteResponse();
            try
            {
                EmployeeByConstructionSite deletedEmployeeByConstructionSite = unitOfWork.GetEmployeeByConstructionSiteRepository().Delete(re.ConvertToEmployeeByConstructionSite());

                Employee employee = unitOfWork.GetEmployeeRepository().GetEmployee((int)deletedEmployeeByConstructionSite.EmployeeId);
                employee.ConstructionSiteCode = "";
                employee.ConstructionSiteName = "";
                employee.UpdatedAt = DateTime.Now;

                EmployeeCard ec = new EmployeeCard()
                {
                    Identifier = Guid.NewGuid(),
                    EmployeeId = deletedEmployeeByConstructionSite.Employee.Id,
                    CardDate = (DateTime)deletedEmployeeByConstructionSite.RealEndDate,
                    Description = "Radnik " + deletedEmployeeByConstructionSite.Employee?.Name + " je prestao da radi na gradilištu " + deletedEmployeeByConstructionSite.ConstructionSite?.Name + ". Prestanak je od: " + ((DateTime)deletedEmployeeByConstructionSite.RealEndDate).ToString("dd.MM.yyyy"),
                    CreatedById = deletedEmployeeByConstructionSite.CreatedBy?.Id,
                    PlusMinus = "-",
                    CompanyId = deletedEmployeeByConstructionSite.Company?.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                unitOfWork.GetEmployeeCardRepository().Create(ec);

                unitOfWork.Save();

                response.EmployeeByConstructionSite = deletedEmployeeByConstructionSite.ConvertToEmployeeByConstructionSiteViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSite = new EmployeeByConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteListResponse Sync(SyncEmployeeByConstructionSiteRequest request)
        {
            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteListResponse();
            try
            {
                response.EmployeeByConstructionSites = new List<EmployeeByConstructionSiteViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.EmployeeByConstructionSites.AddRange(unitOfWork.GetEmployeeByConstructionSiteRepository()
                        .GetEmployeeByConstructionSitesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToEmployeeByConstructionSiteViewModelList() ?? new List<EmployeeByConstructionSiteViewModel>());
                }
                else
                {
                    response.EmployeeByConstructionSites.AddRange(unitOfWork.GetEmployeeByConstructionSiteRepository()
                        .GetEmployeeByConstructionSites(request.CompanyId)
                        ?.ConvertToEmployeeByConstructionSiteViewModelList() ?? new List<EmployeeByConstructionSiteViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSites = new List<EmployeeByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
