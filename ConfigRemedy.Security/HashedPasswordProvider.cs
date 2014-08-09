using System;
using Nancy.Cryptography;

namespace ConfigRemedy.Security
{
    public interface IHashedPasswordProvider
    {
        string GenerateHashedPassword(string originalPassword);
    }

    public class HashedPasswordProvider : IHashedPasswordProvider
    {
        private readonly IHmacProvider _hmacProvider;

        public HashedPasswordProvider(IHmacProvider hmacProvider)
        {
            _hmacProvider = hmacProvider;
        }

        public string GenerateHashedPassword(string originalPassword)
        {
            return Convert.ToBase64String(_hmacProvider.GenerateHmac(originalPassword));
        }
    }
}