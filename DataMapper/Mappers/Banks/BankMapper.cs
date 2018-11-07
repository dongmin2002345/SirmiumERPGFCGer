using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DomainCore.Banks;
using ServiceInterfaces.ViewModels.Banks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Banks
{
    public static class BankMapper
    {
		public static List<BankViewModel> ConvertToBankViewModelList(this IEnumerable<Bank> banks)
		{
			List<BankViewModel> banksViewModels = new List<BankViewModel>();
			foreach (Bank bank in banks)
			{
				banksViewModels.Add(bank.ConvertToBankViewModel());
			}
			return banksViewModels;
		}

		public static BankViewModel ConvertToBankViewModel(this Bank bank)
		{
			BankViewModel bankViewModel = new BankViewModel()
			{
				Id = bank.Id,
				Identifier = bank.Identifier,

				Code = bank.Code,
				Name = bank.Name,
                Swift = bank.Swift,

				Country = bank.Country?.ConvertToCountryViewModelLite(),

                IsActive = bank.Active,

				CreatedBy = bank.CreatedBy?.ConvertToUserViewModelLite(),
				Company = bank.Company?.ConvertToCompanyViewModelLite(),

				UpdatedAt = bank.UpdatedAt,
				CreatedAt = bank.CreatedAt

			};
			return bankViewModel;
		}

		public static List<BankViewModel> ConvertToBankViewModelListLite(this IEnumerable<Bank> banks)
		{
			List<BankViewModel> bankViewModels = new List<BankViewModel>();
			foreach (Bank bank in banks)
			{
				bankViewModels.Add(bank.ConvertToBankViewModelLite());
			}
			return bankViewModels;
		}


		public static BankViewModel ConvertToBankViewModelLite(this Bank bank)
		{
			BankViewModel bankViewModel = new BankViewModel()
			{
				Id = bank.Id,
				Identifier = bank.Identifier,

				Code = bank.Code,
				Name = bank.Name,
                Swift = bank.Swift,

                IsActive = bank.Active,

                CreatedAt = bank.CreatedAt,
				UpdatedAt = bank.UpdatedAt
			};
			return bankViewModel;
		}

		public static Bank ConvertToBank(this BankViewModel bankViewModel)
		{
			Bank bank = new Bank()
			{
				Id = bankViewModel.Id,
				Identifier = bankViewModel.Identifier,

				Code = bankViewModel.Code,
				Name = bankViewModel.Name,
                Swift = bankViewModel.Swift,

                CountryId = bankViewModel.Country?.Id ?? null,

				CreatedById = bankViewModel.CreatedBy?.Id ?? null,
				CompanyId = bankViewModel.Company?.Id ?? null,

				CreatedAt = bankViewModel.CreatedAt,
				UpdatedAt = bankViewModel.UpdatedAt

			};
			return bank;
		}
	}
}
