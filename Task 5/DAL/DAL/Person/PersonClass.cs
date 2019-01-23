namespace DAL.Person
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using DAL.Utility;
  
    /// <summary>
    /// Класс, описывающий пользователя системы
    /// </summary>
    // ki. Считается дурным тоном дублировать тип в названии- например PersonList (вместо этого лучше Persons) или PersonClass. Ведь итак видно, что это
    // класс.
    public class PersonClass
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public PersonClass()
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            ID = 0;
            FirstName = "John";
            LastName = "Doe";
            Username = "JDoe";
            HashedPassword = "123";
            WorkBook = null;
            Role = RoleEnum.None;
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public PersonClass(int id, string firstname, string lastname, string username, string password, WorkBookClass workbook, RoleEnum role)
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            ID = id;
            FirstName = firstname;
            LastName = lastname;
            Username = username;
            HashedPassword = password;
            WorkBook = workbook;
            Role = role;
        }

        /// <summary>
        /// ID пользователя. В норме равен ID в БД. У ново-добавленного пользователя ожидаю ID = -1
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Логин в системе. Буду отслеживать его уникальность при добавлении/сохранении пользователя
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// имя пользователя
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Фамилия (?) пользователя
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// все назначенные (и возможно уже выполненные) тесты
        /// </summary>
        public WorkBookClass WorkBook { get; }

        /// <summary>
        /// назначенные роли пользователя
        /// </summary>
        public RoleEnum Role { get; }

        /// <summary>
        /// пароль пользователя
        /// </summary>
        private string HashedPassword { get; set; }

        /// <summary>
        /// Удаление текущего пользователя в БД
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));

            // there will be ADO.NET implementation;
            return true;
        }

        /// <summary>
        /// Проверка наличия роли у пользователя - Student, Admin и т.д.
        /// </summary>
        // ki. в таком случае проще использовать вычисляемое свойство.
        public bool IsAssignedRole(RoleEnum testrole)
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            return (Role & testrole) != 0;
        }

        /// <summary>
        /// Сохранение изменений текущего/добавление нового пользователя в БД
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));

            // there will be ADO.NET implementation;
            return true;
        }
        
        /// <summary>
        /// Вывести инфо о  пользователе
        /// </summary>
        /// <returns></returns>
        public void Show()
        {
            Logger.Log.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", ID, FirstName, LastName, Username, Role.ToString());
        }
    }
}