using Application.Exceptions;
using Domain.Entities;
using Domain.Enum;
using Domain.Repositories;

namespace Application.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository) 
        {
            _reservationRepository = reservationRepository;
        }

        public Reservation CreateReservation(string email, DateTime date, int numberSeats, Location locationPreference, string observation)
        {
            var reservation = Reservation.Create(email, date, numberSeats, locationPreference, observation);

            _reservationRepository.Save(reservation);

            return reservation;
        }

        public Reservation GetReservation(string email, Role role, Guid id)
        {
            var savedReservation = _reservationRepository.Get(id);

            CheckIfReservationExists(id, savedReservation);
            CheckIfUserHasAccess(email, role, savedReservation);

            return savedReservation;
        }

        public Reservation UpdateReservation
            (string email, Role role, Guid id, DateTime date, int numberSeats, Location locationPreference, string observation)
        {
            var savedReservation = GetReservation(email, role, id);

            var reservation = Reservation.Create(savedReservation.CreatorEmail, date, numberSeats, locationPreference, observation, id);
            _reservationRepository.Update(reservation);

            return reservation;
        }

        public void DeleteReservation(string email, Role role, Guid id)
        {
            GetReservation(email, role, id);

            _reservationRepository.Delete(id);
        }

        public IEnumerable<Reservation> ListUsersReservations(string email, Role role)
        {
            return role == Role.Admin ? _reservationRepository.List() : _reservationRepository.List(email);
        }

        public int CountReservationsByDay(DateTime date)
        { 
            return _reservationRepository.Count(date);
        }

        private static void CheckIfReservationExists(Guid id, Reservation savedReservation)
        {
            if (savedReservation == null) throw new UnregisteredReservationException(id);
        }

        private static void CheckIfUserHasAccess(string email, Role role, Reservation savedReservation)
        {
            if (role != Role.Admin && savedReservation.CreatorEmail != email) throw new UnauthorizedException();
        }
    }
}
