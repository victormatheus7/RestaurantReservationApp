using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Queries
{
    [ExcludeFromCodeCoverage]
    public static class UserQueries
    {
        public static string GetUserByEmail =>
            @"SELECT email AS ""Email"", 
                     password_hash AS ""PasswordHash"",
                     password_salt AS ""PasswordSalt"",
                     role_id AS ""Role"" 
              FROM public.user
              WHERE email = @Email";

        public static string SaveUser =>
            @"INSERT INTO public.user (email, password_hash, password_salt, role_id) 
			  VALUES (@Email, @PasswordHash, @PasswordSalt, @Role)";
    }
}
