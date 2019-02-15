namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DAL;

    /// <summary>
    /// Класс хранит результат прохождения теста, 
    /// в том числе и назначенного (ещё не пройденного) теста
    /// думаю что не буду связывать с QuizCollection по ID теста
    /// </summary>
    public class QuizResultClass
    {
        /// <summary>
        /// список/массив ? ответов на каждый вопрос теста
        /// </summary>
        public AnswerClass AnswerList { get; set; }

        /// <summary>
        /// дата тестирования
        /// </summary>
        public DateTime ResultDate { get; set; }

        /// <summary>
        /// Название теста
        /// </summary>
        public string QuizName { get; set; }

        /// <summary>
        /// Статус прохождения теста
        /// </summary>
        public QuizStatusEnum QuizStatus { get; set; }

        /// <summary>
        /// Количество верных ответов для зачета
        /// </summary>
        public int SuccessRate { get; set; }

        /// <summary>
        /// Дата назначения теста студенту
        /// </summary>
        public DateTime AssignedDate { get; set; }
 
        /// <summary>
        /// Person, назначивший тест студенту
        /// </summary>
        public DAL.Person AssignedBy { get; set; }

        public QuizStatusEnum QuizStatusEnum { get; set; }

        /// <summary>
        /// Сохранение информации в БД 
        /// </summary>
        public void Commit()
        {
            throw new System.NotImplementedException();
        }
    }
}
