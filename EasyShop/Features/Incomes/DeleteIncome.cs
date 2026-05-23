using EasyShop.Data;

namespace EasyShop.Features.Incomes;

public sealed record DeleteIncomeRequest(Guid Id);

public sealed class DeleteIncomeHandler
{
    private readonly ApplicationDbContext _db;
    public DeleteIncomeHandler(ApplicationDbContext db) => _db = db;

    public async Task HandleAsync(DeleteIncomeRequest request, CancellationToken ct = default)
    {
        var income = await _db.Incomes.FindAsync(request.Id, ct);
        if (income is null) return;
        _db.Incomes.Remove(income);
        await _db.SaveChangesAsync(ct);
    }
}
