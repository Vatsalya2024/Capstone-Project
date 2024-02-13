using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    public class NoBankEmployeesFoundException : Exception
    {
        public NoBankEmployeesFoundException()
        {
        }

        public NoBankEmployeesFoundException(string? message) : base(message)
        {
        }

        public NoBankEmployeesFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoBankEmployeesFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}