using EasyShop.Data;

namespace EasyShop.Features.Products;

public sealed record DeleteProductRequest(Guid Id);

public sealed class DeleteProductHandler
{
    private readonly ApplicationDbContext _db;
    public DeleteProductHandler(ApplicationDbContext db) => _db = db;

    public async Task HandleAsync(DeleteProductRequest request, CancellationToken ct = default)
    {
        var product = await _db.Products.FindAsync(request.Id, ct);
        if (product is null) return;
        _db.Products.Remove(product);
        await _db.SaveChangesAsync(ct);
    }
}
