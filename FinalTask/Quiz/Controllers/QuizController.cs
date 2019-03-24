namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Security;
    using DAL;
    using Quiz.Models;
    using static Quiz.Resources.QuizResources;

    /// <summary>
    ///  контроллер редактирования квизов
    /// </summary>
    [QuizExceptionHandler]
    [Authorize(Roles = "Instructor")]
    public class QuizController : Controller
    {
        /// <summary>
        /// Dependency injections fields
        /// </summary>
        private readonly IQuizRepository repository;
        private readonly IPersonRepository personRepository;

        /// <summary>
        /// Конструктор без параметров - для сериализатора
        /// </summary>
        /// <param name="_repository"></param>
        public QuizController()
        {
        }

        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="repo"></param>
        public QuizController(IQuizRepository repo, IPersonRepository personRepo)
        {
            repository = repo;
            personRepository = personRepo;
        }

        /// <summary>
        /// Создание Квиза
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Create()
        {
            var qm = new QuizModel
            {
                Quiz_Id = -1,
                Author = User.Identity.Name
            };
            qm.Author_Id = personRepository.GetAll().First(x => x.UserName == qm.Author).ID;
            if (User.Identity.IsAuthenticated)
            {
                var person = personRepository.GetAll().Find(x => x.UserName == User.Identity.Name);
                qm.Author_Id = person.ID;
                qm.Author = person.FirstName + ' ' + person.LastName;
            }

            return PartialView(qm);
        }

        /// <summary>
        /// Сохраним квиз в БД
        /// </summary>
        /// <param name="quiz"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public ActionResult Create(QuizModel quiz)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = S_InvalidQuiz;
                return View(quiz);
            }

            var createdQuiz = new Quiz
            {
                ID = -1,
                Quiz_Name = quiz.Name,
                Success_Rate = quiz.Success_Rate,
                Author = quiz.Author,
                Author_Id = quiz.Author_Id,
                Created_Date = DateTime.Now
            };

            if (!repository.Save(createdQuiz))
            {
                ViewBag.Error = S_ErrorSaveQuiz;
                return View("Error");
            }

            return RedirectToRoute(new
            {
                controller = "Quiz",
                action = "Index"
            });
        }

        /// <summary>
        /// Просим подтверждения на удаление квиза
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                ViewBag.Error = S_InvalidHTTP;
                return View("Error");
            }

            Quiz quiz;
            if ((quiz = repository.Get(id.Value)) == null)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return PartialView();
            }

            return PartialView((QuizModel)quiz);
        }

        /// <summary>
        /// Удаляем квиз
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public ActionResult Delete(QuizModel qm)
        {
            if (!repository.Delete(qm.Quiz_Id))
            {
                ViewBag.Error = S_ErrorDeleteQuestion;
                return View("Error");
            }

            return RedirectToRoute(new
            {
                controller = "Quiz",
                action = "Index",
                quiz_id = string.Empty
            });
        }

        /// <summary>
        /// Просмотр данных по квизу
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                ViewBag.Error = S_InvalidHTTP;
                return PartialView("Error");
            }

            Quiz quiz;
            if ((quiz = this.repository.Get(id.Value)) == null)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            return PartialView((QuizModel)quiz);
        }

        /// <summary>
        /// выведем поля на редактирование
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Edit(int id)
        {
            Quiz quiz = repository.Get(id);
            if (quiz == null)
            {
                ViewBag.Error = S_QuizNotFound;
                return View("Error");
            }

            List<SelectListItem> authors = new List<SelectListItem>
            {
                new SelectListItem { Text = "Please select", Value = "0" }
            };
            foreach (var user in personRepository.GetAll())
                if (user.ID == quiz.Author_Id)
                    authors.Add(new SelectListItem { Text = user.FirstName + ' ' + user.LastName, Value = user.ID.ToString(), Selected = true });
                else
                    authors.Add(new SelectListItem { Text = user.FirstName + ' ' + user.LastName, Value = user.ID.ToString() });

            ViewBag.Authors = authors;
            QuizModel q = quiz;
            return View(q);
        }

        /// <summary>
        /// сохраним квиз в БД
        /// </summary>
        /// <param name="quiz"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public ActionResult Edit(QuizModel quiz)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = S_InvalidQuiz;
                return View(quiz);
            }

            var editedQuiz = repository.Get(quiz.Quiz_Id);
            editedQuiz.ID = quiz.Quiz_Id;
            editedQuiz.Quiz_Name = quiz.Name;
            editedQuiz.Success_Rate = quiz.Success_Rate;

            if (!repository.Save(editedQuiz))
            {
                ViewBag.Error = S_ErrorSaveQuiz;
                return View(quiz);
            }

            return RedirectToRoute(new
            {
                controller = "Quiz",
                action = "Index",
                id = string.Empty
            });
        }

        /// <summary>
        /// Метод индекс с поиском квиза
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Index(string name = "")
        {
            var searchedQuizes = new List<QuizModel>(0);
            IEnumerable<Quiz> quizes;
            if (name == string.Empty)
                quizes = this.repository.GetAll();
            else

                // применю строку поиска
                quizes = this.repository.GetAll().Where(x => x.Quiz_Name.ToLower().Contains(name.ToLower()));
            foreach (var q in quizes)
                searchedQuizes.Add(q);
            return PartialView(searchedQuizes);
        }
    }
}