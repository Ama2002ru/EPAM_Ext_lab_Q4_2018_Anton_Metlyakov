namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using DAL;

    public class QuizModel
    {
        public int Quiz_Id { get; set; }

        public int Course_Id { get; set; }

        public string Name { get; set; }

        public int Author_Id { get; set; }

        public DateTime Creation_Date { get; set; }

        public float Success_Rate { get; set; }

        public List<Question> Questions { get; set; }
    }
}