using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;

namespace UnitTests.Domain
{
    internal class ReservationTests
    {
        internal class ReservationTestData
        {
            public string CreatorEmail { get; }
            public DateTime Date { get; }
            public int NumberSeats { get; }
            public Location LocationPreference { get; }
            public string Observation { get; }

            public ReservationTestData
            (string creatorEmail, DateTime date, int numberSeats, Location locationPreference, string observation)
            {
                CreatorEmail = creatorEmail;
                Date = date;
                NumberSeats = numberSeats;
                LocationPreference = locationPreference;
                Observation = observation;
            }
        }

        private static readonly ReservationTestData[] createReservation_WithValidInformation_ReturnsCreatedReservation_Input =
        new ReservationTestData[]
        {
            // Arrange
            new ReservationTestData("victor.castro@tests.com", DateTime.Now.AddDays(1), 1, Location.Indoor, ""),
            new ReservationTestData("victor.castro@tests.com.br", DateTime.Now.AddDays(120), 100, Location.Outdoor, "Test message."),
            new ReservationTestData("victor@tests.com", DateTime.Now.AddMinutes(1), 1, Location.Indoor, "Another test message."),
        };

        [Test]
        public void CreateReservation_WithValidInformation_ReturnsCreatedReservation
            ([ValueSource(nameof(createReservation_WithValidInformation_ReturnsCreatedReservation_Input))] ReservationTestData reservation)
        {
            // Act
            var reservationCreated = 
                Reservation.Create(creatorEmail: reservation.CreatorEmail,
                    date: reservation.Date,
                    numberSeats: reservation.NumberSeats,
                    locationPreference: reservation.LocationPreference,
                    observation: reservation.Observation);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(reservationCreated.CreatorEmail, Is.EqualTo(reservation.CreatorEmail));
                Assert.That(reservationCreated.Date, Is.EqualTo(reservation.Date));
                Assert.That(reservationCreated.NumberSeats, Is.EqualTo(reservation.NumberSeats));
                Assert.That(reservationCreated.LocationPreference, Is.EqualTo(reservation.LocationPreference));
                Assert.That(reservationCreated.Observation, Is.EqualTo(reservation.Observation));
            });
        }

        private static readonly ReservationTestData[] createReservation_WithInvalidInformation_ThrowsInvalidReservationException_Input =
        new ReservationTestData[]
        {
            // Arrange
            new ReservationTestData("victor.castro@tests.com", DateTime.Now.AddSeconds(-1), 1, Location.Indoor, ""),
            new ReservationTestData("victor.castro@tests.com.br", DateTime.Now.AddDays(1), 0, Location.Outdoor, "Test message."),
            new ReservationTestData("victor@tests.com", DateTime.Now.AddDays(1), 101, Location.Indoor, "Another test message."),
            new ReservationTestData("victor@tests.com", DateTime.Now.AddDays(1), 1, Location.Indoor, "Very long observation" + new string(' ', 1000)),
        };

        [Theory]
        public void CreateReservation_WithInvalidInformation_ThrowsInvalidReservationException
            ([ValueSource(nameof(createReservation_WithInvalidInformation_ThrowsInvalidReservationException_Input))] ReservationTestData reservation)
        {
            try
            {
                // Act
                var reservationCreated =
                    Reservation.Create(creatorEmail: reservation.CreatorEmail,
                        date: reservation.Date,
                        numberSeats: reservation.NumberSeats,
                        locationPreference: reservation.LocationPreference,
                        observation: reservation.Observation);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is InvalidReservationException, Is.True);
                return;
            }

            // Assert
            Assert.Fail();
        }
    }
}
