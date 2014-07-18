using System;
using ConfigRemedy.Api.Annotations;
using ConfigRemedy.Api.Infrastructure;

namespace ConfigRemedy.Api.Modules
{
    [UsedImplicitly]
    public class DiagnosticsModule : BaseModule
    {
        public DiagnosticsModule()
        {
            Get["diagnostics/"] = _ =>
            {
                return new
                {
                    Version = AssemblyHelper.GetVersion(),
                    Platform = Environment.OSVersion.VersionString,
                    Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                    PathToExecutable = AssemblyHelper.GetPath(),

                    Links = new
                    {
                        ServerLogs = "/diagnostics/serverlogs",
                        ClientLogs = "/diagnostics/clientlogs",
                    }
                };
            };
        }
    }
}