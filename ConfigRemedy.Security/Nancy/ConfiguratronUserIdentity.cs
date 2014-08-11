using System.Collections.Generic;
using Nancy.Security;

namespace ConfigRemedy.Security.Nancy
{
    public class ConfiguratronUserIdentity : IUserIdentity
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}