using System.Runtime.Serialization;

namespace Capstone_Project.Controllers
{
    [Serializable]
    public class NoAccountsFoundException : Exception
    {
        public NoAccountsFoundException()
        {
        }

        public NoAccountsFoundException(string? message) : base(message)
        {
        }

        public NoAccountsFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoAccountsFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}