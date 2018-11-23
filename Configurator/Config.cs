using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Configurator
{
    public class Config
    {
        string configFile = Directory.GetCurrentDirectory() + "\\Config.json";

        Dictionary<string, string> Settings { get; set; }

        public Config()
        {
            //{"ConnectionString":"Server=ML350G5\\sd2016;Database=SirmiumERPDB;Trusted_Connection=True;MultipleActiveResultSets=true","ApiConnectionString":"Server=ML350G5\\sd2016;Database=SirmiumERPDB;Trusted_Connection=True;MultipleActiveResultSets=true"}


            Settings = new Dictionary<string, string>();
            Settings["ConnectionString"] = "Server=(localdb)\\MSSQLLocalDB;Database=SirmiumERPGFC;Trusted_Connection=True;MultipleActiveResultSets=true";
            Settings["ApiConnectionString"] = "Server=(localdb)\\MSSQLLocalDB;Database=SirmiumERPGFC;Trusted_Connection=True;MultipleActiveResultSets=true";
            Settings["ServerUrl"] = "http://0.0.0.0:5005/";
            //Settings["ConnectionString"] = "Server=ML350G5\\sd2016;Database=SirmiumERPDB;Trusted_Connection=True;MultipleActiveResultSets=true";
            //Settings["ApiConnectionString"] = "Server=ML350G5\\sd2016;Database=SirmiumERPDB;Trusted_Connection=True;MultipleActiveResultSets=true";


            if (!File.Exists(configFile))
            {
                File.WriteAllText(configFile, JsonConvert.SerializeObject(Settings));
            }
            string content = File.ReadAllText(configFile);
            Settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
        }
        public Dictionary<string, string> GetConfiguration()
        {
            return Settings;
        }

        public void SaveConfiguration()
        {
            File.WriteAllText(configFile, JsonConvert.SerializeObject(Settings));
        }
    }
}
