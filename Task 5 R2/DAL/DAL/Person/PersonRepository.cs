namespace DAL.Person
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using DAL.Interfaces;
    using DAL.Utility;
    using log4net;

    public class PersonRepository : BaseRepositoryClass<PersonClass>  
    {
        /// <summary>
        /// default Constructor
        /// </summary>
        public PersonRepository()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            this.ItemList = new List<PersonClass>(0);
        }

        /// <summary>
        /// method for testing
        /// </summary>
        /// <returns></returns>
        public void Init()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            this.ItemList = new List<PersonClass>
            {
                new PersonClass(), // default person "John Doe"
                new PersonClass(id: 1, firstname: "Igor", lastname: "Kalugin", username: "ki", password: "ki", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student),
                new PersonClass(id: 2, firstname: "Nikolay", lastname: "Piskarev", username: "np", password: "np", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student),
                new PersonClass(id: 3, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student)
            };
        }
    }
}
