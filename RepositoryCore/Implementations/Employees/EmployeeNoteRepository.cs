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
    public class EmployeeNoteRepository : IEmployeeNoteRepository
    {
        ApplicationDbContext context;

        public EmployeeNoteRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<EmployeeNote> GetEmployeeNotes(int companyId)
        {
            List<EmployeeNote> EmployeeNotes = context.EmployeeNotes
                .Include(x => x.Employee)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .AsNoTracking()
                .ToList();

            return EmployeeNotes;
        }

        public List<EmployeeNote> GetEmployeeNotesByEmployee(int EmployeeId)
        {
            List<EmployeeNote> Employees = context.EmployeeNotes
                .Include(x => x.Employee)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.EmployeeId == EmployeeId && x.Active == true)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public List<EmployeeNote> GetEmployeeNotesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeNote> Employees = context.EmployeeNotes
                .Include(x => x.Employee)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .AsNoTracking()
                .ToList();

            return Employees;
        }

        public EmployeeNote Create(EmployeeNote EmployeeNote)
        {
            if (context.EmployeeNotes.Where(x => x.Identifier != null && x.Identifier == EmployeeNote.Identifier).Count() == 0)
            {
                EmployeeNote.Id = 0;

                EmployeeNote.Active = true;

                context.EmployeeNotes.Add(EmployeeNote);
                return EmployeeNote;
            }
            else
            {
                // Load item that will be updated
                EmployeeNote dbEntry = context.EmployeeNotes
                    .FirstOrDefault(x => x.Identifier == EmployeeNote.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = EmployeeNote.CompanyId ?? null;
                    dbEntry.CreatedById = EmployeeNote.CreatedById ?? null;

                    // Set properties
                    dbEntry.Note = EmployeeNote.Note;
                    dbEntry.NoteDate = EmployeeNote.NoteDate;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public EmployeeNote Delete(Guid identifier)
        {
            EmployeeNote dbEntry = context.EmployeeNotes
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
