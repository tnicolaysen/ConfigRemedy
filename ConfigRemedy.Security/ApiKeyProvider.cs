using System;
using ConfigRemedy.Security.Annotations;
using Nancy.Cryptography;

namespace ConfigRemedy.Security
{
    [UsedImplicitly]
    public class ApiKeyProvider
    {
        private readonly IKeyGenerator _keyGenerator;

        public ApiKeyProvider(IKeyGenerator keyGenerator)
        {
            _keyGenerator = keyGenerator;
        }

        public virtual string GenerateApiKey()
        {
            var generateApiKey = Convert.ToBase64String(_keyGenerator.GetBytes(10));
            generateApiKey = generateApiKey.Replace('+', '-');
            return generateApiKey.Replace('/', '_');
        }
    }
}