namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DAL.Interfaces;
    using DAL.Person;

    /// <summary>
    /// Класс описывает тест как группу вопросов
    /// </summary>
    public class QuizClass : BaseRepositoryClass<QuestionClass>, IItem
    {
        /// <summary>
        ///  Уникальный номер квиза в БД
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Автор, квиза
        /// </summary>
        public PersonClass Author { get; set; }
     
        /// <summary>
        /// Дата создания квиза
        /// </summary>
        public DateTime CreatedDate { get; set; }
  
        /// <summary>
        /// Название 
        /// </summary>
        public string QuizName { get; set; }

        /// <summary>
        /// % правильных ответов для зачета квиза
        /// </summary>
        public float PassRate { get; }

        /// <summary>
        /// Сохранение/добавление теста - для админки
        /// как я понимаю эти методы - заявлены в интефейсе IItem, а реализуются 
        /// каждым классом совершенно уникально
        /// - работают с БД, синхронные.
        /// методы *.Refresh() вынесу на уровень выше.
        /// </summary>
        public bool Save()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Удаление теста - для админки
        /// </summary>
        public bool Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}
