﻿namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using DAL;
    using Ninject;
    using Quiz.Models;
    using static Quiz.Resources.QuizResources;

    /// <summary> 
    /// Контроллер для работы с пользователями
    /// </summary>
    [Authorize(Roles = "Admin")]
    [QuizExceptionHandler]
    public class UserController : Controller
    {
        /// <summary>
        /// Dependency injections fields
        /// </summary>
        private readonly IPersonRepository repository;
        private readonly IRolesRepository roles;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_repository"></param>
        /// <param name="_roles"></param>
        public UserController(IPersonRepository repository, IRolesRepository roles)
        {
            this.repository = repository;
            this.roles = roles;
        }

        /// <summary>
        /// Метод строит список пользователей системы
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string name)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            var allUsers = new List<UserModel>(0);
            IEnumerable<Person> persons;
            this.roles.GetAll();
            if (string.IsNullOrEmpty(name))
                persons = this.repository.GetAll();
            else

                // применю строку поиска
                persons = this.repository.GetAll().Where(x => (x.UserName.ToLower().Contains(name.ToLower())
                                                                || x.FirstName.ToLower().Contains(name.ToLower())
                                                                || x.LastName.ToLower().Contains(name.ToLower())));
            foreach (var p in persons)
                allUsers.Add(p);
                return View(allUsers);
    }

        /// <summary>
        /// просмотр информации о пользователе
        /// </summary>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            Person person;
            if (!id.HasValue || (person = this.repository.Get(id.Value)) == null)
            {
                ViewBag.Error = S_InvalidUserRequest;
                return View();
            }

            UserModel user = person;
            return PartialView(user);
        }

        /// <summary>
        /// как я понял этот метод только показывет форму
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            return PartialView();
        }

        /// <summary>
        /// Сохранить в БД нового пользователя, если все поля Ок
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel p)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            if (!ModelState.IsValid)
            {
                ViewBag.Error = S_InvalidUserInfo;
                return View("Error");
            }

            if (p == null)
            {
                ViewBag.Error = S_InvalidHTTP;
                return View("Error");
            }

            if (!PasswordManager.ValidatePassword(p.Password, out string message))
            {
                ViewBag.Error = S_PasswordValidationError + message;
                return View("Error");
            }

            var userSalt = PasswordManager.GenerateSalt();
            var hashedPassword = PasswordManager.HashPassword(
                                    p.Password,
                                    userSalt,
                                    WebConfigurationManager.AppSettings["quizGlobalSalt"]);
            var createdPerson = new Person(
                                    -1,
                                    p.FirstName ?? string.Empty,
                                    p.LastName ?? string.Empty,
                                    p.UserName,
                                    hashedPassword,
                                    userSalt,
                                    null,
                                    p.Roles,
                                    registrationDate: DateTime.Now,
                                    lastLogonDate: null);
            var personValidator = new PersonValidator(repository);
            if (!personValidator.IsValid(createdPerson, out message))
            {
                ViewBag.Error = message;
                return View("Error");
            }

            if (!repository.Save(createdPerson))
            {
                ViewBag.Error = S_ErrorSaveUser;
                return View("Error");
            }

            return RedirectToRoute(new
            {
                controller = "User",
                action = "Index"
            });
       }

        /// <summary>
        /// взять пользователя на редактирование
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                ViewBag.Error = S_InvalidHTTP;
                return View("Error");
            }

            var person = repository.Get(id.Value);
            if (person == null)
            {
                ViewBag.Error = S_UserNotFound;
                return View("Error");
            }

            UserModel editedUser = person;
            return PartialView(editedUser);
        }

        /// <summary>
        /// Сохраним отредактированного пользователя
        /// экшн очень похож на сохранение нового пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel p)
        {
            string message;
            if (!ModelState.IsValid)
            {
                ViewBag.Error = S_InvalidUserInfo;
                return View("Error");
            }

            // 1. Если пользователь заполнил поле "пароль" - проверить на соотв-е политикам
            // если не заполнил - не менять. 
            var editedPerson = repository.Get(p.Id);
            editedPerson.FirstName = p.FirstName ?? string.Empty;
            editedPerson.LastName = p.LastName ?? string.Empty;
            editedPerson.UserName = p.UserName;

            if (!string.IsNullOrEmpty(p.Password))
            {
                if (!PasswordManager.ValidatePassword(p.Password, out message))
                {
                    ViewBag.Error = S_PasswordValidationError + message;
                    return View("Error");
                }

                editedPerson.HashedPassword = PasswordManager.HashPassword(
                                    p.Password,
                                    p.UserSalt,
                                    WebConfigurationManager.AppSettings["quizGlobalSalt"]);
            }

            editedPerson.Role = p.Roles;
            var personValidator = new PersonValidator(repository);
            if (!personValidator.IsValid(editedPerson, out message))
            {
                ViewBag.Error = S_InvalidUserInfoError + message;
                return View("Error");
            }

            // согласно контракту, Save должен возвращать bool
            // а так бы мог передать информативное сообщение об ошибке
            if (!repository.Save(editedPerson))
            {
                ViewBag.Error = S_ErrorSaveUser;
                return View("Error");
            }

            return RedirectToRoute(new
            {
                controller = "User",
                action = "Index",
                id = string.Empty
            });
        }
                
        /// <summary>
        /// Удаляем пользователя по ИД
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserModel user)
        {
            var personValidator = new PersonValidator(repository);
            if (!personValidator.IsDeleteOK(repository.Get(user.Id), out string message))
            {
                ViewBag.Error = S_CantDeleteUser + message;
                return View("Error");
            }

            if (!repository.Delete(user.Id))
            {
                ViewBag.Error = S_ErrorDeleteUser;
                return View("Error");
            }

            return RedirectToRoute(new
            {
                controller = "User",
                action = "Index"
            });
         }
        
        /// <summary>
        /// Просим подтверждения на удаление
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                ViewBag.Error = S_InvalidHTTP;
                return View("Error");
            }

            var person = repository.Get(id.Value);
            if (person == null)
                return RedirectToRoute(new
                {
                    controller = "User",
                    action = "Index"
                });
            UserModel deletingUser = person;
            return PartialView(deletingUser);
        }
    }
}