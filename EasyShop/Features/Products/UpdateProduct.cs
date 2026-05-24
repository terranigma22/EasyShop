using EasyShop.Data;
using EasyShop.Domain.Commons;
using EasyShop.Domain.Models;
using EasyShop.Domain.Primitives;
using EasyShop.Features.Commons;

namespace EasyShop.Features.Products;

public sealed record UpdateProductRequest(
    Guid Id,
    string Name,
    int Units,
    double Multiplicator,
    MoneyRequest TotalCost,
    MoneyRequest UnitPrice,
    MoneyRequest UnitChangePrice,
    string? ImageDataUri
);

public sealed class UpdateProductHandler
{
    private readonly ApplicationDbContext _db;
    public UpdateProductHandler(ApplicationDbContext db) => _db = db;

    public async Task HandleAsync(UpdateProductRequest request, CancellationToken ct = default)
    {
        var product = await _db.Products.FindAsync([request.Id], ct);

        if (product is null)
            return;

        product.Name = request.Name;
        product.Units = request.Units;
        product.Multiplicator = request.Multiplicator;
        product.TotalCost = new Money(request.TotalCost.Amount, request.TotalCost.Currency);
        product.UnitPrice = new Money(request.UnitPrice.Amount, request.UnitPrice.Currency);
        product.UnitChangePrice = new Money(request.UnitChangePrice.Amount, request.UnitChangePrice.Currency);
        product.ImageDataUri = request.ImageDataUri;
        product.UpdatedAt = DomainHelpers.Now;

        await _db.SaveChangesAsync(ct);
    }
}
