using System;
using Nancy.Cryptography;

namespace ConfigRemedy.Security
{
    public interface IApiKeyProvider
    {
        string GenerateApiKey();
    }

    public class ApiKeyProvider : IApiKeyProvider
    {
        private readonly IKeyGenerator _keyGenerator;

        public ApiKeyProvider(IKeyGenerator keyGenerator)
        {
            _keyGenerator = keyGenerator;
        }

        public string GenerateApiKey()
        {
            var generateApiKey = Convert.ToBase64String(_keyGenerator.GetBytes(10));
            generateApiKey = generateApiKey.Replace('+', '-');
            return generateApiKey.Replace('/', '_');
        }
    }
}