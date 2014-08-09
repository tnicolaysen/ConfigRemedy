using ConfigRemedy.Security.Domain;
using Nancy.Cryptography;

namespace ConfigRemedy.Security
{
    public interface IUserRegistrationService
    {
        User CreateUser(UserRegistration userRegistration);
    }

    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IHmacProvider _hmacProvider;

        public UserRegistrationService(IHmacProvider hmacProvider)
        {
            _hmacProvider = hmacProvider;
        }

        public User CreateUser(UserRegistration userRegistration)
        {
            var user = new User
            {
                Username = userRegistration.Username,
                DisplayName = userRegistration.DisplayName,
                Email = userRegistration.Email,
                HashedPassword = _hmacProvider.GenerateHmac(userRegistration.Password)
            };

            return user;
        }
    }
}