namespace VyshKat.Domain.interfaces;

public interface IParkRegistry
{
    void Add(IInventoryItem item);
    IReadOnlyList<IInventoryItem> GetAll();
}