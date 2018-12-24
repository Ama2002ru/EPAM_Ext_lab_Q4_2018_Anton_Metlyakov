namespace Task6
{
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

    /// <summary>
    /// Класс-сортировщик строк
    /// </summary>
    public class StringArray
    {
        /// <summary>
        /// собственно массив строк.
        /// </summary>
        private string[] stringsArray;

        /// <summary>
        /// Тип делегата ?, я правильно понимаю ?
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public delegate int DelegateComparer(string s, string t);

        /// <summary>
        /// поменять местами 2 строки в массиве
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        public static void Swap(ref string s1, ref string s2)
        {
            if (s1 == null || s2 == null) throw new NullReferenceException();
            string temp_s = s1;
            s1 = s2;
            s2 = temp_s;
        }

        /// <summary>
        /// реализую кастомное стравнение строк согласно условия задачи 6.1.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static int MyCompareTo(string s1, string s2)
        {
            if (s1 == null || s2 == null) throw new NullReferenceException();
            if (s1.Length < s2.Length) return -1;
            if (s1.Length > s2.Length) return 1;
            return s1.CompareTo(s2);
        }

        /// <summary>
        /// Прочитать откуда-нибудь массив
        /// </summary>
        public void Init()
        {
            // читаю файл Program.cs
            stringsArray = File.ReadAllLines(
                Environment.CurrentDirectory + @"..\..\..\Program.cs",
                Encoding.UTF8);
        }

        /// <summary>
        /// Вывести массив на консоль
        /// </summary>
        public void Show()
        {
            foreach (var s in stringsArray)
            {
                int extraWidth = 1;
                if (s.Length > Console.WindowWidth + extraWidth)
                    try
                    {
                        Console.WindowWidth = s.Length + extraWidth;
                    }
                    catch (ArgumentOutOfRangeException)
                    { }
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// сортируем массив, в параметре - метод сравнения строк
        /// </summary>
        public void Sort(DelegateComparer myFunc)
        {
            for (int i = stringsArray.GetLowerBound(0); i < stringsArray.GetUpperBound(0); i++)
            {
                for (int j = i + 1; j <= stringsArray.GetUpperBound(0); j++)
                {
                    if (myFunc(stringsArray[i], stringsArray[j]) > 0)
                    {
                        Swap(ref stringsArray[i], ref stringsArray[j]);
                    }
                }
            }
        }
    }
}
