namespace DAL.Person
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using DAL.Utility;

    /// <summary>
    /// класс, содержащий метод проверки инф о пользователе
    /// </summary>
    public static class PersonValidator
    {
        /// <summary>
        /// Логика проверки введенной инф. о пользователе
        /// </summary>
        /// <param name="person">Класс персона</param>
        /// <param name="">сообщение об ошибке</param>
        /// <returns>результат, прошла проверка или нет</returns>
        public static bool IsValid(PersonClass person, out string message)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool boolresult = false;
            string strresult = "Person is Ok";

            // Надо бы проверить на ID + совпадение логинов в БД!, длин имен и все такое...
            boolresult = true;
            message = strresult;
            return boolresult;
        }

        /// <summary>
        /// Проверка, разрешу ли удалять пользователя
        /// </summary>
        /// <param name="person">Класс персона<</param>
        /// <param name="">сообщение, причина отказа</param>
        /// <returns>результат, прошла проверка или нет</returns>
        public static bool IsDeleteOK(PersonClass person, out string message)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool boolresult = false;
            string strresult = "Delete is Ok";

            // в будущем проверю - 
            // а) это последний админ ?
            // б) это автор тестов ?
            boolresult = true;
            message = strresult;
            return boolresult;
        }
    }
}
