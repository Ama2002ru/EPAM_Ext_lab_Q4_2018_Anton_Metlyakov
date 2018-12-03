namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ArrayTask1 : SubTask
    {
        private const int PositionsToPrint = 5;
        private const int ArraySize = 20;
        private int[] myArray;

        /// <summary>
        /// Main functionality of an object is here
        /// </summary>
        public override void Run()
        {
            Random randomobject = new Random(DateTime.Now.Millisecond); 
            this.myArray = new int[ArraySize];
            for (int i = this.myArray.GetLowerBound(0); i <= this.myArray.GetUpperBound(0); i++)
            {
                this.myArray[i] = randomobject.Next(-ArraySize, ArraySize);
            }

            Console.WriteLine("Array[{0}] is :", ArraySize);
            this.PrintArray();
            this.SortArray();
            Console.WriteLine(
                    "Min value = {0}, Max value = {1} ",
                    (this.GetMin().ToString() != string.Empty) ? this.GetMin().ToString() : "n/a",
                    (this.GetMax().ToString() != string.Empty) ? this.GetMax().ToString() : "n/a");
            Console.WriteLine("Sorted array[{0}] is :", ArraySize);
            this.PrintArray();
        }

        /// <summary>
        /// Sorts object's array by means of BinaryTreeNode object
        /// </summary>
        public void SortArray()
        {
            if (this.myArray == null)
            {
                throw new NullReferenceException();
            }

            BinaryTreeNode myBTree = null;
            for (int i = this.myArray.GetLowerBound(0); i <= this.myArray.GetUpperBound(0); i++)
            {
                if (myBTree == null)
                {
                    myBTree = new BinaryTreeNode(this.myArray[i]);
                }
                else
                {
                    myBTree.Add(this.myArray[i]);
                }
            }

            if (myBTree == null)
            {
                return;
            }

            myBTree.GetSortedTree().CopyTo(this.myArray, 0);
        }

        /// <summary>
        ///  Gets min value in an array
        /// </summary>
        /// <returns>returns min vlaue in an array, or null</returns>
        public int? GetMin()
        {
            if (this.myArray == null)
            {
                throw new NullReferenceException();
            }

            int? min = null;
            for (int i = this.myArray.GetLowerBound(0); i <= this.myArray.GetUpperBound(0); i++)
            {
                if (min == null || this.myArray[i] < min)
                {
                    min = this.myArray[i];
                }
            }

            return min;
        }

        /// <summary>
        ///  Gets max value in an array
        /// </summary>
        /// <returns>returns max vlaue in an array, or null</returns>
        public int? GetMax()
        {
            if (this.myArray == null)
            {
                throw new NullReferenceException();
            }

            int? max = null;
            for (int i = this.myArray.GetLowerBound(0); i <= this.myArray.GetUpperBound(0); i++)
            {
                if (max == null || this.myArray[i] > max)
                {
                    max = this.myArray[i];
                }
            }

            return max;
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
            return "3.7 Array data processing";
        }
    }
}
