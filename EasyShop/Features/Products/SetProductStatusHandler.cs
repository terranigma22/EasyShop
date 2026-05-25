using EasyShop.Data;
using EasyShop.Domain.Models;

namespace EasyShop.Features.Products;

public sealed record SetProductStatusRequest(Guid ProductId, ProductStatus Status);

public sealed class SetProductStatusHandler
{
    private readonly ApplicationDbContext _db;
    public SetProductStatusHandler(ApplicationDbContext db) => _db = db;

    public async Task HandleAsync(SetProductStatusRequest request, CancellationToken ct = default)
    {
        var product = await _db.Products.FindAsync(new object[] { request.ProductId }, ct);
        if (product is null) return;

        product.Status = request.Status;
        await _db.SaveChangesAsync(ct);
    }
}
