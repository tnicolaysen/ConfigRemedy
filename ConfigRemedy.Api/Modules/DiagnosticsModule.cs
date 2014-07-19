using System;
using ConfigRemedy.Api.Annotations;
using ConfigRemedy.Api.Infrastructure;
using ConfigRemedy.Api.Infrastructure.Settings;

namespace ConfigRemedy.Api.Modules
{
    [UsedImplicitly]
    public class DiagnosticsModule : BaseModule
    {
        public DiagnosticsModule()
        {
            Get["diagnostics/"] = _ => new
            {
                Version = AssemblyHelper.GetVersion(),
                Platform = Environment.OSVersion.VersionString, 
                Environment.Is64BitOperatingSystem,
                PathToExecutable = AssemblyHelper.GetPath(), 
                Settings.Hostname,
                Settings.Port,
                Settings.DbPath,
                Links = new
                {
                    ServerLogs = "/diagnostics/serverlogs",
                    ClientLogs = "/diagnostics/clientlogs",
                }
            };
        }
    }
}