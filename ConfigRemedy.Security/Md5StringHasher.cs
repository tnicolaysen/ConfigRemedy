using System;
using System.Security.Cryptography;
using System.Text;

namespace ConfigRemedy.Security
{
    public class Md5StringHasher : IStringHasher
    {
        public string CreateHash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            return Convert.ToBase64String(hash);
        }

        public bool AreIdentical(string input, string hash)
        {
            return CreateHash(input) == hash;
        }
    }
}