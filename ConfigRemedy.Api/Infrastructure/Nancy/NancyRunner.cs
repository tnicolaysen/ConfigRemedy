using System;
using Nancy.Hosting.Self;
using Serilog;

namespace ConfigRemedy.Api.Infrastructure.Nancy
{
    public class NancyRunner
    {
        private NancyHost _host;

        public void Start()
        {
            var uri = new Uri(Settings.Settings.ApiUrl);
            _host = new NancyHost(uri);
            _host.Start();

            Log.Information("Configuratron.Api is now accepting requests on {0}", Settings.Settings.ApiUrl);
        }

        public void Stop()
        {
            _host.Dispose();
            Log.Information("Configuratron.Api stopped accepting requests on {0}", Settings.Settings.ApiUrl);
        } 
    }
}