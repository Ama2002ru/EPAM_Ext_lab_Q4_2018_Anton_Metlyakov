namespace Task4
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Класс, описывающий пользователя системы
    /// </summary>
    public class PersonClass
    {
        /// <summary>
        /// имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// назначенные и выполненные тесты
        /// </summary>
        public WorkBookClass WorkBook { get; set; }

        /// <summary>
        /// Логин в системе
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// имя пользователя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// пароль пользователя
        /// </summary>
        public string HashedPassword { get; set; }

        /// <summary>
        /// назначенные роли пользователя
        /// </summary>
        public RolesClass RolesFlag
        {
            get => default(RolesClass);
            set
            {
            }
        }
    }
}
