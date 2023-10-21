namespace UnitTests.Application
{
    internal class ReservationServiceTests
    {
        [Theory]
        public void CreateReservation_WithValidInformation_ReturnsCreatedReservation()
        {
            Assert.Fail();
        }

        [Theory]
        public void CreateReservation_WithInvalidInformation_ReturnsError()
        {
            Assert.Fail();
        }

        [Test]
        public void GetReservations_AsCustomer_ReturnsCustomerReservations()
        {
            Assert.Fail();
        }

        [Test]
        public void GetReservations_AsAdministrator_ReturnsAllReservations()
        {
            Assert.Fail();
        }

        [Test]
        public void GetSpecificReservation_AsCustomerWithPermission_ReturnsReservation()
        {
            Assert.Fail();
        }

        [Test]
        public void GetSpecificReservation_AsCustomerWithoutPermission_ReturnsError()
        {
            Assert.Fail();
        }

        [Test]
        public void GetSpecificReservation_AsAdministrator_ReturnsReservation()
        {
            Assert.Fail();
        }

        [Theory]
        public void UpdateReservation_WithPermission_ReturnsUpdatedReservation()
        {
            Assert.Fail();
        }

        [Theory]
        public void UpdateReservation_WithoutPermission_ReturnsError()
        {
            Assert.Fail();
        }

        [Theory]
        public void DeleteReservation_WithPermission_ReturnsSuccess()
        {
            Assert.Fail();
        }

        [Theory]
        public void DeleteReservation_WithoutPermission_ReturnsError()
        {
            Assert.Fail();
        }

        [Test]
        public void GetReservationsCountByDay_OfADayWithReservations_ReturnsReservationsCountByDay()
        {
            Assert.Fail();
        }

        [Test]
        public void GetReservationsCountByDay_OfADayWithoutReservations_ReturnsZero()
        {
            Assert.Fail();
        }
    }
}
