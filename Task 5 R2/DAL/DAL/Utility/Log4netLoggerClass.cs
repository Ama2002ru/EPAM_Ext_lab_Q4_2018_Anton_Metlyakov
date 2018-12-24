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
    /// Прикладной класс-адаптер для библиотеки Log4net. Нужен ли ?
    /// </summary>
    public class Log4NetLogger : IMyLog
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private static Lazy<ILog> lazyLog;
    
        public Log4NetLogger()
        {
            if (lazyLog == null) lazyLog = new Lazy<ILog>(() => LogManager.GetLogger("LOGGER"));
            InitLogger();
        }

        /// <summary>
        /// Инициализация конфигурации
        /// </summary>
        public void InitLogger()
        {
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// реализация интерфейсных методов
        /// </summary>
        /// <param name="s">строка в лог </param>
        public void Info(string s)
        {
            lazyLog.Value.Info(s);
        }

        /// <summary>
        /// реализация интерфейсных методов
        /// </summary>
        /// <param name="s">строка в лог </param>
        public void Debug(string s)
        {
            lazyLog.Value.Debug(s);
        }

        /// <summary>
        /// реализация интерфейсных методов
        /// </summary>
        /// <param name="s">строка в лог </param>
        public void Error(string s)
        {
            lazyLog.Value.Error(s);
        }
    }
}