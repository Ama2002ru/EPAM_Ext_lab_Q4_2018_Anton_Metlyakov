namespace DAL.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using DAL.Interfaces;

    /// <summary>
    /// Класс для хранения "глобально-библиотечной" интерфейсной переменной-Логгера
    /// </summary>
    public static class Logger 
    {
        /// <summary>
        /// Интерфейсная переменная
        /// </summary>
        private static IMyLog log;

        /// <summary>
        /// свойство-обертка для переменной
        /// </summary>
        public static IMyLog Log
        {
            get
            {
                // fail-safe инициализация
                if (log == null) log = new Log4NetLogger();
                return log;
            }

            set
            {
                log = Log;
            }
        }

        /// <summary>
        /// сделать запись в лог
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            Log?.Debug(message);
        }

        /// <summary>
        /// сделать запись в лог
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            Log?.Error(message);
        }

        /// <summary>
        /// сделать запись в лог
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            Log?.Info(message);
        }
    }
}