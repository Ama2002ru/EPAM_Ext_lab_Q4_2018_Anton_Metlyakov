namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using DAL;

    public class QuestionModel
    {
        public int Question_Id { get; set; }

        public int Quiz_Id { get; set; }

        public string Info { get; set; }

        public string Text { get; set; }

        public int CorrectOptionFlag { get; set; }

    }

}