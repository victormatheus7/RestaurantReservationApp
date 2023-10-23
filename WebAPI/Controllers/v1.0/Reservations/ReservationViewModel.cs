using Domain.Enum;

namespace WebAPI.Controllers.v1._0.Reservations
{
    public record ReservationViewModel(DateTime Date, int NumberSeats, Location LocationPreference, string Observation);
}
