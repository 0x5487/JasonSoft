using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.SqlClient;

namespace JasonSoft.Services.JobSchelude.Data
{
    [Database(Name = "ServiceDB")]
    public partial class ServiceDataContext : DataContext
    {
        public ServiceDataContext() : base(global::System.Configuration.ConfigurationManager.ConnectionStrings["ServiceDB"].ConnectionString)
        {

        }

        public void Reset()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = "JasonSoft.EmbeddedResource.JobScheduleDatabaseScript.txt";
            StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(name));
            String sql = sr.ReadToEnd();
            ExecuteCommand(sql);
        }
    }
}
