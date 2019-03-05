namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using System.Text;
    using DAL;

    /// <summary>
    /// Описываю права доступа Роли
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// 
        [Obsolete]
        public Role()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            ID = 1;
            Name = "Student";
            RoleFlag = RoleEnum.Student;
            AllowedMethods = "QuizClass.Show;QuizClass.Run;";
        }

        /// <summary>
        /// Конструктор со всеми параметрами
        /// </summary>
        public Role(int id, string name, RoleEnum role, string allowedMethods)
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
        public string Name { get; }

        /// <summary>
        /// comma-separated string с именами разрешенных к исплнению 
        /// методов для данной роли в виде Класс.Метод;
        /// </summary>
        public string AllowedMethods { get; }

        /// <summary>
        /// Бит-флаг роли
        /// </summary>
        public RoleEnum RoleFlag { get; }

        /// <summary>
        /// Номер роли
        /// </summary>
        public int ID { get; }
    }
}
