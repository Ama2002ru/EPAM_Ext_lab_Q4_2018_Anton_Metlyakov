namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SqlDB
    {
        private SQLConnector db;

        /// <summary>
        /// свойство-обертка для переменной
        /// </summary>
        public SQLConnector DB
        {
            get
            {
                // fail-safe инициализация
                if (db == null) db = new SQLConnector("ParameterlessCall");
                return db;
            }

            set
            {
                db = DB;
            }
        }
    }
}
