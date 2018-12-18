namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    
    /// <summary>
    /// Класс хранит информацию об ответе на 1 вопрос теста
    /// </summary>
    public class AnswerClass : VariantsClass
    {
        /// <summary>
        /// Битовое поле ответов студента
        /// </summary>
        public int AnswerFlag { get; set; }

        /// <summary>
        ///  Сколько времени потребовалось на ответ
        /// </summary>
        public DateTime TimeElapsed { get; set; }

        /// <summary>
        /// Фиксирую потерю фокуса на вопросе
        /// </summary>
        public bool IsFocusLost { get; set; }
    }
}
