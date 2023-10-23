using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using Domain.Enum;
using Domain.Repositories;
using Moq;

namespace UnitTests.Application
{
    internal class UserServiceTests
    {
        [Test]
        public void CreateUser_WithValidInformation_ReturnsCreatedUser()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var password = "123456";
            var role = Role.Admin;

            var userRepositoryMock = new Mock<IUserRepository>();
            int callOrder = 0;
            userRepositoryMock.Setup(ur => ur.Get(email)).Callback(() => Assert.That(callOrder++, Is.EqualTo(0)));
            userRepositoryMock.Setup(ur => ur.Save(It.IsAny<User>())).Callback(() => Assert.That(callOrder++, Is.EqualTo(1)));

            var userService = new UserService(userRepositoryMock.Object);

            // Act
            var user = userService.CreateUser(email, password, role);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(user.Email, Is.EqualTo(email));
                Assert.That(user.Role, Is.EqualTo(role));
            });
            userRepositoryMock.Verify(ur => ur.Get(email), Times.Once());
            userRepositoryMock.Verify(ur => ur.Save(user), Times.Once());
        }

        [Test]
        public void CreateUser_WithAlreadyUsedEmail_ThrowsEmailAlreadyUsedException()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var password = "123456";
            var role = Role.Admin;

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(ur => ur.Get(email)).Returns(new User(email, new byte[0], new byte[0], (short)role));

            var userService = new UserService(userRepositoryMock.Object);

            try
            {
                // Act
                userService.CreateUser(email, password, role);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is EmailAlreadyUsedException, Is.True);
                userRepositoryMock.Verify(ur => ur.Get(email), Times.Once());
                userRepositoryMock.Verify(ur => ur.Save(It.IsAny<User>()), Times.Never);
                return;
            }

            // Assert
            Assert.Fail();
        }

        [Test]
        public void LoginUser_WithValidCredentials_ReturnsLogedUser()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var password = "123456";

            var savedUser = User.Create(email, password, Role.Admin);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(ur => ur.Get(email)).Returns(savedUser);

            var userService = new UserService(userRepositoryMock.Object);

            // Act
            var user = userService.LoginUser(email, password);

            // Assert
            Assert.That(user.Email, Is.EqualTo(email));
            userRepositoryMock.Verify(ur => ur.Get(email), Times.Once());
        }

        [Test]
        public void LoginUser_WithoutRegistration_ThrowsInvalidCredentialsException()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var password = "123456";

            var userRepositoryMock = new Mock<IUserRepository>();

            var userService = new UserService(userRepositoryMock.Object);

            try
            {
                // Act
                userService.LoginUser(email, password);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is InvalidCredentialsException, Is.True);
                userRepositoryMock.Verify(ur => ur.Get(email), Times.Once());
                return;
            }

            // Assert
            Assert.Fail();
        }

        [Test]
        public void LoginUser_WithInvalidCredentials_ThrowsInvalidCredentialsException()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var password = "123456";

            var savedUser = User.Create(email, password + "789", Role.Admin);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(ur => ur.Get(email)).Returns(savedUser);

            var userService = new UserService(userRepositoryMock.Object);

            try
            {
                // Act
                userService.LoginUser(email, password);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is InvalidCredentialsException, Is.True);
                userRepositoryMock.Verify(ur => ur.Get(email), Times.Once());
                return;
            }

            // Assert
            Assert.Fail();
        }
    }
}
