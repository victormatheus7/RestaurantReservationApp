namespace Application.Exceptions
{
    public sealed class UnregisteredReservationException : ApplicationException
    {
        public UnregisteredReservationException(Guid id) : base($"The reservation {id} doesn't exist.") { }
    }
}
