using Nancy;
using System;

namespace ConfigRemedy.Api.Modules
{
    public class BaseModule : NancyModule
    {
        // ReSharper disable MemberCanBeProtected.Global

        public BaseModule()
        {
        }

        public BaseModule(string modulePath) : base(modulePath)
        {
        }

        // ReSharper restore MemberCanBeProtected.Global

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