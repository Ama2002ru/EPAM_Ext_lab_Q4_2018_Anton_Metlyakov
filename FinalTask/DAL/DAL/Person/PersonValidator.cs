namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using DAL;

    /// <summary>
    /// класс, содержащий метод проверки инф о пользователе
    /// </summary>
    public class PersonValidator
    {
        private IPersonRepository personRepository;

        public PersonValidator(IPersonRepository repo)
        {
            personRepository = repo;
        }

        /// <summary>
        /// Логика проверки введенной инф. о пользователе
        /// </summary>
        /// <param name="person">Класс персона</param>
        /// <param name="">сообщение об ошибке</param>
        /// <returns>результат, прошла проверка или нет</returns>
        public bool IsValid(Person person, out string message)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool boolresult = true;
            message = "Person is Ok";
            var persons2 = personRepository.GetAll();
            var personfound = personRepository.GetAll().Find(x => (x.UserName.ToLower() == person.UserName.ToLower()));

            // Надо бы проверить на ID + совпадение логинов в БД!, длин имен и все такое...
            if ((personfound != null) && (person.ID != personfound.ID))
            {
                // Новый person, коллизия Username
                boolresult = false;
                message = "Username already used! Choose another!";
            }

            return boolresult;
        }

        /// <summary>
        /// Проверка, разрешу ли удалять пользователя
        /// </summary>
        /// <param name="person">Класс персона<</param>
        /// <param name="">сообщение, причина отказа</param>
        /// <returns>результат, прошла проверка или нет</returns>
        public bool IsDeleteOK(Person person, out string message)
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
