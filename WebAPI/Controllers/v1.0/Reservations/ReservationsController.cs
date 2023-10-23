using Application.Services;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace WebAPI.Controllers.v1._0.Reservations
{
    [ApiController]
    [Route("api/v1.0/reservations")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly ILogger<ReservationsController> _logger;
        private readonly ReservationService _reservationService;

        public ReservationsController(ILogger<ReservationsController> logger, ReservationService reservationService)
        {
            _logger = logger;
            _reservationService = reservationService;
        }

        [HttpPost]
        public IActionResult CreateReservation([FromBody]ReservationViewModel reservation)
        {
            try
            {
                var userEmail = GetAuthenticatedUserEmail();

                _reservationService.CreateReservation(userEmail,
                    reservation.Date,
                    reservation.NumberSeats,
                    reservation.LocationPreference,
                    reservation.Observation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Reservation), (int)HttpStatusCode.OK)]
        public IActionResult GetReservation(Guid id)
        {
            Reservation reservation;
            try
            {
                var userEmail = GetAuthenticatedUserEmail();
                var userRole = GetAuthenticatedUserRole();

                reservation = _reservationService.GetReservation(userEmail, userRole, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(reservation);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Reservation>), (int)HttpStatusCode.OK)]
        public IActionResult ListReservations()
        {
            IEnumerable<Reservation> reservations;
            try
            {
                var userEmail = GetAuthenticatedUserEmail();
                var userRole = GetAuthenticatedUserRole();

                reservations = _reservationService.ListUsersReservations(userEmail, userRole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(reservations);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateReservation(Guid id, [FromBody]ReservationViewModel reservation)
        {
            try
            {
                var userEmail = GetAuthenticatedUserEmail();
                var userRole = GetAuthenticatedUserRole();

                _reservationService.UpdateReservation(userEmail, userRole,
                    id,
                    reservation.Date,
                    reservation.NumberSeats,
                    reservation.LocationPreference,
                    reservation.Observation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteReservation(Guid id)
        {
            try
            {
                var userEmail = GetAuthenticatedUserEmail();
                var userRole = GetAuthenticatedUserRole();

                _reservationService.DeleteReservation(userEmail, userRole, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet("count/{date:dateTime}"), AllowAnonymous]
        public IActionResult CountReservationsByDay(DateTime date)
        {
            int count;
            try
            {
                count = _reservationService.CountReservationsByDay(date);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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
