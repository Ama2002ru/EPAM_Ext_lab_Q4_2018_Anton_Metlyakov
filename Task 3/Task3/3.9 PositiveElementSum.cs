namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PositiveElementSum : SubTask
    {
        private const int PositionsToPrint = 5;
        private const int ArraySize = 10;
        private int[] myArray;

        /// <summary>
        /// Main functionality of an object is here
        /// </summary>
        public override void Run()
        {
            Random randomobject = new Random(DateTime.Now.Millisecond);
            this.myArray = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                 this.myArray[i] = randomobject.Next(-ArraySize, ArraySize);
            }

            Console.WriteLine("Array[{0}] is :", ArraySize);
            this.PrintArray();
            try
            {
                Console.WriteLine("Sum of positive elements is : {0}", this.SumArray());
            }
            catch (OverflowException)
            {
                Console.WriteLine("Sorry, can't summarize, array is too big\n");
            }
        }

        /// <summary>
        /// Replaces positive values by zeroes
        /// </summary>
        public int SumArray()
        {
            int sum = 0;
            for (int i = 0; i < ArraySize; i++)
            {
                sum += this.myArray[i] > 0 ? this.myArray[i] : 0;
            }

            return sum;
        }

        /// <summary>
        /// just prints out an array
        /// </summary>
        public void PrintArray()
        {
            string printString = string.Empty;
            for (int i = 0; i < ArraySize; i++)
            {
                printString = string.Format("{0}", this.myArray[i]);
                Console.Write(
                        "{0}{1}",
                        string.Format("{0}", new string(' ', PositionsToPrint - printString.Length)),
                        printString);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public override string TellAboutMyself()
        {
            return "3.9 Positive elements sum in an array";
        }
    }
}
