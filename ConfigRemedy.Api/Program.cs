using ConfigRemedy.Api.Annotations;

namespace ConfigRemedy.Api
{
    using Nancy.Hosting.Self;
    using System;

    [UsedImplicitly]
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:2403");

            using (var host = new NancyHost(uri))
            {
                host.Start();

                Console.WriteLine("Your application is running on " + uri);
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
            }
        }
    }
}
