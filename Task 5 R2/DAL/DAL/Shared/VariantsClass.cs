namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DAL.Interfaces;

    /// <summary>
    /// Класс описывает варианты ответов и правильный ответ(ы) на вопрос 
    /// </summary>
    public class VariantsClass : IItem
    {
        /// <summary>
        /// ID варианта ответа БД
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Ссылка на ID вопроса в БД - В БД надо, а надо ли иметь это поле здесь? - да для ответов студента
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
        /// Удаление блока ответов к вопросу теста. пока не понимаю механизма реализации.
        /// скорей всего этод метод будет переопределен в QuestionClass
        /// </summary>
        public bool Delete()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Добавление/изменение блока ответов к вопросу теста. пока не понимаю механизма реализации.
        /// скорей всего этод метод будет переопределен в QuestionClass
        /// </summary>
        public bool Save()
        {
            throw new System.NotImplementedException();
        }

        public void Show()
        {
            throw new System.NotImplementedException();
        }
    }
}
