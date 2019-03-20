namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using DAL;

    /// <summary>
    /// Класс хранит информацию об ответе на 1 вопрос теста
    /// </summary>
    public class AnswerModel
    {
        public AnswerModel()
        {
        }

        /// <summary>
        /// Ссылка на ID в таблице назначенных квизов пользователю
        /// </summary>
        public int QuizResult_Id { get; set; }

        /// <summary>
        /// Ссылка на ID в таблице ответов
        /// </summary>
        public int Answer_Id { get; set; }

        /// <summary>
        /// Ссылка на ID квиза в БД
        /// </summary>
        public int Quiz_Id { get; set; }

        /// <summary>
        /// Ссылка на ID вопроса в БД
        /// </summary>
        public int Question_Id { get; set; }

        /// <summary>
        /// Битовое поле ответов студента
        /// </summary>
        public int Answer_Flag { get; set; }

        /// <summary>
        ///  Сколько времени потребовалось на ответ - потом попробую рассчитать
        /// </summary>
        public DateTime? TimeStamp { get; set; }

        public static implicit operator AnswerModel(Answer a)
        {
            if (a == null) return null;
            var am = new AnswerModel();
            am.QuizResult_Id = a.QuizResult_Id;
            am.Answer_Id = a.Answer_Id;
            am.Question_Id = a.Question_Id;
            am.Quiz_Id = a.Quiz_Id;
            am.Answer_Flag = a.Answer_Flag;
            am.TimeStamp = a.TimeStamp;
            return am;
        }
    }
}