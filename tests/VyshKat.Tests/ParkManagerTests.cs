namespace VyshKat.Tests;

using Moq;
using VyshKat.Domain.Entities;
using VyshKat.Domain.interfaces;
using VyshKat.services;
using Xunit;
public class ParkManagerTests

{
    [Fact]
    public void ServiceCenterAllows_ItemAddToRegistry()
    {
        var registry = new ParkRegistry();
        var serviceCenter = StubServiceCenter.AlwaysAccept();
        var manager = new ParkManager(registry, serviceCenter);
        var scooter = new Scooter("HSE_S999", "BBB", 0.5, 8);
        var accept = manager.TryAccept(scooter, out var reason);
        Assert.True(accept);
        Assert.Equal("accepted", reason);
        Assert.Single(registry.GetAll());
    }
    [Fact]
    public void ServiceCenterRejects_ItemNotAddToRegistry()
    {
        var registry = new ParkRegistry();
        var serviceCenter = StubServiceCenter.AlwaysReject();
        var manager = new ParkManager(registry, serviceCenter);
        var scooter = new Scooter("HSE_S999", "BBB", 0.5, 8);
        var accept = manager.TryAccept(scooter, out var reason);
        Assert.False(accept);
        Assert.Equal("rejected by service center", reason);
        Assert.Empty(registry.GetAll());
    }
    [Fact]
    public void DelegateDecisionToServiceCenter()
    {
        var registry = new ParkRegistry();
        var mock = new Mock<IServiceCenter>();
        mock.Setup(s => s.Inspect(It.IsAny<IInventoryItem>())).Returns(true);
        var manager = new ParkManager(registry, mock.Object);
        var helmet = new Helmet("HSE_S9939", "hhhh");
        manager.TryAccept(helmet, out _);
    }
    [Fact]
    public void AccessoryWithoutEnergy()
    {
        var registry = new ParkRegistry();
        var manager = new ParkManager(registry, StubServiceCenter.AlwaysAccept());
        var helmet = new Helmet("HSE_S99139", "hhhh1");
        var accept = manager.TryAccept(helmet, out _);
        Assert.True(accept);
        Assert.Single(registry.GetAll());
    }
}
