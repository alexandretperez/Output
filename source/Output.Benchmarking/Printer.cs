using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using Output.Benchmarking.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Output.Benchmarking
{
    public class Printer
    {
        private readonly Formatting _formatting;

        public Printer(Formatting formatting)
        {
            _formatting = formatting;
        }

        public void Print<TSource, TTarget>(TestBase<TSource, TTarget> test) where TTarget : class
        {
            var type = test.GetType();
            PrintTitle(type.Name);

            var results = new List<string>();
            foreach (var method in type.GetMethods().Where(p => p.GetCustomAttribute<BenchmarkAttribute>() != null))
            {
                try
                {
                    var value = PrintJson(method.Invoke(test, null), method.Name);
                    results.Add(value);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{method.Name} fail!");
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }

            if (results.Distinct().Skip(1).Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ONE OR MORE RESULTS ARE DIFFERENT!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("ALL RESULTS MATCH!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private string PrintJson<T>(T o, string label)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(label);
            Console.ForegroundColor = ConsoleColor.Gray;
            var value = JsonConvert.SerializeObject(o, _formatting, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
            Console.WriteLine(value);
            Console.WriteLine();
            Console.WriteLine();
            return value;
        }

        private static void PrintTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("==============================");
            Console.WriteLine(title);
            Console.WriteLine("==============================");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}