using Domain.Entities;

namespace Domain.Repositories
{
    public interface IReservationRepository
    {
        void Save(Reservation reservation);
        Reservation? Get(Guid id);
        void Update(Reservation reservation);
        void Delete(Guid id);
        IEnumerable<Reservation> List(string? email = null);
        int Count(DateTime day);
    }
}
