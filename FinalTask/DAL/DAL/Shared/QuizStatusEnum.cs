namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// Возможные статусы тестов, назначенных студенту
    /// </summary>
    public enum QuizStatusEnum
    {
        /// <summary>
        /// эти атрибуты для корректного считывания типа инт из БД
        /// </summary>
        [XmlEnum(Name = "0")]
        None = 0,
        [XmlEnum(Name = "1")]
        Assigned = 1,   // инструктор назначил тест студенту
        [XmlEnum(Name = "2")]
        InProgress = 2, // в данный момент студент проходит тест
        [XmlEnum(Name = "3")]
        Passed = 3,     // успешно пройден
        [XmlEnum(Name = "4")]
        Failed = 4      // тест не взят
    }
}
