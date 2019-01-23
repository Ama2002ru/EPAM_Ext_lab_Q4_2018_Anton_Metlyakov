namespace Task_7
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public static class ExtensionsClass
    {
        /// <summary>
        /// Метод расширения вычисляет сумму элементов коллекции.
        /// Сделал на обобщениях с динамик типами. Работает.
        /// Но VS & intellisense предлагают для подобных задач
        /// перегруженные методы для каждого значимого типа.
        /// Есть какие-то подводные камни при использовании dynamic ?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static T SumOfArray<T>(this IEnumerable<T> enumerable) where T : struct, IComparable
        {
            T sumvalue = default(T);
            foreach (var element in enumerable)
                // ki. какой грязный трюк с dynamic! может упасть на этапе выполнения но за находчивость плюс!
                sumvalue = (dynamic)sumvalue + (dynamic)element;
            return sumvalue;
        }

        /// <summary>
        /// Метод определяет,является ли строка положительным целым числом.
        /// Работает на регулярных выражениях. Не стреляйте в пианиста :), использую регекспы второй раз.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        //ki. молодец, хорошая идея и реализация. 
        public static bool IsNatural(this string s)
        {
            if (s == null)
                return false;                           // буду считать, что числа в строке нет.
            var testString = s;
            testString.Trim();
            if (testString[0] == '+') testString = testString.Remove(0, 1);
            var regex = new Regex(@"^(\d*)[1-9]+");     // должны быть только цифры, и не менее одной. 
                                                        // и хотя бы одна цифра должна быть не 0
            return regex.IsMatch(testString.Trim());
        }
    }
}
