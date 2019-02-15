namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DAL;
 
    /// <summary>
    /// Класс описывает тест как группу вопросов
    /// </summary>
    public class Quiz : IItem
    {
        /// <summary>
        /// Список/массив сущностей класса Question
        /// </summary>
        public List<QuestionClass> Questions { get; set; }

        public int ID { get; set; }

        public QuestionClass QuestionClass
        {
            get => default(QuestionClass);
            set
            {
            }
        }

        /// <summary>
        /// Автор, создавший тест
        /// </summary>
        public Person Author { get; set; }
     
        /// <summary>
        /// Дата создания теста
        /// </summary>
        public DateTime CreatedDate { get; set; }
  
        /// <summary>
        /// Название теста
        /// </summary>
        public string QuizName { get; set; }

        /// <summary>
        /// Сохранение/добавление теста - для админки
        /// </summary>
        public bool Save(IdbConnector db)
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

        /// <summary>
        /// Удаление теста - для админки
        /// </summary>
        public void Show()
        {
            throw new System.NotImplementedException();
        }
    }
}
