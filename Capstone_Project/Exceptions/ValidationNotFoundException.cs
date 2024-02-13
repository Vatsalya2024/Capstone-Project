using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    public class ValidationNotFoundException : Exception
    {
        public ValidationNotFoundException()
        {
        }

        public ValidationNotFoundException(string? message) : base(message)
        {
        }

        public ValidationNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ValidationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}