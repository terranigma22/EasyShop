using EasyShop.Domain.Commons;

namespace EasyShop.Domain.Primitives;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public DateOnly UpdatedAt { get; set; } = DomainHelpers.Now;
}
