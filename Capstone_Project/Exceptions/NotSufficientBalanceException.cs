using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    internal class NotSufficientBalanceException : Exception
    {
        string message;
        public NotSufficientBalanceException()
        {
            message = "Not Sufficient Balance";
        }

        public override string Message => message;

    }
}