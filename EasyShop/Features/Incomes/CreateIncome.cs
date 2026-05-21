using EasyShop.Data;
using EasyShop.Domain.Models;
using EasyShop.Domain.Primitives;
using EasyShop.Features.Commons;

namespace EasyShop.Features.Incomes;

public sealed record CreateIncomeRequest(
    Guid TravelId,
    string Description,
    MoneyRequest Value
);

public sealed class CreateIncomeHandler
{
    private readonly ApplicationDbContext _db;
    public CreateIncomeHandler(ApplicationDbContext db) => _db = db;

    public async Task<Guid> HandleAsync(CreateIncomeRequest request, CancellationToken ct = default)
    {
        var income = new Income
        {
            TravelId = request.TravelId,
            Description = request.Description,
            Value = new Money(request.Value.Amount, request.Value.Currency)
        };

        _db.Incomes.Add(income);
        await _db.SaveChangesAsync(ct);

        return income.Id;
    }
}
