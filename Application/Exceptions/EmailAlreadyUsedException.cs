namespace Application.Exceptions
{
    public sealed class EmailAlreadyUsedException : ApplicationException
    {
        public EmailAlreadyUsedException(string email) : base($"The email \"{email}\" was already used.") { }
    }
}
