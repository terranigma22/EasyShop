using EasyShop.Data;
using EasyShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyShop.Features.Travels;

public sealed record GetTravelRequest(Guid Id);

public sealed class GetTravelHandler
{
    private readonly ApplicationDbContext _db;
    public GetTravelHandler(ApplicationDbContext db) => _db = db;

    public async Task<Travel?> HandleAsync(GetTravelRequest request, CancellationToken ct = default)
    {
        return await _db.Travels
            .Include(t => t.Products)
            .Include(t => t.Expenses)
            .Include(t => t.Incomes)
            .FirstOrDefaultAsync(t => t.Id == request.Id, ct);
    }
}
