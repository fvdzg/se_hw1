namespace VyshKat.ui;
using System.Diagnostics.CodeAnalysis;
[ExcludeFromCodeCoverage]
public sealed class ConsoleUserInterface : IUserInterface
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}