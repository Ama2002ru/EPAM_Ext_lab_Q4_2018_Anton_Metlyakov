namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using DAL;
    using Quiz.Models;

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
            try
            {
                var qm = new QuizModel();
                qm.Quiz_Id = -1;
                qm.Author = User.Identity.Name;
                qm.Author_Id = personRepository.GetAll().First(x => x.UserName == qm.Author).ID;
                if (User.Identity.IsAuthenticated)
                {
                    var person = personRepository.GetAll().Find(x => x.UserName == User.Identity.Name);
                    qm.Author_Id = person.ID;
                    qm.Author = person.FirstName + ' ' + person.LastName;
                }

                return PartialView(qm);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error create quiz !";
                return PartialView();
            }
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
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "Quiz is Non Valid!";
                    return PartialView(quiz);
                }

                var createdQuiz = new Quiz();
                createdQuiz.ID = -1;
                createdQuiz.Quiz_Name = quiz.Name;
                createdQuiz.Success_Rate = quiz.Success_Rate;
                createdQuiz.Author = quiz.Author;
                createdQuiz.Author_Id = quiz.Author_Id;
                createdQuiz.Created_Date = DateTime.Now;

                if (!repository.Save(createdQuiz))
                {
                    ViewBag.Error = "Quiz save failed!";
                    return View();
                }

                return RedirectToRoute(new
                {
                    controller = "Quiz",
                    action = "Index"
                });
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error save quiz!";
                return PartialView();
            }
        }

        /// <summary>
        /// Просим подтверждения на удаление квиза
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    ViewBag.Error = "Invalid HTTP request !";
                    return PartialView();
                }

                Quiz quiz;
                if ((quiz = repository.Get(id.Value)) == null)
                {
                    ViewBag.Error = "Invalid quiz request !";
                    return PartialView();
                }

                return PartialView((QuizModel)quiz);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error delete quiz !";
                return PartialView();
            }
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
            try
            {
                if (!repository.Delete(qm.Quiz_Id))
                {
                    ViewBag.Error = "Delete question error!";
                    return View();
                }

                return RedirectToRoute(new
                {
                    controller = "Quiz",
                    action = "Index",
                    quiz_id = string.Empty
                });
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error delete quiz !";
                return PartialView();
            }
        }

        /// <summary>
        /// Просмотр данных по квизу
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Details(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    ViewBag.Error = "Invalid HTTP request !";
                    return PartialView();
                }

                Quiz quiz;
                if ((quiz = this.repository.Get(id.Value)) == null)
                {
                    ViewBag.Error = "Invalid quiz request!";
                    return PartialView();
                }

                return PartialView((QuizModel)quiz);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error view quiz !";
                return PartialView();
            }
        }

        /// <summary>
        /// выведем поля на редактирование
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Edit(int id)
        {
            try
            {
                Quiz quiz = repository.Get(id);
                if (quiz == null)
                {
                    ViewBag.Error = "Quiz not found!";
                    return PartialView("Error");
                }

                List<SelectListItem> authors = new List<SelectListItem>();
                authors.Add(new SelectListItem { Text = "Please select", Value = "0" });
                foreach (var user in personRepository.GetAll())
                    if (user.ID == quiz.Author_Id)
                        authors.Add(new SelectListItem { Text = user.FirstName + ' ' + user.LastName, Value = user.ID.ToString(), Selected = true });
                    else
                        authors.Add(new SelectListItem { Text = user.FirstName + ' ' + user.LastName, Value = user.ID.ToString() });

                ViewBag.Authors = authors;
                QuizModel q = quiz;
                return View(q);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error view quiz !";
                return PartialView();
            }
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
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "Quiz Non Valid";
                    return View(quiz);
                }

                var editedQuiz = repository.Get(quiz.Quiz_Id);
                editedQuiz.ID = quiz.Quiz_Id;
                editedQuiz.Quiz_Name = quiz.Name;
                editedQuiz.Success_Rate = quiz.Success_Rate;

                if (!repository.Save(editedQuiz))
                {
                    ViewBag.Error = "Quiz save failed!";
                    return View(quiz);
                }

                return RedirectToRoute(new
                {
                    controller = "Quiz",
                    action = "Index",
                    id = string.Empty
                });
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error save quiz !";
                return PartialView();
            }
        }

        /// <summary>
        /// Метод индекс с поиском квиза
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Index(string name = "")
        {
            try
            {
                // FormsAuthentication.SetAuthCookie("Admin", true);
                // FormsAuthentication.RedirectFromLoginPage("Admin", true);
                // var s = User.Identity.IsAuthenticated;
                // var me = User.Identity.Name;
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
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error get quiz list !";
                return PartialView();
            }
        }
    }
}