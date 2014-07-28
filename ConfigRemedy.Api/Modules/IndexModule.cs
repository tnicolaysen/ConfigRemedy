using ConfigRemedy.Api.Annotations;
using Nancy;

namespace ConfigRemedy.Api.Modules
{
    [UsedImplicitly]
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["app/"] = parameters =>
            {
                return View["app/index"];
            };
        }
    }
}