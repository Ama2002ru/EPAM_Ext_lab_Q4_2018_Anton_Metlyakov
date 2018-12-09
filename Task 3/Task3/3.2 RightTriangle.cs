namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RightTriangle : SubTask
    {
        private uint RowsNumber { get; set; }

        /// <summary>
        /// Main functionality of an object is here
        /// </summary>
        public override void Run()
        {
            // ki вопреки троелсену в реальности ключевое слово this используют только в крайнем случае. Чем меньше написано, тем лучше, и в данном случае оно избыточно.
            // ki: я вижу, что ты знаком с явным заданием параметров, но тут это тоже избыточно
            this.RowsNumber = ReadUserInput.ReadUInt("Please enter how many rows to print (15 is default): ", defaultValue: 15);
            if (this.RowsNumber > Console.WindowWidth)
            {
                // ki код красивый, отличное форматирование, а вот лишние фигурные скобки реально лишние. И без них будет еще лучше
                try
                {
                    Console.WindowWidth = (int)this.RowsNumber;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Sorry, your console can't be widened to {0} characters", this.RowsNumber);
                    return;
                }
            }

            this.Draw();
        }

        public void Draw()
        {
            for (int i = 0; i < this.RowsNumber; i++)
            {
                Console.WriteLine("{0}", new string('*', i));
            }
        }

        /// <summary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public override string TellAboutMyself()
        {
            return "3.2 Asterisk right triangle generation";
        }
    }
}
