namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Группы доступа, которые могут присваиваться пользователю
    /// </summary>
    [Flags]
    public enum RoleEnum
    {
        Student = 0x1,
        Instructor = 0x2,
        Admin = 0x4
    }
}
