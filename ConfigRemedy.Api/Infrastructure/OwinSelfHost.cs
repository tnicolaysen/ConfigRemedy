using System;
using Microsoft.Owin.Hosting;

namespace ConfigRemedy.Api.Infrastructure
{
    public class OwinSelfHost
    {
        private IDisposable _host;

        public void Start()
        {
            _host = WebApp.Start<Startup>(Settings.Settings.ApiUrl);
            Console.WriteLine("Configuratron.Service is now listening - " + Settings.Settings.ApiUrl + ". Press ctrl-c to stop");
        }

        public void Stop()
        {
            _host.Dispose();
            Console.WriteLine("Stopped. Good bye!");
        }
    }
}