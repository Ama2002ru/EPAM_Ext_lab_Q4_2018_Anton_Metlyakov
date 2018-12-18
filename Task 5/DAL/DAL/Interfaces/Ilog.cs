namespace DAL.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// интерфейс для "подключения" объектов-логгеров
    /// </summary>
    public interface IMyLog
    {
        /// <summary>
        /// запись типа ИНФО
        /// </summary>
        /// <param name="s"></param>
        void Info(string s);

        /// <summary>
        /// запись типа ДЕБАГ
        /// </summary>
        /// <param name="s"></param>
        void Debug(string s);

        /// <summary>
        /// Запись типа ЕРРОР
        /// </summary>
        /// <param name="s"></param>
        void Error(string s);
    }
}