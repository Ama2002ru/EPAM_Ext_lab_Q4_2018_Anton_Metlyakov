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
    using Quiz.Models;

    public class LogonController : Controller
    {
        private readonly IPersonRepository repository;
        
        public LogonController(IPersonRepository _repository)
        {
            this.repository = _repository;
        }

        // GET: Logon
/*        public ActionResult Index()
        {
            return View();
        }
 */  
        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogonModel userAndPassword, string return_Url)
        {
            var returnUrl = return_Url;  // ????
            // 1 Get Person by Username
            // 2 hash password
            // 3 hashes == ? - redirect
            var user = this.repository.GetAll().Where(x => x.UserName == userAndPassword.User);
            if (user.Count() == 0) return View(userAndPassword);

            var hashedPassword = PasswordManager.HashPassword(
                        userAndPassword.Password,
                        user.First().Salt,
                        WebConfigurationManager.AppSettings["quizGlobalSalt"]);
            if (hashedPassword == user.First().HashedPassword)
            {
                FormsAuthentication.SetAuthCookie(userAndPassword.User, false);
                returnUrl = "~/person/index";
                if (user.First().IsAssignedRole(RoleEnum.Admin)) returnUrl = "~/user/index";
                if (user.First().IsAssignedRole(RoleEnum.Instructor)) returnUrl = "~/quiz/index";
            }
            else
                returnUrl = "~/logon";
            return Redirect(returnUrl);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return Redirect("~/");
        }
    }
}
