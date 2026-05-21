using EasyShop.Data;
using EasyShop.Domain.Primitives;
using EasyShop.Features.Commons;
using EasyShop.Features.Travels.Commons;

namespace EasyShop.Features.Travels;

public sealed record CreateTravelRequest(
    DateOnly StartDate,
    DateOnly EndDate,
    double Multiplicator,
    MoneyRequest ChangeValue,
    MoneyRequest MoneyToTravel
);

public sealed class CreateTravelHandler
{
    private readonly ApplicationDbContext _db;
    public CreateTravelHandler(ApplicationDbContext db) => _db = db;

    public async Task<Guid> HandleAsync(CreateTravelRequest request, CancellationToken ct = default)
    {
        var travel = request.ToEntity();

        _db.Travels.Add(travel);

        await _db.SaveChangesAsync(ct);

        return travel.Id;
    }
}
