namespace Domain.Entities;

public class City
{
    public City(Guid id, string name, string countryName, string countryCode, string postOffice, IList<Hotel> hotels)
    {
        Id = id;
        Name = name;
        CountryName = countryName;
        CountryCode = countryCode;
        PostOffice = postOffice;
        Hotels = hotels;
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string CountryName { get; private set; }
    public string CountryCode { get; private set; }
    public string PostOffice { get; private set; }
    public IList<Hotel> Hotels { get; private set; }
}