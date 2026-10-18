namespace VyshKat.Tests;

using VyshKat.Domain.interfaces;

public sealed class StubServiceCenter : IServiceCenter
{
    private readonly Func<IInventoryItem, bool> _decision;
    public StubServiceCenter(Func<IInventoryItem, bool> decision)
    {
        _decision = decision;
    }
    public bool Inspect(IInventoryItem item) => _decision(item);
    public static StubServiceCenter AlwaysAccept() => new(_ => true);
    public static StubServiceCenter AlwaysReject() => new(_ => false);
}