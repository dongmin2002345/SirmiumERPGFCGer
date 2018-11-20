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
	public class PhysicalPersonService : IPhysicalPersonService
	{
		IUnitOfWork unitOfWork;

		public PhysicalPersonService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public PhysicalPersonListResponse GetPhysicalPersons(int companyId)
		{
			PhysicalPersonListResponse response = new PhysicalPersonListResponse();
			try
			{
				response.PhysicalPersons = unitOfWork.GetPhysicalPersonRepository()
					.GetPhysicalPersons(companyId)
					.ConvertToPhysicalPersonViewModelList();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.PhysicalPersons = new List<PhysicalPersonViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public PhysicalPersonListResponse GetPhysicalPersonsNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			PhysicalPersonListResponse response = new PhysicalPersonListResponse();
			try
			{
				if (lastUpdateTime != null)
				{
					response.PhysicalPersons = unitOfWork.GetPhysicalPersonRepository()
						.GetPhysicalPersonsNewerThen(companyId, (DateTime)lastUpdateTime)
						.ConvertToPhysicalPersonViewModelList();
				}
				else
				{
					response.PhysicalPersons = unitOfWork.GetPhysicalPersonRepository()
						.GetPhysicalPersons(companyId)
						.ConvertToPhysicalPersonViewModelList();
				}
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.PhysicalPersons = new List<PhysicalPersonViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public PhysicalPersonResponse Create(PhysicalPersonViewModel physicalPerson)
		{
			PhysicalPersonResponse response = new PhysicalPersonResponse();
			try
			{
				//// Backup items
				//List<EmployeeItemViewModel> EmployeeItems = Employee.EmployeeItems?.ToList() ?? new List<EmployeeItemViewModel>();
				//Employee.EmployeeItems = null;

				//List<EmployeeProfessionItemViewModel> EmployeeProfessions = Employee.EmployeeProfessions?.ToList() ?? new List<EmployeeProfessionItemViewModel>();
				//Employee.EmployeeProfessions = null;

				//List<EmployeeLicenceItemViewModel> EmployeeLicences = Employee.EmployeeLicences?.ToList() ?? new List<EmployeeLicenceItemViewModel>();
				//Employee.EmployeeLicences = null;

				//List<EmployeeNoteViewModel> EmployeeNotes = Employee.EmployeeNotes?.ToList() ?? new List<EmployeeNoteViewModel>();
				//Employee.EmployeeNotes = null;

				//// Create animal input note
				//Employee createdEmployee = unitOfWork.GetEmployeeRepository()
				//	.Create(Employee.ConvertToEmployee());

				//// Update items
				//var EmployeeItemsFromDB = unitOfWork.GetEmployeeItemRepository().GetEmployeeItemsByEmployee(createdEmployee.Id);
				//foreach (var item in EmployeeItemsFromDB)
				//	if (!EmployeeItems.Select(x => x.Identifier).Contains(item.Identifier))
				//		unitOfWork.GetEmployeeItemRepository().Delete(item.Identifier);
				//foreach (var item in EmployeeItems)
				//{
				//	item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
				//	unitOfWork.GetEmployeeItemRepository().Create(item.ConvertToEmployeeItem());
				//}



				//// Update items
				//var EmployeeLicencesFromDB = unitOfWork.GetEmployeeLicenceRepository().GetEmployeeItemsByEmployee(createdEmployee.Id);
				//foreach (var item in EmployeeLicencesFromDB)
				//	if (!EmployeeProfessions.Select(x => x.Identifier).Contains(item.Identifier))
				//		unitOfWork.GetEmployeeProfessionRepository().Delete(item.Identifier);
				//foreach (var item in EmployeeProfessions)
				//{
				//	item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
				//	unitOfWork.GetEmployeeProfessionRepository().Create(item.ConvertToEmployeeProfession());
				//}

				//// Update items
				//var EmployeeProfessionsFromDB = unitOfWork.GetEmployeeProfessionRepository().GetEmployeeItemsByEmployee(createdEmployee.Id);
				//foreach (var item in EmployeeProfessionsFromDB)
				//	if (!EmployeeProfessions.Select(x => x.Identifier).Contains(item.Identifier))
				//		unitOfWork.GetEmployeeProfessionRepository().Delete(item.Identifier);
				//foreach (var item in EmployeeProfessions)
				//{
				//	item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
				//	unitOfWork.GetEmployeeProfessionRepository().Create(item.ConvertToEmployeeProfession());
				//}

				//// Update items
				//var EmployeeNotesFromDB = unitOfWork.GetEmployeeNoteRepository().GetEmployeeNotesByEmployee(createdEmployee.Id);
				//foreach (var item in EmployeeNotesFromDB)
				//	if (!EmployeeNotes.Select(x => x.Identifier).Contains(item.Identifier))
				//		unitOfWork.GetEmployeeNoteRepository().Delete(item.Identifier);
				//foreach (var item in EmployeeNotes)
				//{
				//	item.Employee = new EmployeeViewModel() { Id = createdEmployee.Id };
				//	unitOfWork.GetEmployeeNoteRepository().Create(item.ConvertToEmployeeNote());
				//}

				//unitOfWork.Save();

				//response.Employee = createdEmployee.ConvertToEmployeeViewModel();
				//response.Success = true;
				PhysicalPerson addedPhysicalPerson = unitOfWork.GetPhysicalPersonRepository().Create(physicalPerson.ConvertToPhysicalPerson());
				unitOfWork.Save();

				response.PhysicalPerson = addedPhysicalPerson.ConvertToPhysicalPersonViewModel();
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.PhysicalPerson = new PhysicalPersonViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;

		}

		public PhysicalPersonResponse Delete(Guid identifier)
		{
			PhysicalPersonResponse response = new PhysicalPersonResponse();
			try
			{
				response.PhysicalPerson = unitOfWork.GetPhysicalPersonRepository().Delete(identifier)?.ConvertToPhysicalPersonViewModel();
				unitOfWork.Save();

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.PhysicalPerson = new PhysicalPersonViewModel();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public PhysicalPersonListResponse Sync(SyncPhysicalPersonRequest request)
		{
			PhysicalPersonListResponse response = new PhysicalPersonListResponse();
			try
			{
				response.PhysicalPersons = new List<PhysicalPersonViewModel>();

				if (request.LastUpdatedAt != null)
				{
					response.PhysicalPersons.AddRange(unitOfWork.GetPhysicalPersonRepository()
						.GetPhysicalPersonsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
						?.ConvertToPhysicalPersonViewModelList() ?? new List<PhysicalPersonViewModel>());
				}
				else
				{
					response.PhysicalPersons.AddRange(unitOfWork.GetPhysicalPersonRepository()
						.GetPhysicalPersons(request.CompanyId)
						?.ConvertToPhysicalPersonViewModelList() ?? new List<PhysicalPersonViewModel>());
				}

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.PhysicalPersons = new List<PhysicalPersonViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
