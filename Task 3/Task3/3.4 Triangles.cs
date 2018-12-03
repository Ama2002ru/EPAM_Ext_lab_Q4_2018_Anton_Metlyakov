namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Triangles : IsoscelesTriangle
    {
        private uint TriangleCount { get; set; }

        /// <summary>
        /// Main functionality of an object is here
        /// </summary>
        public override void Run()
        {
            this.TriangleCount = ReadUserInput.ReadUInt("Please enter how many triangles to print (7 is a default : ", defaultValue: 7);
            this.RowsNumber = this.TriangleCount;
            this.TriangleCenter = (int)this.RowsNumber;
            int consoleToWiden = TriangleCenter * 2;
            if (consoleToWiden > Console.WindowWidth)
            {
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

        public new void Draw()
        {
            for (uint i = 1; i <= this.TriangleCount; i++)
            {
                this.RowsNumber = i;
                base.Draw();
            }
        }

        /// <summary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public override string TellAboutMyself()
        {
            return "3.4 Chain of triangles generation";
        }
    }
}
