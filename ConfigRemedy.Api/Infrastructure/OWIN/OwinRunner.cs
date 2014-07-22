using System;
using Microsoft.Owin.Hosting;
using Serilog;

namespace ConfigRemedy.Api.Infrastructure.OWIN
{
    public class OwinRunner
    {
        private IDisposable _host;

        public void Start()
        {
            _host = WebApp.Start<Startup>(Settings.Settings.ApiUrl);
            Log.Information("Configuratron.Api is now accepting requests on {0}", Settings.Settings.ApiUrl);
        }

        public void Stop()
        {
            _host.Dispose();
            Log.Information("Configuratron.Api stopped accepting requests on {0}", Settings.Settings.ApiUrl);
        }
    }
}