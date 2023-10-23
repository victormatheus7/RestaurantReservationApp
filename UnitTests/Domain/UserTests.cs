using Domain.Entities;
using Domain.Exceptions;

namespace UnitTests.Domain
{
    internal class UserTests
    {
        [Theory]
        [TestCase("victor.castro@tests.com", "123456")]
        [TestCase("victor.castro@tests.com.br", "123456abc")]
        [TestCase("victor@castro.com", "abcdef123456")]
        public void CreateUser_WithValidInformation_ReturnsCreatedUser(string email, string password)
        {
            // Arrange
            var user = new
            {
                Email = email,
                Password = password
            };

            // Act
            var userCreated = new User(email: user.Email, password: user.Password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(userCreated.Email, Is.EqualTo(user.Email));
                Assert.That(userCreated.Password, Is.EqualTo(user.Password));
            });
        }

        [Theory]
        [TestCase("", "123456")]
        [TestCase("@victor.com", "123456")]
        [TestCase("victor.castrotests.com.br", "123456abc")]
        [TestCase("victor@castro", "abcdef123456")]
        [TestCase("very.long@domain.com" +
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" +
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" +
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" +
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" +
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz", "123456")]
        public void CreateUser_WithInvalidEmail_ThrowsInvalidEmailException(string email, string password)
        {
            // Arrange
            var user = new
            {
                Email = email,
                Password = password
            };

            try
            {
                // Act
                var userCreated = new User(email: user.Email, password: user.Password);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is InvalidEmailException, Is.True);
                return;
            }

            // Assert
            Assert.Fail();
        }

        [Theory]
        [TestCase("victor.castro@tests.com", "12345")]
        [TestCase("victor.castro@tests.com", "123451234512345")]
        [TestCase("victor.castro@tests.com.br", "123456@abc")]
        [TestCase("victor@castro.com", "")]
        public void CreateUser_WithInvalidPassword_ThrowsInvalidPasswordException(string email, string password)
        {
            // Arrange
            var user = new
            {
                Email = email,
                Password = password
            };

            try
            {
                // Act
                var userCreated = new User(email: user.Email, password: user.Password);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is InvalidPasswordException, Is.True);
                return;
            }

            // Assert
            Assert.Fail();
        }
    }
}
