namespace Task5
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DAL.Utility;

    /// <summary>
    /// класс-обертка для вывода информации
    /// сейчас отправляю на консоль и в лог-файл
    /// </summary>
    public class OutputClass : IMessenger
    {
        /// <summary>
        /// вывести строку без "\n"
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            Console.Write(message);
            Logger.Debug(message);
        }

        /// <summary>
        /// вывести строку +"\n"
        /// </summary>
        /// <param name="message"></param>
        public void WriteLine(string message)
        {
            Console.Write(message + "\n");
            Logger.Debug(message);
        }
    }
}
