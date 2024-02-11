using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    internal class CustomerCreationException : Exception
    {
        public CustomerCreationException()
        {
        }

        public CustomerCreationException(string? message) : base(message)
        {
        }

        public CustomerCreationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CustomerCreationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}