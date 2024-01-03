using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Common.Persistence.Seeding;

public class PaymentSeeding
{
    public static IEnumerable<Payment> SeedData()
    {
        return new List<Payment>
        {
            new Payment
            {
                Id = new Guid("7f5cc9f0-796f-498d-9f3f-9e5249a4f6ae"),
                BookingId = new Guid("0bf4a177-98b8-4f67-8a56-95669c320890"),
                Method = PaymentMethod.CreditCard,
                Status = PaymentStatus.Completed,
                Amount = 1500.0
            },
            new Payment
            {
                Id = new Guid("1c8d70bd-2534-4991-bddf-84c7edee1a79"),
                BookingId =new Guid("efeb3d13-3dab-46c9-aa9a-9f22dd58e06e"),
                Method = PaymentMethod.Cash,
                Status = PaymentStatus.Pending,
                Amount = 1200.0
            },
            new Payment
            {
                Id = new Guid("8f974636-4f53-48d9-af99-2f7f1d3e0474"),
                BookingId = new Guid("7d3155a2-95f8-4d9b-bc24-662ae053f1c9"),
                Method = PaymentMethod.MobileWallet,
                Status = PaymentStatus.Completed,
                Amount = 2000.0
            }
        };
    }
}