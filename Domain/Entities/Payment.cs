using Domain.Enums;

namespace Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid BookingId { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }
    public double Amount { get; private set; }
}