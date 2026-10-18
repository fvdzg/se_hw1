namespace VyshKat.Domain.Entities;

public sealed class Bicycle : Vehicle
{
    public Bicycle(string inventoryNumber, string name, int beginnerFrienliness)
    : base(inventoryNumber, name, 0.0, beginnerFrienliness) {}
}