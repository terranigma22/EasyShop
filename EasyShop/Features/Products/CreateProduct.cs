using EasyShop.Data;
using EasyShop.Domain.Models;
using EasyShop.Domain.Primitives;
using EasyShop.Features.Commons;

namespace EasyShop.Features.Products;

public sealed record CreateProductRequest(
    Guid TravelId,
    int Units,
    double Multiplicator,
    MoneyRequest TotalCost,
    MoneyRequest UnitPrice
);

public sealed class CreateProductHandler
{
    private readonly ApplicationDbContext _db;
    public CreateProductHandler(ApplicationDbContext db) => _db = db;

    public async Task<Guid> HandleAsync(CreateProductRequest request, CancellationToken ct = default)
    {
        var product = Product.New(
            request.TravelId,
            request.Units,
            request.Multiplicator,
            new Money(request.TotalCost.Amount, request.TotalCost.Currency),
            new Money(request.UnitPrice.Amount, request.UnitPrice.Currency)
        );

        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);

        return product.Id;
    }
}
