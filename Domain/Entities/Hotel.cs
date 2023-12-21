namespace Domain.Entities;

public class Hotel
{
    public Hotel(Guid id, Guid cityId, Guid ownerId, string name, float rating, string streetAddress, string description, string phoneNumber, int floorsNumber)
    {
        Id = id;
        CityId = cityId;
        OwnerId = ownerId;
        Name = name;
        Rating = rating;
        StreetAddress = streetAddress;
        Description = description;
        PhoneNumber = phoneNumber;
        FloorsNumber = floorsNumber;
    }

    public Guid Id { get; private set; }
    public Guid CityId { get; private set; }
    public Guid OwnerId { get; private set; }
    public string Name { get; private set; }
    public float Rating { get; private set; }
    public string StreetAddress { get; private set; }
    public string Description { get; private set; }
    public string PhoneNumber { get; private set; }
    public int FloorsNumber { get; private set; }
    
}