using Nancy;

namespace ConfigRemedy.Api.Modules
{
    public class BaseModule : NancyModule
    {
        // ReSharper disable MemberCanBeProtected.Global

        public BaseModule()
            : base("/api")
        {
        }

        // ReSharper restore MemberCanBeProtected.Global
    }
}