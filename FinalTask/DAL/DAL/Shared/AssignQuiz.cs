namespace DAL
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Класс для View назначения квизов пользователю
    /// </summary>
    public class AssignQuiz
    {
        /// <summary>
        /// поле для назнчачения нового квиза
        /// </summary>
        public int User_Id { get; set; }

        /// <summary>
        /// поле для указания того, кто назначил квиз
        /// </summary>
        public int AssignedUser_Id { get; set; }

        /// <summary>
        /// имя текущего пользователя
        /// </summary>
        public string User_Name { get; set; }

        /// <summary>
        /// список квизов пользователя - все квизы в системе + поле Quiz_Status
        /// </summary>
        public List<AssignQuizList> Assignquizlist { get; set; }
    }
}
