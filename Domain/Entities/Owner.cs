namespace Domain.Entities;

public class Owner
{
    public Owner(Guid id, string firstName, string lastName, string email, string phoneNumber, IList<Hotel> hotels)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Hotels = hotels;
    }
    
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public IList<Hotel> Hotels { get; private set; }
    
}