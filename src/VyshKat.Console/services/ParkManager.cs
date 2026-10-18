namespace VyshKat.services;

using VyshKat.Domain.interfaces;

public sealed class ParkManager
{
    private readonly IParkRegistry _registry;
    private readonly IServiceCenter _serviceCenter;
    public ParkManager(IParkRegistry registry, IServiceCenter serviceCenter)
    {
        _registry = registry;
        _serviceCenter = serviceCenter;
    }
    public bool TryAccept(IInventoryItem item, out string reason)
    {
        if (_serviceCenter.Inspect(item))
        {
            _registry.Add(item);
            reason = "accepted";
            return true;
        }
        reason = "rejected by service center";
        return false;
    }
}
