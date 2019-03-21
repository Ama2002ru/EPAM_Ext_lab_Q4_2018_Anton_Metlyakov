namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Класс для получения из БД статистики
    /// </summary>
    public class StatsByUser
    {
        public int Quiz_Id { get; set; }

        public string Quiz_Name { get; set; }

        public string Quiz_Status { get; set; }

        public string Completed_Rate { get; set; }

        public string Completed_Date { get; set; }

        public string Started_Date { get; set; }

        public string Time_Taken { get; set; }
    }
}
