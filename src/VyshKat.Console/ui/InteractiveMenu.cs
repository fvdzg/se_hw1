namespace VyshKat.ui;

using System.Globalization;
using VyshKat.Domain.Entities;
using VyshKat.Domain.interfaces;
using VyshKat.services;
using System.Diagnostics.CodeAnalysis;
[ExcludeFromCodeCoverage]
public sealed class InteractiveMenu
{
    private readonly ParkManager _parkManager;
    private readonly ReportService _reports;
    private readonly IUserInterface _ui;
    public InteractiveMenu(ParkManager parkManager, ReportService reports, IUserInterface ui)
    {
        _parkManager = parkManager;
        _reports = reports;
        _ui = ui;
    }

    private void AcceptAndReport(IInventoryItem item)
    {
        var f = _parkManager.TryAccept(item, out var reason);
        _ui.WriteLine(f ? $"accepted: {item.Name}" : $"rejected: {reason}");
    }

    private static double StrToDouble(string? s)
    {
        if (double.TryParse(s, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
        {
            return d;
        }
        return 0;
    }

    private static int StrToInt(string? s)
    {
        if (int.TryParse(s, out var i))
        {
            return i;
        }
        return 1;
    }

    private void AddVehicle(string type)
    {
        _ui.WriteLine("InventoryNumber: ");
        var num = _ui.ReadLine() ?? "";
        _ui.WriteLine("Name: ");
        var name = _ui.ReadLine() ?? "";
        _ui.WriteLine("Beginner friendliness: ");
        var b = StrToInt(_ui.ReadLine());
        IInventoryItem v;
        if (type == "bicycle")
        {
            v = new Bicycle(num, name, b);
        }
        else {
            _ui.WriteLine("Daily kWh: ");
            var kwh = StrToDouble(_ui.ReadLine());
            if (type == "scooter")
            {
                v = new Scooter(num, name, kwh, b);
            }
            else if(type == "electric bike")
            {
                v = new ElectricBicycle(num, name, kwh, b);
            }
            else
            {
                throw new InvalidOperationException($"unknown vehicle: {type}");
            }
        }
        AcceptAndReport(v);
    }
    private void AddAccessory(string type)
    {
        _ui.WriteLine("InventoryNumber: ");
        var num = _ui.ReadLine() ?? "";
        _ui.WriteLine("Name: ");
        var name = _ui.ReadLine() ?? "";
        IInventoryItem a;
        if (type == "helmet")
        {
            a = new Helmet(num, name);
        }
        else
        {
            _ui.WriteLine("Daily kWh: ");
            var kwh = StrToDouble(_ui.ReadLine());
            if (type == "dock station")
            {
                a = new DockStation(num, name, kwh);
            }
            else
            {
                a = new ChargingCabinet(num, name, kwh);
            }
        }
       
        AcceptAndReport(a);
    }

    public void PrintReports()
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
        while (true)
        {
            _ui.WriteLine(
                "\n___menu___\n" +
                "0) Exit\n" +
                "1) Add scooter\n" +
                "2) Add electric bike\n" +
                "3) Add bicycle\n" +
                "4) Add helmet\n" +
                "5) Add dock station\n" +
                "6) Add charging cabinet\n" +
                "7) Reports\n");
            var choice = _ui.ReadLine();
            switch(choice)
            {
                case "0": return;
                case "1": AddVehicle("scooter"); break;
                case "2": AddVehicle("electric bike"); break;
                case "3": AddVehicle("bicycle");break;
                case "4": AddAccessory("helmet"); break;
                case "5": AddAccessory("dock station"); break;
                case "6": AddAccessory("charging cabinet"); break;
                case "7": PrintReports(); break;
                default: _ui.WriteLine("unknown option"); break;

            }
        }
    }
}