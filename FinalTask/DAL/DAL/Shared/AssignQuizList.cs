namespace DAL
{ 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Подкласс для View назначения квизов пользователю
    /// список квизов у пользователя - все квизы в системе + поле Quiz_Status
    /// </summary>
    public class AssignQuizList
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
