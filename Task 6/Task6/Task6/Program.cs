namespace Task6
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Console.WriteLine("Task 6 - Delegates\n");
            Console.WriteLine("\nPart 1: Array sorting\n");

            var myStringArray = new StringArray();
            try
            {
                myStringArray.Init();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Sorry, no file found!");
                return;
            }

            Console.WriteLine("String array had been loaded. To see it press any key");
            Console.ReadKey();
            myStringArray.Show();
            myStringArray.Sort(StringArray.MyCompareTo);
            Console.WriteLine("String array had been sorted. To see it press any key");
            Console.ReadKey();
            myStringArray.Show();
            Console.ReadKey();

            Console.WriteLine("\nPart 2: Office sumulator\n");

            var eppam = new OfficeClass();
            eppam.SimulateWorkDay();
        }
    }
}
