using System;
using Microsoft.Owin.Hosting;

namespace ConfigRemedy.Api.Infrastructure
{
    public class OwinSelfHost
    {
        private IDisposable _host;

        public void Start()
        {
            const string url = "http://+:2403/";

            _host = WebApp.Start<Startup>(url);
            Console.WriteLine("Nancy now listening - http://+:2403/. Press ctrl-c to stop");
        }

        public void Stop()
        {
            _host.Dispose();
            Console.WriteLine("Stopped. Good bye!");
        }
    }
}