namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Security;
    using DAL;
    using Ninject;
    using Quiz.Models;

    /// <summary>
    /// логика проверки пароля пользователя
    /// </summary>
    [AllowAnonymous]
    public class LogonController : Controller
    {
        private readonly IAuthProvider myAuthProvider;

        public LogonController()
        {
        }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="_repository"></param>
        public LogonController(IAuthProvider auth)
        {
            myAuthProvider = auth;
        }

        /// <summary>
        /// отображение формы ввода пароля
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Logon()
        {
            return View();
        }

        /// <summary>
        /// проверка пароля
        /// </summary>
        /// <param name="userAndPassword"></param>
        /// <param name="return_Url"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logon(LogonModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (myAuthProvider.Authenticate(model.UserName, model.Password))
                {
                    string controller = string.Empty;
                    if (User.IsInRole("Admin")) controller = "User";
                    if (User.IsInRole("Student")) controller = "Myquizes";
                    if (User.IsInRole("Instructor")) controller = "Quiz";
                    return Redirect(returnUrl ?? Url.Action("Index", controller));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorrect username or password");
                    return View();
                }
            }
            else
                return View();
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/logon");
        }
    }
}
