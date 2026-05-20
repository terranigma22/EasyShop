using EasyShop.Domain.Primitives;

namespace EasyShop.Features.Commons;

public sealed record MoneyRequest(decimal Amount, CurrencyCode Currency);
