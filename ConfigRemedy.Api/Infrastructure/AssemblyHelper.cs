using System.Diagnostics;
using System.Reflection;

namespace ConfigRemedy.Api.Infrastructure
{
    public class AssemblyHelper
    {
        public static string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return  fileVersionInfo.FileVersion;
        }
        
        public static string GetPath()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.Location;
        }
    }
}