using Nancy;
using NLog;

namespace ConfigRemedy.Api.Modules
{
    public class BaseModule : NancyModule
    {
        // ReSharper disable MemberCanBeProtected.Global

        public BaseModule() : base("/api") {}

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // ReSharper restore MemberCanBeProtected.Global
    }
}