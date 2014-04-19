using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigRemedy.Domain
{
    public class Environment
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<Application> Applications { get; set; }

        public Environment()
        {
            Applications = new List<Application>();
        }

        public bool HasApplication(string appName)
        {
            return Applications.Any(a => a.Name == appName);
        }

        public Application GetApplication(string appName)
        {
            if (!HasApplication(appName))
                throw new Exception(string.Format("Application '{0}' was not found", appName));

            return Applications.Single(a => a.Name == appName);
        }
    }
}