namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FontAttributes : SubTask
    {
        [Flags]
        private enum FontAttr
        {
            none = 0, // this value supposed to be the first one, in order to validate user input
            bold = 1,
            italic = 2,
            underline = 4
        }

        /// <summary>
        /// Main functionality of an object is here
        /// </summary>
        public override void Run()
        {
            FontAttr textInfo = FontAttr.none;
            int fontAttrToChange = 0;
            ConsoleKeyInfo keyPressed;
            Console.WriteLine("Text attributes are : {0}", textInfo.ToString());

            do
            {
                Console.WriteLine(string.Format(
                                    "To change text attributes enter :\n" +
                                    "{0} for {1}\n" +
                                    "{2} for {3}\n" +
                                    "{4} for {5}\n" +
                                    "Any other key to exit\n",
                                    Array.IndexOf(Enum.GetValues(textInfo.GetType()), FontAttr.bold),
                                    FontAttr.bold,
                                    Array.IndexOf(Enum.GetValues(textInfo.GetType()), FontAttr.italic),
                                    FontAttr.italic,
                                    Array.IndexOf(Enum.GetValues(textInfo.GetType()), FontAttr.underline),
                                    FontAttr.underline));
                keyPressed = Console.ReadKey();
                if (!(keyPressed.Key == ConsoleKey.D1 || keyPressed.Key == ConsoleKey.D2 || keyPressed.Key == ConsoleKey.D3))
                {
                    break;
                }

                switch (keyPressed.Key)
                {
                case ConsoleKey.D1:
                    fontAttrToChange = 1;
                    break;
                case ConsoleKey.D2:
                    fontAttrToChange = 2;
                    break;
                case ConsoleKey.D3:
                    fontAttrToChange = 3;
                    break;
                default: break;
                }

                textInfo ^= (FontAttr)Enum.GetValues(textInfo.GetType()).GetValue(fontAttrToChange);
                Console.WriteLine("Text attributes are : {0}", textInfo.ToString());
            }
            while (true);
        }

        /// <summary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public override string TellAboutMyself()
        {
            return "3.6 Font attributes demo";
        }
    }
}
