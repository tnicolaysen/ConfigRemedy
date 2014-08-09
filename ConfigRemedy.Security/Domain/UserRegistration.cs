using ConfigRemedy.Core.Properties;

namespace ConfigRemedy.Security.Domain
{
    [UsedImplicitly, MeansImplicitUse]
    public class UserRegistration
    {
        [NotNull] public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}