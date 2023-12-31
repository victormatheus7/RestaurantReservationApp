﻿using Domain.Enum;
using Domain.Exceptions;
using Domain.Extensions;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Domain.Entities
{
    public class User
    {
        public string Email { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public Role Role { get; private set; }

        public User(string email, byte[] passwordHash, byte[] passwordSalt, short role)
        {
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Role = (Role)role;
        }

        public static User Create(string email, string password, Role role, byte[]? passwordSalt = null)
        {
            CheckUserData(email, password);

            using var hmac = passwordSalt is not null ? new HMACSHA512(passwordSalt) : new HMACSHA512();

            return new User(email,
                hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)),
                hmac.Key,
                (short)role);
        }

        private static void CheckUserData(string email, string password)
        {
            if (!IsValidEmail(email)) throw new InvalidEmailException(email);

            if (!IsValidPassword(password)) throw new InvalidPasswordException();
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
