using System;

namespace Promoter
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            if (args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("You have not provided any arguments to this app. You should pass three arguments to the application.");
                Console.WriteLine("\t1. This argument will be the source directory and is manadatory");
                Console.WriteLine("\t2. This argument should be the source doc environment Ex:'qa|dev|stage|prod' (if not passed will be defaulted to 'dev')");
                Console.WriteLine("\t3. This argument should be the target environment Ex:'qa|dev|stage|prod' (If not passed will be defaulted to 'qa').");
                return;
            }
            var path = args.Length >= 1 ? args[0] : $@"C:\Convert";
            var sourceEnv = args.Length >= 2 ? args[1] : "dev";
            var targetEnv = args.Length >= 3 ? args[2] : "qa";
            Console.WriteLine($"path to files: {path}, source environment:{sourceEnv}, targetEnvironment:{targetEnv}");
            var process = new Process(path, sourceEnv, targetEnv);
            process.StartProcess();
        }
    }
}
