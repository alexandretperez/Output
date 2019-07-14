using BenchmarkDotNet.Running;
using Output.Benchmarking.Tests;
using System;

namespace Output.Benchmarking
{
    public static class Program
    {
        public static void Main()
        {
            Console.Clear();

            var values = new[] {
                "Choose an option:",
                "[1] - Print Samples",
                "[2] - Choose Test to Run...",
                "[3] - Run All Tests! NOTE: TestBase Benchmark(BaseLine = true) must be removed",
                "[0] - Quit!",
                "",
                "* For better accuracy, run the benchmark tests in Release mode."
            };

            foreach (var message in values)
                Console.WriteLine(message);

            var key = Console.ReadKey().Key;
            Console.Clear();
            switch (key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    Console.WriteLine("Choose the sample:");
                    Console.WriteLine();
                    values = new[]
                    {
                        "Single Object",
                        "   [0] - BasicTest",
                        "   [1] - ComplexTest",
                        "   [2] - IntenseTest",
                        "   [3] - CustomTest",
                        "   [4] - FlatteningTest"
                    };

                    foreach (var message in values)
                        Console.WriteLine(message);

                    key = Console.ReadKey().Key;
                    PrintSamples(key);
                    break;

                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    RunBenchmarkDotNet(false);
                    break;

                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    RunBenchmarkDotNet(true);
                    break;

                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                    Environment.Exit(0);
                    break;
            }

            Console.ReadKey();
            Main();
        }

        public static void PrintSamples(ConsoleKey key)
        {
            Console.Clear();

            var printer = new Printer(Newtonsoft.Json.Formatting.None);
            switch (key)
            {
                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                    printer.Print(new BasicTest());
                    break;

                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    printer.Print(new ComplexTest());
                    break;

                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    printer.Print(new IntenseTest());
                    break;

                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    printer.Print(new CustomTest());
                    break;

                case ConsoleKey.NumPad4:
                case ConsoleKey.D4:
                    printer.Print(new FlatteningTest());
                    break;
            }
        }

        public static void RunBenchmarkDotNet(bool allTests)
        {
            Console.Clear();

            var benchmarks = new BenchmarkSwitcher(new[] {
                typeof(BasicTest),
                typeof(ComplexTest),
                typeof(IntenseTest),
                typeof(CustomTest),
                typeof(FlatteningTest)
            });

            if (allTests)
            {
                benchmarks.RunAllJoined();
                return;
            }

            benchmarks.Run();
        }
    }
}