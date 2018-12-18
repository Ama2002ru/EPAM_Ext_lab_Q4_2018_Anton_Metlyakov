namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DAL.Person;

    /// <summary>
    /// Класс описывает тест как группу вопросов
    /// </summary>
    public class QuizClass
    {
        /// <summary>
        /// Список/массив сущностей класса Question
        /// </summary>
        public List<QuestionClass> Questions { get; set; }

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
        public PersonClass Author { get; set; }
     
        /// <summary>
        /// Дата создания теста
        /// </summary>
        public DateTime CreatedDate { get; set; }
  
        /// <summary>
        /// Название теста
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Добавление теста - для админки
        /// </summary>
        public void Add()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Обновление теста - для админки
        /// </summary>
        public void Update()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Удаление теста - для админки
        /// </summary>
        public void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}
