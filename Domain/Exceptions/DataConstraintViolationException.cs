namespace Domain.Exceptions;

public class DataConstraintViolationException : Exception
{
    public DataConstraintViolationException(string message) : base(message)
    {
        
    }
}