using EasyShop.Data;

namespace EasyShop.Features.Travels;

public sealed record DeleteTravelRequest(Guid Id);

public sealed class DeleteTravelHandler
{
    private readonly ApplicationDbContext _db;
    public DeleteTravelHandler(ApplicationDbContext db) => _db = db;

    public async Task HandleAsync(DeleteTravelRequest request, CancellationToken ct = default)
    {
        var travel = await _db.Travels.FindAsync(request.Id, ct);

        if (travel is null) 
            return;

        _db.Travels.Remove(travel);

        await _db.SaveChangesAsync(ct);
    }
}
