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

    public class RolesRepository : IRolesRepository
    {
        /// <summary>
        ///  список ролей в системе
        /// </summary>
        private List<Role> rolesList;

        public RolesRepository(IdbConnector db)
        {
            Db = db;
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
        }

        public int Count
        {
            get
            {
                return rolesList.Count;
            }
        }

        /// <summary>
        /// реализация инверсии зависимостей
        /// </summary>
        private IdbConnector Db { get; set; }

         /// <summary>
        /// Метод создает набор ролей приложения, пока не реализован ADO.NET коннектор
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static List<Role> Init()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            return new List<Role>
            {
                 new Role(id: 1, name: "Admin", role: RoleEnum.Admin, allowedMethods: "PersonRepository.Add;PersonRepository.Delete;"), // последняя ";" обязательна
                 new Role(id: 2, name: "Student", role: RoleEnum.Student, allowedMethods: "QuizClass.Show;QuizClass.Run;"),
                 new Role(id: 3, name: "Instructor", role: RoleEnum.Instructor, allowedMethods: "QuizClass.Show;QuizClass.Run;QuizClass.Add;QuizClass.Delete;")
            };
        }

        /// <summary>
        /// Метод загружает набор ролей приложения из БД
        /// </summary>
        /// <returns></returns>
        public List<Role> LoadRoles()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));

            List<Role> newRolesList = new List<Role>(0);
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    command.CommandText = V_GetAllRoles; // see DALResources.resx,
 ///                    command.Parameters.Add(Db.CreateParameter("@rc", DbType.Int32, "-1", null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var roles = command.ExecuteReader())
                    {
                        while (roles.Read())
                        {
                            newRolesList.Add(new Role(
                                                        (int)roles[0],          // id
                                                        roles[1].ToString(),    // Name
                                                        (RoleEnum)roles[2],     // RoleFlag
                                                        roles[3].ToString()));  // AllowedMethods
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                newRolesList = null;
                Logger.Info(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            rolesList = newRolesList;
            return rolesList;
        }

        /// <summary>
        /// Реализация метода интерфейса
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Role Get(int id)
        {
            // мне стыдно, но пока я оставлю это здесь :(
            if (rolesList == null) rolesList = LoadRoles();
            return rolesList.Find(x => x.ID == id);
        }

        /// <summary>
        /// Реализация метода интерфейса
        /// </summary>
        /// <returns></returns>
        public List<Role> GetAll()
        {
            rolesList = LoadRoles();
            return rolesList;
        }

        /// <summary>
        /// Роли в режиме рид-онли
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Save(Role entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Роли в режиме рид-онли
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверим, допускает ли какая-либо роль пользователя выполнение указанного метода
        /// </summary>
        public bool CheckIsAllowed(Person person, string callingMethod)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            string callingMethodName = callingMethod + ";"; // последняя ; обязательна
            bool boolresult = false;
            foreach (RoleEnum role in Enum.GetValues(typeof(RoleEnum)))
            {
                // у персоны есть эта роль ?
                if (person.Role.HasFlag(role))
                {
                    // тогда поищу в списке методов
                    if (boolresult = rolesList.Find(x => x.RoleFlag == role).AllowedMethods.Contains(callingMethodName))
                        break;
                }
            }

            return boolresult;
        }
    }
}
