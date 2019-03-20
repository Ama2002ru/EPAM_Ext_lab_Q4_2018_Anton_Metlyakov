namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Класс для передачи списка пользователей/ID пользователя  и списка квизов в форму прсмотра назначенных квизов
    /// для назначения новых квизов
    /// </summary>
    public class MyQuizesIndexModel
    {
        /// <summary>
        /// для выбора из списка пользователей
        /// </summary>
        [Display(Name = "Select user:")]
        public SelectList Users { get; set; }

        public int User_id { get; set; }

        public List<QuizResultModel> List { get; set; }
    }
}