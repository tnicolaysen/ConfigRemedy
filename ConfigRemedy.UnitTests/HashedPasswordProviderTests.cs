using System;
using ConfigRemedy.Security;
using Nancy.Cryptography;
using NUnit.Framework;

namespace ConfigRemedy.UnitTests
{
    [TestFixture]
    public class HashedPasswordProviderTests
    {
        private readonly HashedPasswordProvider _provider = new HashedPasswordProvider(new DefaultHmacProvider(new RandomKeyGenerator()));

        [Test]
        public void hashed_value_for_the_same_password_should_be_equal()
        {

            var hashedPassword1 = _provider.GenerateHashedPassword("this_is_test_password");
            var hashedPassword2 = _provider.GenerateHashedPassword("this_is_test_password");

            Assert.That(hashedPassword1, Is.EqualTo(hashedPassword2));
        } 
        
        [Test]
        public void hashed_value_for_different_passwords_should_be_different()
        {

            var hashedPassword1 = _provider.GenerateHashedPassword("this_is_test_password");
            var hashedPassword2 = _provider.GenerateHashedPassword("this_is_different_password");

            Assert.That(hashedPassword1, Is.Not.EqualTo(hashedPassword2));
        }
    }
}