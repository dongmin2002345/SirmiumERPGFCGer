using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Configurator;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SirmiumERPWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string addressWithPort = "http://0.0.0.0:5001/";
            try
            {
                addressWithPort = (new Config()).GetConfiguration()["ServerUrl"];
            }
            catch (Exception ex)
            {

            }

            BuildWebHost(args, addressWithPort).Run();
        }

        public static IWebHost BuildWebHost(string[] args, string inServerAddress) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls(inServerAddress)
                .Build();

    }
}
