using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    internal class EmployeeUpdateException : Exception
    {
        public EmployeeUpdateException()
        {
        }

        public EmployeeUpdateException(string? message) : base(message)
        {
        }

        public EmployeeUpdateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EmployeeUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}