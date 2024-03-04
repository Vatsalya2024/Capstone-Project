using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    public class BankTransactionServiceException : Exception
    {
        public BankTransactionServiceException()
        {
        }

        public BankTransactionServiceException(string? message) : base(message)
        {
        }

        public BankTransactionServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BankTransactionServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}