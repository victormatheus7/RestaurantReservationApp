using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Queries
{
    [ExcludeFromCodeCoverage]
    public static class UserQueries
    {
        public static string GetUserByEmail =>
            @"SELECT [Email], 
                     [PasswordHash],
                     [PasswordSalt],
                     [Role] 
              FROM [User]
              WHERE [Email] = @Email";

        public static string SaveUser =>
            @"INSERT INTO [User] ([Email], [PasswordHash], [PasswordSalt], [Role]) 
			  VALUES (@Email, @PasswordHash, @PasswordSalt, @Role)";
    }
}
