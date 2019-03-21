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
    using static Quiz.Resources.QuizResources;

    /// <summary>
    /// логика проверки пароля пользователя
    /// </summary>
    [AllowAnonymous]
    public class LogonController : Controller
    {
        private readonly IAuthProvider myAuthProvider;
        private readonly IPersonRepository personRepository;

        public LogonController()
        {
        }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="_repository"></param>
        public LogonController(IAuthProvider auth, IPersonRepository personRepo)
        {
            myAuthProvider = auth;
            personRepository = personRepo;
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
        /// <returns></returns>
        /// 
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logon(LogonModel model)
        {
            if (ModelState.IsValid)
            {
                if (myAuthProvider.Authenticate(model.UserName, model.Password))
                {
                    SetLogonDate.Set_Logon_Date(personRepository, model.UserName);
                    return Redirect(Url.Action("WelcomeView", "Logon"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, S_IncorrectUsername);
                    return View();
                }
            }
            else
                return View();
        }

        /// <summary>
        /// Регистрация нового пользователя. 
        /// Откроем форму
        /// </summary>
        [AllowAnonymous]
        public ActionResult Register()
        {
            var registrationmodel = new UserModel();
            registrationmodel.UserName = "test";
            return View(registrationmodel);
        }

        /// <summary>
        /// Регистрация нового пользователя. 
        /// Роль зашью жестко - студент
        /// </summary>
        /// <param name="userAndPassword"></param>
        /// <param name="return_Url"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserModel registrationmodel)
        {
            if (registrationmodel == null)
            {
                ViewBag.Error = S_InvalidHTTP;
                return View();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Error = S_InvalidUserInfo;
                return View();
            }

            if (!(personRepository.GetAll().Find(x => x.UserName.ToUpper() == registrationmodel.UserName.ToUpper()) == null))
            {
                ModelState.AddModelError(string.Empty, S_UsernameExists);
                return View();
            }

            // попробую переиспользовать код - создам контроллер
            registrationmodel.Roles = RoleEnum.Student;
            registrationmodel.Id = -1;  // new user
            var userController = new UserController(personRepository, null);
            userController.Create(registrationmodel);
            if (personRepository.GetAll().Find(x => x.UserName.ToUpper() == registrationmodel.UserName.ToUpper()) == null)
            {
                ViewBag.Error = S_ErrorSaveUser;
                return View();
            }

            // залогиниться
            Logon(new LogonModel()
                {
                    UserName = registrationmodel.UserName,
                    Password = registrationmodel.Password
                });
            return Redirect(Url.Action("WelcomeView", "Logon"));
        }

        /// <summary>
        /// логофф пользоватля
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Student,Instructor")]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/logon");
        }

        /// <summary>
        /// отображение формы приветсвия залогинившегося пользователя
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Student,Instructor")]
        public ActionResult WelcomeView()
        {
            return View();
        }
    }
}
