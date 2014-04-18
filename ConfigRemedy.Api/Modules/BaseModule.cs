using System;
using Nancy;

namespace ConfigRemedy.Api.Modules
{
    public class BaseModule : NancyModule
    {
        public BaseModule()
        {
        }

        public BaseModule(string modulePath) : base(modulePath)
        {
        }

        /// <summary>
        /// Ensures that a dynamic parma is present and casts it to a string
        /// </summary>
        protected string RequiredParam(dynamic _, string paramName)
        {
            string parmaAsString = _[paramName];

            if (string.IsNullOrWhiteSpace(parmaAsString))
                throw new ArgumentNullException("paramName", "Required param was null or empty string");

            return parmaAsString;
        }
    }
}