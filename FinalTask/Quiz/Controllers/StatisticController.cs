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
    using static Quiz.Resources.QuizResources;

   /// <summary>
   /// Контроллер для расчета статистики
   /// </summary>
    [Authorize]
    [QuizExceptionHandler]
    public class StatisticController : Controller
    {
        /// <summary>
        /// Dependency injections fields
        /// </summary>
        private readonly IQuizRepository quizRepository;
        private readonly IPersonRepository personRepository;

        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="repo"></param>
        public StatisticController(IQuizRepository repo, IPersonRepository personRepo)
        {
            quizRepository = repo;
            personRepository = personRepo;
        }

        /// <summary>
        /// Общая статистика по квизам
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult AllQuizes()
        {
            var statsAllQuizes = Statistic.AllQuizes(quizRepository);
            return PartialView(statsAllQuizes);
        }

        /// <summary>
        /// Общая статистика по пользователям
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult AllUsers()
        {
            var statsAllUsers = Statistic.AllUsers(quizRepository);
            return PartialView(statsAllUsers);
        }

        /// <summary>
        /// подробная статистика по квизу
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult ByQuiz(int? quiz_id)
        {
            if (!quiz_id.HasValue)
            {
                ViewBag.Error = S_InvalidHTTP;
                return View("Error");
            }

            var statsByUser = Statistic.ByQuiz(quizRepository, quiz_id.Value);
            return PartialView(statsByUser);
        }
       
        /// <summary>
        /// подробная статистика по пользователю
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Student,Instructor")]
        public ActionResult ByUser(int? user_id)
        {
            if (!user_id.HasValue)
            {
                ViewBag.Error = S_InvalidHTTP;
                return View("Error");
            }

            var statsByUser = Statistic.ByUser(quizRepository, user_id.Value);
            return PartialView(statsByUser);
        }

        /// <summary>
        /// подробная статистика по пользователю -по квизу
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Student,Instructor")]
        public ActionResult ByUserQuiz(int? user_id, int? quiz_id)
        {
            if (!user_id.HasValue || !quiz_id.HasValue)
            {
                ViewBag.Error = S_InvalidHTTP;
                return View("Error");
            }

            List<StatsByUserQuiz> statsByUser = null;

            // Пользователю дам возможность просматривать только пройденный квиз
            // может просматривать 1. Инструктор.
            // 2. Пользователь, у которого этот квиз имеет статус, отличный от Assigned
            var test_var = ((QuizRepository)quizRepository).GetQuizAssignment(user_id.Value)
                        .Assignquizlist.First(x => x.Quiz_Id == quiz_id.Value).Quiz_Status;
            if (User.IsInRole("Instructor") ||
                (!User.IsInRole("Instructor") &&
                test_var != QuizStatusEnum.Assigned && test_var != QuizStatusEnum.None))
            {
                statsByUser = Statistic.ByUserQuiz(quizRepository, user_id.Value, quiz_id.Value);
                return PartialView(statsByUser);
            }

            ViewBag.Error = S_QuizNotFinished;
            return View("Error");
        }

        /// <summary>
        /// Главная страница статистики
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Student,Instructor")]
        public ActionResult Index()
        {
            var statistic = new StatisticModel();
            statistic.Users = new SelectList(personRepository.GetAll(), "ID", "UserName");
            statistic.Quizes = new SelectList(quizRepository.GetAll(), "ID", "Quiz_Name");
            return PartialView(statistic);
        }

        /// <summary>
        /// Главная страница - после нажатия кнопки
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Student,Instructor")]
        public ActionResult Index(StatisticModel stats)
        {
            // Инструктор может выбирать пользователя
            // студент не может. Для студента уберу возможность выбора пользователя на View,
            // но если что то пойдет не так ...
            var isInstructor = User.IsInRole("Instructor");
            int current_user_id = 0;
            if (stats.Selected_User_Id.HasValue && isInstructor)

            // Если пользователь выбран инструктором
                current_user_id = stats.Selected_User_Id.Value;
            if (!isInstructor)
            {
                // берем текущего залогиненного студента
                current_user_id = personRepository.GetAll().First(x => x.UserName == User.Identity.Name).ID;
                if (stats.Selected_Quiz_Id.HasValue)
                {
                    return RedirectToRoute(new
                    {
                        controller = "Statistic",
                        action = "ByUserQuiz",
                        quiz_id = stats.Selected_Quiz_Id.Value,
                        user_id = current_user_id
                    });
                }

                return RedirectToRoute(new
                {
                    controller = "Statistic",
                    action = "ByUser",
                    user_id = current_user_id
                });
            }

            // Ветка кода для Инструктора
            if (stats.Selected_Quiz_Id.HasValue && stats.Selected_User_Id.HasValue)
            {
                return RedirectToRoute(new
                {
                    controller = "Statistic",
                    action = "ByUserQuiz",
                    quiz_id = stats.Selected_Quiz_Id.Value,
                    user_id = stats.Selected_User_Id.Value
                });
            }

            if (stats.Selected_Quiz_Id.HasValue)
            {
                return RedirectToRoute(new
                {
                    controller = "Statistic",
                    action = "ByQuiz",
                    quiz_id = stats.Selected_Quiz_Id.Value
                });
            }

            if (stats.Selected_User_Id.HasValue)
            {
                return RedirectToRoute(new
                {
                    controller = "Statistic",
                    action = "ByUser",
                    user_id = current_user_id
                });
            }

            ViewBag.Error = S_InvalidHTTP;
            return View("Error");
        }
    }
}