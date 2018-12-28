namespace Task7
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Task7.Classes;

    public class Program
    {
        public const int ArraySize = 20;
        public const int TestPerformanceCount = 1000;
        public const int TestArraySize = 1000000;
        public const int StopWatchMinCount = 2;

        /// <summary>
        /// Перечисление для теста производительности методов
        /// </summary>
        public enum SearchMethods
        {
            DirectMethod = 1,
            DelegateMethod = 2,
            AnonymousDelegateMethod = 3,
            LyambdaDelegateMethod = 4,
            LINQ_Method = 5
        }

        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Console.WriteLine("Task 7. Extensions and LINQ\n");
            Console.WriteLine("7.1 Extensions. Array elements summarization.\n");

            var arr_float = Init<float>(ArraySize);
            var arr_int = Init<int>(ArraySize);

            var result = arr_int.SumOfArray();
            Console.WriteLine("{0} array sum is {1}", arr_int.GetType(), result);

            var result_f = arr_float.SumOfArray();
            Console.WriteLine("{0} array sum is {1}", arr_float.GetType(), result_f);

            Console.WriteLine("\n7.2 Extensions. String parse.\n");
            Console.WriteLine("string \"{0}\" represents a natural number : {1}", "10", "10".IsNatural().ToString());
            Console.WriteLine("string \"{0}\" represents a natural number : {1}", "+0", "+0".IsNatural().ToString());
            Console.WriteLine("string \"{0}\" represents a natural number : {1}", "-20", "-20".IsNatural().ToString());

            Console.WriteLine("\n7.3.1 Extensions. Direct array search.\n");
            var arrIntPositive = arr_int.FindAllPositives();
            Console.WriteLine("Positive elements of int array :");
            Show(arrIntPositive);
            Console.WriteLine("\n");

            Console.WriteLine("7.3.2 Extensions. Delegate array search.\n");
            var arrIntDelegate = arr_int.FindAllByDelegate(ArraySearchExtensionsClass.IsBetween, 5, 10);
            Console.WriteLine("Elements of int array between 5 and 10 :");
            Show(arrIntDelegate);
            Console.WriteLine("\n");

            Console.WriteLine("7.3.3 Extensions. Anonymous delegate array search.\n");
            arrIntDelegate = arr_int.FindAllByDelegate(
                delegate(int element, int lowerBound, int upperBound)
                {
                    return element.CompareTo(lowerBound) >= 0 && element.CompareTo(upperBound) < 0;
                }, 
                5,
                10);
            Console.WriteLine("Elements of int array between 5 and 10 :");
            Show(arrIntDelegate);
            Console.WriteLine("\n");

            Console.WriteLine("7.3.4 Extensions. Lyambda delegate array search.\n");
            var arrFloatLyambdaDelegate = arr_float.FindAllByLyambdaDelegate(
                (x) => (x.CompareTo(0) >= 0 && x.CompareTo(float.MaxValue) < 0));
            Console.WriteLine("Positive elements of float array :");
            Show(arrFloatLyambdaDelegate);
            Console.WriteLine("\n");

            Console.WriteLine("7.3.5 Extensions. LINQ array search.\n");
            var arrFloatDelegate = arr_float.Where((x) => (x > 2.1f && x < 7f)).OrderByDescending(x => x).Select(x => x);
            Console.WriteLine("Elements of float array between (2.1,7), order by descending");
            Show(arrFloatDelegate);

            Console.WriteLine("\nMethod performance benchmark. Array size is 1М, 1К repeat count ...\n");
            foreach (SearchMethods method in Enum.GetValues(typeof(SearchMethods)))
            {
                var timeElapsed = MeasurePerformance(method, TestPerformanceCount);
                Console.WriteLine($"Elapsed time of {method} = {timeElapsed}");
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Просто распечатаю коллекцию
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        public static void Show<T>(IEnumerable<T> collection) where T : struct
        {
            foreach (var element in collection)
                Console.Write("{0} ", element.ToString());
        }

        /// <summary>
        /// Инициализация массивов случайными значениями в диапазоне (-arraySize, +arraySize)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        public static IEnumerable<T> Init<T>(int arraySize) where T : struct
        {
            var rnd = new Random();
            var newArray = new List<T>(arraySize);
            for (int i = 0; i < arraySize; i++)
                newArray.Add((T)(dynamic)(arraySize - (2 * rnd.NextDouble() * arraySize))); 
            return newArray.ToArray();
        }

        /// <summary>
        /// Метод измеряет время выполнения методов
        /// </summary>
        /// <param name="method"></param>
        /// <param name="testPerformanceCount"></param>
        /// <returns></returns>
        public static TimeSpan MeasurePerformance(SearchMethods method, int testPerformanceCount)
        {
            Stopwatch stopwatch = new Stopwatch();
            var arr_float = Init<float>(TestArraySize);
            if (testPerformanceCount < StopWatchMinCount) throw new Exception("количество повторов теста слишком мало");
            var tresult = new List<float>(TestArraySize);
            for (int i = 0; i < testPerformanceCount; i++)
            {
                if (i == StopWatchMinCount)
                    stopwatch.Start();      // первые круги "прогревочные"
                switch (method)
                {
                    case SearchMethods.DirectMethod:
                        {
                            arr_float.FindAllPositives();
                            break;
                        }

                    case SearchMethods.DelegateMethod:
                        {
                            arr_float.FindAllByDelegate(ArraySearchExtensionsClass.IsBetween, 5, 10);
                            break;
                        }

                    case SearchMethods.AnonymousDelegateMethod:
                        {
                            arr_float.FindAllByDelegate(delegate(float element, float lowerBound, float upperBound) { return element.CompareTo(lowerBound) >= 0 && element.CompareTo(upperBound) < 0; }, 5f, 10f);
                            break;
                        }

                    case SearchMethods.LyambdaDelegateMethod:
                        {
                            arr_float.FindAllByLyambdaDelegate((x) => (x.CompareTo(0) >= 0 && x.CompareTo(float.MaxValue) < 0));
                            break;
                        }

                    case SearchMethods.LINQ_Method:
                        {
                            arr_float.Where((x) => (x > 2.1f && x < 7f)).OrderByDescending(x => x).Select(x => x);
                            break;
                        }
                }
            }

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
