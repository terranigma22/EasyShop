using EasyShop.Data;
using EasyShop.Domain.Models;
using EasyShop.Domain.Primitives;
using EasyShop.Features.Commons;

namespace EasyShop.Features.Expenses;

public sealed record CreateExpenseRequest(
    Guid TravelId,
    string Description,
    MoneyRequest Value
);

public sealed class CreateExpenseHandler
{
    private readonly ApplicationDbContext _db;
    public CreateExpenseHandler(ApplicationDbContext db) => _db = db;

    public async Task<Guid> HandleAsync(CreateExpenseRequest request, CancellationToken ct = default)
    {
        var expense = new Expense
        {
            TravelId = request.TravelId,
            Description = request.Description,
            Value = new Money(request.Value.Amount, request.Value.Currency)
        };

        _db.Expenses.Add(expense);
        await _db.SaveChangesAsync(ct);

        return expense.Id;
    }
}
