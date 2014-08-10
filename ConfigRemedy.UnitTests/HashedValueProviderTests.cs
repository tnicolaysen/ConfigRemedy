using ConfigRemedy.Security;
using NUnit.Framework;

namespace ConfigRemedy.UnitTests
{
    [TestFixture]
    public class HashedValueProviderTests
    {
        private readonly HashedValueProvider _provider = new HashedValueProvider();

        [Test]
        public void two_instances_of_provider_generate_hashed_values_for_the_same_password_should_be_equal()
        {
            var provider1 = new HashedValueProvider();
            var provider2 = new HashedValueProvider();
            var hashedPassword1 = provider1.GetHash("this_is_test_password");
            var hashedPassword2 = provider2.GetHash("this_is_test_password");

            Assert.That(hashedPassword1, Is.EqualTo(hashedPassword2));
        }  
        
        [Test]
        public void hashed_value_for_the_same_password_should_be_equal()
        {

            var hashedPassword1 = _provider.GetHash("this_is_test_password");
            var hashedPassword2 = _provider.GetHash("this_is_test_password");

            Assert.That(hashedPassword1, Is.EqualTo(hashedPassword2));
        } 
        
        [Test]
        public void hashed_value_for_different_passwords_should_be_different()
        {

            var hashedPassword1 = _provider.GetHash("this_is_test_password");
            var hashedPassword2 = _provider.GetHash("this_is_different_password");

            Assert.That(hashedPassword1, Is.Not.EqualTo(hashedPassword2));
        }
    }
}