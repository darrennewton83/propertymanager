using DbUp;
using System.Reflection;

public class Program
{
    static int Main(string[] args)
    {
        Console.WriteLine("args=" + args.FirstOrDefault());
        if (args.FirstOrDefault() == null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        var connectionString = args.FirstOrDefault();

        EnsureDatabase.For.SqlDatabase(connectionString);
        var upgrader =
            DeployChanges.To
                .SqlDatabase(connectionString)
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
        return 0;
    }
}