using Configurator;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Context;
using RepositoryCore.Implementations.Limitations;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceCore.Implementations.Limitations;
using ServiceInterfaces.ViewModels.Limitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SirmiumERPWeb.Helpers
{
    public static class MailHelper
    {
        static string FROM = "SirmiumERP NoReply";
        static string FROM_MAIL = "testsmerp123@gmail.com";
        static string FROM_PASSWD = "Secret123$";

        static string SUBJECT_DEFAULT = "Izvestaj - na dan {0}";

        public static void SendLimitations(List<LimitationEmailViewModel> users)
        {
            try
            {
                var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
                builder.UseSqlServer(new Config().GetConfiguration()["ConnectionString"]);

                ApplicationDbContext context = new ApplicationDbContext(builder.Options);

                var limitation = context.Limitations.FirstOrDefault();



                var subject = String.Format(SUBJECT_DEFAULT, DateTime.Now.ToString("dd.MM.yyyy"));
                var message = "<html>";
                message += "<body>";

                message += "<h4>Izvestaj za " + DateTime.Now.ToString("dd.MM.yyyy") + "</h4>";
                message += "<hr/>";


                message += "<p>Gradilista kojima istice ugovor:</p>";
                message += "<ul>";
                var constructionSitesNearExpiration = context.ConstructionSites
                    .Where(x => x.ContractExpiration != null 
                        && DateTime.Now.AddDays(limitation.ConstructionSiteLimit).Date >= x.ContractExpiration.Date 
                        && x.Active == true)
                    .ToList();
                foreach (var item in constructionSitesNearExpiration)
                    message += "<li>Gradilište:  (" + item.InternalCode + ") " + item.Name + " (ističe " + item.ContractExpiration.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Firme kojima istice ugovor sa gradilistem:</p>";
                message += "<ul>";
                var businessPartnerConstructionSiteNearExpiration = context.BusinessPartnerByConstructionSites
                    .Include(x => x.BusinessPartner)
                    .Include(x => x.ConstructionSite)
                    .Where(x => x.EndDate != null 
                        && DateTime.Now.AddDays(limitation.BusinessPartnerConstructionSiteLimit).Date >= x.EndDate.Date 
                        && x.BusinessPartner.Active == true
                        && x.ConstructionSite.Active == true
                        && x.Active == true)
                    .ToList();
                foreach (var item in businessPartnerConstructionSiteNearExpiration)
                    message += "<li>Firma: (" + item.BusinessPartner.InternalCode + ") " + item.BusinessPartner?.NameGer + ", gradilište: (" + item.ConstructionSite?.InternalCode + ")  " + item.ConstructionSite.Name + " (ističe " + item.EndDate.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Radnici kojima istice ugovor sa gradilistem:</p>";
                message += "<ul>";
                var employeeConstructionSiteNearExpiration = context.EmployeeByConstructionSites
                    .Include(x => x.Employee)
                    .Include(x => x.ConstructionSite)
                    .Where(x => x.EndDate != null 
                        && DateTime.Now.AddDays(limitation.EmployeeConstructionSiteLimit).Date >= x.EndDate.Date 
                        && x.Employee.Active == true
                        && x.ConstructionSite.Active == true
                        && x.Active == true)
                    .ToList();
                foreach (var item in employeeConstructionSiteNearExpiration)
                    message += "<li>Radnik: (" + item.Employee.EmployeeCode + ") " + item.Employee?.Name + " " + item.Employee?.SurName + ", gradilište: (" + item.ConstructionSite?.InternalCode + ")  " + item.ConstructionSite.Name + " (ističe " + item.EndDate.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Radnici kojima istice ugovor sa firmom:</p>";
                message += "<ul>";
                var employeeBusinessPartnerNearExpiration = context.EmployeeByBusinessPartners
                    .Include(x => x.Employee)
                    .Include(x => x.BusinessPartner)
                    .Where(x => x.EndDate != null 
                        && DateTime.Now.AddDays(limitation.EmployeeBusinessPartnerLimit).Date >= x.EndDate.Date 
                        && x.Employee.Active == true
                        && x.BusinessPartner.Active == true
                        && x.Active == true)
                    .ToList();
                foreach (var item in employeeBusinessPartnerNearExpiration)
                    message += "<li>Radnik: (" + item.Employee.EmployeeCode + ") " + item.Employee?.Name + " " + item.Employee?.SurName + ", firma: (" + item.BusinessPartner?.InternalCode + ")  " + item.BusinessPartner?.Name + " (ističe " + item.EndDate.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Rodjendani poslovnih partnera:</p>";
                message += "<ul>";
                var employeeBirthdayLimit = context.BusinessPartnerPhones
                    .Include(x => x.BusinessPartner)
                    .Where(x => x.Birthday != null && 
                        DateTime.Now.AddDays(limitation.EmployeeBirthdayLimit).DayOfYear >= ((DateTime)x.Birthday).Date.DayOfYear && 
                        DateTime.Now.DayOfYear <= ((DateTime)x.Birthday).Date.DayOfYear &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeBirthdayLimit)
                    message += "<li>Poslovni partner: (" + item.BusinessPartner.InternalCode + " " + item.BusinessPartner.NameGer + ") " + item.ContactPersonFirstName + " " + item.ContactPersonLastName + " (rodjendan " + ((DateTime)item.Birthday).ToString("dd.MM.yyyy") + "), Napomena: " + item.Description + "</li>";
                message += "</ul>";
                message += "<hr/>";




                message += "<p>Datum isteka pasoša radnika:</p>";
                message += "<ul>";
                var employeePassportLimit = context.Employees
                    .Where(x => x.VisaTo != null &&
                        DateTime.Now.AddDays(limitation.EmployeePassportLimit).Date >= ((DateTime)x.VisaTo) &&
                        DateTime.Now.Date <= ((DateTime)x.VisaTo).Date &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeePassportLimit)
                    message += "<li>Radnik: (" + item.EmployeeCode + ") " + item.Name + " " + item.SurName + " (istek pasoša " + item.VisaTo?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum odlaska u ambasadu radnika:</p>";
                message += "<ul>";
                var employeeEmbasyLimit = context.Employees
                    .Where(x => x.EmbassyDate != null &&
                        DateTime.Now.AddDays(limitation.EmployeeEmbasyLimit).Date >= ((DateTime)x.EmbassyDate).Date &&
                        DateTime.Now.Date <= ((DateTime)x.EmbassyDate).Date &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeEmbasyLimit)
                    message += "<li>Radnik: (" + item.EmployeeCode + ") " + item.Name + " " + item.SurName + " (odlazak u ambasadu " + item.EmbassyDate?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum podizanja vize radnika:</p>";
                message += "<ul>";
                var employeeVisaTekeOffLimit = context.Employees
                    .Where(x => x.VisaDate != null &&
                        DateTime.Now.AddDays(limitation.EmployeeVisaTakeOffLimit).Date >= ((DateTime)x.VisaDate).Date &&
                        DateTime.Now.Date <= ((DateTime)x.VisaDate).Date &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeVisaTekeOffLimit)
                    message += "<li>Radnik: (" + item.EmployeeCode + ") " + item.Name + " " + item.SurName + " (podizanje vize " + item.VisaDate?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";
                

                message += "<p>Datum isteka vize radnika:</p>";
                message += "<ul>";
                var employeeVisaLimit = context.Employees
                    .Where(x => x.VisaValidTo != null &&
                        DateTime.Now.AddDays(limitation.EmployeeVisaLimit).Date >= ((DateTime)x.VisaValidTo).Date &&
                        DateTime.Now.Date <= ((DateTime)x.VisaValidTo).Date &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeVisaLimit)
                    message += "<li>Radnik: (" + item.EmployeeCode + ") " + item.Name + " " + item.SurName + " (istek vize " + item.VisaValidTo?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum isteka radne dozvole radnika:</p>";
                message += "<ul>";
                var employeeWorkLicenceLimit = context.Employees
                    .Where(x => x.WorkPermitTo != null &&
                        DateTime.Now.AddDays(limitation.EmployeeWorkLicenceLimit).Date >= ((DateTime)x.WorkPermitTo).Date &&
                        DateTime.Now.Date <= ((DateTime)x.WorkPermitTo).Date &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeWorkLicenceLimit)
                    message += "<li>Radnik: (" + item.EmployeeCode + ") " + item.Name + " " + item.SurName + " (istek radne dozvole " + item.WorkPermitTo?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum isteka dozvole radnika:</p>";
                message += "<ul>";
                var employeeDriveLicenceLimit = context.EmployeeLicences
                    .Include(x => x.Licence)
                    .Include(x => x.Employee)
                    .Where(x => x.ValidTo != null &&
                        DateTime.Now.AddDays(limitation.EmployeeEmbasyFamilyLimit).Date >= ((DateTime)x.ValidTo).Date &&
                        DateTime.Now.Date <= ((DateTime)x.ValidTo).Date &&
                        x.Employee.Active == true &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeDriveLicenceLimit)
                    message += "<li>Radnik (" + item.Employee.EmployeeCode + ") " + item.Employee?.Name + " " + item.Employee?.SurName + ", Dozvola: " + item.Licence?.Description + " (datum isteka " + item.ValidTo?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum ambasade za člana porodice radnika:</p>";
                message += "<ul>";
                var employeeEmbasyFamilyLimit = context.EmployeeItems
                    .Include(x => x.Employee)
                    .Where(x => x.EmbassyDate != null &&
                        DateTime.Now.AddDays(limitation.EmployeeEmbasyFamilyLimit).Date >= ((DateTime)x.EmbassyDate).Date &&
                        DateTime.Now.Date <= ((DateTime)x.EmbassyDate).Date &&
                        x.Employee.Active == true &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeEmbasyFamilyLimit)
                    message += "<li>Radnik (" + item.Employee.EmployeeCode + ") " + item.Employee?.Name + " " + item.Employee?.SurName + ", Ime: " + item.Name + " (datum  ambasade za člana porodice " + item.EmbassyDate?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";







                message += "<p>Datum isteka pasoša fizičkog lica:</p>";
                message += "<ul>";
                var personPassportLimit = context.PhysicalPersons
                    .Where(x => x.VisaTo != null &&
                        DateTime.Now.AddDays(limitation.PersonPassportLimit).Date >= ((DateTime)x.VisaTo).Date &&
                        DateTime.Now.Date <= ((DateTime)x.VisaTo).Date &&
                        x.Active == true)
                    .ToList();
                foreach (var item in personPassportLimit)
                    message += "<li>Fizičko lice: (" + item.PhysicalPersonCode + ") " + item.Name + " " + item.SurName + " (istek pasoša " + item.VisaTo?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum odlaska u ambasadu fizičkog licaa:</p>";
                message += "<ul>";
                var personEmbasyLimit = context.PhysicalPersons
                    .Where(x => x.EmbassyDate != null &&
                        DateTime.Now.AddDays(limitation.PersonEmbasyLimit).Date >= ((DateTime)x.EmbassyDate).Date &&
                        DateTime.Now.Date <= ((DateTime)x.EmbassyDate).Date &&
                        x.Active == true)
                    .ToList();
                foreach (var item in personEmbasyLimit)
                    message += "<li>Fizičko lice: (" + item.PhysicalPersonCode + ") " + item.Name + " " + item.SurName + " (odlazak u ambasadu " + item.EmbassyDate?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum podizanja vize Fizičkog lica:</p>";
                message += "<ul>";
                var personVisaTekeOffLimit = context.PhysicalPersons
                    .Where(x => x.VisaDate != null &&
                        DateTime.Now.AddDays(limitation.PersonVisaTakeOffLimit).Date >= ((DateTime)x.VisaDate).Date &&
                        DateTime.Now.Date <= ((DateTime)x.VisaDate).Date &&
                        x.Active == true)
                    .ToList();
                foreach (var item in personVisaTekeOffLimit)
                    message += "<li>Fizičko lice: (" + item.PhysicalPersonCode + ") " + item.Name + " " + item.SurName + " (podizanje vize " + item.VisaDate?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum isteka vize fizičkog lica:</p>";
                message += "<ul>";
                var personVisaLimit = context.PhysicalPersons
                    .Where(x => x.VisaValidTo != null &&
                        DateTime.Now.AddDays(limitation.PersonVisaLimit).Date >= ((DateTime)x.VisaValidTo).Date &&
                        DateTime.Now.Date <= ((DateTime)x.VisaValidTo).Date &&
                        x.Active == true)
                    .ToList();
                foreach (var item in personVisaLimit)
                    message += "<li>Fizičko lice: (" + item.PhysicalPersonCode + ") " + item.Name + " " + item.SurName + " (istek vize " + item.VisaValidTo?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum isteka radne dozvole fizičkog lica:</p>";
                message += "<ul>";
                var personWorkLicenceLimit = context.PhysicalPersons
                    .Where(x => x.WorkPermitTo != null &&
                        DateTime.Now.AddDays(limitation.PersonWorkLicenceLimit).Date >= ((DateTime)x.WorkPermitTo).Date &&
                        DateTime.Now.Date <= ((DateTime)x.WorkPermitTo).Date &&
                        x.Active == true)
                    .ToList();
                foreach (var item in personWorkLicenceLimit)
                    message += "<li>Fizičko lice: (" + item.PhysicalPersonCode + ") " + item.Name + " " + item.SurName + " (istek radne dozvole " + item.WorkPermitTo?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";
                



                message += "</body>";
                message += "</html>";

                SendMail(FROM_MAIL, FROM, users, subject, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static bool SendMail(string to, string name, List<LimitationEmailViewModel> cc, string subject, string message)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.googlemail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(FROM_MAIL, FROM_PASSWD);

                var mail = new MailMessage(FROM_MAIL, to, subject, message)
                {
                    IsBodyHtml = true,
                    From = new MailAddress(FROM_MAIL, FROM),
                    Subject = subject,
                    Body = message,
                    Sender = new MailAddress(FROM_MAIL, FROM),
                };

                if (cc != null && cc.Count > 0)
                {
                    foreach (var item in cc)
                    {
                        mail.CC.Add(new MailAddress(item.Email, item.Name + " " + item.LastName));
                    }
                }
                client.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
