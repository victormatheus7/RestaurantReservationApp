using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPI.Controllers.v1._0.Reservations
{
    [ApiController]
    [Route("api/v1.0/reservations")]
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
                _reservationService.CreateReservation("test@test.com",
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


        [Route("{id:guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(Reservation), (int)HttpStatusCode.OK)]
        public IActionResult GetReservation(Guid id)
        {
            Reservation reservation;
            try
            {
                reservation = _reservationService.GetReservation("test@test.com", Domain.Enum.Role.Admin, id);
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
                reservations = _reservationService.ListUsersReservations("test@test.com", Domain.Enum.Role.Admin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(reservations);
        }

        [Route("{id:guid}")]
        [HttpPatch]
        public IActionResult UpdateReservation(Guid id, [FromBody]ReservationViewModel reservation)
        {
            try
            {
                _reservationService.UpdateReservation("test@test.com", Domain.Enum.Role.Admin,
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

        [Route("{id:guid}")]
        [HttpDelete]
        public IActionResult DeleteReservation(Guid id)
        {
            try
            {
                _reservationService.DeleteReservation("test@test.com", Domain.Enum.Role.Admin, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Route("count/{date:dateTime}")]
        [HttpGet]
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
    }
}
