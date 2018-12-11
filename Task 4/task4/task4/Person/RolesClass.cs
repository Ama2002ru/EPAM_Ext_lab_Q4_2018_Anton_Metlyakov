namespace Task4
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Описываю права доступа Роли
    /// </summary>
    public class RolesClass
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RolesClass()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Название роли
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Бит-маска ID роли
        /// </summary>
        public RoleEnum RoleFlag
        {
            get => default(RoleEnum);
            set
            {
            }
        }

        /// <summary>
        /// comma-separated string с именами разрешенных к исплнению 
        /// методов для данной роли в виде [Класс.Метод]
        /// </summary>
        private string AllowedMethods { get; set; }

        /// <summary>
        /// Проверка наличия роли у пользователя
        /// </summary>
        public bool IsAssignedRole(PersonClass person, RoleEnum role)
        {
            throw new System.NotImplementedException();
        }
               
        /// <summary>
        /// Проверим, допускают ли роли пользователя выполнение метода
        /// </summary>
        public void CheckIsAllowed(PersonClass person, string methodName)
        {
            throw new System.NotImplementedException();
        }
    }
}
