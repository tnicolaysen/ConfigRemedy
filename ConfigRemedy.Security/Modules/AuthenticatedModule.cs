using ConfigRemedy.Core.Modules;
using ConfigRemedy.Security.Annotations;
using Nancy.Security;

namespace ConfigRemedy.Security.Modules
{
    /// <summary>
    /// Modules inheriting from this class will require authentication.
    /// </summary>
    public class AuthenticatedModule : BaseModule
    {
        [UsedImplicitly]
        public AuthenticatedModule()
        {
            this.RequiresAuthentication();
        }
    }
}