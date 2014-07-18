using Owin;

namespace ConfigRemedy.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        } 
    }
}