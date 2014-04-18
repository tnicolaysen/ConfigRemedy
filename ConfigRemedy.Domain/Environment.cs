using System.Collections.Generic;

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
    }
}