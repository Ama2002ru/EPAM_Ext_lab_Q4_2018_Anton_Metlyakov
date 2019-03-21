namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Класс для получения из БД статистики - по квизу
    /// </summary>
    public class StatsByQuiz
    {
        public int User_Id { get; set; }

        public string User_Name { get; set; }

        public string Quiz_Status { get; set; }

        public string Completed_Rate { get; set; }

        public string Completed_Date { get; set; }

        public string Started_Date { get; set; }

        public string Time_Taken { get; set; }
    }
}
