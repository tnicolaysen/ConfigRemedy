using ConfigRemedy.Domain;
using ConfigRemedy.Security.Nancy;

namespace ConfigRemedy.Security
{
    public static class Mappers
    {
        public static ConfiguratronUserIdentity ToConfiguratronUserIdentity(this User self)
        {
            return new ConfiguratronUserIdentity
            {
                UserName = self.Username,
                DisplayName = self.DisplayName,
                Role = "admin",  // todo: temp solution while I am still thinking about authrization model
                Claims = new[] { "admin", "user" }, // todo: temp solution while I am still thinking about authrization model
            };
        }
    }
}