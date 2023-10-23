using Domain.Enum;

namespace WebAPI.Controllers.v1._0.Users
{
    public record UserViewModel(string email, string password, Role role);
}
