namespace UnitTests.Application
{
    internal class UserServiceTests
    {
        [Test]
        public void CreateUser_WithValidInformation_ReturnsCreatedUser()
        {
            Assert.Fail();
        }

        [Test]
        public void CreateUser_WithAlreadyUsedEmail_ThrowsEmailAlreadyUsedException()
        {
            Assert.Fail();
        }

        [Test]
        public void LoginUser_WithValidCredentials_ReturnsJWT()
        {
            Assert.Fail();
        }

        [Test]
        public void LoginUser_WithInvalidCredentials_ThrowsInvalidCredentialsException()
        {
            Assert.Fail();
        }
    }
}
