namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using System.Text;
    using DAL;
    using log4net;
    using static DALResources;

    /// <summary>
    /// Список пользователей
    /// </summary>
    public class PersonRepository : IPersonRepository  // В задании 9 используется "IUserRepository", тут просто другое слово
    {
       /// <summary>
        /// default Constructor
        /// </summary>
        [Obsolete]
        public PersonRepository()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            this.ItemList = new List<Person>(0);
        }

        /// <summary>
        ///  Конструктор с инверсией зависимости
        /// </summary>
        public PersonRepository(IdbConnector db)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            Db = db;
            this.ItemList = new List<Person>(0);
        }

        /// <summary>
        /// Количество записей в списке
        /// </summary>
        public int Count
        {
            get
            {
                return ItemList.Count;
            }
        }

        public IdbConnector Db { get; set; }

        private List<Person> ItemList { get; set; }

        /// <summary>
        /// Удалить Item, в т.ч. и из БД, id = Item.ID
        /// <returns>bool Success?</returns>
        /// </summary>
        public bool Delete(int id)
        {
            // Обновлено. Метод Delete репозитория теперь не обращается к методу Person.Delete
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool boolresult = false;
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    command.CommandText = P_DeleteUser;
                    command.Parameters.Add(Db.CreateParameter("@id", DbType.Int32, id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(Db.CreateParameter("@er", DbType.Int32, null, null, ParameterDirection.Output));
                    command.Parameters.Add(Db.CreateParameter("@et", DbType.String, null, 100, ParameterDirection.Output));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();
                    var deleteError = (int)((IDbDataParameter)command.Parameters["@er"]).Value;
                    var deleteErrorText = (string)((IDbDataParameter)command.Parameters["@et"]).Value;
                    Logger.Info(string.Format("P_DeleteUser out : {0} {1}\n", deleteError.ToString(), deleteErrorText));
                    if (deleteError == 0) boolresult = true;
                }
            }
            catch (DbException ex)
            {
                Logger.Info(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Logger.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                throw new Exception(string.Empty, ex);
            }

            return boolresult;
        }

        /// <summary>
        /// Вернуть из БД item, id = item.ID
        /// </summary>
        /// <param name="id"> id = item.ID</param>
        /// <returns>ссылка на Item в списке, Null - что то пошло не так :(</returns>
        public Person Get(int id)
        {
            // ki. Не реализовано получение одного элемента из БД напрямую!
            // Обновлено. Реализовал.
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            Person person = null;
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    command.CommandText = V_Get_User; // see DALResources.resx,
                    command.Parameters.Add(Db.CreateParameter("@user_id", DbType.Int32, id.ToString(), null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    DateTime? lastLogonDate = null;
                    using (var users = command.ExecuteReader())
                    {
                        while (users.Read())
                        {
                            // Nullable column processing ...
                            if (users.IsDBNull(8))
                                lastLogonDate = null;
                            else
                                lastLogonDate = DateTime.ParseExact(users[8].ToString(), S_DatetimeFormatString, System.Globalization.CultureInfo.InvariantCulture);
                            person = new Person(
                                                        (int)users[0],       // user_id
                                                        users[1].ToString(), // firstname
                                                        users[2].ToString(), // lastname
                                                        users[3].ToString(), // username 
                                                        users[4].ToString(), // password
                                                        users[5].ToString(), // salt
                                                        null,                // QuizResults
                                                        (RoleEnum)users[6],  // roles
                                                        DateTime.ParseExact(users[7].ToString(), S_DatetimeFormatString, System.Globalization.CultureInfo.InvariantCulture),
                                                        lastLogonDate);      // lastlogondate
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                person = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return person;
        }

        /// <summary>
        /// Возвращает список всех Person
        /// </summary>
        /// <returns>ItemList</returns>
        public List<Person> GetAll()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            this.LoadPersons();
            return ItemList;
        }

        /// <summary>
        ///  Записать в БД инфо о item.Ожидаю что item.ID будет корректным и не проверяю его
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Save(Person person)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool boolresult = false;
            try
            {
                boolresult = person.Save(Db);
            }
            catch (NullReferenceException ex)
            {
                Logger.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }

            return boolresult;
        }
    
        /// <summary>
        /// method for testing
        /// </summary>
        /// <returns></returns>
        /// 
        [Obsolete]
        public void Init()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            this.ItemList = new List<Person>
            {
                new Person(), // default person "John Doe"
                new Person(id: 2, firstname: "Igor", lastname: "Kalugin", username: "ki", password: "ki", salt: "salt", quizResults: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null),
                new Person(id: 3, firstname: "Nikolay", lastname: "Piskarev", username: "np", password: "np", salt: "salt", quizResults: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null),
                new Person(id: 4, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", salt: "salt", quizResults: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null)
            };
        }

        /// <summary>
        /// Метод загружает список пользователей из БД
        /// </summary>
        /// <returns>список пользователей</returns>
        private void LoadPersons()
        {
            List<Person> newPersonList = new List<Person>(0);
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    command.CommandText = P_GetAllUsers; // see DALResources.resx,
                    command.Parameters.Add(Db.CreateParameter("@rc", DbType.Int32, "-1", null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    DateTime? lastLogonDate = null;
                    using (var users = command.ExecuteReader())
                    {
                        while (users.Read())
                        {
                            // Nullable column processing ...
                            if (users.IsDBNull(8))
                                lastLogonDate = null;
                            else
                                lastLogonDate = DateTime.ParseExact(users[8].ToString(), S_DatetimeFormatString, System.Globalization.CultureInfo.InvariantCulture);
                            int id = (int)users[0];
                            var roles = (RoleEnum)users[6];
                            newPersonList.Add(new Person(
                                                        id,
                                                        users[1].ToString(), // firstname
                                                        users[2].ToString(), // lastname
                                                        users[3].ToString(), // username 
                                                        users[4].ToString(), // password
                                                        users[5].ToString(), // salt
                                                        null,                  // workbook
                                                        roles,  // roles
                                                        DateTime.ParseExact(users[7].ToString(), S_DatetimeFormatString, System.Globalization.CultureInfo.InvariantCulture),
                                                        lastLogonDate));      // lastlogondate
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                newPersonList = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            ItemList = newPersonList;
        }
    }
}
