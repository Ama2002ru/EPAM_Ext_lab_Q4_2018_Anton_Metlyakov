namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    /// <summary>
    /// интерфейс для работы с СУБД
    /// </summary>
    public interface IdbConnector
    {
        /// <summary>
        ///  сигнатура метода CreateConnection();
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateConnection();

        /// <summary>
        /// сигнатура метода для выполнения SQL-скрипта, 1 возвращаемое значение (например  результат хранимой процедуры)
        /// </summary>
        /// <param name="command"> Объект-типа IDbCommand</param>
        /// <returns></returns>
        object ExecuteScalar(string commandText, List<IDbDataParameter> sqlParams);

        /// <summary>
        ///  сигнатура метода для выполнения SQL-скрипта, метод должен возвращать Датасет
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string commandText /* позже добавлю список параметров */);

        /// <summary>
        ///  сигнатура метода для выполнения SQL-скрипта
        /// </summary>
        /// <param name="command"></param>
        /// <returns> возвращает количество обработанных записей</returns>
        int ExecuteNonQuery(string commandText /* позже добавлю список параметров */);

        /// <summary>
        /// Сигнатура метода добавления параметра
        /// </summary>
        /// <param name="name"></param>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        DbParameter CreateParameter(string name, DbType t, string value, int? length, ParameterDirection? dir);
    }
}