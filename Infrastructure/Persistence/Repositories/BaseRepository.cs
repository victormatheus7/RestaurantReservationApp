using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        private readonly IConfiguration _configuration;

        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("Default"));
        }
    }
}
