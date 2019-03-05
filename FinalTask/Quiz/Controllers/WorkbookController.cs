namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;
    using Quiz.Models;

    public class WorkbookController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            var workbook = new WorkbookModel();
            workbook.Student = 1;
            workbook.Quizes = new List<Quiz>(0);
            return View(workbook);
        }
    }
}
