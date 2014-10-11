using ConfigRemedy.Domain;
using ConfigRemedy.Security.Annotations;

namespace ConfigRemedy.Security
{
    [UsedImplicitly]
    public class UserRegistrationService
    {
        private readonly IPasswordHasher _passwordHasher;

        public UserRegistrationService(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public virtual User CreateUser(UserRegistration userRegistration)
        {
            var hashedPassword = _passwordHasher.CreateHash(userRegistration.Password);

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