namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using DAL;
    using Ninject;
    using Quiz.Models;

    /// <summary> 
    /// User controller 
    /// </summary>
    public class UserController : Controller
    {
        private readonly IPersonRepository repository;
        private readonly IRolesRepository roles;

        public UserController(IPersonRepository _repository, IRolesRepository _roles)
        {
            this.repository = _repository;
            this.roles = _roles;
        }

        /// <summary>
        /// строит список пользователей системы
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string name = "")
        {
            var modelledUsers = new List<UserModel>(0);
            IEnumerable<Person> persons;
            this.roles.GetAll();
            if (name == string.Empty)
                persons = this.repository.GetAll();
            else
            {
                // применю строку поиска
                persons = this.repository.GetAll().Where(x => (x.UserName.ToLower().Contains(name.ToLower())
                                                               || x.FirstName.ToLower().Contains(name.ToLower())
                                                               || x.LastName.ToLower().Contains(name.ToLower())));
            }

            foreach (var p in persons)
            {
                modelledUsers.Add(p);
            }
            
            return PartialView(modelledUsers);
        }

        /// <summary>
        /// просмотр информации о пользователе
        /// </summary>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (!id.HasValue) return HttpNotFound();
            var person = this.repository.Get(id.Value);
            UserModel editedUser = person;
            return PartialView(editedUser);
        }

        /// <summary>
        /// как я понял этот метод только показывет форму
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return PartialView();
        }

        /// <summary>
        /// Сохранить в БД нового пользователя, если все поля Ок
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel p, string action)
        {
            if (action == "cancel") //todo pn не надо так делать, намного проще просто закрыть модалку без какого-либо действия (да js, но ты вроде бы разбираешься в нём :)).
                return RedirectToRoute(new
                {
                    controller = "User",
                    action = "Index",
                    id = string.Empty
                });
            if (!ModelState.IsValid)
                return RedirectToRoute(new
                {
                    controller = "User",
                    action = "Index",
                    id = string.Empty
                });
            // поле "пароль" - проверить на соотв-е политикам
            if (!PasswordManager.ValidatePassword(p.Password))

            {       // пока не знаю как показать окно ошибки
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No product with ID = {0}", 1)),
                    ReasonPhrase = "Product ID Not Found"
                };
//                throw new HttpResponseException(resp);
//                throw new HttpException(403, "Пароль не удовлетворяет политике сложности");
            }
            var userSalt          = PasswordManager.GenerateSalt();
            var hashedPassword    = PasswordManager.HashPassword(
                                    p.Password == null ? string.Empty : p.Password,
                                    userSalt,
                                    WebConfigurationManager.AppSettings["quizGlobalSalt"]);
            var createdPerson = new Person(//todo pn почему не испльзуешь automapper?
                                    -1, 
                                    p.FirstName == null ? string.Empty : p.FirstName, 
                                    p.LastName == null ? string.Empty : p.LastName,
                                    p.UserName, 
                                    hashedPassword,
                                    userSalt,
                                    null,
                                    p.Roles,
                                    registrationDate: DateTime.Now, 
                                    lastLogonDate: null);

            if (!PersonValidator.IsValid(createdPerson, out string message))
            {
                // пока не знаю как сказать об ошибке
                return RedirectToRoute(new { controller = "Shared", action = "Error" });//todo pn не надо никуда редиректать. Нужно ModelState заполнить сообщением об ошибке и вернуть ТУ ЖЕ вьюху. На клиенте валидация подхватится и отобразится пользователю. https://stackoverflow.com/questions/37801718/adding-custom-error-message-for-model-property
            }

            if (!repository.Save(createdPerson))
            {
                return RedirectToRoute(new
                {
                    controller = "Shared",
                    action = "Error"
                });
            }

            return RedirectToRoute(new
            {
                controller = "User",
                action = "Index",
                id = string.Empty
            });
        }

        /// <summary>
        /// Under construction ...
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel p, string action)//todo pn тебе не кажется что у тебя Edit перекликается с Create? мб объединить в один?
        {
            if (action == "cancel")
            return RedirectToRoute(new
            {
                controller = "User",
                action = "Index",
                id = string.Empty
            });

            // 1. Если пользователь заполнил поле "пароль" - проверить на соотв-е политикам
            // если не заполнил - не менять. 
            var editedPerson = repository.Get(p.Id); 
            editedPerson.FirstName = p.FirstName == null ? string.Empty : p.FirstName;
            editedPerson.LastName = p.LastName == null ? string.Empty : p.LastName;
            editedPerson.UserName = p.UserName;

            if (!string.IsNullOrEmpty(p.Password))
            {
                if (!PasswordManager.ValidatePassword(p.Password))
                    return RedirectToRoute(new
                    {
                        controller = "Shared",
                        action = "Error"
                    });
                    editedPerson.HashedPassword = PasswordManager.HashPassword(
                                    p.Password,
                                    p.UserSalt,
                                    WebConfigurationManager.AppSettings["quizGlobalSalt"]);
            }

            editedPerson.Role = p.Roles;
            if (!PersonValidator.IsValid(editedPerson, out string message))
                return RedirectToRoute(new { controller = "Shared", action = "Error" });
            if (!repository.Save(editedPerson))
            {
                return RedirectToRoute(new
                {
                    controller = "Shared",
                    action = "Error"
                });
            }

            return RedirectToRoute(new
            {
                controller = "User",
                action = "Index",
                id = string.Empty
            });
        }

        public ActionResult Edit(int id)
        {
            var person = repository.Get(id);
            if (person == null)
                return RedirectToRoute(new
                {
                    controller = "User",
                    action = "Index",
                    id = string.Empty
                });

            UserModel editedUser = person;
            return PartialView(editedUser);
        }
                
        /// <summary>
        /// Удаляем пользователя по ИД
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string action)
        {
            if (!PersonValidator.IsDeleteOK(repository.Get(id), out string message))
            {
                return RedirectToRoute(new
                {
                    controller = "Shared",
                    action = "Error"
                });
            }

            if (action == "delete")
                repository.Delete(id);
            return RedirectToRoute(new
            {
                controller = "User",
                action = "Index",
                id = string.Empty
            });
        }

        /// <summary>
        /// Просим подтверждения на удаление
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            var person = repository.Get(id);
            
            if (person == null)
                return RedirectToRoute(new
                {
                    controller = "User",
                    action = "Index",
                    id = string.Empty
                });

            UserModel deletingUser = person;
            return PartialView(deletingUser);
        }

        /*
        /// <summary>
        /// Просто инфо о пользователе  - заменил на Details
        /// </summary>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            var person = repository.Get(id);
            UserModel viewedUser = person;
            return View(viewedUser);
        }
        */
    }
}