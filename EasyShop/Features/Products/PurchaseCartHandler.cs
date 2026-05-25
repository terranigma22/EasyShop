using EasyShop.Data;
using EasyShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyShop.Features.Products;

public sealed record PurchaseCartRequest(Guid TravelId);

public sealed class PurchaseCartHandler
{
    private readonly ApplicationDbContext _db;
    public PurchaseCartHandler(ApplicationDbContext db) => _db = db;

    public async Task HandleAsync(PurchaseCartRequest request, CancellationToken ct = default)
    {
        var products = await _db.Products
            .Where(p => p.TravelId == request.TravelId && p.Status == ProductStatus.InCart)
            .ToListAsync(ct);

        foreach (var product in products)
        {
            product.Status = ProductStatus.Purchased;
        }

        await _db.SaveChangesAsync(ct);
    }
}
