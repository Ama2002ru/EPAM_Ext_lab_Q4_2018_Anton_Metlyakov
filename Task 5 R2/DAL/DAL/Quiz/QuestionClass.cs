namespace DAL
{
    using System;
    using DAL.Interfaces;

    /// <summary>
    /// Класс описывает сущность - 1 вопрос теста.
    /// </summary>
    public class QuestionClass : BaseRepositoryClass<VariantsClass>, IItem
    {
        /// <summary>
        /// Уникальный номер вопроса в БД
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Преамбула вопроса, если потребуется
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Текст самого вопроса
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// количество попыток ответа. Думаю что всегда будет =1
        /// </summary>
        public int NoOfTries { get; set; }

        // Массив вариантов ответов из 1 элемента List<T> ItemList
        // объявлен в BaseRepositoryClass

        /// <summary>
        /// Удаление вопроса - для админки
        /// </summary>
        public bool Delete()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Добавление/изменение вопроса - для админки
        /// </summary>
        public bool Save()
        {
            throw new System.NotImplementedException();
        }
    }
}
