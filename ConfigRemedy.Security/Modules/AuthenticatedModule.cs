using ConfigRemedy.Core.Modules;
using Nancy.Security;

namespace ConfigRemedy.Security.Modules
{
    public class AuthenticatedModule : BaseModule
    {
        public AuthenticatedModule()
        {
            this.RequiresAuthentication();
        }
    }
}