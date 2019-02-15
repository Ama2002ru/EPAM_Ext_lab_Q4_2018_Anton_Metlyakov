namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Список всех тестов студента
    /// </summary>
    public class WorkBookClass
    {
        /// <summary>
        ///  дата  начала ведения "тетради"
        /// </summary>
        public DateTime DateStarted { get; set; }

        /// <summary>
        /// Список объектов-тестов (или массив ?)
        /// </summary>
        public QuizResultClass QuizResultList { get; set; }
    }
}
