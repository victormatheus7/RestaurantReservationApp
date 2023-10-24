using Application.Services;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers.v1._0.Reservations
{
    [ApiController]
    [Route("api/v1.0/reservations")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationsController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public IActionResult CreateReservation([FromBody]ReservationViewModel reservation)
        {
            var userEmail = GetAuthenticatedUserEmail();

            _reservationService.CreateReservation(userEmail,
                reservation.Date,
                reservation.NumberSeats,
                reservation.LocationPreference,
                reservation.Observation);

            return Ok();
        }

        [HttpGet("{id:guid}")]
        public ActionResult<IEnumerable<Reservation>> GetReservation(Guid id)
        {
            var userEmail = GetAuthenticatedUserEmail();
            var userRole = GetAuthenticatedUserRole();

            var reservation = _reservationService.GetReservation(userEmail, userRole, id);

            return Ok(reservation);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> ListReservations()
        {
            var userEmail = GetAuthenticatedUserEmail();
            var userRole = GetAuthenticatedUserRole();

            var reservations = _reservationService.ListUsersReservations(userEmail, userRole);

            return Ok(reservations);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateReservation(Guid id, [FromBody]ReservationViewModel reservation)
        {
            var userEmail = GetAuthenticatedUserEmail();
            var userRole = GetAuthenticatedUserRole();

            _reservationService.UpdateReservation(userEmail, userRole,
                id,
                reservation.Date,
                reservation.NumberSeats,
                reservation.LocationPreference,
                reservation.Observation);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteReservation(Guid id)
        {
            var userEmail = GetAuthenticatedUserEmail();
            var userRole = GetAuthenticatedUserRole();

            _reservationService.DeleteReservation(userEmail, userRole, id);

            return Ok();
        }

        [HttpGet("count/{date:dateTime}"), AllowAnonymous]
        public ActionResult<int> CountReservationsByDay(DateTime date)
        {
            var count = _reservationService.CountReservationsByDay(date);

            return Ok(count);
        }

        private string GetAuthenticatedUserEmail()
        {
            return User.FindFirst(ClaimTypes.Email).Value;
        }

        private Role GetAuthenticatedUserRole()
        {
            return (Role)short.Parse(User.FindFirst(ClaimTypes.Role).Value);
        }
    }
}
