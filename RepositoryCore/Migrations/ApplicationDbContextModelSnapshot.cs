﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RepositoryCore.Context;

namespace RepositoryCore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DomainCore.Common.BusinessPartners.BusinessPartner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("ActivityCode");

                    b.Property<string>("Address");

                    b.Property<string>("BankAccountNumber");

                    b.Property<DateTime?>("BranchOpeningDate");

                    b.Property<string>("Code");

                    b.Property<int?>("CompanyId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<string>("Director");

                    b.Property<string>("Email");

                    b.Property<Guid>("Identifier");

                    b.Property<string>("InoAddress");

                    b.Property<string>("MatCode");

                    b.Property<string>("Mobile");

                    b.Property<string>("Name");

                    b.Property<DateTime>("OpeningDate");

                    b.Property<string>("PIB");

                    b.Property<string>("Phone");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedById");

                    b.ToTable("BusinessPartners");
                });

            modelBuilder.Entity("DomainCore.Common.Companies.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Address");

                    b.Property<string>("BankAccountName");

                    b.Property<string>("BankAccountNo");

                    b.Property<int?>("CityId");

                    b.Property<int>("Code");

                    b.Property<int?>("CountryId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email");

                    b.Property<string>("IdentificationNumber");

                    b.Property<Guid>("Identifier");

                    b.Property<string>("IndustryCode");

                    b.Property<string>("IndustryName");

                    b.Property<string>("Name");

                    b.Property<string>("PDVNumber");

                    b.Property<string>("PIBNumber");

                    b.Property<string>("PIONumber");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("WebSite");

                    b.Property<long?>("tmpIdLong");

                    b.Property<string>("tmpIdString");

                    b.Property<decimal?>("tmpPropDecimal");

                    b.Property<long?>("tmpPropLong");

                    b.Property<string>("tmpPropString");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("DomainCore.Common.Identity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int?>("CompanyId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<Guid>("Identifier");

                    b.Property<string>("LastName");

                    b.Property<string>("PasswordHash");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId")
                        .IsUnique()
                        .HasFilter("[CompanyId] IS NOT NULL");

                    b.HasIndex("CreatedById");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DomainCore.Common.Individuals.Individual", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Address");

                    b.Property<int>("Code");

                    b.Property<int?>("CompanyId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<DateTime>("EmbassyDate");

                    b.Property<bool>("Family");

                    b.Property<Guid>("Identifier");

                    b.Property<string>("Interest");

                    b.Property<string>("License");

                    b.Property<string>("Name");

                    b.Property<string>("Passport");

                    b.Property<string>("SurName");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<DateTime>("VisaFrom");

                    b.Property<DateTime>("VisaTo");

                    b.Property<DateTime>("WorkPermitFrom");

                    b.Property<DateTime>("WorkPermitTo");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Individuals");
                });

            modelBuilder.Entity("DomainCore.Common.Locations.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<int?>("CompanyId");

                    b.Property<int?>("CountryId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<Guid>("Identifier");

                    b.Property<int?>("MunicipalityId");

                    b.Property<string>("Name");

                    b.Property<int?>("RegionId");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("ZipCode");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CountryId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("MunicipalityId");

                    b.HasIndex("RegionId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("DomainCore.Common.Locations.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("AlfaCode");

                    b.Property<string>("Code");

                    b.Property<int?>("CompanyId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<Guid>("Identifier");

                    b.Property<string>("Mark");

                    b.Property<string>("Name");

                    b.Property<string>("NumericCode");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("DomainCore.Common.Locations.Municipality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<int?>("CompanyId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<Guid>("Identifier");

                    b.Property<string>("MunicipalityCode");

                    b.Property<string>("Name");

                    b.Property<int?>("RegionId");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("RegionId");

                    b.ToTable("Municipalities");
                });

            modelBuilder.Entity("DomainCore.Common.Locations.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<int?>("CompanyId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<Guid>("Identifier");

                    b.Property<string>("Name");

                    b.Property<string>("RegionCode");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("DomainCore.Common.OutputInvoices.OutputInvoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<decimal>("Base");

                    b.Property<string>("BusinessPartner");

                    b.Property<int>("Code");

                    b.Property<int?>("CompanyId");

                    b.Property<string>("Construction");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<Guid>("Identifier");

                    b.Property<DateTime>("InvoiceDate");

                    b.Property<string>("InvoiceType");

                    b.Property<decimal>("PDV");

                    b.Property<decimal>("Price");

                    b.Property<decimal>("Quantity");

                    b.Property<decimal>("Rebate");

                    b.Property<decimal>("RebateValue");

                    b.Property<decimal>("Total");

                    b.Property<DateTime>("TrafficDate");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedById");

                    b.ToTable("OutputInvoices");
                });

            modelBuilder.Entity("DomainCore.Common.Professions.Profession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<int?>("CompanyId");

                    b.Property<int?>("CountryId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<Guid>("Identifier");

                    b.Property<string>("Name");

                    b.Property<string>("SecondCode");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CountryId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Professions");
                });

            modelBuilder.Entity("DomainCore.Common.Sectors.Sector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<int?>("CompanyId");

                    b.Property<int?>("CountryId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<Guid>("Identifier");

                    b.Property<string>("Name");

                    b.Property<string>("SecondCode");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CountryId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Sectors");
                });

            modelBuilder.Entity("DomainCore.ConstructionSites.ConstructionSite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Address");

                    b.Property<int?>("CityId");

                    b.Property<string>("Code");

                    b.Property<int?>("CompanyId");

                    b.Property<DateTime>("ContractExpiration");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<Guid>("Identifier");

                    b.Property<int>("MaxWorkers");

                    b.Property<string>("Name");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedById");

                    b.ToTable("ConstructionSites");
                });

            modelBuilder.Entity("DomainCore.Common.BusinessPartners.BusinessPartner", b =>
                {
                    b.HasOne("DomainCore.Common.Companies.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DomainCore.Common.Identity.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("DomainCore.Common.Companies.Company", b =>
                {
                    b.HasOne("DomainCore.Common.Locations.City")
                        .WithMany("Companies")
                        .HasForeignKey("CityId");

                    b.HasOne("DomainCore.Common.Locations.Country")
                        .WithMany("Companies")
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("DomainCore.Common.Identity.User", b =>
                {
                    b.HasOne("DomainCore.Common.Companies.Company", "Company")
                        .WithOne("CreatedBy")
                        .HasForeignKey("DomainCore.Common.Identity.User", "CompanyId");

                    b.HasOne("DomainCore.Common.Identity.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("DomainCore.Common.Individuals.Individual", b =>
                {
                    b.HasOne("DomainCore.Common.Companies.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DomainCore.Common.Identity.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("DomainCore.Common.Locations.City", b =>
                {
                    b.HasOne("DomainCore.Common.Companies.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DomainCore.Common.Locations.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("DomainCore.Common.Identity.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("DomainCore.Common.Locations.Municipality", "Municipality")
                        .WithMany()
                        .HasForeignKey("MunicipalityId");

                    b.HasOne("DomainCore.Common.Locations.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId");
                });

            modelBuilder.Entity("DomainCore.Common.Locations.Country", b =>
                {
                    b.HasOne("DomainCore.Common.Companies.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DomainCore.Common.Identity.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("DomainCore.Common.Locations.Municipality", b =>
                {
                    b.HasOne("DomainCore.Common.Companies.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DomainCore.Common.Identity.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("DomainCore.Common.Locations.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId");
                });

            modelBuilder.Entity("DomainCore.Common.Locations.Region", b =>
                {
                    b.HasOne("DomainCore.Common.Companies.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DomainCore.Common.Identity.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("DomainCore.Common.OutputInvoices.OutputInvoice", b =>
                {
                    b.HasOne("DomainCore.Common.Companies.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DomainCore.Common.Identity.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("DomainCore.Common.Professions.Profession", b =>
                {
                    b.HasOne("DomainCore.Common.Companies.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DomainCore.Common.Locations.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("DomainCore.Common.Identity.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("DomainCore.Common.Sectors.Sector", b =>
                {
                    b.HasOne("DomainCore.Common.Companies.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DomainCore.Common.Locations.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("DomainCore.Common.Identity.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("DomainCore.ConstructionSites.ConstructionSite", b =>
                {
                    b.HasOne("DomainCore.Common.Locations.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.HasOne("DomainCore.Common.Companies.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DomainCore.Common.Identity.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });
#pragma warning restore 612, 618
        }
    }
}
