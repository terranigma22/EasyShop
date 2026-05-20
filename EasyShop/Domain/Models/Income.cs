using EasyShop.Domain.Primitives;

namespace EasyShop.Domain.Models;

public class Income : BaseEntity
{
    public Guid TravelId { get; set; }
    public string Description { get; set; } = string.Empty;
    public Money Value { get; set; } = new Money(0m, CurrencyCode.CUP);
}
