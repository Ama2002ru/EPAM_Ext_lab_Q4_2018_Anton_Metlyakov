namespace DAL.Person
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using DAL.Interfaces;
    using DAL.Utility;
    using log4net;

    public class PersonRepository : IBaseService<PersonClass>  
    {
        /// <summary>
        /// Список пользователей в системе
        /// </summary>
        private List<PersonClass> personList;

        /// <summary>
        /// default Constructor
        /// </summary>
        public PersonRepository()
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            personList = Init();
        }

        /// <summary>
        /// static method for testing
        /// </summary>
        /// <returns></returns>
        public List<PersonClass> Init()
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            return new List<PersonClass>
            {
                new PersonClass(), // default person "John Doe"
                new PersonClass(id: 1, firstname: "Igor", lastname: "Kalugin", username: "ki", password: "ki", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student),
                new PersonClass(id: 2, firstname: "Nikolay", lastname: "Piskarev", username: "np", password: "np", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student),
                new PersonClass(id: 3, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student)
            };
        }

        /// <summary>
        /// Создать пользователя (и сохранить в БД)
        /// new person ID = -1 !
        /// <returns>bool Success?</returns>
        /// </summary>
        public bool Add(PersonClass person)
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool boolresult = false;
            string errormessage; // в будущем доработаю использование сообщения
            try
            {
                if (!PersonValidator.IsValid(person, out errormessage)) return boolresult;
                if (boolresult = person.Save())

                    // Если сохранение успешно, надо перезапросить из БД список пользователей с правильными ID
                    // а пока просто добавлю в список
                    personList.Add(person);
            }
            catch (NullReferenceException ex)
            {
                Logger.Log.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }

            return boolresult;
        }

        /// <summary>
        /// Удалить пользователя, в т.ч. и из БД, id = person.ID
        /// <returns>bool Success?</returns>
        /// </summary>
        public bool Delete(int id)
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            string errormessage; // в будущем доработаю использование сообщения
            bool boolresult = false;
            try
            {
                int personListIndex = personList.FindIndex(p => p.ID == id);
                if (!PersonValidator.IsDeleteOK(personList[personListIndex], out errormessage)) return boolresult;
                if (boolresult = personList[personListIndex].Delete())

                    // Если удаление успешно, надо перезапросить из БД список пользователей
                    // а пока просто удалю из списка
                    personList.RemoveAt(personListIndex);
            }
            catch (ArgumentOutOfRangeException ex) 
            {
                boolresult = false;
                Logger.Log.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }

            return boolresult;
        }

        /// <summary>
        /// Вернуть из списка пользователя, id = person.ID
        /// </summary>
        /// <param name="id"> id = person.ID</param>
        /// <returns>Person instance, Null - что то пошло не так :(</returns>
        public PersonClass Get(int id)
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            try
            {
                return personList[personList.FindIndex(p => p.ID == id)];
            }
            catch (NullReferenceException ex)
            {
                Logger.Log.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return null;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Logger.Log.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Возвращает список всех пользователей системы
        /// </summary>
        /// <returns></returns>
        public List<PersonClass> GetAll()
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            List<PersonClass> copyOfPersonList = null;
            try
            {
                copyOfPersonList = new List<PersonClass>(personList);
            }
            catch (NullReferenceException ex)
            {
                Logger.Log.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }

            return copyOfPersonList;
        }

        /// <summary>
        ///  Записать в БД инфо о пользователе.Ожидаю что person.ID будет корректным и не проверяю его
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public bool Save(PersonClass person)
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool boolresult = false;
            string errormessage; // в будущем доработаю использование сообщения
            try
            {
                if (!PersonValidator.IsValid(person, out errormessage)) return boolresult;
                if (boolresult = person.Save())

                    // надо обновить инф. в списке пользователей
                    personList[personList.FindIndex(p => p.ID == person.ID)] = person;
            }
            catch (NullReferenceException ex)
            {
                Logger.Log.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }

            return boolresult;
        }

        /// <summary>
        /// Вывести информацию о всех пользователях
        /// </summary>
        public void ShowAll()
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            try
            {
                foreach (var person in personList)
                    person.Show();
            }
            catch (NullReferenceException ex)
            {
                Logger.Log.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }
    }
}
