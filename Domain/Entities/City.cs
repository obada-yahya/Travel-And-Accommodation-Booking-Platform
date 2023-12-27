namespace Domain.Entities;

public class City : Entity
{
    public string Name { get; set; }
    public string CountryName { get; set; }
    public string CountryCode { get; set; }
    public string PostOffice { get; set; }
    public IList<Hotel> Hotels { get; set; }
}