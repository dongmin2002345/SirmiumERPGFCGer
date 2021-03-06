﻿using Configurator;
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
    public class EmployeeAttachmentRepository : IEmployeeAttachmentRepository
    {
        private ApplicationDbContext context;
        private readonly string connectionString;

        private readonly string selectPart = @"
            SELECT AttachmentId, AttachmentIdentifier, AttachmentCode, AttachmentOK, Active, UpdatedAt, 
                EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, EmployeeInternalCode,
                CreatedById, CreatedByFirstName, createdByLastName, 
                CompanyId, CompanyName 
            FROM vEmployeeAttachments 
        ";

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
            var attachmentFromDb = context.EmployeeAttachments
                .Union(context.ChangeTracker.Entries()
                   .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(EmployeeAttachment))
                   .Select(x => x.Entity as EmployeeAttachment))
                .FirstOrDefault(x => x.Identifier == identifier);
            if (attachmentFromDb != null)
            {
                attachmentFromDb.Active = false;
                attachmentFromDb.UpdatedAt = DateTime.Now;
            }
            return attachmentFromDb;
        }

        public List<EmployeeAttachment> GetEmployeeAttachments(int companyId)
        {
            List<EmployeeAttachment> attachments = new List<EmployeeAttachment>();
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
                        EmployeeAttachment attachment = Read(reader);

                        attachments.Add(attachment);
                    }
                }
            }

            return attachments;
        }

        private EmployeeAttachment Read(SqlDataReader reader)
        {
            EmployeeAttachment attachment = new EmployeeAttachment();
            if(reader["AttachmentId"] != DBNull.Value)
            {
                attachment.Id = Int32.Parse(reader["AttachmentId"].ToString());
                attachment.Identifier = Guid.Parse(reader["AttachmentIdentifier"].ToString());
                attachment.Code = reader["AttachmentCode"]?.ToString();
                attachment.OK = bool.Parse(reader["AttachmentOK"].ToString());

                attachment.Active = bool.Parse(reader["Active"].ToString());
            }

            if(reader["EmployeeId"] != DBNull.Value)
            {
                attachment.Employee = new Employee();
                attachment.EmployeeId = Int32.Parse(reader["EmployeeId"].ToString());
                attachment.Employee.Id = Int32.Parse(reader["EmployeeId"].ToString());
                attachment.Employee.Identifier = Guid.Parse(reader["EmployeeIdentifier"].ToString());
                attachment.Employee.Code = reader["EmployeeCode"]?.ToString();
                attachment.Employee.Name = reader["EmployeeName"]?.ToString();
                attachment.Employee.EmployeeCode = reader["EmployeeInternalCode"]?.ToString();
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

        public List<EmployeeAttachment> GetEmployeeAttachmentsByEmployee(int EmployeeId)
        {
            List<EmployeeAttachment> attachments = new List<EmployeeAttachment>();
            string queryString = selectPart +
                "WHERE EmployeeId = @EmployeeId AND Active = 1;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@EmployeeId", EmployeeId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EmployeeAttachment attachment = Read(reader);

                        attachments.Add(attachment);
                    }
                }
            }

            return attachments;
        }

        public List<EmployeeAttachment> GetEmployeeAttachmentsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<EmployeeAttachment> attachments = new List<EmployeeAttachment>();
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
                        EmployeeAttachment attachment = Read(reader);

                        attachments.Add(attachment);
                    }
                }
            }

            return attachments;
        }
    }
}
