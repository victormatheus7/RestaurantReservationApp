using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using Domain.Enum;
using Domain.Repositories;
using Moq;

namespace UnitTests.Application
{
    internal class ReservationServiceTests
    {
        [Test]
        public void CreateReservation_WithValidInformation_ReturnsCreatedReservation()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservationRepository = new Mock<IReservationRepository>();

            var reservationService = new ReservationService(reservationRepository.Object);

            // Act
            var reservation = reservationService.CreateReservation(email, date, numberSeats, locationPreference, observation);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(reservation.CreatorEmail, Is.EqualTo(email));
                Assert.That(reservation.Date, Is.EqualTo(date));
                Assert.That(reservation.NumberSeats, Is.EqualTo(numberSeats));
                Assert.That(reservation.LocationPreference, Is.EqualTo(locationPreference));
                Assert.That(reservation.Observation, Is.EqualTo(observation));
            });
            reservationRepository.Verify(rr => rr.Save(It.IsAny<Reservation>()), Times.Once());
        }

        [Test]
        public void GetReservations_AsCustomer_ReturnsCustomerReservations()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Client;
            var id = Guid.NewGuid();

            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservationRepository = new Mock<IReservationRepository>();
            reservationRepository.Setup(rr => rr.List(email))
                .Returns(new List<Reservation>() { new Reservation(id, email, date, numberSeats, locationPreference, observation) });

            var reservationService = new ReservationService(reservationRepository.Object);

            // Act
            var reservation = reservationService.ListUsersReservations(email, role);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(reservation.Any(), Is.True);
                Assert.That(reservation.All(r => r.CreatorEmail == email), Is.True);
            });
            reservationRepository.Verify(rr => rr.List(email), Times.Once());
        }

        [Test]
        public void GetReservations_AsAdministrator_ReturnsAllReservations()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Admin;
            var id = Guid.NewGuid();

            var customerEmail = "customer@company.com";
            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservationRepository = new Mock<IReservationRepository>();
            reservationRepository.Setup(rr => rr.List(It.IsAny<string>()))
                .Returns(new List<Reservation>() { new Reservation(id, customerEmail, date, numberSeats, locationPreference, observation) });

            var reservationService = new ReservationService(reservationRepository.Object);

            // Act
            var reservation = reservationService.ListUsersReservations(email, role);

            // Assert
            Assert.That(reservation.Any(), Is.True);
            reservationRepository.Verify(rr => rr.List(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void GetSpecificReservation_AsCustomerWithPermission_ReturnsReservation()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Client;
            var id = Guid.NewGuid();

            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservationRepository = new Mock<IReservationRepository>();
            reservationRepository.Setup(rr => rr.Get(id)).Returns(new Reservation(id, email, date, numberSeats, locationPreference, observation));

            var reservationService = new ReservationService(reservationRepository.Object);

            // Act
            var reservation = reservationService.GetReservation(email, role, id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(reservation.Id, Is.EqualTo(id));
                Assert.That(reservation.CreatorEmail, Is.EqualTo(email));
                Assert.That(reservation.Date, Is.EqualTo(date));
                Assert.That(reservation.NumberSeats, Is.EqualTo(numberSeats));
                Assert.That(reservation.LocationPreference, Is.EqualTo(locationPreference));
                Assert.That(reservation.Observation, Is.EqualTo(observation));
            });
            reservationRepository.Verify(rr => rr.Get(id), Times.Once());
        }

        [Test]
        public void GetSpecificReservation_AsCustomerWithoutPermission_ThrowsUnauthorizedException()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Client;
            var id = Guid.NewGuid();

            var customerEmail = "customer@company.com";
            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservationRepository = new Mock<IReservationRepository>();
            reservationRepository.Setup(rr => rr.Get(id)).Returns(new Reservation(id, customerEmail, date, numberSeats, locationPreference, observation));

            var reservationService = new ReservationService(reservationRepository.Object);

            try
            {
                // Act
                reservationService.GetReservation(email, role, id);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is UnauthorizedException, Is.True);
                reservationRepository.Verify(rr => rr.Get(id), Times.Once());
                return;
            }

            // Assert
            Assert.Fail();
        }

        [Test]
        public void GetSpecificReservation_AsAdministrator_ReturnsReservation()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Admin;
            var id = Guid.NewGuid();

            var customerEmail = "customer@company.com";
            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservationRepository = new Mock<IReservationRepository>();
            reservationRepository.Setup(rr => rr.Get(id)).Returns(new Reservation(id, customerEmail, date, numberSeats, locationPreference, observation));

            var reservationService = new ReservationService(reservationRepository.Object);

            // Act
            var reservation = reservationService.GetReservation(email, role, id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(reservation.Id, Is.EqualTo(id));
                Assert.That(reservation.CreatorEmail, Is.EqualTo(customerEmail));
                Assert.That(reservation.Date, Is.EqualTo(date));
                Assert.That(reservation.NumberSeats, Is.EqualTo(numberSeats));
                Assert.That(reservation.LocationPreference, Is.EqualTo(locationPreference));
                Assert.That(reservation.Observation, Is.EqualTo(observation));
            });
            reservationRepository.Verify(rr => rr.Get(id), Times.Once());
        }

        [Test]
        public void GetSpecificReservation_WithUnregisteredReservation_ReturnsUnregisteredException()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Client;
            var id = Guid.NewGuid();

            var reservationRepository = new Mock<IReservationRepository>();

            var reservationService = new ReservationService(reservationRepository.Object);

            try
            {
                // Act
                reservationService.GetReservation(email, role, id);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is UnregisteredReservationException, Is.True);
                reservationRepository.Verify(rr => rr.Get(id), Times.Once());
                return;
            }

            // Assert
            Assert.Fail();
        }

        [Test]
        public void UpdateReservation_AsCustomerWithPermission_ReturnsUpdatedReservation()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Client;
            var id = Guid.NewGuid();

            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservation = new Reservation(id, email, date, numberSeats, locationPreference, observation);

            var reservationRepository = new Mock<IReservationRepository>();
            int callOrder = 0;
            reservationRepository.Setup(rr => rr.Get(id)).Returns(reservation).Callback(() => Assert.That(callOrder++, Is.EqualTo(0)));
            reservationRepository.Setup(rr => rr.Update(It.IsAny<Reservation>())).Callback(() => Assert.That(callOrder++, Is.EqualTo(1)));

            var reservationService = new ReservationService(reservationRepository.Object);

            // Act
            var updatedReservation = 
                reservationService.UpdateReservation(email, role, id, date, numberSeats, locationPreference, observation);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(updatedReservation.Id, Is.EqualTo(id));
                Assert.That(updatedReservation.CreatorEmail, Is.EqualTo(email));
                Assert.That(updatedReservation.Date, Is.EqualTo(date));
                Assert.That(updatedReservation.NumberSeats, Is.EqualTo(numberSeats));
                Assert.That(updatedReservation.LocationPreference, Is.EqualTo(locationPreference));
                Assert.That(updatedReservation.Observation, Is.EqualTo(observation));
            });
            reservationRepository.Verify(rr => rr.Get(id), Times.Once());
            reservationRepository.Verify(rr => rr.Update(It.IsAny<Reservation>()), Times.Once());
        }

        [Test]
        public void UpdateReservation_AsCustomerWithoutPermission_ThrowsUnauthorizedException()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Client;
            var id = Guid.NewGuid();

            var customerEmail = "customer@company.com";
            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservation = new Reservation(id, customerEmail, date, numberSeats, locationPreference, observation);

            var reservationRepository = new Mock<IReservationRepository>();
            reservationRepository.Setup(rr => rr.Get(id)).Returns(reservation);

            var reservationService = new ReservationService(reservationRepository.Object);

            try
            {
                // Act
                reservationService.UpdateReservation(email, role, id, date, numberSeats, locationPreference, observation);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is UnauthorizedException, Is.True);
                reservationRepository.Verify(rr => rr.Get(id), Times.Once());
                return;
            }

            // Assert
            Assert.Fail();
        }

        [Test]
        public void UpdateReservation_AsAdministrator_ReturnsUpdatedReservation()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Admin;
            var id = Guid.NewGuid();

            var customerEmail = "customer@company.com";
            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservation = new Reservation(id, customerEmail, date, numberSeats, locationPreference, observation);

            var reservationRepository = new Mock<IReservationRepository>();
            int callOrder = 0;
            reservationRepository.Setup(rr => rr.Get(id)).Returns(reservation).Callback(() => Assert.That(callOrder++, Is.EqualTo(0)));
            reservationRepository.Setup(rr => rr.Update(It.IsAny<Reservation>())).Callback(() => Assert.That(callOrder++, Is.EqualTo(1)));

            var reservationService = new ReservationService(reservationRepository.Object);

            // Act
            var updatedReservation =
                reservationService.UpdateReservation(email, role, id, date, numberSeats, locationPreference, observation);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(updatedReservation.Id, Is.EqualTo(id));
                Assert.That(updatedReservation.CreatorEmail, Is.EqualTo(customerEmail));
                Assert.That(updatedReservation.Date, Is.EqualTo(date));
                Assert.That(updatedReservation.NumberSeats, Is.EqualTo(numberSeats));
                Assert.That(updatedReservation.LocationPreference, Is.EqualTo(locationPreference));
                Assert.That(updatedReservation.Observation, Is.EqualTo(observation));
            });
            reservationRepository.Verify(rr => rr.Get(id), Times.Once());
            reservationRepository.Verify(rr => rr.Update(It.IsAny<Reservation>()), Times.Once());
        }

        [Test]
        public void UpdateReservation_WithUnregisteredReservation_ReturnsUnregisteredException()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Client;
            var id = Guid.NewGuid();

            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservationRepository = new Mock<IReservationRepository>();

            var reservationService = new ReservationService(reservationRepository.Object);

            try
            {
                // Act
                reservationService.UpdateReservation(email, role, id, date, numberSeats, locationPreference, observation);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is UnregisteredReservationException, Is.True);
                reservationRepository.Verify(rr => rr.Get(id), Times.Once());
                return;
            }

            // Assert
            Assert.Fail();
        }

        [Test]
        public void DeleteReservation_AsCustomerWithPermission_ReturnsWithoutErrors()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Client;
            var id = Guid.NewGuid();

            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservation = new Reservation(id, email, date, numberSeats, locationPreference, observation);

            var reservationRepository = new Mock<IReservationRepository>();
            int callOrder = 0;
            reservationRepository.Setup(rr => rr.Get(id)).Returns(reservation).Callback(() => Assert.That(callOrder++, Is.EqualTo(0)));
            reservationRepository.Setup(rr => rr.Delete(It.IsAny<Guid>())).Callback(() => Assert.That(callOrder++, Is.EqualTo(1)));

            var reservationService = new ReservationService(reservationRepository.Object);

            // Act
            reservationService.DeleteReservation(email, role, id);

            // Assert
            reservationRepository.Verify(rr => rr.Get(id), Times.Once());
            reservationRepository.Verify(rr => rr.Delete(It.IsAny<Guid>()), Times.Once());
        }

        [Test]
        public void DeleteReservation_AsCustomerWithoutPermission_ThrowsUnauthorizedException()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Client;
            var id = Guid.NewGuid();

            var customerEmail = "customer@company.com";
            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservation = new Reservation(id, customerEmail, date, numberSeats, locationPreference, observation);

            var reservationRepository = new Mock<IReservationRepository>();
            reservationRepository.Setup(rr => rr.Get(id)).Returns(reservation);

            var reservationService = new ReservationService(reservationRepository.Object);

            try
            {
                // Act
                reservationService.DeleteReservation(email, role, id);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is UnauthorizedException, Is.True);
                reservationRepository.Verify(rr => rr.Get(id), Times.Once());
                return;
            }

            // Assert
            Assert.Fail();
        }

        [Test]
        public void DeleteReservation_AsAdministrator_ReturnsWithoutErrors()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Admin;
            var id = Guid.NewGuid();

            var customerEmail = "customer@company.com";
            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservation = new Reservation(id, customerEmail, date, numberSeats, locationPreference, observation);

            var reservationRepository = new Mock<IReservationRepository>();
            int callOrder = 0;
            reservationRepository.Setup(rr => rr.Get(id)).Returns(reservation).Callback(() => Assert.That(callOrder++, Is.EqualTo(0)));
            reservationRepository.Setup(rr => rr.Delete(It.IsAny<Guid>())).Callback(() => Assert.That(callOrder++, Is.EqualTo(1)));

            var reservationService = new ReservationService(reservationRepository.Object);

            // Act
            reservationService.DeleteReservation(email, role, id);

            // Assert
            reservationRepository.Verify(rr => rr.Get(id), Times.Once());
            reservationRepository.Verify(rr => rr.Delete(It.IsAny<Guid>()), Times.Once());
        }

        [Test]
        public void DeleteReservation_WithUnregisteredReservation_ReturnsUnregisteredException()
        {
            // Arrange
            var email = "victor.castro@tests.com";
            var role = Role.Client;
            var id = Guid.NewGuid();

            var reservationRepository = new Mock<IReservationRepository>();

            var reservationService = new ReservationService(reservationRepository.Object);

            try
            {
                // Act
                reservationService.DeleteReservation(email, role, id);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex is UnregisteredReservationException, Is.True);
                reservationRepository.Verify(rr => rr.Get(id), Times.Once());
                return;
            }

            // Assert
            Assert.Fail();
        }

        [Test]
        public void GetReservationsCountByDay_OfADayWithReservations_ReturnsReservationsCountByDay()
        {
            // Arrange
            var id = Guid.NewGuid();
            var customerEmail = "customer@company.com";
            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservation = new Reservation(id, customerEmail, date, numberSeats, locationPreference, observation);

            var reservationRepository = new Mock<IReservationRepository>();
            reservationRepository.Setup(rr => rr.Count(date)).Returns(1);

            var reservationService = new ReservationService(reservationRepository.Object);

            // Act
            var count = reservationService.CountReservationsByDay(date);

            // Assert
            Assert.That(count, Is.EqualTo(1));
            reservationRepository.Verify(rr => rr.Count(date), Times.Once());
        }

        [Test]
        public void GetReservationsCountByDay_OfADayWithoutReservations_ReturnsZero()
        {
            // Arrange
            var id = Guid.NewGuid();
            var customerEmail = "customer@company.com";
            var date = DateTime.Now.AddDays(1);
            var numberSeats = 1;
            var locationPreference = Location.Indoor;
            var observation = "This is a test.";

            var reservation = new Reservation(id, customerEmail, date, numberSeats, locationPreference, observation);

            var reservationRepository = new Mock<IReservationRepository>();
            reservationRepository.Setup(rr => rr.Count(date)).Returns(1);

            var reservationService = new ReservationService(reservationRepository.Object);

            // Act
            var count = reservationService.CountReservationsByDay(date.AddDays(1));

            // Assert
            Assert.That(count, Is.EqualTo(0));
            reservationRepository.Verify(rr => rr.Count(date.AddDays(1)), Times.Once());
        }
    }
}
