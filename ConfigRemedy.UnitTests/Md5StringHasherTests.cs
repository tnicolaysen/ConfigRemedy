using ConfigRemedy.Security;
using NUnit.Framework;

namespace ConfigRemedy.UnitTests
{
    [TestFixture]
    public class Md5StringHasherTests
    {
        private const string TestPassword = "this_is_test_password";

        private Md5StringHasher _stringHasher;

        [SetUp]
        public void SetUp()
        {
            _stringHasher = new Md5StringHasher();
        }

        [Test]
        public void Two_instances_of_provider_generate_hashed_values_for_the_same_password_should_be_equal()
        {
            var provider1 = new Md5StringHasher();
            var provider2 = new Md5StringHasher();
            var hashedPassword1 = provider1.CreateHash(TestPassword);
            var hashedPassword2 = provider2.CreateHash(TestPassword);

            Assert.That(hashedPassword1, Is.EqualTo(hashedPassword2));
        }

        [Test]
        public void Hashed_value_for_the_same_password_should_be_equal()
        {

            var hashedPassword1 = _stringHasher.CreateHash(TestPassword);
            var hashedPassword2 = _stringHasher.CreateHash(TestPassword);

            Assert.That(hashedPassword1, Is.EqualTo(hashedPassword2));
        }

        [Test]
        public void Hashed_value_for_different_passwords_should_be_different()
        {

            var hashedPassword1 = _stringHasher.CreateHash(TestPassword);
            var hashedPassword2 = _stringHasher.CreateHash(TestPassword + "!");

            Assert.That(hashedPassword1, Is.Not.EqualTo(hashedPassword2));
        }
    }
}