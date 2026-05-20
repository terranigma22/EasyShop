namespace EasyShop.Domain.Commons;

internal static class DomainHelpers
{
    public static DateOnly Now => DateOnly.FromDateTime(DateTime.Now);
}
