using System.Linq;
using ConfigRemedy.Core;
using ConfigRemedy.Domain;
using ConfigRemedy.Repository;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace ConfigRemedy.Security.Modules
{
    public class UsersModule : AuthenticatedModule
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRegistrationService _registrationService;

        public UsersModule(IUserRegistrationService registrationService, IUserRepository userRepository) 
        {
            _registrationService = registrationService;
            _userRepository = userRepository;

            Get[Constants.ApiResourceUsers] = _ => GetAllUsers();
            Get[Constants.ApiResourceUsers + "/{username}"] = _ => GetUser(_.username);
            Post[Constants.ApiResourceUsers] = _ => CreateUser();
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
}