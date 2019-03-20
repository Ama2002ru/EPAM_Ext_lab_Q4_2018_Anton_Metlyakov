namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using DAL;

    /// <summary>
    /// Класс для формы назначения квизов пользователю
    /// </summary>
    public class AssignQuizModel
    {
        /// <summary>
        /// собственно сам пользователь
        /// </summary>
        public int User_Id { get; set; }

        [Display(Name = "User name")]
        public string User_Name { get; set; }

        /// <summary>
        /// список квизов для пользователя
        /// </summary>
        public List<AssignQuizListModel> Assignquizlistmodel { get; set; }

        /// <summary>
        /// присваивания классов из DAL и View, для упрощения основного кода
        /// </summary>
        /// <param name="aqm"></param>
        public static implicit operator AssignQuizModel(AssignQuiz aq)
        {
            if (aq == null) return null;
            var aqm = new AssignQuizModel();
            aqm.User_Id = aq.User_Id;
            aqm.User_Name = aq.User_Name;
            aqm.Assignquizlistmodel = new List<AssignQuizListModel>(aq.Assignquizlist.Count);
            foreach (var assquiz in aq.Assignquizlist)
            {
                aqm.Assignquizlistmodel.Add(new AssignQuizListModel()
                {
                    QuizResult_Id = assquiz.QuizResult_Id,
                    Quiz_Id = assquiz.Quiz_Id,
                    Quiz_Name = assquiz.Quiz_Name,
                    Quiz_Status = assquiz.Quiz_Status
                });
            }

            return aqm;
        }

        /// <summary>
        /// присваивания классов из DAL и View, для упрощения основного кода
        /// </summary>
        /// <param name="aqm"></param>
        public static implicit operator AssignQuiz(AssignQuizModel aqm)
        {
            if (aqm == null) return null;
            var aq = new AssignQuiz();
            aq.User_Id = aqm.User_Id;
            aq.User_Name = aqm.User_Name;
            var assignquizlist = new List<AssignQuizList>(aqm.Assignquizlistmodel.Count);
            foreach (var assquiz in aqm.Assignquizlistmodel)
            {
                assignquizlist.Add(new AssignQuizList()
                {
                    QuizResult_Id = assquiz.QuizResult_Id,
                    Quiz_Id = assquiz.Quiz_Id,
                    Quiz_Name = assquiz.Quiz_Name,
                    Quiz_Status = assquiz.Quiz_Status
                });
            }

            return aq;
        }

        public class AssignQuizListModel
        {
            public int Quiz_Id { get; set; }

            public string Quiz_Name { get; set; }

            public QuizStatusEnum Quiz_Status { get; set; }

            /// <summary>
            /// для занесения существующих назначений - выберу из БД и ID
            /// </summary>
            public int QuizResult_Id { get; set; }
        }
    }
}