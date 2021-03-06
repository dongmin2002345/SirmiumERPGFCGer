﻿using DataMapper.Mappers.Common.Companies;
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
        public static List<ConstructionSiteCalculationViewModel> ConvertToConstructionSiteCalculationViewModelList(this IEnumerable<ConstructionSiteCalculation> constructionSiteCalculations)
        {
            List<ConstructionSiteCalculationViewModel> constructionSiteCalculationViewModels = new List<ConstructionSiteCalculationViewModel>();
            foreach (ConstructionSiteCalculation constructionSiteCalculation in constructionSiteCalculations)
            {
                constructionSiteCalculationViewModels.Add(constructionSiteCalculation.ConvertToConstructionSiteCalculationViewModel());
            }
            return constructionSiteCalculationViewModels;
        }

        public static ConstructionSiteCalculationViewModel ConvertToConstructionSiteCalculationViewModel(this ConstructionSiteCalculation constructionSiteCalculation)
        {
            ConstructionSiteCalculationViewModel constructionSiteCalculationViewModel = new ConstructionSiteCalculationViewModel()
            {
                Id = constructionSiteCalculation.Id,
                Identifier = constructionSiteCalculation.Identifier,

                ConstructionSite = constructionSiteCalculation.ConstructionSite?.ConvertToConstructionSiteViewModelLite(),

                StatusDate = constructionSiteCalculation.StatusDate,
                NumOfEmployees = constructionSiteCalculation.NumOfEmployees,
                EmployeePrice = constructionSiteCalculation.EmployeePrice,
                NumOfMonths = constructionSiteCalculation.NumOfMonths,

                OldValue = constructionSiteCalculation.OldValue, 
                NewValue = constructionSiteCalculation.NewValue, 
                ValueDifference = constructionSiteCalculation.ValueDifference,

                PlusMinus = constructionSiteCalculation.PlusMinus,
                ItemStatus = constructionSiteCalculation.ItemStatus,
                IsPaid = constructionSiteCalculation.IsPaid,
                IsRefunded = constructionSiteCalculation.IsRefunded,
                DateCondition = constructionSiteCalculation.DateCondition,

                IsActive = constructionSiteCalculation.Active,

                CreatedBy = constructionSiteCalculation.CreatedBy?.ConvertToUserViewModelLite(),
                Company = constructionSiteCalculation.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = constructionSiteCalculation.UpdatedAt,
                CreatedAt = constructionSiteCalculation.CreatedAt
            };

            return constructionSiteCalculationViewModel;
        }

        public static ConstructionSiteCalculationViewModel ConvertToConstructionSiteCalculationViewModelLite(this ConstructionSiteCalculation constructionSiteCalculation)
        {
            ConstructionSiteCalculationViewModel constructionSiteCalculationViewModel = new ConstructionSiteCalculationViewModel()
            {
                Id = constructionSiteCalculation.Id,
                Identifier = constructionSiteCalculation.Identifier,

                StatusDate= constructionSiteCalculation.StatusDate,
                NumOfEmployees = constructionSiteCalculation.NumOfEmployees,
                EmployeePrice = constructionSiteCalculation.EmployeePrice,
                NumOfMonths = constructionSiteCalculation.NumOfMonths,

                OldValue = constructionSiteCalculation.OldValue,
                NewValue = constructionSiteCalculation.NewValue,
                ValueDifference = constructionSiteCalculation.ValueDifference,

                PlusMinus = constructionSiteCalculation.PlusMinus,
                ItemStatus = constructionSiteCalculation.ItemStatus,
                IsPaid = constructionSiteCalculation.IsPaid,
                IsRefunded = constructionSiteCalculation.IsRefunded,
                DateCondition = constructionSiteCalculation.DateCondition,
                IsActive = constructionSiteCalculation.Active,

                UpdatedAt = constructionSiteCalculation.UpdatedAt,
                CreatedAt = constructionSiteCalculation.CreatedAt
            };

            return constructionSiteCalculationViewModel;
        }

        public static ConstructionSiteCalculation ConvertToConstructionSiteCalculation(this ConstructionSiteCalculationViewModel constructionSiteCalculationViewModel)
        {
            ConstructionSiteCalculation constructionSiteCalculation = new ConstructionSiteCalculation()
            {
                Id = constructionSiteCalculationViewModel.Id,
                Identifier = constructionSiteCalculationViewModel.Identifier,

                ConstructionSiteId = constructionSiteCalculationViewModel.ConstructionSite?.Id ?? null,

                StatusDate = constructionSiteCalculationViewModel.StatusDate,
                NumOfEmployees = constructionSiteCalculationViewModel.NumOfEmployees,
                EmployeePrice = constructionSiteCalculationViewModel.EmployeePrice,
                NumOfMonths = constructionSiteCalculationViewModel.NumOfMonths,

                OldValue = constructionSiteCalculationViewModel.OldValue,
                NewValue = constructionSiteCalculationViewModel.NewValue,
                ValueDifference = constructionSiteCalculationViewModel.ValueDifference,

                PlusMinus = constructionSiteCalculationViewModel.PlusMinus,
                ItemStatus = constructionSiteCalculationViewModel.ItemStatus,
                IsPaid = constructionSiteCalculationViewModel.IsPaid,
                IsRefunded = constructionSiteCalculationViewModel.IsRefunded,
                DateCondition = constructionSiteCalculationViewModel.DateCondition,
                CreatedById = constructionSiteCalculationViewModel.CreatedBy?.Id ?? null,
                CompanyId = constructionSiteCalculationViewModel.Company?.Id ?? null,

                CreatedAt = constructionSiteCalculationViewModel.CreatedAt,
                UpdatedAt = constructionSiteCalculationViewModel.UpdatedAt
            };

            return constructionSiteCalculation;
        }
    }
}
