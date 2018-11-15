using DomainCore.Common.OutputInvoices;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Context;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Invoices
{
    public class OutputInvoiceRepository : IOutputInvoiceRepository
    {
        private ApplicationDbContext context;

        public OutputInvoiceRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<OutputInvoice> GetOutputInvoices(int companyId)
        {
            List<OutputInvoice> OutputInvoices = context.OutputInvoices
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return OutputInvoices;
        }

        public List<OutputInvoice> GetOutputInvoicesNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<OutputInvoice> OutputInvoices = context.OutputInvoices
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return OutputInvoices;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.OutputInvoices
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(OutputInvoice))
                    .Select(x => x.Entity as OutputInvoice))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "IZLAZNI-00001";
            else
            {
                string activeCode = context.OutputInvoices
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(OutputInvoice))
                        .Select(x => x.Entity as OutputInvoice))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("IZLAZNI-", ""));
                    return "IZLAZNI-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public OutputInvoice Create(OutputInvoice outputInvoice)
        {
            if (context.OutputInvoices.Where(x => x.Identifier != null && x.Identifier == outputInvoice.Identifier).Count() == 0)
            {
                outputInvoice.Id = 0;

                outputInvoice.Code = GetNewCodeValue(outputInvoice.CompanyId ?? 0);
                outputInvoice.Active = true;

                outputInvoice.UpdatedAt = DateTime.Now;
                outputInvoice.CreatedAt = DateTime.Now;

                context.OutputInvoices.Add(outputInvoice);
                return outputInvoice;
            }
            else
            {
                // Load outputInvoice that will be updated
                OutputInvoice dbEntry = context.OutputInvoices
                .FirstOrDefault(x => x.Identifier == outputInvoice.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = outputInvoice.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = outputInvoice.CompanyId ?? null;
                    dbEntry.CreatedById = outputInvoice.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = outputInvoice.Code;
                    dbEntry.Supplier = outputInvoice.Supplier;
                    dbEntry.Address = outputInvoice.Address;
                    dbEntry.InvoiceNumber = outputInvoice.InvoiceNumber;
                    dbEntry.InvoiceDate = outputInvoice.InvoiceDate;
                    dbEntry.AmountNet = outputInvoice.AmountNet;
                    dbEntry.PdvPercent = outputInvoice.PdvPercent;
                    dbEntry.Pdv = outputInvoice.Pdv;
                    dbEntry.AmountGross = outputInvoice.AmountGross;
                    dbEntry.Currency = outputInvoice.Currency;
                    dbEntry.DateOfPayment = outputInvoice.DateOfPayment;
                    dbEntry.Status = outputInvoice.Status;
                    dbEntry.StatusDate = outputInvoice.StatusDate;
                    dbEntry.Description = outputInvoice.Description;


                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public OutputInvoice Delete(Guid identifier)
        {
            // Load OutputInvoice that will be deleted
            OutputInvoice dbEntry = context.OutputInvoices
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
        //public List<OutputInvoice> GetOutputInvoicesByPages(int currentPage = 1, int itemsPerPage = 50, string filterString = "")
        //{
        //    OutputInvoiceViewModel filterObject = JsonConvert.DeserializeObject<OutputInvoiceViewModel>(filterString);

        //    List<OutputInvoice> outputInvoices = context.OutputInvoices
        //        .Include(x => x.CreatedBy)
        //        .Where(x => x.Active == true)
        //        .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_Code) || x.Code.ToString().ToLower().Contains(filterObject.SearchBy_Code.ToLower()))
        //        .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_Construction) || x.Construction.ToLower().Contains(filterObject.SearchBy_Construction.ToLower()))
        //        .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_BusinessPartner) || x.BusinessPartner.ToLower().Contains(filterObject.SearchBy_BusinessPartner.ToLower()))
        //        .Where(x => filterObject == null || filterObject.SearchBy_DateFrom == null || x.InvoiceDate.Date >= ((DateTime)filterObject.SearchBy_DateFrom).Date)
        //        .Where(x => filterObject == null || filterObject.SearchBy_DateTo == null || x.InvoiceDate.Date <= ((DateTime)filterObject.SearchBy_DateTo).Date)
        //        .OrderByDescending(x => x.UpdatedAt)
        //        .Skip((currentPage - 1) * itemsPerPage)
        //        .Take(itemsPerPage)
        //        .AsNoTracking()
        //        .ToList();

        //    return outputInvoices;
        //}

        //public int GetOutputInvoicesCount(string filterString = "")
        //{
        //    OutputInvoiceViewModel filterObject = JsonConvert.DeserializeObject<OutputInvoiceViewModel>(filterString);

        //    return context.OutputInvoices
        //        .Where(x => x.Active == true)
        //        .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_Code) || x.Code.ToString().ToLower().Contains(filterObject.SearchBy_Code.ToLower()))
        //        .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_Construction) || x.Construction.ToLower().Contains(filterObject.SearchBy_Construction.ToLower()))
        //        .Where(x => filterObject == null || String.IsNullOrEmpty(filterObject.SearchBy_BusinessPartner) || x.BusinessPartner.ToLower().Contains(filterObject.SearchBy_BusinessPartner.ToLower()))
        //        .Where(x => filterObject == null || filterObject.SearchBy_DateFrom == null || x.InvoiceDate.Date >= ((DateTime)filterObject.SearchBy_DateFrom).Date)
        //        .Where(x => filterObject == null || filterObject.SearchBy_DateTo == null || x.InvoiceDate.Date <= ((DateTime)filterObject.SearchBy_DateTo).Date)
        //        .Count();
        //}

        //public List<OutputInvoice> GetOutputInvoicesByBusinessPartner(int businessPartnerId)
        //{
        //    List<OutputInvoice> outputInvoices = context.OutputInvoices
        //        .Include(x => x.CreatedBy)
        //        .Where(x => x.Active == true)
        //        //.Where(x => x.Status != (int)InvoiceHelpers.Locked)
        //        .ToList();

        //    return outputInvoices;
        //}

        //public List<OutputInvoice> GetOutputInvoicesForPopup(string filterString)
        //{
        //    List<OutputInvoice> outputInvoices = context.OutputInvoices
        //        .Where(x => String.IsNullOrEmpty(filterString) || x.Code.ToString().Contains(filterString.ToLower()))
        //        .Where(x => x.Active == true)
        //        .OrderByDescending(x => x.CreatedAt)
        //        .Take(100)
        //        .AsNoTracking()
        //        .ToList();

        //    return outputInvoices;
        //}

        //public OutputInvoice GetOutputInvoice(int id)
        //{
        //    OutputInvoice outputInvoice = context.OutputInvoices
        //        .Include(x => x.CreatedBy)
        //        //.Include(x => x.InvoiceItems)
        //        .FirstOrDefault(x => x.Id == id && x.Active == true);

        //    //if (outputInvoice != null && outputInvoice.InvoiceItems != null)
        //    //{
        //    //    outputInvoice.InvoiceItems.RemoveAll(x => x.Active == false);
        //    //}

        //    return outputInvoice;
        //}

        //public int GetNewCodeValue()
        //{
        //    int prevIndex;
        //    try
        //    {
        //        prevIndex = context.OutputInvoices
        //            .Where(x => x.Active == true)
        //            .Max(x => x.Code);
        //    }
        //    catch (Exception ex)
        //    {
        //        prevIndex = 0;
        //    }
        //    var newId = prevIndex + 1;
        //    return (int)newId;
        //}

        //public OutputInvoice SetInvoiceStatus(int id, int status)
        //{
        //    OutputInvoice dbEntry = context.OutputInvoices
        //        .FirstOrDefault(x => x.Id == id && x.Active == true);

        //    if (dbEntry != null)
        //    {
        //        // Set status
        //        //dbEntry.Status = status;

        //        // Set timestamp
        //        dbEntry.UpdatedAt = DateTime.Now;
        //    }

        //    return dbEntry;
        //}

        //public OutputInvoice SetInvoiceLock(int id, bool locked)
        //{
        //    OutputInvoice dbEntry = context.OutputInvoices
        //        .FirstOrDefault(x => x.Id == id && x.Active == true);

        //    //if (dbEntry != null)
        //    //{
        //    //    dbEntry.IsLocked = locked;

        //    //    if (locked)
        //    //        dbEntry.LockedDate = DateTime.Now;
        //    //    else
        //    //        dbEntry.LockedDate = null;

        //    //    // Set timestamp
        //    //    dbEntry.UpdatedAt = DateTime.Now;
        //    //}

        //    return dbEntry;
        //}

        //public OutputInvoice Create(OutputInvoice outputInvoice)
        //{
        //    //// Attach business partner
        //    //int businessPartnerId = outputInvoice.BusinessPartner?.Id ?? 0;
        //    //outputInvoice.BusinessPartner = context.BusinessPartners
        //    //    .FirstOrDefault(x => x.Id == businessPartnerId && x.Active == true);

        //    // Attach company
        //    int companyId = outputInvoice.Company?.Id ?? 0;
        //    outputInvoice.Company = context.Companies
        //        .FirstOrDefault(x => x.Id == companyId && x.Active == true);

        //    // Attach user
        //    int userId = outputInvoice?.CreatedBy?.Id ?? 0;
        //    outputInvoice.CreatedBy = context.Users
        //        .FirstOrDefault(x => x.Id == userId && x.Active == true);

        //    // Set activity
        //    outputInvoice.Active = true;

        //    // Set timestamps
        //    outputInvoice.CreatedAt = DateTime.Now;
        //    outputInvoice.UpdatedAt = DateTime.Now;

        //    // Add OutputInvoice to database
        //    context.OutputInvoices.Add(outputInvoice);

        //    return outputInvoice;
        //}

        //public OutputInvoice Update(OutputInvoice outputInvoice)
        //{
        //    // Load output invoice that will be updated
        //    OutputInvoice dbEntry = context.OutputInvoices
        //        //.Include(x => x.BusinessPartner)
        //        .Include(x => x.CreatedBy)
        //        .FirstOrDefault(x => x.Id == outputInvoice.Id && x.Active == true);

        //    if (dbEntry != null)
        //    {
        //        // Attach business partner
        //        //int businessPartnerId = outputInvoice.BusinessPartner?.Id ?? 0;
        //        //dbEntry.BusinessPartner = context.BusinessPartners
        //        //    .FirstOrDefault(x => x.Id == businessPartnerId && x.Active == true);

        //        // Attach company
        //        int companyId = outputInvoice.Company?.Id ?? 0;
        //        dbEntry.Company = context.Companies
        //            .FirstOrDefault(x => x.Id == companyId && x.Active == true);

        //        // Attach user
        //        int userId = outputInvoice?.CreatedBy?.Id ?? 0;
        //        dbEntry.CreatedBy = context.Users
        //            .FirstOrDefault(x => x.Id == userId && x.Active == true);

        //        // Set properties
        //        dbEntry.Code = outputInvoice.Code;
        //        dbEntry.Construction = outputInvoice.Construction;

        //        dbEntry.InvoiceDate = outputInvoice.InvoiceDate;

        //        dbEntry.BusinessPartner = outputInvoice.BusinessPartner;
        //        dbEntry.InvoiceType = outputInvoice.InvoiceType;
        //        dbEntry.Quantity = outputInvoice.Quantity;
        //        dbEntry.TrafficDate = outputInvoice.TrafficDate;
        //        dbEntry.Price = outputInvoice.Price;

        //        dbEntry.Rebate = outputInvoice.Rebate;
        //        dbEntry.RebateValue = outputInvoice.RebateValue;

        //        dbEntry.Base = outputInvoice.Base;
        //        dbEntry.PDV = outputInvoice.PDV;
        //        dbEntry.Total = outputInvoice.Total;


        //        // Set timestamp
        //        dbEntry.UpdatedAt = DateTime.Now;
        //    }

        //    return dbEntry;
        //}

        //public OutputInvoice Delete(int id)
        //{
        //    // Load output invoice that will be deleted
        //    OutputInvoice dbEntry = context.OutputInvoices
        //        //.Include(o => o.InvoiceItems)
        //        .FirstOrDefault(x => x.Id == id && x.Active == true);

        //    if (dbEntry != null)
        //    {
        //        // Set activity
        //        dbEntry.Active = false;
        //        // Set timestamp
        //        dbEntry.UpdatedAt = DateTime.Now;

        //        //if (dbEntry.InvoiceItems != null && dbEntry.InvoiceItems.Count > 0)
        //        //{
        //        //    foreach (var item in dbEntry.InvoiceItems)
        //        //    {
        //        //        if (item != null)
        //        //        {
        //        //            item.Active = false;
        //        //        }
        //        //    }
        //        //}
        //    }

        //    return dbEntry;
        //}

        //public OutputInvoice CancelOutputInvoice(int id)
        //{
        //    OutputInvoice outputInvoice = context.OutputInvoices
        //        //.Include(x => x.BusinessPartner)
        //        .Include(x => x.CreatedBy)
        //        //.Include(x => x.InvoiceItems)
        //        .FirstOrDefault(x => x.Id == id && x.Active == true);

        //    //outputInvoice.IsLocked = true;
        //    //outputInvoice.LockedDate = DateTime.Now;

        //    OutputInvoice cancelledOutputInvoice;
        //    if (outputInvoice != null)
        //    {

        //        //int businessPartnerId = outputInvoice?.BusinessPartner?.Id ?? 0;
        //        int createdById = outputInvoice?.CreatedBy?.Id ?? 0;


        //        var createdBy = context.Users.FirstOrDefault(x => x.Id == createdById && x.Active == true);

        //        cancelledOutputInvoice = new OutputInvoice()
        //        {
        //            Code = outputInvoice.Code,
        //            Construction = outputInvoice.Construction,

        //            //BusinessPartner = context.BusinessPartners.FirstOrDefault(x => x.Id == businessPartnerId && x.Active == true),

        //            InvoiceDate = outputInvoice.InvoiceDate,
        //            BusinessPartner = outputInvoice.BusinessPartner,
        //            InvoiceType = outputInvoice.InvoiceType,

        //            Quantity = -outputInvoice.Quantity,

        //            TrafficDate = outputInvoice.TrafficDate,

        //            Price  = -outputInvoice.Price,
        //            Rebate = -outputInvoice.Rebate,
        //            RebateValue = -outputInvoice.RebateValue,
        //            Base = -outputInvoice.Base,
        //            PDV = -outputInvoice.PDV,
        //            Total = -outputInvoice.Total,

        //            //Status = outputInvoice.Status,
        //            //IsCancelItem = true,
        //            //IsLocked = outputInvoice.IsLocked,
        //            //LockedDate = outputInvoice.LockedDate,

        //            // Set activity
        //            Active = true,

        //            // Set timestamps
        //            CreatedAt = DateTime.Now,
        //            UpdatedAt = DateTime.Now,

        //            CreatedBy = createdBy
        //        };

        //        int outputInvoiceId = outputInvoice?.Id ?? 0;
        //        //List<OutputInvoiceItem> outputInvoiceItems = context.OutputInvoiceItems
        //        //    .Include(x => x.OutputInvoice)
        //        //    .Include(x => x.Product)
        //        //    .Include(x => x.Product.UnitOfMeasurement)
        //        //    .Include(x => x.Company)
        //        //    .Include(x => x.CreatedBy)
        //        //    .Include(o => o.ConnectedWarrant)
        //        //    .Where(x => x.OutputInvoice.Id == outputInvoiceId && x.Active == true)
        //        //    .ToList();

        //        //List<OutputInvoiceItem> cancelledOutputInvoiceItems = new List<OutputInvoiceItem>();

        //        //if (outputInvoiceItems != null && outputInvoiceItems.Count > 0)
        //        //{
        //        //    foreach (var item in outputInvoiceItems)
        //        //    {
        //        //        OutputInvoiceItem newItem = new OutputInvoiceItem()
        //        //        {
        //        //            Product = item.Product,

        //        //            PlannedPrice = -item.PlannedPrice,

        //        //            NumOfItems = -item.NumOfItems,
        //        //            Quantity = -item.Quantity,

        //        //            Price = -item.Price,
        //        //            Rebate = -item.Rebate,
        //        //            RebateValue = -item.RebateValue,
        //        //            Value = -item.Value,
        //        //            Base = -item.Base,
        //        //            PdvPercent = item.PdvPercent,
        //        //            Pdv = -item.Pdv,
        //        //            TotalPrice = -item.TotalPrice,
        //        //            SellingPricePerUnit = -item.SellingPricePerUnit,

        //        //            Guid = Guid.NewGuid(),


        //        //            // Set timestamps
        //        //            CreatedAt = DateTime.Now,
        //        //            UpdatedAt = DateTime.Now,

        //        //            Pdv0 = -item.Pdv0,
        //        //            Pdv8 = -item.Pdv8,
        //        //            Pdv10 = -item.Pdv10,
        //        //            Pdv20 = -item.Pdv20,

        //        //            Base0 = -item.Base0,
        //        //            Base8 = -item.Base8,
        //        //            Base10 = -item.Base10,
        //        //            Base20 = -item.Base20,

        //        //            Company = company,
        //        //            CreatedBy = createdBy
        //        //        };
        //        //        cancelledOutputInvoiceItems.Add(newItem);
        //        //    }
        //        //}


        //        //cancelledOutputInvoice.InvoiceItems = new List<OutputInvoiceItem>();
        //        //cancelledOutputInvoice.InvoiceItems.AddRange(cancelledOutputInvoiceItems);

        //        context.OutputInvoices.Add(cancelledOutputInvoice);
        //    }
        //    else
        //    {
        //        // da bismo znali da nije pronadjen izlazni racun i ne moze se odraditi storniranje
        //        cancelledOutputInvoice = new OutputInvoice() { Id = 0, Active = false };
        //    }
        //    return cancelledOutputInvoice;
        //}
    }
}

