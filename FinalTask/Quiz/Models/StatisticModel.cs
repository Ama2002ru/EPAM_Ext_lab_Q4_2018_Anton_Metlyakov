namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;

    /// <summary>
    /// Класс для главного Вью статистики
    /// </summary>
    public class StatisticModel
    {
        /// <summary>
        /// для выбора из списка
        /// </summary>
        public SelectList Quizes { get; set; }

        /// <summary>
        /// для выбора из списка
        /// </summary>
        public SelectList Users { get; set; }

        /// <summary>
        /// выбрали ли кого-то на форме ?
        /// </summary>
        public int? Selected_Quiz_Id { get; set; }

        public int? Selected_User_Id { get; set; }
    }
}