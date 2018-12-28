namespace Task_7
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
    public class ExtensionMethodSumClass : ITest
    {
        public ExtensionMethodSumClass(string name)
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
                arr_int.Add(arraySize - 2 * rnd.Next(arraySize));
            return arr_int;
        }

        /// <summary>
        /// Основное поведение класса
        /// </summary>
        /// <param name="ArraySize"></param>
        public void Run(int arraySize)
        {
            Console.WriteLine("7.1 Extensions. Array elements summarization.\n");
            var arr_int = Init(arraySize);
            var result = arr_int.SumOfArray();
            Console.WriteLine("int array sum is {0}", result);
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
            return null;
        }
    }
}
