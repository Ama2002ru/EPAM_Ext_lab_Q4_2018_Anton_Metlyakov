namespace Task4
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
        Admin   = 0x1,
        Student = 0x2,
        Instructor = 0x4
    }
}
