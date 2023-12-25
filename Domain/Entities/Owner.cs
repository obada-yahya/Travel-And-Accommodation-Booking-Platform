namespace Domain.Entities;

public class Owner : Person
{
    public IList<Hotel> Hotels { get; set; }
}