using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CThun.Properties
{
    internal class LibraryInfo
    {
        public static readonly AssemblyName AssemblyName = Assembly.GetExecutingAssembly().GetName();
        public static readonly Version AssemblyVersion = AssemblyName.Version;

        public static string Name = AssemblyName.Name;
        public static readonly Version Version = new Version(AssemblyVersion.Major, AssemblyVersion.Minor, AssemblyVersion.Build);
    }
}
