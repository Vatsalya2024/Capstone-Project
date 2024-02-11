using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    public class NoCustomersFoundException : Exception
    {
        public NoCustomersFoundException()
        {
        }

        public NoCustomersFoundException(string? message) : base(message)
        {
        }

        public NoCustomersFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoCustomersFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}