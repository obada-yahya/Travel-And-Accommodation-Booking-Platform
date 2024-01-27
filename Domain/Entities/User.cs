using Domain.Enums;

namespace Domain.Entities;

public class User : Person
{
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public UserRole Role { get; set; }
    public IList<Booking> Bookings { get; set; }
}