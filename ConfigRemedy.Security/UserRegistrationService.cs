using ConfigRemedy.Security.Domain;

namespace ConfigRemedy.Security
{
    public interface IUserRegistrationService
    {
        User CreateUser(UserRegistration userRegistration);
    }

    public class UserRegistrationService : IUserRegistrationService
    {

        private readonly IHashedPasswordProvider _hashedPasswordProvider;

        public UserRegistrationService(IHashedPasswordProvider hashedPasswordProvider)
        {
            _hashedPasswordProvider = hashedPasswordProvider;
        }

        public User CreateUser(UserRegistration userRegistration)
        {
            var hashedPassword = _hashedPasswordProvider.GenerateHashedPassword(userRegistration.Password);

            var user = new User
            {
                Username = userRegistration.Username,
                DisplayName = userRegistration.DisplayName,
                Email = userRegistration.Email,
                HashedPassword = hashedPassword
            };

            return user;
        }
    }
}