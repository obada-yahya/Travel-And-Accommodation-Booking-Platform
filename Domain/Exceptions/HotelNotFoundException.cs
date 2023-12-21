namespace Domain.Exceptions;

public class HotelNotFoundException : Exception
{
    public HotelNotFoundException(Guid id)
        : base($"The hotel with the ID = {id} was not found")
    {
        
    }
}