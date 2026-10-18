namespace VyshKat.Domain.Entities;

using VyshKat.Domain.interfaces;

public abstract class Accessory : IInventoryItem
{
    public string InventoryNumber { get; }
    public string Name { get; }

    protected Accessory(string inventoryNumber, string name)
    {
        if (string.IsNullOrWhiteSpace(inventoryNumber))
        {
            throw new ArgumentException("Inventory number is needed", nameof(inventoryNumber));
        }
        InventoryNumber = inventoryNumber;
        Name = name;
    }
}