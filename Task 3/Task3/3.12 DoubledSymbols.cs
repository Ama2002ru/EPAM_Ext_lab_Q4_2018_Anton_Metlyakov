namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DoubledSymbols : SubTask
    {
        /// <summary>
        /// Main functionality of an object is here
        /// </summary>
        public override void Run()
        {
            string sentence, multiplierSentence;
            StringBuilder resultString = new StringBuilder(string.Empty);
            Console.WriteLine("Please enter the first string (Enter for default string): ");
            sentence = Console.ReadLine();
            if (sentence == string.Empty)
            {
                sentence = "написать программу, которая";
                Console.WriteLine("Default string \"{0}\" is used.", sentence);
            }

            Console.WriteLine("Please enter the second string (Enter for default string)");
            multiplierSentence = Console.ReadLine();
            if (multiplierSentence == string.Empty)
            {
                multiplierSentence = "описание";
                Console.WriteLine("Default string\"{0}\" is used.", multiplierSentence);
            }

            multiplierSentence = multiplierSentence.ToUpper();
            for (int i = 0; i < sentence.Length; i++)
            {
                resultString.Append(sentence[i]);
                string s = sentence.Substring(i, 1).ToUpper();
                if (multiplierSentence.Contains(sentence.Substring(i, 1).ToUpper()))
                {
                    resultString.Append(sentence[i]);
                }
            }

            Console.WriteLine("Result string is : {0}", resultString);                
        }

        /// <sum
        /// mary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public override string TellAboutMyself()
        {
            return "3.12 expand original string with matching symbols";
        }
    }
}
