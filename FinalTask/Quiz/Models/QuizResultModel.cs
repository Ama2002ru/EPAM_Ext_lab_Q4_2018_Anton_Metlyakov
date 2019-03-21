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
    /// Класс хранит тест, назначенный студенту (только статус меняется)
    /// </summary>
    public class QuizResultModel 
    {
        public QuizResultModel()
        {
            Answer_List = new List<AnswerModel>(0);
        }

        /// <summary>
        /// Для работы с БД
        /// </summary>
        public int QuizResult_Id { get; set; }

        /// <summary>
        /// собственно ID студента.
        /// </summary>
        public int User_Id { get; set; }
        
        /// <summary>
        /// собственно имя студента.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Для работы с БД
        /// </summary>
        public int Quiz_Id { get; set; }

        /// <summary>
        /// название теста
        /// </summary>
        [Display(Name = "Quiz Name")]
        public string Quiz_Name { get; set; }

        /// <summary>
        /// Person, назначивший тест студенту
        /// </summary>
        [Display(Name = "Assigned By")]
        public string AssignedBy { get; set; }

        /// <summary>
        /// дата назначения
        /// </summary>
        [Display(Name = "Assign Date")]
        public DateTime? Assigned_Date { get; set; }

        /// <summary>
        ///  дата прохождения
        /// </summary>
        [Display(Name = "Completed Date")]
        public DateTime? Completed_Date { get; set; }

        [Display(Name = "Time Taken")]
        public TimeSpan? Time_Taken { get; set; }

        /// <summary>
        /// сохраню состояние теста - назначен/пройден/провален ...
        /// </summary>
        [Display(Name = "Quiz Status")]
        public QuizStatusEnum QuizResult_Status { get; set; }

        /// <summary>
        /// Рейт, полученный в результате прохождения теста
        /// </summary>
        [Display(Name = "User Rate")]
        public float? Completed_Rate { get; set; }

        /// <summary>
        /// список/массив ? ответов на каждый вопрос теста
        /// </summary>
        public List<AnswerModel> Answer_List { get; set; }

        /// <summary>
        /// Преобразование типа QuizResult в QuizResultModel
        /// </summary>
        /// <param name="qr"></param>
        public static implicit operator QuizResultModel(QuizResult qr)
        {
            if (qr == null) return null;
            var qrm = new QuizResultModel();
            qrm.User_Id = qr.User_Id;
            qrm.UserName = qr.UserName;
            qrm.QuizResult_Id = qr.QuizResult_Id;
            qrm.Quiz_Id = qr.Quiz_Id;
            qrm.Quiz_Name = qr.Quiz_Name;
            qrm.AssignedBy = qr.AssignedBy;
            qrm.Assigned_Date = qr.Assigned_Date;
            qrm.Completed_Date = qr.Completed_Date;
            qrm.QuizResult_Status = qr.QuizResult_Status;
            qrm.Completed_Rate = qr.Completed_Rate;
            qrm.Time_Taken = qr.Time_Taken;
            qrm.Answer_List = new List<AnswerModel>(0);
            foreach (var q in qr.Answer_List)
                qrm.Answer_List.Add(q);
            return qrm;
        }
    }
}