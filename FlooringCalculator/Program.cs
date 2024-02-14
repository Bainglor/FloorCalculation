using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Collections.Generic;
using System.Numerics;

namespace FlooringCalculator
{
    /// <summary>
    /// Create nessecary dependencies and execute calculation.
    /// </summary>
    
    internal class Program
    {
        static void Main(string[] args)
        {
            // create a logger
            Log.Logger = new LoggerConfiguration()
                            //.WriteTo.Console()
                            .WriteTo.File("logs\\FlooringCalcualtor.txt",
                                          restrictedToMinimumLevel: LogEventLevel.Information,
                                          rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                            .MinimumLevel.Debug()
                            .CreateLogger();



            List<int> boards = new List<int>();
            boards.Add(2);
            boards.Add(3);

            //Should add the Microsoft DI package and add the dependencies for Logging
            //and PermutationCalculator to it.
            PermutationCalculator permutationCalculator = new PermutationCalculator(Log.Logger);
            FloorLayoutCalculator floorLayoutCalculator = new FloorLayoutCalculator(boards, Log.Logger, permutationCalculator);
            var floorLayoutCount = floorLayoutCalculator.GetFloorLayoutCount(32, 10);

            var message = "Total Floor Layouts ";
            Log.Logger.Information(message + floorLayoutCount);
            Console.WriteLine(message + floorLayoutCount.ToString());
            Console.ReadLine();
        }

        
    }
}