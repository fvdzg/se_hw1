using VyshKat.Domain.interfaces;

namespace VyshKat.Domain.Entities;

public sealed class ChargingCabinet : Accessory, IEnergyConsumer
{
    public double DailyEnergyKwh { get; }
    public ChargingCabinet(string inventoryNumber, string name, double  dailyEnergyKwh)
    : base(inventoryNumber, name)
    {
        DailyEnergyKwh = dailyEnergyKwh;
    }
}