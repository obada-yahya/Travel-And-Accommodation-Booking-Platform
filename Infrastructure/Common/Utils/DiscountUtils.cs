using Domain.Entities;

namespace Infrastructure.Common.Utils;

public static class DiscountUtils
{
    public static float GetActiveDiscount(IEnumerable<Discount> roomType)
    {
        return roomType
            .FirstOrDefault(discount =>
                discount.FromDate.Date <= DateTime.Today.Date && 
                discount.ToDate.Date >= DateTime.Today.Date)
            ?.DiscountPercentage ?? 0.0f;
    }
}