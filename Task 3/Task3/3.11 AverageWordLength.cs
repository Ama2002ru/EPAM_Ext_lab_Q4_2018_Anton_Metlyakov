namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AverageWordLength : SubTask
    {
        /// <summary>
        /// Main functionality of an object is here
        /// Algorithm:
        /// 1. Scan input string
        /// 2. count all letters
        /// 3. count all non-letter->letter transitions
        /// 4. Profit :)        
        /// </summary>
        public override void Run()
        {
            string sentence;
            int letterCount = 0;
            int wordCount = 0;
            Console.WriteLine("Please enter string to analyze (Enter for default string): ");
            sentence = Console.ReadLine();
            if (sentence == string.Empty)
            {
                sentence = "Введите первую строку: написать программу, которая";
                Console.WriteLine("Default string \"{0}\" is used.", sentence);
            }

            for (int i = 0; i < sentence.Length; i++)
            {
                if (char.IsLetter(sentence[i]))
                {
                    // current symbol is a letter
                    letterCount++;
                    if (i == 0 || !char.IsLetter(sentence[i - 1]))
                    {
                        // it is start of a string, or previous symbol is not a letter
                        wordCount++;
                    }
                }
            }

            Console.WriteLine(wordCount > 0 ?
                string.Format("Average word length is {0}", (float)letterCount / (float)wordCount) :
                string.Format("Sorry, no words at all!"));
        }
        
        /// <sum
        /// mary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public override string TellAboutMyself()
        {
            return "3.11 Calculate average word length";
        }
    }
}
