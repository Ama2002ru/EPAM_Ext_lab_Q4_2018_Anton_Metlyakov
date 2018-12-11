namespace Task4
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Статусы тестов, назначенных студенту
    /// </summary>
    public enum QuizStatusEnum
    {
        None = 0,
        Assigned = 1,   // учитель помещает тест в "тетрадь" студента
        InProgress = 2, // в данный момент студент проходит тест
        Passed = 3,     // успешно пройден
        Failed = 4      // тест не взят
    }
}
