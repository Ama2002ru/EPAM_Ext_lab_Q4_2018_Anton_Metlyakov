namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Класс описывает варианты ответов и правильный ответ(ы) на вопрос 
    /// </summary>
    public class VariantsClass
    {
        /// <summary>
        /// Ссылка на ID вопроса в БД
        /// </summary>
        public int QuestionID { get; set; }

        /// <summary>
        ///  Массив вариантов ответов в human-readable виде
        /// </summary>
        public string[] Options { get; set; }

        /// <summary>
        /// Битовое поле - правильные ответы. 0х1 -1й, 0х2 - 2й, 0х4 -3й и т.д.
        /// </summary>
        public int CorrectOptionBits { get; set; }

        /// <summary>
        /// Добавление нового блока ответов к вопросу теста. пока не понимаю механизма реализации.
        /// скорей всего этод метод будет переопределен в QuestionClass
        /// </summary>
        public void Add()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Изменение блока ответов к вопросу теста. пока не понимаю механизма реализации.
        /// скорей всего этод метод будет переопределен в QuestionClass
        /// </summary>
        public void Update()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Удалить блок ответов
        /// </summary>
        public void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}
