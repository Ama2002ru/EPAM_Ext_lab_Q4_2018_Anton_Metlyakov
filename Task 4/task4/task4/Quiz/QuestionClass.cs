namespace Task4
{
    using System;

    /// <summary>
    /// Класс описывает сущность - 1 вопрос теста.
    /// </summary>
    public class QuestionClass : VariantsClass
    {
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

        /// <summary>
        /// Добавление вопроса - для админки
        /// </summary>
        public new void Add()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Обновление вопроса - для админки
        /// </summary>
        public new void Update()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Удаление вопроса - для админки
        /// </summary>
        public new void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}
