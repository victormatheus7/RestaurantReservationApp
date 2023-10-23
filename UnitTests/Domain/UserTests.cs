using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;

namespace UnitTests.Domain
{
    internal class UserTests
    {
        [Theory]
        [TestCase("victor.castro@tests.com", "123456", Role.Admin)]
        [TestCase("victor.castro@tests.com.br", "123456abc", Role.Client)]
        [TestCase("victor@castro.com", "abcdef123456", Role.Client)]
        public void CreateUser_WithValidInformation_ReturnsCreatedUser(string email, string password, Role role)
        {
            // Arrange
            var user = new
            {
                Email = email,
                Password = password,
                Role = role
            };

            // Act
            var userCreated1 = User.CreateUser(email: user.Email, password: user.Password, role: user.Role);
            var userCreated2 = User.CreateUser(email: user.Email, password: user.Password, role: user.Role, passwordSalt: userCreated1.PasswordSalt);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(userCreated1.Email, Is.EqualTo(user.Email));
                Assert.That(userCreated1.PasswordHash, Is.EqualTo(userCreated2.PasswordHash));
                Assert.That(userCreated1.PasswordSalt, Is.EqualTo(userCreated2.PasswordSalt));
                Assert.That(userCreated1.Role, Is.EqualTo(user.Role));
            });
        }

        [Theory]
        [TestCase("", "123456", Role.Admin)]
        [TestCase("@victor.com", "123456", Role.Admin)]
        [TestCase("victor.castrotests.com.br", "123456abc", Role.Admin)]
        [TestCase("victor@castro", "abcdef123456", Role.Client)]
        [TestCase("very.long@domain.com" +
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" +
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" +
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" +
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" +
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz", "123456", Role.Client)]
        public void CreateUser_WithInvalidEmail_ThrowsInvalidEmailException(string email, string password, Role role)
        {
            // Arrange
            var user = new
            {
                Email = email,
                Password = password,
                Role = role
            };

            try
            {
                // Act
                var userCreated = User.CreateUser(email: user.Email, password: user.Password, role: user.Role);
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
        [TestCase("victor.castro@tests.com", "12345", Role.Admin)]
        [TestCase("victor.castro@tests.com", "123451234512345", Role.Admin)]
        [TestCase("victor.castro@tests.com.br", "123456@abc", Role.Client)]
        [TestCase("victor@castro.com", "", Role.Client)]
        public void CreateUser_WithInvalidPassword_ThrowsInvalidPasswordException(string email, string password, Role role)
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
                var userCreated = User.CreateUser(email: user.Email, password: user.Password, role: role);
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
