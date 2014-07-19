using System;
using Microsoft.Owin.Hosting;
using NLog;

namespace ConfigRemedy.Api.Infrastructure.OWIN
{
    public class OwinRunner
    {
        private IDisposable _host;

        public void Start()
        {
            _host = WebApp.Start<Startup>(Settings.Settings.ApiUrl);
            Logger.Info("Configuratron.Api is now accepting requests on {0}", Settings.Settings.ApiUrl);
        }

        public void Stop()
        {
            _host.Dispose();
            Logger.Info("Configuratron.Api stopped accepting requests on {0}", Settings.Settings.ApiUrl);
        }

        static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    }
}