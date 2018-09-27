using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Configurator;

namespace RepositoryCore.Context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public string ConnectionString { get; private set; }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.EnableSensitiveDataLogging(true);
            builder.UseSqlServer(new Config().GetConfiguration()["ConnectionString"] as string, b => b.UseRowNumberForPaging());
            return new ApplicationDbContext(builder.Options);
        }
    }
}
