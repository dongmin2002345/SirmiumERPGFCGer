using DataMapper.Mappers.Employees;
using DomainCore.Employees;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Implementations.Employees
{
    public class EmployeeService : IEmployeeService
    {
        IUnitOfWork unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmployeeListResponse GetEmployees(int companyId)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            try
            {
                response.Employees = unitOfWork.GetEmployeeRepository()
                    .GetEmployees(companyId)
                    .ConvertToEmployeeViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Employees = new List<EmployeeViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeListResponse GetEmployeesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.Employees = unitOfWork.GetEmployeeRepository()
                        .GetEmployeesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToEmployeeViewModelList();
                }
                else
                {
                    response.Employees = unitOfWork.GetEmployeeRepository()
                        .GetEmployees(companyId)
                        .ConvertToEmployeeViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Employees = new List<EmployeeViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeResponse Create(EmployeeViewModel Employee)
        {
            EmployeeResponse response = new EmployeeResponse();
            try
            {
                // Backup items
                List<EmployeeItemViewModel> EmployeeItems = Employee.EmployeeItems?.ToList() ?? new List<EmployeeItemViewModel>();
                Employee.EmployeeItems = null;

                List<EmployeeProfessionItemViewModel> EmployeeProfessions = Employee.EmployeeProfessions?.ToList() ?? new List<EmployeeProfessionItemViewModel>();
                Employee.EmployeeLicences = null;

                List<EmployeeLicenceItemViewModel> EmployeeLicences = Employee.EmployeeLicences?.ToList() ?? new List<EmployeeLicenceItemViewModel>();
                Employee.EmployeeProfessions = null;

                // Create animal input note
                Employee createdEmployee = unitOfWork.GetEmployeeRepository()
                    .Create(Employee.ConvertToEmployee());

                // Update items
                var EmployeeItemsFromDB = unitOfWork.GetEmployeeItemRepository().GetEmployeeItemsByEmployee(createdEmployee.Id);
                foreach (var item in EmployeeItemsFromDB)
                    if (!EmployeeItems.Select(x => x.Identifier).Contains(item.Identifier))
                        unitOfWork.GetEmployeeItemRepository().Delete(item.Identifier);
                foreach (var item in EmployeeItems)
                {
                    item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                    unitOfWork.GetEmployeeItemRepository().Create(item.ConvertToEmployeeItem());
                }



                // Update items
                var EmployeeLicencesFromDB = unitOfWork.GetEmployeeLicenceRepository().GetEmployeeItemsByEmployee(createdEmployee.Id);
                foreach (var item in EmployeeLicencesFromDB)
                    if (!EmployeeProfessions.Select(x => x.Identifier).Contains(item.Identifier))
                        unitOfWork.GetEmployeeProfessionRepository().Delete(item.Identifier);
                foreach (var item in EmployeeProfessions)
                {
                    item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                    unitOfWork.GetEmployeeProfessionRepository().Create(item.ConvertToEmployeeProfession());
                }

                // Update items
                var EmployeeProfessionsFromDB = unitOfWork.GetEmployeeProfessionRepository().GetEmployeeItemsByEmployee(createdEmployee.Id);
                foreach (var item in EmployeeProfessionsFromDB)
                    if (!EmployeeProfessions.Select(x => x.Identifier).Contains(item.Identifier))
                        unitOfWork.GetEmployeeProfessionRepository().Delete(item.Identifier);
                foreach (var item in EmployeeProfessions)
                {
                    item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                    unitOfWork.GetEmployeeProfessionRepository().Create(item.ConvertToEmployeeProfession());
                }

                unitOfWork.Save();

                response.Employee = createdEmployee.ConvertToEmployeeViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Employee = new EmployeeViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public EmployeeListResponse Sync(SyncEmployeeRequest request)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            try
            {
                response.Employees = new List<EmployeeViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Employees.AddRange(unitOfWork.GetEmployeeRepository()
                        .GetEmployeesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToEmployeeViewModelList() ?? new List<EmployeeViewModel>());
                }
                else
                {
                    response.Employees.AddRange(unitOfWork.GetEmployeeRepository()
                        .GetEmployees(request.CompanyId)
                        ?.ConvertToEmployeeViewModelList() ?? new List<EmployeeViewModel>());
                }

                List<Employee> added = new List<Employee>();
                foreach (var item in request.UnSyncedEmployees)
                {
                    if (item.Id == 0)
                        added.Add(unitOfWork.GetEmployeeRepository().Create(item.ConvertToEmployee()));
                    ////else
                    ////    added.Add(unitOfWork.GetEmployeeRepository().Update(item.ConvertToFoodInputHay()));
                }

                unitOfWork.Save();

                ////foreach (var item in added)
                ////{
                ////    response.Employees.Add(unitOfWork.GetEmployeeRepository().GetEmployees(item.Identifier).C());
                ////}

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Employees = new List<EmployeeViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
