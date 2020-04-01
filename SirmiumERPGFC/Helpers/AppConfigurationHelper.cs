using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Helpers
{
    public class GfcConfiguration
    {
        public string DefaultNetworkDirectory { get; set; } = "C:\\Users\\Public\\Documents";
        public string ApiUrl { get; set; } = "";

        public string OutlookDefinedPath { get; set; } = "C:\\Program Files (x86)\\Microsoft Office\\Office14\\OUTLOOK.EXE";
    }
    public static class AppConfigurationHelper
    {
        public static GfcConfiguration Configuration { get;set; }

        static AppConfigurationHelper()
        {
            Configuration = new GfcConfiguration();

        }

        public static GfcConfiguration GetConfiguration() => Configuration;

        static void FillUnsetValues()
        {
            if (String.IsNullOrEmpty(Configuration.ApiUrl))
            {
                if (Debugger.IsAttached)
                {
                    Configuration.ApiUrl = "http://localhost:5005/api";
                }
                else
                {
                    Configuration.ApiUrl = "http://sirmiumerp.com:5005/api";
                }
            }

            if(String.IsNullOrEmpty(Configuration.DefaultNetworkDirectory))
            {
                Configuration.DefaultNetworkDirectory = "C:\\Users\\Public\\Documents";
            }

            if (String.IsNullOrEmpty(Configuration.OutlookDefinedPath))
                Configuration.OutlookDefinedPath = "C:\\Program Files (x86)\\Microsoft Office\\Office14\\OUTLOOK.EXE";
        }

        public static void ReadConfig()
        {
            var path = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

            var configPath = Path.Combine(path, "gfc_settings.bin");

            var oldConfigPath = Path.Combine(path, "gfc.bin");

            if (File.Exists(oldConfigPath))
                File.Delete(oldConfigPath);

            if(!File.Exists(configPath))
            {
                SaveConfig();
            } else
            {
                var json = File.ReadAllText(configPath);
                Configuration = JsonConvert.DeserializeObject<GfcConfiguration>(json);

                FillUnsetValues();

                SaveConfig();
            }
        }

        public static void SaveConfig()
        {
            var path = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

            var configPath = Path.Combine(path, "gfc_settings.bin");


            var json = JsonConvert.SerializeObject(Configuration, Formatting.Indented);
            File.WriteAllText(configPath, json);
        }
        
    }
}
