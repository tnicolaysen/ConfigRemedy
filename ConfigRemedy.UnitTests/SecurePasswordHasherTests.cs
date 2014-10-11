using ConfigRemedy.Security;
using NUnit.Framework;

namespace ConfigRemedy.UnitTests
{
    [TestFixture]
    public class SecurePasswordHasherTests
    {
        private const string TestPassword = "this_is_test_password";

        private IPasswordHasher _provider;

        [SetUp]
        public void SetUp()
        {
            _provider = new SecurePasswordHasher();
        }

        [Test]
        public void Two_hashes_for_the_same_password_should_not_be_equal()
        {
            var hashedPassword1 = _provider.CreateHash(TestPassword);
            var hashedPassword2 = _provider.CreateHash(TestPassword);

            Assert.That(hashedPassword1, Is.Not.EqualTo(hashedPassword2));
        }

        [Test]
        public void Should_be_able_to_validate_hashed_passowrd()
        {
            var hashedPassword = _provider.CreateHash(TestPassword);


            Assert.That(_provider.ValidatePassword(TestPassword, hashedPassword), Is.True,
                        "Password could not be validated.");
        }
    }
}