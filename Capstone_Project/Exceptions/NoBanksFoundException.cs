using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    internal class NoBanksFoundException : Exception
    {
        public NoBanksFoundException()
        {
        }

        public NoBanksFoundException(string? message) : base(message)
        {
        }

        public NoBanksFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoBanksFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}