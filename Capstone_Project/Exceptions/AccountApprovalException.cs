using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    internal class AccountApprovalException : Exception
    {
        public AccountApprovalException()
        {
        }

        public AccountApprovalException(string? message) : base(message)
        {
        }

        public AccountApprovalException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccountApprovalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}