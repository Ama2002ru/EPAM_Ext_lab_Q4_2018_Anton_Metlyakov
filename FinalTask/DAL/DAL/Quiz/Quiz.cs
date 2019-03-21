namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml.Serialization;
 
    /// <summary>
    /// Класс описывает тест как группу вопросов
    /// </summary>
    public class Quiz 
    {
        public Quiz()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Параметризованный конструктор
        /// </summary>
        public Quiz(int id, string name, int author_id, string author, DateTime created_date, float success_rate, List<Question> questions)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            ID = id;
            Author_Id = author_id;
            Author = author;
            Quiz_Name = name;
            Created_Date = created_date;
            Success_Rate = success_rate;
            Questions = questions;
        }

        /// <summary>
        /// Список/массив сущностей класса Question
        /// </summary>
        public List<Question> Questions { get; set; }

        public int ID { get; set; }

         /// <summary>
        /// Автор, создавший тест
        /// </summary>
        public int Author_Id { get; set; }

        /// <summary>
        /// Автор, создавший тест
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Дата создания теста
        /// </summary>
        [XmlElement("Created_Date", typeof(DateTime))]
        public DateTime Created_Date { get; set; }
  
        /// <summary>
        /// Название теста
        /// </summary>
        public string Quiz_Name { get; set; }

        /// <summary>
        /// проходной коэффициент
        /// </summary>
        public float Success_Rate { get; set; }
    }
}
