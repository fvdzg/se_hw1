namespace VyshKat.services;

using VyshKat.Domain.Entities;
using VyshKat.Domain.interfaces;

public sealed class ReportService
{
    private readonly IParkRegistry _registry;
    public ReportService(IParkRegistry registry) => _registry = registry;
    public int TotalUnitsCount => _registry.GetAll().Count;
    public double TotalDailyEnergy()
    {
        return _registry.GetAll().OfType<IEnergyConsumer>().Sum(c => c.DailyEnergyKwh);
    }
    public IReadOnlyList<Vehicle> BeginnerFriendlyVehicles()
    {
        return _registry.GetAll().OfType<Vehicle>().Where(v => v.IsSuitableForBeginners).ToList();
    }
    public IReadOnlyList<IInventoryItem> AllInventory() => _registry.GetAll();
}
