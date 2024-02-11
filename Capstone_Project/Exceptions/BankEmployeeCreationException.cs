using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    internal class BankEmployeeCreationException : Exception
    {
        public BankEmployeeCreationException()
        {
        }

        public BankEmployeeCreationException(string? message) : base(message)
        {
        }

        public BankEmployeeCreationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BankEmployeeCreationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}