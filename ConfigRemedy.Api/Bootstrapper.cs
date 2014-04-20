using Nancy;
using Nancy.TinyIoc;

namespace ConfigRemedy.Api
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register(RavenFactory.Create());
        }
    }
}