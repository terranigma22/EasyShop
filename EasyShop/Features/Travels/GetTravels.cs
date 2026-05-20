using EasyShop.Data;
using EasyShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyShop.Features.Travels;

public sealed record GetTravelsRequest;

public sealed class GetTravelsHandler
{
    private readonly ApplicationDbContext _db;
    public GetTravelsHandler(ApplicationDbContext db) => _db = db;

    public async Task<List<Travel>> HandleAsync(GetTravelsRequest request, CancellationToken ct = default)
    {
        return await _db.Travels
            .OrderByDescending(t => t.StartDate)
            .ToListAsync(ct);
    }
}
