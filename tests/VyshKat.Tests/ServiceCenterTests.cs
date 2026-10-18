namespace VyshKat.Tests;

using Moq;
using VyshKat.Domain.Entities;
using VyshKat.Domain.interfaces;
using VyshKat.services;
using Xunit;
public class ServiceCenterTests

{
    private readonly ServiceCenter _serviceCenter = new();
    [Fact]
    public void NullItem_Reject()
    {
        var result = _serviceCenter.Inspect(null!);
        Assert.False(result);
    }
    [Fact]
    public void EmptyInventoryNumber_Reject()
    {
        var mockItem = new Mock<IInventoryItem>();
        mockItem.Setup(i => i.InventoryNumber).Returns("");
        mockItem.Setup(i => i.Name).Returns("NoNumber");
        var result = _serviceCenter.Inspect(mockItem.Object);
        Assert.False(result);
    }
    [Fact]
    public void WrongPrefix_Reject()
    {
        var scooter = new Scooter("AAAAAAAAAAA", "SAD SCOOTER", 0.5, 7);
        var result = _serviceCenter.Inspect(scooter);
        Assert.False(result);
    }
    [Fact]
    public void OkScooter_Accepted()
    {
        var scooter = new Scooter("HSE-S8888", "HAPPY SCOOTER", 0.5, 7);
        var result = _serviceCenter.Inspect(scooter);
        Assert.True(result);
    }
    [Fact]
    public void WithoutEnergy_Accepted()
    {
        var helmet = new Helmet("HSE-S88", "HAPPY helmet");
        var result = _serviceCenter.Inspect(helmet);
        Assert.True(result);
    }
    [Fact]
    public void OverLimitEnergy_Reject()
    {
        var scooter = new Scooter("HSE-1111", "SAD SCOOTER 2.0", 9.9, 5);
        var result = _serviceCenter.Inspect(scooter);
        Assert.False(result);
    }
    [Fact]
    public void AtLimitEnergy_Accepted()
    {
        var dock = new DockStation("HSE-S8888", "doooooock", 5.0);
        var result = _serviceCenter.Inspect(dock);
        Assert.True(result);
    }


}
