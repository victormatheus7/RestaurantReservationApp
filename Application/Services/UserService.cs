using Application.Exceptions;
using Domain.Entities;
using Domain.Enum;
using Domain.Repositories;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public User CreateUser(string email, string password, Role role)
        {
            CheckIfUserAlreadyExists(email);

            var user = User.Create(email, password, role);

            _userRepository.Save(user);

            return user;
        }

        public User LoginUser(string email, string password)
        {
            var savedUser = _userRepository.Get(email);
            CheckIfUserExists(savedUser);

            var user = User.Create(email, password, savedUser.Role, savedUser.PasswordSalt);
            CheckIfCredentialsMatch(savedUser, user);

            return savedUser;
        }

        private static void CheckIfUserExists(User savedUser)
        {
            if (savedUser == null) throw new InvalidCredentialsException();
        }

        private static void CheckIfCredentialsMatch(User savedUser, User user)
        {
            if (!user.PasswordHash.SequenceEqual(savedUser.PasswordHash)) throw new InvalidCredentialsException();
        }

        private void CheckIfUserAlreadyExists(string email)
        {
            var user = _userRepository.Get(email);
            if (user is not null) throw new EmailAlreadyUsedException(email);
        }
    }
}
