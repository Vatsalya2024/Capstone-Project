using System;
namespace Capstone_Project.Exceptions
{
    public class DeactivatedUserException : Exception
    {
        public DeactivatedUserException() : base("User deactivated")
        {
        }
    }
}

