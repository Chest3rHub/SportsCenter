namespace SportsCenter.Application.Exceptions.UsersException
{
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException() : base("Invalid credentials.")
        {
        }
    }
}
