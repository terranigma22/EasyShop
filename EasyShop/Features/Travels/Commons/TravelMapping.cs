using EasyShop.Domain.Commons;
using EasyShop.Domain.Models;
using EasyShop.Domain.Primitives;

namespace EasyShop.Features.Travels.Commons;

internal static class TravelMapping
{
    //public static ProductDto ToDto(this Product entity)
    //{
    //    return new ProductDto
    //    {
    //        Id = entity.Id,
    //        Name = entity.Name,
    //        Description = entity.Description,
    //        CategoryId = entity.CategoryId,
    //        CategoryName = entity.Category?.Code,
    //        CostPrice = entity.CostPrice,
    //        BasePrice = entity.BasePrice,
    //        QuantityInStock = entity.QuantityInStock,
    //        IsDeleted = entity.IsDeleted
    //    };
    //}

    public static Travel ToEntity(this CreateTravelRequest request)
    {
        return new Travel
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Multiplicator = request.Multiplicator <= 0 ? 1.0 : request.Multiplicator,
            ChangeValue = new Money(request.ChangeValue.Amount, request.ChangeValue.Currency),
            MoneyToTravel = new Money(request.MoneyToTravel.Amount, request.MoneyToTravel.Currency)
        };
    }

    public static Travel Update(this Travel entity, UpdateTravelRequest request)
    {
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.Multiplicator = request.Multiplicator <= 0 ? 1.0 : request.Multiplicator;
        entity.ChangeValue = new Money(request.ChangeValue.Amount, request.ChangeValue.Currency);
        entity.MoneyToTravel = new Money(request.MoneyToTravel.Amount, request.MoneyToTravel.Currency);

        entity.UpdatedAt = DomainHelpers.Now;

        return entity;
    }
}
