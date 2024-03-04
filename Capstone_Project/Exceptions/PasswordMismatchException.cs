using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    public class PasswordMismatchException : Exception
    {
        public PasswordMismatchException()
        {
        }

        public PasswordMismatchException(string? message) : base(message)
        {
        }

        public PasswordMismatchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PasswordMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}