using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Employees;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Employees
{
    public class PhysicalPersonAttachmentRepository : IPhysicalPersonAttachmentRepository
    {
        private ApplicationDbContext context;
        private readonly string connectionString;

        private readonly string selectPart = @"
            SELECT AttachmentId, AttachmentIdentifier, AttachmentCode, AttachmentOK, Active, UpdatedAt, 
                PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName,
                CreatedById, CreatedByFirstName, createdByLastName, 
                CompanyId, CompanyName 
            FROM vPhysicalPersonAttachments 
        ";

        public PhysicalPersonAttachmentRepository(ApplicationDbContext ctx)
        {
            context = ctx;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }
        public PhysicalPersonAttachment Create(PhysicalPersonAttachment attachment)
        {
            if(context.PhysicalPersonAttachments.Where(x => x.Identifier == attachment.Identifier).Count() == 0)
            {
                attachment.CreatedAt = DateTime.Now;
                attachment.UpdatedAt = DateTime.Now;
                attachment.Active = true;

                context.PhysicalPersonAttachments.Add(attachment);
                return attachment;
            } else
            {
                var attachmentFromDb = context.PhysicalPersonAttachments.FirstOrDefault(x => x.Identifier == attachment.Identifier);
                if(attachmentFromDb != null)
                {
                    attachmentFromDb.PhysicalPersonId = attachment?.PhysicalPersonId ?? null;
                    attachmentFromDb.OK = attachment.OK;
                    attachmentFromDb.UpdatedAt = DateTime.Now;
                }
                return attachmentFromDb;
            }
        }

        public PhysicalPersonAttachment Delete(Guid identifier)
        {
            var attachmentFromDb = context.PhysicalPersonAttachments
                .Union(context.ChangeTracker.Entries()
                   .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhysicalPersonAttachment))
                   .Select(x => x.Entity as PhysicalPersonAttachment))
                .FirstOrDefault(x => x.Identifier == identifier);
            if (attachmentFromDb != null)
            {
                attachmentFromDb.Active = false;
                attachmentFromDb.UpdatedAt = DateTime.Now;
            }
            return attachmentFromDb;
        }

        public List<PhysicalPersonAttachment> GetPhysicalPersonAttachments(int companyId)
        {
            List<PhysicalPersonAttachment> attachments = new List<PhysicalPersonAttachment>();
            string queryString = selectPart +
                "WHERE CompanyId = @CompanyId AND Active = 1;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        PhysicalPersonAttachment attachment = Read(reader);

                        attachments.Add(attachment);
                    }
                }
            }

            return attachments;
        }

        private PhysicalPersonAttachment Read(SqlDataReader reader)
        {
            PhysicalPersonAttachment attachment = new PhysicalPersonAttachment();
            if(reader["AttachmentId"] != DBNull.Value)
            {
                attachment.Id = Int32.Parse(reader["AttachmentId"].ToString());
                attachment.Identifier = Guid.Parse(reader["AttachmentIdentifier"].ToString());
                attachment.Code = reader["AttachmentCode"]?.ToString();
                attachment.OK = bool.Parse(reader["AttachmentOK"].ToString());

                attachment.Active = bool.Parse(reader["Active"].ToString());
            }

            if(reader["PhysicalPersonId"] != DBNull.Value)
            {
                attachment.PhysicalPerson = new PhysicalPerson();
                attachment.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                attachment.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                attachment.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                attachment.PhysicalPerson.Code = reader["PhysicalPersonCode"]?.ToString();
                attachment.PhysicalPerson.Name = reader["PhysicalPersonName"]?.ToString();
            }


            if (reader["CreatedById"] != DBNull.Value)
            {
                attachment.CreatedBy = new User();
                attachment.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                attachment.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                attachment.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                attachment.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                attachment.Company = new Company();
                attachment.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                attachment.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                attachment.Company.Name = reader["CompanyName"]?.ToString();
            }
            return attachment;
        }

        public List<PhysicalPersonAttachment> GetPhysicalPersonAttachmentsByPhysicalPerson(int PhysicalPersonId)
        {
            List<PhysicalPersonAttachment> attachments = new List<PhysicalPersonAttachment>();
            string queryString = selectPart +
                "WHERE PhysicalPersonId = @PhysicalPersonId AND Active = 1;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@PhysicalPersonId", PhysicalPersonId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PhysicalPersonAttachment attachment = Read(reader);

                        attachments.Add(attachment);
                    }
                }
            }

            return attachments;
        }

        public List<PhysicalPersonAttachment> GetPhysicalPersonAttachmentsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<PhysicalPersonAttachment> attachments = new List<PhysicalPersonAttachment>();
            string queryString = selectPart +
                @"  WHERE CompanyId = @CompanyId 
                    AND CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));
                ";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PhysicalPersonAttachment attachment = Read(reader);

                        attachments.Add(attachment);
                    }
                }
            }

            return attachments;
        }
    }
}
