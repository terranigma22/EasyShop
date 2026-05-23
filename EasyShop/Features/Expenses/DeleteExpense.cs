using EasyShop.Data;

namespace EasyShop.Features.Expenses;

public sealed record DeleteExpenseRequest(Guid Id);

public sealed class DeleteExpenseHandler
{
    private readonly ApplicationDbContext _db;
    public DeleteExpenseHandler(ApplicationDbContext db) => _db = db;

    public async Task HandleAsync(DeleteExpenseRequest request, CancellationToken ct = default)
    {
        var expense = await _db.Expenses.FindAsync(request.Id, ct);
        if (expense is null) return;
        _db.Expenses.Remove(expense);
        await _db.SaveChangesAsync(ct);
    }
}
