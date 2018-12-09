namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CubeArray : SubTask
    { 
        private const int ArraySize = 4;
        private const int PositionsToPrint = 5;
        private int[,,] myArray;

        /// <summary>
        /// Main functionality of an object is here
        /// </summary>
        public override void Run()
        {
            Random randomobject = new Random(DateTime.Now.Millisecond);
            this.myArray = new int[ArraySize, ArraySize, ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                for (int j = 0; j < ArraySize; j++)
                {
                    for (int k = 0; k < ArraySize; k++)
                    {
                        this.myArray[i, j, k] = randomobject.Next(-ArraySize, ArraySize);
                    }
                }
            }

            Console.WriteLine("Array[{0},{0},{0}] is :", ArraySize);
            this.PrintArray();
            this.ProcessArray();
            Console.WriteLine("Processed array[{0},{0},{0}] is :", ArraySize);
            this.PrintArray();
        }

        /// <summary>
        /// Replaces positive values by zeroes
        /// </summary>
        public void ProcessArray()
        {
            // ki. все отлично, только скобки лишкие фигурные и опять ключевое слово this. Избавишься от этих привычек - будет совсем хорошо. 
            for (int i = 0; i < ArraySize; i++)
            {
                for (int j = 0; j < ArraySize; j++)
                {
                    for (int k = 0; k < ArraySize; k++)
                    {
                        if (this.myArray[i, j, k] > 0)
                        {
                            this.myArray[i, j, k] = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// just prints out an array
        /// </summary>
        public void PrintArray()
        {
            string printString = string.Empty;
            for (int i = 0; i < ArraySize; i++)
            {
                for (int j = 0; j < ArraySize; j++)
                {
                    for (int k = 0; k < ArraySize; k++)
                    {
                        printString = string.Format("{0}", this.myArray[i, j, k]);
                        Console.Write(
                                "{0}{1}",
                                string.Format("{0}", new string(' ', PositionsToPrint - printString.Length)),
                                printString);
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        /// <summary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public override string TellAboutMyself()
        {
            return "3.8 Cube Array processing";
        }
    }
}
