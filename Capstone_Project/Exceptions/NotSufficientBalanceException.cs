using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    internal class NotSufficientBalanceException : Exception
    {
        public NotSufficientBalanceException()
        {
        }

        public NotSufficientBalanceException(string? message) : base(message)
        {
        }

        public NotSufficientBalanceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotSufficientBalanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}