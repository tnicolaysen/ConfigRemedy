using Nancy;

namespace ConfigRemedy.Core.Modules
{
    public class BaseModule : NancyModule
    {
        // ReSharper disable MemberCanBeProtected.Global
        public BaseModule() : base(Constants.ApiUrlBase) {}
        // ReSharper restore MemberCanBeProtected.Global
    }
}