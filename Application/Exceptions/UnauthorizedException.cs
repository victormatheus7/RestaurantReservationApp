namespace Application.Exceptions
{
    public sealed class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException() : base("You do not have authorization to perform this action.") { }
    }
}
