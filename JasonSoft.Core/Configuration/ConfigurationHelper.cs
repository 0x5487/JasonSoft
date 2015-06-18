using System;
using System.IO;
using System.Xml.Linq;

namespace JasonSoft.Configuration
{
    public class ConfigurationHelper
    {
        public ConfigurationHelper(FileInfo configFile)
        {
            if(!configFile.Exists) throw new ArgumentException("file is not exist", "configFile");

            _configFile = configFile;
        }

        private FileInfo _configFile;


        /// <summary>
        /// case sensitive
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        /// <param name="providerName"></param>
        public void ChangeConfigurationString(String name, String connectionString, String providerName)
        {
            
            XDocument xDocument = XDocument.Load(_configFile.FullName);

            foreach (XElement element in xDocument.Elements("configuration").Elements("connectionStrings").Elements("add"))
            {
                if (element.Attribute("name").Value == name)
                {
                    element.Attribute("connectionString").Value = connectionString;
                    element.Attribute("providerName").Value = providerName;
                }
            }

            xDocument.Save(_configFile.FullName);
        }

    }
}
