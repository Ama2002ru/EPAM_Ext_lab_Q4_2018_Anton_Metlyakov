namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StringAndStringBuilder : SubTask
    {
        /// <summary>
        /// Main functionality of an object is here
        /// </summary>
        public override void Run()
        {
            Stopwatch stopWatchStr = new Stopwatch();
            stopWatchStr.Start();
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();
            int count = 100000;
            for (int i = 0; i < count; i++)
            {
                str += "*";
            }

            stopWatchStr.Stop();
            TimeSpan tsstr = stopWatchStr.Elapsed;
            Stopwatch stopWatchSB = new Stopwatch();
            stopWatchSB.Start();
            for (int i = 0; i < count; i++)
            {
                sb.Append("*");
            }

            stopWatchSB.Stop();
            TimeSpan tssb = stopWatchSB.Elapsed;

            string elapsedTimeStr = string.Format(
                                           "{0:00}:{1:00}.{2:00}",
                                           tsstr.Minutes, 
                                           tsstr.Seconds,
                                           tsstr.Milliseconds);

            string elapsedTimeSB = string.Format(
                                          "{0:00}:{1:00}.{2:00}",
                                          tssb.Minutes,
                                          tssb.Seconds,
                                          tssb.Milliseconds);

            Console.WriteLine("RunTime for {1} iterations of string is {0}", elapsedTimeStr, count);
            Console.WriteLine("RunTime for {1} iterations of StringBuilder is {0}", elapsedTimeSB, count);
        }

        /// <summary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public override string TellAboutMyself()
        {
            return "3.13 Performance analysis of string and StringBulder class";
        }
    }
}
