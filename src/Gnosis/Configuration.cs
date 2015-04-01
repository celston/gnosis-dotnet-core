using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Gnosis
{
    public static class Configuration
    {
        private static Lazy<Dictionary<string, string>> environments = new Lazy<Dictionary<string, string>>(() =>
        {
            string envConfig = ConfigurationManager.AppSettings["Gnosis_Environments"];
            if (string.IsNullOrEmpty(envConfig))
            {
                return null;
            }

            Dictionary<string, string> result = new Dictionary<string, string>();
            string[] envKeys = envConfig.Split(",".ToCharArray());

            foreach (string key in envKeys)
            {
                string hostKey = string.Format("{0}_Host", key);
                if (!ConfigurationManager.AppSettings.AllKeys.Contains(hostKey))
                {
                    throw new MissingEnvironmentHostnameException(key);
                }
                string host = ConfigurationManager.AppSettings[hostKey];
                result.Add(host, key);
            }

            return result;
        });

        private static Lazy<string> currentEnvironment = new Lazy<string>(() =>
        {
            if (Environments == null)
            {
                return null;
            }
            if (HttpContext.Current == null)
            {
                return null;
            }

            string host = HttpContext.Current.Request.Url.Host;
            if (!Environments.ContainsKey(host))
            {
                throw new UnknownEnvironmentHost(host);
            }

            return Environments[host];
        });
        public static string CurrentEnvironment
        {
            get
            {
                return currentEnvironment.Value;
            }
        }

        public static Dictionary<string, string> Environments
        {
            get
            {
                return environments.Value;
            }
        }

        public static string[] StringArraySetting(string key, string[] defaultValue)
        {
            string setting = Setting(key);
            if (string.IsNullOrEmpty(setting))
            {
                return defaultValue;
            }

            return setting.Split(",".ToCharArray());
        }

        public static ConnectionStringSettings ConnectionString
        {
            get
            {
                return GetConnectionString("");
            }
        }

        public static ConnectionStringSettings GetConnectionString(string key)
        {
            ConnectionStringSettings result = null;
            if (!string.IsNullOrEmpty(CurrentEnvironment))
            {
                if (string.IsNullOrEmpty(key))
                {
                    result = ConfigurationManager.ConnectionStrings[CurrentEnvironment];
                    if (result == null)
                    {
                        throw new MissingEnvironmentConnectionStringException(CurrentEnvironment);
                    }

                    return result;
                }
                else
                {
                    string envKey = string.Format("{0}_{1}", CurrentEnvironment, key);
                    result = ConfigurationManager.ConnectionStrings[envKey];
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            result = ConfigurationManager.ConnectionStrings[key];
            if (result != null)
            {
                return result;
            }

            throw new MissingConnectionStringException(key);
        }

        public static string Setting(string key)
        {
            return Setting(key, null);
        }

        public static string Setting(string key, string defaultValue)
        {
            if (!string.IsNullOrEmpty(CurrentEnvironment))
            {
                string envKey = string.Format("{0}_{1}", CurrentEnvironment, key);
                if (ConfigurationManager.AppSettings.AllKeys.Contains(envKey))
                {
                    return ConfigurationManager.AppSettings[envKey];
                }
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return ConfigurationManager.AppSettings[key];
            }

            return defaultValue;
        }
    }
}
