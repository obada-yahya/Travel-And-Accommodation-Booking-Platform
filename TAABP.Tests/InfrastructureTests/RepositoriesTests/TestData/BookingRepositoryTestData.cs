using Domain.Entities;

namespace TAABP.Tests.InfrastructureTests.RepositoriesTests.TestData;

public class BookingRepositoryTestData
{
    public static IEnumerable<object[]> BookingRepositoryValidTestData
    {
        get
        {
            yield return new object[] { new Booking
            {
                Id = Guid.NewGuid(),
                Payment = null,
                Review = null,
                BookingDate = DateTime.Today,
                CheckInDate = DateTime.Today.AddDays(3),
                CheckOutDate = DateTime.Today.AddDays(5),
                GuestId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
            }};
            yield return new object[] { new Booking
            {
                Id = Guid.NewGuid(),
                Payment = null,
                Review = null,
                BookingDate = DateTime.Today,
                CheckInDate = DateTime.Today.AddDays(3),
                CheckOutDate = DateTime.Today.AddDays(5),
                GuestId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
            }};
            yield return new object[] { new Booking
            {
                Id = Guid.NewGuid(),
                Payment = null,
                Review = null,
                BookingDate = DateTime.Today,
                CheckInDate = DateTime.Today.AddDays(15),
                CheckOutDate = DateTime.Today.AddDays(22),
                GuestId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
            }};
            yield return new object[] { new Booking
            {
                Id = Guid.NewGuid(),
                Payment = null,
                Review = null,
                BookingDate = DateTime.Today,
                CheckInDate = DateTime.Today.AddDays(20),
                CheckOutDate = DateTime.Today.AddDays(23),
                GuestId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
            }};
            yield return new object[] { new Booking
            {
                Id = Guid.NewGuid(),
                Payment = null,
                Review = null,
                BookingDate = DateTime.Today,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(2),
                GuestId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
            }};
        }
    }
    
    public static IEnumerable<object[]> BookingsTestData()
    {
        yield return new object[]
        {
            new List<Booking>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Payment = null,
                    Review = null,
                    BookingDate = DateTime.Today,
                    CheckInDate = DateTime.Today.AddDays(3),
                    CheckOutDate = DateTime.Today.AddDays(5),
                    GuestId = Guid.NewGuid(),
                    RoomId = Guid.NewGuid(),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Payment = null,
                    Review = null,
                    BookingDate = DateTime.Today,
                    CheckInDate = DateTime.Today.AddDays(6),
                    CheckOutDate = DateTime.Today.AddDays(7),
                    GuestId = Guid.NewGuid(),
                    RoomId = Guid.NewGuid(),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Payment = null,
                    Review = null,
                    BookingDate = DateTime.Today,
                    CheckInDate = DateTime.Today.AddDays(10),
                    CheckOutDate = DateTime.Today.AddDays(15),
                    GuestId = Guid.NewGuid(),
                    RoomId = Guid.NewGuid(),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Payment = null,
                    Review = null,
                    BookingDate = DateTime.Today,
                    CheckInDate = DateTime.Today.AddDays(5),
                    CheckOutDate = DateTime.Today.AddDays(25),
                    GuestId = Guid.NewGuid(),
                    RoomId = Guid.NewGuid(),
                },
                
            }
        };
    }
}