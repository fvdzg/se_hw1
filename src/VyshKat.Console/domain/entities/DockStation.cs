using VyshKat.Domain.interfaces;

namespace VyshKat.Domain.Entities;

public sealed class DockStation : Accessory, IEnergyConsumer
{
    public double DailyEnergyKwh { get; }
    public DockStation(string inventoryNumber, string name, double  dailyEnergyKwh)
    : base(inventoryNumber, name)
    {
        DailyEnergyKwh = dailyEnergyKwh;
    }
}