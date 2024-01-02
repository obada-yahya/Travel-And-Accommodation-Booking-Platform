using Domain.Entities;

namespace TAABP.Tests.InfrastructureTests.RepositoriesTests.TestData;

public class GuestRepositoryTestData
{
    public static IEnumerable<object[]> GuestRepositoryValidTestData
    {
        get
        {
            yield return new object[] {new Guest()
            {
                Id = Guid.Parse("d62a25b1-95c4-4c3a-8fc3-352b7f3991f5"),
                FirstName = "Obada",
                LastName = "Yahya",
                Email = "obadayahya.an@gmail.com",
                PhoneNumber = "0592341234",
                Bookings = new List<Booking>()
                {
                    new Booking
                    {
                        Id = Guid.NewGuid(),
                        Payment = null,
                        Review = null,
                        BookingDate = DateTime.Today,
                        GuestId = Guid.Parse("d62a25b1-95c4-4c3a-8fc3-352b7f3991f5"),
                        RoomId = Guid.Parse("d62a25b1-95c4-4c3a-8fc3-352b7f3991f5"),
                        CheckInDate = DateTime.Now.AddDays(5),
                        CheckOutDate = DateTime.Now.AddDays(10)
                    }
                }
            }};
            
            yield return new object[] {new Guest()
            {
                Id = Guid.Parse("d62a25b1-95c4-4c3a-8fc3-352b7f3991f5"),
                FirstName = "Obada",
                LastName = "Yahya",
                Email = "obadayahya.an@gmail.com",
                PhoneNumber = "0592341234",
                Bookings = new List<Booking>()
                {
                    new Booking
                    {
                        Id = Guid.NewGuid(),
                        Payment = null,
                        Review = null,
                        BookingDate = DateTime.Today,
                        GuestId = Guid.Parse("d62a25b1-95c4-4c3a-8fc3-352b7f3991f5"),
                        RoomId = Guid.Parse("d62a25b1-95c4-4c3a-8fc3-352b7f3991f5"),
                        CheckInDate = DateTime.Now.AddDays(5),
                        CheckOutDate = DateTime.Now.AddDays(10)
                    },
                    new Booking
                    {
                        Id = Guid.NewGuid(),
                        Payment = null,
                        Review = null,
                        BookingDate = DateTime.Today,
                        GuestId = Guid.Parse("d62a25b1-95c4-4c3a-8fc3-352b7f3991f5"),
                        RoomId = Guid.Parse("d62a25b1-95c4-4c3a-8fc3-352b7f3991f5"),
                        CheckInDate = DateTime.Now.AddDays(15),
                        CheckOutDate = DateTime.Now.AddDays(20)
                    }
                }
            }};
        }
    }
    
    public static IEnumerable<object[]> GuestsTestData()
    {
        yield return new object[]
        {
            new List<Guest>
            {
                new Guest{ Id = Guid.NewGuid(), FirstName = "Obada", LastName = "Yahya", Email = "obadayahya.an@gmail.com", PhoneNumber = "+9700597853218"},
                new Guest{ Id = Guid.NewGuid(), FirstName = "Abed", LastName = "Yahya", Email = "abedyahya.an@gmail.com", PhoneNumber = "+9700597853218"},
                new Guest{ Id = Guid.NewGuid(), FirstName = "Sameh", LastName = "Yahya", Email = "tahayahya.an@gmail.com", PhoneNumber = "+9700597853218"},
                new Guest{ Id = Guid.NewGuid(), FirstName = "Taha", LastName = "Yahya", Email = "samehyahya.an@gmail.com", PhoneNumber = "+9700597853218"},
            }
        };
    }
}