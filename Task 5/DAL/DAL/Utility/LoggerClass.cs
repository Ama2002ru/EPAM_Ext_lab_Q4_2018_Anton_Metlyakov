namespace DAL.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using DAL.Interfaces;
    using log4net;
    using log4net.Config;

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
                if (log == null) log = new Log4NetLogger();
                return log;
            }

            set
            {
                log = Log;
            }
        }
    }
}