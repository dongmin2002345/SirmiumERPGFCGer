using Configurator;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SirmiumERPWeb.Tasks
{
    public class MailTask
    {
        public static void SendMailTime(string dailyTime)
        {
            while (true)
            {
                var timeParts = dailyTime.Split(new char[1] { ':' });

                var dateNow = DateTime.Now;
                var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
                           int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
                TimeSpan ts;
                if (date > dateNow)
                    ts = date - dateNow;
                else
                {
                    date = date.AddDays(1);
                    ts = date - dateNow;
                }

                //waits certan time and run the code
                Thread.Sleep(ts);
                new MailTask().SendEmail();
            }
        }

        void SendEmail()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
                builder.UseSqlServer(new Config().GetConfiguration()["ConnectionString"]);

            ApplicationDbContext context = new ApplicationDbContext(builder.Options);

            var limitation = context.Limitations.FirstOrDefault();

            Console.WriteLine("Method is called.");
        }
    }
}
