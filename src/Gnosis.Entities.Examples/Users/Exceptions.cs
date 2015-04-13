namespace Gnosis.Entities.Examples.Users
{
    public class UsernameMissingException : GException
    {
    }

    public class PasswordMissingException : GException
    {
    }

    public class InvalidCredentialsException : GException
    {
        public InvalidCredentialsException(IUserCredentials credentials)
            : base("Invalid credentials for user {0}", credentials.Username)
        {
        }
    }
}