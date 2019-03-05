namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;
    using Quiz.Models;

    public class QuizController : Controller
    {
        // GET: Quiz
        public ActionResult Index()
        {
            var quizes = new List<QuizModel>(0);
            quizes.Add(new QuizModel());
            return View(quizes);
        }
    }
}