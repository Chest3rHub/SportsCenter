namespace SportsCenter.Application.Exceptions
{
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException() : base("Invalid credentials.")
        {
        }
    }
}
