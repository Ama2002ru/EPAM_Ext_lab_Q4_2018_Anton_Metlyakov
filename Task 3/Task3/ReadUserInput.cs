namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ReadUserInput
    {
        /// <summary>
        /// Reads user input until user hit Enter or enter valid unsigned int number
        /// </summary>
        /// <param name="helpMSG"> message to user</param>
        /// <returns>int value entered</returns>
        public static uint ReadUInt(string helpMSG, uint defaultValue = 0)
        {
            uint retVal = 0;
            string userInput = string.Empty;
            bool error = false;
            do
            {
                if (error)
                {
                    Console.Write("Wrong input! Please enter unsigned integer number\n");
                }

                Console.Write(helpMSG);
                userInput = Console.ReadLine();
                if (userInput == string.Empty)
                {
                    return defaultValue;
                }
            }
            while (error = !uint.TryParse(userInput, out retVal));
            return retVal;
        }

        /// <summary>
        /// Reads user input until user  user hit Enter or enter valid int number
        /// </summary>
        /// <param name="helpMSG"> message to user</param>
        /// <returns>int value entered</returns>
        public static int ReadInt(string helpMSG, int defaultValue = 0)
        {
            int retVal = 0;
            string userInput = string.Empty;
            bool error = false;
            do
            {
                if (error) 
                {
                    Console.Write("Wrong input! Please enter an integer number\n");
                }

                Console.Write(helpMSG);
                userInput = Console.ReadLine();
                if (userInput == string.Empty)
                {
                    return defaultValue;
                }
            }
            while (error = !int.TryParse(userInput, out retVal));
            return retVal;
        }
    }
}