using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        void SaveUser(User user);
        User GetUser(string email);
    }
}
