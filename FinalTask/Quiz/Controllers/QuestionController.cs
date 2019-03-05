namespace Quiz.Controllers
{ 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;
    using Quiz.Models;

    public class QuestionController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            var questions = new List<QuestionModel>(0);
            questions.Add(new QuestionModel());
            return View(questions);
        }
    }
}