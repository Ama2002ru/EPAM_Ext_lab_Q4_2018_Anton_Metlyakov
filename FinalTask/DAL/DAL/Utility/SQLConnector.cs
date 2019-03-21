namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Text;
    using DAL;

    /// <summary>
    /// Реализация интерфейса для MSSQL
    /// </summary>
    public class SQLConnector : IdbConnector
    {
        /// <summary>
        /// Строка конфигурации подключения к БД
        /// </summary>
        private ConnectionStringSettings dbconfig;

        /// <summary>
        ///  "Поставщик" подключения
        /// </summary>
        private DbProviderFactory factory;

        /// <summary>
        /// main Ctor
        /// </summary>
        public SQLConnector(string configString)
        {
            this.dbconfig = ConfigurationManager.ConnectionStrings[configString];
            if (this.dbconfig == null) throw new Exception($"No configuration for \"{configString}\" found!");
            this.factory = DbProviderFactories.GetFactory(this.dbconfig.ProviderName);
        }

        /// <summary>
        /// заготовка функции для читабельности вышележащего кода
        /// </summary>
        /// <returns>объект connection</returns>
        public IDbConnection CreateConnection()
        {
            IDbConnection connect = this.factory.CreateConnection();
            connect.ConnectionString = this.dbconfig.ConnectionString;
            return connect;
        }

        /// <summary>
        /// заготовка функции для читабельности вышележащего кода
        /// </summary>
        /// <returns>объект Parameter</returns>
        public DbParameter CreateParameter(string name, DbType t, string value, int? length, ParameterDirection? dir)
        {
            DbParameter param = this.factory.CreateParameter();
            param.ParameterName = name;
            param.DbType = t;
            if (value != null) param.Value   = value;
            if (length != null) param.Size   = length.Value;
            if (dir != null) param.Direction = dir.Value;
            return param;
        }

        /// <summary>
        /// сигнатура метода для выполнения SQL-скрипта, 1 возвращаемое значение (например  результат хранимой процедуры)
        /// </summary>
        /// <param name="command">SQL-строка </param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, List<IDbDataParameter> sqlParams)
        {
            object result;
            using (IDbConnection idbConnection = this.factory.CreateConnection())
            {
                idbConnection.Open();
                SqlCommand command = (SqlCommand)idbConnection.CreateCommand();
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                if (sqlParams != null)
                    command.Parameters.Add(sqlParams);
                result = command.ExecuteScalar();
            }

            return result;
        }

        /// <summary>
        ///  сигнатура метода для выполнения SQL-скрипта
        /// </summary>
        /// <param name="command"> SQL-строка </param>
        /// <returns>метод должен возвращать Датасет</returns>
        public IDataReader ExecuteReader(string commandText)
        {
            IDataReader result;
            using (IDbConnection idbConnection = this.factory.CreateConnection())
            {
                idbConnection.Open();
                var command = idbConnection.CreateCommand();
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                result = command.ExecuteReader();
                while (result.Read())
                    Console.WriteLine(result[0].ToString());
            }

            return result;
        }

        /// <summary>
        ///  сигнатура метода для выполнения SQL-скрипта
        /// </summary>
        /// <param name="command"> SQL-строка </param>
        /// <returns> возвращает количество обработанных записей</returns>
        public int ExecuteNonQuery(string commandText)
        {
            int affected;
            using (IDbConnection idbConnection = this.factory.CreateConnection())
            {
                idbConnection.Open();
                var command = idbConnection.CreateCommand();
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                affected = command.ExecuteNonQuery();
            }

            return affected;
        }
    }
}
