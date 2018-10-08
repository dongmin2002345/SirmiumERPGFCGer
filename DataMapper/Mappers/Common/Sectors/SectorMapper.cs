using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DomainCore.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Sectors
{
  public static  class SectorMapper
    {
		public static List<SectorViewModel> ConvertToSectorViewModelList(this IEnumerable<Sector> sectors)
		{
			List<SectorViewModel> sectorsViewModels = new List<SectorViewModel>();
			foreach (Sector sector in sectors)
			{
				sectorsViewModels.Add(sector.ConvertToSectorViewModel());
			}
			return sectorsViewModels;
		}

		public static SectorViewModel ConvertToSectorViewModel(this Sector sector)
		{
			SectorViewModel sectorViewModel = new SectorViewModel()
			{
				Id = sector.Id,
				Identifier = sector.Identifier,

				Code = sector.Code,
				SecondCode = sector.SecondCode,
				Name = sector.Name,

                Country = sector.Country?.ConvertToCountryViewModelLite(),

                CreatedBy = sector.CreatedBy?.ConvertToUserViewModelLite(),
				Company = sector.Company?.ConvertToCompanyViewModelLite(),

				UpdatedAt = sector.UpdatedAt,
				CreatedAt = sector.CreatedAt

			};
			return sectorViewModel;
		}

		public static List<SectorViewModel> ConvertToSectorViewModelListLite(this IEnumerable<Sector> sectors)
		{
			List<SectorViewModel> sectorViewModels = new List<SectorViewModel>();
			foreach (Sector sector in sectors)
			{
				sectorViewModels.Add(sector.ConvertToSectorViewModelLite());
			}
			return sectorViewModels;
		}


		public static SectorViewModel ConvertToSectorViewModelLite(this Sector sector)
		{
			SectorViewModel sectorViewModel = new SectorViewModel()
			{
				Id = sector.Id,
				Identifier = sector.Identifier,

				Code = sector.Code,
				SecondCode = sector.SecondCode,
				Name = sector.Name,

				CreatedAt = sector.CreatedAt,
				UpdatedAt = sector.UpdatedAt
			};
			return sectorViewModel;
		}

		public static Sector ConvertToSector(this SectorViewModel sectorViewModel)
		{
			Sector sector = new Sector()
			{
				Id = sectorViewModel.Id,
				Identifier = sectorViewModel.Identifier,

				Code = sectorViewModel.Code,
				SecondCode = sectorViewModel.SecondCode ?? "",
				Name = sectorViewModel.Name,

                CountryId = sectorViewModel.Country?.Id ?? null,

                CreatedById = sectorViewModel.CreatedBy?.Id ?? null,
				CompanyId = sectorViewModel.Company?.Id ?? null,

				CreatedAt = sectorViewModel.CreatedAt,
				UpdatedAt = sectorViewModel.UpdatedAt

			};
			return sector;
		}
	}
}
