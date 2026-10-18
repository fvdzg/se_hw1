namespace VyshKat.Domain.Entities;

public sealed class Scooter : Vehicle
{
    public Scooter(string inventoryNumber, string name, double dailyEnergyKwh, int beginnerFrienliness)
    : base(inventoryNumber, name, dailyEnergyKwh, beginnerFrienliness) {}
}