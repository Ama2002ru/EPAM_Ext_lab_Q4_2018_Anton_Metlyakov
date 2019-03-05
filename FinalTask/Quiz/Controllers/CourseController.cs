namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;
    using Quiz.Models;

    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            var courses = new List<CourseModel>(0);
            courses.Add(new CourseModel() { Course_Id = 1, Course_Name = "C#"});
            return View(courses);
        }
    }
}