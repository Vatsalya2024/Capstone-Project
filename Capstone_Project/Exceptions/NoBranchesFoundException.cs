using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    internal class NoBranchesFoundException : Exception
    {
        public NoBranchesFoundException()
        {
        }

        public NoBranchesFoundException(string? message) : base(message)
        {
        }

        public NoBranchesFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoBranchesFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}