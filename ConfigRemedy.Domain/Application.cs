using System.Collections.Generic;

namespace ConfigRemedy.Domain
{
    public class Application
    {
        public string Name { get; set; }
        public List<Setting> Settings { get; set; }

        public Application()
        {
            Settings = new List<Setting>();
        }
    }
}