namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using DAL;

    public class WorkbookModel
    {
        public int Student { get; set; }

        public List<Quiz> Quizes { get; set; }

    }
}