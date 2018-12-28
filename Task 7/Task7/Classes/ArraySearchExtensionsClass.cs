namespace Task7.Classes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// здесь живут методы поиска элемента(ов) в массиве
    /// </summary>
    public static class ArraySearchExtensionsClass
    {
        /// <summary>
        /// Метод, реализующий поиск напрямую
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindAllPositives<T>(this IEnumerable<T> enumerable) where T : IComparable
        {
            var tresult = new List<T>();
            foreach (var element in enumerable)
                if (element.CompareTo(default(T)) > 0)    // element.value is positive
                    tresult.Add(element);
            return tresult;
        }

        /// <summary>
        /// Функция определяет, входит ли arg в диапазон
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <returns></returns>
        public static bool IsBetween<T>(T arg, T lowerBound, T upperBound) where T : IComparable
        {
            if ((arg.CompareTo(lowerBound) >= 0) && (arg.CompareTo(upperBound) <= 0))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Метод, реализующий поиск через делегат Func<>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindAllByDelegate<T>(
                                     this IEnumerable<T> enumerable,
                                     Func<T, T, T, bool> condition, 
                                     T lowerValue,
                                     T upperValue) where T : IComparable
        {
            var tresult = new List<T>();
            foreach (var element in enumerable)
                if (condition(element, lowerValue, upperValue)) 
                    tresult.Add(element);
            return tresult;
        }

        /// <summary>
        /// Метод, реализующий поиск через лямбда-делегат Func<>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindAllByLyambdaDelegate<T>(
                                     this IEnumerable<T> enumerable,
                                     Func<T, bool> condition) where T : IComparable
        {
            var tresult = new List<T>();
            foreach (var element in enumerable)
                if (condition(element))
                    tresult.Add(element);
            return tresult;
        }
    }
}
