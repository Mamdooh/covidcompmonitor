using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace viewcovidcomp
{
    public class Config
    {
        #region Default Data
        private const string DEF_LOCAL_GATE_IP = "0.0.0.0";
        private const string DEF_USER = "USER";
        private const string DEF_PASS = "PASS";

        #endregion

        // name of the .xml file
        public static string CONFIG_FNAME = Path.Combine(Environment.GetFolderPath(
Environment.SpecialFolder.ApplicationData), "CTI_settings.xml");

        public static ConfigData GetConfigData()
        {
            if (!File.Exists(CONFIG_FNAME)) // create config file with default values
            {
                using (FileStream fs = new FileStream(CONFIG_FNAME, FileMode.Create))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ConfigData));
                    ConfigData sxml = new ConfigData();
                    xs.Serialize(fs, sxml);
                    return sxml;
                }
            }
            else // read configuration from file
            {
                using (FileStream fs = new FileStream(CONFIG_FNAME, FileMode.Open))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ConfigData));
                    ConfigData sc = (ConfigData)xs.Deserialize(fs);
                    return sc;
                }
            }
        }

        public static bool SaveConfigData(ConfigData config)
        {
            if (!File.Exists(CONFIG_FNAME)) return false; // don't do anything if file doesn't exist

            using (FileStream fs = new FileStream(CONFIG_FNAME, FileMode.Open))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ConfigData));
                xs.Serialize(fs, config);
                return true;
            }
        }

        // this class holds configuration data
        public class ConfigData
        {
            public string local_gate_ip;
            public string ftp_username;
            public string ftp_pass;
            

            public ConfigData()
            {
                local_gate_ip = DEF_LOCAL_GATE_IP;
                ftp_username = DEF_USER;
                ftp_pass = DEF_PASS;

            }
        }
    }
}

