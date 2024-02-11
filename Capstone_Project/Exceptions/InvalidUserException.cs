using System.Runtime.Serialization;

namespace Capstone_Project.Controllers
{
    [Serializable]
    public class InvalidUserException : Exception
    {
        public InvalidUserException()
        {
        }

        public InvalidUserException(string? message) : base("Invalid username or password")
        {
        }


    }
}