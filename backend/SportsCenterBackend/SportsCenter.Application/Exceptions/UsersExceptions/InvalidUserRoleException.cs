namespace SportsCenter.Application.Exceptions.UsersException
{
    public class InvalidUserRoleException : Exception
    {
        public InvalidUserRoleException() : base("Unable to determine user role. No valid role found for the user.")
        {
        }
    }
}