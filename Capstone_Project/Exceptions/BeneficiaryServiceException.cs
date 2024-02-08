using System.Runtime.Serialization;

namespace Capstone_Project.Exceptions
{
    [Serializable]
    internal class BeneficiaryServiceException : Exception
    {
        public BeneficiaryServiceException()
        {
        }

        public BeneficiaryServiceException(string? message) : base(message)
        {
        }

        public BeneficiaryServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BeneficiaryServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}