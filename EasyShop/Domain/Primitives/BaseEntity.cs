namespace EasyShop.Domain.Primitives;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
}
