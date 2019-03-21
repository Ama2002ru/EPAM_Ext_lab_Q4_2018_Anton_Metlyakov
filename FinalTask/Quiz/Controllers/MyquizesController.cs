namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;
    using Quiz.Models;
    using static Quiz.Resources.QuizResources;

    /// <summary>
    ///  Класс-контроллер, содержащий методы для работыс назначенными и назначаемыми квизами пользователя
    /// </summary>
    [Authorize(Roles = "Student,Instructor")]
    public class MyquizesController : Controller
    {
        private readonly IQuizRepository quizRepository;

        private readonly IPersonRepository personRepository;

        public MyquizesController()
        {
        }

        /// <summary>
        /// конструктор с инверсиями зависимостей
        /// </summary>
        /// <param name="_quiz"></param>
        /// <param name="_person"></param>
        public MyquizesController(IQuizRepository quiz, IPersonRepository person)
        {
            quizRepository = quiz;
            personRepository = person;
        }

        /// <summary>
        ///  Вывод "подробной" информации о квизе
        /// </summary>
        /// <param name="quizresult_id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Student,Instructor")]
        public ActionResult Details(int quizresult_id)
        {
            var q = new QuizResult(quizRepository); // объект-пустышка
            var myQuizResultToShow = q.GetQuizResult(quizresult_id);
            return PartialView((QuizResultModel)myQuizResultToShow);
        }

        /// <summary>
        /// Показ списка квизов пользователя
        /// 1. Получаем ID пользователя 
        /// 2. Получаю список всех квизов, которые уже назначены пользовтелю 
        /// (где статус <> none)
        /// 3. Показываю их
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Student,Instructor")]
        public ActionResult Index(int? user_id)
        {
            try
            {
                var listOfQuizResult = new MyQuizesIndexModel();
                List<QuizResult> myQuizes = null;
                int internal_user_id = 0;
                if (User.IsInRole("Student") && !User.IsInRole("Instructor"))
                {
                    // Заполним User_id - возьму текущего пользователя
                    string username = User.Identity.Name;
                    if (user_id.HasValue)
                        internal_user_id = user_id.Value;
                    else
                        internal_user_id = personRepository.GetAll().First(x => x.UserName == username).ID;
                    listOfQuizResult.User_id = internal_user_id;

                    // запрошу список назначенных квизов для пользователя
                    var dummyQuizResult = new QuizResult(quizRepository);
                    myQuizes = dummyQuizResult.Get(internal_user_id) ?? new List<QuizResult>(0);
                    var new_list = new List<QuizResultModel>(myQuizes.Count);

                    // преобразование типов
                    foreach (var t in myQuizes)
                        new_list.Add(t);
                    listOfQuizResult.List = new_list;
                }

                if (User.IsInRole("Instructor"))
                {
                    // Заполним список пользователей
                    listOfQuizResult.Users = new SelectList(personRepository.GetAll(), "ID", "UserName");

                    // список квизов  обнулю
                    listOfQuizResult.List = null;
                }

                return View(listOfQuizResult);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = S_ErrorLoadMyQuizes;
                return View();
            }
        }

       /// <summary>
       ///  назначение квизов пользователям 
       /// 1. Инструктор выбирает пользователя - я получаю его ID в модели
       /// 1.1. Пользователь может назначить квиз только себе
       /// 2. Получаю список всех квизов, которые уже назначены пользовтелю (или генерю пустой List)
       /// (где статус <> none)
       /// 3. Показываю их
       /// </summary>
       /// <param name="user_id"></param>
       /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public ActionResult Index(MyQuizesIndexModel myQuizesIndexModel)
        {
            try
            {
                // заполню таблицу назначенных квизов
                // User_id возьму из модели
                List<QuizResult> myQuizes = null;
                var dummyQuizResult = new QuizResult(quizRepository);
                myQuizes = dummyQuizResult.Get(myQuizesIndexModel.User_id) ?? new List<QuizResult>(0);
                var listOfQuizResultModel = new MyQuizesIndexModel();
                var new_list = new List<QuizResultModel>(myQuizes.Count);
                foreach (var t in myQuizes)
                    new_list.Add(t);
                myQuizesIndexModel.List = new_list;
                myQuizesIndexModel.Users = new SelectList(personRepository.GetAll(), "ID", "UserName");
                return View(myQuizesIndexModel);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error load MyQuizes data !";
                return View();
            }
        }

        /// <summary>
        /// Запуск прохождения теста из списка назначенных квизов
        /// - HTTPGET !
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Student,Instructor")]
        public ActionResult StartQuiz(int quizresult_id)
        {
            try
            {
                return View(GetNextQuestion(quizresult_id));
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = S_ErrorGetNextQuiestion;
                return View();
            }
        }

        /// <summary>
        /// получить следующий вопрос из БД и отобразить его
        /// Так как контроллер stateless, логика прохождения квиза такая
        /// 1. Из списка квизов вызываем Start (через HttpGET)
        ///    - устанавливаем статус квиза в inprogrees 
        ///    - отдаем 1й вопрос
        /// 2. через HttpPost отдаем ответ на предыдущий вопрос
        ///    и ищем следующий вопрос. Нашли - отдаем во View.
        ///    не нашли - заканчиваем квиз.
        /// </summary>
        /// <param name="quiz_id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Student,Instructor")]
        public ActionResult GetNextQuestion(FormCollection collection)
        {
            int quizresult_id = 0;
            QuestionModel currentQuestion = null;
            try
            {
                if (!SaveAnswer(collection))
                {
                    ViewBag.Error = S_ErrorSaveAnswer;
                    return View();
                }

                // буду таскать эту переменную между форм ? (QuizResult_Id)
                int.TryParse(collection["QuizResult_Id"], out quizresult_id);
                currentQuestion = GetNextQuestion(quizresult_id);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = S_ErrorGetNextQuiestion;
                return View();
            }

            if (currentQuestion != null)
                return View(currentQuestion);

            // вопросы кончились, завершаем квиз
            // посчитать рейты, и поставить статусы прохождения квиза 
            var q = new QuizResult(quizRepository); // объект-пустышка, т.к. все расчеты и сохранения веду в БД
            if (q.Save(quizresult_id))
                return RedirectToRoute(new
                {
                    controller = "Myquizes",
                    action = "FinishQuiz",
                    quizresult_id
                });
            ViewBag.Error = S_ErrorGetNextQuiestion;
            return View();
        }

        /// <summary>
        /// Получить очередной вопрос из БД по квизу
        /// </summary>
        /// <param name="quizresult_id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Student,Instructor")]
        public QuestionModel GetNextQuestion(int quizresult_id)
        {
            List<Question> nextQuestionList = null;
             nextQuestionList = ((QuizRepository)quizRepository).GetNextQuestion(quizresult_id);
            if (nextQuestionList == null || nextQuestionList.Count <= 0) return null;  // значит вопросы кончились, завершаем квиз
            QuestionModel nextQuestionModel = nextQuestionList.First();

            // добавлю доп поля на форму, которых нет в стандартном вопросе
            nextQuestionModel.QuizResult_Id = quizresult_id;
            return nextQuestionModel;
        }

        /// <summary>
        /// сохранить ответ в БД
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [Authorize(Roles = "Student,Instructor")]
        public bool SaveAnswer(FormCollection collection)
        {
            var currentAnswer = new Answer(quizRepository);
            int result = 0;
            int.TryParse(collection["QuizResult_Id"], out result);
            currentAnswer.QuizResult_Id = result;
            int.TryParse(collection["Question_Id"], out result);
            currentAnswer.Question_Id = result;
            currentAnswer.TimeStamp = null;
            currentAnswer.Answer_Id = -1;  // новая запись в БД
                                           // соберу чекнутые чекбоксы с формы
            currentAnswer.Answer_Flag = 0;
            for (int i = 0; i < 32; i++)
            {
                var res = collection["variant" + i.ToString()];
                if (res == "on")
                    currentAnswer.Answer_Flag |= 1 << i;
            }

            return currentAnswer.Save();
        }

        /// <summary>
        /// завершение квиза - расчет рейта и показ инфо
        /// </summary>
        /// <param name="quizresult_id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Student,Instructor")]
        public ActionResult FinishQuiz(int quizresult_id)
        {
            // показать сообщение о прохождении 
            var q = new QuizResult(quizRepository); // объект-пустышка, т.к. все расчеты и сохранения веду в БД
            var myQuizResultToShow = q.GetQuizResult(quizresult_id);

            return View((QuizResultModel)myQuizResultToShow);
        }

        /// <summary>
        /// Показать привязанные квизы для пользователя
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Student,Instructor")]
        public ActionResult AssignQuiz(int user_id)
        {
            var quizes = ((QuizRepository)quizRepository).GetQuizAssignment(user_id);
            return PartialView((AssignQuizModel)quizes);
        }

        /// <summary>
        /// Сохранить привязку квизов для пользователя
        /// На самом деле в параметрах будет FormCollection
        /// </summary>
        /// <param name="assignedquizes"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Student,Instructor")]
        public ActionResult AssignQuiz(FormCollection collection)
        {
            QuizStatusEnum quiz_status;
            int quiz_id;
            int user_id;
            int.TryParse(collection["User_Id"], out user_id);
            int max_quiz_id;
            int.TryParse(collection["MaxQuiz_ID"], out max_quiz_id);
            var assignedquizes = new AssignQuiz();
            assignedquizes.User_Id = user_id;
            assignedquizes.AssignedUser_Id = personRepository.GetAll().First(x => x.UserName == User.Identity.Name).ID;

            // запишем в список инфу про все квизы
            assignedquizes.Assignquizlist = new List<AssignQuizList>(max_quiz_id);
            for (int i = 0; i <= max_quiz_id; i++)
            {
                quiz_id = i;
                var res = collection["assigned_quiz_" + i.ToString()];
                if (res == "on")
                    quiz_status = QuizStatusEnum.Assigned; // точнее статусы разберу в хранимой процедуре
                else
                    quiz_status = QuizStatusEnum.None;
                int.TryParse(collection["QuizResult_Id_" + i.ToString()], out int quizresult_id);

                assignedquizes.Assignquizlist.Add(new AssignQuizList() { Quiz_Id = quiz_id, Quiz_Status = quiz_status, QuizResult_Id = quizresult_id });
            }

            if (!((QuizRepository)quizRepository).SaveQuizAssignment(assignedquizes))
            {
                ViewBag.Error = S_ErrorGetNextQuiestion;
                return View();
            }

            return RedirectToRoute(new
            {
                controller = "Myquizes",
                action = "Index",
                user_id = user_id
            });
        }
    }
}