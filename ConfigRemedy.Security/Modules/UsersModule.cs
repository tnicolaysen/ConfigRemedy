using System;
using System.Linq;
using ConfigRemedy.Core;
using ConfigRemedy.Security.Domain;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Raven.Client;

namespace ConfigRemedy.Security.Modules
{
    public class UsersModule : AuthenticatedModule
    {
        private readonly IDocumentSession _session;
        private readonly IUserRegistrationService _registrationService;
        public UsersModule(IDocumentSession session, IUserRegistrationService registrationService) 
        {
            _session = session;
            _registrationService = registrationService;

            Get["users"] = _ => GetAllUsers();
            Get["users/{username}"] = _ => GetUser(_.username);
            Post["users"] = _ => CreateUser();
        }

        private dynamic GetUser(string userName)
        {
            Guard.NotNullOrEmpty(() => userName, userName);

            var user = GetUser(_session, userName);

            if (user == null)
                return HttpStatusCode.NotFound;

            return user;
        }

        private dynamic CreateUser()
        {
            var userRegistration = this.Bind<UserRegistration>();
            if (GetUser(_session, userRegistration.Username) != null)
            {
                return new TextResponse(HttpStatusCode.Forbidden, "Duplicates are not allowed")
                {
                    ReasonPhrase = "Duplicates are not allowed"
                };
            }

            var user = _registrationService.CreateUser(userRegistration);
            _session.Store(user);
            _session.SaveChanges();

            return Negotiate
                .WithModel(user)
                .WithHeader("Location", Request.Path + user.Username)
                .WithStatusCode(HttpStatusCode.Created);
        }

        private static User GetUser(IDocumentSession session, string username)
        {
            return session.Load<User>("users/" + username);
        }

        private dynamic GetAllUsers()
        {
            var applications = _session.Query<User>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(5)))
                .ToList();

            var userProjection = applications.Select(a => new
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
            return string.Format("users/{0}", user.Username);
        }
    }
}