using EasyShop.Data;
using EasyShop.Domain.Primitives;
using EasyShop.Features.Commons;
using EasyShop.Features.Travels.Commons;

namespace EasyShop.Features.Travels;

public sealed record UpdateTravelRequest(
    Guid Id,
    DateOnly StartDate,
    DateOnly EndDate,
    CurrencyCode PriceCurrencyCode,
    double Multiplicator,
    MoneyRequest ChangeValue,
    MoneyRequest MoneyToTravel
);

public sealed class UpdateTravelHandler
{
    private readonly ApplicationDbContext _db;
    public UpdateTravelHandler(ApplicationDbContext db) => _db = db;

    public async Task HandleAsync(UpdateTravelRequest request, CancellationToken ct = default)
    {
        var travel = await _db.Travels.FindAsync(request.Id, ct);

        if (travel is null) 
            return;

        travel = travel.Update(request);

        await _db.SaveChangesAsync(ct);
    }
}
