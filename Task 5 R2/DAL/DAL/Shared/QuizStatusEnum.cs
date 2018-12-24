namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Статусы квизов, назначенных студенту
    /// </summary>
    public enum QuizStatusEnum
    {
        None = 0,
        Assigned = 1,   // новый квиз в "тетради" студента
        InProgress = 2, // в данный момент студент проходит квиз
        Passed = 3,     // квиз успешно пройден
        Failed = 4      // квиз "не взят"
    }
}
