using DomainCore.Common.Sectors;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Sectors;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Sectors
{
    public class SectorRepository : ISectorRepository
	{
		private ApplicationDbContext context;

		public SectorRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		public List<Sector> GetSectors(int companyId)
		{
			List<Sector> sectors = context.Sectors
                .Include(x => x.Country)
                .Include(x => x.Company)
				.Include(x => x.CreatedBy)
				.Where(x => x.Active == true && x.CompanyId == companyId)
				.OrderByDescending(x => x.CreatedAt)
				.AsNoTracking()
				.ToList();

			return sectors;
		}

		public List<Sector> GetSectorsNewerThen(int companyId, DateTime lastUpdateTime)
		{
			List<Sector> sectors = context.Sectors
                .Include(x => x.Country)
                .Include(x => x.Company)
				.Include(x => x.CreatedBy)
				.Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
				.OrderByDescending(x => x.UpdatedAt)
				.AsNoTracking()
				.ToList();

			return sectors;
		}

		private string GetNewCodeValue(int companyId)
		{
			int count = context.Sectors
				.Union(context.ChangeTracker.Entries()
					.Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Sector))
					.Select(x => x.Entity as Sector))
				.Where(x => x.CompanyId == companyId).Count();
			if (count == 0)
				return "SEK-00001";
			else
			{
				string activeCode = context.Sectors
					.Union(context.ChangeTracker.Entries()
						.Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Sector))
						.Select(x => x.Entity as Sector))
					.Where(x => x.CompanyId == companyId)
					.OrderByDescending(x => x.Id).FirstOrDefault()
					.Code;
				if (!String.IsNullOrEmpty(activeCode))
				{
					int intValue = Int32.Parse(activeCode.Replace("SEK-", ""));
					return "SEK-" + (intValue + 1).ToString("00000");
				}
				else
					return "";
			}
		}

		public Sector Create(Sector sector)
		{
			if (context.Sectors.Where(x => x.Identifier != null && x.Identifier == sector.Identifier).Count() == 0)
			{
				sector.Id = 0;

				sector.Code = GetNewCodeValue(sector.CompanyId ?? 0);
				sector.Active = true;

				sector.UpdatedAt = DateTime.Now;
				sector.CreatedAt = DateTime.Now;

				context.Sectors.Add(sector);
				return sector;
			}
			else
			{
				// Load Sector that will be updated
				Sector dbEntry = context.Sectors
				.FirstOrDefault(x => x.Identifier == sector.Identifier && x.Active == true);

				if (dbEntry != null)
				{
                    dbEntry.CountryId = sector.Country?.Id ?? null;
                    dbEntry.CompanyId = sector.CompanyId ?? null;
					dbEntry.CreatedById = sector.CreatedById ?? null;

					// Set properties
					dbEntry.Code = sector.Code;
					dbEntry.SecondCode = sector.SecondCode;
					dbEntry.Name = sector.Name;

					// Set timestamp
					dbEntry.UpdatedAt = DateTime.Now;
				}

				return dbEntry;
			}
		}

		public Sector Delete(Guid identifier)
		{
			// Load Remedy that will be deleted
			Sector dbEntry = context.Sectors
				.FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

			if (dbEntry != null)
			{
				// Set activity
				dbEntry.Active = false;
				// Set timestamp
				dbEntry.UpdatedAt = DateTime.Now;
			}

			return dbEntry;
		}
	}
}
