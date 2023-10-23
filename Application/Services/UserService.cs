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

            var user = User.CreateUser(email, password, role);

            _userRepository.SaveUser(user);

            return user;
        }

        public User LoginUser(string email, string password)
        {
            var userSaved = _userRepository.GetUser(email);
            CheckIfUserExists(userSaved);

            var user = User.CreateUser(email, password, userSaved.Role, userSaved.PasswordSalt);
            CheckIfCredentialsMatch(userSaved, user);

            return userSaved;
        }

        private static void CheckIfUserExists(User userSaved)
        {
            if (userSaved == null) throw new InvalidCredentialsException();
        }

        private static void CheckIfCredentialsMatch(User userSaved, User user)
        {
            if (!user.PasswordHash.SequenceEqual(userSaved.PasswordHash)) throw new InvalidCredentialsException();
        }

        private void CheckIfUserAlreadyExists(string email)
        {
            var user = _userRepository.GetUser(email);
            if (user is not null) throw new EmailAlreadyUsedException(email);
        }
    }
}
