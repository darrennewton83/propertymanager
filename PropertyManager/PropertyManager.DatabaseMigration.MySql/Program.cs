using DbUp;
using System.Reflection;

public class Program
{
    static int Main(string[] args)
    {
        Console.Title = "Property Manager Database Migration (MySql)";
        if (args.FirstOrDefault() == null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        var connectionString = args.FirstOrDefault();

        EnsureDatabase.For.MySqlDatabase(connectionString);
        var upgrader =
            DeployChanges.To
                .MySqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
#if DEBUG
            Console.ReadLine();
#endif
            return -1;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
        Console.ReadLine ();
        return 0;
    }
}