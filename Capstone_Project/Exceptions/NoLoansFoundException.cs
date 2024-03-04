using System.Runtime.Serialization;

[Serializable]
public class NoLoansFoundException : Exception
{
    public NoLoansFoundException()
    {
    }

    public NoLoansFoundException(string? message) : base(message)
    {
    }

    public NoLoansFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NoLoansFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}