using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Queries;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public User? Get(string email)
        {
            using var connection = CreateConnection();
            connection.Open();

            var user = connection.QueryFirstOrDefault<User>(UserQueries.GetUserByEmail, new { Email = email });

            return user;
        }

        public void Save(User user)
        {
            using var connection = CreateConnection();
            connection.Open();

            connection.Execute(UserQueries.SaveUser, 
                new 
                { 
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    PasswordSalt = user.PasswordSalt,
                    Role = user.Role
                });
        }
    }
}
