using Domain.Exceptions;
using Domain.Extensions;
using System.Text.RegularExpressions;

namespace Domain.Entities
{
    public class User
    {
        public string Email { get; }
        public string Password { get; }

        public User(string email, string password)
        {
            if (!IsValidEmail(email)) throw new InvalidEmailException(email);

            if (!IsValidPassword(password)) throw new InvalidPasswordException();


            Email = email;
            Password = password;
        }

        private static bool IsValidEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (string.IsNullOrEmpty(email) || email.Length > 254)
                return false;

            var regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }

        private static bool IsValidPassword(string password)
        {
            return password.Length >= 6 && password.Length <= 12 && password.IsAllLettersOrDigits();
        }

    }
}
