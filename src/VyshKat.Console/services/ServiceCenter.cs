namespace VyshKat.services;

using VyshKat.Domain.interfaces;

public sealed class ServiceCenter : IServiceCenter
{
    public const double MaxDailyEnergyKwh = 5.0;
    public bool Inspect(IInventoryItem item)
    {
        if (item == null)
        {
            return false;
        }
        if(string.IsNullOrWhiteSpace(item.InventoryNumber))
        {
            return false;
        }
        if (!item.InventoryNumber.StartsWith("HSE-", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        if (item is IEnergyConsumer consumer && consumer.DailyEnergyKwh > MaxDailyEnergyKwh)
        {
            return false;
        }
        return true;
    }
}