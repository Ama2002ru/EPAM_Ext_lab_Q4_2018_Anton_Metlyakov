namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using DAL.Interfaces;
    using DAL.Utility;
    using log4net;

    /// <summary>
    /// Класс содержит список со всеми тестами в системе
    /// </summary>
    public class QuizRepository : BaseRepositoryClass<QuizClass>
    {
        /// <summary>
        /// default Constructor
        /// </summary>
        public QuizRepository()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            this.ItemList = new List<QuizClass>(0);
        }

/*
        /// <summary>
        /// method for testing
        /// </summary>
        /// <returns></returns>
        public void Init()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            this.ItemList = new List<QuizClass>
            {
                new PersonClass(), // default person "John Doe"
                new PersonClass(id: 1, firstname: "Igor", lastname: "Kalugin", username: "ki", password: "ki", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student),
                new PersonClass(id: 2, firstname: "Nikolay", lastname: "Piskarev", username: "np", password: "np", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student),
                new PersonClass(id: 3, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student)
            };
        }
*/
    }
}
