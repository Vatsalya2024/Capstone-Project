using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    internal class BeneficiaryAlreadyExistsException : Exception
    {
        public BeneficiaryAlreadyExistsException()
        {
        }

        public BeneficiaryAlreadyExistsException(string? message) : base(message)
        {
        }

        public BeneficiaryAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BeneficiaryAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}