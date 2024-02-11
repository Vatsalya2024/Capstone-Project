using System;
namespace Capstone_Project.Exceptions
{
    public class NoTransactionsException : Exception
    {
        public NoTransactionsException(string message) : base(message)
        {
        }
    }
}

