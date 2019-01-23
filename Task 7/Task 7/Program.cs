namespace Task_7
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class Program
    {
        /// <summary>
        /// размер массива для метода Run
        /// </summary>
        public const int ArraySize = 20;

        /// <summary>
        /// Количество прогонов по тестовому массиву
        /// </summary>
        public const int TestPerformanceCount = 1000;

        /// <summary>
        /// Размер массива для измерения скорости  вычислений
        /// </summary>
        public const int TestArraySize = 1000000;

        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Console.WriteLine("Task 7. Extensions and LINQ\n");
            List<ITest> tests = new List<ITest>
            {
               // new ExtensionMethodSumClass("ExtensionMethodSummarization"),
                //new ExtensionMethodStringParseClass("ExtensionMethodStringParse"),
                new DirectArraySearchClass("Direct search method"),
                new DelegateArraySearchClass("Delegate method"),
                new AnonymousDelegateSearchClass("Anonymous delegate method"),
                new LyambdaDelegateSearchClass("Lyambda delegate method"),
                new LINQSearchClass("LINQ method")
            };

            foreach (var test in tests)
            {
                test.Run(ArraySize);
            }

            Console.WriteLine($"Method performance benchmark. Array size is {TestArraySize}, {TestPerformanceCount} repeat count ...\n");
            foreach (var test in tests)
            {
                var timeElapsed = test.TestPerformance(TestArraySize, TestPerformanceCount);
                if (timeElapsed != null) Console.WriteLine($"Elapsed time of {test.MethodName} = {timeElapsed}");
            }

            Console.ReadKey();
        }
    }
}
