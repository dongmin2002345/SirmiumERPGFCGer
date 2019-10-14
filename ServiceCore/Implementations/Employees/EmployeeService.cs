using DataMapper.Mappers.Employees;
using DomainCore.Employees;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Gloabals;
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
                //Backup items
                List<EmployeeCardViewModel> employeeCards = Employee
                    .EmployeeCards?.ToList() ?? new List<EmployeeCardViewModel>();
                Employee.EmployeeCards = null;

                List<EmployeeDocumentViewModel> employeeDocuments = Employee
                    .EmployeeDocuments?.ToList() ?? new List<EmployeeDocumentViewModel>();
                Employee.EmployeeDocuments = null;

                List<EmployeeItemViewModel> employeeItems = Employee
                    .EmployeeItems?.ToList() ?? new List<EmployeeItemViewModel>();
                Employee.EmployeeItems = null;

                List<EmployeeLicenceItemViewModel> employeeLicences = Employee
                    .EmployeeLicences?.ToList() ?? new List<EmployeeLicenceItemViewModel>();
                Employee.EmployeeLicences = null;

                List<EmployeeNoteViewModel> employeeNotes = Employee
                    .EmployeeNotes?.ToList() ?? new List<EmployeeNoteViewModel>();
                Employee.EmployeeNotes = null;

                List<EmployeeProfessionItemViewModel> employeeProfessions = Employee
                    .EmployeeProfessions?.ToList() ?? new List<EmployeeProfessionItemViewModel>();
                Employee.EmployeeProfessions = null;

                Employee createdEmployee = unitOfWork.GetEmployeeRepository()
                    .Create(Employee.ConvertToEmployee());

                // Update items
                if (employeeCards != null && employeeCards.Count > 0)
                {
                    foreach (EmployeeCardViewModel item in employeeCards
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<EmployeeCardViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetEmployeeCardRepository().Create(item.ConvertToEmployeeCard());
                    }

                    foreach (EmployeeCardViewModel item in employeeCards
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<EmployeeCardViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        unitOfWork.GetEmployeeCardRepository().Create(item.ConvertToEmployeeCard());

                        unitOfWork.GetEmployeeCardRepository().Delete(item.Identifier);
                    }

                }

                // Update items
                if (employeeDocuments != null && employeeDocuments.Count > 0)
                {
                    foreach (EmployeeDocumentViewModel item in employeeDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<EmployeeDocumentViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetEmployeeDocumentRepository().Create(item.ConvertToEmployeeDocument());
                    }

                    foreach (EmployeeDocumentViewModel item in employeeDocuments
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<EmployeeDocumentViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        unitOfWork.GetEmployeeDocumentRepository().Create(item.ConvertToEmployeeDocument());

                        unitOfWork.GetEmployeeDocumentRepository().Delete(item.Identifier);
                    }

                }

                // Update items
                if (employeeItems != null && employeeItems.Count > 0)
                {
                    foreach (EmployeeItemViewModel item in employeeItems
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<EmployeeItemViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetEmployeeItemRepository().Create(item.ConvertToEmployeeItem());
                    }

                    foreach (EmployeeItemViewModel item in employeeItems
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<EmployeeItemViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        unitOfWork.GetEmployeeItemRepository().Create(item.ConvertToEmployeeItem());

                        unitOfWork.GetEmployeeItemRepository().Delete(item.Identifier);
                    }

                }

                // Update items
                if (employeeLicences != null && employeeLicences.Count > 0)
                {
                    foreach (EmployeeLicenceItemViewModel item in employeeLicences
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<EmployeeLicenceItemViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetEmployeeLicenceRepository().Create(item.ConvertToEmployeeLicence());
                    }

                    foreach (EmployeeLicenceItemViewModel item in employeeLicences
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<EmployeeLicenceItemViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        unitOfWork.GetEmployeeLicenceRepository().Create(item.ConvertToEmployeeLicence());

                        unitOfWork.GetEmployeeLicenceRepository().Delete(item.Identifier);
                    }

                }

                // Update items
                if (employeeNotes != null && employeeNotes.Count > 0)
                {
                    foreach (EmployeeNoteViewModel item in employeeNotes
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<EmployeeNoteViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetEmployeeNoteRepository().Create(item.ConvertToEmployeeNote());
                    }

                    foreach (EmployeeNoteViewModel item in employeeNotes
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<EmployeeNoteViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        unitOfWork.GetEmployeeNoteRepository().Create(item.ConvertToEmployeeNote());

                        unitOfWork.GetEmployeeNoteRepository().Delete(item.Identifier);
                    }

                }

                // Update items
                if (employeeProfessions != null && employeeProfessions.Count > 0)
                {
                    foreach (EmployeeProfessionItemViewModel item in employeeProfessions
                        .Where(x => x.ItemStatus == ItemStatus.Added || x.ItemStatus == ItemStatus.Edited)?.ToList() ?? new List<EmployeeProfessionItemViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        item.ItemStatus = ItemStatus.Submited;
                        var createdItem = unitOfWork.GetEmployeeProfessionRepository().Create(item.ConvertToEmployeeProfession());
                    }

                    foreach (EmployeeProfessionItemViewModel item in employeeProfessions
                        .Where(x => x.ItemStatus == ItemStatus.Deleted)?.ToList() ?? new List<EmployeeProfessionItemViewModel>())
                    {
                        item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
                        unitOfWork.GetEmployeeProfessionRepository().Create(item.ConvertToEmployeeProfession());

                        unitOfWork.GetEmployeeProfessionRepository().Delete(item.Identifier);
                    }

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

        public EmployeeResponse Delete(Guid identifier)
        {
            EmployeeResponse response = new EmployeeResponse();
            try
            {
                response.Employee = unitOfWork.GetEmployeeRepository().Delete(identifier)?.ConvertToEmployeeViewModel();
                unitOfWork.Save();

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
