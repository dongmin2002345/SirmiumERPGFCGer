using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Identity;
using DomainCore.Common.Individuals;
using ServiceInterfaces.ViewModels.Common.Individuals;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Individuals
{
    public static class IndividualMapper
    {
        public static List<IndividualViewModel> ConvertToIndividualViewModelList(this IEnumerable<Individual> individuals)
        {
            List<IndividualViewModel> individualViewModels = new List<IndividualViewModel>();
            foreach (Individual individual in individuals)
            {
                individualViewModels.Add(individual.ConvertToIndividualViewModel());
            }
            return individualViewModels;
        }

        public static List<IndividualViewModel> ConvertToIndividualViewModelListLite(this IEnumerable<Individual> individuals)
        {
            List<IndividualViewModel> individualViewModels = new List<IndividualViewModel>();
            foreach (Individual individual in individuals)
            {
                individualViewModels.Add(individual.ConvertToIndividualViewModelLite());
            }
            return individualViewModels;
        }

        public static List<Individual> ConvertToIndividualList(this List<IndividualViewModel> individuals)
        {
            List<Individual> individualList = new List<Individual>();
            foreach (IndividualViewModel individual in individuals)
            {
                individualList.Add(individual.ConvertToIndividual());
            }
            return individualList;
        }

        public static IndividualViewModel ConvertToIndividualViewModel(this Individual individual)
        {
            IndividualViewModel individualViewModel = new IndividualViewModel()
            {
                Id = individual.Id,
                Identifier = individual.Identifier,

                Code = individual.Code,
                Name = individual.Name,
                SurName = individual.SurName,

                DateOfBirth = individual.DateOfBirth,

                Address = individual.Address,
                Passport = individual.Passport,
                Interest = individual.Interest,
                License = individual.License,

                EmbassyDate = individual.EmbassyDate,
                VisaFrom = individual.VisaFrom, 
                VisaTo = individual.VisaTo,
                WorkPermitFrom = individual.WorkPermitFrom,
                WorkPermitTo = individual.WorkPermitTo,

                Family = individual.Family,

                UpdatedAt = individual.UpdatedAt,
                CreatedBy = individual.CreatedBy?.ConvertToUserViewModelLite(),
                CreatedAt = individual.CreatedAt
            };

            return individualViewModel;
        }

        public static IndividualViewModel ConvertToIndividualViewModelLite(this Individual individual)
        {
            IndividualViewModel individualViewModel = new IndividualViewModel()
            {
                Id = individual.Id,
                Identifier = individual.Identifier,

                Code = individual.Code,
                Name = individual.Name,
                SurName = individual.SurName,

                DateOfBirth = individual.DateOfBirth,

                Address = individual.Address,
                Passport = individual.Passport,
                Interest = individual.Interest,
                License = individual.License,

                EmbassyDate = individual.EmbassyDate,
                VisaFrom = individual.VisaFrom,
                VisaTo = individual.VisaTo,
                WorkPermitFrom = individual.WorkPermitFrom,
                WorkPermitTo = individual.WorkPermitTo,

                Family = individual.Family,

                UpdatedAt = individual.UpdatedAt,
                CreatedAt = individual.CreatedAt
            };
            return individualViewModel;
        }

        public static Individual ConvertToIndividual(this IndividualViewModel individualViewModel)
        {
            Individual individual = new Individual()
            {
                Id = individualViewModel.Id,
                Identifier = individualViewModel.Identifier,

                Code = individualViewModel.Code,
                Name = individualViewModel.Name,
                SurName = individualViewModel.SurName,

                DateOfBirth = individualViewModel.DateOfBirth,

                Address = individualViewModel.Address,
                Passport = individualViewModel.Passport,
                Interest = individualViewModel.Interest,
                License = individualViewModel.License,

                EmbassyDate = individualViewModel.EmbassyDate,
                VisaFrom = individualViewModel.VisaFrom,
                VisaTo = individualViewModel.VisaTo,
                WorkPermitFrom = individualViewModel.WorkPermitFrom,
                WorkPermitTo = individualViewModel.WorkPermitTo,

                Family = individualViewModel.Family,

                CreatedBy = new User() { Id = individualViewModel.CreatedBy?.Id ?? 0 },
                CreatedAt = individualViewModel.CreatedAt
            };
            return individual;

        }
    }
}
