using EasyShop.Domain.Models;
using EasyShop.Features.Travels;

namespace EasyShop.Services;

public sealed class TravelStateService
{
    public Travel? CurrentTravel { get; private set; }
    public event Action? OnTravelChanged;

    private readonly GetTravelHandler _getTravelHandler;
    private readonly GetTravelsHandler _getTravelsHandler;

    public TravelStateService(GetTravelHandler getTravelHandler, GetTravelsHandler getTravelsHandler)
    {
        _getTravelHandler = getTravelHandler;
        _getTravelsHandler = getTravelsHandler;
    }

    public async Task LoadLastTravelAsync()
    {
        var travels = await _getTravelsHandler.HandleAsync(new GetTravelsRequest());
        var last = travels.FirstOrDefault();
        if (last != null)
            await SetCurrentTravelAsync(last.Id);
    }

    public async Task SetCurrentTravelAsync(Guid travelId)
    {
        CurrentTravel = await _getTravelHandler.HandleAsync(new GetTravelRequest(travelId));
        OnTravelChanged?.Invoke();
    }

    public void SetCurrentTravel(Travel travel)
    {
        CurrentTravel = travel;
        OnTravelChanged?.Invoke();
    }

    public void Clear()
    {
        CurrentTravel = null;
        OnTravelChanged?.Invoke();
    }
}
