using Cursova;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        ConsoleMenu menu = new ConsoleMenu();
        menu.ShowMainMenu();
    }
}
