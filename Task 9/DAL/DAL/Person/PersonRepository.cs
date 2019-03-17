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
        ///  Constructor
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

        private IdbConnector Db { get; set; }

        private List<Person> ItemList { get; set; }

        /// <summary>
        /// Удалить Item, в т.ч. и из БД, id = Item.ID
        /// <returns>bool Success?</returns>
        /// </summary>
        public bool Delete(int id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool boolresult = false;
            try
            {
                int personListIndex = ItemList.FindIndex(p => p.ID == id);
///                if (personListIndex == -1) 
                if (boolresult = ItemList[personListIndex].Delete())

                    // надо убрать отсюда
                    // Если удаление успешно, надо перезапросить из БД список item
                    // а пока просто удалю из списка
                    ItemList.RemoveAt(personListIndex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                boolresult = false;
                Logger.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }

            return boolresult;
        }

        /// <summary>
        /// Вернуть из списка item, id = item.ID
        /// </summary>
        /// <param name="id"> id = item.ID</param>
        /// <returns>ссылка на Item в списке, Null - что то пошло не так :(</returns>
        public Person Get(int id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            try
            {
                // ki. Не реализовано получение одного элемента из БД напрямую!
                return ItemList[ItemList.FindIndex(item => item.ID == id)];
            }
            catch (NullReferenceException ex)
            {
                Logger.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return null;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Logger.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return null;
            }
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
        /// Метод загружает список пользователей из БД
        /// </summary>
        /// <returns>список пользователей</returns>
        public void LoadPersons()
        {
            List<Person> newPersonList = new List<Person>(0);
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
///                    command.CommandText = SQLLoadPersons; // see DALResources.resx, "SELECT all fields FROM View QuizDB.dbo.V_M_Users";
                    command.CommandText = P_GetAllUsers;
                    command.Parameters.Add(Db.CreateParameter("@rc", DbType.Int32, "-1", null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var users = command.ExecuteReader())
                    {
                        while (users.Read())
                        {
                            newPersonList.Add(new Person((int)users[0], users[1].ToString(), users[2].ToString(), users[3].ToString(), users[4].ToString(), null, (RoleEnum)users[5]));
                            Logger.Info(string.Format("{0} {1}\n", users[0].ToString(), users[1].ToString()));
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                newPersonList = null;
                Logger.Info(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            ItemList = newPersonList;
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
                new Person(id: 2, firstname: "Igor", lastname: "Kalugin", username: "ki", password: "ki", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student),
                new Person(id: 3, firstname: "Nikolay", lastname: "Piskarev", username: "np", password: "np", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student),
                new Person(id: 4, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student)
            };
        }
    }
}
