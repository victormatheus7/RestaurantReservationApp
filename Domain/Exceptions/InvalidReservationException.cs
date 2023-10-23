namespace Domain.Exceptions
{
    public sealed class InvalidReservationException : DomainException
    {
        public InvalidReservationException(IEnumerable<string> errorMessages) 
            : base($"The reservation contains incorrect data. {String.Join(", ",errorMessages)}.") { }
    }
}
