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
                    .Where(x => x.ContractExpiration != null && DateTime.Now.AddDays(limitation.ConstructionSiteLimit) > x.ContractExpiration.Date && x.Active == true)
                    .ToList();
                foreach (var item in constructionSitesNearExpiration)
                    message += "<li>" + item.Name + " (ističe " + item.ContractExpiration.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Firme kojima istice ugovor sa gradilistem:</p>";
                message += "<ul>";
                var businessPartnerConstructionSiteNearExpiration = context.BusinessPartnerByConstructionSites
                    .Include(x => x.BusinessPartner)
                    .Include(x => x.ConstructionSite)
                    .Where(x => x.EndDate != null && DateTime.Now.AddDays(limitation.BusinessPartnerConstructionSiteLimit) > x.EndDate.Date && x.Active == true)
                    .ToList();
                foreach (var item in businessPartnerConstructionSiteNearExpiration)
                    message += "<li>Firma: " + item.BusinessPartner?.NameGer + ", gradilište: " + item.ConstructionSite.Name + " (ističe " + item.EndDate.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Radnici kojima istice ugovor sa gradilistem:</p>";
                message += "<ul>";
                var employeeConstructionSiteNearExpiration = context.EmployeeByConstructionSites
                    .Include(x => x.Employee)
                    .Include(x => x.ConstructionSite)
                    .Where(x => x.EndDate != null && DateTime.Now.AddDays(limitation.EmployeeConstructionSiteLimit) > x.EndDate.Date && x.Active == true)
                    .ToList();
                foreach (var item in employeeConstructionSiteNearExpiration)
                    message += "<li>Radnik: " + item.Employee?.Name + " " + item.Employee?.SurName + ", gradilište: " + item.ConstructionSite.Name + " (ističe " + item.EndDate.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Radnici kojima istice ugovor sa firmom:</p>";
                message += "<ul>";
                var employeeBusinessPartnerNearExpiration = context.EmployeeByBusinessPartners
                    .Include(x => x.Employee)
                    .Include(x => x.BusinessPartner)
                    .Where(x => x.EndDate != null && DateTime.Now.AddDays(limitation.EmployeeBusinessPartnerLimit) > x.EndDate.Date && x.Active == true)
                    .ToList();
                foreach (var item in employeeBusinessPartnerNearExpiration)
                    message += "<li>Radnik: " + item.Employee?.Name + " " + item.Employee?.SurName + ", firma: " + item.BusinessPartner?.Name + " (ističe " + item.EndDate.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Rodjendani radnika:</p>";
                message += "<ul>";
                var employeeBirthdayLimit = context.Employees
                    .Where(x => x.DateOfBirth != null && 
                        DateTime.Now.AddDays(limitation.EmployeeBirthdayLimit).DayOfYear > x.DateOfBirth.Date.DayOfYear && 
                        DateTime.Now.DayOfYear <= x.DateOfBirth.Date.DayOfYear &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeBirthdayLimit)
                    message += "<li>Radnik: " + item.Name + " " + item.SurName + " (rodjendan " + item.DateOfBirth.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum isteka pasoša radnika:</p>";
                message += "<ul>";
                var employeePassportLimit = context.Employees
                    .Where(x => x.VisaTo != null &&
                        DateTime.Now.AddDays(limitation.EmployeePassportLimit).DayOfYear > ((DateTime)x.VisaTo).DayOfYear &&
                        DateTime.Now.DayOfYear <= ((DateTime)x.VisaTo).DayOfYear &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeePassportLimit)
                    message += "<li>Radnik: " + item.Name + " " + item.SurName + " (istek pasoša " + item.VisaTo?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum odlaska u ambasadu radnika:</p>";
                message += "<ul>";
                var employeeEmbasyLimit = context.Employees
                    .Where(x => x.EmbassyDate != null &&
                        DateTime.Now.AddDays(limitation.EmployeeEmbasyLimit).DayOfYear > ((DateTime)x.EmbassyDate).DayOfYear &&
                        DateTime.Now.DayOfYear <= ((DateTime)x.EmbassyDate).DayOfYear &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeEmbasyLimit)
                    message += "<li>Radnik: " + item.Name + " " + item.SurName + " (odlazak u ambasadu " + item.EmbassyDate?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum podizanja vize radnika:</p>";
                message += "<ul>";
                var employeeVisaTekeOffLimit = context.Employees
                    .Where(x => x.VisaDate != null &&
                        DateTime.Now.AddDays(limitation.EmployeeVisaTakeOffLimit).DayOfYear > ((DateTime)x.VisaDate).DayOfYear &&
                        DateTime.Now.DayOfYear <= ((DateTime)x.VisaDate).DayOfYear &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeVisaTekeOffLimit)
                    message += "<li>Radnik: " + item.Name + " " + item.SurName + " (podizanje vize " + item.VisaDate?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";
                

                message += "<p>Datum isteka vize radnika:</p>";
                message += "<ul>";
                var employeeVisaLimit = context.Employees
                    .Where(x => x.VisaValidTo != null &&
                        DateTime.Now.AddDays(limitation.EmployeeVisaLimit).DayOfYear > ((DateTime)x.VisaValidTo).DayOfYear &&
                        DateTime.Now.DayOfYear <= ((DateTime)x.VisaValidTo).DayOfYear &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeVisaLimit)
                    message += "<li>Radnik: " + item.Name + " " + item.SurName + " (istek vize " + item.VisaValidTo?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum isteka radne dozvole radnika:</p>";
                message += "<ul>";
                var employeeWorkLicenceLimit = context.Employees
                    .Where(x => x.WorkPermitTo != null &&
                        DateTime.Now.AddDays(limitation.EmployeeWorkLicenceLimit).DayOfYear > ((DateTime)x.WorkPermitTo).DayOfYear &&
                        DateTime.Now.DayOfYear <= ((DateTime)x.WorkPermitTo).DayOfYear &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeWorkLicenceLimit)
                    message += "<li>Radnik: " + item.Name + " " + item.SurName + " (istek radne dozvole " + item.WorkPermitTo?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum isteka dozvole radnika:</p>";
                message += "<ul>";
                var employeeDriveLicenceLimit = context.EmployeeLicences
                    .Include(x => x.Licence)
                    .Include(x => x.Employee)
                    .Where(x => x.ValidTo != null &&
                        DateTime.Now.AddDays(limitation.EmployeeEmbasyFamilyLimit).DayOfYear > ((DateTime)x.ValidTo).DayOfYear &&
                        DateTime.Now.DayOfYear <= ((DateTime)x.ValidTo).DayOfYear &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeDriveLicenceLimit)
                    message += "<li>Ime: " + item.Licence?.Description + ", Radnik " + item.Employee?.Name + " " + item.Employee?.SurName + " (datum isteka " + item.ValidTo?.ToString("dd.MM.yyyy") + ")</li>";
                message += "</ul>";
                message += "<hr/>";


                message += "<p>Datum ambasade za člana porodice radnika:</p>";
                message += "<ul>";
                var employeeEmbasyFamilyLimit = context.EmployeeItems
                    .Include(x => x.Employee)
                    .Where(x => x.EmbassyDate != null &&
                        DateTime.Now.AddDays(limitation.EmployeeEmbasyFamilyLimit).DayOfYear > ((DateTime)x.EmbassyDate).DayOfYear &&
                        DateTime.Now.DayOfYear <= ((DateTime)x.EmbassyDate).DayOfYear &&
                        x.Active == true)
                    .ToList();
                foreach (var item in employeeEmbasyFamilyLimit)
                    message += "<li>Ime: " + item.Name + ", Radnik " + item.Employee?.Name + " " + item.Employee?.SurName + " (datum  ambasade za člana porodice " + item.EmbassyDate?.ToString("dd.MM.yyyy") + ")</li>";
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
