namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;

    /// <summary>
    /// Модель для отображения вопроса квиза
    /// </summary>
    public class QuestionModel
    {
        public int Question_Id { get; set; }

        public int Quiz_Id { get; set; }

        public string Info { get; set; }

        public string Text { get; set; }

        // Основное поле, где храню ответы
        public int CorrectOptionFlag { get; set; }

        // массив вариантов ответа 
        // имею поля Text - просто строка
        // Value - попробую хранить состояние checkbox'a
        public Variant[] Options { get; set; }

        /// <summary>
        /// Введу свойство ниже для прохождения теста
        /// </summary>
        public int QuizResult_Id { get; set; }

        public static implicit operator QuestionModel(Question q)
        {
            if (q == null) return null;
            var qm = new QuestionModel();
            qm.Quiz_Id = q.Quiz_Id;
            qm.Question_Id = q.Question_Id;
            qm.Info = q.Info;
            qm.Text = q.Text;
            qm.CorrectOptionFlag = q.CorrectOptionFlag;
            int len = q.Options != null ? q.Options.Length : 0;
            qm.Options = new Variant[len];
            for (int i = 0; i < len; i++)
                qm.Options[i] = q.Options[i];
            return qm;
         }
    }
}