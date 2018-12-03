namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BoxAreaCalculation : SubTask
    {
        private uint SizeX { get; set; }

        private uint SizeY { get; set; }
        
        /// <summary>
        /// Main functionality of an object is here
        /// </summary>
        public override void Run()
        {
            this.SizeX = ReadUserInput.ReadUInt("Please enter the side size of a box (10 is a default):", defaultValue: 10);
            this.SizeY = ReadUserInput.ReadUInt("Please enter another side size of a box (20 is a default):", defaultValue: 20);
            try
            {
                Console.WriteLine("Area of the box is {0}", this.Area());
            }
            catch (OverflowException)
            {
                Console.WriteLine("Sorry, i'm not so smart, i can't imagine such a box!");
            }
        }

        /// <summary>
        /// Calculates area of a box by multiplying its sides.
        /// </summary>
        /// <returns> uint area, possible overflow exception</returns>
        public uint Area()
        {
            return checked(this.SizeX * this.SizeY);
        }

        /// <summary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public override string TellAboutMyself()
        {
            return "3.1 box area calculation";
        }
    }
}
