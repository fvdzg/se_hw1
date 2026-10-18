namespace VyshKat.services;

using VyshKat.Domain.interfaces;

public sealed class ParkRegistry : IParkRegistry
{
    private readonly List<IInventoryItem> _items = new();
    public void Add(IInventoryItem item)
    {
        _items.Add(item);
    }
    public IReadOnlyList<IInventoryItem> GetAll()
    {
        return _items.AsReadOnly();
    }
}