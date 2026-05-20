namespace EasyShop.Domain.Primitives;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}
