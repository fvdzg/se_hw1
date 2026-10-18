namespace VyshKat.Domain.Entities;

using VyshKat.Domain.interfaces;

public abstract class Vehicle : IInventoryItem, IEnergyConsumer
{
    public string InventoryNumber { get; }
    public string Name { get; }
    public double DailyEnergyKwh { get; }
    public int BeginnerFriendliness { get; }

    protected Vehicle(string inventoryNumber, string name, double dailyEnergyKwh, int beginnerFriendLines)
    {
        if (string.IsNullOrWhiteSpace(inventoryNumber))
        {
            throw new ArgumentException("Inventory number is needed", nameof(inventoryNumber));
        }
        if ((beginnerFriendLines < 1) || (beginnerFriendLines > 10))
        {
            throw new ArgumentOutOfRangeException(nameof(beginnerFriendLines), "must be 1...10");
        }
        InventoryNumber = inventoryNumber;
        Name = name;
        DailyEnergyKwh = dailyEnergyKwh;
        BeginnerFriendliness = beginnerFriendLines;
    }
    public bool IsSuitableForBeginners => BeginnerFriendliness >= 6;
}