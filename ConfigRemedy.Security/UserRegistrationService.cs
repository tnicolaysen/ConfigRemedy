using ConfigRemedy.Domain;

namespace ConfigRemedy.Security
{
    public interface IUserRegistrationService
    {
        User CreateUser(UserRegistration userRegistration);
    }

    public class UserRegistrationService : IUserRegistrationService
    {

        private readonly IHashedValueProvider _hashedValueProvider;

        public UserRegistrationService(IHashedValueProvider hashedValueProvider)
        {
            _hashedValueProvider = hashedValueProvider;
        }

        public User CreateUser(UserRegistration userRegistration)
        {
            var hashedPassword = _hashedValueProvider.GetHash(userRegistration.Password);

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