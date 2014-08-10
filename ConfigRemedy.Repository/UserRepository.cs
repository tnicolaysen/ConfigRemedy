using System;
using System.Collections.Generic;
using System.Linq;
using ConfigRemedy.Domain;
using Raven.Client;

namespace ConfigRemedy.Repository
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
        void Store(User user);
        List<User> GetAllUsers();
        User GetUserById(string userId);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDocumentSession _session;

        public UserRepository(IDocumentSession session)
        {
            _session = session;
        }

        public User GetUserByUsername(string username)
        {
            return GetUserById("users/" + username);
        }

        public void Store(User user)
        {
            _session.Store(user);
            _session.SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            var users = _session.Query<User>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(5)))
                .ToList();

            return users;
        }

        public User GetUserById(string userId)
        {
            return _session.Load<User>(userId);
        }
    }
}