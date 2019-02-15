namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SqlDB
    {
        private SQLConnectorClass db;

        /// <summary>
        /// свойство-обертка для переменной
        /// </summary>
        public SQLConnectorClass DB
        {
            get
            {
                // fail-safe инициализация
                if (db == null) db = new SQLConnectorClass("ParameterlessCall");
                return db;
            }

            set
            {
                db = DB;
            }
        }
    }
}
