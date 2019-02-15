namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using DAL;

    /// <summary>
    /// Описываю права доступа Роли
    /// </summary>
    public class Roles
    { 
        /// <summary>
        /// Статический список ролей в системе
        /// </summary>
        private static readonly Lazy<List<Roles>> RolesList = new Lazy<List<Roles>>(() => Init());

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// 
        public Roles()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            ID = 0;
            Name = "No access";
            RoleFlag = RoleEnum.None;
            AllowedMethods = "Test.Test;";
        }

        /// <summary>
        /// Конструктор со всеми параметрами
        /// </summary>
        public Roles(int id, string name, RoleEnum role, string allowedMethods)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            ID = id;
            Name = name;
            RoleFlag = role;
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
        public static List<Roles> Init()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            return new List<Roles>
            {
                 new Roles(id: 0, name: "No access", role: RoleEnum.None, allowedMethods: "Test.Test;"), // последняя ";" обязательна
                 new Roles(id: 1, name: "Admin", role: RoleEnum.Admin, allowedMethods: "PersonRepository.Add;PersonRepository.Delete;"), // последняя ";" обязательна
                 new Roles(id: 2, name: "Student", role: RoleEnum.Student, allowedMethods: "QuizClass.Show;QuizClass.Run;"),
                 new Roles(id: 3, name: "Instructor", role: RoleEnum.Instructor, allowedMethods: "QuizClass.Show;QuizClass.Run;QuizClass.Add;QuizClass.Delete;")
            }; 
        }

        /// <summary>
        /// Проверим, допускает ли какая-либо роль пользователя выполнение указанного метода
        /// </summary>
        public static bool CheckIsAllowed(Person person, string callingMethod)
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
                    if (boolresult = RolesList.Value.Find(x => x.RoleFlag == role).AllowedMethods.Contains(callingMethodName))
                        break;
                }
            }

            return boolresult;
        }
    }
}
