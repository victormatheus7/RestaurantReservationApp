using Domain.Enum;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public string CreatorEmail { get; }
        public DateTime Date { get; }
        public int NumberSeats { get; }
        public Location LocationPreference { get; }
        public string Observation { get; }

        public Reservation
            (Guid id, string creatorEmail, DateTime date, int numberSeats, short locationPreference, string observation)
        {
            Id = id;
            CreatorEmail = creatorEmail;
            Date = date;
            NumberSeats = numberSeats;
            LocationPreference = (Location)locationPreference;
            Observation = observation;
        }

        public static Reservation Create
            (string creatorEmail, DateTime date, int numberSeats, Location locationPreference, string observation, Guid? id = null)
        {
            CheckReservationData(date, numberSeats, observation);

            return new Reservation(id ?? Guid.NewGuid(), creatorEmail, date, numberSeats, (short)locationPreference, observation);
        }

        private static void CheckReservationData(DateTime date, int numberSeats, string observation)
        {
            var messages = new List<string>();

            if (!IsValidDate(date)) messages.Add("The date must be greater than the current date");
            if (!IsValidNumberSeats(numberSeats)) messages.Add("The number of seats must be greater than zero and less than one hundred");
            if (!IsValidObservation(observation)) messages.Add("The observation must have less than one thousand characters"); 
            
            if (messages.Any())
                throw new InvalidReservationException(messages);
        }

        private static bool IsValidDate(DateTime date)
        {
            return date > DateTime.Now;
        }

        private static bool IsValidNumberSeats(int numberSeats)
        {
            return numberSeats > 0 && numberSeats <= 100;
        }

        private static bool IsValidObservation(string observation)
        {
            return observation.Length < 1000;
        }
    }
}
