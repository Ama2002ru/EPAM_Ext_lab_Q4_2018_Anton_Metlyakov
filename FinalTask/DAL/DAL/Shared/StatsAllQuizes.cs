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
    public class StatsAllQuizes
    {
        public string Quiz_Name { get; set; }

        public float Success_Rate { get; set; }

        public int Total_Assigned { get; set; }

        public int Total_Passed { get; set; }

        public int Total_Failed { get; set; }

        public string Average_Rate { get; set; }

        public float Percent_Passed { get; set; }
    }
}
