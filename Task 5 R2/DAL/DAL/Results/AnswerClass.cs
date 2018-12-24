namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DAL.Interfaces;
    
    /// <summary>
    /// Класс хранит информацию об ответе на 1 вопрос теста
    /// </summary>
    public class AnswerClass : BaseRepositoryClass<VariantsClass>, IItem
    {   
        /// <summary>
        /// ID ответа студента на вопрос
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Битовое поле ответов студента
        /// </summary>
        public int AnswerFlag { get; set; }

        /// <summary>
        ///  Сколько времени потребовалось на ответ
        /// </summary>
        public TimeSpan TimeElapsed { get; set; }

        /// <summary>
        /// Фиксирую потерю фокуса на вопросе
        /// </summary>
        public bool IsFocusLost { get; set; }

        /// <summary>
        /// думаю, что удаление ответа студента не должно быть типовым поведением приложения ?
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            return false;
        }

        /// <summary>
        /// Сохранить ответ в БД
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            // для сохранения ответа студента имеем Variants.ID, QuestionID в List<VariantsClass> ItemList
            throw new NotImplementedException();
        }
    }
}
