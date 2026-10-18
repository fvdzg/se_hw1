namespace VyshKat.Tests;

using VyshKat.Domain.Entities;
using VyshKat.services;
using Xunit;
public class ReportServiceTests
{
    private static (ParkRegistry Registry, ReportService Reports) BuildPArk()
    {
        var registry = new ParkRegistry();
        return (registry, new ReportService(registry));
    }
    [Fact]
    public void TotalDailyEnergy()
    {
        var (registry, reports) = BuildPArk();
        registry.Add(new Scooter("HSE-S991", "scooter a", 0.5, 8));
        registry.Add(new ElectricBicycle("HSE-E001", "nxgf", 1.25, 8));
        registry.Add(new Helmet("HSE-H5", "helmet 333"));
        registry.Add(new Bicycle("HSE-B991", "qgraar", 8));
        registry.Add(new DockStation("HSE-D991", "dock j", 5.0));
        Assert.Equal(6.75, reports.TotalDailyEnergy(), precision: 5);
        

    }
    [Fact]
    public void BeginnerFriendly_Rule6()
    {
        var (registry, reports) = BuildPArk();
        registry.Add(new Scooter("HSE-S991", "scooter 1", 0.5, 8));
        registry.Add(new Scooter("HSE-S992", "scooter 2", 0.5, 6));
        registry.Add(new Scooter("HSE-S993", "scooter 3", 0.5, 5));
        var result = reports.BeginnerFriendlyVehicles();
        Assert.Equal(2, result.Count);
        Assert.All(result, v => Assert.True(v.BeginnerFriendliness >= 6));
    }
    [Fact]
    public void BeginnerFriendl_WithNotVehicles()
    {
        var (registry, reports) = BuildPArk();
        registry.Add(new Helmet("HSE-H9091", "heeeeelmeeeeeeeet"));
        registry.Add(new DockStation("HSE-D123", "dddock", 2.0));
        registry.Add(new Scooter("HSE-S9973", "Scooter", 0.5, 8));
        var result = reports.BeginnerFriendlyVehicles();
        Assert.Single(result);
        Assert.Equal("Scooter", result[0].Name);
    }
    [Fact]
    public void TotalUnitsCount()
    {
        var (registry, reports) = BuildPArk();
        registry.Add(new Scooter("HSE-S777", "Scooter", 0.5, 8));
        registry.Add(new Helmet("HSE-H901", "heeeeelmeeeeeet"));
        registry.Add(new ChargingCabinet("HSE-C123", "Cabinet", 3.0));
        Assert.Equal(3, reports.TotalUnitsCount);
    }
     [Fact]
    public void AllInveentory()
    {
        var (registry, reports) = BuildPArk();
        registry.Add(new Bicycle("HSE-B111", "bike", 9));
        registry.Add(new DockStation("HSE-D221", "dock", 2.5));
        var result = reports.AllInventory();
        Assert.Equal(2, result.Count);
    }

}
