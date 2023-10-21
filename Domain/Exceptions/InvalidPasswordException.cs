namespace Domain.Exceptions
{
    public sealed class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException() 
            : base("The password must have a minimum of 6 and a maximum of 12 characters, containing only letters and numbers.") { }
    }
}
