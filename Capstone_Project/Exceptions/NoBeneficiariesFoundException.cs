using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    public class NoBeneficiariesFoundException : Exception
    {
        public NoBeneficiariesFoundException()
        {
        }

        public NoBeneficiariesFoundException(string? message) : base(message)
        {
        }

        public NoBeneficiariesFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoBeneficiariesFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}