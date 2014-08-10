using System;
using System.Security.Cryptography;
using System.Text;

namespace ConfigRemedy.Security
{
    public interface IHashedValueProvider
    {
        string GetHash(string input);
    }

    public class HashedValueProvider : IHashedValueProvider
    {
        public string GetHash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            return Convert.ToBase64String(hash);
        }
    }
}