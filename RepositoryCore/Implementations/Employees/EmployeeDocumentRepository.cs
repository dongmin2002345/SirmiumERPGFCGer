using DomainCore.Employees;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Employees
{
    public class EmployeeDocumentRepository : IEmployeeDocumentRepository
    {
        ApplicationDbContext context;

        public EmployeeDocumentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<EmployeeDocument> GetEmployeeDocuments(int companyId)
        {
            List<EmployeeDocument> EmployeeDocuments = context.EmployeeDocuments
                .Include(x => x.Employee)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return EmployeeDocuments;
        }

        public List<EmployeeDocument> GetEmployeeDocumentsByEmployee(int EmployeeId)
        {
            List<EmployeeDocument> Employees = context.EmployeeDocuments
                .Include(x => x.Employee)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public List<EmployeeDocument> GetEmployeeDocumentsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeDocument> Employees = context.EmployeeDocuments
                .Include(x => x.Employee)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public EmployeeDocument Create(EmployeeDocument EmployeeDocument)
        {
            if (context.EmployeeDocuments.Where(x => x.Identifier != null && x.Identifier == EmployeeDocument.Identifier).Count() == 0)
            {
                EmployeeDocument.Id = 0;

                EmployeeDocument.Active = true;

                context.EmployeeDocuments.Add(EmployeeDocument);
                return EmployeeDocument;
            }
            else
            {
                // Load item that will be updated
                EmployeeDocument dbEntry = context.EmployeeDocuments
                    .FirstOrDefault(x => x.Identifier == EmployeeDocument.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = EmployeeDocument.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeDocument.CreatedById ?? null;

                    // Set properties
                    dbEntry.Name = EmployeeDocument.Name;
                    dbEntry.CreateDate = EmployeeDocument.CreateDate;
                    dbEntry.Path = EmployeeDocument.Path;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeDocument Delete(Guid identifier)
        {
            EmployeeDocument dbEntry = context.EmployeeDocuments
                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

            if (dbEntry != null)
            {
                dbEntry.Active = false;
                dbEntry.UpdatedAt = DateTime.Now;
            }
            return dbEntry;
        }
    }
}
