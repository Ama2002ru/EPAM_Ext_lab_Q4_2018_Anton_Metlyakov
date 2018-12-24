namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DAL.Interfaces;

    /// <summary>
    /// Список всех тестов студента
    /// </summary>
    public class WorkBookClass : BaseRepositoryClass<QuizResultClass>
    {
        /// <summary>
        ///  дата  начала ведения "тетради" 
        /// </summary>
        public DateTime DateStarted { get; set; }

        // ItemList в базовом классе
    }
}
