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
                ViewBag.Error = S_ErrorCreateQuiz;
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
                    ViewBag.Message = S_InvalidQuiz;
                    return View(quiz);
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
                    ViewBag.Error = S_ErrorSaveQuiz;
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
                ViewBag.Error = S_ErrorSaveQuiz;
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
                    ViewBag.Error = S_InvalidHTTP;
                    return PartialView();
                }

                Quiz quiz;
                if ((quiz = repository.Get(id.Value)) == null)
                {
                    ViewBag.Error = S_InvalidQuizRequest;
                    return PartialView();
                }

                return PartialView((QuizModel)quiz);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = S_ErrorDeleteQuiz;
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
                    ViewBag.Error = S_ErrorDeleteQuestion;
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
                ViewBag.Error = S_ErrorDeleteQuiz;
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
                    ViewBag.Error = S_InvalidHTTP;
                    return PartialView();
                }

                Quiz quiz;
                if ((quiz = this.repository.Get(id.Value)) == null)
                {
                    ViewBag.Error = S_InvalidQuizRequest;
                    return PartialView();
                }

                return PartialView((QuizModel)quiz);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = S_ErrorViewQuiz;
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
                    ViewBag.Error = S_QuizNotFound;
                    return PartialView();
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
                ViewBag.Error = S_ErrorViewQuiz;
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
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = S_ErrorSaveQuiz;
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
                ViewBag.Error = S_ErrorGetQuizList;
                return PartialView();
            }
        }

        /// <summary>
        /// Лечение проблемы с JQuery Validate и точкой/запятой во Float поле
        /// так как разделитель-точка захардкожена в JQuery, переведу и свой UI в en-US вид
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var appsetting = WebConfigurationManager.AppSettings["quizlocale"] ?? "en-US";
            var appuisetting = WebConfigurationManager.AppSettings["quizuilocale"] ?? "en";
            Thread.CurrentThread.CurrentCulture = new CultureInfo(appsetting, false);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(appuisetting, false);
        }
    }
}