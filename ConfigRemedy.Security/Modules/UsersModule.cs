using System.Linq;
using ConfigRemedy.Core;
using ConfigRemedy.Domain;
using ConfigRemedy.Repository;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace ConfigRemedy.Security.Modules
{
    public class UsersModule : AuthenticatedModule
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenizer _tokenizer;
        private readonly UserRegistrationService _registrationService;
        private readonly IPasswordHasher _passwordHasher;

        public UsersModule(ITokenizer tokenizer, UserRegistrationService registrationService,
                           IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _tokenizer = tokenizer;
            _registrationService = registrationService;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;

            Get[Constants.ApiResourceUsers] = _ => GetAllUsers();
            Get[Constants.ApiResourceUsers + "/{username}"] = _ => GetUser(_.username);
            Put[Constants.ApiResourceUsers + "/{username}"] = _ => UpdateUser(_.username); // TODO: Add tests
            Post[Constants.ApiResourceUsers] = _ => CreateUser();
        }

        private dynamic UpdateUser(string username)
        {
            Guard.NotNullOrEmpty(() => username, username);

            var updatedProfile = this.Bind<UpdateProfileRequest>();

            var storedUser = _userRepository.GetUserByUsername(username);
            if (storedUser == null)
            {
                return new TextResponse(HttpStatusCode.NotFound, "User not found. Cannot update.")
                {
                    ReasonPhrase = "User not found. Cannot update."
                };
            }

            storedUser.DisplayName = updatedProfile.DisplayName;
            storedUser.Email = updatedProfile.Email;
            
            if (!string.IsNullOrWhiteSpace(updatedProfile.Password))
                storedUser.HashedPassword = _passwordHasher.CreateHash(updatedProfile.Password);

            _userRepository.Store(storedUser);

            var userIdentity = storedUser.ToConfiguratronUserIdentity();
            var token = _tokenizer.Tokenize(userIdentity, Context);

            return new
            {
                UserName = userIdentity.UserName,
                DisplayName = userIdentity.DisplayName,
                Role = userIdentity.Role,
                Token = token,
            };
        }

        private dynamic GetUser(string userName)
        {
            Guard.NotNullOrEmpty(() => userName, userName);

            var user = _userRepository.GetUserByUsername(userName);

            if (user == null)
                return HttpStatusCode.NotFound;

            return user;
        }

        private dynamic CreateUser()
        {
            var userRegistration = this.Bind<UserRegistration>();
            if (_userRepository.GetUserByUsername(userRegistration.Username) != null)
            {
                return new TextResponse(HttpStatusCode.Forbidden, "Duplicates are not allowed")
                {
                    ReasonPhrase = "Duplicates are not allowed"
                };
            }

            var user = _registrationService.CreateUser(userRegistration);
            _userRepository.Store(user);

            return Negotiate
                .WithModel(user)
                .WithHeader("Location", Request.Path + user.Username)
                .WithStatusCode(HttpStatusCode.Created);
        }

        private dynamic GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();

            var userProjection = users.Select(a => new
            {
                a.Id,
                a.Username,
                a.DisplayName,
                a.Email,
                Link = CreateLinkForUser(a)
            });

            return userProjection;
        }

        private string CreateLinkForUser(User user)
        {
            return string.Format("{0}/{1}", Constants.ApiResourceUsers, user.Username);
        }
    }

    public class UpdateProfileRequest
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}