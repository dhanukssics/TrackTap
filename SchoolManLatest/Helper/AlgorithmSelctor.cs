using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Helper
{
    public class AlgorithmSelctor
    {
        public static Crypto.Algorithm GetConfigAlgorithm(string key)
        {
            return GetConfigAlgorithm(key, "");
        }

        public static string GetAppConfig(string Key, string defaultValue)
        {
            string appConfigValue = "";
            string AppValue = System.Configuration.ConfigurationManager.AppSettings[Key];
            if (string.IsNullOrEmpty(AppValue))
            {
                if (!string.IsNullOrEmpty(defaultValue))
                    appConfigValue = defaultValue;
                else
                    appConfigValue = "";
            }
            else
            {
                appConfigValue = AppValue;
            }
            return appConfigValue;
        }

        public static Crypto.Algorithm GetConfigAlgorithm(string key, string defaultValue)
        {
            string ConfigValue = GetAppConfig(key, defaultValue);
            Crypto.Algorithm algorithm = new Crypto.Algorithm();
            if (!string.IsNullOrEmpty(ConfigValue))
            {
                switch (ConfigValue.ToLower())
                {
                    case "sha1":
                        algorithm = Crypto.Algorithm.SHA1;
                        break;
                    case "sha256":
                        algorithm = Crypto.Algorithm.SHA256;
                        break;
                    case "sha384":
                        algorithm = Crypto.Algorithm.SHA384;
                        break;
                    case "sha512":
                        algorithm = Crypto.Algorithm.SHA512;
                        break;
                    case "md5":
                        algorithm = Crypto.Algorithm.MD5;
                        break;
                    default:
                        throw new ArgumentException("Invalid algorithm configured in configuration", "Algorithm");
                }
            }
            else
                throw new ArgumentException("Invalid algorithm configured in configuration", "Algorithm");
            return algorithm;
        }
    }
}