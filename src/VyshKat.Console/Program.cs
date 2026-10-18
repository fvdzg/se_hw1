using Microsoft.Extensions.DependencyInjection;
using VyshKat.Domain.interfaces;
using VyshKat.services;
using  VyshKat.ui;

var services = new ServiceCollection();
services.AddSingleton<IParkRegistry, ParkRegistry>();
services.AddSingleton<IServiceCenter, ServiceCenter>();
services.AddSingleton<ParkManager>();
services.AddSingleton<ReportService>();
services.AddSingleton<IUserInterface, ConsoleUserInterface>();
services.AddSingleton<ConsoleDemo>();
services.AddSingleton<InteractiveMenu>();
using var s = services.BuildServiceProvider();
var ui = s.GetRequiredService<IUserInterface>();
ui.WriteLine("___VyshKat___");
s.GetRequiredService<ConsoleDemo>().Run();
s.GetRequiredService<InteractiveMenu>().Run();
