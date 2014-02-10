using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using JasonSoft;

namespace JasonSoft.IO
{
    public static class IOExtension
    {
        /// <summary>
        /// Read a text file and obtain it's contents.
        /// </summary>
        /// <param name="absolutePath">The complete file path to write to.</param>
        /// <returns>String containing the content of the file.</returns>
        public static string GetText(this FileInfo source)
        {
            if(source.Exists == false) throw new FileNotFoundException();
            using (StreamReader sr = new StreamReader(source.FullName))
            {
                return sr.ReadToEnd();
            }
        }



        public static Boolean IsWriteable(this DirectoryInfo source)
        {
            if (source.Exists == false) throw new ArgumentException(String.Empty, "source");

            FileInfo tempFile = new FileInfo(Path.Combine(source.FullName, Guid.NewGuid().ToString("N") + ".txt"));
            if (tempFile.Exists) tempFile.Delete();
            try
            {
                tempFile.Create().Close();
                tempFile.Delete();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public static Boolean IsSubDirectoryOf(this DirectoryInfo source, DirectoryInfo destination)
        {
            if(source.FullName.Length <= destination.FullName.Length) return false;

            if (destination.FullName == source.FullName.Substring(0, destination.FullName.Length))
                return true;
            else
                return false;
        }


        public static void CopyTo(this DirectoryInfo source, DirectoryInfo destination)
        {
            if (source.Exists == false) throw new ArgumentException(String.Empty, "source");
            if (destination.IsSubDirectoryOf(source)) throw new ArgumentException("destination folder can't be subfolder of the source folder");
            if (source.FullName == destination.FullName) return;
            if(destination.Exists == false) destination.Create();

            foreach (FileInfo fileInfo in source.GetFiles())
            {
                fileInfo.CopyTo(Path.Combine(destination.FullName, fileInfo.Name), true);
            }

            foreach (DirectoryInfo sourceSubDir in source.GetDirectories())
            {
                DirectoryInfo targetSubDir = new DirectoryInfo(Path.Combine(destination.FullName, sourceSubDir.Name));
                if (!targetSubDir.Exists) targetSubDir.Create();
                sourceSubDir.CopyTo(targetSubDir);
            }
        }

        public static void CopyTo(this FileInfo source, DirectoryInfo destination)
        {
            source.CopyTo(destination, true);
        }

        public static void CopyTo(this FileInfo source, DirectoryInfo destination, Boolean overwrite)
        {
            if(source == null) throw new ArgumentNullException("source");
            if(destination.Exists == false) destination.Create();

            source.CopyTo(Path.Combine(destination.FullName, source.Name), overwrite);
        }

        public static Int64 DirSize(this DirectoryInfo source)
        {
            Int32 fileCount = 0;
            return source.DirSize(ref fileCount);
        }

        /// <summary>
        /// Return in byte unit
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Int64 DirSize(this DirectoryInfo source, ref Int32 fileCount)
        {
            if(source.Exists == false) throw new ArgumentException("Dir isn't exists", "source");

            Int64 size = 0;

            // Add file sizes.
            FileInfo[] fis = source.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
                fileCount++;
            }

            // Add subdirectory sizes.
            DirectoryInfo[] dis = source.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += di.DirSize(ref fileCount);
            }

            return size; 
        }


        public static String GetNameWithoutExtension(this FileInfo source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return Path.GetFileNameWithoutExtension(source.FullName);
            
            //string ext = source.Extension;
            //if (String.IsNullOrEmpty(ext)) return source.Name;

            //return source.Name.Substring(0, source.Name.Length - source.Name.LastIndexOf(ext));
        }

        public static DirectoryInfo GetSubDirectory(this DirectoryInfo source, String subDirName)
        {
            return new DirectoryInfo(Path.Combine(source.FullName, subDirName));
        }

        public static void WriteToFile(this String source, String location)
        {
            source.WriteToFile(location, false);
        }

        public static void WriteToFile(this String source, String location, Boolean overwrite)
        {
            if (source.IsNullOrEmpty()) throw new ArgumentNullException("source");
            
            //ensure file path exist
            DirectoryInfo path = new FileInfo(location).Directory;
            if (!path.Exists) path.Create();

            StreamWriter sw = null;

            if (overwrite)
                sw = new StreamWriter(location, false, Encoding.UTF8, 512);
            else
                sw = new StreamWriter(location, true, Encoding.UTF8, 512);

            sw.Write(source);
            sw.Flush();
            sw.Close();
        }

        public static void WriteToFile(this Stream source, String path, Boolean overwrite)
        {
            const int length = 512;
            Byte[] buffer = new Byte[length];

            //Directory.CreateDirectory(Path.GetDirectoryName(path));
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
            if(directoryInfo.Exists == false) directoryInfo.Create();
            
            FileStream fileStream = null;

            if(overwrite)
                fileStream = new FileStream(path, FileMode.Create);
            else
                fileStream = new FileStream(path, FileMode.CreateNew);

            source.Position = 0;
            int bytesRead = source.Read(buffer, 0, length);
            // write the required bytes
            while (bytesRead > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
                bytesRead = source.Read(buffer, 0, length);
            }

            source.Close();
            fileStream.Close();
        }

        /// <summary>
        /// Subfolders and Files only:
        ///InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit
        ///PropagationFlags.InheritOnly
        ///
        ///This Folder, Subfolders and Files:
        ///InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit
        ///PropagationFlags.None
        ///
        ///This Folder, Subfolders and Files:
        ///InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
        ///PropagationFlags.NoPropagateInherit
        ///
        ///This folder and subfolders:
        ///InheritanceFlags.ContainerInherit,
        ///PropagationFlags.None
        ///
        ///Subfolders only:
        ///InheritanceFlags.ContainerInherit,
        ///PropagationFlags.InheritOnly
        ///
        ///This folder and files:
        ///InheritanceFlags.ObjectInherit,
        ///PropagationFlags.None
        ///
        ///This folder and files:
        ///InheritanceFlags.ObjectInherit,
        ///PropagationFlags.NoPropagateInherit

        /// </summary>
        /// <param name="source"></param>
        /// <param name="Account">Domain//NETWORK SERVICE or NETWORK SERVICE</param>
        /// <param name="Rights"></param>
        /// <param name="Inheritance"></param>
        /// <param name="Propogation"></param>
        /// <param name="ControlType"></param>
        public static void SetNtfsPermission(this DirectoryInfo source, String Account, FileSystemRights Rights,
                                        InheritanceFlags Inheritance, PropagationFlags Propogation,
                                        AccessControlType ControlType)
        {
            if(source.Exists == false) throw new ArgumentException("source directory is not exist", "source");


            // Get a DirectorySecurity object that represents the  
            // current security settings. 
            DirectorySecurity dSecurity = source.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings.  
            dSecurity.AddAccessRule(new FileSystemAccessRule(Account,
                                                             Rights,
                                                             Inheritance,
                                                             Propogation,
                                                             ControlType));
            // Set the new access settings. 
            source.SetAccessControl(dSecurity);
        }

        public static FileInfo Rename(this FileInfo source, String newName)
        {
            return source.Rename(newName, false);
        }

        public static FileInfo Rename(this FileInfo source, String newName, Boolean overwrite)
        {
            if(source == null) throw new ArgumentNullException("source");
            if(source.Exists == false) throw new ArgumentException();

            String path = Path.GetDirectoryName(source.FullName);
            FileInfo result = source.CopyTo(Path.Combine(path, newName), overwrite);
            source.Delete();
            return result;
        }

        public static void CopyStream(this Stream source, Stream output)
        {
            int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            while (true)
            {
                int read = source.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    return;
                }
                output.Write(buffer, 0, read);
            }
        }
    }
}
