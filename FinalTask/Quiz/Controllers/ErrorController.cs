namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
     
    public class ErrorController : Controller
    {
        public ActionResult NotFound(string errortext)
        {
            Response.StatusCode = 404;
            Response.StatusDescription = errortext;
            return View(model: Response);
        }

        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return View();
        }

        public ActionResult InternalError()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}