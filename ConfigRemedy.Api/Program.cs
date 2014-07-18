using ConfigRemedy.Api.Annotations;
using ConfigRemedy.Api.Infrastructure;
using Topshelf;

namespace ConfigRemedy.Api
{
    [UsedImplicitly]
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                    
            {
                x.Service<OwinSelfHost>(s =>                       
                {
                    s.ConstructUsing(name => new OwinSelfHost());  
                    s.WhenStarted(tc => tc.Start());                
                    s.WhenStopped(tc => tc.Stop());                 
                });

                x.UseLog4Net();
                x.RunAsLocalSystem();
                x.SetDescription("Configuratron: Server and web portal");           
                x.SetDisplayName("Configuratron");
                x.SetServiceName("Configuratron");                          
            });                 
        }
    }
}
