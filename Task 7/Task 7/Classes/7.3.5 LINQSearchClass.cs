﻿namespace Task_7
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// класс, реализующий прямой метод поиска
    /// </summary>
    public class LINQSearchClass : ITest
    {
        /// <summary>
        /// Сколько делать предварительных прогонов теста перед измерением затрат времени
        /// </summary>
        public const int StopWatchMinCount = 2;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        public LINQSearchClass(string name)
        {
            MethodName = name;
        }

        /// <summary>
        /// отображаемое имя класса
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// Создание и заполнение массива
        /// </summary>
        /// <param name="arraySize"></param>
        /// <returns></returns>
        public List<int> Init(int arraySize)
        {
            var rnd = new Random();
            var arr_int = new List<int>(arraySize);
            for (int i = 0; i < arraySize; i++)
                arr_int.Add((int)(arraySize - (2 * rnd.NextDouble() * arraySize)));
            return arr_int;
        }

        /// <summary>
        /// Основное поведение класса
        /// </summary>
        /// <param name="ArraySize"></param>
        public void Run(int arraySize)
        {
            var arr_int = Init(arraySize);
            Console.WriteLine("7.3.5 Extensions. LINQ array search.\n");
            var arrIntDelegate = arr_int.Where((x) => (x > 2 && x < 7)).OrderByDescending(x => x).Select(x => x);
            Console.WriteLine("Elements of int array between (2,7), order by descending");
            foreach (var element in arrIntDelegate)
                Console.Write("{0} ", element.ToString());
            Console.WriteLine("\n");
        }

        /// <summary>
        /// Специальный метод тестирования скорости работы метода поиска
        /// </summary>
        /// <param name="TestArraySize"></param>
        /// <param name="TestPerformanceCount"></param>
        /// <returns></returns>
        public TimeSpan? TestPerformance(int testArraySize, int testPerformanceCount)
        {
            Stopwatch stopwatch = new Stopwatch();
            var arr_int = Init(testArraySize);
            if (testPerformanceCount < StopWatchMinCount) throw new Exception("количество повторов теста слишком мало");
            var tresult = new List<float>(testArraySize);
            for (int i = 0; i < testPerformanceCount; i++)
            {
                if (i == StopWatchMinCount)
                    stopwatch.Start();      // первые круги "прогревочные"
                arr_int.Where((x) => (x > 2 && x < 700)).OrderByDescending(x => x).Select(x => x);
            }

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
