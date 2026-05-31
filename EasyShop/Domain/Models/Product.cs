using EasyShop.Domain.Primitives;

namespace EasyShop.Domain.Models;

public class Product : BaseEntity
{
    public Guid TravelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Units { get; set; } = 12;
    public double Multiplicator { get; set; } = 2.5;
    public Money TotalCost { get; set; } = new Money(0m, CurrencyCode.USD);
    public Money UnitPrice { get; set; } = new Money(0m, CurrencyCode.USD);
    public Money UnitChangePrice { get; set; } = new Money(0m, CurrencyCode.USD);
    public string? Description { get; set; }
    public string? ImageDataUri { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.InCart;

    public decimal GetUnitCost() 
        => Units == 0 ? 0 : TotalCost.Amount / Units;

    private Product() { }

    public static Product New(Guid travelId, string name, int units, double multiplicator, Money totalCost, Money unitPrice, Money unitChangePrice, ProductStatus status, string? imageDataUri = null, string? description = null)
    {
        return new Product
        {
            TravelId = travelId,
            Name = name,
            Units = units,
            Multiplicator = multiplicator,
            TotalCost = totalCost,
            UnitPrice = unitPrice,
            UnitChangePrice = unitChangePrice,
            ImageDataUri = imageDataUri,
            Description = description,
            Status = status
        };
    }

    public decimal GetProfit()
        => UnitPrice.Amount * Units - TotalCost.Amount;

    public decimal GetTotalPrice()
        => UnitPrice.Amount * Units;

    public decimal GetTotalChangePrice()
        => UnitChangePrice.Amount * Units;
}