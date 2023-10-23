using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Queries;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Repositories
{
    public class ReservationRepository : BaseRepository, IReservationRepository
    {
        public ReservationRepository(IConfiguration configuration) : base(configuration) { }

        public int Count(DateTime day)
        {
            using var connection = CreateConnection();
            connection.Open();

            var count = connection.QueryFirst<int>(ReservationQueries.CountReservationsByDateRange, 
                new 
                { 
                    StartDate = day.Date,
                    EndDate = day.Date.AddDays(1).AddSeconds(-1)
                });

            return count;
        }

        public void Delete(Guid id)
        {
            using var connection = CreateConnection();
            connection.Open();

            connection.Execute(ReservationQueries.DeleteReservation, new { Id = id });
        }

        public Reservation? Get(Guid id)
        {
            using var connection = CreateConnection();
            connection.Open();

            var query = ReservationQueries.ListReservations + " WHERE id = @Id";
            var reservation = connection.QueryFirstOrDefault<Reservation>(query, new { Id = id });

            return reservation;
        }

        public IEnumerable<Reservation> List(string? email = null)
        {
            using var connection = CreateConnection();
            connection.Open();

            var query = ReservationQueries.ListReservations + (String.IsNullOrEmpty(email) ? "" : " WHERE creator_email = @Email");
            var reservation = connection.Query<Reservation>(query, new { Email = email });

            return reservation;
        }

        public void Save(Reservation reservation)
        {
            using var connection = CreateConnection();
            connection.Open();

            connection.Execute(ReservationQueries.SaveReservation, 
                new 
                { 
                    Id = reservation.Id,
                    CreatorEmail = reservation.CreatorEmail,
                    Date = reservation.Date,
                    NumberSeats = reservation.NumberSeats,
                    LocationPreference = reservation.LocationPreference,
                    Observation = reservation.Observation
                });
        }

        public void Update(Reservation reservation)
        {
            using var connection = CreateConnection();
            connection.Open();

            connection.Execute(ReservationQueries.SaveReservation,
                new
                {
                    Id = reservation.Id,
                    CreatorEmail = reservation.CreatorEmail,
                    Date = reservation.Date,
                    NumberSeats = reservation.NumberSeats,
                    LocationPreference = reservation.LocationPreference,
                    Observation = reservation.Observation
                });
        }
    }
}
