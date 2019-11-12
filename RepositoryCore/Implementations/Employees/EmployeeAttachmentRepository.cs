using Configurator;
using DomainCore.Employees;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Employees
{
    public class EmployeeAttachmentRepository : IEmployeeAttachmentRepository
    {
        private ApplicationDbContext context;
        private readonly string connectionString;

        public EmployeeAttachmentRepository(ApplicationDbContext ctx)
        {
            context = ctx;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }
        public EmployeeAttachment Create(EmployeeAttachment attachment)
        {
            if(context.EmployeeAttachments.Where(x => x.Identifier == attachment.Identifier).Count() == 0)
            {
                attachment.CreatedAt = DateTime.Now;
                attachment.UpdatedAt = DateTime.Now;
                attachment.Active = true;

                context.EmployeeAttachments.Add(attachment);
                return attachment;
            } else
            {
                var attachmentFromDb = context.EmployeeAttachments.FirstOrDefault(x => x.Identifier == attachment.Identifier);
                if(attachmentFromDb != null)
                {
                    attachmentFromDb.EmployeeId = attachment?.EmployeeId ?? null;
                    attachmentFromDb.OK = attachment.OK;
                    attachmentFromDb.UpdatedAt = DateTime.Now;
                }
                return attachmentFromDb;
            }
        }

        public EmployeeAttachment Delete(Guid identifier)
        {
            var attachmentFromDb = context.EmployeeAttachments.FirstOrDefault(x => x.Identifier == identifier);
            if (attachmentFromDb != null)
            {
                attachmentFromDb.Active = false;
                attachmentFromDb.UpdatedAt = DateTime.Now;
            }
            return attachmentFromDb;
        }

        public List<EmployeeAttachment> GetEmployeeAttachments(int companyId)
        {
            throw new NotImplementedException();
        }

        public List<EmployeeAttachment> GetEmployeeAttachmentsByEmployee(int EmployeeId)
        {
            throw new NotImplementedException();
        }

        public List<EmployeeAttachment> GetEmployeeAttachmentsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            throw new NotImplementedException();
        }
    }
}
