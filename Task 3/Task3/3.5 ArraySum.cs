namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FilteredArraySum : SubTask
    {
        private const int RangeLow = 0;
        private const int RangeHigh = 1000;
        private readonly int[] dividers = { 3, 5 };

        /// <summary>
        /// Main functionality of an object is here
        /// </summary>
        public override void Run()
        {
            int rangeSum = 0;
            bool evenFlag;
            try
            {
                for (int i = RangeLow; i <= RangeHigh; i++)
                {
                    evenFlag = true;
                    for (int j = 0; j < this.dividers.Length; j++)
                    {
                        if ((i % this.dividers[j]) != 0)
                        {
                            evenFlag = false;
                        }
                    }

                    if (evenFlag)
                    {
                        rangeSum += i;
                    }
                }
            }
            catch (OverflowException)
            {
                Console.WriteLine("Sorry, can't summarize, array is too big\n");
            }

    Console.WriteLine("Sum of numbers, divisible by 3 and 5 in range [{0},{1}] is {2}", RangeLow, RangeHigh, rangeSum);
        }

        /// <summary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public override string TellAboutMyself()
        {
            return "3.5 Calculate the sum of a number range, divisible by 3 and 5";
        }
    }
}
