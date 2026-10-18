namespace VyshKat.ui;

using VyshKat.Domain.Entities;
using VyshKat.services;
using System.Diagnostics.CodeAnalysis;
[ExcludeFromCodeCoverage]
public sealed class ConsoleDemo
{
    private readonly ParkManager _parkManager;
    private readonly ReportService _reports;
    private readonly IUserInterface _ui;
    public ConsoleDemo(ParkManager parkManager, ReportService reports, IUserInterface ui)
    {
        _parkManager = parkManager;
        _reports = reports;
        _ui = ui;
    }

    public void PrintAllReports()
    {
        _ui.WriteLine($"[report] total units: {_reports.TotalUnitsCount}");
        _ui.WriteLine($"[report] total daily energy: {_reports.TotalDailyEnergy():F2} kWh");
        _ui.WriteLine($"[report] beginner-friendly vehicles:");
        foreach (var v in _reports.BeginnerFriendlyVehicles())
        {
            _ui.WriteLine($"{v.Name} ({v.InventoryNumber}) friendliness: {v.BeginnerFriendliness}");
        }
        _ui.WriteLine("report all inventory:");
        foreach (var v in _reports.AllInventory())
        {
            _ui.WriteLine($"{v.Name, -20} ({v.InventoryNumber})");
        }
    }

    public void Run()
    {
        _ui.WriteLine("____demo____");
        var itemsDemo = new Domain.interfaces.IInventoryItem[]
        {
            new Scooter("HSE-S6767", "cool scooter 3000", 0.45, 8),
            new Scooter("HSE-S0123", "cool scooter 4000", 0.5, 3),
            new ElectricBicycle("HSE-E105", "eeeee", 0.6, 7),
            new Bicycle("HSE-B111111111111", "amazing bicycle", 9),
            new Helmet("HSE-H9999", "HELMET"),
            new DockStation("HSE-D66666", "dock dock", 3.5),
            new ChargingCabinet("HSE-C1", "CAAAABINET", 1.7),
            new Scooter("AAAAAAAA", "not cool scooter", 0.3, 7),
            new Scooter("HSE-S2222", "KWH", 9.0, 5),
        };
    foreach (var item in itemsDemo)
    {
        var f = _parkManager.TryAccept(item, out var reason);
        _ui.WriteLine($"[{(f ? " OK " : "FAIL")}] {item.Name, -20}" + $"{item.InventoryNumber, -13} reason: {reason}");
    }
    PrintAllReports();
    _ui.WriteLine("____end_demo___");
    }
}