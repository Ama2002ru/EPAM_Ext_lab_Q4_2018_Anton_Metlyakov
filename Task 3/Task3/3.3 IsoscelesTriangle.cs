namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IsoscelesTriangle : SubTask
    {
        protected uint RowsNumber { get; set; }

        protected int TriangleCenter { get; set; }

        /// <summary>
        /// Main functionality of an object is here
        /// </summary>
        public override void Run()
        {
            this.RowsNumber = ReadUserInput.ReadUInt("Please enter how many rows to print (12 is a default): ", defaultValue: 12);
            this.TriangleCenter = (int)this.RowsNumber;
            int consoleToWiden = this.TriangleCenter * 2;
            if (consoleToWiden > Console.WindowWidth)
            {
                // ki - перехват специализированного исключения, это отличное решение. 
                try
                {
                    Console.WindowWidth = consoleToWiden;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Sorry, your console can't be widened to {0} characters", consoleToWiden);
                    return;
                }
            }

            this.Draw();
        }

        public void Draw()
        {
            for (int i = 0; i < this.RowsNumber; i++)
            {
               Console.WriteLine(
                       "{0}{1}", 
                       new string(' ', this.TriangleCenter - i),
                       new string('*', 1 + (i * 2)));
            }
        }

        /// <summary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public override string TellAboutMyself()
        {
            return "3.3 Asterisk isosceles triangle generation";
        }
    }
}
