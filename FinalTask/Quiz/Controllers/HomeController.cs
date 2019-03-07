namespace Quiz.Controllers
{
    using System;
    using System.Web.Mvc;

    public class HomeController : Controller //todo pn лишние контроллеры и экшены (и вьюхи) лучше удалить
    {
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return this.View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return this.View();
        }

        public ActionResult PersonList()
        {
            return this.View();
        }
    }
}