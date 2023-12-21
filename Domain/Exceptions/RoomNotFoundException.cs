namespace Domain.Exceptions;

public class RoomNotFoundException : Exception
{
    public RoomNotFoundException(Guid id):
        base($"The room with the ID = {id} was not found")
    {
        
    }
}