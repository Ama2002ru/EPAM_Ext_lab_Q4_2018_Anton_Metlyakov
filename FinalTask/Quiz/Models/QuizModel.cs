namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;

    /// <summary>
    /// Класс описывает Тест как совокупность вопросов
    /// </summary>
    public class QuizModel
    {
        public int Quiz_Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Quiz Name length should be between 2 to 100 symbols")]
        [Display(Name = "Quiz Name")]
        public string Name { get; set; }

        public int Author_Id { get; set; }

        public string Author { get; set; }

        public DateTime Creation_Date { get; set; }

        [Required]
        [Range(0.1, 1.0, ErrorMessage = "Success rate must be in range (0,1]")]
        [Display(Name = "Quiz pass success rate ")]
        [DisplayFormat(DataFormatString = "{0:F4}")]
        public float Success_Rate { get; set; }

        public List<QuestionModel> Questions { get; set; }

        /// <summary>
        /// Упрощу код - приведу объекты Quiz -> QuizModel
        /// </summary>
        /// <param name="q"></param>
        public static implicit operator QuizModel(Quiz q)
        {
            if (q == null) return null;
            var quiz = new QuizModel();
            quiz.Quiz_Id = q.ID;
            quiz.Name = q.Quiz_Name;
            quiz.Success_Rate = q.Success_Rate;
            quiz.Author_Id = q.Author_Id;
            quiz.Author = q.Author;
            quiz.Creation_Date = q.Created_Date;
            quiz.Questions = new List<QuestionModel>(0);
            foreach (var question in q.Questions)
                quiz.Questions.Add(question);
            return quiz;
        }
    }
}