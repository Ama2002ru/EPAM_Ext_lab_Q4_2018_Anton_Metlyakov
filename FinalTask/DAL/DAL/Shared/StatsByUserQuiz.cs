namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Класс для получения из БД статистики - подробная по квизу
    /// </summary>
    public class StatsByUserQuiz
    {
        public string Info { get; set; }

        public string Text { get; set; }

        public string Result { get; set; }
    }
}
