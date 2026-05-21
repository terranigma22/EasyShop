using EasyShop.Domain.Primitives;

namespace EasyShop.Domain.Models;

public class Product : BaseEntity
{
    public Guid TravelId { get; set; }
    public int Units { get; set; } = 12;
    public double Multiplicator { get; set; } = 2.5;
    public Money TotalCost { get; set; } = new Money(0m, CurrencyCode.USD);
    public Money UnitPrice { get; set; } = new Money(0m, CurrencyCode.USD);
    

    public decimal GetUnitCost() 
        => Units == 0 ? 0 : TotalCost.Amount / Units;

    //public decimal GetUnitPrice(double multiplicator) 
    //    => GetUnitCost() * (decimal)multiplicator;

    //public decimal GetTotalPrice(double multiplicator) 
    //    => GetUnitPrice(multiplicator) * Units;
    private Product() { }

    public static Product New(Guid travelId, int units, double multiplicator, Money totalCost, Money unitPrice)
    {
        return new Product
        {
            TravelId = travelId,
            Units = units,
            Multiplicator = multiplicator,
            TotalCost = totalCost,
            UnitPrice = unitPrice
        };
    }

    public decimal GetProfit()
        => UnitPrice.Amount * Units - TotalCost.Amount;
}