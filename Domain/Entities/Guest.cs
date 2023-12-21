namespace Domain.Entities;

public class Guest
{
    public Guest(Guid id, string firstName, string lastName, string email, string phoneNumber, IList<Booking> bookings)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Bookings = bookings;
    }

    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public IList<Booking> Bookings { get; private set; }
}