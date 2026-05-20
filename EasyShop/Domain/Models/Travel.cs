using EasyShop.Domain.Primitives;

namespace EasyShop.Domain.Models;

public class Travel : BaseEntity
{
    public CurrencyCode CostCurrencyCode { get; set; } = CurrencyCode.USD;
    public CurrencyCode PriceCurrencyCode { get; set; } = CurrencyCode.CUP;
    public double Multiplicator { get; set; } = 1.0;
    public Money ChangeValue { get; set; } = new Money(500m, CurrencyCode.CUP);
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    // Navigation
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();
}
