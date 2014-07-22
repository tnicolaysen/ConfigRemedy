using ConfigRemedy.Api.Annotations;
using ConfigRemedy.Api.Infrastructure.OWIN;
using Topshelf;
using Serilog.Extras.Topshelf;

namespace ConfigRemedy.Api
{
    [UsedImplicitly]
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            Bootstrapper.ConfigureLogging();

            HostFactory.Run(x =>                                    
            {
                x.Service<OwinRunner>(s =>                       
                {
                    s.ConstructUsing(name => new OwinRunner());  
                    s.WhenStarted(tc => tc.Start());                
                    s.WhenStopped(tc => tc.Stop());                 
                });

                x.UseSerilog();
                x.RunAsLocalSystem();
                x.SetDescription("Configuratron: Server and web portal");           
                x.SetDisplayName("Configuratron");
                x.SetServiceName("Configuratron");                          
            });                 
        }
    }
}
