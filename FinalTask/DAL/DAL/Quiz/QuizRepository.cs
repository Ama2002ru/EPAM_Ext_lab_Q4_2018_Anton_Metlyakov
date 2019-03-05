namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using DAL;
    using log4net;

    /// <summary>
    /// Класс содержит список со всеми тестами в системе
    /// </summary>
    public class QuizRepository : IQuizRepository // BaseRepository<QuizClass>
    {
        /// <summary>
        /// default Constructor
        /// </summary>
        public QuizRepository()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            this.ItemList = new List<Quiz>(0);
        }

        /// <summary>
        /// Количество записей в списке
        /// </summary>
        public int Count
        {
            get
            {
                return ItemList.Count;
            }
        }

        private IdbConnector Db { get; set; }

        private List<Quiz> ItemList { get; set; }

        public bool Delete(int id)
        {
            return false;
        }

        public Quiz Get(int id)
        {
            return new Quiz();
        }

        public List<Quiz> GetAll()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            return ItemList;
        }

        public bool Save(Quiz q)
        {
            return false;
        }
    }
}