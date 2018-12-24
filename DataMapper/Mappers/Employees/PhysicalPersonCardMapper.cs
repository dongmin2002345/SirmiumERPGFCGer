using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class PhysicalPersonCardMapper
    {
        public static List<PhysicalPersonCardViewModel> ConvertToPhysicalPersonCardViewModelList(this IEnumerable<PhysicalPersonCard> employeeCards)
        {
            List<PhysicalPersonCardViewModel> PhysicalPersonCardViewModels = new List<PhysicalPersonCardViewModel>();
            foreach (PhysicalPersonCard PhysicalPersonCard in employeeCards)
            {
                PhysicalPersonCardViewModels.Add(PhysicalPersonCard.ConvertToPhysicalPersonCardViewModel());
            }
            return PhysicalPersonCardViewModels;
        }

        public static PhysicalPersonCardViewModel ConvertToPhysicalPersonCardViewModel(this PhysicalPersonCard employeeCard)
        {
            PhysicalPersonCardViewModel PhysicalPersonCardViewModel = new PhysicalPersonCardViewModel()
            {
                Id = employeeCard.Id,
                Identifier = employeeCard.Identifier,

                PhysicalPerson = employeeCard.PhysicalPerson?.ConvertToPhysicalPersonViewModelLite(),

                CardDate = employeeCard.CardDate,
                Description = employeeCard.Description,
                PlusMinus = employeeCard.PlusMinus,

                IsActive = employeeCard.Active,

                CreatedBy = employeeCard.CreatedBy?.ConvertToUserViewModelLite(),
                Company = employeeCard.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = employeeCard.UpdatedAt,
                CreatedAt = employeeCard.CreatedAt
            };

            return PhysicalPersonCardViewModel;
        }

        public static PhysicalPersonCardViewModel ConvertToPhysicalPersonCardViewModelLite(this PhysicalPersonCard employeeCard)
        {
            PhysicalPersonCardViewModel PhysicalPersonCardViewModel = new PhysicalPersonCardViewModel()
            {
                Id = employeeCard.Id,
                Identifier = employeeCard.Identifier,

                CardDate = employeeCard.CardDate,
                Description = employeeCard.Description,
                PlusMinus = employeeCard.PlusMinus,

                IsActive = employeeCard.Active,

                UpdatedAt = employeeCard.UpdatedAt,
                CreatedAt = employeeCard.CreatedAt
            };

            return PhysicalPersonCardViewModel;
        }

        public static PhysicalPersonCard ConvertToPhysicalPersonCard(this PhysicalPersonCardViewModel employeeCardViewModel)
        {
            PhysicalPersonCard PhysicalPersonCard = new PhysicalPersonCard()
            {
                Id = employeeCardViewModel.Id,
                Identifier = employeeCardViewModel.Identifier,

                PhysicalPersonId = employeeCardViewModel.PhysicalPerson?.Id ?? null,

                CardDate = (DateTime)employeeCardViewModel.CardDate,
                Description = employeeCardViewModel.Description,
                PlusMinus = employeeCardViewModel.PlusMinus,

                Active = employeeCardViewModel.IsActive,

                CreatedById = employeeCardViewModel.CreatedBy?.Id ?? null,
                CompanyId = employeeCardViewModel.Company?.Id ?? null,

                CreatedAt = employeeCardViewModel.CreatedAt,
                UpdatedAt = employeeCardViewModel.UpdatedAt
            };

            return PhysicalPersonCard;
        }
    }
}
