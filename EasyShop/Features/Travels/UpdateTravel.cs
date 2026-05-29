using EasyShop.Data;
using EasyShop.Domain.Commons;
using EasyShop.Domain.Models;
using EasyShop.Domain.Primitives;
using EasyShop.Features.Commons;
using EasyShop.Features.Travels.Commons;
using Microsoft.EntityFrameworkCore;

namespace EasyShop.Features.Travels;

public sealed record UpdateTravelRequest(
    Guid Id,
    DateOnly StartDate,
    DateOnly EndDate,
    double Multiplicator,
    MoneyRequest ChangeValue,
    MoneyRequest MoneyToTravel,
    bool RecalculatePrices = false
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

        if (request.RecalculatePrices)
        {
            var changeAmount = request.ChangeValue.Amount <= 0 ? 1.0m : (decimal)request.ChangeValue.Amount;
            var changeCurrency = request.ChangeValue.Currency;

            var products = await _db.Products
                .Where(p => p.TravelId == request.Id)
                .ToListAsync(ct);

            foreach (var product in products)
            {
                product.UnitChangePrice = new Money(
                    product.UnitPrice.Amount * changeAmount,
                    changeCurrency
                );
                product.UpdatedAt = DomainHelpers.Now;
            }
        }

        await _db.SaveChangesAsync(ct);
    }
}
