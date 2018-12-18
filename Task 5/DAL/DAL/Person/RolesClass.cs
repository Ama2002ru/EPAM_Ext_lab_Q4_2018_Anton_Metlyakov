namespace DAL.Person
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using DAL.Interfaces;
    using DAL.Utility;

    /// <summary>
    /// Описываю права доступа Роли
    /// </summary>
    public class RolesClass
    { 
        /// <summary>
        /// Статический список ролей в системе
        /// </summary>
        private static readonly Lazy<List<RolesClass>> Roles = new Lazy<List<RolesClass>>(() => Init());

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// 
        public RolesClass()
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            ID = 0;
            Name = "No access";
            RoleFlag = RoleEnum.None;
            AllowedMethods = "Test.Test;";
        }

        /// <summary>
        /// Конструктор со всеми параметрами
        /// </summary>
        public RolesClass(int id, string name, RoleEnum roles, string allowedMethods)
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            ID = id;
            Name = name;
            RoleFlag = roles;
            AllowedMethods = allowedMethods;
        }

        /// <summary>
        /// Название роли
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// comma-separated string с именами разрешенных к исплнению 
        /// методов для данной роли в виде Класс.Метод;
        /// </summary>
        public string AllowedMethods { get; }

        /// <summary>
        /// Бит-флаг роли
        /// </summary>
        private RoleEnum RoleFlag { get; set; }

        /// <summary>
        /// Номер роли -пока не нужен
        /// </summary>
        private int ID { get; set; }

        /// <summary>
        /// Метод создает набор ролей приложения, пока не реализован ADO.NET коннектор
        /// </summary>
        /// <returns></returns>
        public static List<RolesClass> Init()
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            return new List<RolesClass>
            {
                            new RolesClass(),
                            new RolesClass(id: 1, name: "Admin", roles: RoleEnum.Admin, allowedMethods: "PersonRepository.Add;PersonRepository.Delete;"), // последняя ";" обязательна
                            new RolesClass(id: 2, name: "Student", roles: RoleEnum.Student, allowedMethods: "QuizClass.Show;QuizClass.Run;"),
                            new RolesClass(id: 3, name: "Instructor", roles: RoleEnum.Instructor, allowedMethods: "QuizClass.Show;QuizClass.Run;QuizClass.Add;QuizClass.Delete;")
            }; 
        }

        /// <summary>
        /// Проверим, допускает ли какая-либо роль пользователя выполнение указанного метода
        /// </summary>
        public static bool CheckIsAllowed(PersonClass person, string callingMethod)
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            string callingMethodName = callingMethod + ";"; // последняя ; обязательна
            bool boolresult = false;
            foreach (RoleEnum role in Enum.GetValues(typeof(RoleEnum)))
            {   
                // у персоны есть эта роль ?
                if (person.Role.HasFlag(role)) 
                {
                    // тогда поищу в списке методов
                    if (boolresult = Roles.Value.Find(x => x.RoleFlag == role).AllowedMethods.Contains(callingMethodName))
                        break;
                }
            }

            return boolresult;
        }
    }
}
