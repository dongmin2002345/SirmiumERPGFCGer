using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.ConstructionSites
{
    public static class ConstructionSiteCalculationMapper
    {
        public static List<ConstructionSiteCalculationViewModel> ConvertToConstructionSiteCalculationViewModelList(this IEnumerable<ConstructionSiteCalculation> ConstructionSiteCalculations)
        {
            List<ConstructionSiteCalculationViewModel> ConstructionSiteCalculationViewModels = new List<ConstructionSiteCalculationViewModel>();
            foreach (ConstructionSiteCalculation ConstructionSiteCalculation in ConstructionSiteCalculations)
            {
                ConstructionSiteCalculationViewModels.Add(ConstructionSiteCalculation.ConvertToConstructionSiteCalculationViewModel());
            }
            return ConstructionSiteCalculationViewModels;
        }

        public static ConstructionSiteCalculationViewModel ConvertToConstructionSiteCalculationViewModel(this ConstructionSiteCalculation ConstructionSiteCalculation)
        {
            ConstructionSiteCalculationViewModel ConstructionSiteCalculationViewModel = new ConstructionSiteCalculationViewModel()
            {
                Id = ConstructionSiteCalculation.Id,
                Identifier = ConstructionSiteCalculation.Identifier,

                ConstructionSite = ConstructionSiteCalculation.ConstructionSite?.ConvertToConstructionSiteViewModelLite(),

                NumOfEmployees = ConstructionSiteCalculation.NumOfEmployees,
                EmployeePrice = ConstructionSiteCalculation.EmployeePrice,
                NumOfMonths = ConstructionSiteCalculation.NumOfMonths,

                OldValue = ConstructionSiteCalculation.OldValue, 
                NewValue = ConstructionSiteCalculation.NewValue, 
                ValueDifference = ConstructionSiteCalculation.ValueDifference,

                PlusMinus = ConstructionSiteCalculation.PlusMinus,

                IsActive = ConstructionSiteCalculation.Active,

                CreatedBy = ConstructionSiteCalculation.CreatedBy?.ConvertToUserViewModelLite(),
                Company = ConstructionSiteCalculation.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = ConstructionSiteCalculation.UpdatedAt,
                CreatedAt = ConstructionSiteCalculation.CreatedAt
            };

            return ConstructionSiteCalculationViewModel;
        }

        public static ConstructionSiteCalculationViewModel ConvertToConstructionSiteCalculationViewModelLite(this ConstructionSiteCalculation ConstructionSiteCalculation)
        {
            ConstructionSiteCalculationViewModel ConstructionSiteCalculationViewModel = new ConstructionSiteCalculationViewModel()
            {
                Id = ConstructionSiteCalculation.Id,
                Identifier = ConstructionSiteCalculation.Identifier,

                NumOfEmployees = ConstructionSiteCalculation.NumOfEmployees,
                EmployeePrice = ConstructionSiteCalculation.EmployeePrice,
                NumOfMonths = ConstructionSiteCalculation.NumOfMonths,

                OldValue = ConstructionSiteCalculation.OldValue,
                NewValue = ConstructionSiteCalculation.NewValue,
                ValueDifference = ConstructionSiteCalculation.ValueDifference,

                PlusMinus = ConstructionSiteCalculation.PlusMinus,

                IsActive = ConstructionSiteCalculation.Active,

                UpdatedAt = ConstructionSiteCalculation.UpdatedAt,
                CreatedAt = ConstructionSiteCalculation.CreatedAt
            };

            return ConstructionSiteCalculationViewModel;
        }

        public static ConstructionSiteCalculation ConvertToConstructionSiteCalculation(this ConstructionSiteCalculationViewModel ConstructionSiteCalculationViewModel)
        {
            ConstructionSiteCalculation ConstructionSiteCalculation = new ConstructionSiteCalculation()
            {
                Id = ConstructionSiteCalculationViewModel.Id,
                Identifier = ConstructionSiteCalculationViewModel.Identifier,

                ConstructionSiteId = ConstructionSiteCalculationViewModel.ConstructionSite?.Id ?? null,

                NumOfEmployees = ConstructionSiteCalculationViewModel.NumOfEmployees,
                EmployeePrice = ConstructionSiteCalculationViewModel.EmployeePrice,
                NumOfMonths = ConstructionSiteCalculationViewModel.NumOfMonths,

                OldValue = ConstructionSiteCalculationViewModel.OldValue,
                NewValue = ConstructionSiteCalculationViewModel.NewValue,
                ValueDifference = ConstructionSiteCalculationViewModel.ValueDifference,

                PlusMinus = ConstructionSiteCalculationViewModel.PlusMinus,

                CreatedById = ConstructionSiteCalculationViewModel.CreatedBy?.Id ?? null,
                CompanyId = ConstructionSiteCalculationViewModel.Company?.Id ?? null,

                CreatedAt = ConstructionSiteCalculationViewModel.CreatedAt,
                UpdatedAt = ConstructionSiteCalculationViewModel.UpdatedAt
            };

            return ConstructionSiteCalculation;
        }
    }
}
