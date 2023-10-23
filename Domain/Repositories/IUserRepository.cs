using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        void Save(User user);
        User? Get(string email);
    }
}
