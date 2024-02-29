using System.Runtime.Serialization;

namespace Capstone_Project.Services
{
    [Serializable]
    public class NotSufficientBalanceException : Exception
    {
        string message;
        public NotSufficientBalanceException()
        {
            message = "Not Sufficient Balance";
        }

        public override string Message => message;

    }
}