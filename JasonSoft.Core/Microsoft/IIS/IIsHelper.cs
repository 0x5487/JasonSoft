using System;
using System.Diagnostics;
using System.DirectoryServices;

namespace JasonSoft.Microsoft.IIS
{

    static internal class IIsHelper
    {

        #region "Constants"


        public const String IIsProviderPath = "IIS://LocalHost";
        public const String ServiceSchemaClassName = "IIsWebService";
        public const String SiteSchemaClassName = "IIsWebServer";
        public const String VirtualDirectorySchemaClassName = "IIsWebVirtualDir";
        public const String DirectorySchemaClassName = "IIsWebDirectory";
        public const String FileSchemaClassName = "IIsWebFile";
        public const String ApplicationPoolsSchemaClassName = "IIsApplicationPools";
        public const String ApplicationPoolSchemaClassName = "IIsApplicationPool";
        public const String LocalMachineNamespace = "/LM";

        #endregion

        #region "Functions and Methods"

        public static object GetProperty(string PropertyName, DirectoryEntry DirEntry)
        {

            try
            {

                DirEntry.RefreshCache();

                if ((DirEntry.Properties[PropertyName].Value != null))
                {

                    return DirEntry.Properties[PropertyName].Value;
                }

                else
                {

                    return null;

                }
            }

            catch (Exception ex)
            {

                GetPropertyEx(PropertyName, ex);
                return null;

            }

        }

        public static String[] GetPropertyA(string PropertyName, DirectoryEntry DirEntry)
        {

            try
            {

                DirEntry.RefreshCache();

                if ((DirEntry.Properties[PropertyName].Value != null))
                {

                    if (DirEntry.Properties[PropertyName].Value.GetType().IsArray)
                    {

                        return (String[])DirEntry.Properties[PropertyName].Value;
                    }

                    else
                    {

                        return new String[] { DirEntry.Properties[PropertyName].Value.ToString() };

                    }
                }

                else
                {

                    return new String[0];

                }
            }

            catch (Exception ex)
            {

                GetPropertyEx(PropertyName, ex);
                return new String[0];

            }

        }

        public static void GetPropertyEx(string PropertyName, Exception Ex)
        {

            throw new Exception(string.Format("An error occured while trying to get the value of property '{0}'.", PropertyName), Ex);

        }

        public static void SetProperty(string PropertyName, object PropertyValue, DirectoryEntry DirEntry)
        {

            try
            {

                if (DirEntry.Properties[PropertyName].Value == null)
                {

                    DirEntry.Properties[PropertyName].Value = PropertyValue;
                }

                else
                {

                    DirEntry.Properties[PropertyName][0] = PropertyValue;

                }

                DirEntry.CommitChanges();
            }

            catch (Exception ex)
            {

                SetPropertyEx(PropertyName, ex);

            }

        }

        public static void SetPropertyA(string PropertyName, object[] PropertyValue, DirectoryEntry DirEntry)
        {

            try
            {

                if (DirEntry.Properties[PropertyName].Value == null)
                {

                    DirEntry.Properties[PropertyName].Value = PropertyValue;
                }

                else
                {

                    DirEntry.Properties[PropertyName][0] = PropertyValue;

                }

                DirEntry.CommitChanges();
            }

            catch (Exception ex)
            {

                SetPropertyEx(PropertyName, ex);

            }

        }

        public static void SetPropertyEx(string PropertyName, Exception Ex)
        {

            throw new Exception(string.Format("An error occured while trying to set the value of property '{0}'.", PropertyName), Ex);

        }

        public static ASPNETVersions GetScriptMaps(string ApplicationPath)
        {
            ASPNETVersions functionReturnValue = default(ASPNETVersions);

            try
            {

                object SystemRoot = Environment.GetEnvironmentVariable("SystemRoot");
                ASPNETVersions ServiceVersion = ASPNETVersions.v1_1_4322;
                String AppPath = ApplicationPath.Replace(IIsProviderPath + "/", string.Empty).Replace('\\', '/') + "/";
                String ServicePath = AppPath.Split('/')[0] + "/";
                string Out = string.Empty;

                using (Process p = new Process())
                {

                    p.StartInfo.FileName = string.Format("{0}\\Microsoft.NET\\Framework\\v1.1.4322\\aspnet_regiis.exe", SystemRoot);
                    p.StartInfo.Arguments = "-lk";
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;

                    p.Start();
                    p.WaitForExit();

                    Out = p.StandardOutput.ReadToEnd();

                }

                foreach (String s in Out.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {

                    if (s.Split('\t')[0].ToLower() == ServicePath.ToLower())
                    {

                        ServiceVersion = s.Split('\t')[0].StartsWith("1.1.4322") ? ASPNETVersions.v1_1_4322 : ASPNETVersions.v2_0_50727;

                    }

                    if (s.Split('\t')[0].ToLower() == AppPath.ToLower())
                    {

                        functionReturnValue = s.Split('\t')[0].StartsWith("1.1.4322") ? ASPNETVersions.v1_1_4322 : ASPNETVersions.v2_0_50727;

                    }

                }

                if (functionReturnValue == 0) functionReturnValue = ServiceVersion;
            }

            catch (Exception ex)
            {

                GetPropertyEx("ASPNETVersion", ex);

            }
            return functionReturnValue;

        }

        public static void SetScriptMaps(string ApplicationPath, ASPNETVersions ASPNETVersion)
        {

            try
            {

                object SystemRoot = Environment.GetEnvironmentVariable("SystemRoot");
                object AppRoot = ApplicationPath.Replace(IIsProviderPath + "/", string.Empty);

                using (Process p = new Process())
                {

                    p.StartInfo.FileName = string.Format("{0}\\Microsoft.NET\\Framework\\v{1}\\aspnet_regiis.exe", SystemRoot, ASPNETVersion == ASPNETVersions.v1_1_4322 ? "1.1.4322" : "2.0.50727");
                    p.StartInfo.Arguments = string.Format("-s {0}", AppRoot);
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;

                    p.Start();
                    p.WaitForExit();

                }
            }

            catch (Exception ex)
            {

                SetPropertyEx("ASPNETVersion", ex);

            }

        }

        public static void ConstructorEx(string SchemaClassName)
        {

            throw new Exception(string.Format("The provided DirectoryEntry does not have the {0} SchemaClassName.", SchemaClassName));

        }

        #endregion

    }

}
