namespace VyshKat.Domain.Entities;

public sealed class ElectricBicycle : Vehicle
{
    public ElectricBicycle(string inventoryNumber, string name, double dailyEnergyKwh, int beginnerFrienliness)
    : base(inventoryNumber, name, dailyEnergyKwh, beginnerFrienliness) {}
}