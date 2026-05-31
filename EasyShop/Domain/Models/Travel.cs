using EasyShop.Domain.Primitives;

namespace EasyShop.Domain.Models;

public class Travel : BaseEntity
{
    public Money MoneyToTravel { get; set; } = new Money(0m, CurrencyCode.USD);
    public double Multiplicator { get; set; } = 1.0;
    public Money ChangeValue { get; set; } = new Money(500m, CurrencyCode.CUP);
    public Money ChangeValueToBuy { get; set; } = new Money(500m, CurrencyCode.CUP);
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    // Navigation
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();

    public decimal GetExpensesTotal()
        => Expenses.Sum(e => e.Value.Amount);

    public decimal GetProductsTotal()
        => Products.Where(p => p.Status == ProductStatus.Purchased).Sum(p => p.TotalCost.Amount);

    public decimal GetIncomesTotal()
        => Incomes.Sum(i => i.Value.Amount);
    public decimal GetNetTotal()
        => GetIncomesTotal() - GetExpensesTotal() - GetProductsTotal();

    public decimal GetRemaining()
        => MoneyToTravel.Amount - GetExpensesTotal() - GetProductsTotal() + GetIncomesTotal();

    public decimal GetProfitInTravelCurrency()
    {
        var totalCosts = GetProductsTotal() + GetExpensesTotal();

        if (ChangeValueToBuy.Currency != MoneyToTravel.Currency && ChangeValueToBuy.Amount > 0)
            return (GetTotalChangePrices() / ChangeValueToBuy.Amount) - totalCosts;

        return GetTotalPrices() - totalCosts;
    }

    public decimal GetTotalPrices()
        => Products.Where(p => p.Status == ProductStatus.Purchased).Sum(p => p.GetTotalPrice());

    public decimal GetTotalChangePrices()
        => Products.Where(p => p.Status == ProductStatus.Purchased).Sum(p => p.GetTotalChangePrice());
}
